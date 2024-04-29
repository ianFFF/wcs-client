using System;
using System.Reflection;
using log4net;
using log4net.Config;

namespace wcs.core.common
{
    public class LLog
    {
        private static LLog _instance;
        private ILog _log;

        private LLog()
        {
            var log4netRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log4netRepository, new FileInfo("log4net.config"));

            this._log = LogManager.GetLogger(log4netRepository.Name, "wcs.core");
        }

        public static ILog log
        {
            get
            {
                if (LLog._instance == null)
                {
                    LLog._instance = new LLog();
                }

                return LLog._instance._log;
            }
        }
    }
}

