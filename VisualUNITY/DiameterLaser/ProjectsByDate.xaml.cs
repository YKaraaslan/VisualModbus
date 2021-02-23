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
    /// Interaction logic for ProjectYear.xaml
    /// </summary>
    public partial class ProjectsByDate : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsProjectsAll> projectsAll = new List<VarsProjectsAll>();
        List<VarsProjectsAll> filteredList = new List<VarsProjectsAll>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        DateTime dt1, dt2;

        public ProjectsByDate(DateTime dateTime1, DateTime dateTime2)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            dt1 = dateTime1; dt2 = dateTime2;
            init(dateTime1, dateTime2);
        }

        private void init(DateTime dateTime1, DateTime dateTime2)
        {
            localDbConnection.Open();

            SqlCommand commandProject = new SqlCommand("SELECT * FROM dbo.getProjectsByDate(@Param1, @Param2)", localDbConnection);

            commandProject.Parameters.AddWithValue("@Param1", dateTime2);
            commandProject.Parameters.AddWithValue("@Param2", dateTime1);

            SqlDataAdapter dataAdapterProjects = new SqlDataAdapter(commandProject);
            DataTable dataTableProjects = new DataTable();
            dataAdapterProjects.Fill(dataTableProjects);

            for (int i = 0; i < dataTableProjects.Rows.Count; i++)
            {
                projectsAll.Add(new VarsProjectsAll()
                {
                    id = dataTableProjects.Rows[i][0].ToString(),
                    project = dataTableProjects.Rows[i][1].ToString(),
                    company = dataTableProjects.Rows[i][2].ToString(),
                    product = dataTableProjects.Rows[i][3].ToString(),
                    device = dataTableProjects.Rows[i][4].ToString(),
                    operators = dataTableProjects.Rows[i][5].ToString(),
                    explanation = dataTableProjects.Rows[i][6].ToString(),
                    dateCreated = dataTableProjects.Rows[i][7].ToString(),
                    dateDue = dataTableProjects.Rows[i][8].ToString(),
                    dateFinished = dataTableProjects.Rows[i][9].ToString(),
                    explanationFinished = dataTableProjects.Rows[i][10].ToString()
                });
            }
            listViewProjects.ItemsSource = projectsAll;
            localDbConnection.Close();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredList.Clear();

            if (search.Text.Equals(""))
            {
                filteredList.AddRange(projectsAll);
            }
            else
            {
                foreach (VarsProjectsAll varsProjectAll in projectsAll)
                {

                    if (varsProjectAll.project.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.company.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.product.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.device.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.operators.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.explanation.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.explanationFinished.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateCreated.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateDue.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateFinished.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void projectAdd_Closed(object sender, EventArgs e)
        {
            search.Text = "";
            init(dt1, dt2);
        }
    }
}
