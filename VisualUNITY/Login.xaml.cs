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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualUNITY.DiameterLast;
using VisualUNITY.TensionControl;
using VisualUNITY.Transmitter;
using VisualUNITY.WebGuide;

namespace VisualUNITY
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        DockPanel panel;
        SqlConnection localDbConnection;
        string userId, userPassword;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        WelcomeScreen welcomeScreen;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public Login(DockPanel dockPanel, WelcomeScreen welcome)
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

            if (Properties.Settings.Default.signedIn)
            {
                MainPage mainPage = new MainPage();
                welcomeScreen.Close();
                mainPage.Show();
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonLoginEvent();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void HandleOKMethod()
        {

        }

        private void buttonLoginEvent()
        {
            card.Visibility = Visibility.Visible;
            userId = id.Text.Trim();
            userPassword = password.Password.Trim();

            if (userId.Trim() == "" || userPassword.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("blanksCannotBeEmpty", cultureInfo), "OK", () => HandleOKMethod());
                card.Visibility = Visibility.Hidden;
                return;
            }
            string query = "SELECT NameSurname, Mail, Phone, Id FROM Users WHERE UserId = '" + userId + "' AND Password = '" + userPassword + "' ";
            SqlCommand command = new SqlCommand(query, localDbConnection);

            string queryForAdmin = "SELECT NameSurname FROM Admins WHERE userID = '" + userId + "' AND Password = '" + userPassword + "' ";
            SqlCommand commandForAdmin = new SqlCommand(queryForAdmin, localDbConnection);

            localDbConnection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Variables.UserName = reader[0].ToString();
                string status = Convert.ToBoolean(checkBoxRemember.IsChecked) ? "+" : "-";
                if (status == "+")
                {
                    Properties.Settings.Default.username = userId;
                    Properties.Settings.Default.password = userPassword;
                    Properties.Settings.Default.nameSurname = reader[0].ToString();
                    Properties.Settings.Default.userMail = reader[1].ToString();
                    Properties.Settings.Default.signedIn = true;
                    Properties.Settings.Default.phoneNumber = reader[2].ToString();
                    Properties.Settings.Default.userDbID = reader[3].ToString();
                    Properties.Settings.Default.Save();
                }
                myMessageQueue.Enqueue(rm.GetString("welcome", cultureInfo), "OK", () => HandleOKMethod());
                localDbConnection.Close();
                if (!Properties.Settings.Default.userFirstTime)
                    openSelectedProject();
                else
                    openSelectProject();
                card.Visibility = Visibility.Hidden;
                return;
            }
            localDbConnection.Close();
            localDbConnection.Open();
            SqlDataReader readerForAdmin = commandForAdmin.ExecuteReader();
            if (readerForAdmin.Read())
            {
                Variables.UserName = readerForAdmin[0].ToString();
                Properties.Settings.Default.nameSurname = readerForAdmin[0].ToString();
                myMessageQueue.Enqueue(rm.GetString("welcome", cultureInfo), "OK", () => HandleOKMethod());
                localDbConnection.Close();
                Vars.IsAdmin = true;
                if (!Properties.Settings.Default.userFirstTime)
                    openSelectedProject();
                else
                    openSelectProject();
                localDbConnection.Close();
                card.Visibility = Visibility.Hidden;
                return;
            }
            else
                myMessageQueue.Enqueue(rm.GetString("incorrectIdOrPassword", cultureInfo), "OK", () => HandleOKMethod());

            localDbConnection.Close();
            card.Visibility = Visibility.Hidden;
        }

        private void openSelectProject()
        {
            ChooseScreen chooseScreen = new ChooseScreen();
            chooseScreen.Show();
            welcomeScreen.Close();
        }

        private void openSelectedProject()
        {
            if (Properties.Settings.Default.projectDiameter)
            {
                MainPage mainPage = new MainPage();
                mainPage.Show();
            }
            else if (Properties.Settings.Default.projectWeb)
            {
                HomeScreen homeScreen = new HomeScreen();
                homeScreen.Show();
            }
            else if (Properties.Settings.Default.projectTension)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else if (Properties.Settings.Default.projectTransmitter)
            {
                WindowMain mainWindow = new WindowMain();
                mainWindow.Show();
            }
            welcomeScreen.Close();
        }

        private void passwordForgotten_Click(object sender, RoutedEventArgs e)
        {
            PasswordForgotten passwordForgotten = new PasswordForgotten(panel, welcomeScreen);
            DockPanel.SetDock(passwordForgotten, Dock.Left);
            panel.Children.Clear();
            panel.Children.Add(passwordForgotten);
        }

        private void ToggleButtonFacebook_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.facebook.com/unityotomasyon.com.tr/");
            System.Diagnostics.Process.Start(@"https://www.facebook.com/unityotomasyonofficial");
        }

        private void ToggleButtonInstagram_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://instagram.com/unity.otomasyon?igshid=14ai0n44fasvd");
        }

        private void ToggleButtonTwitter_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://twitter.com/unityotomasyon");
        }

        private void ToggleButtonUnity_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.unityotomasyon.com.tr");
        }

        private void ToggleButtonYoutube_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.youtube.com/channel/UCkGAeeXojw8ExlUiMwWMFYg");
        }

        private void ToggleButtonLinkedIn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://tr.linkedin.com/company/unity-otomasyon");
        }

        private void KeyDownPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                buttonLoginEvent();
            }
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp(panel, welcomeScreen);
            DockPanel.SetDock(signUp, Dock.Left);
            panel.Children.Clear();
            panel.Children.Add(signUp);
        }
    }
}
