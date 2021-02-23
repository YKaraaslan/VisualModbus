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
using System.Windows.Threading;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for ReportsGraphic.xaml
    /// </summary>
    public partial class ReportsGraphic : Window
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        int i = 0, deviceType;
        string db;
        public SeriesCollection SeriesCollection { get; set; }
        ChartValues<double> diameterValue = new ChartValues<double>();

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public ReportsGraphic(string id)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            init(int.Parse(id));
        }

        private void init(int id)
        {
            localDbConnection.Open();
            localDbConnectionUnity.Open();

            SqlCommand commandToGetInfo = new SqlCommand("SELECT * FROM dbo.getInformationForReportGraphics(@id)", localDbConnection);
            commandToGetInfo.Parameters.AddWithValue("@id", id);
            SqlDataAdapter dataAdapterInfo = new SqlDataAdapter(commandToGetInfo);
            DataTable dataTableInfo = new DataTable();
            dataAdapterInfo.Fill(dataTableInfo);

            deviceName.Content = dataTableInfo.Rows[0][0].ToString();
            operatorName.Content = dataTableInfo.Rows[0][1].ToString();
            companyName.Content = dataTableInfo.Rows[0][2].ToString();
            productName.Content = dataTableInfo.Rows[0][3].ToString();
            projectName.Content = dataTableInfo.Rows[0][4].ToString();
            db = dataTableInfo.Rows[0][5].ToString();
            deviceType = int.Parse(dataTableInfo.Rows[0][6].ToString());

            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SeriesCollection = new SeriesCollection();
            ChartValues<double> diameterList = new ChartValues<double>();
            ChartValues<double> diameterSetList = new ChartValues<double>();
            ChartValues<double> plusToleranceList = new ChartValues<double>();
            ChartValues<double> minusToleranceList = new ChartValues<double>();

            string query = String.Format("SELECT * FROM {0}", db);
            SqlCommand command = new SqlCommand(query, localDbConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                diameterList.Add(double.Parse(reader[1].ToString()));
                diameterSetList.Add(double.Parse(reader[2].ToString()));
                plusToleranceList.Add(double.Parse(reader[4].ToString()));
                minusToleranceList.Add(double.Parse(reader[5].ToString()));
                i++;
            }
            SeriesCollection.Add(new LineSeries { Title = rm.GetString("diameter"), Values = diameterList, PointGeometry = null });
            SeriesCollection.Add(new LineSeries { Title = rm.GetString("diameterSet"), Values = diameterSetList, PointGeometry = null });
            SeriesCollection.Add(new LineSeries { Title = rm.GetString("plusTolerance"), Values = plusToleranceList, PointGeometry = null });
            SeriesCollection.Add(new LineSeries { Title = rm.GetString("minusTolerance"), Values = minusToleranceList, PointGeometry = null });

            labelTime.Content = i.ToString();
            DataContext = this;

            localDbConnection.Close();
            localDbConnectionUnity.Close();
        }
    }
}
