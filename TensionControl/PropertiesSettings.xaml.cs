using EasyModbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TensionControl
{
    /// <summary>
    /// Interaction logic for PropertiesSettings.xaml
    /// </summary>
    public partial class PropertiesSettings : UserControl
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        ModbusClient modbusClient = new ModbusClient();
        byte slave = 247;
        int valueSetSettings, valueStart, valueDacyon, valueLoadcell, valueManAuto, valueLanguage, valueStartVoltage, valueBaudRate;

        public PropertiesSettings()
        {
            InitializeComponent();
            dispatcherTimer.Tick += timerWork;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            init();
            dispatcherTimer.Start();
        }

        private void init()
        {
            try
            {
                if (Vars.client != null)
                {
                    modbusClient = Vars.client;
                    if (!modbusClient.Connected)
                        modbusClient.Connect();
                }
                else
                {
                    string[] ports = SerialPort.GetPortNames();
                    modbusClient.SerialPort = ports[0];
                    modbusClient.UnitIdentifier = Convert.ToByte(Properties.Settings.Default.tensionSlave);
                    modbusClient.Baudrate = Properties.Settings.Default.tensionBaudRate;
                    modbusClient.Parity = Parity.None;
                    modbusClient.StopBits = StopBits.Two;
                    modbusClient.ConnectionTimeout = 1000;
                    modbusClient.Connect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timerWork(object sender, EventArgs e)
        {
            int[] registers = modbusClient.ReadHoldingRegisters(1, 28);

            kp.Content = registers[0].ToString();

            tensionSet.Content = (registers[5]/100).ToString();

            startTime.Content = registers[7].ToString();
            stopTime.Content = registers[8].ToString();

            proxSel.Content = registers[9].ToString();
            startTime1.Content = registers[10].ToString();
            stopTime1.Content = registers[11].ToString();
            stopVoltage.Content = registers[12].ToString();
            minVoltage.Content = registers[13].ToString();

            modbusAdress.Content = registers[17].ToString();

            netWeight.Content = registers[19].ToString();
            filter.Content = registers[20].ToString();
            filterAverage.Content = registers[21].ToString();
            zeroMultipler.Content = registers[22].ToString();

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

            if (valueSetSettings == 1)
                setSetting.Content = "Panel";
            else
                setSetting.Content = "Klemens";

            if (registers[2] == 1)
                startAdress.Content = "Panel";
            else
                startAdress.Content = "Klemens";

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
                startVoltage.Content = "Stop Gerilimi";
            else if (registers[14] == 1)
                startVoltage.Content = "Son Gerilim";
            else
                startVoltage.Content = "Stop Son";

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
        }

        private void setSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingsSetSettings settings = new SettingsSetSettings(valueSetSettings);
            settings.ShowDialog();
        }

        private void tensionSet_Click(object sender, RoutedEventArgs e)
        {
            SettingsTensionSet settings = new SettingsTensionSet(Convert.ToInt32(tensionSet.Content));
            settings.ShowDialog();
        }

        private void startAdress_Click(object sender, RoutedEventArgs e)
        {
            SettingsStart settings = new SettingsStart(valueStart);
            settings.ShowDialog();
        }

        private void startTime_Click(object sender, RoutedEventArgs e)
        {
            SettingsStartTime settings = new SettingsStartTime(Convert.ToInt32(startTime.Content));
            settings.ShowDialog();
        }

        private void stopTime_Click(object sender, RoutedEventArgs e)
        {
            SettingsStopTime settings = new SettingsStopTime(Convert.ToInt32(stopTime.Content));
            settings.ShowDialog();
        }

        private void proxSel_Click(object sender, RoutedEventArgs e)
        {
            SettingsProx settings = new SettingsProx(Convert.ToInt32(proxSel.Content));
            settings.ShowDialog();
        }

        private void startTime1_Click(object sender, RoutedEventArgs e)
        {
            SettingsStartTime1 settings = new SettingsStartTime1(Convert.ToInt32(startTime1.Content));
            settings.ShowDialog();
        }

        private void stopTime1_Click(object sender, RoutedEventArgs e)
        {
            SettingsStopTime1 settings = new SettingsStopTime1(Convert.ToInt32(stopTime1.Content));
            settings.ShowDialog();
        }

        private void stopVoltage_Click(object sender, RoutedEventArgs e)
        {
            SettingsStopVoltage settings = new SettingsStopVoltage(Convert.ToInt32(stopVoltage.Content));
            settings.ShowDialog();
        }

        private void minVoltage_Click(object sender, RoutedEventArgs e)
        {
            SettingsMinVoltage settings = new SettingsMinVoltage(Convert.ToInt32(minVoltage.Content));
            settings.ShowDialog();
        }

        private void startVoltage_Click(object sender, RoutedEventArgs e)
        {
            SettingsStartVoltage settings = new SettingsStartVoltage(valueStartVoltage);
            settings.ShowDialog();
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            SettingsFilterMedian settings = new SettingsFilterMedian(Convert.ToInt32(filter.Content));
            settings.ShowDialog();
        }

        private void filterAverage_Click(object sender, RoutedEventArgs e)
        {
            SettingsFilterAverage settings = new SettingsFilterAverage(Convert.ToInt32(filterAverage.Content));
            settings.ShowDialog();
        }

        private void baudRate_Click(object sender, RoutedEventArgs e)
        {
            SettingsBaudRate settings = new SettingsBaudRate(valueBaudRate);
            settings.ShowDialog();
        }

        private void modbusAdress_Click(object sender, RoutedEventArgs e)
        {
            SettingsModBusAdress settings = new SettingsModBusAdress(Convert.ToInt32(modbusAdress.Content));
            settings.ShowDialog();
        }

        private void language_Click(object sender, RoutedEventArgs e)
        {
            SettingsLanguage settings = new SettingsLanguage(valueLanguage);
            settings.ShowDialog();
        }

        private void manuelAuto_Click(object sender, RoutedEventArgs e)
        {
            SettingsManuelAuto settings = new SettingsManuelAuto(valueManAuto);
            settings.ShowDialog();
        }

        private void loadcell_Click(object sender, RoutedEventArgs e)
        {
            SettingsLoadcell settings = new SettingsLoadcell(valueLoadcell);
            settings.ShowDialog();
        }

        private void dacyon_Click(object sender, RoutedEventArgs e)
        {
            SettingsDacyon settings = new SettingsDacyon(valueDacyon);
            settings.ShowDialog();
        }

        private void kp_Click(object sender, RoutedEventArgs e)
        {
            SettingsKP settings = new SettingsKP(Convert.ToInt32(kp.Content));
            settings.ShowDialog();
        }
    }
}
