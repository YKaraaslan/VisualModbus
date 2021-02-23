using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WebGuide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection localDbConnection;

        public MainWindow()
        {
            InitializeComponent();
            openUserControl(new Device());
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
        }

        private void clicked(object sender, RoutedEventArgs e)
        {
            string queryForAdmin = "SELECT NameSurname FROM UserInformation WHERE Id = 1";
            SqlCommand commandForAdmin = new SqlCommand(queryForAdmin, localDbConnection);

            localDbConnection.Open();
            SqlDataReader reader = commandForAdmin.ExecuteReader();
            if (reader.Read())
            {
                MessageBox.Show(reader[0].ToString());
            }
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
            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_messages.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
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
            openUserControl(new Device());
        }

        private void ListViewItemMail_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //openUserControl(new Mail());
        }

        private void ListViewItemProjects_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //openUserControl(new Projects());
        }

        private void ListViewItemSignOut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

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
            string filename = System.AppDomain.CurrentDomain.BaseDirectory + @"\Pdf\capolcer.pdf";
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

        }
    }
}
