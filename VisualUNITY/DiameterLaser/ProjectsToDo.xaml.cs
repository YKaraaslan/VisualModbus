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
    /// Interaction logic for ProjectsToDo.xaml
    /// </summary>
    public partial class ProjectsToDo : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsProjectsToDo> projectsToDo = new List<VarsProjectsToDo>();
        List<VarsProjectsToDo> filteredList = new List<VarsProjectsToDo>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public ProjectsToDo()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
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
            ProjectsCreate projectsCreate = new ProjectsCreate(null, null);
            projectsCreate.Closed += projectAdd_Closed;
            projectsCreate.ShowDialog();
        }

        private void projectAdd_Closed(object sender, EventArgs e)
        {
            search.Text = "";
            init();
        }

        private void projectDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listViewProjects.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
            VarsProjectsToDo varsProject = (VarsProjectsToDo)listViewProjects.SelectedItem;

            string[] vars = { VarsDevices.device1DbName, VarsDevices.device2DbName, VarsDevices.Device3DbName,
            VarsDevices.Device4DbName, VarsDevices.Device5DbName, VarsDevices.Device6DbName, VarsDevices.Device7DbName,
            VarsDevices.Device8DbName, VarsDevices.Device9DbName, VarsDevices.Device10DbName, VarsDevices.Device11DbName,
            VarsDevices.Device12DbName, VarsDevices.Device13DbName, VarsDevices.Device14DbName };

            for (int i = 0; i < vars.Length; i++)
            {
                if (vars[i] == getDbName(int.Parse(varsProject.id)))
                {
                    MessageBox.Show(rm.GetString("projectCannotBeDeleted"), rm.GetString("system"),
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (MessageBox.Show(rm.GetString("areYouSureToDelete", cultureInfo), rm.GetString("system", cultureInfo),
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                deleteSelectedRow();
            }
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
            string dbName = dataTableDb.Rows[0][0].ToString();
            return dbName;
        }

        private void HandleOKMethod()
        {
            
        }

        private void deleteSelectedRow()
        {
            VarsProjectsToDo varsProjectsToDo = (VarsProjectsToDo)listViewProjects.SelectedItem;

            AdminConfirmation adminConfirmation = new AdminConfirmation(int.Parse(varsProjectsToDo.id), "project", null);
            adminConfirmation.Closed += adminConfirmationClosed;
            this.Close();
            adminConfirmation.ShowDialog();
        }

        private void adminConfirmationClosed(object sender, EventArgs e)
        {
            ProjectsToDo projectsToDo = new ProjectsToDo();
            projectsToDo.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
