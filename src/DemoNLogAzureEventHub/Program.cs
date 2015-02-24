using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace DemoNLogAzureEventHub
{
    class Program
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            log.Info("info message test");
            log.Debug("debug message test");
            log.Warn("warn message test");
            log.Error("error message test");

            Console.WriteLine("press any key to end");
            Console.ReadLine();
        }
    }
}
