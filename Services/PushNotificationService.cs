using NotificationApp.Interfaces;
using System;
using System.Threading;

namespace NotificationApp.Services
{
    public class PushNotificationService : INotificationService
    {
        private readonly ILogger _logger;
        private static readonly Random _random = new Random();
        public string Name => "Push";

        public PushNotificationService(ILogger logger)
        {
            _logger = logger;
        }

        public void Send(string message)
        {
            _logger.LogInfo($"PushService: отправка '{message}'");
            Thread.Sleep(400);
            _logger.LogInfo("Push-уведомление успешно отправлено.");
        }
    }
}