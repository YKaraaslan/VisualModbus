using EasyModbus;
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
    /// Interaction logic for SettingsStart.xaml
    /// </summary>
    public partial class SettingsStart : Window
    {
        int send;
        ModbusClient modbusClient = VarsTension.client;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        BackgroundWorker backgroundWorker = new BackgroundWorker();

        public SettingsStart(int selected, BackgroundWorker backgroundWorkerReceived)
        {
            backgroundWorker = backgroundWorkerReceived;
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);

            if (selected == 1)
            {
                panel.Background = Brushes.LightGreen;
                klemens.Background = Brushes.White;
            }
            if (selected == 2)
            {
                panel.Background = Brushes.White;
                klemens.Background = Brushes.LightGreen;
            }
        }

        private void panelButton_Click(object sender, RoutedEventArgs e)
        {
            panel.Background = Brushes.LightGreen;
            klemens.Background = Brushes.White;
            send = 1;
        }

        private void klemensButton_Click(object sender, RoutedEventArgs e)
        {
            panel.Background = Brushes.White;
            klemens.Background = Brushes.LightGreen;
            send = 2;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorker.CancelAsync();
                Thread.Sleep(100);
                modbusClient.WriteSingleRegister(3, send);
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
