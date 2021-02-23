using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Interaction logic for ProjectOpen.xaml
    /// </summary>
    public partial class ProjectOpen : Window
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsProjectsToDo> projectsToDo = new List<VarsProjectsToDo>();
        List<VarsProjectsToDo> filteredList = new List<VarsProjectsToDo>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        string operators, deviceId, deviceDbName, dbName, deviceNameReceived, operatorName;
        int deviceType;

        public ProjectOpen(string operatorName, string idReceived, int deviceTypeReceived)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            operators = operatorName;
            deviceId = idReceived;
            deviceType = deviceTypeReceived;
            init();
        }

        private void init()
        {
            localDbConnection.Open();

            SqlCommand commandProject = new SqlCommand("SELECT * FROM dbo.getProjectsToDo()", localDbConnection);

            SqlDataAdapter dataAdapterProjects = new SqlDataAdapter(commandProject);
            DataTable dataTableProjects = new DataTable();
            dataAdapterProjects.Fill(dataTableProjects);

            projectsToDo.Clear();

            for (int i = 0; i < dataTableProjects.Rows.Count; i++)
            {
                projectsToDo.Add(new VarsProjectsToDo()
                {
                    id = dataTableProjects.Rows[i][0].ToString(),
                    projectToDo = dataTableProjects.Rows[i][1].ToString(),
                    companyToDo = dataTableProjects.Rows[i][2].ToString(),
                    productToDo = dataTableProjects.Rows[i][3].ToString(),
                    deviceToDo = dataTableProjects.Rows[i][4].ToString(),
                    operatorsToDo = dataTableProjects.Rows[i][5].ToString(),
                    explanationToDo = dataTableProjects.Rows[i][6].ToString(),
                    dateCreatedToDo = dataTableProjects.Rows[i][7].ToString(),
                    dateDueToDo = dataTableProjects.Rows[i][8].ToString()
                });
            }
            listViewProjects.ItemsSource = projectsToDo;
            localDbConnection.Close();
        }

        private void projectSelect_Click(object sender, RoutedEventArgs e)
        {
            if (listViewProjects.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
            VarsProjectsToDo varsProject = (VarsProjectsToDo)listViewProjects.SelectedItem;
            dbName = getDbName(int.Parse(varsProject.id));

            string[] vars = { VarsDevices.device1DbName, VarsDevices.device2DbName, VarsDevices.Device3DbName,
            VarsDevices.Device4DbName, VarsDevices.Device5DbName, VarsDevices.Device6DbName, VarsDevices.Device7DbName,
            VarsDevices.Device8DbName, VarsDevices.Device9DbName, VarsDevices.Device10DbName, VarsDevices.Device11DbName,
            VarsDevices.Device12DbName, VarsDevices.Device13DbName, VarsDevices.Device14DbName };

            for (int i = 0; i < vars.Length; i++)
            {
                if (vars[i] == dbName)
                {
                    myMessageQueue.Enqueue(rm.GetString("projectAlreadySelected"), rm.GetString("ok"), () => HandleOKMethod());
                    return;
                }
            }
            
            checkDevice(int.Parse(varsProject.id));
            switch (int.Parse(deviceId))
            {
                case 0:
                    VarsDevices.Device1DbName = dbName;
                    break;
                case 1:
                    VarsDevices.Device2DbName = dbName;
                    break;
                case 2:
                    VarsDevices.Device3DbName = dbName;
                    break;
                case 3:
                    VarsDevices.Device4DbName = dbName;
                    break;
                case 4:
                    VarsDevices.Device5DbName = dbName;
                    break;
                case 5:
                    VarsDevices.Device6DbName = dbName;
                    break;
                case 6:
                    VarsDevices.Device7DbName = dbName;
                    break;
                case 7:
                    VarsDevices.Device8DbName = dbName;
                    break;
                case 8:
                    VarsDevices.Device9DbName = dbName;
                    break;
                case 9:
                    VarsDevices.Device10DbName = dbName;
                    break;
                case 10:
                    VarsDevices.Device11DbName = dbName;
                    break;
                case 11:
                    VarsDevices.Device12DbName = dbName;
                    break;
                case 12:
                    VarsDevices.Device13DbName = dbName;
                    break;
                case 13:
                    VarsDevices.Device14DbName = dbName;
                    break;
            }
            VarsDevices.deviceProjectReady += 1;
            this.Close();
            if (operatorName != operators)
            {
                DeviceShowOperator deviceOperatorShow = new DeviceShowOperator(operators, int.Parse(varsProject.id));
                deviceOperatorShow.ShowDialog();
            }
        }

        private void projectCancel_Click(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(deviceId))
            {
                case 0:
                    if (VarsDevices.Device1DbName != null)
                    {
                        VarsDevices.Device1DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 1:
                    if (VarsDevices.Device2DbName != null)
                    {
                        VarsDevices.Device2DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 2:
                    if (VarsDevices.Device3DbName != null)
                    {
                        VarsDevices.Device3DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 3:
                    if (VarsDevices.Device4DbName != null)
                    {
                        VarsDevices.Device4DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 4:
                    if (VarsDevices.Device5DbName != null)
                    {
                        VarsDevices.Device5DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 5:
                    if (VarsDevices.Device6DbName != null)
                    {
                        VarsDevices.Device6DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 6:
                    if (VarsDevices.Device7DbName != null)
                    {
                        VarsDevices.Device7DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 7:
                    if (VarsDevices.Device8DbName != null)
                    {
                        VarsDevices.Device8DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 8:
                    if (VarsDevices.Device9DbName != null)
                    {
                        VarsDevices.Device9DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 9:
                    if (VarsDevices.Device10DbName != null)
                    {
                        VarsDevices.Device10DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 10:
                    if (VarsDevices.Device11DbName != null)
                    {
                        VarsDevices.Device11DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 11:
                    if (VarsDevices.Device12DbName != null)
                    {
                        VarsDevices.Device12DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 12:
                    if (VarsDevices.Device13DbName != null)
                    {
                        VarsDevices.Device13DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
                case 13:
                    if (VarsDevices.Device14DbName != null)
                    {
                        VarsDevices.Device14DbName = null;
                        VarsDevices.deviceProjectReady -= 1;
                    }
                    break;
            }
            this.Close();
        }

        private void checkDevice(int projectID)
        {
            localDbConnectionUnity.Open();
            SqlCommand commandToGetDb = new SqlCommand("SELECT * FROM dbo.getDeviceName(@id)", localDbConnectionUnity);
            commandToGetDb.Parameters.AddWithValue("@id", int.Parse(deviceId)+1);
            SqlDataAdapter dataAdapterDb = new SqlDataAdapter(commandToGetDb);
            DataTable dataTableDb = new DataTable();
            dataAdapterDb.Fill(dataTableDb);
            localDbConnectionUnity.Close();
            deviceNameReceived = dataTableDb.Rows[0][0].ToString();
            if (deviceNameReceived != deviceDbName)
            {
                updateProjectsDevice(deviceNameReceived, projectID);
                updateDataBaseByAxis(deviceType);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void updateDataBaseByAxis(int deviceType)
        {
            localDbConnection.Open();

            SqlCommand commandToUpdate = new SqlCommand("dbo.recreateTable", localDbConnection);

            commandToUpdate.CommandType = CommandType.StoredProcedure;
            commandToUpdate.Parameters.AddWithValue("@DatabaseName", dbName);
            commandToUpdate.Parameters.AddWithValue("@DeviceType", deviceType);
            commandToUpdate.ExecuteNonQuery();

            localDbConnection.Close();
        }

        private void updateProjectsDevice(string deviceDbName, int idDb)
        {
            localDbConnection.Open();

            SqlCommand commandToUpdate = new SqlCommand("dbo.updateDeviceInProjectsToDo", localDbConnection);

            commandToUpdate.CommandType = CommandType.StoredProcedure;
            commandToUpdate.Parameters.AddWithValue("@id", idDb);
            commandToUpdate.Parameters.AddWithValue("@device", deviceDbName);
            commandToUpdate.Parameters.AddWithValue("@deviceType", deviceType);
            commandToUpdate.ExecuteNonQuery();

            localDbConnection.Close();
        }

        private void HandleOKMethod()
        {
            
        }

        private string getDbName(int idDb)
        {
            localDbConnection.Open();
            SqlCommand commandToGetDb = new SqlCommand("SELECT * FROM dbo.getDbName(@id)", localDbConnection);
            commandToGetDb.Parameters.AddWithValue("@id", idDb);
            SqlDataAdapter dataAdapterDb = new SqlDataAdapter(commandToGetDb);
            DataTable dataTableDb = new DataTable();
            dataAdapterDb.Fill(dataTableDb);
            localDbConnection.Close();
            dbName = dataTableDb.Rows[0][0].ToString();
            deviceDbName = dataTableDb.Rows[0][1].ToString();
            operatorName = dataTableDb.Rows[0][2].ToString();
            return dbName;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredList.Clear();

            if (search.Text.Equals(""))
            {
                filteredList.AddRange(projectsToDo);
            }
            else
            {
                foreach (VarsProjectsToDo varsProjectToDo in projectsToDo)
                {

                    if (varsProjectToDo.projectToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }

                    if (varsProjectToDo.companyToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }

                    if (varsProjectToDo.productToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }

                    if (varsProjectToDo.deviceToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }

                    if (varsProjectToDo.operatorsToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }

                    if (varsProjectToDo.explanationToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }

                    if (varsProjectToDo.dateCreatedToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }

                    if (varsProjectToDo.dateDueToDo.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectToDo);
                        continue;
                    }
                }
            }
            listViewProjects.ItemsSource = filteredList.ToList();
        }

        private void projectAdd_Click(object sender, RoutedEventArgs e)
        {
            ProjectsCreate projectsCreate = new ProjectsCreate(operators, deviceId);
            projectsCreate.ShowDialog();
        }
    }
}
