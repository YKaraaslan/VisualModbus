using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualUNITY
{
    public partial class WorkOrder1 : Form
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;

        public WorkOrder1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
        }

        private void WorkOrder1_Load(object sender, EventArgs e)
        {
            localDbConnection.Open();
            SqlDataAdapter dA = new SqlDataAdapter(String.Format("SELECT * FROM ProjectWorkOrder"), localDbConnection);
            DataTable dT = new DataTable();
            dA.Fill(dT);
            localDbConnection.Close();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dT);
            
            this.reportViewer1.LocalReport.DisplayName = rm.GetString("report", cultureInfo) + "_" + DateTime.Now.ToString("dd_MM_yyyy");
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.LocalReport.Refresh();


            ReportParameterCollection responsiblePerson = new ReportParameterCollection();
            responsiblePerson.Add(new ReportParameter("responsible", Properties.Settings.Default.nameSurname));
            this.reportViewer1.LocalReport.SetParameters(responsiblePerson);
            this.reportViewer1.RefreshReport();

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();
            SqlCommand command = new SqlCommand("dbo.deleteWorkOrder", localDbConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            localDbConnection.Close();
        }
    }
}
