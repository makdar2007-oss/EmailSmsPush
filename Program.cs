using Microsoft.Extensions.DependencyInjection;
using NotificationApp.Interfaces;
using NotificationApp.Logging;
using NotificationApp.Services;
using System;
using System.Windows.Forms;

namespace NotificationApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            services.AddSingleton<ILogger, FileAndUILogger>();

            services.AddTransient<INotificationService, EmailService>();
            services.AddTransient<INotificationService, SmsService>();
            services.AddTransient<INotificationService, PushNotificationService>();

            services.AddTransient<NotificationSender>();
            services.AddTransient<Func<INotificationService, NotificationSender>>(provider =>
            {
                return service =>
                    ActivatorUtilities.CreateInstance<NotificationSender>(provider, service);
            });
            services.AddTransient<MainForm>();

            using (var provider = services.BuildServiceProvider())
            {
                var form = provider.GetRequiredService<MainForm>();
                Application.Run(form);
            }
        }
    }
}