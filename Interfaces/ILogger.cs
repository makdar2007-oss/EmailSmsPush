using System;

namespace NotificationApp.Interfaces
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
        event Action<string> OnLogAdded;
    }
}