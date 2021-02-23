using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : UserControl
    {
        DockPanel panel;
        WelcomeScreen welcomeScreen;
        SqlConnection localDbConnection;
        string name, id, pass, passRepeat, passAdmin, mail, phone;
        protected string adminPass;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public SignUp(DockPanel dockPanel, WelcomeScreen welcome)
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
        private void KeyDownPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                buttonSignUpEvent();
            }
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textMail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textMail.Text.Contains("@"))
            {
                textMail.Text = textMail.Text.Replace("@", String.Empty);
                comboBoxMail.Focus();
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login(panel, welcomeScreen);
            DockPanel.SetDock(login, Dock.Left);
            panel.Children.Clear();
            panel.Children.Add(login);
        }
        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            buttonSignUpEvent();
        }

        private void buttonSignUpEvent()
        {
            name = textNameSurname.Text.Trim();
            id = textUserId.Text.Trim();
            pass = textPassword.Password.Trim();
            passRepeat = textPasswordRepeat.Password.Trim();
            passAdmin = textAdminConfirmation.Password.Trim();
            mail = textMail.Text.Trim() + "@" + comboBoxMail.Text.Trim();
            phone = phoneNumber.Text.Trim();

            if (name.Trim() == "" || id.Trim() == "" || pass.Trim() == "" || passRepeat.Trim() == "" || passAdmin == "" || phone.Trim() == "")
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
            string query = "SELECT Password FROM Admins";
            SqlCommand command = new SqlCommand(query, localDbConnection);
            localDbConnection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                adminPass = reader[0].ToString().Trim();
                if (adminPass == passAdmin)
                {
                    localDbConnection.Close();
                    register();
                    return;
                }
            }
            localDbConnection.Close();
            myMessageQueue.Enqueue(rm.GetString("adminConfirmationNotValid", cultureInfo), "OK", () => HandleUndoMethod());
            return;
        }

        private void HandleUndoMethod()
        {
            
        }

        private void register()
        {
            localDbConnection.Open();
            string saveIntoDatabase = "INSERT INTO Users (NameSurname, UserId, Password, Mail, Phone)" +
                   " VALUES (@Name, @UserId, @Password, @Mail, @Phone);";
            SqlCommand commandUsers = new SqlCommand(saveIntoDatabase, localDbConnection);

            commandUsers.Parameters.AddWithValue("@Name", name);
            commandUsers.Parameters.AddWithValue("@UserId", id);
            commandUsers.Parameters.AddWithValue("@Password", pass);
            commandUsers.Parameters.AddWithValue("@Mail", mail);
            commandUsers.Parameters.AddWithValue("@Phone", phone);
            try
            {
                commandUsers.ExecuteNonQuery();
                localDbConnection.Close();
                MessageBox.Show(rm.GetString("signUpSucceeded", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Login login = new Login(panel, welcomeScreen);
                DockPanel.SetDock(login, Dock.Left);
                panel.Children.Clear();
                panel.Children.Add(login);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);  }
            finally { localDbConnection.Close(); }
        }
    }
}
