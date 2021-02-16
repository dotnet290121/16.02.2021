using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4netMssql
{
    class Program
    {
        private static readonly log4net.ILog my_logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            my_logger.Info("******************** System startup");
            my_logger.Debug("This is a debug message");
            my_logger.Warn("This is a warning message");
            my_logger.Error("This is an ERROR message");
            my_logger.Fatal("This is a FATAL message");
            my_logger.Info("******************** System shutdown");

        }
    }
}
