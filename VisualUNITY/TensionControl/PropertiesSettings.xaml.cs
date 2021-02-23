using EasyModbus;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace VisualUNITY.TensionControl
{
    /// <summary>
    /// Interaction logic for PropertiesSettings.xaml
    /// </summary>
    public partial class PropertiesSettings : UserControl
    {
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        bool whileBoolean = true;
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        BackgroundWorker backgroundWorkerRead = new BackgroundWorker();
        ModbusClient modbusClient = new ModbusClient();
        int valueSetSettings, valueStart, valueDacyon, valueLoadcell, valueManAuto, valueLanguage, valueStartVoltage, valueBaudRate, valueZeroMultipler;
        int kpInt, tensionSetInt;

        public PropertiesSettings()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);

            backgroundWorker.DoWork += BackgroundWorker_DoWork;

            backgroundWorkerRead.WorkerSupportsCancellation = true;
            backgroundWorkerRead.DoWork += BackgroundWorkerRead_DoWork;

            setClickables();
            if (VarsTension.client == null)
            {
                try
                {
                    init();
                    enableButtons();
                }
                catch { }
            }
            else
            {
                modbusClient = VarsTension.client;
                if (!modbusClient.Connected)
                    modbusClient.Connect();
                if (!backgroundWorkerRead.IsBusy)
                    backgroundWorkerRead.RunWorkerAsync();
                ellipseConnected.Fill = Brushes.LightGreen;
                enableButtons();
            }

            if (!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (whileBoolean)
            {
                Thread.Sleep(500);
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
                        enableButtons();
                    }
                    else if (modbusClient.Connected && !backgroundWorkerRead.IsBusy)
                    {
                        if (!backgroundWorkerRead.IsBusy)
                            backgroundWorkerRead.RunWorkerAsync();
                        enableButtons();
                    }
                }
                catch { }
            }
        }

        private void setClickables()
        {
            kp.IsHitTestVisible = Properties.Settings.Default.tensionKpClickable;
            setSetting.IsHitTestVisible = Properties.Settings.Default.tensionSetSettingClickable;
            startAdress.IsHitTestVisible = Properties.Settings.Default.tensionStartAddressClickable;
            dacyon.IsHitTestVisible = Properties.Settings.Default.tensionDacyonClickable;
            loadcell.IsHitTestVisible = Properties.Settings.Default.tensionLoadcellClickable;
            tensionSet.IsHitTestVisible = Properties.Settings.Default.tensionTensionSetClickable;
            manuelAuto.IsHitTestVisible = Properties.Settings.Default.tensionManuelAutoClickable;
            startTime.IsHitTestVisible = Properties.Settings.Default.tensionStartTimeClickable;
            stopTime.IsHitTestVisible = Properties.Settings.Default.tensionStopTimeClickable;
            proxSel.IsHitTestVisible = Properties.Settings.Default.tensionProxSelClickable;
            startTime1.IsHitTestVisible = Properties.Settings.Default.tensionStartTime1Clickable;
            stopTime1.IsHitTestVisible = Properties.Settings.Default.tensionStopTime1Clickable;
            stopVoltage.IsHitTestVisible = Properties.Settings.Default.tensionStopVoltageClickable;
            minVoltage.IsHitTestVisible = Properties.Settings.Default.tensionMinVoltageClickable;
            startVoltage.IsHitTestVisible = Properties.Settings.Default.tensionStartVoltageClickable;
            language.IsHitTestVisible = Properties.Settings.Default.tensionLanguageClickable;
            modbusAdress.IsHitTestVisible = Properties.Settings.Default.tensionModbusAddressClickable;
            baudRate.IsHitTestVisible = Properties.Settings.Default.tensionBaudRateClickable;
            netWeight.IsHitTestVisible = Properties.Settings.Default.tensionNetWeightClickable;
            zeroMultipler.IsHitTestVisible = Properties.Settings.Default.tensionZeroMultiplerClickable;
            filter.IsHitTestVisible = Properties.Settings.Default.tensionMedianFilterClickable;
            filterAverage.IsHitTestVisible = Properties.Settings.Default.tensionAverageFilterClickable;
            filledPocket.IsHitTestVisible = Properties.Settings.Default.tensionFullScaleClickable;
            calibration.IsHitTestVisible = Properties.Settings.Default.tensionCalibrationClickable;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            backgroundWorkerRead.CancelAsync();
            whileBoolean = false;
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
                enableButtons();
            }
            catch
            {
                backgroundWorkerRead.CancelAsync();
                modbusClient.Disconnect();
            }
        }

        private void enableButtons()
        {
            Button[] buttons = { kp, tensionSet, startTime, stopTime, proxSel, startTime1, stopTime1, stopVoltage, minVoltage, modbusAdress,
                netWeight, filter, filterAverage, filledPocket, calibration, setSetting, startAdress, dacyon, loadcell, manuelAuto, startVoltage,
                language, baudRate, zeroMultipler};

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].IsEnabled = true;
            }
        }

        private void BackgroundWorkerRead_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorkerRead.CancellationPending)
            {
                try
                {
                    int[] registers = modbusClient.ReadHoldingRegisters(1, 28);

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        ellipseConnected.Fill = Brushes.LightGreen;
                        enableButtons();

                        kpInt = registers[0];
                        kp.Content = "%" + kpInt.ToString();

                        tensionSetInt = registers[5];
                        tensionSet.Content = tensionSetInt.ToString();

                        startTime.Content = registers[7].ToString();
                        stopTime.Content = registers[8].ToString();

                        proxSel.Content = registers[9].ToString();
                        startTime1.Content = registers[10].ToString();
                        stopTime1.Content = registers[11].ToString();
                        stopVoltage.Content = registers[12].ToString();
                        minVoltage.Content = registers[13].ToString();

                        modbusAdress.Content = registers[17].ToString();

                        filter.Content = registers[20].ToString();
                        filterAverage.Content = registers[21].ToString();

                        filledPocket.Content = registers[26].ToString();
                        calibration.Content = registers[27].ToString();

                        valueSetSettings = registers[1];
                        valueStart = registers[2];
                        valueDacyon = registers[3];
                        valueLoadcell = registers[4];
                        valueManAuto = registers[6];
                        valueLanguage = registers[15];
                        valueStartVoltage = registers[14];
                        valueBaudRate = registers[18];
                        valueZeroMultipler = registers[22];

                        if (valueSetSettings == 1)
                            setSetting.Content = rm.GetString("panel", cultureInfo);
                        else
                            setSetting.Content = rm.GetString("terminal", cultureInfo);

                        if (registers[2] == 1)
                            startAdress.Content = rm.GetString("panel", cultureInfo);
                        else
                            startAdress.Content = rm.GetString("terminal", cultureInfo);

                        if (registers[3] == 0)
                            dacyon.Content = "0-10V";
                        else
                            dacyon.Content = "10-0V";

                        if (registers[4] == 1)
                            loadcell.Content = "50 KG";
                        else if (registers[4] == 2)
                            loadcell.Content = "100 KG";
                        else
                            loadcell.Content = "250 KG";

                        if (registers[6] == 0)
                            manuelAuto.Content = "Loadcell";
                        else if (registers[6] == 1)
                            manuelAuto.Content = "Manuel";
                        else if (registers[6] == 2)
                            manuelAuto.Content = "Dancer";
                        else
                            manuelAuto.Content = "Auto";

                        if (registers[14] == 0)
                            startVoltage.Content = rm.GetString("stopVoltage", cultureInfo);
                        else if (registers[14] == 1)
                            startVoltage.Content = rm.GetString("lastVoltage", cultureInfo);
                        else
                            startVoltage.Content = rm.GetString("stopLast", cultureInfo);

                        if (registers[15] == 1)
                            language.Content = "Türkçe";
                        else if (registers[15] == 2)
                            language.Content = "English";

                        if (registers[18] == 1)
                            baudRate.Content = "2400";
                        else if (registers[18] == 2)
                            baudRate.Content = "4800";
                        else if (registers[18] == 3)
                            baudRate.Content = "9600";
                        else if (registers[18] == 4)
                            baudRate.Content = "19200";
                        else
                            baudRate.Content = "38400";

                        double convertedNumber = Convert.ToDouble(registers[19]) / 100;
                        netWeight.Content = convertedNumber.ToString("N2");

                        if (registers[22] == 0)
                            zeroMultipler.Content = "10 Gr";
                        else if (registers[22] == 1)
                            zeroMultipler.Content = "20 Gr";
                        else if (registers[22] == 2)
                            zeroMultipler.Content = "50 Gr";
                        else if (registers[22] == 3)
                            zeroMultipler.Content = "100 Gr";
                        else if (registers[22] == 4)
                            zeroMultipler.Content = "200 Gr";
                        else if (registers[22] == 5)
                            zeroMultipler.Content = "500 Gr";
                    }));
                }
                catch
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        backgroundWorkerRead.CancelAsync();
                        modbusClient.Disconnect();
                        setAllMinus();
                        ellipseConnected.Fill = Brushes.IndianRed;
                    }));
                }
            }
        }

        private void setAllMinus()
        {
            Button[] buttons = { kp, tensionSet, startTime, stopTime, proxSel, startTime1, stopTime1, stopVoltage, minVoltage, modbusAdress,
                netWeight, filter, filterAverage, filledPocket, calibration, setSetting, startAdress, dacyon, loadcell, manuelAuto, startVoltage,
                language, baudRate, zeroMultipler};

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Content = "-";
                buttons[i].IsEnabled = false;
            }
        }

        private void setSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingsSetSettings settings = new SettingsSetSettings(valueSetSettings, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void tensionSet_Click(object sender, RoutedEventArgs e)
        {
            SettingsTensionSet settings = new SettingsTensionSet(tensionSetInt, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void startAdress_Click(object sender, RoutedEventArgs e)
        {
            SettingsStart settings = new SettingsStart(valueStart, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void startTime_Click(object sender, RoutedEventArgs e)
        {
            SettingsStartTime settings = new SettingsStartTime(Convert.ToInt32(startTime.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void stopTime_Click(object sender, RoutedEventArgs e)
        {
            SettingsStopTime settings = new SettingsStopTime(Convert.ToInt32(stopTime.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void proxSel_Click(object sender, RoutedEventArgs e)
        {
            SettingsProx settings = new SettingsProx(Convert.ToInt32(proxSel.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void startTime1_Click(object sender, RoutedEventArgs e)
        {
            SettingsStartTime1 settings = new SettingsStartTime1(Convert.ToInt32(startTime1.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void stopTime1_Click(object sender, RoutedEventArgs e)
        {
            SettingsStopTime1 settings = new SettingsStopTime1(Convert.ToInt32(stopTime1.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void stopVoltage_Click(object sender, RoutedEventArgs e)
        {
            SettingsStopVoltage settings = new SettingsStopVoltage(Convert.ToInt32(stopVoltage.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void minVoltage_Click(object sender, RoutedEventArgs e)
        {
            SettingsMinVoltage settings = new SettingsMinVoltage(Convert.ToInt32(minVoltage.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void startVoltage_Click(object sender, RoutedEventArgs e)
        {
            SettingsStartVoltage settings = new SettingsStartVoltage(valueStartVoltage, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            SettingsFilterMedian settings = new SettingsFilterMedian(Convert.ToInt32(filter.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void filterAverage_Click(object sender, RoutedEventArgs e)
        {
            SettingsFilterAverage settings = new SettingsFilterAverage(Convert.ToInt32(filterAverage.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void calibration_Click(object sender, RoutedEventArgs e)
        {
            SettingsCalibration settings = new SettingsCalibration(Convert.ToInt32(calibration.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void filledPocket_Click(object sender, RoutedEventArgs e)
        {
            SettingsFilledPocket settings = new SettingsFilledPocket(Convert.ToInt32(filledPocket.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void zeroMultipler_Click(object sender, RoutedEventArgs e)
        {
            SettingsZeroMultipler settings = new SettingsZeroMultipler(valueZeroMultipler, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void baudRate_Click(object sender, RoutedEventArgs e)
        {
            SettingsBaudRate settings = new SettingsBaudRate(valueBaudRate, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void modbusAdress_Click(object sender, RoutedEventArgs e)
        {
            SettingsModBusAdress settings = new SettingsModBusAdress(Convert.ToInt32(modbusAdress.Content), backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void language_Click(object sender, RoutedEventArgs e)
        {
            SettingsLanguage settings = new SettingsLanguage(valueLanguage, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void manuelAuto_Click(object sender, RoutedEventArgs e)
        {
            SettingsManuelAuto settings = new SettingsManuelAuto(valueManAuto, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void loadcell_Click(object sender, RoutedEventArgs e)
        {
            SettingsLoadcell settings = new SettingsLoadcell(valueLoadcell, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void dacyon_Click(object sender, RoutedEventArgs e)
        {
            SettingsDacyon settings = new SettingsDacyon(valueDacyon, backgroundWorkerRead);
            settings.ShowDialog();
        }

        private void kp_Click(object sender, RoutedEventArgs e)
        {
            SettingsKP settings = new SettingsKP(kpInt, backgroundWorkerRead);
            settings.ShowDialog();
        }
    }
}
