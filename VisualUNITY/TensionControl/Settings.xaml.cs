using MaterialDesignThemes.Wpf;
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

namespace VisualUNITY.TensionControl
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;

        public Settings()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            Snackbar.MessageQueue = myMessageQueue;
            init();
        }

        private void init()
        {
            modulatingGain.IsChecked = Properties.Settings.Default.tensionKpClickable;
            setSettings.IsChecked = Properties.Settings.Default.tensionSetSettingClickable;
            startAddress.IsChecked = Properties.Settings.Default.tensionStartAddressClickable;
            dacyonAdress.IsChecked = Properties.Settings.Default.tensionDacyonClickable;
            loadcell.IsChecked = Properties.Settings.Default.tensionLoadcellClickable;
            tensionSet.IsChecked = Properties.Settings.Default.tensionTensionSetClickable;
            manuelAuto.IsChecked = Properties.Settings.Default.tensionManuelAutoClickable;
            startTime.IsChecked = Properties.Settings.Default.tensionStartTimeClickable;
            stopTime.IsChecked = Properties.Settings.Default.tensionStopTimeClickable;
            proxsel.IsChecked = Properties.Settings.Default.tensionProxSelClickable;
            startTime1.IsChecked = Properties.Settings.Default.tensionStartTime1Clickable;
            stopTime1.IsChecked = Properties.Settings.Default.tensionStopTime1Clickable;
            stopOutputVoltage.IsChecked = Properties.Settings.Default.tensionStopVoltageClickable;
            minOutputVoltage.IsChecked = Properties.Settings.Default.tensionMinVoltageClickable;
            startVoltage.IsChecked = Properties.Settings.Default.tensionStartVoltageClickable;
            languages.IsChecked = Properties.Settings.Default.tensionLanguageClickable;
            modbusAddress.IsChecked = Properties.Settings.Default.tensionModbusAddressClickable;
            baudRate.IsChecked = Properties.Settings.Default.tensionBaudRateClickable;
            netWeight.IsChecked = Properties.Settings.Default.tensionNetWeightClickable;
            zeroMultiplier.IsChecked = Properties.Settings.Default.tensionZeroMultiplerClickable;
            medianFilterValue.IsChecked = Properties.Settings.Default.tensionMedianFilterClickable;
            averageFilterValue.IsChecked = Properties.Settings.Default.tensionAverageFilterClickable;
            fullScale.IsChecked = Properties.Settings.Default.tensionFullScaleClickable;
            calibration.IsChecked = Properties.Settings.Default.tensionCalibrationClickable;

            if (Properties.Settings.Default.language == "tr-TR")
                turkish.IsChecked = true;

            if (Properties.Settings.Default.language == "en-EN")
                english.IsChecked = true;

            if (Properties.Settings.Default.language == "ar-AR")
                arabic.IsChecked = true;

            slaveId.Text = Properties.Settings.Default.tensionSlave.ToString();
            setBaudRate(Properties.Settings.Default.tensionBaudRate);
        }

        private void setBaudRate(int deviceBaudRate)
        {
            if (deviceBaudRate == 2400)
                baud.SelectedIndex = 0;
            if (deviceBaudRate == 4800)
                baud.SelectedIndex = 1;
            if (deviceBaudRate == 9600)
                baud.SelectedIndex = 2;
            if (deviceBaudRate == 19200)
                baud.SelectedIndex = 3;
            if (deviceBaudRate == 38400)
                baud.SelectedIndex = 4;
            if (deviceBaudRate == 57600)
                baud.SelectedIndex = 5;
            if (deviceBaudRate == 115200)
                baud.SelectedIndex = 6;
        }

        private void HandleOKMethod()
        {
            this.Close();
        }

        private void turkish_Checked(object sender, RoutedEventArgs e)
        {
            english.IsChecked = false;
            arabic.IsChecked = false;
        }

        private void english_Checked(object sender, RoutedEventArgs e)
        {
            turkish.IsChecked = false;
            arabic.IsChecked = false;
        }

        private void arabic_Checked(object sender, RoutedEventArgs e)
        {
            english.IsChecked = false;
            turkish.IsChecked = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Flipper_OnIsFlippedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            Properties.Settings.Default.tensionKpClickable = (bool)modulatingGain.IsChecked;
            Properties.Settings.Default.tensionSetSettingClickable = (bool)setSettings.IsChecked;
            Properties.Settings.Default.tensionStartAddressClickable = (bool)startAddress.IsChecked;
            Properties.Settings.Default.tensionDacyonClickable = (bool)dacyonAdress.IsChecked;
            Properties.Settings.Default.tensionLoadcellClickable = (bool)loadcell.IsChecked;
            Properties.Settings.Default.tensionTensionSetClickable = (bool)tensionSet.IsChecked;
            Properties.Settings.Default.tensionManuelAutoClickable = (bool)manuelAuto.IsChecked;
            Properties.Settings.Default.tensionStartTimeClickable = (bool)startTime.IsChecked;
            Properties.Settings.Default.tensionStopTimeClickable = (bool)stopTime.IsChecked;
            Properties.Settings.Default.tensionProxSelClickable = (bool)proxsel.IsChecked;
            Properties.Settings.Default.tensionStartTime1Clickable = (bool)startTime1.IsChecked;
            Properties.Settings.Default.tensionStopTime1Clickable = (bool)stopTime1.IsChecked;
            Properties.Settings.Default.tensionStopVoltageClickable = (bool)stopOutputVoltage.IsChecked;
            Properties.Settings.Default.tensionMinVoltageClickable = (bool)minOutputVoltage.IsChecked;
            Properties.Settings.Default.tensionStartVoltageClickable = (bool)startVoltage.IsChecked;
            Properties.Settings.Default.tensionLanguageClickable = (bool)languages.IsChecked;
            Properties.Settings.Default.tensionModbusAddressClickable = (bool)modbusAddress.IsChecked;
            Properties.Settings.Default.tensionBaudRateClickable = (bool)baudRate.IsChecked;
            Properties.Settings.Default.tensionNetWeightClickable = (bool)netWeight.IsChecked;
            Properties.Settings.Default.tensionZeroMultiplerClickable = (bool)zeroMultiplier.IsChecked;
            Properties.Settings.Default.tensionMedianFilterClickable = (bool)medianFilterValue.IsChecked;
            Properties.Settings.Default.tensionAverageFilterClickable = (bool)averageFilterValue.IsChecked;
            Properties.Settings.Default.tensionFullScaleClickable = (bool)fullScale.IsChecked;
            Properties.Settings.Default.tensionCalibrationClickable = (bool)calibration.IsChecked;

            if (turkish.IsChecked == true)
            {
                cultureInfo = new CultureInfo("tr-TR");
                Properties.Settings.Default.language = "tr-TR";
            }
            else if (english.IsChecked == true)
            {
                cultureInfo = new CultureInfo("en-EN");
                Properties.Settings.Default.language = "en-EN";
            }
            else if (arabic.IsChecked == true)
            {
                cultureInfo = new CultureInfo("ar-AR");
                Properties.Settings.Default.language = "ar-AR";
            }

            try
            {
                int value = Convert.ToInt32(slaveId.Text.ToString());
                if (value > 247 || value < 0)
                {
                    myMessageQueue.Enqueue(rm.GetString("valueRange247Exceeded"), rm.GetString("ok"), () => HandleOKMethod());
                    return;
                }
                Properties.Settings.Default.tensionSlave = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, rm.GetString("system", cultureInfo), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Properties.Settings.Default.tensionSlave = Convert.ToInt32(slaveId.Text);
            Properties.Settings.Default.tensionBaudRate = int.Parse(baud.Text);

            Properties.Settings.Default.Save();

            myMessageQueue.Enqueue(rm.GetString("settingsSaved", cultureInfo), rm.GetString("ok"),
                () => HandleOKMethod());
        }
    }
}
