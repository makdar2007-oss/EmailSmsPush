using NotificationApp.Interfaces;
using System.Threading;

namespace NotificationApp.Services
{
    public class EmailService : INotificationService
    {
        private readonly ILogger _logger;
        public string Name => "Email";

        public EmailService(ILogger logger)
        {
            _logger = logger;
        }

        public void Send(string message)
        {
            _logger.LogInfo($"EmailService: отправка '{message}'");
            Thread.Sleep(500);
            _logger.LogInfo("Email успешно отправлен.");
        }
    }
}