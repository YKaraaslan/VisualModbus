using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using VisualUNITY.TensionControl;
using VisualUNITY.WebGuide;
using VisualUNITY.Transmitter;

namespace VisualUNITY
{
    /// <summary>
    /// Interaction logic for ChooseScreen.xaml
    /// </summary>
    public partial class ChooseScreen : Window
    {
        public ChooseScreen()
        {
            InitializeComponent();
            diameter.IsChecked = true;
        }

        private void diameter_Checked(object sender, RoutedEventArgs e)
        {
            diameter.IsChecked = true;
            web.IsChecked = false;
            tension.IsChecked = false;
            transmitter.IsChecked = false;

            labelDiameter.Foreground = Brushes.Green;
            webDiameter.Foreground = Brushes.Black;
            tensionDiameter.Foreground = Brushes.Black;
            transmitterDiameter.Foreground = Brushes.Black;
        }

        private void web_Checked(object sender, RoutedEventArgs e)
        {
            diameter.IsChecked = false;
            web.IsChecked = true;
            tension.IsChecked = false;
            transmitter.IsChecked = false;

            labelDiameter.Foreground = Brushes.Black;
            webDiameter.Foreground = Brushes.Green;
            tensionDiameter.Foreground = Brushes.Black;
            transmitterDiameter.Foreground = Brushes.Black;
        }

        private void tension_Checked(object sender, RoutedEventArgs e)
        {
            diameter.IsChecked = false;
            web.IsChecked = false;
            tension.IsChecked = true;
            transmitter.IsChecked = false;

            labelDiameter.Foreground = Brushes.Black;
            webDiameter.Foreground = Brushes.Black;
            tensionDiameter.Foreground = Brushes.Green;
            transmitterDiameter.Foreground = Brushes.Black;
        }

        private void transmitter_Checked(object sender, RoutedEventArgs e)
        {

            diameter.IsChecked = false;
            web.IsChecked = false;
            tension.IsChecked = false;
            transmitter.IsChecked = true;

            labelDiameter.Foreground = Brushes.Black;
            webDiameter.Foreground = Brushes.Black;
            tensionDiameter.Foreground = Brushes.Black;
            transmitterDiameter.Foreground = Brushes.Green;
        }

        private int countChecked()
        {
            int count = 0;
            if ((bool)diameter.IsChecked)
                count++;
            if ((bool)web.IsChecked)
                count++;
            if ((bool)tension.IsChecked)
                count++;
            if ((bool)transmitter.IsChecked)
                count++;

            return count;
        }

        private void continue_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.projectDiameter = false;
            Properties.Settings.Default.projectWeb = false;
            Properties.Settings.Default.projectTension = false;
            Properties.Settings.Default.projectTransmitter = false;
            Properties.Settings.Default.Save();


            if (diameter.IsChecked == true)
            {
                Properties.Settings.Default.projectDiameter = true;
                MainPage mainPage = new MainPage();
                mainPage.Show();
            }
            else if (web.IsChecked == true)
            {
                HomeScreen homeScreen = new HomeScreen();
                homeScreen.Show();
            }
            else if (tension.IsChecked == true)
            {
                Properties.Settings.Default.projectTension = true;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else if (transmitter.IsChecked == true)
            {
                Properties.Settings.Default.projectTransmitter = true;
                WindowMain mainWindow = new WindowMain();
                mainWindow.Show();
            }

            Properties.Settings.Default.signedIn = true;
            Properties.Settings.Default.userFirstTime = false;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void diameter_Unchecked(object sender, RoutedEventArgs e)
        {
            if (countChecked() < 1)
                diameter.IsChecked = true;
        }

        private void web_Unchecked(object sender, RoutedEventArgs e)
        {
            if (countChecked() < 1)
                web.IsChecked = true;
        }

        private void tension_Unchecked(object sender, RoutedEventArgs e)
        {
            if (countChecked() < 1)
                tension.IsChecked = true;
        }

        private void transmitter_Unchecked(object sender, RoutedEventArgs e)
        {
            if (countChecked() < 1)
                transmitter.IsChecked = true;
        }
    }
}
