using EasyModbus;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TensionControl
{
    /// <summary>
    /// Interaction logic for SettingsBaudRate.xaml
    /// </summary>
    public partial class SettingsBaudRate : Window
    {
        int send, value;
        ModbusClient modbusClient = Vars.client;

        public SettingsBaudRate(int selected)
        {
            InitializeComponent(); ;

            if (selected == 1)
            {
                oneCard.Background = Brushes.LightGreen;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.White;
                value = 2400;
            }
            if (selected == 2)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.LightGreen;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.White;
                value = 4800;
            }
            if (selected == 3)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.LightGreen;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.White;
                value = 9600;
            }
            if (selected == 4)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.LightGreen;
                fiveCard.Background = Brushes.White;
                value = 19200;
            }
            if (selected == 5)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
                fourCard.Background = Brushes.White;
                fiveCard.Background = Brushes.LightGreen;
                value = 38400;
            }
        }

        private void one_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.LightGreen;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.White;
            send = 1;
            value = 2400;
        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.LightGreen;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.White;
            send = 2;
            value = 4800;
        }

        private void three_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.LightGreen;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.White;
            send = 3;
            value = 9600;
        }

        private void four_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.LightGreen;
            fiveCard.Background = Brushes.White;
            send = 4;
            value = 19200;
        }

        private void five_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            fourCard.Background = Brushes.White;
            fiveCard.Background = Brushes.LightGreen;
            send = 5;
            value = 38400;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                modbusClient.WriteSingleRegister(19, send);
                Vars.client.Disconnect();
                Vars.client.Baudrate = value;
                Vars.client.Connect();
                Properties.Settings.Default.tensionBaudRate = value;
                Properties.Settings.Default.Save();
                MessageBox.Show("Adrese yazıldı");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
