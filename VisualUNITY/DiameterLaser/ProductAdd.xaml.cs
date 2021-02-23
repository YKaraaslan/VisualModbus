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
    /// Interaction logic for ProductAdd.xaml
    /// </summary>
    public partial class ProductAdd : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public ProductAdd()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            addProduct();
        }

        private void projectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addProduct();
            }
        }

        private void addProduct()
        {
            if (projectName.Text.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("fillBlanks"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
            localDbConnection.Open();

            SqlCommand checkCompanyName = new SqlCommand("SELECT dbo.checkProductName(@Product)", localDbConnection);
            checkCompanyName.Parameters.AddWithValue("@Product", projectName.Text.Trim());

            if ((int)checkCompanyName.ExecuteScalar() > 0)
            {
                myMessageQueue.Enqueue(rm.GetString("productInformationAlreadyExists"), rm.GetString("ok"), () => HandleOKMethod());
                localDbConnection.Close();
                return;
            }

            SqlCommand commandToAddDatabase = new SqlCommand("dbo.addProduct", localDbConnection);

            commandToAddDatabase.CommandType = CommandType.StoredProcedure;
            commandToAddDatabase.Parameters.AddWithValue("@Product", projectName.Text);
            commandToAddDatabase.ExecuteNonQuery();

            localDbConnection.Close();
            MessageBox.Show(rm.GetString("productSaved"), rm.GetString("system"), 
                MessageBoxButton.OK , MessageBoxImage.Information);
            this.Close();
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
