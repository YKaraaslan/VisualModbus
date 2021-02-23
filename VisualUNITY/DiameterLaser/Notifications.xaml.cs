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
    /// Interaction logic for Notifications.xaml
    /// </summary>
    public partial class Notifications : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;

        public Notifications()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);

            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
            init();
        }

        private void init()
        {
            if(localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM dbo.getNotificationData() ORDER BY ID DESC", localDbConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            if (Properties.Settings.Default.language == "tr-TR")
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    wrapPanel.Children.Add(new NotificationItems()
                    {
                        NotificationText = dataTable.Rows[i][2].ToString() + " cihazında yapılacak " + dataTable.Rows[i][1].ToString() + " projesinin son günü.",
                        TimeText = dataTable.Rows[i][3].ToString()
                    });
                }
            }
            else
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    wrapPanel.Children.Add(new NotificationItems()
                    {
                        NotificationText = "Last day for project " + dataTable.Rows[i][1].ToString() + " on device " + dataTable.Rows[i][2].ToString(),
                        TimeText = dataTable.Rows[i][3].ToString()
                    });
                }
            }
            
            localDbConnection.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
