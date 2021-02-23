﻿using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for CustomerCompanyAmount.xaml
    /// </summary>
    public partial class CustomerCompanyAmount : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsCompanies> customersAll = new List<VarsCompanies>();
        List<VarsCompanies> filteredList = new List<VarsCompanies>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public CustomerCompanyAmount()
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            init();
        }
        private void init()
        {
            localDbConnection.Open();

            SqlCommand commandCompaniesGroup = new SqlCommand("SELECT * FROM dbo.companiesGroup() ORDER BY 2 DESC", localDbConnection);
            SqlDataAdapter dataAdapterCompaniesGroup = new SqlDataAdapter(commandCompaniesGroup);
            DataTable dataTableCompaniesGroup = new DataTable();
            dataAdapterCompaniesGroup.Fill(dataTableCompaniesGroup);

            customersAll.Clear();
            for (int i = 0; i < dataTableCompaniesGroup.Rows.Count; i++)
            {
                customersAll.Add(new VarsCompanies() { company = dataTableCompaniesGroup.Rows[i][0].ToString(), ID = int.Parse(dataTableCompaniesGroup.Rows[i][1].ToString()) });
            }
            listViewCustomers.ItemsSource = customersAll;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredList.Clear();

            if (search.Text.Equals(""))
            {
                filteredList.AddRange(customersAll);
            }
            else
            {
                foreach (VarsCompanies varsCustomerAll in customersAll)
                {
                    if (varsCustomerAll.company.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsCustomerAll);
                        continue;
                    }
                }
            }
            listViewCustomers.ItemsSource = filteredList.ToList();
        }

        private void customerInformation_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCustomers.SelectedItem == null)
            {
                myMessageQueue.Enqueue(rm.GetString("selectRowToTakeAction", cultureInfo), rm.GetString("ok", cultureInfo), () => HandleOKMethod());
                return;
            }
            VarsCompanies varsCompanies = (VarsCompanies)listViewCustomers.SelectedItem;
            CustomerInformation customerInformation = new CustomerInformation(varsCompanies.company);
            customerInformation.ShowDialog();
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
