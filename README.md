# nlog-targets-azureeventhub
NLog Target for Azure ServiceBus [EventHub](http://azure.microsoft.com/en-us/services/event-hubs/)

## Installation
You can install this target using NuGet.   

`Install-Package nlog.targets.azureeventhub`
   
See the included demo project to see how you can use this NLog target to send your log messages to Azure ServiceBus Event Hub.   
## Example Configuration
See below for sample NLog configuration.  

	<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  		<extensions>
    		<add assembly="NLog.Targets.AzureEventHub"/>
  		</extensions>

  		<variable name="DefaultLayout" value="${longdate} | ${level:uppercase=true:padding=5} | ${message} | ${exception:format=type,tostring}" />

  		<targets>
    		<target name="eh" type="AzureEventHub" layout="${DefaultLayout}" EventHubConnectionString="Endpoint=sb://yournamespace/;SharedAccessKeyName=send;SharedAccessKey=yourkey;TransportType=Amqp" EventHubPath="eventhubname" PartitionKey=""/>
    		<target name="ColorConsole" xsi:type="ColoredConsole" layout="${DefaultLayout}" />
  		</targets>

  		<rules>
    		<logger name="*" minlevel="Trace" writeTo="ColorConsole,eh" />
  		</rules>
	</nlog>
   
   
## NLog Version
This package references NLog Version 3.2.0.0   

## Azure ServiceBus SDK
This package also has a dependency on 2.6.1  


   


 

