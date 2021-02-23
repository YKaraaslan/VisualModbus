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
    /// Interaction logic for OperatorsSettings.xaml
    /// </summary>
    public partial class OperatorsSettings : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        int userId;
        string oldName;
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public OperatorsSettings(string userNamed, int id)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            init(userNamed);
            oldName = userNamed;
            userId = id;
        }

        private void init(string userNamed)
        {
            operatorName.Text = userNamed;
        }

        private void save(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void update()
        {
            if (operatorName.Text.Trim() == "")
            {
                myMessageQueue.Enqueue(rm.GetString("fillBlanks"), rm.GetString("ok"), () => HandleOKMethod());
                return;
            }
            localDbConnection.Open();

            SqlCommand checkOperatorName = new SqlCommand("SELECT dbo.checkOperatorName(@Operator)", localDbConnection);
            checkOperatorName.Parameters.AddWithValue("@Operator", operatorName.Text.Trim());

            if ((int)checkOperatorName.ExecuteScalar() > 0)
            {
                myMessageQueue.Enqueue(rm.GetString("operatorInformationAlreadyExists"), rm.GetString("ok"), () => HandleOKMethod());
                localDbConnection.Close();
                return;
            }

            SqlCommand commandToAddDatabase = new SqlCommand("dbo.updateOperator", localDbConnection);

            commandToAddDatabase.CommandType = CommandType.StoredProcedure;
            commandToAddDatabase.Parameters.AddWithValue("@newName", operatorName.Text.Trim());
            commandToAddDatabase.Parameters.AddWithValue("@oldName", oldName);

            commandToAddDatabase.ExecuteNonQuery();

            localDbConnection.Close();
            MessageBox.Show(rm.GetString("operatorInformationUpdated"), rm.GetString("system"),
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void HandleOKMethod()
        {
            
        }

        private void operatorName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                update();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
