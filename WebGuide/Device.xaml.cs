using System;
using System.Collections.Generic;
using System.IO;
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
using Modbus.Device;

namespace WebGuide
{
    /// <summary>
    /// Interaction logic for Device.xaml
    /// </summary>
    public partial class Device : UserControl
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer rightTimer = new DispatcherTimer();
        DispatcherTimer leftTimer = new DispatcherTimer();
        DispatcherTimer upTimer = new DispatcherTimer();
        DispatcherTimer downTimer = new DispatcherTimer();
        SerialPort connection;
        IModbusSerialMaster master;
        byte slave = 247;

        public Device()
        {
            InitializeComponent();
            dispatcherTimer.Tick += timer1;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatcherTimer.Start();

            rightTimer.Tick += rightTimerWork;
            leftTimer.Tick += leftTimerWork;
            upTimer.Tick += upTimerWork;
            downTimer.Tick += downTimerWork;

            rightTimer.Interval = TimeSpan.FromMilliseconds(10);
            leftTimer.Interval = TimeSpan.FromMilliseconds(10);
            upTimer.Interval = TimeSpan.FromMilliseconds(0);
            downTimer.Interval = TimeSpan.FromMilliseconds(10);

            connection = new SerialPort();
            init();
        }

        private void timer1(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int integer = 150 + rnd.Next(1, 100);
            rectangle.Margin = new Thickness(integer, 66, 0, 0);

            if (integer <= 165)
            {
                ellipse1.Visibility = Visibility.Visible;
                ellipse2.Visibility = Visibility.Visible;
                ellipse3.Visibility = Visibility.Visible;
                ellipse4.Visibility = Visibility.Visible;
                ellipse5.Visibility = Visibility.Visible;
            }
            else if (integer > 165 && integer <= 180)
            {
                ellipse1.Visibility = Visibility.Visible;
                ellipse2.Visibility = Visibility.Visible;
                ellipse3.Visibility = Visibility.Visible;
                ellipse4.Visibility = Visibility.Visible;
                ellipse5.Visibility = Visibility.Hidden;
            }
            else if (integer > 180 && integer <= 195)
            {
                ellipse1.Visibility = Visibility.Visible;
                ellipse2.Visibility = Visibility.Visible;
                ellipse3.Visibility = Visibility.Visible;
                ellipse4.Visibility = Visibility.Hidden;
                ellipse5.Visibility = Visibility.Hidden;
            }
            else if (integer > 195 && integer <= 210)
            {
                ellipse1.Visibility = Visibility.Visible;
                ellipse2.Visibility = Visibility.Visible;
                ellipse3.Visibility = Visibility.Hidden;
                ellipse4.Visibility = Visibility.Hidden;
                ellipse5.Visibility = Visibility.Hidden;
            }
            else if (integer > 210 && integer <= 225)
            {
                ellipse1.Visibility = Visibility.Visible;
                ellipse2.Visibility = Visibility.Hidden;
                ellipse3.Visibility = Visibility.Hidden;
                ellipse4.Visibility = Visibility.Hidden;
                ellipse5.Visibility = Visibility.Hidden;
            }
            else
            {
                ellipse1.Visibility = Visibility.Hidden;
                ellipse2.Visibility = Visibility.Hidden;
                ellipse3.Visibility = Visibility.Hidden;
                ellipse4.Visibility = Visibility.Hidden;
                ellipse5.Visibility = Visibility.Hidden;
            }
        }

        private void init()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                connection.PortName = ports[0];
                connection.BaudRate = 9600;
                connection.Parity = Parity.Even;
                connection.DataBits = 8;
                connection.StopBits = StopBits.One;
                connection.ReadTimeout = 2000;
                connection.Open();
                master = ModbusSerialMaster.CreateRtu(connection);
                ellipseConnected.Fill = Brushes.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ellipseConnected_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //upTimer.Start();
            ushort adr = 18;
            ushort adrd = 2;
            ushort[] registers = master.ReadHoldingRegisters(slave, adr, adrd);
            label.Content = registers[0].ToString();
            //MessageBox.Show(registers[0].ToString());
            //MessageBox.Show(registers[1].ToString());
            //connection.Close();
            //init();
        }

        int i = 0;

        private void sensorStatus_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            label.Content = i++.ToString();
            //master.WriteSingleRegister(slave, 32, 1);
        }

        private void autoManual_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            master.WriteSingleRegister(slave, 31, 1);
        }

        private void set_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            master.WriteSingleRegister(slave, 33, 1);
        }

        private void center_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            master.WriteSingleRegister(slave, 36, 1);
        }

        private void up_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            upTimer.Start();
        }

        private void up_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            upTimer.Stop();
        }

        private void down_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            downTimer.Start();
        }

        private void down_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            downTimer.Stop();
        }

        private void left_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            leftTimer.Start();
        }

        private void left_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftTimer.Stop();
        }

        private void right_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //modbus.WriteSingleRegister(247, 35, 1);
            rightTimer.Start();
        }

        private void right_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            rightTimer.Stop();
        }

        private void downTimerWork(object sender, EventArgs e)
        {
            master.WriteSingleRegister(slave, 10, ushort.Parse("10"));
        }

        int cnt = 0;

        private void upTimerWork(object sender, EventArgs e)
        {
            //try
            //{
            //    ushort[] registers = master.ReadHoldingRegisters(247, ushort.Parse(cnt.ToString()), 3);
            //    MessageBox.Show(cnt.ToString() + registers[0].ToString());
            //    label.Content = registers[0].ToString();
            //}
            //catch (NotImplementedException) { }
            //catch (IOException) { }
            //cnt++;
        }

        private void leftTimerWork(object sender, EventArgs e)
        {
            master.WriteSingleRegister(slave, 10, ushort.Parse("10"));
        }

        private void rightTimerWork(object sender, EventArgs e)
        {
            label.Content = i++.ToString();
        }
    }
}
