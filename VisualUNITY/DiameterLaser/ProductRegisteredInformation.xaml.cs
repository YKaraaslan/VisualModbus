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
    /// Interaction logic for ProductRegisteredInformation.xaml
    /// </summary>
    public partial class ProductRegisteredInformation : Window
    {
        SqlConnection localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsProducts> productsAll = new List<VarsProducts>();
        List<VarsProducts> filteredList = new List<VarsProducts>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public ProductRegisteredInformation()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True");
            init();
        }

        private void init()
        {
            localDbConnectionUnity.Open();

            SqlCommand commandCompaniesGroup = new SqlCommand("SELECT * FROM dbo.getProducts()", localDbConnectionUnity);
            SqlDataAdapter dataAdapterCompaniesGroup = new SqlDataAdapter(commandCompaniesGroup);
            DataTable dataTableCompaniesGroup = new DataTable();
            dataAdapterCompaniesGroup.Fill(dataTableCompaniesGroup);

            productsAll.Clear();
            for (int i = 0; i < dataTableCompaniesGroup.Rows.Count; i++)
            {
                productsAll.Add(new VarsProducts() { product = dataTableCompaniesGroup.Rows[i][0].ToString() });
            }
            listViewProducts.ItemsSource = productsAll;

            localDbConnectionUnity.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredList.Clear();

            if (search.Text.Equals(""))
            {
                filteredList.AddRange(productsAll);
            }
            else
            {
                foreach (VarsProducts varsProcuts in productsAll)
                {
                    if (varsProcuts.product.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProcuts);
                        continue;
                    }
                }
            }
            listViewProducts.ItemsSource = filteredList.ToList();
        }

        private void productAdd_CLick(object sender, RoutedEventArgs e)
        {
            ProductAdd productAdd = new ProductAdd();
            productAdd.Closed += productAdd_Closed;
            productAdd.ShowDialog();
        }

        private void productAdd_Closed(object sender, EventArgs e)
        {
            this.Close();
            ProductRegisteredInformation productRegisteredInformation = new ProductRegisteredInformation();
            productRegisteredInformation.ShowDialog();
        }

        private void productDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listViewProducts.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction"), rm.GetString("ok"), () => HandleOKMethod());
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
            VarsProducts varsProducts = (VarsProducts)listViewProducts.SelectedItem;
            AdminConfirmation adminConfirmation = new AdminConfirmation(0, "product", varsProducts.product);
            adminConfirmation.Closed += adminConfirmationClosed;
            this.Close();
            adminConfirmation.ShowDialog();
        }

        private void adminConfirmationClosed(object sender, EventArgs e)
        {
            ProductRegisteredInformation productRegisteredInformation = new ProductRegisteredInformation();
            productRegisteredInformation.Show();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
