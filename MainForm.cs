using NotificationApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotificationApp
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<string, INotificationService> _services;
        private readonly ILogger _logger;
        private readonly Func<INotificationService, NotificationSender> _senderFactory;
        private int _emptyMessageCounter = 0;
        private int _successCounter = 0;
        private int _smsCounter = 0;

        public MainForm(IEnumerable<INotificationService> services, ILogger logger,Func<INotificationService, NotificationSender> senderFactory)
        {
            _services = services.ToDictionary(s => s.Name);
            _logger = logger;
            _senderFactory = senderFactory;

            InitializeComponent();

            cmbServiceType.DataSource = _services.Keys.ToList();
            _logger.OnLogAdded += AppendLog;
        }

        private void TxtMessage_GotFocus(object sender, EventArgs e)
        {
            if (txtMessage.Text == "Введите сообщение...")
            {
                txtMessage.Text = "";
                txtMessage.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtMessage_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                txtMessage.Text = "Введите сообщение...";
                txtMessage.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSend_Click(btnSend, EventArgs.Empty);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string egg = "Йоуууууууу, 2 команда лучшая!!!";

            MessageBox.Show(egg,"Секретная пасхалка",MessageBoxButtons.OK,MessageBoxIcon.Information);
            _logger.LogInfo($"[ПАСХАЛКА] {egg}");
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();

            if (string.IsNullOrEmpty(message) || message == "Введите сообщение...")
            {
                _emptyMessageCounter++;

                if (_emptyMessageCounter >= 2)
                {
                    string easter = " Тимурка-мурка";
                    MessageBox.Show(easter, "А вот и пасхалка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _logger.LogInfo($"[ПАСХАЛКА] {easter}");
                    _emptyMessageCounter = 0;
                }
                else
                {
                    string error = "Сообщение не может быть пустым.";
                    MessageBox.Show(error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _logger.LogError(error);
                }

                return;
            }

            string selectedService = cmbServiceType.SelectedItem.ToString();

            if (!_services.TryGetValue(selectedService, out var service))
            {
                _logger.LogError($"Не найден сервис {selectedService}");
                return;
            }

            btnSend.Enabled = false;

            try
            {
                _logger.LogInfo($"Выбран сервис: {selectedService}. Сообщение: '{message}'");

                var notificationSender = _senderFactory(service);
                await Task.Run(() => notificationSender.SendNotification(message));

                _successCounter++;

                if (selectedService == "SMS")
                {
                    _smsCounter++;
                }

                await ShowEasterEggs(selectedService, message);

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Ошибка отправки через {selectedService}: {ex.Message}";
                _logger.LogError(errorMsg);
                MessageBox.Show(errorMsg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }

        private void cmbServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbServiceType.SelectedItem.ToString();
            _logger.LogInfo($"Пользователь выбрал сервис: {selected}");
        }

        private async Task ShowEasterEggs(string serviceType, string message)
        {
            if (_successCounter == 2)
            {
                string egg = "Артём говорит отстооооооой!";
                _logger.LogInfo($"[ПАСХАЛКА] {egg}");
                MessageBox.Show(egg, " ПССС, пасхалка", MessageBoxButtons.OK, MessageBoxIcon.None);
                await Task.Delay(100);
            }

            if (message.ToLower().Contains("тимур"))
            {
                string egg = "Тимурка, ну ты чего? Мог бы и постараться... не полениться";

                _logger.LogInfo("[ПАСХАЛКА] Пасхалка");

                MessageBox.Show( egg, "Нуу пасхалка вроде", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await Task.Delay(100);
            }

            if (_successCounter % 5 == 0 && _successCounter != 2) 
            {
                string egg = $" Артём опять говорит, что это отстооооооой, но ты уже отправил(а) {_successCounter} уведомлений!";
                _logger.LogInfo($"[ПАСХАЛКА] {egg}");
                MessageBox.Show(egg, " ПССС, пасхалка", MessageBoxButtons.OK, MessageBoxIcon.None);
                await Task.Delay(100);
            }

            if (serviceType == "SMS" && _smsCounter % 2 == 0 && _smsCounter > 0)
            {
                string egg = " Оля легенда!";
                _logger.LogInfo($"[ПАСХАЛКА] {egg}");
                MessageBox.Show(egg, "Йоу пасхалка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await Task.Delay(100);
            }


            if (message.Contains("67"))
            {
                string egg = "six-seveeeen";

                _logger.LogInfo("[ПАСХАЛКА] Активирована пасхалка 67");

                MessageBox.Show(egg, "Системное сообщение", MessageBoxButtons.OK,MessageBoxIcon.Information);

                await Task.Delay(100);
            }
        }

        private void AppendLog(string logEntry)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => AppendLog(logEntry)));
                return;
            }
            rtbLog.AppendText(logEntry + Environment.NewLine);
            rtbLog.ScrollToCaret();
        }
    }
}