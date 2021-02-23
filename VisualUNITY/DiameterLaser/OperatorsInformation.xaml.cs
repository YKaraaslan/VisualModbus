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
using System.Windows.Shapes;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for OperatorsInformation.xaml
    /// </summary>
    public partial class OperatorsInformation : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DateTime dateNow, dateYearAgo, dateMonthAgo, dateWeekAgo;
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollectionStepLine { get; set; }

        string userNameString;

        public OperatorsInformation(string userNamed)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            userNameString = userNamed;
            init();
        }

        private void init()
        {
            localDbConnection.Open();

            dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            dateYearAgo = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy HH:mm:ss"));
            dateMonthAgo = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy HH:mm:ss"));
            dateWeekAgo = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy HH:mm:ss"));

            SqlCommand commandCountProjectsByPerson = new SqlCommand("SELECT dbo.countProjectsByOperator(@username)", localDbConnection);
            commandCountProjectsByPerson.Parameters.AddWithValue("@username", userNameString);

            SqlCommand commandCountProjectsToDoByPerson = new SqlCommand("SELECT dbo.countProjectsToDoByOperator(@username)", localDbConnection);
            commandCountProjectsToDoByPerson.Parameters.AddWithValue("@username", userNameString);

            SqlCommand commandCountProjectsByDateForPerson = new SqlCommand("SELECT dbo.countProjectsByDateForOperator(@start_date, @end_date, @username)", localDbConnection);
            commandCountProjectsByDateForPerson.Parameters.AddWithValue("@start_date", dateWeekAgo);
            commandCountProjectsByDateForPerson.Parameters.AddWithValue("@end_date", dateNow);
            commandCountProjectsByDateForPerson.Parameters.AddWithValue("@username", userNameString);

            SqlCommand commandCountProjectsToDoByPersonForToday = new SqlCommand("SELECT dbo.countProjectsByDateForOperator(@start_date, @end_date, @username)", localDbConnection);
            commandCountProjectsToDoByPersonForToday.Parameters.AddWithValue("@start_date", DateTime.Today);
            commandCountProjectsToDoByPersonForToday.Parameters.AddWithValue("@end_date", DateTime.Today.AddDays(1));
            commandCountProjectsToDoByPersonForToday.Parameters.AddWithValue("@username", userNameString);

            projectsAmount.Content = commandCountProjectsByPerson.ExecuteScalar();
            projectsToDoAmount.Content = commandCountProjectsToDoByPerson.ExecuteScalar();
            projectsWeekAmount.Content = commandCountProjectsByDateForPerson.ExecuteScalar();
            projectsDayAmount.Content = commandCountProjectsToDoByPersonForToday.ExecuteScalar();

            operatorName.Content = userNameString;

            SeriesCollection = new SeriesCollection();

            SqlCommand commandOperatorsGraph = new SqlCommand("SELECT * FROM dbo.countProjectsByDayForWeek(@start_date, @end_date, @username) ORDER BY 1 ASC", localDbConnection);
            commandOperatorsGraph.Parameters.AddWithValue("@start_date", dateWeekAgo);
            commandOperatorsGraph.Parameters.AddWithValue("@end_date", dateNow);
            commandOperatorsGraph.Parameters.AddWithValue("@username", userNameString);
            SqlDataAdapter dataAdapterOperatorsDayByDay = new SqlDataAdapter(commandOperatorsGraph);
            DataTable dataTableOperatorsGraphDayByDay = new DataTable();
            dataAdapterOperatorsDayByDay.Fill(dataTableOperatorsGraphDayByDay);

            for (int i = 0; i < dataTableOperatorsGraphDayByDay.Rows.Count; i++)
            {
                SeriesCollection.Add(new PieSeries
                {
                    Title = rm.GetString("day") + (i + 1).ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableOperatorsGraphDayByDay.Rows[i][0].ToString())
                    }
                });
                if (i == 6)
                    break;
            }
            SeriesCollectionStepLine = new SeriesCollection();

            SqlCommand commandOperatorsGraphAll = new SqlCommand("SELECT * FROM dbo.countProjectsByDayForWeek(@start_date, @end_date, @username) ORDER BY 1 ASC", localDbConnection);

            commandOperatorsGraphAll.Parameters.AddWithValue("@start_date", dateMonthAgo);
            commandOperatorsGraphAll.Parameters.AddWithValue("@end_date", dateNow);
            commandOperatorsGraphAll.Parameters.AddWithValue("@username", userNameString);
            SqlDataAdapter dataAdapterOperatorsDayByDayAll = new SqlDataAdapter(commandOperatorsGraphAll);
            DataTable dataTableOperatorsGraphDayByDayAll = new DataTable();
            dataAdapterOperatorsDayByDayAll.Fill(dataTableOperatorsGraphDayByDayAll);

            for (int i = 0; i < dataTableOperatorsGraphDayByDayAll.Rows.Count; i++)
            {
                SeriesCollectionStepLine.Add(new StepLineSeries
                {
                    Title = rm.GetString("day") + (i + 1).ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableOperatorsGraphDayByDayAll.Rows[i][0].ToString())
                    }
                });
            }

            DataContext = this;
            localDbConnection.Close();
        }

        private void projectsDoneByTheOperator(object sender, MouseButtonEventArgs e)
        {
            OperatorProjectsDone operatorProjectsDone = new OperatorProjectsDone(userNameString);
            operatorProjectsDone.ShowDialog();
        }

        private void projectsDoneByTheOperator_Click(object sender, RoutedEventArgs e)
        {
            OperatorProjectsDone operatorProjectsDone = new OperatorProjectsDone(userNameString);
            operatorProjectsDone.ShowDialog();
        }

        private void projectsToDoByTheOperator(object sender, MouseButtonEventArgs e)
        {
            OperatorProjectToDo operatorProjectToDo = new OperatorProjectToDo(userNameString);
            operatorProjectToDo.ShowDialog();
        }

        private void projectsToDoByTheOperator_Click(object sender, RoutedEventArgs e)
        {
            OperatorProjectToDo operatorProjectToDo = new OperatorProjectToDo(userNameString);
            operatorProjectToDo.ShowDialog();
        }

        private void projectsDoneThisWeekByTheOperator(object sender, MouseButtonEventArgs e)
        {
            OperatorProjectsDoneByDate operatorProjectsDoneByDate = new OperatorProjectsDoneByDate(userNameString, dateWeekAgo, dateNow);
            operatorProjectsDoneByDate.ShowDialog();
        }

        private void projectsDoneThisWeekByTheOperator_Click(object sender, RoutedEventArgs e)
        {
            dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            dateWeekAgo = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy HH:mm:ss"));
            OperatorProjectsDoneByDate operatorProjectsDoneByDate = new OperatorProjectsDoneByDate(userNameString, dateWeekAgo, dateNow);
            operatorProjectsDoneByDate.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void projectsDoneTodayByTheOperator(object sender, MouseButtonEventArgs e)
        {
            DateTime dateTomorrow = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"));
            OperatorProjectsDoneByDate operatorProjectsDoneByDate = new OperatorProjectsDoneByDate(userNameString, dateTomorrow, dateNow);
            operatorProjectsDoneByDate.ShowDialog();
        }

        private void projectsDoneTodayByTheOperator_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTomorrow = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"));
            OperatorProjectsDoneByDate operatorProjectsDoneByDate = new OperatorProjectsDoneByDate(userNameString, dateTomorrow, dateNow);
            operatorProjectsDoneByDate.ShowDialog();
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (PieChart)chartPoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
