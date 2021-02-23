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
    /// Interaction logic for SettingsLoadcell.xaml
    /// </summary>
    public partial class SettingsLoadcell : Window
    {
        int send;
        ModbusClient modbusClient = Vars.client;

        public SettingsLoadcell(int selected)
        {
            InitializeComponent();

            if (selected == 1)
            {
                oneCard.Background = Brushes.LightGreen;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.White;
            }
            if (selected == 2)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.LightGreen;
                threeCard.Background = Brushes.White;
            }
            if (selected == 3)
            {
                oneCard.Background = Brushes.White;
                twoCard.Background = Brushes.White;
                threeCard.Background = Brushes.LightGreen;
            }
        }

        private void one_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.LightGreen;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.White;
            send = 1;
        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.LightGreen;
            threeCard.Background = Brushes.White;
            send = 2;
        }

        private void three_Click(object sender, RoutedEventArgs e)
        {
            oneCard.Background = Brushes.White;
            twoCard.Background = Brushes.White;
            threeCard.Background = Brushes.LightGreen;
            send = 3;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                modbusClient.WriteSingleRegister(5, send);
                MessageBox.Show("Adrese yazıldı");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
