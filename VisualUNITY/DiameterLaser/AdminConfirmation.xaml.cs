using MaterialDesignThemes.Wpf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Input;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for AdminConfirmation.xaml
    /// </summary>
    public partial class AdminConfirmation : Window
    {
        SqlConnection localDbConnectionUnity, localDbConnectionUnity2, localDbConnectionProject1;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        int id;
        string transaction, nameReceived;
        
        public AdminConfirmation(int idReceived, string transactionReceived, string name)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            string pathToDBFile2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionUnity2 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionProject1 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile2 + ";Integrated Security=True");
            id = idReceived;
            transaction = transactionReceived;
            nameReceived = name;
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            doWork(transaction);
        }

        private void HandleOKMethod()
        {
            
        }

        private void confirmation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                doWork(transaction);
            }
        }

        private void doWork(string work)
        {
            switch (work)
            {
                case "project":
                    deleteProject();
                    break;

                case "product":
                    deleteProduct();
                    break;

                case "company":
                    deleteCompany();
                    break;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void deleteProject()
        {
            if (confirmation.Password == "")
            {
                myMessageQueue.Enqueue(rm.GetString("confirmationIsEmpty"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }

            if (localDbConnectionUnity.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();

            if (localDbConnectionProject1.State == ConnectionState.Closed)
                localDbConnectionProject1.Open();

            string query = "SELECT Password FROM Admins";
            SqlCommand command = new SqlCommand(query, localDbConnectionUnity);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader[0].ToString().Trim() == confirmation.Password.Trim())
                {
                    SqlCommand commandToCreateProject = new SqlCommand("dbo.deleteProjectToDo", localDbConnectionProject1);
                    commandToCreateProject.CommandType = CommandType.StoredProcedure;
                    commandToCreateProject.Parameters.AddWithValue("@id", id);
                    commandToCreateProject.ExecuteNonQuery();

                    MessageBox.Show(rm.GetString("projectDeleted"), rm.GetString("system"),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    localDbConnectionUnity.Close();
                    localDbConnectionProject1.Close();
                    this.Close();
                    return;
                }
            }

            localDbConnectionUnity.Close();
            localDbConnectionProject1.Close();
            myMessageQueue.Enqueue(rm.GetString("adminConfirmationNotValid", cultureInfo), "OK", () => HandleOKMethod());
        }

        private void deleteCompany()
        {
            if (confirmation.Password == "")
            {
                myMessageQueue.Enqueue(rm.GetString("confirmationIsEmpty"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }

            if (localDbConnectionUnity.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();

            if (localDbConnectionUnity2.State == ConnectionState.Closed)
                localDbConnectionUnity2.Open();

            string query = "SELECT Password FROM Admins";
            SqlCommand command = new SqlCommand(query, localDbConnectionUnity);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader[0].ToString().Trim() == confirmation.Password.Trim())
                {
                    SqlCommand commandToCreateProject = new SqlCommand("dbo.deleteCompany", localDbConnectionUnity2);
                    commandToCreateProject.CommandType = CommandType.StoredProcedure;
                    commandToCreateProject.Parameters.AddWithValue("@name", nameReceived);
                    commandToCreateProject.ExecuteNonQuery();

                    MessageBox.Show(rm.GetString("companyInformationDeleted"), rm.GetString("system"),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    localDbConnectionUnity.Close();
                    localDbConnectionUnity2.Close();
                    return;
                }
            }
            localDbConnectionUnity.Close();
            localDbConnectionUnity2.Close();
            myMessageQueue.Enqueue(rm.GetString("adminConfirmationNotValid", cultureInfo), "OK", () => HandleOKMethod());
        }

        private void deleteProduct()
        {
            if (confirmation.Password == "")
            {
                myMessageQueue.Enqueue(rm.GetString("confirmationIsEmpty"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }

            if (localDbConnectionUnity.State == ConnectionState.Closed)
                localDbConnectionUnity.Open();

            if (localDbConnectionUnity2.State == ConnectionState.Closed)
                localDbConnectionUnity2.Open();

            string query = "SELECT Password FROM Admins";
            SqlCommand command = new SqlCommand(query, localDbConnectionUnity);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader[0].ToString().Trim() == confirmation.Password.Trim())
                {
                    SqlCommand commandToCreateProject = new SqlCommand("dbo.deleteProduct", localDbConnectionUnity2);
                    commandToCreateProject.CommandType = CommandType.StoredProcedure;
                    commandToCreateProject.Parameters.AddWithValue("@id", nameReceived);
                    commandToCreateProject.ExecuteNonQuery();

                    MessageBox.Show(rm.GetString("productDeleted"), rm.GetString("system"),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    localDbConnectionUnity.Close();
                    localDbConnectionUnity2.Close();
                    return;
                }
            }
            localDbConnectionUnity.Close();
            localDbConnectionUnity2.Close();
            myMessageQueue.Enqueue(rm.GetString("adminConfirmationNotValid", cultureInfo), "OK", () => HandleOKMethod());
        }
    }
}
