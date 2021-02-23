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
    /// Interaction logic for SearchProjectsToDoOpen.xaml
    /// </summary>
    public partial class SearchProjectsToDoOpen : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsProjectsToDo> projectsToDo = new List<VarsProjectsToDo>();
        List<VarsProjectsToDo> filteredList = new List<VarsProjectsToDo>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public SearchProjectsToDoOpen(string key)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            init(key);
        }

        private void init(string key)
        {
            localDbConnection.Open();

            SqlCommand commandProject = new SqlCommand("SELECT * FROM dbo.getProjectsToDoForSearch(@key)", localDbConnection);
            commandProject.Parameters.AddWithValue("@key", key);

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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
