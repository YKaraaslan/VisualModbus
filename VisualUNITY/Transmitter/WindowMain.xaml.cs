using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
using VisualUNITY.DiameterLast;

namespace VisualUNITY.Transmitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;

        public WindowMain()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            openUserControl(new DeviceTransmitter());
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_properties.Visibility = Visibility.Collapsed;
                tt_messages.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_properties.Visibility = Visibility.Visible;
                tt_messages.Visibility = Visibility.Visible;
                tt_signout.Visibility = Visibility.Visible;
            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            panel.IsEnabled = true;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            panel.IsEnabled = false;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void ListViewItemHome_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            openUserControl(new DeviceTransmitter());
        }

        private void ListViewItemMail_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            openUserControl(new TransmitterMail());
        }

        private void ListViewItemSignOut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show(rm.GetString("areYouSureToExit"), rm.GetString("system"), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Properties.Settings.Default.username = string.Empty;
                Properties.Settings.Default.password = string.Empty;
                Properties.Settings.Default.nameSurname = string.Empty;
                Properties.Settings.Default.userMail = string.Empty;
                Properties.Settings.Default.signedIn = false;
                Properties.Settings.Default.userDbID = string.Empty;
                Properties.Settings.Default.phoneNumber = string.Empty;
                Properties.Settings.Default.Save();
                Vars.JustSignedOut = true;
                Vars.IsAdmin = false;
                WelcomeScreen welcomeScreen = new WelcomeScreen();
                welcomeScreen.Show();
                this.Close();
            }
        }

        private void openUserControl(UserControl myControl)
        {
            DockPanel.SetDock(myControl, Dock.Left);
            dockPanel.Children.Clear();
            dockPanel.Children.Add(myControl);

            if (Tg_Btn.IsChecked == true)
            {
                HideStackPanel.Begin();
                Tg_Btn.IsChecked = false;
            }
        }

        private void settingsClicked(object sender, RoutedEventArgs e)
        {
            //Settings settings = new Settings();
            //settings.ShowDialog();
        }

        private void informationClicked(object sender, RoutedEventArgs e)
        {
            string filename = System.AppDomain.CurrentDomain.BaseDirectory + @"\Pdf\transmitter.pdf";
            Process.Start(filename);
        }

        private void notification_Click(object sender, RoutedEventArgs e)
        {
            //Notifications notifications = new Notifications();
            //notifications.ShowDialog();
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                searchEvent();
            }
        }

        private void searchEvent()
        {

        }

        private void UserSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!Vars.IsAdmin)
            {
                SettingsUser userSettings = new SettingsUser();
                userSettings.ShowDialog();
            }
        }

        private void ListViewItemProperties_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //openUserControl(new PropertiesSettings());
        }
    }
}
