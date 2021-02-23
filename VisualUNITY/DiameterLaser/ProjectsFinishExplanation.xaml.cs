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
    /// Interaction logic for ProjectsFinishExplanation.xaml
    /// </summary>
    public partial class ProjectsFinishExplanation : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        string db;

        public ProjectsFinishExplanation(string dbName)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            db = dbName;
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            finish();
        }

        private void explanation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                finish();
            }
        }

        private void finish()
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SqlCommand commandToCreateProject = new SqlCommand("dbo.finishProject", localDbConnection);
            commandToCreateProject.CommandType = CommandType.StoredProcedure;
            commandToCreateProject.Parameters.AddWithValue("@DatabaseName", db);
            commandToCreateProject.Parameters.AddWithValue("@Date", Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
            commandToCreateProject.Parameters.AddWithValue("@ExplanationFinished", explanation.Text);
            commandToCreateProject.ExecuteNonQuery();
            localDbConnection.Close();
            MessageBox.Show(rm.GetString("projectFinished"), rm.GetString("system"), 
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
