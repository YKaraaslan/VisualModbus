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

namespace VisualUNITY.WebGuide
{
    /// <summary>
    /// Interaction logic for WebGuideMailList.xaml
    /// </summary>
    public partial class WebGuideMailList : Window
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        List<VarsWebGuide> mails = new List<VarsWebGuide>();

        public WebGuideMailList()
        {
            InitializeComponent();
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

            SqlCommand commandMail = new SqlCommand("SELECT * FROM dbo.getMails() ORDER BY Date DESC", localDbConnection);

            SqlDataAdapter dataAdapterProjects = new SqlDataAdapter(commandMail);
            DataTable dataTableMail = new DataTable();
            dataAdapterProjects.Fill(dataTableMail);

            mails.Clear();

            for (int i = 0; i < dataTableMail.Rows.Count; i++)
            {
                mails.Add(new VarsWebGuide()
                {
                    sender = dataTableMail.Rows[i][0].ToString(),
                    senderMail = dataTableMail.Rows[i][1].ToString(),
                    subject = dataTableMail.Rows[i][2].ToString(),
                    content = dataTableMail.Rows[i][3].ToString(),
                    date = dataTableMail.Rows[i][4].ToString()
                });
            }
            listViewProjects.ItemsSource = mails;
            localDbConnection.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
