using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using VisualUNITY.DiameterLast;
using VisualUNITY.TensionControl;

namespace VisualUNITY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public SplashScreen()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.language);
            InitializeComponent();
            dispatcherTimer.Tick += timer1_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(2300);
            dispatcherTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Properties.Settings.Default.userFirstTime = true;
            Properties.Settings.Default.signedIn = false;
            Properties.Settings.Default.Save();
            try
            {
                if (Properties.Settings.Default.signedIn)
                {
                    openProjectSelected();
                }
                else
                {
                    WelcomeScreen welcomeScreen = new WelcomeScreen();
                    welcomeScreen.Show();
                }            
            }
            catch { }
            dispatcherTimer.Stop();
            this.Close();
        }

        private void openProjectSelected()
        {
            if (Properties.Settings.Default.projectDiameter)
            {
                MainPage mainPage = new MainPage();
                mainPage.Show();
            }
            else if (Properties.Settings.Default.projectWeb)
            {
                Process.Start("WebGuide.exe");
            }
            else if (Properties.Settings.Default.projectTension)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else if (Properties.Settings.Default.projectTransmitter)
            {
                Process.Start("Transmitter.exe");
            }
        }
    }
}
