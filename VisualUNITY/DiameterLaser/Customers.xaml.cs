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
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : UserControl
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollectionStep { get; set; }
        public Customers()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True");
            init();
            DataContext = this;
        }

        private void init()
        {
            localDbConnection.Open();
            localDbConnectionUnity.Open();

            SqlCommand commandCompaniesGroup = new SqlCommand("SELECT * FROM dbo.companiesGroup() ORDER BY 2 DESC", localDbConnection);
            SqlDataAdapter dataAdapterCompaniesGroup = new SqlDataAdapter(commandCompaniesGroup);
            DataTable dataTableCompaniesGroup = new DataTable();
            dataAdapterCompaniesGroup.Fill(dataTableCompaniesGroup);

            SeriesCollection = new SeriesCollection();
            listViewCustomers.Items.Clear();
            for (int i = 0; i < dataTableCompaniesGroup.Rows.Count; i++)
            {
                listViewCustomers.Items.Add(new VarsCompanies() { company = dataTableCompaniesGroup.Rows[i][0].ToString(), ID = int.Parse(dataTableCompaniesGroup.Rows[i][1].ToString()) });
                SeriesCollection.Add(new PieSeries
                {
                    Title = dataTableCompaniesGroup.Rows[i][0].ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableCompaniesGroup.Rows[i][1].ToString())
                     }
                });
                if (i == 4)
                    break;
            }
            SeriesCollectionStep = new SeriesCollection();

            for (int i = 0; i < dataTableCompaniesGroup.Rows.Count; i++)
            {
                SeriesCollectionStep.Add(new ColumnSeries
                {
                    Title = dataTableCompaniesGroup.Rows[i][0].ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableCompaniesGroup.Rows[i][1].ToString())
                     }
                });
                if (i == 4)
                    break;
            }

            SqlCommand commandCustomers = new SqlCommand("SELECT Count(*) FROM dbo.getProjectsAllForCustomer()", localDbConnection);
            customerAmount.Content = commandCustomers.ExecuteScalar();

            SqlCommand commandCustomersToDo = new SqlCommand("SELECT Count(*) FROM dbo.getProjectsToDoForCustomer()", localDbConnection);
            customerToDoAmount.Content = commandCustomersToDo.ExecuteScalar();

            SqlCommand commandCustomerAmount = new SqlCommand("SELECT dbo.customerAmount()", localDbConnectionUnity);
            customersAll.Content = commandCustomerAmount.ExecuteScalar();

            localDbConnection.Close();
            localDbConnectionUnity.Close();
        }

        private void customer_Click(object sender, RoutedEventArgs e)
        {
            CustomerCompanyAmount customerCompanyAmount = new CustomerCompanyAmount();
            customerCompanyAmount.ShowDialog();
        }

        private void customerToDo_Click(object sender, RoutedEventArgs e)
        {
            CustomerCompanyToDoAmount customerCompanyToDo = new CustomerCompanyToDoAmount();
            customerCompanyToDo.ShowDialog();
        }

        private void customerAll_Click(object sender, RoutedEventArgs e)
        {
            CustomerAllAmount customerAllAmount = new CustomerAllAmount();
            customerAllAmount.Closed += customerAllAmount_Closed;
            customerAllAmount.ShowDialog();
        }

        private void customerAllAmount_Closed(object sender, EventArgs e)
        {
            init();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Chart_OnDataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            var chart = (PieChart)chartPoint.ChartView;

            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
