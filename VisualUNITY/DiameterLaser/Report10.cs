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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualUNITY
{
    public partial class Report10 : Form
    {
        SqlConnection localDbConnection;
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;
        int id;
        DataTable dataTable;
        List<int> columnId;
        List<string> columnNames = new List<string>();
        List<string> headerNames = new List<string>();

        public Report10(string idReceived, List<int> columnIdList)
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            id = int.Parse(idReceived);
            columnId = columnIdList;
        }

        private void Report1_10_Load(object sender, EventArgs e)
        {
            setParameters();

            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM getReportData(@id)", localDbConnection);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            SqlCommand command = new SqlCommand(string.Format("INSERT INTO dbo.Report10 (Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, Param9, Param10) " +
                "SELECT {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10} FROM {0}",
                dataTable.Rows[0][7].ToString(), columnNames[0], columnNames[1], columnNames[2], columnNames[3], columnNames[4],
                columnNames[5], columnNames[6], columnNames[7], columnNames[8], columnNames[9]), localDbConnection);
            command.ExecuteNonQuery();

            SqlDataAdapter dA = new SqlDataAdapter("SELECT * FROM dbo.Report10", localDbConnection);
            DataTable dT = new DataTable();
            dA.Fill(dT);

            localDbConnection.Close();

            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dT);
            this.reportViewer1.LocalReport.DisplayName = rm.GetString("report", cultureInfo) + "_" + DateTime.Now.ToString("dd_MM_yyyy");
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.LocalReport.Refresh();

            init();

            Thread thread = new Thread(new ThreadStart(ThreadTask));
            thread.Start();
            this.reportViewer1.RefreshReport();
        }

        private void ThreadTask()
        {
            if (localDbConnection.State == ConnectionState.Closed)
                localDbConnection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM dbo.Report10", localDbConnection);
            command.ExecuteNonQuery();
            localDbConnection.Close();
        }

        private void init()
        {
            ReportParameterCollection collection = new ReportParameterCollection();
            collection.Add(new ReportParameter("responsible", Properties.Settings.Default.nameSurname));
            collection.Add(new ReportParameter("header1", columnNames[0]));
            collection.Add(new ReportParameter("header2", columnNames[1]));
            collection.Add(new ReportParameter("header3", columnNames[2]));
            collection.Add(new ReportParameter("header4", columnNames[3]));
            collection.Add(new ReportParameter("header5", columnNames[4]));
            collection.Add(new ReportParameter("header6", columnNames[5]));
            collection.Add(new ReportParameter("header7", columnNames[6]));
            collection.Add(new ReportParameter("header8", columnNames[7]));
            collection.Add(new ReportParameter("header9", columnNames[8]));
            collection.Add(new ReportParameter("header10", columnNames[9]));

            collection.Add(new ReportParameter("operator", dataTable.Rows[0][0].ToString()));
            collection.Add(new ReportParameter("device", dataTable.Rows[0][1].ToString()));
            collection.Add(new ReportParameter("company", dataTable.Rows[0][2].ToString()));
            collection.Add(new ReportParameter("product", dataTable.Rows[0][3].ToString()));
            collection.Add(new ReportParameter("time", dataTable.Rows[0][4].ToString()));
            collection.Add(new ReportParameter("timeDue", dataTable.Rows[0][5].ToString()));
            collection.Add(new ReportParameter("explanation", dataTable.Rows[0][6].ToString()));

            this.reportViewer1.LocalReport.SetParameters(collection);
            this.reportViewer1.RefreshReport();
        }

        private void setParameters()
        {
            for (int i = 0; i < columnId.Count; i++)
            {
                switch (columnId[i])
                {
                    case 1:
                        headerNames.Add(rm.GetString("diameter", cultureInfo));
                        columnNames.Add("Cap");
                        break;
                    case 2:
                        headerNames.Add(rm.GetString("diameter2", cultureInfo));
                        columnNames.Add("Cap2");
                        break;
                    case 3:
                        headerNames.Add(rm.GetString("diameterDifference", cultureInfo));
                        columnNames.Add("CapFark");
                        break;
                    case 4:
                        headerNames.Add(rm.GetString("diameterSet", cultureInfo));
                        columnNames.Add("CapSet");
                        break;
                    case 5:
                        headerNames.Add(rm.GetString("plusTolerance", cultureInfo));
                        columnNames.Add("ArtiTolerans");
                        break;
                    case 6:
                        headerNames.Add(rm.GetString("plusTolerance2", cultureInfo));
                        columnNames.Add("ArtiTolerans2");
                        break;
                    case 7:
                        headerNames.Add(rm.GetString("minusTolerance", cultureInfo));
                        columnNames.Add("EksiTolerans");
                        break;
                    case 8:
                        headerNames.Add(rm.GetString("minusTolerance2", cultureInfo));
                        columnNames.Add("EksiTolerans2");
                        break;
                    case 9:
                        headerNames.Add(rm.GetString("metraj", cultureInfo));
                        columnNames.Add("Metraj");
                        break;
                    case 10:
                        headerNames.Add(rm.GetString("xAxis", cultureInfo));
                        columnNames.Add("Xekseni");
                        break;
                    case 11:
                        headerNames.Add(rm.GetString("yAxis", cultureInfo));
                        columnNames.Add("Yekseni");
                        break;
                    case 12:
                        headerNames.Add(rm.GetString("display3", cultureInfo));
                        columnNames.Add("Display3");
                        break;
                    case 13:
                        headerNames.Add(rm.GetString("nc", cultureInfo));
                        columnNames.Add("NC");
                        break;
                    case 14:
                        headerNames.Add(rm.GetString("even", cultureInfo));
                        columnNames.Add("Even");
                        break;
                    case 15:
                        headerNames.Add(rm.GetString("parp", cultureInfo));
                        columnNames.Add("Par_p");
                        break;
                    case 16:
                        headerNames.Add(rm.GetString("pari", cultureInfo));
                        columnNames.Add("Par_i");
                        break;
                }
            }
        }
    }
}
