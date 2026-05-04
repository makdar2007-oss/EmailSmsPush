using NotificationApp.Interfaces;
using System.Threading;

namespace NotificationApp.Services
{
    public class SmsService : INotificationService
    {
        private readonly ILogger _logger;
        public string Name => "SMS";

        public SmsService(ILogger logger)
        {
            _logger = logger;
        }

        public void Send(string message)
        {
            _logger.LogInfo($"SmsService: отправка '{message}'");
            Thread.Sleep(300);
            _logger.LogInfo("SMS успешно отправлена.");
        }
    }
}