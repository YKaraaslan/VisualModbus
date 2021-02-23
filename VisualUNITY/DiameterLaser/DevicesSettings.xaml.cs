using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO.Ports;
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
    /// Interaction logic for DevicesSettings.xaml
    /// </summary>
    public partial class DevicesSettings : Window
    {
        SqlConnection localDbConnection, localDbConnectionProject;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        int deviceId;
        string oldName;

        public DevicesSettings(string deviceNameString, string deviceOperator, int id)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            string pathToDBFileProject = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionProject = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileProject + ";Integrated Security=True");
            deviceId = id;
            oldName = deviceNameString;
            init(deviceNameString, deviceOperator);
        }
        private void init(string deviceNameString, string deviceOperator)
        {
            deviceName.Text = deviceNameString;

            SqlCommand commandOperators = new SqlCommand("SELECT * FROM dbo.getOperators()", localDbConnection);
            SqlDataAdapter dataAdapterOperators = new SqlDataAdapter(commandOperators);
            DataTable dataTableOperators = new DataTable();
            dataAdapterOperators.Fill(dataTableOperators);

            for (int i = 0; i < Properties.Settings.Default.operatorAmount; i++)
            {
                operatorName.Items.Add(dataTableOperators.Rows[i][0].ToString());
            }
            operatorName.SelectedItem = deviceOperator;

            string devicePort = getDevicePort(deviceNameString);

            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    port.Items.Add(ports[i]);
                }
            }
            port.Text = devicePort;
        }

        private void save(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void deviceName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                update();
            }
        }

        private void update()
        {
            if (deviceName.Text.Trim() == "")
            {
                myMessageQueue.Enqueue("Cihaz adı giriniz", "TAMAM", () => HandleOKMethod());
                return;
            }

            localDbConnection.Open();

            SqlCommand checkDeviceName = new SqlCommand("SELECT dbo.checkDeviceName(@Device)", localDbConnection);
            checkDeviceName.Parameters.AddWithValue("@Device", deviceName.Text.Trim());

            if ((int)checkDeviceName.ExecuteScalar() > 0 && deviceName.Text.Trim() != oldName)
            {
                myMessageQueue.Enqueue(rm.GetString("deviceWithTheSameNameExists", cultureInfo), rm.GetString("ok", cultureInfo), () => HandleOKMethod());
                localDbConnection.Close();
                return;
            }

            SqlCommand commandToAddDatabase = new SqlCommand("dbo.updateDevice", localDbConnection);

            commandToAddDatabase.CommandType = CommandType.StoredProcedure;
            commandToAddDatabase.Parameters.AddWithValue("@id", deviceId);
            commandToAddDatabase.Parameters.AddWithValue("@newName", deviceName.Text.Trim());
            commandToAddDatabase.Parameters.AddWithValue("@operatorName", operatorName.Text.Trim());
            commandToAddDatabase.ExecuteNonQuery();

            localDbConnection.Close();

            if(port.Text.Trim() == null || port.Text.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("portIsEmpty", cultureInfo), rm.GetString("ok", cultureInfo), () => HandleOKMethod());
                return;
            }

            setDevicePort(oldName);
            Properties.Settings.Default.Save();
            MessageBox.Show(rm.GetString("deviceInformationUpdated", cultureInfo), rm.GetString("system", cultureInfo),
                        MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void HandleOKMethod()
        {

        }

        private string getDevicePort(string deviceNameString)
        {
            string value = "";
            if (deviceNameString == VarsDevices.device1Name)
                value = device1();
            if (deviceNameString == VarsDevices.device2Name)
                value = device2();
            if (deviceNameString == VarsDevices.device3Name)
                value = device3();
            if (deviceNameString == VarsDevices.device4Name)
                value = device4();
            if (deviceNameString == VarsDevices.device5Name)
                value = device5();
            if (deviceNameString == VarsDevices.device6Name)
                value = device6();
            if (deviceNameString == VarsDevices.device7Name)
                value = device7();
            if (deviceNameString == VarsDevices.device8Name)
                value = device8();
            if (deviceNameString == VarsDevices.device9Name)
                value = device9();
            if (deviceNameString == VarsDevices.device10Name)
                value = device10();
            if (deviceNameString == VarsDevices.device11Name)
                value = device11();
            if (deviceNameString == VarsDevices.device12Name)
                value = device12();
            if (deviceNameString == VarsDevices.device13Name)
                value = device13();
            if (deviceNameString == VarsDevices.device14Name)
                value = device14();

            return value;
        }

        private string device1()
        {
            return Properties.Settings.Default.device1Port;
        }

        private string device2()
        {
            return Properties.Settings.Default.device2Port;
        }

        private string device3()
        {
            return Properties.Settings.Default.device3Port;
        }

        private string device4()
        {
            return Properties.Settings.Default.device4Port;
        }

        private string device5()
        {
            return Properties.Settings.Default.device5Port;
        }

        private string device6()
        {
            return Properties.Settings.Default.device6Port;
        }

        private string device7()
        {
            return Properties.Settings.Default.device7Port;
        }

        private string device8()
        {
            return Properties.Settings.Default.device8Port;
        }

        private string device9()
        {
            return Properties.Settings.Default.device9Port;
        }

        private string device10()
        {
            return Properties.Settings.Default.device10Port;
        }

        private string device11()
        {
            return Properties.Settings.Default.device11Port;
        }

        private string device12()
        {
            return Properties.Settings.Default.device11Port;
        }

        private string device13()
        {
            return Properties.Settings.Default.device12Port;
        }

        private string device14()
        {
            return Properties.Settings.Default.device13Port;
        }

        private void setDevicePort(string deviceNameString)
        {
            if (deviceNameString == VarsDevices.device1Name)
                project1();
            if (deviceNameString == VarsDevices.device2Name)
                project2();
            if (deviceNameString == VarsDevices.device3Name)
                project3();
            if (deviceNameString == VarsDevices.device4Name)
                project4();
            if (deviceNameString == VarsDevices.device5Name)
                project5();
            if (deviceNameString == VarsDevices.device6Name)
                project6();
            if (deviceNameString == VarsDevices.device7Name)
                project7();
            if (deviceNameString == VarsDevices.device8Name)
                project8();
            if (deviceNameString == VarsDevices.device9Name)
                project9();
            if (deviceNameString == VarsDevices.device10Name)
                project10();
            if (deviceNameString == VarsDevices.device11Name)
                project11();
            if (deviceNameString == VarsDevices.device12Name)
                project12();
            if (deviceNameString == VarsDevices.device13Name)
                project13();
            if (deviceNameString == VarsDevices.device14Name)
                project14();
        }

        private void project1()
        {
            Properties.Settings.Default.device1Port = port.Text.Trim();
        }

        private void project2()
        {
            Properties.Settings.Default.device2Port = port.Text.Trim();
        }

        private void project3()
        {
            Properties.Settings.Default.device3Port = port.Text.Trim();
        }

        private void project4()
        {
            Properties.Settings.Default.device4Port = port.Text.Trim();
        }

        private void project5()
        {
            Properties.Settings.Default.device5Port = port.Text.Trim();
        }

        private void project6()
        {
            Properties.Settings.Default.device6Port = port.Text.Trim();
        }

        private void project7()
        {
            Properties.Settings.Default.device7Port = port.Text.Trim();
        }

        private void project8()
        {
            Properties.Settings.Default.device8Port = port.Text.Trim();
        }

        private void project9()
        {
            Properties.Settings.Default.device9Port = port.Text.Trim();
        }

        private void project10()
        {
            Properties.Settings.Default.device10Port = port.Text.Trim();
        }

        private void project11()
        {
            Properties.Settings.Default.device11Port = port.Text.Trim();
        }

        private void project12()
        {
            Properties.Settings.Default.device12Port = port.Text.Trim();
        }

        private void project13()
        {
            Properties.Settings.Default.device13Port = port.Text.Trim();
        }

        private void project14()
        {
            Properties.Settings.Default.device14Port = port.Text.Trim();
        }
    }
}
