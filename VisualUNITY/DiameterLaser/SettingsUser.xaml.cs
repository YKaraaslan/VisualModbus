using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
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
using System.Windows.Shapes;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for SettingsUser.xaml
    /// </summary>
    public partial class SettingsUser : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public SettingsUser()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            init();
        }

        private void init()
        {
            string mail = Properties.Settings.Default.userMail;
            string[] words = mail.Split('@');
            nameSurname.Text = Properties.Settings.Default.nameSurname;
            phoneNumber.Text = Properties.Settings.Default.phoneNumber;
            textMail.Text = words[0];
            comboBoxMail.Text = words[1];
            userName.Text = Properties.Settings.Default.username;
            password.Password = Properties.Settings.Default.password;
            passwordRepeat.Password = Properties.Settings.Default.password;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void KeyDownPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                update();
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

        private void update()
        {
            string name, userID, id, pass, passRepeat, mail, phone;
            name = nameSurname.Text.Trim();
            userID = userName.Text.Trim();
            id = Properties.Settings.Default.userDbID;
            pass = password.Password.Trim();
            passRepeat = passwordRepeat.Password.Trim();
            mail = textMail.Text.Trim() + "@" + comboBoxMail.Text.Trim();
            phone = phoneNumber.Text.Trim();

            if (name.Trim() == "" || id.Trim() == "" || pass.Trim() == "" || passRepeat.Trim() == "" || phone.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("blanksCannotBeEmpty", cultureInfo), "OK", () => HandleUndoMethod());
                return;
            }
            if (pass.Length < 5 || pass.Length > 16)
            {
                myMessageQueue.Enqueue(rm.GetString("passwordMinAndMax", cultureInfo), "OK", () => HandleUndoMethod());
                return;
            }
            if (pass != passRepeat)
            {
                myMessageQueue.Enqueue(rm.GetString("passwordDoesntMatch", cultureInfo), "OK", () => HandleUndoMethod());
                return;
            }
            if (comboBoxMail.Text.Trim() == "" || textMail.Text.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("mailEmpty", cultureInfo), "OK", () => HandleUndoMethod());
                return;
            }

            localDbConnection.Open();
            SqlCommand commandUsers = new SqlCommand("dbo.updateUser", localDbConnection);
            commandUsers.CommandType = CommandType.StoredProcedure;

            commandUsers.Parameters.AddWithValue("@id", id);
            commandUsers.Parameters.AddWithValue("@Name", name);
            commandUsers.Parameters.AddWithValue("@UserId", userID);
            commandUsers.Parameters.AddWithValue("@Password", pass);
            commandUsers.Parameters.AddWithValue("@Mail", mail);
            commandUsers.Parameters.AddWithValue("@Phone", phone);
            try
            {
                commandUsers.ExecuteNonQuery();
                localDbConnection.Close();
                Properties.Settings.Default.username = userID;
                Properties.Settings.Default.password = pass;
                Properties.Settings.Default.nameSurname = name;
                Properties.Settings.Default.userMail = mail;
                Properties.Settings.Default.phoneNumber = phone;
                Properties.Settings.Default.Save();
                MessageBox.Show(rm.GetString("updateSucceeded", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { localDbConnection.Close(); }
        }

        private void HandleUndoMethod()
        {
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
