using System;
using System.Collections.Generic;
using System.Data;
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

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for Mail.xaml
    /// </summary>
    public partial class Mail : UserControl
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;

        public Mail()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
        }

        private void sendMail(object sender, RoutedEventArgs e)
        {
            if (mailSubject.Text.Trim() == "" || mailContent.Text.Trim() == "")
            {

                MessageBox.Show(rm.GetString("fillBlanks", cultureInfo), rm.GetString("system", cultureInfo),
                            MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                string subject = mailSubject.Text.Trim();
                string body = "<html><body><p>Gönderen: " + Properties.Settings.Default.nameSurname + ",</p>"
                         + "<p>" + mailContent.Text.Trim() + "</p>"
                         + "<p>" + Properties.Settings.Default.userMail + " " + Properties.Settings.Default.phoneNumber + "</p></body></html>";

                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.live.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("unitysoftware@hotmail.com", "ya++--1122");
                MailMessage mailMessage = new MailMessage("unitysoftware@hotmail.com", "yazilim@unitygrup.com", subject, body)
                {
                    IsBodyHtml = true,
                    BodyEncoding = UTF8Encoding.UTF8,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                client.Send(mailMessage);
            }
            catch (SmtpException)
            {
                MessageBox.Show(rm.GetString("noInternetConnection", cultureInfo), rm.GetString("system", cultureInfo),
                            MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SqlCommand commandToAddDatabase = new SqlCommand("dbo.addMail", localDbConnection);

            commandToAddDatabase.CommandType = CommandType.StoredProcedure;
            commandToAddDatabase.Parameters.AddWithValue("@Sender", Properties.Settings.Default.nameSurname);
            commandToAddDatabase.Parameters.AddWithValue("@SenderMail", Properties.Settings.Default.userMail);
            commandToAddDatabase.Parameters.AddWithValue("@Subject", mailSubject.Text.Trim());
            commandToAddDatabase.Parameters.AddWithValue("@Content", mailContent.Text.Trim());
            commandToAddDatabase.Parameters.AddWithValue("@Date", Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
            commandToAddDatabase.ExecuteNonQuery();
            localDbConnection.Close();
            mailSubject.Text = "";
            mailContent.Text = "";
            MessageBox.Show(rm.GetString("mailSentSuccessful", cultureInfo), rm.GetString("system", cultureInfo),
                        MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void listMail(object sender, RoutedEventArgs e)
        {
            MailList mailList = new MailList();
            mailList.ShowDialog();
        }
    }
}
