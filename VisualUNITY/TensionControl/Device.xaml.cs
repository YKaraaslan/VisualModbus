using EasyModbus;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO.Ports;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace VisualUNITY.TensionControl
{
    /// <summary>
    /// Interaction logic for Device.xaml
    /// </summary>
    public partial class Device : UserControl
    {
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DispatcherTimer seconds = new DispatcherTimer();
        public SeriesCollection SeriesCollection { get; set; }
        ModbusClient modbusClient = new ModbusClient();
        int counter = 0, graphCounter = 0, graphRange, kpInt;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        BackgroundWorker backgroundWorkerRead = new BackgroundWorker();
        bool whileBoolean = true, second = false;
        double tensionSetDouble, tensionDouble;

        public Device()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            Snackbar.MessageQueue = myMessageQueue;

            pid_i.IsHitTestVisible = false;
            pid_d.IsHitTestVisible = false;
            tension.IsHitTestVisible = false;

            backgroundWorker.DoWork += BackgroundWorker_DoWork;

            backgroundWorkerRead.WorkerSupportsCancellation = true;
            backgroundWorkerRead.DoWork += BackgroundWorkerRead_DoWork;

            seconds.Tick += secondsWork;
            seconds.Interval = TimeSpan.FromSeconds(1);
            seconds.Start();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = rm.GetString("tensionSet", cultureInfo),
                    Values = new ChartValues<double> { },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = rm.GetString("tension", cultureInfo),
                    Values = new ChartValues<double> { },
                    PointGeometry = null
                }
            };

            DataContext = this;
            if (VarsTension.client == null)
            {
                init();
            }
            else
            {
                ellipseConnected.Fill = Brushes.LightGreen;
                modbusClient = VarsTension.client;
                if (!modbusClient.Connected)
                    modbusClient.Connect();
                if(!backgroundWorkerRead.IsBusy)
                    backgroundWorkerRead.RunWorkerAsync();
            }
            if (!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();

            graphRange = Properties.Settings.Default.tensionGraphRange;
            range.Content = graphRange.ToString();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (whileBoolean)
            {
                Thread.Sleep(100);
                try
                {
                    if (!modbusClient.Connected)
                    {
                        string[] ports = SerialPort.GetPortNames();
                        modbusClient.SerialPort = ports[0];
                        modbusClient.UnitIdentifier = Convert.ToByte(Properties.Settings.Default.tensionSlave);
                        modbusClient.Baudrate = Properties.Settings.Default.tensionBaudRate;
                        modbusClient.Connect();
                        VarsTension.client = modbusClient;
                        if (!backgroundWorkerRead.IsBusy)
                            backgroundWorkerRead.RunWorkerAsync();
                    }
                    else if (modbusClient.Connected && !backgroundWorkerRead.IsBusy)
                    {
                        backgroundWorkerRead.RunWorkerAsync();
                    }
                }
                catch { }

                //this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => 
                //{
                //    if (modbusClient.Connected)
                //        ss.Text = i++.ToString();
                //    else
                //        sd.Text = j++.ToString();
                //}));
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            backgroundWorkerRead.CancelAsync();
            whileBoolean = false;
            second = false;
        }

        private void secondsWork(object sender, EventArgs e)
        {
            if (second)
            {
                TimeSpan time = TimeSpan.FromSeconds(counter);
                labelTime.Content = time.ToString(@"hh\:mm\:ss");
                counter++;
            }
        }

        private void init()
        {
            if (modbusClient.Connected)
                modbusClient.Disconnect();
            try
            {
                string[] ports = SerialPort.GetPortNames();
                modbusClient.SerialPort = ports[0];
                modbusClient.UnitIdentifier = Convert.ToByte(Properties.Settings.Default.tensionSlave);
                modbusClient.Baudrate = Properties.Settings.Default.tensionBaudRate;
                modbusClient.Parity = Parity.None;
                modbusClient.StopBits = StopBits.Two;
                modbusClient.ConnectionTimeout = 500;
                modbusClient.Connect();
                VarsTension.client = modbusClient;
                if (!backgroundWorkerRead.IsBusy)
                    backgroundWorkerRead.RunWorkerAsync();
            }
            catch
            {
                backgroundWorkerRead.CancelAsync();
                modbusClient.Disconnect();
            }
        }

        private void BackgroundWorkerRead_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorkerRead.CancellationPending)
            {
                try
                {
                    int[] register1 = modbusClient.ReadHoldingRegisters(6, 1);
                    int[] register2 = modbusClient.ReadHoldingRegisters(20, 1);
                    int[] register3 = modbusClient.ReadHoldingRegisters(1, 1);

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        tensionSetDouble = Convert.ToDouble(register1[0]) / 100;
                        tensionDouble = Convert.ToDouble(register2[0]) / 100;

                        SeriesCollection[0].Values.Add(tensionSetDouble);
                        SeriesCollection[1].Values.Add(tensionDouble);

                        while (SeriesCollection[0].Values.Count > graphRange)
                        {

                            SeriesCollection[0].Values.RemoveAt(graphCounter);
                            SeriesCollection[1].Values.RemoveAt(graphCounter);
                            graphCounter++;
                        }

                        graphCounter = 0;

                        tension.Content = tensionDouble.ToString("N2");
                        tensionSet.Content = tensionSetDouble.ToString("N2");

                        pid_p.Content = "%" + register3[0].ToString();
                        kpInt = register3[0];
                        ellipseConnected.Fill = Brushes.LightGreen;
                        second = true;
                    }));
                }
                catch
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        ellipseConnected.Fill = Brushes.IndianRed;
                    }));
                    second = false;
                    backgroundWorkerRead.CancelAsync();
                    modbusClient.Disconnect();
                }
            }
        }

        private void start_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                backgroundWorkerRead.CancelAsync();
                Thread.Sleep(100);
                modbusClient.WriteSingleRegister(24, 1);
                if(!backgroundWorkerRead.IsBusy)
                    backgroundWorkerRead.RunWorkerAsync();
            }
            catch { }
        }

        private void stop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                backgroundWorkerRead.CancelAsync();
                Thread.Sleep(100);
                modbusClient.WriteSingleRegister(25, 1);
                if (!backgroundWorkerRead.IsBusy)
                    backgroundWorkerRead.RunWorkerAsync();
            }
            catch { }
        }

        private void up_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            backgroundWorkerRead.CancelAsync();
            Thread.Sleep(100);
            trySendingUp();
            Thread.Sleep(100);
            if (!backgroundWorkerRead.IsBusy)
                backgroundWorkerRead.RunWorkerAsync();
        }

        int countedUp, countedDown;

        private void trySendingUp()
        {
            countedUp++;
            try
            {
                int[] values = modbusClient.ReadHoldingRegisters(6, 1);
                int value = values[0] + 10;
                modbusClient.WriteSingleRegister(6, value);
            }
            catch { if (countedUp < 10) trySendingUp(); }
        }

        private void trySendingDown()
        {
            countedDown++;
            try
            {
                int[] values = modbusClient.ReadHoldingRegisters(6, 1);
                int value = values[0] - 10;
                modbusClient.WriteSingleRegister(6, value);
            }
            catch { if (countedDown < 10) trySendingUp(); }
        }

        private void kp_Click(object sender, RoutedEventArgs e)
        {
            SettingsKP settings = new SettingsKP(kpInt, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void tensionSet_Click(object sender, RoutedEventArgs e)
        {
            SettingsTensionSet settings = new SettingsTensionSet(Convert.ToInt32(tensionSetDouble*100), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void plus_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            graphRange += 5;
            if (graphRange > 200)
            {
                graphRange = 200;
            }
            range.Content = graphRange.ToString();
            Properties.Settings.Default.tensionGraphRange = graphRange;
            Properties.Settings.Default.Save();
        }

        private void minus_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            graphRange -= 5;
            if (graphRange < 50)
            {
                graphRange = 50;
            }
            range.Content = graphRange.ToString();
            Properties.Settings.Default.tensionGraphRange = graphRange;
            Properties.Settings.Default.Save();
        }

        private void down_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            backgroundWorkerRead.CancelAsync();
            Thread.Sleep(100);
            trySendingDown();
            Thread.Sleep(100);
            if (!backgroundWorkerRead.IsBusy)
                backgroundWorkerRead.RunWorkerAsync();
        }

        private void tare_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                backgroundWorkerRead.CancelAsync();
                Thread.Sleep(100);
                modbusClient.WriteSingleRegister(26, 1);
                if (!backgroundWorkerRead.IsBusy)
                    backgroundWorkerRead.RunWorkerAsync();
            }
            catch { }
        }
    }
}
