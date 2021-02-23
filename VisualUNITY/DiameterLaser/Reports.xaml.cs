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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsProjectsAll> projectsAll = new List<VarsProjectsAll>();
        List<VarsProjectsAll> filteredList = new List<VarsProjectsAll>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        List<int> idList = new List<int>();

        public Reports()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True");
            init();
        }

        private void init()
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();
            if (localDbConnectionUnity.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();

            SqlCommand commandProject = new SqlCommand("SELECT dbo.countProjects()", localDbConnection);
            SqlCommand commandProjectReport = new SqlCommand("SELECT dbo.countReports()", localDbConnectionUnity);
            reportReady.Content = commandProject.ExecuteScalar();
            reportDone.Content = commandProjectReport.ExecuteScalar();

            SqlCommand commandToGetProject = new SqlCommand("SELECT * FROM dbo.getProjectsAll()", localDbConnection);

            SqlDataAdapter dataAdapterProjects = new SqlDataAdapter(commandToGetProject);
            DataTable dataTableProjects = new DataTable();
            dataAdapterProjects.Fill(dataTableProjects);

            projectsAll.Clear();

            for (int i = 0; i < dataTableProjects.Rows.Count; i++)
            {
                projectsAll.Add(new VarsProjectsAll()
                {
                    id = dataTableProjects.Rows[i][0].ToString(),
                    project = dataTableProjects.Rows[i][1].ToString(),
                    company = dataTableProjects.Rows[i][2].ToString(),
                    product = dataTableProjects.Rows[i][3].ToString(),
                    device = dataTableProjects.Rows[i][4].ToString(),
                    operators = dataTableProjects.Rows[i][5].ToString(),
                    explanation = dataTableProjects.Rows[i][6].ToString(),
                    dateCreated = dataTableProjects.Rows[i][7].ToString(),
                    dateDue = dataTableProjects.Rows[i][8].ToString(),
                    dateFinished = dataTableProjects.Rows[i][9].ToString()
                });
            }
            listViewProjects.ItemsSource = projectsAll;


            diameter.IsChecked = Properties.Settings.Default.diameterCheck;
            diameterDifference.IsChecked = Properties.Settings.Default.diameterDifferenceCheck;
            diameterSet.IsChecked = Properties.Settings.Default.diameterSetCheck;
            plusTolerance.IsChecked = Properties.Settings.Default.plusToleranceCheck;
            minusTolerance.IsChecked = Properties.Settings.Default.minusToleranceCheck;
            display3.IsChecked = Properties.Settings.Default.display3Check;
            nc.IsChecked = Properties.Settings.Default.ncCheck;
            even.IsChecked = Properties.Settings.Default.evenCheck;
            parp.IsChecked = Properties.Settings.Default.parpCheck;
            pari.IsChecked = Properties.Settings.Default.pariCheck;

            diameter2.IsChecked = Properties.Settings.Default.diameter2;
            plusTolerance2.IsChecked = Properties.Settings.Default.plusTolerance2;
            minusTolerance2.IsChecked = Properties.Settings.Default.minusTolerance2;
            xAxis.IsChecked = Properties.Settings.Default.xAxis;
            yAxis.IsChecked = Properties.Settings.Default.yAxis;

            SqlCommand commandCompanies = new SqlCommand("SELECT * FROM dbo.getCompanies()", localDbConnectionUnity);
            SqlCommand commandProducts = new SqlCommand("SELECT * FROM dbo.getProducts()", localDbConnectionUnity);
            SqlCommand commandOperators = new SqlCommand("SELECT * FROM dbo.getOperators()", localDbConnectionUnity);
            SqlCommand commandDevices = new SqlCommand("SELECT * FROM dbo.getDevices()", localDbConnectionUnity);

            SqlDataAdapter dataAdapterCompanies = new SqlDataAdapter(commandCompanies);
            DataTable dataTableCompanies = new DataTable();
            dataAdapterCompanies.Fill(dataTableCompanies);

            for (int i = 0; i < dataTableCompanies.Rows.Count; i++)
            {
                companyBox.Items.Add(dataTableCompanies.Rows[i][0].ToString());
            }

            if (companyBox.Items.Count > 0)
                companyBox.SelectedIndex = 0;

            SqlDataAdapter dataAdapterProducts = new SqlDataAdapter(commandProducts);
            DataTable dataTableProducts = new DataTable();
            dataAdapterProducts.Fill(dataTableProducts);

            for (int i = 0; i < dataTableProducts.Rows.Count; i++)
            {
                productBox.Items.Add(dataTableProducts.Rows[i][0].ToString());
            }

            if (productBox.Items.Count > 0)
                productBox.SelectedIndex = 0;

            SqlDataAdapter dataAdapterOperators = new SqlDataAdapter(commandOperators);
            DataTable dataTableOperators = new DataTable();
            dataAdapterOperators.Fill(dataTableOperators);

            for (int i = 0; i < Properties.Settings.Default.operatorAmount; i++)
            {
                operatorBox.Items.Add(dataTableOperators.Rows[i][0].ToString());
            }

            if (operatorBox != null)
                operatorBox.SelectedIndex = 0;

            SqlDataAdapter dataAdapterDevices = new SqlDataAdapter(commandDevices);
            DataTable dataTableDevices = new DataTable();
            dataAdapterDevices.Fill(dataTableDevices);

            for (int i = 0; i < Properties.Settings.Default.deviceAmount; i++)
            {
                deviceBox.Items.Add(dataTableDevices.Rows[i][0].ToString());
            }

            deviceBox.SelectedIndex = 0;

            localDbConnection.Close();
            localDbConnectionUnity.Close();

            companyCheckBox.IsChecked = true;
            productCheckBox.IsChecked = true;
            operatorCheckBox.IsChecked = true;
            deviceCheckBox.IsChecked = true;
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredList.Clear();

            if (search.Text.Equals(""))
            {
                filteredList.AddRange(projectsAll);
            }
            else
            {
                foreach (VarsProjectsAll varsProjectAll in projectsAll)
                {

                    if (varsProjectAll.project.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.company.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.product.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.device.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.operators.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.explanation.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateCreated.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateDue.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateFinished.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }
                }
            }
            listViewProjects.ItemsSource = filteredList.ToList();
        }

        private void createReport(object sender, RoutedEventArgs e)
        {
            if (listViewProjects.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
            
            int counter2 = countCheckBoxForReport2();
            VarsProjectsAll varsProjectsAll = (VarsProjectsAll)listViewProjects.SelectedItem;

            if (localDbConnectionUnity.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();

            try
            {
                SqlCommand reportCommand = new SqlCommand("INSERT INTO Reports VALUES (@date)", localDbConnectionUnity);
                reportCommand.Parameters.AddWithValue("@date", Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                reportCommand.ExecuteNonQuery();

                SqlCommand commandProjectReport = new SqlCommand("SELECT dbo.countReports()", localDbConnectionUnity);
                reportDone.Content = (int)commandProjectReport.ExecuteScalar();
            }
            catch { }
            
            localDbConnectionUnity.Close();

            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SqlCommand command = new SqlCommand("SELECT dbo.getProjectDeviceType(@id)", localDbConnection);
            command.Parameters.AddWithValue("@id", varsProjectsAll.id);
            int type = int.Parse(command.ExecuteScalar().ToString());

            localDbConnection.Close();

            if(type == 1)
            {
                switch (countCheckBoxForReport1())
                {
                    case 0:
                        myMessageQueue.Enqueue(rm.GetString("selectDataToReport"), rm.GetString("ok"), () => HandleOKMethod());
                        break;
                    case 1:
                        Report1 report1_1 = new Report1(varsProjectsAll.id, idList);
                        report1_1.Show();
                        break;
                    case 2:
                        Report2 report1_2 = new Report2(varsProjectsAll.id, idList);
                        report1_2.Show();
                        break;
                    case 3:
                        Report3 report1_3 = new Report3(varsProjectsAll.id, idList);
                        report1_3.Show();
                        break;
                    case 4:
                        Report4 report1_4 = new Report4(varsProjectsAll.id, idList);
                        report1_4.Show();
                        break;
                    case 5:
                        Report5 report1_5 = new Report5(varsProjectsAll.id, idList);
                        report1_5.Show();
                        break;
                    case 6:
                        Report6 report1_6 = new Report6(varsProjectsAll.id, idList);
                        report1_6.Show();
                        break;
                    case 7:
                        Report7 report1_7 = new Report7(varsProjectsAll.id, idList);
                        report1_7.Show();
                        break;
                    case 8:
                        Report8 report1_8 = new Report8(varsProjectsAll.id, idList);
                        report1_8.Show();
                        break;
                    case 9:
                        Report9 report1_9 = new Report9(varsProjectsAll.id, idList);
                        report1_9.Show();
                        break;
                    case 10:
                        Report10 report1_10 = new Report10(varsProjectsAll.id, idList);
                        report1_10.Show();
                        break;
                    case 11:
                        Report11 report1_11 = new Report11(varsProjectsAll.id, idList);
                        report1_11.Show();
                        break;
                }
            }
            else
            {
                switch (countCheckBoxForReport2())
                {
                    case 0:
                        myMessageQueue.Enqueue(rm.GetString("selectDataToReport"), rm.GetString("ok"), () => HandleOKMethod());
                        break;
                    case 1:
                        Report1 report1_1 = new Report1(varsProjectsAll.id, idList);
                        report1_1.Show();
                        break;
                    case 2:
                        Report2 report1_2 = new Report2(varsProjectsAll.id, idList);
                        report1_2.Show();
                        break;
                    case 3:
                        Report3 report1_3 = new Report3(varsProjectsAll.id, idList);
                        report1_3.Show();
                        break;
                    case 4:
                        Report4 report1_4 = new Report4(varsProjectsAll.id, idList);
                        report1_4.Show();
                        break;
                    case 5:
                        Report5 report1_5 = new Report5(varsProjectsAll.id, idList);
                        report1_5.Show();
                        break;
                    case 6:
                        Report6 report1_6 = new Report6(varsProjectsAll.id, idList);
                        report1_6.Show();
                        break;
                    case 7:
                        Report7 report1_7 = new Report7(varsProjectsAll.id, idList);
                        report1_7.Show();
                        break;
                    case 8:
                        Report8 report1_8 = new Report8(varsProjectsAll.id, idList);
                        report1_8.Show();
                        break;
                    case 9:
                        Report9 report1_9 = new Report9(varsProjectsAll.id, idList);
                        report1_9.Show();
                        break;
                    case 10:
                        Report10 report1_10 = new Report10(varsProjectsAll.id, idList);
                        report1_10.Show();
                        break;
                    case 11:
                        Report11 report1_11 = new Report11(varsProjectsAll.id, idList);
                        report1_11.Show();
                        break;
                    case 12:
                        myMessageQueue.Enqueue(rm.GetString("maxRowToReport"), rm.GetString("ok"), () => HandleOKMethod());
                        break;
                    case 13:
                        myMessageQueue.Enqueue(rm.GetString("maxRowToReport"), rm.GetString("ok"), () => HandleOKMethod());
                        break;
                    case 14:
                        myMessageQueue.Enqueue(rm.GetString("maxRowToReport"), rm.GetString("ok"), () => HandleOKMethod());
                        break;
                    case 15:
                        myMessageQueue.Enqueue(rm.GetString("maxRowToReport"), rm.GetString("ok"), () => HandleOKMethod());
                        break;
                    case 16:
                        myMessageQueue.Enqueue(rm.GetString("maxRowToReport"), rm.GetString("ok"), () => HandleOKMethod());
                        break;
                }
            }
        }

        private void createGraphic(object sender, RoutedEventArgs e)
        {
            if (listViewProjects.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
            VarsProjectsAll varsProjectsAll = (VarsProjectsAll)listViewProjects.SelectedItem;

            ReportsGraphic reportsGraphic = new ReportsGraphic(varsProjectsAll.id);
            reportsGraphic.Show();
        }

        private void HandleOKMethod()
        {
            
        }

        private void companyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            companyBox.IsEnabled = true;
        }

        private void companyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            companyBox.IsEnabled = false;
        }

        private void productCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            productBox.IsEnabled = true;
        }

        private void productCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            productBox.IsEnabled = false;
        }

        private void deviceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            deviceBox.IsEnabled = true;
        }

        private void deviceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            deviceBox.IsEnabled = false;
        }

        private void operatorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            operatorBox.IsEnabled = true;
        }

        private void operatorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            operatorBox.IsEnabled = false;
        }

        private void reportAll(object sender, RoutedEventArgs e)
        {
            listViewProjects.ItemsSource = projectsAll.ToList();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            if (companyCheckBox.IsChecked == false && productCheckBox.IsChecked == false && deviceCheckBox.IsChecked == false && operatorCheckBox.IsChecked == false)
            {
                myMessageQueue.Enqueue(rm.GetString("selectAtLeastOneData"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
                
            filteredList.Clear();

            if (countProjectsForReport() == 4)
            {
                foreach (VarsProjectsAll varsProjectAll in projectsAll)
                { 
                    if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) &&
                        varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) &&
                        varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) &&
                        varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }
                }
            }
            else if (countProjectsForReport() == 1)
            {
                foreach (VarsProjectsAll varsProjectAll in projectsAll)
                {
                    if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true)
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true)
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true)
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true)
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }
                }
            }

            else if (countProjectsForReport() == 2)
            {
                if (companyCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }
                    }
                }

                else if (productCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) &&  companyCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }

                        if (varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }
                    }
                }

                else if (deviceCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if ( varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }

                        if (varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }
                    }
                }

                else if (operatorCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }

                        if (varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }
                    }
                }
            }

            else if (countProjectsForReport() == 3)
            {
                if (companyCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }
                    }
                }

                else if (productCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }

                        if (varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }
                    }
                }

                else if (deviceCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }

                        if (varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && operatorCheckBox.IsChecked == true && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }
                    }
                }

                else if (operatorCheckBox.IsChecked == true)
                {
                    foreach (VarsProjectsAll varsProjectAll in projectsAll)
                    {
                        if (varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                        }

                        if (varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && varsProjectAll.product.ToLower().Contains(productBox.Text.ToLower()) && productCheckBox.IsChecked == true && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }

                        if (varsProjectAll.operators.ToLower().Contains(operatorBox.Text.ToLower()) && varsProjectAll.device.ToLower().Contains(deviceBox.Text.ToLower()) && deviceCheckBox.IsChecked == true && varsProjectAll.company.ToLower().Contains(companyBox.Text.ToLower()) && companyCheckBox.IsChecked == true)
                        {
                            filteredList.Add(varsProjectAll);
                            continue;
                        }
                    }
                }
            }
            listViewProjects.ItemsSource = filteredList.ToList();
        }

        private int countProjectsForReport()
        {
            int i = 0;
            if (companyCheckBox.IsChecked == true)
                i++;
            if (productCheckBox.IsChecked == true)
                i++;
            if (deviceCheckBox.IsChecked == true)
                i++;
            if (operatorCheckBox.IsChecked == true)
                i++;
            return i;
        }

        private int countCheckBoxForReport1()
        {
            int checkedCounter = 0;
            idList = new List<int>();
            idList.Clear();

            if (diameter.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(1);
            }
            if (diameterDifference.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(3);
            }
            if (diameterSet.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(4);
            }
            if (plusTolerance.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(5);
            }
            if (minusTolerance.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(7);
            }
            if (metraj.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(9);
            }
            if (display3.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(12);
            }
            if (nc.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(13);
            }
            if (even.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(14);
            }
            if (parp.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(15);
            }
            if (pari.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(16);
            }
            return checkedCounter;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private int countCheckBoxForReport2()
        {
            int checkedCounter = 0;
            idList = new List<int>();
            idList.Clear();

            if (diameter.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(1);
            }
            if (diameter2.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(2);
            }
            if (diameterDifference.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(3);
            }
            if (diameterSet.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(4);
            }
            if (plusTolerance.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(5);
            }
            if (plusTolerance2.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(6);
            }
            if (minusTolerance.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(7);
            }
            if (minusTolerance2.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(8);
            }
            if (metraj.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(9);
            }
            if (xAxis.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(10);
            }
            if (yAxis.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(11);
            }
            if (display3.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(12);
            }
            if (nc.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(13);
            }
            if (even.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(14);
            }
            if (parp.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(15);
            }
            if (pari.IsChecked == true)
            {
                checkedCounter++;
                idList.Add(16);
            }

            return checkedCounter;
        }
    }
}
