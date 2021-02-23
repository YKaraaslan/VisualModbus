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
    /// Interaction logic for CustomerAllAmount.xaml
    /// </summary>
    public partial class CustomerAllAmount : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsCompanies> varsCompanies = new List<VarsCompanies>();
        List<VarsCompanies> filteredList = new List<VarsCompanies>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public CustomerAllAmount()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            init();
        }

        private void init()
        {
            localDbConnection.Open();

            SqlCommand commandCompaniesGroup = new SqlCommand("SELECT * FROM dbo.getCompanies()", localDbConnection);
            SqlDataAdapter dataAdapterCompaniesGroup = new SqlDataAdapter(commandCompaniesGroup);
            DataTable dataTableCompaniesGroup = new DataTable();
            dataAdapterCompaniesGroup.Fill(dataTableCompaniesGroup);

            varsCompanies.Clear();
            for (int i = 0; i < dataTableCompaniesGroup.Rows.Count; i++)
            {
                varsCompanies.Add(new VarsCompanies() { company = dataTableCompaniesGroup.Rows[i][0].ToString() });
            }
            listViewCustomers.ItemsSource = varsCompanies;

            localDbConnection.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredList.Clear();

            if (search.Text.Equals(""))
            {
                filteredList.AddRange(varsCompanies);
            }
            else
            {
                foreach (VarsCompanies vars in varsCompanies)
                {
                    if (vars.company.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(vars);
                        continue;
                    }
                }
            }
            listViewCustomers.ItemsSource = filteredList.ToList();
        }

        private void companyAdd_CLick(object sender, RoutedEventArgs e)
        {
            CompanyAdd companyAdd = new CompanyAdd();
            companyAdd.Closed += companyAdd_Closed;
            companyAdd.ShowDialog();
        }

        private void companyAdd_Closed(object sender, EventArgs e)
        {
            this.Close();
            CustomerAllAmount customerAllAmount = new CustomerAllAmount();
            customerAllAmount.ShowDialog();
        }

        private void companyDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCustomers.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction", cultureInfo), rm.GetString("ok", cultureInfo), () => HandleOKMethod());
                return;
            }
            if (MessageBox.Show(rm.GetString("areYouSureToDelete", cultureInfo), rm.GetString("system", cultureInfo),
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                deleteSelectedRow();
            }
        }

        private void HandleOKMethod()
        {

        }

        private void deleteSelectedRow()
        {
            VarsCompanies varsCompany = (VarsCompanies)listViewCustomers.SelectedItem;

            AdminConfirmation adminConfirmation = new AdminConfirmation(0, "company", varsCompany.company);
            adminConfirmation.Closed += adminConfirmationClosed;
            this.Close();
            adminConfirmation.ShowDialog();
        }

        private void adminConfirmationClosed(object sender, EventArgs e)
        {
            CustomerAllAmount customerAllAmount = new CustomerAllAmount();
            customerAllAmount.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
