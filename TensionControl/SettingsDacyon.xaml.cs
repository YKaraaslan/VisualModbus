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
    /// Interaction logic for SettingsDacyon.xaml
    /// </summary>
    public partial class SettingsDacyon : Window
    {
        int send;
        ModbusClient modbusClient = Vars.client;

        public SettingsDacyon(int selected)
        {
            InitializeComponent();

            if (selected == 0)
            {
                panel.Background = Brushes.LightGreen;
                klemens.Background = Brushes.White;
            }
            if (selected == 1)
            {
                panel.Background = Brushes.White;
                klemens.Background = Brushes.LightGreen;
            }
        }

        private void panelButton_Click(object sender, RoutedEventArgs e)
        {
            panel.Background = Brushes.LightGreen;
            klemens.Background = Brushes.White;
            send = 0;
        }

        private void klemensButton_Click(object sender, RoutedEventArgs e)
        {
            panel.Background = Brushes.White;
            klemens.Background = Brushes.LightGreen;
            send = 1;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                modbusClient.WriteSingleRegister(4, send);
                MessageBox.Show("Adrese yazıldı");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
