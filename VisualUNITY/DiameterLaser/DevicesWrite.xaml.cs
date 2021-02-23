using MaterialDesignThemes.Wpf;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
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

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for DevicesWrite.xaml
    /// </summary>
    public partial class DevicesWrite : Window
    {
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        IModbusSerialMaster master;
        byte slave;
        ushort address;

        public DevicesWrite(byte slaveReceived, ushort addressReceived)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            master = ModbusSerialMaster.CreateRtu(VarsDevices.Port);
            slave = slaveReceived;
            address = addressReceived;

        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            send();
        }

        private void sendData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                send();
            }
        }

        private void send()
        {
            try
            {
                int.Parse(sendData.Text);
            }
            catch
            {
                myMessageQueue.Enqueue(rm.GetString("onlyInteger", cultureInfo), rm.GetString("ok", cultureInfo), () => HandleOKMethod());
                return;
            }
            
            try
            {
                master.WriteSingleRegister(slave, address, ushort.Parse(sendData.Text));
                MessageBox.Show(rm.GetString("writingSuccessful", cultureInfo), rm.GetString("system", cultureInfo),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (IOException) {

            }
            
        }

        private void HandleOKMethod()
        {
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
