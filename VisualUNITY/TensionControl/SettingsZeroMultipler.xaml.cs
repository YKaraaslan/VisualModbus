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
    /// Interaction logic for SettingsZeroMultipler.xaml
    /// </summary>
    public partial class SettingsZeroMultipler : Window
    {
        int send, value;
        ModbusClient modbusClient = VarsTension.client;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        BackgroundWorker backgroundWorker = new BackgroundWorker();

        public SettingsZeroMultipler(int selected, BackgroundWorker backgroundWorkerReceived)
        {
            backgroundWorker = backgroundWorkerReceived;
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);

            if (selected == 0)
            {
                oneCard.Background = Brushes.LightGreen;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.White;
                sixCard.Background = Brushes.White;
                value = 0;
            }
            if (selected == 1)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.LightGreen;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.White;
                sixCard.Background = Brushes.White;
                value = 1;
            }
            if (selected == 2)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.LightGreen;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.White;
                sixCard.Background = Brushes.White;
                value = 2;
            }
            if (selected == 3)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.LightGreen;
                fiveCard.Background = Brushes.White;
                sixCard.Background = Brushes.White;
                value = 3;
            }
            if (selected == 4)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.LightGreen;
                sixCard.Background = Brushes.White;
                value = 4;
            }
            if (selected == 5)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.White;
                sixCard.Background = Brushes.LightGreen;
                value = 5;
            }
        }

        private void one_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.LightGreen;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.White;
            sixCard.Background = Brushes.White;
            send = 0;
        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.LightGreen;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.White;
            sixCard.Background = Brushes.White;
            send = 1;
        }

        private void three_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.LightGreen;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.White;
            sixCard.Background = Brushes.White;
            send = 2;
        }

        private void four_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.LightGreen;
            fiveCard.Background = Brushes.White;
            sixCard.Background = Brushes.White;
            send = 3;
        }

        private void five_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.LightGreen;
            sixCard.Background = Brushes.White;
            send = 4;
        }

        private void six_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.White;
            sixCard.Background = Brushes.LightGreen;
            send = 5;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorker.CancelAsync();
                Thread.Sleep(100);
                modbusClient.WriteSingleRegister(23, send);
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
