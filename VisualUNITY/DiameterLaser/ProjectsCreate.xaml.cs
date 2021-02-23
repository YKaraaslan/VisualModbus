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
    /// Interaction logic for ProjectsCreate.xaml
    /// </summary>
    public partial class ProjectsCreate : Window
    {
        SqlConnection localDbConnection, localDbConnectionProject;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        string date, time;
        DateTime combined;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        string operatorString, dbName;
        int id;

        public ProjectsCreate(string operatorName, string deviceId)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            string pathToDBFileProject = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionProject = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileProject + ";Integrated Security=True");
            operatorString = operatorName;
            if (deviceId != null)
                id = int.Parse(deviceId);
            init();
        }

        private void init()
        {
            localDbConnection.Open();

            SqlCommand commandCompanies = new SqlCommand("SELECT * FROM dbo.getCompanies()", localDbConnection);
            SqlCommand commandProducts = new SqlCommand("SELECT * FROM dbo.getProducts()", localDbConnection);
            SqlCommand commandOperators = new SqlCommand("SELECT * FROM dbo.getOperators()", localDbConnection);
            SqlCommand commandDevices = new SqlCommand("SELECT * FROM dbo.getDevices()", localDbConnection);

            SqlDataAdapter dataAdapterCompanies = new SqlDataAdapter(commandCompanies);
            DataTable dataTableCompanies = new DataTable();
            dataAdapterCompanies.Fill(dataTableCompanies);

            for (int i = 0; i < dataTableCompanies.Rows.Count; i++)
            {
                company.Items.Add(dataTableCompanies.Rows[i][0].ToString());
            }

            if (company.Items.Count > 0)
                company.SelectedIndex = 0;

            SqlDataAdapter dataAdapterProducts = new SqlDataAdapter(commandProducts);
            DataTable dataTableProducts = new DataTable();
            dataAdapterProducts.Fill(dataTableProducts);

            for (int i = 0; i < dataTableProducts.Rows.Count; i++)
            {
                product.Items.Add(dataTableProducts.Rows[i][0].ToString());
            }

            if (product.Items.Count > 0)
                product.SelectedIndex = 0;

            SqlDataAdapter dataAdapterOperators = new SqlDataAdapter(commandOperators);
            DataTable dataTableOperators = new DataTable();
            dataAdapterOperators.Fill(dataTableOperators);

            for (int i = 0; i < Properties.Settings.Default.operatorAmount; i++)
            {
                operators.Items.Add(dataTableOperators.Rows[i][0].ToString());
            }

            if (operatorString != null)
                operators.SelectedItem = operatorString;
            else
                operators.SelectedIndex = 0;

            SqlDataAdapter dataAdapterDevices = new SqlDataAdapter(commandDevices);
            DataTable dataTableDevices = new DataTable();
            dataAdapterDevices.Fill(dataTableDevices);

            for (int i = 0; i < Properties.Settings.Default.deviceAmount; i++)
            {
                device.Items.Add(dataTableDevices.Rows[i][0].ToString());
            }

            device.SelectedIndex = id;

            localDbConnection.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void addCompany(object sender, RoutedEventArgs e)
        {
            CompanyAdd companyAdd = new CompanyAdd();
            companyAdd.Closed += companyAddClosed;
            companyAdd.ShowDialog();
        }

        private void companyAddClosed(object sender, EventArgs e)
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SqlCommand commandCompanies = new SqlCommand("SELECT * FROM dbo.getCompanies() ORDER BY Id Desc", localDbConnection);

            SqlDataAdapter dataAdapterCompanies = new SqlDataAdapter(commandCompanies);
            DataTable dataTableCompanies = new DataTable();
            dataAdapterCompanies.Fill(dataTableCompanies);

            company.Items.Clear();

            for (int i = 0; i < dataTableCompanies.Rows.Count; i++)
            {
                company.Items.Add(dataTableCompanies.Rows[i][0].ToString());
            }

            if (company.Items.Count > 0)
                company.SelectedIndex = 0;

            localDbConnection.Close();
        }

        private void addProduct(object sender, RoutedEventArgs e)
        {
            ProductAdd productAdd = new ProductAdd();
            productAdd.Closed += productAddClosed;
            productAdd.ShowDialog();
        }

        private void productAddClosed(object sender, EventArgs e)
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SqlCommand commandProducts = new SqlCommand("SELECT * FROM dbo.getProducts() ORDER BY Id Desc", localDbConnection);

            SqlDataAdapter dataAdapterProducts = new SqlDataAdapter(commandProducts);
            DataTable dataTableProducts = new DataTable();
            dataAdapterProducts.Fill(dataTableProducts);

            product.Items.Clear();

            for (int i = 0; i < dataTableProducts.Rows.Count; i++)
            {
                product.Items.Add(dataTableProducts.Rows[i][0].ToString());
            }

            if (product.Items.Count > 0)
                product.SelectedIndex = 0;

            localDbConnection.Close();
        }

        private void timeEnd_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            var oldValue = e.OldValue.HasValue ? e.OldValue.Value.ToLongTimeString() : "NULL";
            time = e.NewValue.HasValue ? e.NewValue.Value.ToLongTimeString() : "NULL";
        }

        private void createProject(object sender, RoutedEventArgs e)
        {
            try
            {
                combined = Convert.ToDateTime(dateEnd.SelectedDate.ToString()).AddMilliseconds(TimeSpan.Parse(time).TotalMilliseconds);
                date = combined.ToString();
            }
            catch {
                myMessageQueue.Enqueue(rm.GetString("checkDate"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }

            if (time == null || date == null || projectName.Text == "" || company.Text == "" ||
                device.Text == "" || product.Text == "" || operators.Text == "" || explanation.Text == "")
            {
                myMessageQueue.Enqueue(rm.GetString("fillBlanks"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }

            if (combined <= DateTime.Now)
            {
                myMessageQueue.Enqueue(rm.GetString("dateIsPast"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }

            localDbConnection.Open();
            localDbConnectionProject.Open();

            SqlCommand commandToGetDeviceType = new SqlCommand("SELECT dbo.getDeviceType(@deviceName)", localDbConnection);
            commandToGetDeviceType.Parameters.AddWithValue("@deviceName", device.Text);
            int deviceType = (int)commandToGetDeviceType.ExecuteScalar();

            dbName = generateDatabaseName();

            SqlCommand commandToCreateProject = new SqlCommand("dbo.insertIntoDatabase", localDbConnectionProject);
            commandToCreateProject.CommandType = CommandType.StoredProcedure;
            commandToCreateProject.Parameters.AddWithValue("@DatabaseName", dbName);
            commandToCreateProject.Parameters.AddWithValue("@Project", projectName.Text);
            commandToCreateProject.Parameters.AddWithValue("@Company", company.Text);
            commandToCreateProject.Parameters.AddWithValue("@Product", product.Text);
            commandToCreateProject.Parameters.AddWithValue("@Device", device.Text);
            commandToCreateProject.Parameters.AddWithValue("@DeviceType", deviceType);
            commandToCreateProject.Parameters.AddWithValue("@Operator", operators.Text);
            commandToCreateProject.Parameters.AddWithValue("@Explanation", explanation.Text);
            commandToCreateProject.Parameters.AddWithValue("@DateCreated", Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
            commandToCreateProject.Parameters.AddWithValue("@DateDue", combined);

            commandToCreateProject.ExecuteNonQuery();

            setVariables(device.SelectedIndex);

            localDbConnection.Close();
            localDbConnectionProject.Close();

            projectName.Text = ""; explanation.Text = "";

            MessageBox.Show(rm.GetString("projectSaved"), rm.GetString("system"), MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void setVariables(int idSelected)
        {
            switch (idSelected)
            {
                case 0:
                    if (VarsDevices.device1DbName == null)
                        VarsDevices.device1DbName = dbName;
                    break;
                case 1:
                    if (VarsDevices.device2DbName == null)
                        VarsDevices.device2DbName = dbName;
                    break;
                case 2:
                    if (VarsDevices.device3DbName == null)
                        VarsDevices.device3DbName = dbName;
                    break;
                case 3:
                    if (VarsDevices.device4DbName == null)
                        VarsDevices.device4DbName = dbName;
                    break;
                case 4:
                    if (VarsDevices.device5DbName == null)
                        VarsDevices.device5DbName = dbName;
                    break;
                case 5:
                    if (VarsDevices.device6DbName == null)
                        VarsDevices.device6DbName = dbName;
                    break;
                case 6:
                    if (VarsDevices.device7DbName == null)
                        VarsDevices.device7DbName = dbName;
                    break;
                case 7:
                    if (VarsDevices.device8DbName == null)
                        VarsDevices.device8DbName = dbName;
                    break;
                case 8:
                    if (VarsDevices.device9DbName == null)
                        VarsDevices.device9DbName = dbName;
                    break;
                case 9:
                    if (VarsDevices.device10DbName == null)
                        VarsDevices.device10DbName = dbName;
                    break;
                case 10:
                    if (VarsDevices.device11DbName == null)
                        VarsDevices.device11DbName = dbName;
                    break;
                case 11:
                    if (VarsDevices.device12DbName == null)
                        VarsDevices.device12DbName = dbName;
                    break;
                case 12:
                    if (VarsDevices.device13DbName == null)
                        VarsDevices.device13DbName = dbName;
                    break;
                case 13:
                    if (VarsDevices.device14DbName == null)
                        VarsDevices.device14DbName = dbName;
                    break;
            }
        }

        private string generateDatabaseName()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        private void HandleOKMethod()
        {
            
        }
    }
}
