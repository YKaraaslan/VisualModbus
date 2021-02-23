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
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : UserControl
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        public SeriesCollection SeriesCollection { get; set; }

        public Products()
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

            SqlCommand commandProductsGroup = new SqlCommand("SELECT * FROM dbo.productsGroup() ORDER BY 2 DESC", localDbConnection);
            SqlDataAdapter dataAdapterProductsGroup = new SqlDataAdapter(commandProductsGroup);
            DataTable dataTableProductsGroup = new DataTable();
            dataAdapterProductsGroup.Fill(dataTableProductsGroup);

            SeriesCollection = new SeriesCollection();
            listViewProducts.Items.Clear();

            for (int i = 0; i < dataTableProductsGroup.Rows.Count; i++)
            {
                listViewProducts.Items.Add(new VarsProducts () { product = dataTableProductsGroup.Rows[i][0].ToString(), ID = int.Parse(dataTableProductsGroup.Rows[i][1].ToString()) });
                SeriesCollection.Add(new PieSeries
                {
                    Title = dataTableProductsGroup.Rows[i][0].ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableProductsGroup.Rows[i][1].ToString())
                     }
                });
                if (i == 4)
                    break;
            }

            SqlCommand commandProducts = new SqlCommand("SELECT dbo.productAmount()", localDbConnectionUnity);
            productRegisteredAmount.Content = commandProducts.ExecuteScalar();

            SqlCommand commandProductsProjects = new SqlCommand("SELECT Count(*) FROM dbo.productsGroup()", localDbConnection);
            productAmount.Content = commandProductsProjects.ExecuteScalar();

            localDbConnection.Close();
            localDbConnectionUnity.Close();
        }

        private void product_Click(object sender, RoutedEventArgs e)
        {
            ProductsAmount productsAmount = new ProductsAmount();
            productsAmount.ShowDialog();
        }

        private void productRegistered_Click(object sender, RoutedEventArgs e)
        {
            ProductRegisteredInformation productRegisteredInformation = new ProductRegisteredInformation();
            productRegisteredInformation.Closed += productRegisteredInformationClosed;
            productRegisteredInformation.ShowDialog();
        }

        private void productRegisteredInformationClosed(object sender, EventArgs e)
        {
            init();
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
