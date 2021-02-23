using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualUNITY
{
    public partial class WorkOrder5 : Form
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DataTable dT;

        public WorkOrder5()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
        }

        private void WorkOrder5_Load(object sender, EventArgs e)
        {
            localDbConnection.Open();
            SqlDataAdapter dA = new SqlDataAdapter(String.Format("SELECT * FROM ProjectWorkOrder"), localDbConnection);
            dT = new DataTable();
            dA.Fill(dT);
            localDbConnection.Close();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dT);
            this.reportViewer1.LocalReport.DisplayName = rm.GetString("report", cultureInfo) + "_" + DateTime.Now.ToString("dd_MM_yyyy");
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.LocalReport.Refresh();
            backgroundWorker1.RunWorkerAsync();
            init();
        }

        private void init()
        {
            ReportParameterCollection responsiblePerson = new ReportParameterCollection();
            responsiblePerson.Add(new ReportParameter("responsible", Properties.Settings.Default.nameSurname));
            this.reportViewer1.LocalReport.SetParameters(responsiblePerson);

            ReportParameterCollection operatorsParameter1 = new ReportParameterCollection();
            ReportParameterCollection machineParameter1 = new ReportParameterCollection();
            ReportParameterCollection companyParameter1 = new ReportParameterCollection();
            ReportParameterCollection productParameter1 = new ReportParameterCollection();
            ReportParameterCollection timeParameter1 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter1 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter1 = new ReportParameterCollection();

            ReportParameterCollection operatorsParameter2 = new ReportParameterCollection();
            ReportParameterCollection machineParameter2 = new ReportParameterCollection();
            ReportParameterCollection companyParameter2 = new ReportParameterCollection();
            ReportParameterCollection productParameter2 = new ReportParameterCollection();
            ReportParameterCollection timeParameter2 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter2 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter2 = new ReportParameterCollection();

            ReportParameterCollection operatorsParameter3 = new ReportParameterCollection();
            ReportParameterCollection machineParameter3 = new ReportParameterCollection();
            ReportParameterCollection companyParameter3 = new ReportParameterCollection();
            ReportParameterCollection productParameter3 = new ReportParameterCollection();
            ReportParameterCollection timeParameter3 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter3 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter3 = new ReportParameterCollection();

            ReportParameterCollection operatorsParameter4 = new ReportParameterCollection();
            ReportParameterCollection machineParameter4 = new ReportParameterCollection();
            ReportParameterCollection companyParameter4 = new ReportParameterCollection();
            ReportParameterCollection productParameter4 = new ReportParameterCollection();
            ReportParameterCollection timeParameter4 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter4 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter4 = new ReportParameterCollection();

            ReportParameterCollection operatorsParameter5 = new ReportParameterCollection();
            ReportParameterCollection machineParameter5 = new ReportParameterCollection();
            ReportParameterCollection companyParameter5 = new ReportParameterCollection();
            ReportParameterCollection productParameter5 = new ReportParameterCollection();
            ReportParameterCollection timeParameter5 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter5 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter5 = new ReportParameterCollection();

            operatorsParameter1.Add(new ReportParameter("operator1", dT.Rows[0][1].ToString()));
            machineParameter1.Add(new ReportParameter("machine1", dT.Rows[0][2].ToString()));
            companyParameter1.Add(new ReportParameter("company1", dT.Rows[0][3].ToString()));
            productParameter1.Add(new ReportParameter("product1", dT.Rows[0][4].ToString()));
            timeParameter1.Add(new ReportParameter("time1", dT.Rows[0][5].ToString()));
            timeDueParameter1.Add(new ReportParameter("timeDue1", dT.Rows[0][6].ToString()));
            explanationParameter1.Add(new ReportParameter("explanation1", dT.Rows[0][7].ToString()));

            operatorsParameter2.Add(new ReportParameter("operator2", dT.Rows[1][1].ToString()));
            machineParameter2.Add(new ReportParameter("machine2", dT.Rows[1][2].ToString()));
            companyParameter2.Add(new ReportParameter("company2", dT.Rows[1][3].ToString()));
            productParameter2.Add(new ReportParameter("product2", dT.Rows[1][4].ToString()));
            timeParameter2.Add(new ReportParameter("time2", dT.Rows[1][5].ToString()));
            timeDueParameter2.Add(new ReportParameter("timeDue2", dT.Rows[1][6].ToString()));
            explanationParameter2.Add(new ReportParameter("explanation2", dT.Rows[1][7].ToString()));

            operatorsParameter3.Add(new ReportParameter("operator3", dT.Rows[2][1].ToString()));
            machineParameter3.Add(new ReportParameter("machine3", dT.Rows[2][2].ToString()));
            companyParameter3.Add(new ReportParameter("company3", dT.Rows[2][3].ToString()));
            productParameter3.Add(new ReportParameter("product3", dT.Rows[2][4].ToString()));
            timeParameter3.Add(new ReportParameter("time3", dT.Rows[2][5].ToString()));
            timeDueParameter3.Add(new ReportParameter("timeDue3", dT.Rows[2][6].ToString()));
            explanationParameter3.Add(new ReportParameter("explanation3", dT.Rows[2][7].ToString()));

            operatorsParameter3.Add(new ReportParameter("operator4", dT.Rows[3][1].ToString()));
            machineParameter3.Add(new ReportParameter("machine4", dT.Rows[3][2].ToString()));
            companyParameter3.Add(new ReportParameter("company4", dT.Rows[3][3].ToString()));
            productParameter3.Add(new ReportParameter("product4", dT.Rows[3][4].ToString()));
            timeParameter3.Add(new ReportParameter("time4", dT.Rows[3][5].ToString()));
            timeDueParameter3.Add(new ReportParameter("timeDue4", dT.Rows[3][6].ToString()));
            explanationParameter3.Add(new ReportParameter("explanation4", dT.Rows[3][7].ToString()));

            operatorsParameter5.Add(new ReportParameter("operator5", dT.Rows[4][1].ToString()));
            machineParameter5.Add(new ReportParameter("machine5", dT.Rows[4][2].ToString()));
            companyParameter5.Add(new ReportParameter("company5", dT.Rows[4][3].ToString()));
            productParameter5.Add(new ReportParameter("product5", dT.Rows[4][4].ToString()));
            timeParameter5.Add(new ReportParameter("time5", dT.Rows[4][5].ToString()));
            timeDueParameter5.Add(new ReportParameter("timeDue5", dT.Rows[4][6].ToString()));
            explanationParameter5.Add(new ReportParameter("explanation5", dT.Rows[4][7].ToString()));

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter1);
            this.reportViewer1.LocalReport.SetParameters(machineParameter1);
            this.reportViewer1.LocalReport.SetParameters(companyParameter1);
            this.reportViewer1.LocalReport.SetParameters(productParameter1);
            this.reportViewer1.LocalReport.SetParameters(timeParameter1);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter1);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter1);

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter2);
            this.reportViewer1.LocalReport.SetParameters(machineParameter2);
            this.reportViewer1.LocalReport.SetParameters(companyParameter2);
            this.reportViewer1.LocalReport.SetParameters(productParameter2);
            this.reportViewer1.LocalReport.SetParameters(timeParameter2);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter2);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter2);

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter3);
            this.reportViewer1.LocalReport.SetParameters(machineParameter3);
            this.reportViewer1.LocalReport.SetParameters(companyParameter3);
            this.reportViewer1.LocalReport.SetParameters(productParameter3);
            this.reportViewer1.LocalReport.SetParameters(timeParameter3);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter3);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter3);

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter4);
            this.reportViewer1.LocalReport.SetParameters(machineParameter4);
            this.reportViewer1.LocalReport.SetParameters(companyParameter4);
            this.reportViewer1.LocalReport.SetParameters(productParameter4);
            this.reportViewer1.LocalReport.SetParameters(timeParameter4);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter4);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter4);

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter5);
            this.reportViewer1.LocalReport.SetParameters(machineParameter5);
            this.reportViewer1.LocalReport.SetParameters(companyParameter5);
            this.reportViewer1.LocalReport.SetParameters(productParameter5);
            this.reportViewer1.LocalReport.SetParameters(timeParameter5);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter5);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter5);

            this.reportViewer1.RefreshReport();
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
