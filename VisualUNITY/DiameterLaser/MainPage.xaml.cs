using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Threading;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DispatcherTimer timer = new DispatcherTimer();
        int valOld = 0;

        public MainPage()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True");
            openUserControl(new Dashboard(dockPanel));
            Vars.selectedUserControl = "Dashboard";
            if (Vars.IsAdmin)
                deviceSettings.Visibility = Visibility.Visible;

            try
            {
                init();
            }
            catch { }

            timer.Tick += timer1_Tick;
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                init();
            }
            catch { }
        }

        private void init()
        {
            expanderUserName.Header = Properties.Settings.Default.nameSurname;
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SqlCommand commandToCreateProject = new SqlCommand("SELECT dbo.getNotificationsAmount()", localDbConnection);
            int val = (int)commandToCreateProject.ExecuteScalar();
            if (val != valOld)
                badge.Badge = val;
            valOld = val;
            localDbConnection.Close();
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
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_projects.Visibility = Visibility.Collapsed;
                tt_devices.Visibility = Visibility.Collapsed;
                tt_contacts.Visibility = Visibility.Collapsed;
                tt_customers.Visibility = Visibility.Collapsed;
                tt_products.Visibility = Visibility.Collapsed;
                tt_reports.Visibility = Visibility.Collapsed;
                tt_messages.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
                tt_deviceSettings.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_projects.Visibility = Visibility.Visible;
                tt_devices.Visibility = Visibility.Visible;
                tt_contacts.Visibility = Visibility.Visible;
                tt_customers.Visibility = Visibility.Visible;
                tt_products.Visibility = Visibility.Visible;
                tt_reports.Visibility = Visibility.Visible;
                tt_messages.Visibility = Visibility.Visible;
                tt_signout.Visibility = Visibility.Visible;
                tt_deviceSettings.Visibility = Visibility.Visible;
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
            if (Vars.selectedUserControl != "Dashboard")
            {
                openUserControl(new Dashboard(dockPanel));
                Vars.selectedUserControl = "Dashboard";
            }
        }

        private void ListViewItemDevices_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "Devices")
            {
                openUserControl(new Devices());
                Vars.selectedUserControl = "Devices";
            }
        }

        private void ListViewItemOperators_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "Operators")
            {
                openUserControl(new Operators());
                Vars.selectedUserControl = "Operators";
            }
        }

        private void ListViewItemCustomers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "Customers")
            {
                openUserControl(new Customers());
                Vars.selectedUserControl = "Customers";
            }
        }

        private void ListViewItemReports_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "Reports")
            {
                openUserControl(new Reports());
                Vars.selectedUserControl = "Reports";
            }
        }

        private void ListViewItemMail_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "Mail")
            {
                openUserControl(new Mail());
                Vars.selectedUserControl = "Mail";
            }
        }

        private void ListViewItemProjects_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "Projects")
            {
                openUserControl(new Projects());
                Vars.selectedUserControl = "Projects";
            }
        }
        private void ListViewItemProducts_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "Products")
            {
                openUserControl(new Products());
                Vars.selectedUserControl = "Products";
            }
        }

        private void ListViewItemDeviceSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Vars.selectedUserControl != "SettingsDevice")
            {
                openUserControl(new SettingsDevice());
                Vars.selectedUserControl = "SettingsDevice";
            }
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
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void informationClicked(object sender, RoutedEventArgs e)
        {
            string filename = System.AppDomain.CurrentDomain.BaseDirectory + @"\Pdf\capolcer.pdf";
            Process.Start(filename);
        }

        private void notification_Click(object sender, RoutedEventArgs e)
        {
            Notifications notifications = new Notifications();
            notifications.ShowDialog();
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
            if (search.Text == "")
                return;

            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            if (localDbConnectionUnity.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();

            int pageCounter = 0;

            SqlCommand commandCount1 = new SqlCommand("SELECT COUNT(*) FROM dbo.searchProjectsAll(@key) ", localDbConnection);
            commandCount1.Parameters.AddWithValue("@key", search.Text.Trim());
            if((int) commandCount1.ExecuteScalar() > 0)
            {
                SearchProjectsOpen searchProjectsOpen = new SearchProjectsOpen(search.Text.Trim());
                searchProjectsOpen.Show();
                pageCounter++;
            }

            SqlCommand commandCount2 = new SqlCommand("SELECT COUNT(*) FROM dbo.getProjectsToDoForSearch(@key) ", localDbConnection);
            commandCount2.Parameters.AddWithValue("@key", search.Text.Trim());
            if ((int)commandCount2.ExecuteScalar() > 0)
            {
                SearchProjectsToDoOpen searchProjectsOpen = new SearchProjectsToDoOpen(search.Text.Trim());
                searchProjectsOpen.Show();
                pageCounter++;
            }

            SqlCommand commandCount3 = new SqlCommand("SELECT COUNT(*) FROM dbo.getOperatorsForSearch(@key) ", localDbConnectionUnity);
            commandCount3.Parameters.AddWithValue("@key", search.Text.Trim());
            if ((int)commandCount3.ExecuteScalar() > 0)
            {
                SqlCommand commandCount3_1 = new SqlCommand("SELECT * FROM dbo.getOperatorsForSearch(@key) ", localDbConnectionUnity);
                commandCount3_1.Parameters.AddWithValue("@key", search.Text.Trim());

                SqlDataAdapter dataAdapterOperatorsProjectAmount = new SqlDataAdapter(commandCount3_1);
                DataTable dataTableOperatorsProjectAmount = new DataTable();
                dataAdapterOperatorsProjectAmount.Fill(dataTableOperatorsProjectAmount);

                for (int i = 0; i < dataTableOperatorsProjectAmount.Rows.Count; i++)
                {
                    OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsProjectAmount.Rows[i][0].ToString());
                    operatorsInformation.Show();
                    pageCounter++;
                }
            }

            SqlCommand commandCount4 = new SqlCommand("SELECT COUNT(*) FROM dbo.companiesGroup() WHERE Company = @key", localDbConnection);
            commandCount4.Parameters.AddWithValue("@key", search.Text.Trim());
            if ((int)commandCount4.ExecuteScalar() > 0)
            {
                SearchCustomers searchCustomers = new SearchCustomers(search.Text.Trim());
                searchCustomers.Show();
                pageCounter++;
            }

            SqlCommand commandCount5 = new SqlCommand("SELECT COUNT(*) FROM dbo.productsGroup() WHERE Product = @key", localDbConnection);
            commandCount5.Parameters.AddWithValue("@key", search.Text.Trim());
            if ((int)commandCount5.ExecuteScalar() > 0)
            {
                SearchProducts searchProducts = new SearchProducts(search.Text.Trim());
                searchProducts.Show();
                pageCounter++;
            }

            localDbConnection.Close();
            localDbConnectionUnity.Close();

            if (pageCounter == 0)
                MessageBox.Show(rm.GetString("searchCannotBeFound"), rm.GetString("system"), MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void UserSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!Vars.IsAdmin)
            {
                SettingsUser userSettings = new SettingsUser();
                userSettings.ShowDialog();
            }
        }
    }
}
