using LiveCharts;
using LiveCharts.Wpf;
using Modbus.Device;
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
using System.Windows.Threading;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for DeviceWork2.xaml
    /// </summary>
    public partial class DeviceWork2 : Window
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        int i = 0;
        string db, deviceNameString;
        public SeriesCollection SeriesCollection { get; set; }
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        byte slave;

        public DeviceWork2(string dbName, int type, string device)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            deviceNameString = device;
            setDeviceSlave(type);
            db = dbName;
            init(dbName);

            dispatcherTimer.Tick += timer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Start();

            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        private void init(string dbName)
        {
            localDbConnection.Open();
            localDbConnectionUnity.Open();

            SqlCommand commandToGetInfo = new SqlCommand("SELECT * FROM dbo.getInformationForDeviceWork(@db)", localDbConnection);
            commandToGetInfo.Parameters.AddWithValue("@db", dbName);
            SqlDataAdapter dataAdapterInfo = new SqlDataAdapter(commandToGetInfo);
            DataTable dataTableInfo = new DataTable();
            dataAdapterInfo.Fill(dataTableInfo);

            deviceName.Content = dataTableInfo.Rows[0][0].ToString();
            operatorName.Content = dataTableInfo.Rows[0][1].ToString();
            companyName.Content = dataTableInfo.Rows[0][2].ToString();
            productName.Content = dataTableInfo.Rows[0][3].ToString();
            projectName.Content = dataTableInfo.Rows[0][4].ToString();

            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            string query = String.Format("SELECT COUNT(*) FROM {0}", db);
            SqlCommand command = new SqlCommand(query, localDbConnection);
            i = (int)command.ExecuteScalar();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Çap",
                    Values = new ChartValues<double> { }
                },
                new LineSeries
                {
                    Title = "Çap 2",
                    Values = new ChartValues<double> { }
                },
                new LineSeries
                {
                    Title = "Çap Set",
                    Values = new ChartValues<double> { }
                },
                new LineSeries
                {
                    Title = "Artı Tolerans",
                    Values = new ChartValues<double> { }
                },
                new LineSeries
                {
                    Title = "Artı Tolerans 2",
                    Values = new ChartValues<double> { }
                },
                new LineSeries
                {
                    Title = "Eksi Tolerans",
                    Values = new ChartValues<double> { }
                },
                new LineSeries
                {
                    Title = "Eksi Tolerans 2",
                    Values = new ChartValues<double> { }
                }
            };

            labelTime.Content = i.ToString();
            dispatcherTimer.Start();

            localDbConnection.Close();
            localDbConnectionUnity.Close();
            DataContext = this;
        }

        private void setDeviceSlave(int type)
        {
            switch (type)
            {
                case 1:
                    slave = Properties.Settings.Default.device1Slave;
                    break;
                case 2:
                    slave = Properties.Settings.Default.device2Slave;
                    break;
                case 3:
                    slave = Properties.Settings.Default.device3Slave;
                    break;
                case 4:
                    slave = Properties.Settings.Default.device4Slave;
                    break;
                case 5:
                    slave = Properties.Settings.Default.device5Slave;
                    break;
                case 6:
                    slave = Properties.Settings.Default.device6Slave;
                    break;
                case 7:
                    slave = Properties.Settings.Default.device7Slave;
                    break;
                case 8:
                    slave = Properties.Settings.Default.device8Slave;
                    break;
                case 9:
                    slave = Properties.Settings.Default.device9Slave;
                    break;
                case 10:
                    slave = Properties.Settings.Default.device10Slave;
                    break;
                case 11:
                    slave = Properties.Settings.Default.device11Slave;
                    break;
                case 12:
                    slave = Properties.Settings.Default.device12Slave;
                    break;
                case 13:
                    slave = Properties.Settings.Default.device13Slave;
                    break;
                case 14:
                    slave = Properties.Settings.Default.device14Slave;
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
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

            labelTime.Content = i.ToString();
        }

        private void finishProject_Click(object sender, RoutedEventArgs e)
        {
            if (deviceNameString == VarsDevices.device1Name)
                finishProject1();
            if (deviceNameString == VarsDevices.device2Name)
                finishProject2();
            if (deviceNameString == VarsDevices.device3Name)
                finishProject3();
            if (deviceNameString == VarsDevices.device4Name)
                finishProject4();
            if (deviceNameString == VarsDevices.device5Name)
                finishProject5();
            if (deviceNameString == VarsDevices.device6Name)
                finishProject6();
            if (deviceNameString == VarsDevices.device7Name)
                finishProject7();
            if (deviceNameString == VarsDevices.device8Name)
                finishProject8();
            if (deviceNameString == VarsDevices.device9Name)
                finishProject9();
            if (deviceNameString == VarsDevices.device10Name)
                finishProject10();
            if (deviceNameString == VarsDevices.device11Name)
                finishProject11();
            if (deviceNameString == VarsDevices.device12Name)
                finishProject12();
            if (deviceNameString == VarsDevices.device13Name)
                finishProject13();
            if (deviceNameString == VarsDevices.device14Name)
                finishProject14();
            this.Close();
        }

        private void finishProject1()
        {
            VarsDevices.device1Connected = false;
            VarsDevices.device1DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject2()
        {
            VarsDevices.device2Connected = false;
            VarsDevices.device2DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject3()
        {
            VarsDevices.device3Connected = false;
            VarsDevices.device3DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject4()
        {
            VarsDevices.device4Connected = false;
            VarsDevices.device4DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject5()
        {
            VarsDevices.device5Connected = false;
            VarsDevices.device5DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject6()
        {
            VarsDevices.device6Connected = false;
            VarsDevices.device6DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject7()
        {
            VarsDevices.device7Connected = false;
            VarsDevices.device7DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject8()
        {
            VarsDevices.device8Connected = false;
            VarsDevices.device8DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject9()
        {
            VarsDevices.device9Connected = false;
            VarsDevices.device9DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject10()
        {
            VarsDevices.device10Connected = false;
            VarsDevices.device10DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject11()
        {
            VarsDevices.device11Connected = false;
            VarsDevices.device11DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject12()
        {
            VarsDevices.device12Connected = false;
            VarsDevices.device12DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject13()
        {
            VarsDevices.device13Connected = false;
            VarsDevices.device13DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishProject14()
        {
            VarsDevices.device14Connected = false;
            VarsDevices.device14DbName = null;
            VarsDevices.deviceWorking -= 1;
            VarsDevices.deviceProjectReady -= 1;
            finishDb();
        }

        private void finishDb()
        {
            ProjectsFinishExplanation projectsFinishExplanation = new ProjectsFinishExplanation(db);
            projectsFinishExplanation.ShowDialog();
        }

        private void project1()
        {
            diameterText.Content = VariablesForDevice1.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice1.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice1.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice1.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice1.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice1.Length.ToString();
            display3Text.Content = VariablesForDevice1.Display3.ToString();
            NCText.Content = VariablesForDevice1.Nc.ToString();
            EvenText.Content = VariablesForDevice1.Even.ToString();
            ParpText.Content = VariablesForDevice1.Parp.ToString();
            PariText.Content = VariablesForDevice1.Pari.ToString();

            diameterText2.Content = VariablesForDevice1.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice1.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice1.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice1.XAxis.ToString();
            yaxis.Content = VariablesForDevice1.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice1.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice1.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice1.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice1.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project2()
        {
            diameterText.Content = VariablesForDevice2.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice2.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice2.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice2.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice2.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice2.Length.ToString();
            display3Text.Content = VariablesForDevice2.Display3.ToString();
            NCText.Content = VariablesForDevice2.Nc.ToString();
            EvenText.Content = VariablesForDevice2.Even.ToString();
            ParpText.Content = VariablesForDevice2.Parp.ToString();
            PariText.Content = VariablesForDevice2.Pari.ToString();

            diameterText2.Content = VariablesForDevice2.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice2.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice2.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice2.XAxis.ToString();
            yaxis.Content = VariablesForDevice2.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice2.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice2.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice2.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice2.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project3()
        {
            diameterText.Content = VariablesForDevice3.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice3.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice3.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice3.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice3.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice3.Length.ToString();
            display3Text.Content = VariablesForDevice3.Display3.ToString();
            NCText.Content = VariablesForDevice3.Nc.ToString();
            EvenText.Content = VariablesForDevice3.Even.ToString();
            ParpText.Content = VariablesForDevice3.Parp.ToString();
            PariText.Content = VariablesForDevice3.Pari.ToString();

            diameterText2.Content = VariablesForDevice3.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice3.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice3.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice3.XAxis.ToString();
            yaxis.Content = VariablesForDevice3.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice3.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice3.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice3.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice3.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project4()
        {
            diameterText.Content = VariablesForDevice4.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice4.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice4.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice4.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice4.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice4.Length.ToString();
            display3Text.Content = VariablesForDevice4.Display3.ToString();
            NCText.Content = VariablesForDevice4.Nc.ToString();
            EvenText.Content = VariablesForDevice4.Even.ToString();
            ParpText.Content = VariablesForDevice4.Parp.ToString();
            PariText.Content = VariablesForDevice4.Pari.ToString();

            diameterText2.Content = VariablesForDevice4.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice4.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice4.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice4.XAxis.ToString();
            yaxis.Content = VariablesForDevice4.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice4.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice4.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice4.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice4.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project5()
        {
            diameterText.Content = VariablesForDevice5.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice5.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice5.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice5.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice5.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice5.Length.ToString();
            display3Text.Content = VariablesForDevice5.Display3.ToString();
            NCText.Content = VariablesForDevice5.Nc.ToString();
            EvenText.Content = VariablesForDevice5.Even.ToString();
            ParpText.Content = VariablesForDevice5.Parp.ToString();
            PariText.Content = VariablesForDevice5.Pari.ToString();

            diameterText2.Content = VariablesForDevice5.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice5.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice5.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice5.XAxis.ToString();
            yaxis.Content = VariablesForDevice5.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice5.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice5.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice5.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice5.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project6()
        {
            diameterText.Content = VariablesForDevice6.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice6.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice6.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice6.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice6.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice6.Length.ToString();
            display3Text.Content = VariablesForDevice6.Display3.ToString();
            NCText.Content = VariablesForDevice6.Nc.ToString();
            EvenText.Content = VariablesForDevice6.Even.ToString();
            ParpText.Content = VariablesForDevice6.Parp.ToString();
            PariText.Content = VariablesForDevice6.Pari.ToString();

            diameterText2.Content = VariablesForDevice6.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice6.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice6.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice6.XAxis.ToString();
            yaxis.Content = VariablesForDevice6.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice6.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice6.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice6.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice6.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project7()
        {
            diameterText.Content = VariablesForDevice7.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice7.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice7.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice7.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice7.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice7.Length.ToString();
            display3Text.Content = VariablesForDevice7.Display3.ToString();
            NCText.Content = VariablesForDevice7.Nc.ToString();
            EvenText.Content = VariablesForDevice7.Even.ToString();
            ParpText.Content = VariablesForDevice7.Parp.ToString();
            PariText.Content = VariablesForDevice7.Pari.ToString();

            diameterText2.Content = VariablesForDevice7.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice7.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice7.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice7.XAxis.ToString();
            yaxis.Content = VariablesForDevice7.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice7.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice7.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice7.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice7.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project8()
        {
            diameterText.Content = VariablesForDevice8.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice8.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice8.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice8.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice8.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice8.Length.ToString();
            display3Text.Content = VariablesForDevice8.Display3.ToString();
            NCText.Content = VariablesForDevice8.Nc.ToString();
            EvenText.Content = VariablesForDevice8.Even.ToString();
            ParpText.Content = VariablesForDevice8.Parp.ToString();
            PariText.Content = VariablesForDevice8.Pari.ToString();

            diameterText2.Content = VariablesForDevice8.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice8.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice8.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice8.XAxis.ToString();
            yaxis.Content = VariablesForDevice8.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice8.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice8.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice8.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice8.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project9()
        {
            diameterText.Content = VariablesForDevice9.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice9.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice9.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice9.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice9.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice9.Length.ToString();
            display3Text.Content = VariablesForDevice9.Display3.ToString();
            NCText.Content = VariablesForDevice9.Nc.ToString();
            EvenText.Content = VariablesForDevice9.Even.ToString();
            ParpText.Content = VariablesForDevice9.Parp.ToString();
            PariText.Content = VariablesForDevice9.Pari.ToString();

            diameterText2.Content = VariablesForDevice9.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice9.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice9.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice9.XAxis.ToString();
            yaxis.Content = VariablesForDevice9.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice9.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice9.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice9.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice9.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project10()
        {
            diameterText.Content = VariablesForDevice10.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice10.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice10.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice10.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice10.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice10.Length.ToString();
            display3Text.Content = VariablesForDevice10.Display3.ToString();
            NCText.Content = VariablesForDevice10.Nc.ToString();
            EvenText.Content = VariablesForDevice10.Even.ToString();
            ParpText.Content = VariablesForDevice10.Parp.ToString();
            PariText.Content = VariablesForDevice10.Pari.ToString();

            diameterText2.Content = VariablesForDevice10.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice10.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice10.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice10.XAxis.ToString();
            yaxis.Content = VariablesForDevice10.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice10.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice10.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice10.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice10.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project11()
        {
            diameterText.Content = VariablesForDevice11.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice11.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice11.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice11.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice11.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice11.Length.ToString();
            display3Text.Content = VariablesForDevice11.Display3.ToString();
            NCText.Content = VariablesForDevice11.Nc.ToString();
            EvenText.Content = VariablesForDevice11.Even.ToString();
            ParpText.Content = VariablesForDevice11.Parp.ToString();
            PariText.Content = VariablesForDevice11.Pari.ToString();

            diameterText2.Content = VariablesForDevice11.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice11.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice11.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice11.XAxis.ToString();
            yaxis.Content = VariablesForDevice11.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice11.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice11.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice11.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice11.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project12()
        {
            diameterText.Content = VariablesForDevice12.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice12.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice12.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice12.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice12.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice12.Length.ToString();
            display3Text.Content = VariablesForDevice12.Display3.ToString();
            NCText.Content = VariablesForDevice12.Nc.ToString();
            EvenText.Content = VariablesForDevice12.Even.ToString();
            ParpText.Content = VariablesForDevice12.Parp.ToString();
            PariText.Content = VariablesForDevice12.Pari.ToString();

            diameterText2.Content = VariablesForDevice12.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice12.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice12.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice12.XAxis.ToString();
            yaxis.Content = VariablesForDevice12.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice12.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice12.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice12.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice12.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project13()
        {
            diameterText.Content = VariablesForDevice13.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice13.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice13.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice13.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice13.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice13.Length.ToString();
            display3Text.Content = VariablesForDevice13.Display3.ToString();
            NCText.Content = VariablesForDevice13.Nc.ToString();
            EvenText.Content = VariablesForDevice13.Even.ToString();
            ParpText.Content = VariablesForDevice13.Parp.ToString();
            PariText.Content = VariablesForDevice13.Pari.ToString();

            diameterText2.Content = VariablesForDevice13.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice13.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice13.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice13.XAxis.ToString();
            yaxis.Content = VariablesForDevice13.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice13.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice13.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice13.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice13.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }

        private void project14()
        {
            diameterText.Content = VariablesForDevice14.Diameter.ToString();
            diameterSetText.Content = VariablesForDevice14.DiameterSet.ToString();
            diameterDifferenceText.Content = VariablesForDevice14.DiameterDifference.ToString();
            plusToleranceText.Content = VariablesForDevice14.PlusTolerance.ToString();
            minusToleranceText.Content = VariablesForDevice14.MinusTolerance.ToString();
            metrajText.Content = VariablesForDevice14.Length.ToString();
            display3Text.Content = VariablesForDevice14.Display3.ToString();
            NCText.Content = VariablesForDevice14.Nc.ToString();
            EvenText.Content = VariablesForDevice14.Even.ToString();
            ParpText.Content = VariablesForDevice14.Parp.ToString();
            PariText.Content = VariablesForDevice14.Pari.ToString();

            diameterText2.Content = VariablesForDevice14.Diameter2.ToString();
            plusToleranceText2.Content = VariablesForDevice14.PlusTolerance2.ToString();
            minusToleranceText2.Content = VariablesForDevice14.MinusTolerance2.ToString();
            xaxis.Content = VariablesForDevice14.XAxis.ToString();
            yaxis.Content = VariablesForDevice14.YAxis.ToString();

            SeriesCollection[0].Values.Add(VariablesForDevice14.Diameter);
            SeriesCollection[1].Values.Add(VariablesForDevice14.DiameterSet);
            SeriesCollection[2].Values.Add(VariablesForDevice14.PlusTolerance);
            SeriesCollection[3].Values.Add(VariablesForDevice14.MinusTolerance);

            if (SeriesCollection[0].Values.Count > 50)
            {
                SeriesCollection[0].Values.RemoveAt(0);
                SeriesCollection[1].Values.RemoveAt(0);
                SeriesCollection[2].Values.RemoveAt(0);
                SeriesCollection[3].Values.RemoveAt(0);
            }
        }


        private void diameterSet_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("70"));
                devicesWrite.ShowDialog();
            }
        }

        private void plusTolerance_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("71"));
                devicesWrite.ShowDialog();
            }
        }

        private void plusTolerance2_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("71"));
                devicesWrite.ShowDialog();
            }
        }

        private void minusTolerance_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("72"));
                devicesWrite.ShowDialog();
            }
        }

        private void minusTolerance2_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("72"));
                devicesWrite.ShowDialog();
            }
        }

        private void display3_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("73"));
                devicesWrite.ShowDialog();
            }
        }

        private void nc_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("74"));
                devicesWrite.ShowDialog();
            }
        }

        private void even_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("75"));
                devicesWrite.ShowDialog();
            }
        }

        private void diameterDifference_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("69"));
                devicesWrite.ShowDialog();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void parp_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("76"));
                devicesWrite.ShowDialog();
            }
        }

        private void pari_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.writing)
            {
                DevicesWrite devicesWrite = new DevicesWrite(slave, ushort.Parse("77"));
                devicesWrite.ShowDialog();
            }
        }
    }
}
