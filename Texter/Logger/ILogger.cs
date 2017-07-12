using System;

namespace Texter.Logger
{
    public interface ILogger
    {
        void LogException(Exception exception);
        void LogException(Exception exception, string customMessage);
        void LogError(string message);
        void LogInfoMessage(string message);
    }
}
