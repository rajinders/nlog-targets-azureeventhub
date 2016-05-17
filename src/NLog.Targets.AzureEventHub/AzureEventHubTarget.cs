using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using Microsoft.ServiceBus.Messaging;

namespace NLog.Targets
{
    [Target("AzureEventHub")]
    public class AzureEventHubTarget : TargetWithLayout
    {
        EventHubClient _eventHubClient = null;
        MessagingFactory _messsagingFactory = null;

        [RequiredParameter]
        public string EventHubConnectionString { get; set; }

        [RequiredParameter]
        public string EventHubPath { get; set; }

        /// <summary>
        /// PartitionKey is optional. If no partition key is supplied the log messages are sent to eventhub
        /// and distributed to various partitions in a round robin manner.
        /// </summary>
        public string PartitionKey { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventHubClient.Close();
                _messsagingFactory.Close();
            }
        }

        /// <summary>
        /// Takes the contents of the LogEvent and sends the message to EventHub
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent)
        {
            var sendTask = SendAsync(PartitionKey, logEvent);

            try
            {
                sendTask.Wait();
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        private async Task<bool> SendAsync(string partitionKey, LogEventInfo logEvent)
        {
            if (this._messsagingFactory == null)
            {
                this._messsagingFactory = MessagingFactory.CreateFromConnectionString(EventHubConnectionString);
            }

            if (this._eventHubClient == null)
            {
                this._eventHubClient = this._messsagingFactory.CreateEventHubClient(EventHubPath);
            }

            string logMessage = this.Layout.Render(logEvent);

            using (var eventHubData = new EventData(Encoding.UTF8.GetBytes(logMessage)) { PartitionKey = partitionKey })
            {
                foreach (var key in logEvent.Properties.Keys)
                {
                    eventHubData.Properties.Add(key.ToString(), logEvent.Properties[key]);
                }

                await _eventHubClient.SendAsync(eventHubData);
                return true;
            } //end of using
        } //end of SendAsync

    } // end of class AzureEventHubTarget
}
