using System;
using System.Collections.Generic;
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
    /// Interaction logic for SettingsDeviceSettings.xaml
    /// </summary>
    public partial class SettingsDeviceSettings : Window
    {
        int deviceID;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;

        public SettingsDeviceSettings(int id)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            deviceID = id;
            init();
        }

        private void init()
        {
            byte[] slaveString = {Properties.Settings.Default.device1Slave, Properties.Settings.Default.device2Slave, Properties.Settings.Default.device3Slave,
            Properties.Settings.Default.device4Slave, Properties.Settings.Default.device5Slave, Properties.Settings.Default.device6Slave, Properties.Settings.Default.device7Slave,
            Properties.Settings.Default.device8Slave, Properties.Settings.Default.device9Slave, Properties.Settings.Default.device10Slave, Properties.Settings.Default.device11Slave,
            Properties.Settings.Default.device12Slave, Properties.Settings.Default.device13Slave, Properties.Settings.Default.device14Slave };

            slave.Text = slaveString[deviceID].ToString().Trim();
        }

        

        private void save(object sender, RoutedEventArgs e)
        {
            saveAll();
        }

        private void slave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                saveAll();
            }
        }

        private void saveAll()
        {
            try { int idTry = int.Parse(slave.Text); }
            catch { MessageBox.Show(rm.GetString("onlyInteger"), rm.GetString("ok"), MessageBoxButton.OK, MessageBoxImage.Warning); }
            switch (deviceID)
            {
                case 0:
                    Properties.Settings.Default.device1Slave = byte.Parse(slave.Text);
                    break;

                case 1:
                    Properties.Settings.Default.device2Slave = byte.Parse(slave.Text);
                    break;

                case 2:
                    Properties.Settings.Default.device3Slave = byte.Parse(slave.Text);
                    break;

                case 3:
                    Properties.Settings.Default.device4Slave = byte.Parse(slave.Text);
                    break;

                case 4:
                    Properties.Settings.Default.device5Slave = byte.Parse(slave.Text);
                    break;

                case 5:
                    Properties.Settings.Default.device6Slave = byte.Parse(slave.Text);
                    break;

                case 6:
                    Properties.Settings.Default.device7Slave = byte.Parse(slave.Text);
                    break;

                case 7:
                    Properties.Settings.Default.device8Slave = byte.Parse(slave.Text);
                    break;

                case 8:
                    Properties.Settings.Default.device9Slave = byte.Parse(slave.Text);
                    break;

                case 9:
                    Properties.Settings.Default.device10Slave = byte.Parse(slave.Text);
                    break;

                case 10:
                    Properties.Settings.Default.device11Slave = byte.Parse(slave.Text);
                    break;

                case 11:
                    Properties.Settings.Default.device12Slave = byte.Parse(slave.Text);
                    break;

                case 12:
                    Properties.Settings.Default.device13Slave = byte.Parse(slave.Text);
                    break;

                case 13:
                    Properties.Settings.Default.device14Slave = byte.Parse(slave.Text);
                    break;
            }
            Properties.Settings.Default.Save();
            MessageBox.Show(rm.GetString("changesSaved"), rm.GetString("ok"), MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
