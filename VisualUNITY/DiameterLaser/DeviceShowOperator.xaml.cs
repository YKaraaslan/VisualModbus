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
    /// Interaction logic for DeviceShowOperator.xaml
    /// </summary>
    public partial class DeviceShowOperator : Window
    {
        SqlConnection localDbConnection, localDbConnectionUnity;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        int id;

        public DeviceShowOperator(string nameReceived, int projectId)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            localDbConnectionUnity = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFileUnity + ";Integrated Security=True");
            id = projectId;
            init(nameReceived);
        }

        private void init(string nameReceived)
        {
            SqlCommand commandOperators = new SqlCommand("SELECT * FROM dbo.getOperators()", localDbConnectionUnity);
            SqlDataAdapter dataAdapterOperators = new SqlDataAdapter(commandOperators);
            DataTable dataTableOperators = new DataTable();
            dataAdapterOperators.Fill(dataTableOperators);

            for (int i = 0; i < Properties.Settings.Default.operatorAmount; i++)
            {
                comboboxOperator.Items.Add(dataTableOperators.Rows[i][0].ToString());
            }
            comboboxOperator.SelectedItem = nameReceived;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void chooseOperator(object sender, RoutedEventArgs e)
        {
            localDbConnection.Open();

            SqlCommand commandToUpdate = new SqlCommand("dbo.updateOperatorInProjectsToDo", localDbConnection);

            commandToUpdate.CommandType = CommandType.StoredProcedure;
            commandToUpdate.Parameters.AddWithValue("@id", id);
            commandToUpdate.Parameters.AddWithValue("@operatorNewName", comboboxOperator.Text);
            commandToUpdate.ExecuteNonQuery();

            localDbConnection.Close();
            MessageBox.Show(rm.GetString("projectOperatorUpdated", cultureInfo), rm.GetString("system", cultureInfo),
                        MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
