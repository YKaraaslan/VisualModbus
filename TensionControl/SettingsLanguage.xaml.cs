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
    /// Interaction logic for SettingsLanguage.xaml
    /// </summary>
    public partial class SettingsLanguage : Window
    {
        int send;
        ModbusClient modbusClient = Vars.client;

        public SettingsLanguage(int selected)
        {
            InitializeComponent();

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
                modbusClient.WriteSingleRegister(16, send);
                MessageBox.Show("Adrese yazıldı");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
