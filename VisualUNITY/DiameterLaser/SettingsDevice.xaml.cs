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

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for SettingsDevice.xaml
    /// </summary>
    public partial class SettingsDevice : UserControl
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        int[] type = new int[14];
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public SettingsDevice()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            init();
        }

        private void init()
        {
            TextBlock[] operators = { operator1Text, operator2Text, operator3Text, operator4Text, operator5Text, operator6Text,
            operator7Text, operator8Text, operator9Text, operator10Text, operator11Text, operator12Text, operator13Text, operator14Text };

            TextBlock[] devices = { device1Text, device2Text, device3Text, device4Text, device5Text, device6Text,
            device7Text, device8Text, device9Text, device10Text, device11Text, device12Text, device13Text, device14Text };

            Card[] cards = { device1Card, device2Card, device3Card, device4Card, device5Card, device6Card,
            device7Card, device8Card, device9Card, device10Card, device11Card, device12Card, device13Card, device14Card };

            Image[] images = {image1, image2, image3, image4, image5, image6, image7, image8,
            image9, image10, image11, image12, image13, image14};

            for (int i = 0; i < Properties.Settings.Default.deviceAmount; i++)
            {
                images[i].Source = new BitmapImage(new Uri(@"/Assets/device_working.png", UriKind.Relative));
            }

            SqlCommand commandDevices = new SqlCommand("SELECT * FROM dbo.getDevices()", localDbConnectionUnity);
            SqlDataAdapter dataAdapterDevices = new SqlDataAdapter(commandDevices);
            DataTable dataTableDevices = new DataTable();
            dataAdapterDevices.Fill(dataTableDevices);

            for (int i = 0; i < Properties.Settings.Default.deviceAmount; i++)
            {
                devices[i].Text = dataTableDevices.Rows[i][0].ToString();
                operators[i].Text = dataTableDevices.Rows[i][1].ToString();
                type[i] = int.Parse(dataTableDevices.Rows[i][2].ToString());
                cards[i].Visibility = Visibility.Visible;
            }

            setParity(Properties.Settings.Default.deviceParity);
            setStopBit(Properties.Settings.Default.deviceStopBit);
            setBaudRate(Properties.Settings.Default.deviceBaudRate);
            writingCheck.IsChecked = Properties.Settings.Default.writing;
        }

        private void devicesSettings1(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(0);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings2(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(1);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings3(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(2);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings4(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(3);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings5(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(4);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings6(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(5);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings7(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(6);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings8(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(7);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings9(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(8);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings10(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(9);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings11(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(10);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings12(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(11);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings13(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(12);
            settingsDeviceSettings.ShowDialog();
        }

        private void devicesSettings14(object sender, RoutedEventArgs e)
        {
            SettingsDeviceSettings settingsDeviceSettings = new SettingsDeviceSettings(13);
            settingsDeviceSettings.ShowDialog();
        }

        private void save(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.deviceParity = parity.Text;
            Properties.Settings.Default.deviceStopBit = stopBit.Text;
            Properties.Settings.Default.deviceBaudRate = int.Parse(baudRate.Text);
            Properties.Settings.Default.writing = (bool)writingCheck.IsChecked;
            Properties.Settings.Default.Save();
            myMessageQueue.Enqueue(rm.GetString("connectionSettingsSaved"), rm.GetString("ok"), () => HandleOKMethod());
        }

        private void HandleOKMethod()
        {
            
        }

        private void setParity(string parityString)
        {
            if (parityString == "Even")
                parity.SelectedIndex = 0;
            if (parityString == "Mark")
                parity.SelectedIndex = 1;
            if (parityString == "None")
                parity.SelectedIndex = 2;
            if (parityString == "Odd")
                parity.SelectedIndex = 3;
            if (parityString == "Space")
                parity.SelectedIndex = 4;
        }

        private void setStopBit(string stopBitString)
        {
            if (stopBitString == "None")
                stopBit.SelectedIndex = 0;
            if (stopBitString == "1")
                stopBit.SelectedIndex = 1;
            if (stopBitString == "1.5")
                stopBit.SelectedIndex = 2;
            if (stopBitString == "2")
                stopBit.SelectedIndex = 3;
        }

        private void setBaudRate(int deviceBaudRate)
        {
            if (deviceBaudRate == 2400)
                baudRate.SelectedIndex = 0;
            if (deviceBaudRate == 4800)
                baudRate.SelectedIndex = 1;
            if (deviceBaudRate == 9600)
                baudRate.SelectedIndex = 2;
            if (deviceBaudRate == 19200)
                baudRate.SelectedIndex = 3;
            if (deviceBaudRate == 38400)
                baudRate.SelectedIndex = 4;
            if (deviceBaudRate == 57600)
                baudRate.SelectedIndex = 5;
            if (deviceBaudRate == 115200)
                baudRate.SelectedIndex = 6;
        }
    }
}
