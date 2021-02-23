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
    /// Interaction logic for SearchProducts.xaml
    /// </summary>
    public partial class SearchProducts : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsProducts> productsAll = new List<VarsProducts>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public SearchProducts(string key)
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

            SqlCommand commandCompaniesGroup = new SqlCommand("SELECT * FROM dbo.productsGroup() WHERE Product = @key ORDER BY 2 DESC", localDbConnection);
            commandCompaniesGroup.Parameters.AddWithValue("@key", key);
            SqlDataAdapter dataAdapterCompaniesGroup = new SqlDataAdapter(commandCompaniesGroup);
            DataTable dataTableCompaniesGroup = new DataTable();
            dataAdapterCompaniesGroup.Fill(dataTableCompaniesGroup);

            productsAll.Clear();
            for (int i = 0; i < dataTableCompaniesGroup.Rows.Count; i++)
            {
                productsAll.Add(new VarsProducts() { product = dataTableCompaniesGroup.Rows[i][0].ToString(), ID = int.Parse(dataTableCompaniesGroup.Rows[i][1].ToString()) });
            }
            listViewProducts.ItemsSource = productsAll;

            localDbConnection.Close();
        }

        private void productInformation_Click(object sender, RoutedEventArgs e)
        {
            if (listViewProducts.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
            VarsProducts varsProducts = (VarsProducts)listViewProducts.SelectedItem;
            ProductsInformation productsInformation = new ProductsInformation(varsProducts.product);
            productsInformation.ShowDialog();
        }

        private void HandleOKMethod()
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
