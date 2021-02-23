using MaterialDesignThemes.Wpf;
using Microsoft.Ajax.Utilities;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Resources;
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

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for Devices.xaml
    /// </summary>
    public partial class Devices : UserControl
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SerialPort connection;
        IModbusSerialMaster modbus;
        int[] type = new int[14];
        List<string> databases = new List<string>();

        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        BackgroundWorker backgroundWorker2 = new BackgroundWorker();
        BackgroundWorker backgroundWorker3 = new BackgroundWorker();
        BackgroundWorker backgroundWorker4 = new BackgroundWorker();
        BackgroundWorker backgroundWorker5 = new BackgroundWorker();
        BackgroundWorker backgroundWorker6 = new BackgroundWorker();
        BackgroundWorker backgroundWorker7 = new BackgroundWorker();
        BackgroundWorker backgroundWorker8 = new BackgroundWorker();
        BackgroundWorker backgroundWorker9 = new BackgroundWorker();
        BackgroundWorker backgroundWorker10 = new BackgroundWorker();
        BackgroundWorker backgroundWorker11 = new BackgroundWorker();
        BackgroundWorker backgroundWorker12 = new BackgroundWorker();
        BackgroundWorker backgroundWorker13 = new BackgroundWorker();
        BackgroundWorker backgroundWorker14 = new BackgroundWorker();

        public Devices()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");

            if (VarsDevices.Port == null)
            {
                connection = new SerialPort();
                connectionSettings();
            }
            else
                connection = VarsDevices.Port;
            init();


            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker4.WorkerSupportsCancellation = true;
            backgroundWorker5.WorkerSupportsCancellation = true;
            backgroundWorker6.WorkerSupportsCancellation = true;
            backgroundWorker7.WorkerSupportsCancellation = true;
            backgroundWorker8.WorkerSupportsCancellation = true;
            backgroundWorker9.WorkerSupportsCancellation = true;
            backgroundWorker10.WorkerSupportsCancellation = true;
            backgroundWorker11.WorkerSupportsCancellation = true;
            backgroundWorker12.WorkerSupportsCancellation = true;
            backgroundWorker13.WorkerSupportsCancellation = true;
            backgroundWorker14.WorkerSupportsCancellation = true;

            backgroundWorker1.DoWork += work1;
            backgroundWorker2.DoWork += work2;
            backgroundWorker3.DoWork += work3;
            backgroundWorker4.DoWork += work4;
            backgroundWorker5.DoWork += work5;
            backgroundWorker6.DoWork += work6;
            backgroundWorker7.DoWork += work7;
            backgroundWorker8.DoWork += work8;
            backgroundWorker9.DoWork += work9;
            backgroundWorker10.DoWork += work10;
            backgroundWorker11.DoWork += work11;
            backgroundWorker12.DoWork += work12;
            backgroundWorker13.DoWork += work13;
            backgroundWorker14.DoWork += work14;
        }

        private void init()
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();
            if (localDbConnectionUnity.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();

            deviceAmount.Content = Properties.Settings.Default.deviceAmount.ToString();
            deviceWorking.Content = VarsDevices.DeviceWorking.ToString();
            deviceProjectReady.Content = VarsDevices.DeviceProjectReady.ToString();

            TextBlock[] operators = { operator1Text, operator2Text, operator3Text, operator4Text, operator5Text, operator6Text,
            operator7Text, operator8Text, operator9Text, operator10Text, operator11Text, operator12Text, operator13Text, operator14Text };

            TextBlock[] devices = { device1Text, device2Text, device3Text, device4Text, device5Text, device6Text,
            device7Text, device8Text, device9Text, device10Text, device11Text, device12Text, device13Text, device14Text };

            Card[] cards = { device1Card, device2Card, device3Card, device4Card, device5Card, device6Card,
            device7Card, device8Card, device9Card, device10Card, device11Card, device12Card, device13Card, device14Card };

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

            VarsDevices.device1Name = device1Text.Text;
            VarsDevices.device2Name = device2Text.Text;
            VarsDevices.device3Name = device3Text.Text;
            VarsDevices.device4Name = device4Text.Text;
            VarsDevices.device5Name = device5Text.Text;
            VarsDevices.device6Name = device6Text.Text;
            VarsDevices.device7Name = device7Text.Text;
            VarsDevices.device8Name = device8Text.Text;
            VarsDevices.device9Name = device9Text.Text;
            VarsDevices.device10Name = device10Text.Text;
            VarsDevices.device11Name = device11Text.Text;
            VarsDevices.device12Name = device12Text.Text;
            VarsDevices.device13Name = device13Text.Text;
            VarsDevices.device14Name = device14Text.Text;

            string[] vars = {VarsDevices.device1DbName, VarsDevices.device2DbName,
            VarsDevices.device3DbName, VarsDevices.device4DbName, VarsDevices.device5DbName,
            VarsDevices.device6DbName, VarsDevices.device7DbName, VarsDevices.device8DbName,
            VarsDevices.device9DbName, VarsDevices.device10DbName, VarsDevices.device11DbName,
            VarsDevices.device12DbName, VarsDevices.device13DbName, VarsDevices.device14DbName };

            Image[] images = {image1, image2, image3, image4, image5, image6, image7, image8,
            image9, image10, image11, image12, image13, image14};

            int readyDevices = 0;

            for (int i = 0; i < vars.Length; i++)
            {
                if (vars[i] != null)
                {
                    images[i].Source = new BitmapImage(new Uri(@"Assets/device_ready.png", UriKind.Relative));
                    images[i].IsEnabled = true;
                    readyDevices++;
                }
                else
                {
                    images[i].Source = new BitmapImage(new Uri(@"Assets/device_default.png", UriKind.Relative));
                    images[i].IsEnabled = false;
                }
            }

            deviceProjectReady.Content = readyDevices.ToString();

            Button[] buttons = { projectOpen1, projectOpen2, projectOpen3, projectOpen4, projectOpen5,
            projectOpen6, projectOpen7, projectOpen8, projectOpen9, projectOpen10, projectOpen11,
                projectOpen12, projectOpen13, projectOpen14 };

            Button[] settingsButtons = { settings1, settings2, settings3, settings4, settings5,
            settings6, settings7, settings8, settings9, settings10, settings11,
                settings12, settings13, settings14 };

            bool[] varsForWork = {VarsDevices.device1Connected, VarsDevices.device2Connected,
            VarsDevices.device3Connected, VarsDevices.device4Connected, VarsDevices.device5Connected,
            VarsDevices.device6Connected, VarsDevices.device7Connected, VarsDevices.device8Connected,
            VarsDevices.device9Connected, VarsDevices.device10Connected, VarsDevices.device11Connected,
            VarsDevices.device12Connected, VarsDevices.device13Connected, VarsDevices.device14Connected };

            int runningDevices = 0;

            for (int i = 0; i < varsForWork.Length; i++)
            {
                if (varsForWork[i] == true)
                {
                    cards[i].Background = Brushes.LightGreen;
                    buttons[i].IsEnabled = false;
                    settingsButtons[i].IsEnabled = false;
                    runningDevices++;
                }
                else
                {
                    buttons[i].IsEnabled = true;
                    settingsButtons[i].IsEnabled = true;
                    cards[i].Background = Brushes.White;
                }
            }

            deviceWorking.Content = runningDevices.ToString();

            if (runningDevices < 0)
            {
                localDbConnection.Close();
                localDbConnectionUnity.Close();
            }
        }

        private void connectionSettings()
        {
            if (VarsDevices.Port == null)
            {
                string[] ports = SerialPort.GetPortNames();
                try
                {
                    connection.PortName = ports[0];
                    connection.BaudRate = Properties.Settings.Default.deviceBaudRate;
                    connection.Parity = setParity(Properties.Settings.Default.deviceParity);
                    connection.DataBits = 8;
                    connection.StopBits = setStopBit(Properties.Settings.Default.deviceStopBit);
                    connection.ReadTimeout = 300;
                    modbus = ModbusSerialMaster.CreateRtu(connection);
                    VarsDevices.Port = connection;
                    VarsDevices.Master = modbus;
                }
                catch { }
            }
        }

        private StopBits setStopBit(string deviceStopBit)
        {
            switch (deviceStopBit)
            {
                case "None":
                    return StopBits.None;
                case "1.5":
                    return StopBits.OnePointFive;
                case "2":
                    return StopBits.Two;
                default:
                    return StopBits.One;
            }
        }

        private Parity setParity(string deviceParity)
        {
            switch (deviceParity)
            {
                case "Even":
                    return Parity.Even;
                case "Mark":
                    return Parity.Even;
                case "Odd":
                    return Parity.Even;
                case "Space":
                    return Parity.Even;
                default:
                    return Parity.None;
            }
        }

        private void devicesSettings1(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device1Text.Text, operator1Text.Text, 1);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings2(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device2Text.Text, operator2Text.Text, 2);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings3(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device3Text.Text, operator3Text.Text, 3);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings4(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device4Text.Text, operator4Text.Text, 4);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings5(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device5Text.Text, operator5Text.Text, 5);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings6(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device6Text.Text, operator6Text.Text, 6);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings7(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device7Text.Text, operator7Text.Text, 7);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings8(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device8Text.Text, operator8Text.Text, 8);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings9(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device9Text.Text, operator9Text.Text, 9);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings10(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device10Text.Text, operator10Text.Text, 10);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings11(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device11Text.Text, operator11Text.Text, 11);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings12(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device12Text.Text, operator12Text.Text, 12);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings13(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device13Text.Text, operator13Text.Text, 13);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void devicesSettings14(object sender, RoutedEventArgs e)
        {
            DevicesSettings devicesSettings = new DevicesSettings(device14Text.Text, operator14Text.Text, 14);
            devicesSettings.Closed += Closed;
            devicesSettings.ShowDialog();
        }

        private void projectOpen_1(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator1Text.Text, "0", getDeviceType(1));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_2(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator2Text.Text, "1", getDeviceType(2));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_3(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator3Text.Text, "2", getDeviceType(3));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_4(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator4Text.Text, "3", getDeviceType(4));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_5(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator5Text.Text, "4", getDeviceType(5));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_6(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator6Text.Text, "5", getDeviceType(6));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_7(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator7Text.Text, "6", getDeviceType(7));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_8(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator8Text.Text, "7", getDeviceType(8));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_9(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator9Text.Text, "8", getDeviceType(9));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();

        }

        private void projectOpen_10(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator10Text.Text, "9", getDeviceType(10));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_11(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator11Text.Text, "10", getDeviceType(11));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_12(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator12Text.Text, "11", getDeviceType(12));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_13(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator13Text.Text, "12", getDeviceType(13));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void projectOpen_14(object sender, RoutedEventArgs e)
        {
            ProjectOpen projectOpen = new ProjectOpen(operator14Text.Text, "13", getDeviceType(14));
            projectOpen.Closed += Closed;
            projectOpen.ShowDialog();
        }

        private void Closed(object sender, EventArgs e)
        {
            init();
        }

        private void openDeviceWork(string deviceDbName, int id, string deviceName)
        {
            if (getDeviceType(id) == 1)
            {
                DeviceWork deviceWork = new DeviceWork(deviceDbName, id, deviceName);
                deviceWork.Closed += Closed;
                deviceWork.Show();
            }
            else
            {
                DeviceWork2 deviceWork = new DeviceWork2(deviceDbName, id, deviceName);
                deviceWork.Closed += Closed;
                deviceWork.ShowDialog();
            }
        }

        private void device1_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device1DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device1Connected)
                {
                    startConnectingDevice1();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device1DbName, 1, device1Text.Text);
                }
            }
        }

        private void device2_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device2DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device2Connected)
                {
                    startConnectingDevice2();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device2DbName, 2, device2Text.Text);
                }
            }
        }

        private void device3_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device3DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device3Connected)
                {
                    startConnectingDevice3();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device3DbName, 3, device3Text.Text);
                }
            }
        }

        private void device4_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device4DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device4Connected)
                {
                    startConnectingDevice4();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device4DbName, 4, device4Text.Text);
                }
            }
        }

        private void device5_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device5DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device5Connected)
                {
                    startConnectingDevice5();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device5DbName, 5, device5Text.Text);
                }
            }
        }

        private void device6_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device6DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device6Connected)
                {
                    startConnectingDevice6();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device6DbName, 6, device6Text.Text);
                }
            }
        }

        private void device7_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device7DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device7Connected)
                {
                    startConnectingDevice7();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device7DbName, 7, device7Text.Text);
                }
            }
        }

        private void device8_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device8DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device8Connected)
                {
                    startConnectingDevice8();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device8DbName, 8, device8Text.Text);
                }
            }
        }

        private void device9_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device9DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device9Connected)
                {
                    startConnectingDevice9();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device9DbName, 9, device9Text.Text);
                }
            }
        }

        private void device10_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device10DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device10Connected)
                {
                    startConnectingDevice10();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device10DbName, 10, device10Text.Text);
                }
            }
        }

        private void device11_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device11DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device11Connected)
                {
                    startConnectingDevice11();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device11DbName, 11, device11Text.Text);
                }
            }
        }

        private void device12_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device12DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device12Connected)
                {
                    startConnectingDevice12();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device12DbName, 12, device12Text.Text);
                }
            }
        }

        private void device13_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device13DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device13Connected)
                {
                    startConnectingDevice13();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device13DbName, 13, device13Text.Text);
                }
            }
        }

        private void device14_Click(object sender, MouseButtonEventArgs e)
        {
            if (VarsDevices.device14DbName != null)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                if (!VarsDevices.Device14Connected)
                {
                    startConnectingDevice14();
                    deviceWorking.Content = VarsDevices.deviceWorking.ToString();
                }
                else
                {
                    openDeviceWork(VarsDevices.device14DbName, 14, device14Text.Text);
                }
            }
        }

        private void connect()
        {
            if (!connection.IsOpen)
                connection.Open();
        }

        private void startConnectingDevice1()
        {
            try
            {
                connect();
                backgroundWorker1.RunWorkerAsync();
                VarsDevices.device1Connected = true;
                projectOpen1.IsEnabled = false;
                settings1.IsEnabled = false;
                device1Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice2()
        {
            try
            {
                connect();
                backgroundWorker2.RunWorkerAsync();
                VarsDevices.device2Connected = true;
                projectOpen2.IsEnabled = false;
                settings2.IsEnabled = false;
                device2Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice3()
        {
            try
            {
                connect();
                backgroundWorker3.RunWorkerAsync();
                VarsDevices.device3Connected = true;
                projectOpen3.IsEnabled = false;
                settings3.IsEnabled = false;
                device3Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice4()
        {
            try
            {
                connect();
                backgroundWorker4.RunWorkerAsync();
                VarsDevices.device4Connected = true;
                projectOpen4.IsEnabled = false;
                settings4.IsEnabled = false;
                device4Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice5()
        {
            try
            {
                connect();
                backgroundWorker5.RunWorkerAsync();
                VarsDevices.device5Connected = true;
                projectOpen5.IsEnabled = false;
                settings5.IsEnabled = false;
                device5Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice6()
        {
            try
            {
                connect();
                backgroundWorker6.RunWorkerAsync();
                VarsDevices.device6Connected = true;
                projectOpen6.IsEnabled = false;
                settings6.IsEnabled = false;
                device6Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice7()
        {
            try
            {
                connect();
                backgroundWorker7.RunWorkerAsync();
                VarsDevices.device7Connected = true;
                projectOpen7.IsEnabled = false;
                settings7.IsEnabled = false;
                device7Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice8()
        {
            try
            {
                connect();
                backgroundWorker8.RunWorkerAsync();
                VarsDevices.device8Connected = true;
                projectOpen8.IsEnabled = false;
                settings8.IsEnabled = false;
                device8Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice9()
        {
            try
            {
                connect();
                backgroundWorker9.RunWorkerAsync();
                VarsDevices.device9Connected = true;
                projectOpen9.IsEnabled = false;
                settings9.IsEnabled = false;
                device9Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice10()
        {
            try
            {
                connect();
                backgroundWorker10.RunWorkerAsync();
                VarsDevices.device10Connected = true;
                projectOpen10.IsEnabled = false;
                settings10.IsEnabled = false;
                device10Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice11()
        {
            try
            {
                connect();
                backgroundWorker11.RunWorkerAsync();
                VarsDevices.device11Connected = true;
                projectOpen11.IsEnabled = false;
                settings11.IsEnabled = false;
                device11Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice12()
        {
            try
            {
                connect();
                backgroundWorker12.RunWorkerAsync();
                VarsDevices.device12Connected = true;
                projectOpen12.IsEnabled = false;
                settings12.IsEnabled = false;
                device12Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice13()
        {
            try
            {
                connect();
                backgroundWorker13.RunWorkerAsync();
                VarsDevices.device13Connected = true;
                projectOpen13.IsEnabled = false;
                settings13.IsEnabled = false;
                device13Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void startConnectingDevice14()
        {
            try
            {
                connect();
                backgroundWorker14.RunWorkerAsync();
                VarsDevices.device14Connected = true;
                projectOpen14.IsEnabled = false;
                settings14.IsEnabled = false;
                device14Card.Background = Brushes.LightGreen;
                VarsDevices.deviceWorking++;
            }
            catch (IOException)
            {
                MessageBox.Show(rm.GetString("noPort", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch { }
        }

        private void stop(Exception ex)
        {
            VarsDevices.deviceWorking -= 1;
            if (VarsDevices.deviceWorking < 1)
            {
                VarsDevices.deviceWorking = 0;
                closeConnections();
            }
            MessageBox.Show(ex.Message);
            init();
        }


        private void work1(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
                try
                {
                    insertIntoDb(Properties.Settings.Default.device1Slave, VarsDevices.device1DbName, type[0]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device1Connected = false;
                        projectOpen1.IsEnabled = true;
                        settings1.IsEnabled = true;
                        device1Card.Background = Brushes.IndianRed;
                        backgroundWorker1.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work2(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker2.CancellationPending)
            {
                try
                {
                    insertIntoDb2(Properties.Settings.Default.device2Slave, VarsDevices.device2DbName, type[1]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device2Connected = false;
                        projectOpen2.IsEnabled = true;
                        settings2.IsEnabled = true;
                        settings2.IsEnabled = true;
                        device2Card.Background = Brushes.IndianRed;
                        backgroundWorker2.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work3(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker3.CancellationPending)
            {
                try
                {
                    insertIntoDb3(Properties.Settings.Default.device3Slave, VarsDevices.device3DbName, type[2]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device3Connected = false;
                        projectOpen3.IsEnabled = true;
                        settings3.IsEnabled = true;
                        device3Card.Background = Brushes.IndianRed;
                        backgroundWorker3.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }


        private void work4(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker4.CancellationPending)
            {
                try
                {
                    insertIntoDb4(Properties.Settings.Default.device4Slave, VarsDevices.device4DbName, type[3]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device4Connected = false;
                        projectOpen4.IsEnabled = true;
                        settings4.IsEnabled = true;
                        device4Card.Background = Brushes.IndianRed;
                        backgroundWorker4.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work5(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker5.CancellationPending)
            {
                try
                {
                    insertIntoDb5(Properties.Settings.Default.device5Slave, VarsDevices.device5DbName, type[4]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device5Connected = false;
                        projectOpen5.IsEnabled = true;
                        settings5.IsEnabled = true;
                        device5Card.Background = Brushes.IndianRed;
                        backgroundWorker5.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work6(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker6.CancellationPending)
            {
                try
                {
                    insertIntoDb6(Properties.Settings.Default.device6Slave, VarsDevices.device6DbName, type[5]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device6Connected = false;
                        projectOpen6.IsEnabled = true;
                        settings6.IsEnabled = true;
                        device6Card.Background = Brushes.IndianRed;
                        backgroundWorker6.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work7(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker7.CancellationPending)
            {
                try
                {
                    insertIntoDb7(Properties.Settings.Default.device7Slave, VarsDevices.device7DbName, type[6]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device7Connected = false;
                        projectOpen7.IsEnabled = true;
                        settings7.IsEnabled = true;
                        device7Card.Background = Brushes.IndianRed;
                        backgroundWorker7.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work8(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker8.CancellationPending)
            {
                try
                {
                    insertIntoDb8(Properties.Settings.Default.device8Slave, VarsDevices.device8DbName, type[7]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device8Connected = false;
                        projectOpen8.IsEnabled = true;
                        settings8.IsEnabled = true;
                        device8Card.Background = Brushes.IndianRed;
                        backgroundWorker8.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work9(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker9.CancellationPending)
            {
                try
                {
                    insertIntoDb9(Properties.Settings.Default.device9Slave, VarsDevices.device9DbName, type[8]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device9Connected = false;
                        projectOpen9.IsEnabled = true;
                        settings9.IsEnabled = true;
                        device9Card.Background = Brushes.IndianRed;
                        backgroundWorker9.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work10(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker10.CancellationPending)
            {
                try
                {
                    insertIntoDb10(Properties.Settings.Default.device10Slave, VarsDevices.device10DbName, type[9]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device10Connected = false;
                        projectOpen10.IsEnabled = true;
                        settings10.IsEnabled = true;
                        device10Card.Background = Brushes.IndianRed;
                        backgroundWorker10.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work11(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker11.CancellationPending)
            {
                try
                {
                    insertIntoDb11(Properties.Settings.Default.device11Slave, VarsDevices.device11DbName, type[10]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device11Connected = false;
                        projectOpen11.IsEnabled = true;
                        settings11.IsEnabled = true;
                        device11Card.Background = Brushes.IndianRed;
                        backgroundWorker11.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work12(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker12.CancellationPending)
            {
                try
                {
                    insertIntoDb12(Properties.Settings.Default.device12Slave, VarsDevices.device12DbName, type[11]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device12Connected = false;
                        projectOpen12.IsEnabled = true;
                        settings12.IsEnabled = true;
                        device12Card.Background = Brushes.IndianRed;
                        backgroundWorker12.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work13(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker13.CancellationPending)
            {
                try
                {
                    insertIntoDb13(Properties.Settings.Default.device13Slave, VarsDevices.device13DbName, type[12]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device13Connected = false;
                        projectOpen13.IsEnabled = true;
                        settings13.IsEnabled = true;
                        device13Card.Background = Brushes.IndianRed;
                        backgroundWorker13.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void work14(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker14.CancellationPending)
            {
                try
                {
                    insertIntoDb14(Properties.Settings.Default.device14Slave, VarsDevices.device14DbName, type[13]);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        VarsDevices.device14Connected = false;
                        projectOpen14.IsEnabled = true;
                        settings14.IsEnabled = true;
                        device14Card.Background = Brushes.IndianRed;
                        backgroundWorker14.CancelAsync();
                        stop(ex);
                    }));
                }
            }
        }

        private void setDataDevice1(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice1.Diameter = diameter;
            VariablesForDevice1.DiameterSet = diameterSet;
            VariablesForDevice1.DiameterDifference = diameterDifference;
            VariablesForDevice1.PlusTolerance = plusTolerance;
            VariablesForDevice1.MinusTolerance = minusTolerance;
            VariablesForDevice1.Length = length;
            VariablesForDevice1.Display3 = display3;
            VariablesForDevice1.Nc = nc;
            VariablesForDevice1.Even = even;
            VariablesForDevice1.Parp = parp;
            VariablesForDevice1.Pari = pari;

            VariablesForDevice1.Diameter2 = diameter2;
            VariablesForDevice1.PlusTolerance2 = plusTolerance2;
            VariablesForDevice1.MinusTolerance2 = minusTolerance2;
            VariablesForDevice1.XAxis = xAxis;
            VariablesForDevice1.YAxis = yAxis;

        }

        private void setDataDevice2(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice2.Diameter = diameter;
            VariablesForDevice2.DiameterSet = diameterSet;
            VariablesForDevice2.DiameterDifference = diameterDifference;
            VariablesForDevice2.PlusTolerance = plusTolerance;
            VariablesForDevice2.MinusTolerance = minusTolerance;
            VariablesForDevice2.Length = length;
            VariablesForDevice2.Display3 = display3;
            VariablesForDevice2.Nc = nc;
            VariablesForDevice2.Even = even;
            VariablesForDevice2.Parp = parp;
            VariablesForDevice2.Pari = pari;

            VariablesForDevice2.Diameter2 = diameter2;
            VariablesForDevice2.PlusTolerance2 = plusTolerance2;
            VariablesForDevice2.MinusTolerance2 = minusTolerance2;
            VariablesForDevice2.XAxis = xAxis;
            VariablesForDevice2.YAxis = yAxis;
        }

        private void setDataDevice3(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice3.Diameter = diameter;
            VariablesForDevice3.DiameterSet = diameterSet;
            VariablesForDevice3.DiameterDifference = diameterDifference;
            VariablesForDevice3.PlusTolerance = plusTolerance;
            VariablesForDevice3.MinusTolerance = minusTolerance;
            VariablesForDevice3.Length = length;
            VariablesForDevice3.Display3 = display3;
            VariablesForDevice3.Nc = nc;
            VariablesForDevice3.Even = even;
            VariablesForDevice3.Parp = parp;
            VariablesForDevice3.Pari = pari;

            VariablesForDevice3.Diameter2 = diameter2;
            VariablesForDevice3.PlusTolerance2 = plusTolerance2;
            VariablesForDevice3.MinusTolerance2 = minusTolerance2;
            VariablesForDevice3.XAxis = xAxis;
            VariablesForDevice3.YAxis = yAxis;
        }

        private void setDataDevice4(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice4.Diameter = diameter;
            VariablesForDevice4.DiameterSet = diameterSet;
            VariablesForDevice4.DiameterDifference = diameterDifference;
            VariablesForDevice4.PlusTolerance = plusTolerance;
            VariablesForDevice4.MinusTolerance = minusTolerance;
            VariablesForDevice4.Length = length;
            VariablesForDevice4.Display3 = display3;
            VariablesForDevice4.Nc = nc;
            VariablesForDevice4.Even = even;
            VariablesForDevice4.Parp = parp;
            VariablesForDevice4.Pari = pari;

            VariablesForDevice4.Diameter2 = diameter2;
            VariablesForDevice4.PlusTolerance2 = plusTolerance2;
            VariablesForDevice4.MinusTolerance2 = minusTolerance2;
            VariablesForDevice4.XAxis = xAxis;
            VariablesForDevice4.YAxis = yAxis;
        }

        private void setDataDevice5(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice5.Diameter = diameter;
            VariablesForDevice5.DiameterSet = diameterSet;
            VariablesForDevice5.DiameterDifference = diameterDifference;
            VariablesForDevice5.PlusTolerance = plusTolerance;
            VariablesForDevice5.MinusTolerance = minusTolerance;
            VariablesForDevice5.Length = length;
            VariablesForDevice5.Display3 = display3;
            VariablesForDevice5.Nc = nc;
            VariablesForDevice5.Even = even;
            VariablesForDevice5.Parp = parp;
            VariablesForDevice5.Pari = pari;

            VariablesForDevice5.Diameter2 = diameter2;
            VariablesForDevice5.PlusTolerance2 = plusTolerance2;
            VariablesForDevice5.MinusTolerance2 = minusTolerance2;
            VariablesForDevice5.XAxis = xAxis;
            VariablesForDevice5.YAxis = yAxis;
        }

        private void setDataDevice6(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice6.Diameter = diameter;
            VariablesForDevice6.DiameterSet = diameterSet;
            VariablesForDevice6.DiameterDifference = diameterDifference;
            VariablesForDevice6.PlusTolerance = plusTolerance;
            VariablesForDevice6.MinusTolerance = minusTolerance;
            VariablesForDevice6.Length = length;
            VariablesForDevice6.Display3 = display3;
            VariablesForDevice6.Nc = nc;
            VariablesForDevice6.Even = even;
            VariablesForDevice6.Parp = parp;
            VariablesForDevice6.Pari = pari;

            VariablesForDevice6.Diameter2 = diameter2;
            VariablesForDevice6.PlusTolerance2 = plusTolerance2;
            VariablesForDevice6.MinusTolerance2 = minusTolerance2;
            VariablesForDevice6.XAxis = xAxis;
            VariablesForDevice6.YAxis = yAxis;
        }

        private void setDataDevice7(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice7.Diameter = diameter;
            VariablesForDevice7.DiameterSet = diameterSet;
            VariablesForDevice7.DiameterDifference = diameterDifference;
            VariablesForDevice7.PlusTolerance = plusTolerance;
            VariablesForDevice7.MinusTolerance = minusTolerance;
            VariablesForDevice7.Length = length;
            VariablesForDevice7.Display3 = display3;
            VariablesForDevice7.Nc = nc;
            VariablesForDevice7.Even = even;
            VariablesForDevice7.Parp = parp;
            VariablesForDevice7.Pari = pari;

            VariablesForDevice7.Diameter2 = diameter2;
            VariablesForDevice7.PlusTolerance2 = plusTolerance2;
            VariablesForDevice7.MinusTolerance2 = minusTolerance2;
            VariablesForDevice7.XAxis = xAxis;
            VariablesForDevice7.YAxis = yAxis;
        }

        private void setDataDevice8(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice8.Diameter = diameter;
            VariablesForDevice8.DiameterSet = diameterSet;
            VariablesForDevice8.DiameterDifference = diameterDifference;
            VariablesForDevice8.PlusTolerance = plusTolerance;
            VariablesForDevice8.MinusTolerance = minusTolerance;
            VariablesForDevice8.Length = length;
            VariablesForDevice8.Display3 = display3;
            VariablesForDevice8.Nc = nc;
            VariablesForDevice8.Even = even;
            VariablesForDevice8.Parp = parp;
            VariablesForDevice8.Pari = pari;

            VariablesForDevice8.Diameter2 = diameter2;
            VariablesForDevice8.PlusTolerance2 = plusTolerance2;
            VariablesForDevice8.MinusTolerance2 = minusTolerance2;
            VariablesForDevice8.XAxis = xAxis;
            VariablesForDevice8.YAxis = yAxis;
        }

        private void setDataDevice9(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice9.Diameter = diameter;
            VariablesForDevice9.DiameterSet = diameterSet;
            VariablesForDevice9.DiameterDifference = diameterDifference;
            VariablesForDevice9.PlusTolerance = plusTolerance;
            VariablesForDevice9.MinusTolerance = minusTolerance;
            VariablesForDevice9.Length = length;
            VariablesForDevice9.Display3 = display3;
            VariablesForDevice9.Nc = nc;
            VariablesForDevice9.Even = even;
            VariablesForDevice9.Parp = parp;
            VariablesForDevice9.Pari = pari;

            VariablesForDevice9.Diameter2 = diameter2;
            VariablesForDevice9.PlusTolerance2 = plusTolerance2;
            VariablesForDevice9.MinusTolerance2 = minusTolerance2;
            VariablesForDevice9.XAxis = xAxis;
            VariablesForDevice9.YAxis = yAxis;
        }

        private void setDataDevice10(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice10.Diameter = diameter;
            VariablesForDevice10.DiameterSet = diameterSet;
            VariablesForDevice10.DiameterDifference = diameterDifference;
            VariablesForDevice10.PlusTolerance = plusTolerance;
            VariablesForDevice10.MinusTolerance = minusTolerance;
            VariablesForDevice10.Length = length;
            VariablesForDevice10.Display3 = display3;
            VariablesForDevice10.Nc = nc;
            VariablesForDevice10.Even = even;
            VariablesForDevice10.Parp = parp;
            VariablesForDevice10.Pari = pari;

            VariablesForDevice10.Diameter2 = diameter2;
            VariablesForDevice10.PlusTolerance2 = plusTolerance2;
            VariablesForDevice10.MinusTolerance2 = minusTolerance2;
            VariablesForDevice10.XAxis = xAxis;
            VariablesForDevice10.YAxis = yAxis;
        }

        private void setDataDevice11(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice11.Diameter = diameter;
            VariablesForDevice11.DiameterSet = diameterSet;
            VariablesForDevice11.DiameterDifference = diameterDifference;
            VariablesForDevice11.PlusTolerance = plusTolerance;
            VariablesForDevice11.MinusTolerance = minusTolerance;
            VariablesForDevice11.Length = length;
            VariablesForDevice11.Display3 = display3;
            VariablesForDevice11.Nc = nc;
            VariablesForDevice11.Even = even;
            VariablesForDevice11.Parp = parp;
            VariablesForDevice11.Pari = pari;

            VariablesForDevice11.Diameter2 = diameter2;
            VariablesForDevice11.PlusTolerance2 = plusTolerance2;
            VariablesForDevice11.MinusTolerance2 = minusTolerance2;
            VariablesForDevice11.XAxis = xAxis;
            VariablesForDevice11.YAxis = yAxis;
        }

        private void setDataDevice12(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice12.Diameter = diameter;
            VariablesForDevice12.DiameterSet = diameterSet;
            VariablesForDevice12.DiameterDifference = diameterDifference;
            VariablesForDevice12.PlusTolerance = plusTolerance;
            VariablesForDevice12.MinusTolerance = minusTolerance;
            VariablesForDevice12.Length = length;
            VariablesForDevice12.Display3 = display3;
            VariablesForDevice12.Nc = nc;
            VariablesForDevice12.Even = even;
            VariablesForDevice12.Parp = parp;
            VariablesForDevice12.Pari = pari;

            VariablesForDevice12.Diameter2 = diameter2;
            VariablesForDevice12.PlusTolerance2 = plusTolerance2;
            VariablesForDevice12.MinusTolerance2 = minusTolerance2;
            VariablesForDevice12.XAxis = xAxis;
            VariablesForDevice12.YAxis = yAxis;
        }

        private void setDataDevice13(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice13.Diameter = diameter;
            VariablesForDevice13.DiameterSet = diameterSet;
            VariablesForDevice13.DiameterDifference = diameterDifference;
            VariablesForDevice13.PlusTolerance = plusTolerance;
            VariablesForDevice13.MinusTolerance = minusTolerance;
            VariablesForDevice13.Length = length;
            VariablesForDevice13.Display3 = display3;
            VariablesForDevice13.Nc = nc;
            VariablesForDevice13.Even = even;
            VariablesForDevice13.Parp = parp;
            VariablesForDevice13.Pari = pari;

            VariablesForDevice13.Diameter2 = diameter2;
            VariablesForDevice13.PlusTolerance2 = plusTolerance2;
            VariablesForDevice13.MinusTolerance2 = minusTolerance2;
            VariablesForDevice13.XAxis = xAxis;
            VariablesForDevice13.YAxis = yAxis;
        }

        private void setDataDevice14(double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double length, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {
            VariablesForDevice14.Diameter = diameter;
            VariablesForDevice14.DiameterSet = diameterSet;
            VariablesForDevice14.DiameterDifference = diameterDifference;
            VariablesForDevice14.PlusTolerance = plusTolerance;
            VariablesForDevice14.MinusTolerance = minusTolerance;
            VariablesForDevice14.Length = length;
            VariablesForDevice14.Display3 = display3;
            VariablesForDevice14.Nc = nc;
            VariablesForDevice14.Even = even;
            VariablesForDevice14.Parp = parp;
            VariablesForDevice14.Pari = pari;

            VariablesForDevice14.Diameter2 = diameter2;
            VariablesForDevice14.PlusTolerance2 = plusTolerance2;
            VariablesForDevice14.MinusTolerance2 = minusTolerance2;
            VariablesForDevice14.XAxis = xAxis;
            VariablesForDevice14.YAxis = yAxis;
        }

        private void write1(string dbName, double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double metraj, double display3, double nc, double even, double parp, double pari)
        {
            string saveIntoDatabase = String.Format("INSERT INTO {0} (Cap, CapSet, CapFark, ArtiTolerans, EksiTolerans, Metraj, Display3, NC, Even, Par_p, Par_i, Tarih) " +
                        " VALUES (@Diameter, @DiameterSet, @DiameterDifference, @PlusTolerance, @MinusTolerance, @Metraj, @Display3, @Nc, @Even, @Parp, @Pari, @DateTimes);", dbName);

            SqlCommand saveLogsCommand = new SqlCommand(saveIntoDatabase, localDbConnection);

            saveLogsCommand.Parameters.AddWithValue("@Diameter", diameter);
            saveLogsCommand.Parameters.AddWithValue("@DiameterSet", diameterSet);
            saveLogsCommand.Parameters.AddWithValue("@DiameterDifference", diameterDifference);
            saveLogsCommand.Parameters.AddWithValue("@PlusTolerance", plusTolerance);
            saveLogsCommand.Parameters.AddWithValue("@MinusTolerance", minusTolerance);
            saveLogsCommand.Parameters.AddWithValue("@Metraj", metraj);
            saveLogsCommand.Parameters.AddWithValue("@Display3", display3);
            saveLogsCommand.Parameters.AddWithValue("@Nc", nc);
            saveLogsCommand.Parameters.AddWithValue("@Even", even);
            saveLogsCommand.Parameters.AddWithValue("@Parp", parp);
            saveLogsCommand.Parameters.AddWithValue("@Pari", pari);
            saveLogsCommand.Parameters.AddWithValue("@DateTimes", Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
            saveLogsCommand.ExecuteNonQuery();
        }

        private void write2(string dbName, double diameter, double diameterSet, double diameterDifference, double plusTolerance,
            double minusTolerance, double metraj, double display3, double nc, double even, double parp, double pari,
            double diameter2, double plusTolerance2, double minusTolerance2, double xAxis, double yAxis)
        {

            string saveIntoDatabase = String.Format("INSERT INTO {0} (Cap, Cap2, CapSet, CapFark, ArtiTolerans, ArtiTolerans2, EksiTolerans, EksiTolerans2, Metraj, Xekseni, Yekseni, Display3, NC, Even, Par_p, Par_i, Tarih) " +
                " VALUES (@Diameter1, @Diameter2, @DiameterSet, @DiameterDifference, @PlusTolerance1, @PlusTolerance2, @MinusTolerance1, @MinusTolerance2, @Metraj, @Xaxis, @Yaxis, @Display3, @Nc, @Even, @Parp, @Pari, @DateTimes);", dbName);

            SqlCommand saveLogsCommand = new SqlCommand(saveIntoDatabase, localDbConnection);

            saveLogsCommand.Parameters.AddWithValue("@Diameter1", diameter);
            saveLogsCommand.Parameters.AddWithValue("@Diameter2", diameter2);
            saveLogsCommand.Parameters.AddWithValue("@DiameterSet", diameterSet);
            saveLogsCommand.Parameters.AddWithValue("@DiameterDifference", diameterDifference);
            saveLogsCommand.Parameters.AddWithValue("@PlusTolerance1", plusTolerance);
            saveLogsCommand.Parameters.AddWithValue("@PlusTolerance2", plusTolerance2);
            saveLogsCommand.Parameters.AddWithValue("@MinusTolerance1", minusTolerance);
            saveLogsCommand.Parameters.AddWithValue("@MinusTolerance2", minusTolerance2);
            saveLogsCommand.Parameters.AddWithValue("@Metraj", metraj);
            saveLogsCommand.Parameters.AddWithValue("@Xaxis", xAxis);
            saveLogsCommand.Parameters.AddWithValue("@Yaxis", yAxis);
            saveLogsCommand.Parameters.AddWithValue("@Display3", display3);
            saveLogsCommand.Parameters.AddWithValue("@Nc", nc);
            saveLogsCommand.Parameters.AddWithValue("@Even", even);
            saveLogsCommand.Parameters.AddWithValue("@Parp", parp);
            saveLogsCommand.Parameters.AddWithValue("@Pari", pari);
            saveLogsCommand.Parameters.AddWithValue("@DateTimes", Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
            saveLogsCommand.ExecuteNonQuery();
        }

        private void insertIntoDb(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice1(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice1(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb2(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice2(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice2(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb3(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice3(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice3(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb4(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice4(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice4(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb5(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice5(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice5(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb6(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice6(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice6(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb7(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice7(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice7(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb8(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice8(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice8(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb9(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice9(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice9(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb10(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice10(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice10(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb11(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice11(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice11(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb12(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice12(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice12(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb13(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice13(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice13(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void insertIntoDb14(byte slave, string dbName, int type)
        {
            try
            {
                if (type == 1)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice14(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, 0, 0, 0, 0, 0);

                    write1(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari);
                }
                if (type == 2)
                {
                    ushort[] registers = modbus.ReadHoldingRegisters(slave, 66, 12);
                    double diameter = double.Parse(registers[0].ToString()) / 1000;
                    double diameterDifference = double.Parse(registers[3].ToString()) / 1000;
                    double diameterSet = double.Parse(registers[4].ToString()) / 1000;
                    double plusTolerance = double.Parse(registers[5].ToString()) / 1000;
                    double minusTolerance = double.Parse(registers[6].ToString()) / 1000;
                    double display3 = double.Parse(registers[7].ToString());
                    double nc = double.Parse(registers[8].ToString());
                    double even = double.Parse(registers[9].ToString());
                    double parp = double.Parse(registers[10].ToString());
                    double pari = double.Parse(registers[11].ToString());
                    double metraj = 0;
                    plusTolerance = diameterSet + plusTolerance;
                    minusTolerance = diameterSet - minusTolerance;

                    setDataDevice14(diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                    write2(dbName, diameter, diameterSet, diameterDifference, plusTolerance, minusTolerance, metraj, display3,
                        nc, even, parp, pari, diameter, plusTolerance, minusTolerance, 100, 100);

                }
            }
            catch (NullReferenceException) { connectionSettings(); }
        }

        private void closeConnections()
        {
            connection.Close();
            localDbConnection.Close();
            localDbConnectionUnity.Close();
        }

        private void WorkOrder_Click(object sender, RoutedEventArgs e)
        {
            int counter = countSelectedProjects();
            if (counter == 0)
            {
                MessageBox.Show(rm.GetString("workOrderNoProject", cultureInfo), rm.GetString("system", cultureInfo),
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            for (int i = 0; i < counter; i++)
            {
                if (localDbConnection.State == ConnectionState.Closed)
                    localDbConnection.Open();
                SqlCommand commands = new SqlCommand("SELECT * FROM dbo.getWorkOrderData(@db)", localDbConnection);
                commands.Parameters.AddWithValue("@db", databases[i]);
                SqlDataAdapter dataAdapterCompaniesGroup = new SqlDataAdapter(commands);
                DataTable dataTableCompaniesGroup = new DataTable();
                dataAdapterCompaniesGroup.Fill(dataTableCompaniesGroup);

                SqlCommand command = new SqlCommand("dbo.createWorkOrder", localDbConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@operators", dataTableCompaniesGroup.Rows[0][0].ToString());
                command.Parameters.AddWithValue("@device", dataTableCompaniesGroup.Rows[0][1].ToString());
                command.Parameters.AddWithValue("@company", dataTableCompaniesGroup.Rows[0][2].ToString());
                command.Parameters.AddWithValue("@product", dataTableCompaniesGroup.Rows[0][3].ToString());
                command.Parameters.AddWithValue("@dateCreated", Convert.ToDateTime(dataTableCompaniesGroup.Rows[0][4].ToString()));
                command.Parameters.AddWithValue("@dateDue", Convert.ToDateTime(dataTableCompaniesGroup.Rows[0][5].ToString()));
                command.Parameters.AddWithValue("@explanation", dataTableCompaniesGroup.Rows[0][6].ToString());
                command.ExecuteNonQuery();
            }
            switch (counter)
            {
                case 1:
                    devicesWorkOrderReport1();
                    break;

                case 2:
                    devicesWorkOrderReport2();
                    break;

                case 3:
                    devicesWorkOrderReport3();
                    break;

                case 4:
                    devicesWorkOrderReport4();
                    break;

                case 5:
                    devicesWorkOrderReport5();
                    break;

                case 6:
                    devicesWorkOrderReport6();
                    break;

                case 7:
                    devicesWorkOrderReport7();
                    break;

                case 8:
                    devicesWorkOrderReport8();
                    break;

                case 9:
                    devicesWorkOrderReport9();
                    break;

                case 10:
                    devicesWorkOrderReport10();
                    break;

                case 11:
                    devicesWorkOrderReport11();
                    break;

                case 12:
                    devicesWorkOrderReport12();
                    break;

                case 13:
                    devicesWorkOrderReport13();
                    break;

                case 14:
                    devicesWorkOrderReport14();
                    break;
            }
        }

        private void devicesWorkOrderReport14()
        {
            WorkOrder14 workOrder14 = new WorkOrder14();
            workOrder14.Show();
        }

        private void devicesWorkOrderReport13()
        {
            WorkOrder13 workOrder13 = new WorkOrder13();
            workOrder13.Show();
        }

        private void devicesWorkOrderReport12()
        {
            WorkOrder12 workOrder12 = new WorkOrder12();
            workOrder12.Show();
        }

        private void devicesWorkOrderReport11()
        {
            WorkOrder11 workOrder11 = new WorkOrder11();
            workOrder11.Show();
        }

        private void devicesWorkOrderReport10()
        {
            WorkOrder10 workOrder10 = new WorkOrder10();
            workOrder10.Show();
        }

        private void devicesWorkOrderReport9()
        {
            WorkOrder9 workOrder9 = new WorkOrder9();
            workOrder9.Show();
        }

        private void devicesWorkOrderReport8()
        {
            WorkOrder8 workOrder8 = new WorkOrder8();
            workOrder8.Show();
        }

        private void devicesWorkOrderReport7()
        {
            WorkOrder7 workOrder7 = new WorkOrder7();
            workOrder7.Show();
        }

        private void devicesWorkOrderReport6()
        {
            WorkOrder6 workOrder6 = new WorkOrder6();
            workOrder6.Show();
        }

        private void devicesWorkOrderReport5()
        {
            WorkOrder5 workOrder5 = new WorkOrder5();
            workOrder5.Show();
        }

        private void devicesWorkOrderReport4()
        {
            WorkOrder4 workOrder4 = new WorkOrder4();
            workOrder4.Show();
        }

        private void devicesWorkOrderReport3()
        {
            WorkOrder3 workOrder3 = new WorkOrder3();
            workOrder3.Show();
        }

        private void devicesWorkOrderReport2()
        {
            WorkOrder2 workOrder2 = new WorkOrder2();
            workOrder2.Show();
        }

        private void devicesWorkOrderReport1()
        {
            WorkOrder1 workOrder1 = new WorkOrder1();
            workOrder1.Show();
        }

        private int countSelectedProjects()
        {
            List<string> parameters = new List<string>();
            databases.Clear();

            parameters.Add(VarsDevices.device1DbName);
            parameters.Add(VarsDevices.device2DbName);
            parameters.Add(VarsDevices.device3DbName);
            parameters.Add(VarsDevices.device4DbName);
            parameters.Add(VarsDevices.device5DbName);
            parameters.Add(VarsDevices.device6DbName);
            parameters.Add(VarsDevices.device7DbName);
            parameters.Add(VarsDevices.device8DbName);
            parameters.Add(VarsDevices.device9DbName);
            parameters.Add(VarsDevices.device10DbName);
            parameters.Add(VarsDevices.device11DbName);
            parameters.Add(VarsDevices.device12DbName);
            parameters.Add(VarsDevices.device13DbName);
            parameters.Add(VarsDevices.device14DbName);

            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] != null)
                {
                    databases.Add(parameters[i]);
                }
            }
            return databases.Count;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private int getDeviceType(int id)
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();
            SqlCommand commandToGetDb = new SqlCommand("SELECT * FROM dbo.getDeviceName(@id)", localDbConnectionUnity);
            commandToGetDb.Parameters.AddWithValue("@id", id);
            SqlDataAdapter dataAdapterDb = new SqlDataAdapter(commandToGetDb);
            DataTable dataTableDb = new DataTable();
            dataAdapterDb.Fill(dataTableDb);
            localDbConnectionUnity.Close();
            return int.Parse(dataTableDb.Rows[0][1].ToString());
        }
    }
}
