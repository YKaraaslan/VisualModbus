using EasyModbus;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Shapes;

namespace VisualUNITY.TensionControl
{
    /// <summary>
    /// Interaction logic for SettingsFilledPocket.xaml
    /// </summary>
    public partial class SettingsFilledPocket : Window
    {
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        ModbusClient modbusClient = VarsTension.client;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        BackgroundWorker backgroundWorker = new BackgroundWorker();

        public SettingsFilledPocket(int sent, BackgroundWorker backgroundWorkerReceived)
        {
            backgroundWorker = backgroundWorkerReceived;
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            Snackbar.MessageQueue = myMessageQueue;
            kp.Text = sent.ToString();
        }

        private void HandleOKMethod()
        {

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            writeSingleData();
        }

        private void kp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                writeSingleData();
        }

        private void writeSingleData()
        {
            try
            {
                int value = Convert.ToInt32(kp.Text.ToString());
                backgroundWorker.CancelAsync();
                Thread.Sleep(100);
                modbusClient.WriteSingleRegister(27, value);
                if (!backgroundWorker.IsBusy)
                    backgroundWorker.RunWorkerAsync();
                MessageBox.Show(rm.GetString("writingSuccessful", cultureInfo), rm.GetString("system", cultureInfo),
                MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, rm.GetString("system", cultureInfo), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
