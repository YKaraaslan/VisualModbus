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
    public partial class WorkOrder9 : Form
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        DataTable dT;

        public WorkOrder9()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True");
        }

        private void WorkOrder9_Load(object sender, EventArgs e)
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

            ReportParameterCollection operatorsParameter6 = new ReportParameterCollection();
            ReportParameterCollection machineParameter6 = new ReportParameterCollection();
            ReportParameterCollection companyParameter6 = new ReportParameterCollection();
            ReportParameterCollection productParameter6 = new ReportParameterCollection();
            ReportParameterCollection timeParameter6 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter6 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter6 = new ReportParameterCollection();

            ReportParameterCollection operatorsParameter7 = new ReportParameterCollection();
            ReportParameterCollection machineParameter7 = new ReportParameterCollection();
            ReportParameterCollection companyParameter7 = new ReportParameterCollection();
            ReportParameterCollection productParameter7 = new ReportParameterCollection();
            ReportParameterCollection timeParameter7 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter7 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter7 = new ReportParameterCollection();

            ReportParameterCollection operatorsParameter8 = new ReportParameterCollection();
            ReportParameterCollection machineParameter8 = new ReportParameterCollection();
            ReportParameterCollection companyParameter8 = new ReportParameterCollection();
            ReportParameterCollection productParameter8 = new ReportParameterCollection();
            ReportParameterCollection timeParameter8 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter8 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter8 = new ReportParameterCollection();

            ReportParameterCollection operatorsParameter9 = new ReportParameterCollection();
            ReportParameterCollection machineParameter9 = new ReportParameterCollection();
            ReportParameterCollection companyParameter9 = new ReportParameterCollection();
            ReportParameterCollection productParameter9 = new ReportParameterCollection();
            ReportParameterCollection timeParameter9 = new ReportParameterCollection();
            ReportParameterCollection timeDueParameter9 = new ReportParameterCollection();
            ReportParameterCollection explanationParameter9 = new ReportParameterCollection();

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

            operatorsParameter6.Add(new ReportParameter("operator6", dT.Rows[5][1].ToString()));
            machineParameter6.Add(new ReportParameter("machine6", dT.Rows[5][2].ToString()));
            companyParameter6.Add(new ReportParameter("company6", dT.Rows[5][3].ToString()));
            productParameter6.Add(new ReportParameter("product6", dT.Rows[5][4].ToString()));
            timeParameter6.Add(new ReportParameter("time6", dT.Rows[5][5].ToString()));
            timeDueParameter6.Add(new ReportParameter("timeDue6", dT.Rows[5][6].ToString()));
            explanationParameter6.Add(new ReportParameter("explanation6", dT.Rows[5][7].ToString()));

            operatorsParameter6.Add(new ReportParameter("operator7", dT.Rows[6][1].ToString()));
            machineParameter6.Add(new ReportParameter("machine7", dT.Rows[6][2].ToString()));
            companyParameter6.Add(new ReportParameter("company7", dT.Rows[6][3].ToString()));
            productParameter6.Add(new ReportParameter("product7", dT.Rows[6][4].ToString()));
            timeParameter6.Add(new ReportParameter("time7", dT.Rows[6][5].ToString()));
            timeDueParameter6.Add(new ReportParameter("timeDue7", dT.Rows[6][6].ToString()));
            explanationParameter6.Add(new ReportParameter("explanation7", dT.Rows[6][7].ToString()));

            operatorsParameter6.Add(new ReportParameter("operator8", dT.Rows[7][1].ToString()));
            machineParameter6.Add(new ReportParameter("machine8", dT.Rows[7][2].ToString()));
            companyParameter6.Add(new ReportParameter("company8", dT.Rows[7][3].ToString()));
            productParameter6.Add(new ReportParameter("product8", dT.Rows[7][4].ToString()));
            timeParameter6.Add(new ReportParameter("time8", dT.Rows[7][5].ToString()));
            timeDueParameter6.Add(new ReportParameter("timeDue8", dT.Rows[7][6].ToString()));
            explanationParameter6.Add(new ReportParameter("explanation8", dT.Rows[7][7].ToString()));

            operatorsParameter6.Add(new ReportParameter("operator9", dT.Rows[8][1].ToString()));
            machineParameter6.Add(new ReportParameter("machine9", dT.Rows[8][2].ToString()));
            companyParameter6.Add(new ReportParameter("company9", dT.Rows[8][3].ToString()));
            productParameter6.Add(new ReportParameter("product9", dT.Rows[8][4].ToString()));
            timeParameter6.Add(new ReportParameter("time9", dT.Rows[8][5].ToString()));
            timeDueParameter6.Add(new ReportParameter("timeDue9", dT.Rows[8][6].ToString()));
            explanationParameter6.Add(new ReportParameter("explanation9", dT.Rows[8][7].ToString()));

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

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter6);
            this.reportViewer1.LocalReport.SetParameters(machineParameter6);
            this.reportViewer1.LocalReport.SetParameters(companyParameter6);
            this.reportViewer1.LocalReport.SetParameters(productParameter6);
            this.reportViewer1.LocalReport.SetParameters(timeParameter6);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter6);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter6);

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter7);
            this.reportViewer1.LocalReport.SetParameters(machineParameter7);
            this.reportViewer1.LocalReport.SetParameters(companyParameter7);
            this.reportViewer1.LocalReport.SetParameters(productParameter7);
            this.reportViewer1.LocalReport.SetParameters(timeParameter7);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter7);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter7);

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter8);
            this.reportViewer1.LocalReport.SetParameters(machineParameter8);
            this.reportViewer1.LocalReport.SetParameters(companyParameter8);
            this.reportViewer1.LocalReport.SetParameters(productParameter8);
            this.reportViewer1.LocalReport.SetParameters(timeParameter8);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter8);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter8);

            this.reportViewer1.LocalReport.SetParameters(operatorsParameter9);
            this.reportViewer1.LocalReport.SetParameters(machineParameter9);
            this.reportViewer1.LocalReport.SetParameters(companyParameter9);
            this.reportViewer1.LocalReport.SetParameters(productParameter9);
            this.reportViewer1.LocalReport.SetParameters(timeParameter9);
            this.reportViewer1.LocalReport.SetParameters(timeDueParameter9);
            this.reportViewer1.LocalReport.SetParameters(explanationParameter9);

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
