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
    /// Interaction logic for CompanyAdd.xaml
    /// </summary>
    public partial class CompanyAdd : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public CompanyAdd()
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
            addCompany();
        }

        private void companyName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addCompany();
            }
        }

        private void addCompany()
        {
            if (companyName.Text.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("fillBlanks", cultureInfo), rm.GetString("ok", cultureInfo), () => HandleOKMethod());
                return;
                
            }
            localDbConnection.Open();

            SqlCommand checkCompanyName = new SqlCommand("SELECT dbo.checkCompanyName(@Company)", localDbConnection);
            checkCompanyName.Parameters.AddWithValue("@Company", companyName.Text.Trim());

            if ((int)checkCompanyName.ExecuteScalar() > 0)
            {
                myMessageQueue.Enqueue(rm.GetString("companyInformationAlreadyExists", cultureInfo), rm.GetString("ok", cultureInfo), () => HandleOKMethod());
                localDbConnection.Close();
                return;
            }

            SqlCommand commandToAddDatabase = new SqlCommand("dbo.addCompany", localDbConnection);

            commandToAddDatabase.CommandType = CommandType.StoredProcedure;
            commandToAddDatabase.Parameters.AddWithValue("@Company", companyName.Text);
            commandToAddDatabase.ExecuteNonQuery();

            localDbConnection.Close();
            MessageBox.Show(rm.GetString("companyInformationSaved", cultureInfo), rm.GetString("system", cultureInfo),
                MessageBoxButton.OK, MessageBoxImage.Information);
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
