using NotificationApp.Interfaces;

namespace NotificationApp
{
    public class NotificationSender
    {
        private readonly INotificationService _service;
        private readonly ILogger _logger;

        public NotificationSender(INotificationService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public void SendNotification(string message)
        {
            _logger.LogInfo($"NotificationSender использует сервис: {_service.Name}");
            _service.Send(message);
        }
    }
}