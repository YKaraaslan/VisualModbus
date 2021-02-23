using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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
using LiveCharts.Definitions.Points;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using MaterialDesignThemes.Wpf;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for Operators.xaml
    /// </summary>
    public partial class Operators : UserControl
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DataTable dataTableOperatorsName;
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollectionAmount { get; set; }

        public Operators()
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

            SqlCommand commandOperators = new SqlCommand("SELECT * FROM dbo.operatorsDashboard() ORDER BY 2 DESC", localDbConnection);
            SqlDataAdapter dataAdapterOperators = new SqlDataAdapter(commandOperators);
            DataTable dataTableOperators = new DataTable();
            dataAdapterOperators.Fill(dataTableOperators);
            listViewOperators.Items.Clear();
            SeriesCollection = new SeriesCollection();
            for (int i = 0; i < dataTableOperators.Rows.Count; i++)
            {
                listViewOperators.Items.Add(new VarsOperators() { name = dataTableOperators.Rows[i][0].ToString(), amount = int.Parse(dataTableOperators.Rows[i][1].ToString()) });
                SeriesCollection.Add(new PieSeries
                {
                    Title = dataTableOperators.Rows[i][0].ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableOperators.Rows[i][1].ToString())
                     }
                });
                if (i == 4)
                    break;
            }

            SqlCommand commandOperatorProjectAmount = new SqlCommand("SELECT * FROM dbo.countProjectAmountByOperators()", localDbConnection);
            SqlDataAdapter dataAdapterOperatorsProjectAmount = new SqlDataAdapter(commandOperatorProjectAmount);
            DataTable dataTableOperatorsProjectAmount = new DataTable();
            dataAdapterOperatorsProjectAmount.Fill(dataTableOperatorsProjectAmount);
            SeriesCollectionAmount = new SeriesCollection();
            for (int i = 0; i < dataTableOperatorsProjectAmount.Rows.Count; i++)
            {
                SeriesCollectionAmount.Add(new ColumnSeries
                {
                    Title = dataTableOperatorsProjectAmount.Rows[i][1].ToString(),
                    Values = new ChartValues<double> { double.Parse(dataTableOperatorsProjectAmount.Rows[i][0].ToString())
                     }
                });
                if (i == 4)
                    break;
            }

            SqlCommand operatorsName = new SqlCommand("SELECT * FROM dbo.getOperators()", localDbConnectionUnity);
            SqlDataAdapter dataAdapterOperatorsName = new SqlDataAdapter(operatorsName);
            dataTableOperatorsName = new DataTable();
            dataAdapterOperatorsName.Fill(dataTableOperatorsName);

            TextBlock[] operators = { operator1Text, operator2Text, operator3Text, operator4Text, operator5Text, operator6Text,
            operator7Text, operator8Text, operator9Text, operator10Text, operator11Text, operator12Text, operator13Text, operator14Text };

            Card[] cards = { operator1Card, operator2Card, operator3Card, operator4Card, operator5Card, operator6Card,
            operator7Card, operator8Card, operator9Card, operator10Card, operator11Card, operator12Card, operator13Card, operator14Card };

            try
            {
                for (int i = 0; i < Properties.Settings.Default.operatorAmount; i++)
                {
                    operators[i].Text = dataTableOperatorsName.Rows[i][0].ToString();
                    cards[i].Visibility = Visibility.Visible;
                }
            }
            catch {  }

            localDbConnection.Close();
            localDbConnectionUnity.Close();
        }

        private void operatorInformation(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[0][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation2(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[1][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation3(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[2][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation4(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[3][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation5(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[4][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation6(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[5][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation7(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[6][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation8(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[7][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation9(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[8][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation10(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[9][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation11(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[10][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation12(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[11][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation13(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[12][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void operatorInformation14(object sender, RoutedEventArgs e)
        {
            OperatorsInformation operatorsInformation = new OperatorsInformation(dataTableOperatorsName.Rows[13][0].ToString());
            operatorsInformation.ShowDialog();
        }

        private void settings1(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[0][0].ToString(), 1);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings2(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[1][0].ToString(), 2);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings3(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[2][0].ToString(), 3);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings4(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[3][0].ToString(), 4);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings5(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[4][0].ToString(), 5);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings6(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[5][0].ToString(), 6);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings7(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[6][0].ToString(), 7);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings8(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[7][0].ToString(), 8);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings9(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[8][0].ToString(), 9);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings10(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[9][0].ToString(), 10);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings11(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[10][0].ToString(), 11);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings12(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[11][0].ToString(), 12);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings13(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[12][0].ToString(), 13);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void settings14(object sender, RoutedEventArgs e)
        {
            OperatorsSettings operatorsSettings = new OperatorsSettings(dataTableOperatorsName.Rows[13][0].ToString(), 14);
            operatorsSettings.Closed += Closed;
            operatorsSettings.ShowDialog();
        }

        private void Closed(object sender, EventArgs e)
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
