using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DateTime dateNow, dateYearAgo, dateMonthAgo, dateWeekAgo;
        //SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        DockPanel dockPanel;

        public Dashboard(DockPanel panel)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            dockPanel = panel;
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            init();
        }

        private void init()
        {
            localDbConnection.Open();
            localDbConnectionUnity.Open();

            dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            dateYearAgo = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy HH:mm:ss"));
            dateMonthAgo = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy HH:mm:ss"));
            dateWeekAgo = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy HH:mm:ss"));

            SqlCommand commandProject = new SqlCommand("SELECT dbo.countProjects()", localDbConnection);
            SqlCommand commandProjectsToDo = new SqlCommand("SELECT dbo.countProjectsToDo()", localDbConnection);
            SqlCommand commandProjectsYear = new SqlCommand("SELECT dbo.countProjectsByDate(@Year1, @Year2)", localDbConnection);
            SqlCommand commandProjectsMonth = new SqlCommand("SELECT dbo.countProjectsByDate(@Month1, @Month2)", localDbConnection);
            SqlCommand commandProjectsWeek = new SqlCommand("SELECT dbo.countProjectsByDate(@Week1, @Week2)", localDbConnection);
            SqlCommand commandProducts = new SqlCommand("SELECT dbo.productAmount()", localDbConnectionUnity);
            SqlCommand commandCustomers = new SqlCommand("SELECT dbo.customerAmount()", localDbConnectionUnity);

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
            productAmount.Content = commandProducts.ExecuteScalar();
            customerAmount.Content = commandCustomers.ExecuteScalar();

            deviceAmount.Content = Properties.Settings.Default.deviceAmount.ToString();
            deviceWorking.Content = VarsDevices.DeviceWorking.ToString();
            deviceProjectReady.Content = VarsDevices.DeviceProjectReady.ToString();

            SqlCommand commandCompaniesGroup = new SqlCommand("SELECT * FROM dbo.companiesGroup() ORDER BY 2 DESC", localDbConnection);
            SqlDataAdapter dataAdapterCompaniesGroup = new SqlDataAdapter(commandCompaniesGroup);
            DataTable dataTableCompaniesGroup = new DataTable();
            dataAdapterCompaniesGroup.Fill(dataTableCompaniesGroup);

            SeriesCollection = new SeriesCollection();
            Labels = new string[5];
            listViewCompany.Items.Clear();
            for (int i = 0; i < dataTableCompaniesGroup.Rows.Count; i++)
            {
                listViewCompany.Items.Add(new VarsCompanies() { company = dataTableCompaniesGroup.Rows[i][0].ToString(), ID = int.Parse(dataTableCompaniesGroup.Rows[i][1].ToString()) });
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = dataTableCompaniesGroup.Rows[i][0].ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableCompaniesGroup.Rows[i][1].ToString())
                     }
                });
                if (i == 4)
                    break;
            }

            SqlCommand commandLastProjects = new SqlCommand("SELECT * FROM dbo.projectsLast()", localDbConnection);
            SqlDataAdapter dataAdapterLastProjects = new SqlDataAdapter(commandLastProjects);
            DataTable dataTableLastProjects = new DataTable();
            dataAdapterLastProjects.Fill(dataTableLastProjects);
            listViewLastProjects.Items.Clear();
            for (int i = 0; i < dataTableLastProjects.Rows.Count; i++)
            {
                listViewLastProjects.Items.Add(new VarsLastProjects() { company = dataTableLastProjects.Rows[i][0].ToString(), project = dataTableLastProjects.Rows[i][1].ToString() });
                if (i == 4)
                    break;
            }

            SqlCommand commandOperators = new SqlCommand("SELECT * FROM dbo.operatorsDashboard() ORDER BY 2 DESC", localDbConnection);
            SqlDataAdapter dataAdapterOperators = new SqlDataAdapter(commandOperators);
            DataTable dataTableOperators = new DataTable();
            dataAdapterOperators.Fill(dataTableOperators);
            listViewOperators.Items.Clear();
            for (int i = 0; i < dataTableOperators.Rows.Count; i++)
            {
                listViewOperators.Items.Add(new VarsOperators() { name = dataTableOperators.Rows[i][0].ToString(), amount = int.Parse(dataTableOperators.Rows[i][1].ToString()) });
                if (i == 4)
                    break;
            }

            DataContext = this;

            localDbConnection.Close();
            localDbConnectionUnity.Close();
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

        private void devicesClicked(object sender, MouseButtonEventArgs e)
        {
            openUserControl(new Devices());
        }

        private void devicesbuttonClicked(object sender, RoutedEventArgs e)
        {
            openUserControl(new Devices());
        }

        private void openUserControl(UserControl myControl)
        {
            DockPanel.SetDock(myControl, Dock.Left);
            dockPanel.Children.Clear();
            dockPanel.Children.Add(myControl);
        }

        private void productButtonClicked(object sender, RoutedEventArgs e)
        {
            openUserControl(new Products());
        }

        private void productClicked(object sender, MouseButtonEventArgs e)
        {
            openUserControl(new Products());
        }

        private void customerClicked(object sender, MouseButtonEventArgs e)
        {
            openUserControl(new Customers());
        }

        private void customerButtonClicked(object sender, RoutedEventArgs e)
        {
            openUserControl(new Customers());
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void showProjectToDo()
        {
            ProjectsToDo projectsToDo = new ProjectsToDo();
            projectsToDo.Closed += projectsClosed;
            projectsToDo.ShowDialog();
        }

        private void showProjectAll()
        {
            ProjectAll projectAll = new ProjectAll();
            projectAll.Closed += projectsClosed;
            projectAll.ShowDialog();
        }

        private void projectsClosed(object sender, EventArgs e)
        {
            openUserControl(this);
        }

        private void showProjectByDate(DateTime date1, DateTime date2)
        {
            ProjectsByDate projectsByDate = new ProjectsByDate(date1, date2);
            projectsByDate.Closed += projectsClosed;
            projectsByDate.ShowDialog();
        }
    }
}
