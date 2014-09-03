using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SilosWrapper
{
    [Target("Silos")]
    public class SilosTarget : TargetWithLayout
    {
        private readonly Uri host = new Uri("http://silos.ermlab.com/api/call/");

        public SilosTarget()
        {
        }

        private string apiKey;

        [RequiredParameter]
        public string ApiKey
        {
            get
            {
                return apiKey;
            }
            set
            {
                apiKey = value;
            }
        }

        private string logbookName;

        [RequiredParameter]
        public string LogbookName
        {
            get
            {
                return logbookName;
            }
            set
            {
                logbookName = value;
            }
        }

        private string logger;

        public string Logger
        {
            get
            {
                return logger;
            }
            set
            {
                logger = value;
            }
        }


        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["name"] = this.LogbookName;
                values["logger"] = this.Logger;
                values["body"] = logMessage;
                values["level"] = this.findLevel(logEvent);

                var response = client.UploadValues(combineHostWithKey(), values);

                var responseString = Encoding.Default.GetString(response);
            }
        }

        private string combineHostWithKey()
        {
            return string.Format("{0}{1}", this.host, this.ApiKey);
        }

        private string findLevel(LogEventInfo logEvent)
        {
            //default level is debug
            int result = 2;

            if (logEvent.Level == LogLevel.Trace)
            {
                result = 1;
            }
            else if (logEvent.Level == LogLevel.Debug)
            {
                result = 2;
            }
            else if (logEvent.Level == LogLevel.Info)
            {
                result = 3;
            }
            else if (logEvent.Level == LogLevel.Warn)
            {
                result = 4;
            }
            else if (logEvent.Level == LogLevel.Error)
            {
                result = 5;
            }
            else if (logEvent.Level == LogLevel.Fatal)
            {
                result = 6;
            }

            return result.ToString();
        }
    }
}
