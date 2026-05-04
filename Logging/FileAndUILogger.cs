using NotificationApp.Interfaces;
using System;
using System.IO;

namespace NotificationApp.Logging
{
    public class FileAndUILogger : ILogger
    {
        private readonly string _logFilePath;
        public event Action<string> OnLogAdded;

        public FileAndUILogger()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_log.txt");
            File.WriteAllText(_logFilePath, $"Лог запущен: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n");
        }

        private void WriteToFile(string logEntry)
        {
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                OnLogAdded?.Invoke($"[ERROR] Ошибка записи в файл: {ex.Message}");
            }
        }

        public void LogInfo(string message)
        {
            string entry = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            WriteToFile(entry);
            OnLogAdded?.Invoke(entry);
        }

        public void LogError(string message)
        {
            string entry = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            WriteToFile(entry);
            OnLogAdded?.Invoke(entry);
        }
    }
}