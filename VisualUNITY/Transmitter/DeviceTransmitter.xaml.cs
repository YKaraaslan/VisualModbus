using EasyModbus;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
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
using System.Windows.Threading;

namespace VisualUNITY.Transmitter
{
    /// <summary>
    /// Interaction logic for DeviceTransmitter.xaml
    /// </summary>
    public partial class DeviceTransmitter : UserControl
    {
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        byte slave = 247;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer seconds = new DispatcherTimer();
        public SeriesCollection SeriesCollection { get; set; }
        ModbusClient modbusClient = new ModbusClient();

        public DeviceTransmitter()
        {
            InitializeComponent();
            //assembly = Assembly.Load("VisualUNITY");
            //rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            //cultureInfo = new CultureInfo(Properties.Settings.Default.language);

            dispatcherTimer.Tick += timerWork;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            seconds.Tick += secondsWork;
            seconds.Interval = TimeSpan.FromSeconds(1);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Gergi Set",
                    Values = new ChartValues<int> { }
                },
                new LineSeries
                {
                    Title = "Gergi",
                    Values = new ChartValues<int> { }
                }
            };

            DataContext = this;
            if (VarsTransmitter.client == null) init();
            else modbusClient = VarsTransmitter.client;
        }

        int counter = 1;

        private void secondsWork(object sender, EventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds(counter);
            string str = time.ToString(@"hh\:mm\:ss");
            labelTime.Content = str;
            counter++;
        }

        private void init()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                modbusClient.SerialPort = ports[0];
                modbusClient.UnitIdentifier = Convert.ToByte(Properties.Settings.Default.tensionSlave);
                modbusClient.Baudrate = Properties.Settings.Default.tensionBaudRate;
                modbusClient.Parity = Parity.None;
                modbusClient.StopBits = StopBits.Two;
                modbusClient.ConnectionTimeout = 1000;
                modbusClient.Connect();
                VarsTransmitter.client = modbusClient;
                //ellipseConnected.Fill = Brushes.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timerWork(object sender, EventArgs e)
        {
            ushort adr = 6;
            ushort adr2 = 20;
            ushort adrd = 1;
            int[] register1 = modbusClient.ReadHoldingRegisters(adr, adrd);
            int[] register2 = modbusClient.ReadHoldingRegisters(adr2, adrd);

            SeriesCollection[0].Values.Add(register1[0]);
            SeriesCollection[1].Values.Add(register2[0]);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
            }
            label.Content = register2[0].ToString();
        }

        private void start_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            modbusClient.WriteSingleRegister(24, 1);
        }

        private void stop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            modbusClient.WriteSingleRegister(25, 1);
        }

        private void up_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dispatcherTimer.Start();
            seconds.Start();
        }

        private void down_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void esc_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void menu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            modbusClient.WriteSingleRegister(19, 3);
            MessageBox.Show("yazildi");

        }

        private void enter_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tare_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            modbusClient.WriteSingleRegister(26, 1);
        }

        int i = 2;

        private void ellipseConnected_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (i % 2 == 0)
                dispatcherTimer.Start();
            else
                dispatcherTimer.Stop();
            i++;
        }
    }
}
