using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for Projects.xaml
    /// </summary>
    public partial class Projects : UserControl
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DateTime dateNow, dateYearAgo, dateMonthAgo, dateWeekAgo;
        public SeriesCollection SeriesCollection { get; set; }

        public Projects()
        {
            InitializeComponent();
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

            dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            dateYearAgo = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy HH:mm:ss"));
            dateMonthAgo = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy HH:mm:ss"));
            dateWeekAgo = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy HH:mm:ss"));

            SqlCommand commandProject = new SqlCommand("SELECT dbo.countProjects()", localDbConnection);
            SqlCommand commandProjectsToDo = new SqlCommand("SELECT dbo.countProjectsToDo()", localDbConnection);
            SqlCommand commandProjectsYear = new SqlCommand("SELECT dbo.countProjectsByDate(@Year1, @Year2)", localDbConnection);
            SqlCommand commandProjectsMonth = new SqlCommand("SELECT dbo.countProjectsByDate(@Month1, @Month2)", localDbConnection);
            SqlCommand commandProjectsWeek = new SqlCommand("SELECT dbo.countProjectsByDate(@Week1, @Week2)", localDbConnection);

            commandProjectsYear.Parameters.AddWithValue("@Year1", dateYearAgo);
            commandProjectsYear.Parameters.AddWithValue("@Year2", dateNow);

            commandProjectsMonth.Parameters.AddWithValue("@Month1", dateMonthAgo);
            commandProjectsMonth.Parameters.AddWithValue("@Month2", dateNow);

            commandProjectsWeek.Parameters.AddWithValue("@Week1", dateWeekAgo);
            commandProjectsWeek.Parameters.AddWithValue("@Week2", dateNow);

            projectsAll.Content = commandProject.ExecuteScalar();
            projectsToDo.Content = commandProjectsToDo.ExecuteScalar();
            projectsYear.Content = commandProjectsYear.ExecuteScalar();
            projectsMonth.Content = commandProjectsMonth.ExecuteScalar();
            projectsWeek.Content = commandProjectsWeek.ExecuteScalar();

            SqlCommand commandLastProjects = new SqlCommand("SELECT * FROM dbo.projectsLast()", localDbConnection);
            SqlDataAdapter dataAdapterLastProjects = new SqlDataAdapter(commandLastProjects);
            DataTable dataTableLastProjects = new DataTable();
            dataAdapterLastProjects.Fill(dataTableLastProjects);

            for (int i = 0; i < dataTableLastProjects.Rows.Count; i++)
            {
                listViewProjects.Items.Add(new VarsLastProjects() { company = dataTableLastProjects.Rows[i][0].ToString(), project = dataTableLastProjects.Rows[i][1].ToString() });
                if (i == 4)
                    break;
            }

            SqlCommand commandProjectToDo = new SqlCommand("SELECT * FROM dbo.getProjectsToDo()", localDbConnection);
            SqlDataAdapter dataAdapterProjectsToDo = new SqlDataAdapter(commandProjectToDo);
            DataTable dataTableProjectsToDo = new DataTable();
            dataAdapterProjectsToDo.Fill(dataTableProjectsToDo);

            for (int i = 0; i < dataTableProjectsToDo.Rows.Count; i++)
            {
                listViewProjectsToDo.Items.Add(new VarsProjectsToDo() { projectToDo = dataTableProjectsToDo.Rows[i][1].ToString(), companyToDo = dataTableProjectsToDo.Rows[i][2].ToString() });
                if (i == 4)
                    break;
            }

            SeriesCollection = new SeriesCollection();

            SqlCommand commandProjectsDayByDay = new SqlCommand("SELECT * FROM dbo.countProjectsDayByDay(@date1, @date2) ORDER BY 1 ASC", localDbConnection);
            commandProjectsDayByDay.Parameters.AddWithValue("@date1", dateWeekAgo);
            commandProjectsDayByDay.Parameters.AddWithValue("@date2", dateNow);
            SqlDataAdapter dataAdapterProjectsDayByDay = new SqlDataAdapter(commandProjectsDayByDay);
            DataTable dataTableProjectsDayByDay = new DataTable();
            dataAdapterProjectsDayByDay.Fill(dataTableProjectsDayByDay);

            for (int i = 0; i < dataTableProjectsDayByDay.Rows.Count; i++)
            {
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Gün " + (i+1).ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableProjectsDayByDay.Rows[i][0].ToString())
                    }
                });
                if (i == 6)
                    break;
            }

            DataContext = this;
            localDbConnection.Close();
        }

        private void projectAllClicked(object sender, MouseButtonEventArgs e)
        {
            showProjectAll();
        }

        private void projectAllButtonClicked(object sender, RoutedEventArgs e)
        {
            showProjectAll();
        }

        private void projectToDoClicked(object sender, MouseButtonEventArgs e)
        {
            showProjectToDo();
        }

        private void projectsToDoButtonClicked(object sender, RoutedEventArgs e)
        {
            showProjectToDo();
        }
        private void projectsYearButtonClicked(object sender, RoutedEventArgs e)
        {
            showProjectByDate(dateNow, dateYearAgo);
        }
        private void projectYearClicked(object sender, MouseButtonEventArgs e)
        {
            showProjectByDate(dateNow, dateYearAgo);
        }

        private void projectMonthClicked(object sender, MouseButtonEventArgs e)
        {
            showProjectByDate(dateNow, dateMonthAgo);
        }

        private void projectsMonthButtonClicked(object sender, RoutedEventArgs e)
        {
            showProjectByDate(dateNow, dateMonthAgo);
        }

        private void projectWeekClicked(object sender, MouseButtonEventArgs e)
        {
            showProjectByDate(dateNow, dateWeekAgo);
        }

        private void projectsWeekButtonClicked(object sender, RoutedEventArgs e)
        {
            showProjectByDate(dateNow, dateWeekAgo);
        }

        private void companyGraph_DataClick(object sender, ChartPoint chartPoint)
        {

        }

        private void Flipper_OnIsFlippedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {

        }

        private void showProjectToDo()
        {
            ProjectsToDo projectsToDo = new ProjectsToDo();
            projectsToDo.ShowDialog();
        }

        private void showProjectAll()
        {
            ProjectAll projectAll = new ProjectAll();
            projectAll.ShowDialog();
        }

        private void showProjectByDate(DateTime date1, DateTime date2)
        {
            ProjectsByDate projectsByDate = new ProjectsByDate(date1, date2);
            projectsByDate.ShowDialog();
        }
    }
}
