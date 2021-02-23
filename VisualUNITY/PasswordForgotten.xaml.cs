using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualUNITY.DiameterLast;

namespace VisualUNITY
{
    /// <summary>
    /// Interaction logic for PasswordForgotten.xaml
    /// </summary>
    public partial class PasswordForgotten : UserControl
    {
        DockPanel panel;
        SqlConnection localDbConnection;
        string userName, userId, userMail;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        WelcomeScreen welcomeScreen;
        string password;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public PasswordForgotten(DockPanel dockPanel, WelcomeScreen welcome)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);

            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            panel = dockPanel;
            welcomeScreen = welcome;

            Snackbar.MessageQueue = myMessageQueue;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login(panel, welcomeScreen);
            DockPanel.SetDock(login, Dock.Left);
            panel.Children.Clear();
            panel.Children.Add(login);
        }

        private void HandleUndoMethod()
        {
            
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            buttonSendEvent();
        }

        private void KeyDownPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                buttonSendEvent();
            }
        }

        private void textMail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textMail.Text.Contains("@"))
            {
                textMail.Text = textMail.Text.Replace("@", String.Empty);
                comboBoxMail.Focus();
            }
        }

        private void buttonSendEvent()
        {
            card.Visibility = Visibility.Visible;
            userName = textNameSurname.Text.Trim();
            userId = textUserId.Text.Trim();
            userMail = textMail.Text.Trim() + "@" + comboBoxMail.Text.Trim();

            if (userName.Trim() == "" || userId.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("blanksCannotBeEmpty", cultureInfo), "OK", () => HandleUndoMethod());
                card.Visibility = Visibility.Hidden;
                return;
            }
            if (comboBoxMail.Text.Trim() == "" || textMail.Text.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("mailEmpty", cultureInfo), "OK", () => HandleUndoMethod());
                card.Visibility = Visibility.Hidden;
                return;
            }

            string query = "SELECT * FROM Users WHERE UserId = '" + userId + "' ";
            SqlCommand command = new SqlCommand(query, localDbConnection);
            localDbConnection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["NameSurname"].ToString().Trim().ToLower() == userName.ToLower() && reader["Mail"].ToString().ToLower() == userMail.ToLower())
                {
                    password = reader["Password"].ToString();
                    try
                    {
                        sendPassword();
                    }
                    catch (SmtpException) 
                    { 
                        myMessageQueue.Enqueue(rm.GetString("noInternetConnection", cultureInfo), "OK", () => HandleUndoMethod());
                        localDbConnection.Close();
                        card.Visibility = Visibility.Hidden;
                        return;
                    }
                    
                    Login login = new Login(panel, welcomeScreen);
                    DockPanel.SetDock(login, Dock.Left);
                    panel.Children.Clear();
                    panel.Children.Add(login);
                    card.Visibility = Visibility.Hidden;
                    MessageBox.Show(rm.GetString("mailSent"), rm.GetString("system"),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            localDbConnection.Close();
            myMessageQueue.Enqueue(rm.GetString("userNotFound", cultureInfo), "OK", () => HandleUndoMethod());
            card.Visibility = Visibility.Hidden;
            return;
        }

        private void sendPassword()
        {
            string subject = "Unity Mod Bus Studio Şifre Yenileme";
            string body = "<html><body><p>Sayın " + userName + ",</p>"
                     + "<p>Şifrenizle ilgili talebiniz ulaşmıştır. Bilgilerinizin güvenliği için kimseyle paylaşmayınız.</p>"
                     + "<br>Kullanıcı Adınız: " + userId + "</br>"
                     + "<p>Şifreniz: " + password + "</p>"
                     + "<br>Şifre talebini siz oluşturmadıysanız hemen iletişime geçiniz.</br>"
                     +"<p>Unity Yazılım Ekibi</p></body></html>";

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.live.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("unitysoftware@hotmail.com", "ya++--1122");
            MailMessage mailMessage = new MailMessage("unitysoftware@hotmail.com", userMail, subject, body)
            {
                IsBodyHtml = true,
                BodyEncoding = UTF8Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };
            client.Send(mailMessage);
        }
    }
}
