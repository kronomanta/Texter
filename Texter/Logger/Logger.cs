using System;
using System.Globalization;

namespace Texter.Logger
{
    public class Logger : ILogger
    {
        private static readonly log4net.ILog log =
                                log4net.LogManager.GetLogger(typeof(Logger));

        #region ILogger Members

        public void LogException(Exception exception, string message)
        {
            if (log.IsErrorEnabled)
                log.Error(
                     string.Format(CultureInfo.InvariantCulture, "{0}", message),
                     exception);
        }

        public void LogException(Exception exception)
        {
            LogException(exception, exception.Message);
        }

        public void LogError(string message)
        {
            if (log.IsErrorEnabled)
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        public void LogInfoMessage(string message)
        {
            if (log.IsInfoEnabled)
                log.Info(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        #endregion
    }

}
