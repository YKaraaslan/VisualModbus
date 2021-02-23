﻿using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for SearchProjectsOpen.xaml
    /// </summary>
    public partial class SearchProjectsOpen : Window
    {
        SqlConnection localDbConnection;
        List<VarsProjectsAll> projectsAll = new List<VarsProjectsAll>();
        List<VarsProjectsAll> filteredList = new List<VarsProjectsAll>();
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public SearchProjectsOpen(string key)
        {
            InitializeComponent();
            Snackbar.MessageQueue = myMessageQueue;
            string pathToDBFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Projects.mdf";
            string pathToDBFileUnity = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisualUnity\Unity.mdf";
            localDbConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDBFile + ";Integrated Security=True;User ID=unityDatabase;Password=unitySoftware");
            init(key);
        }

        private void init(string key)
        {
            localDbConnection.Open();

            SqlCommand commandProject = new SqlCommand("SELECT * FROM dbo.getProjectsAllForSearch(@key)", localDbConnection);
            commandProject.Parameters.AddWithValue("@key", key);

            SqlDataAdapter dataAdapterProjects = new SqlDataAdapter(commandProject);
            DataTable dataTableProjects = new DataTable();
            dataAdapterProjects.Fill(dataTableProjects);

            projectsAll.Clear();

            for (int i = 0; i < dataTableProjects.Rows.Count; i++)
            {
                projectsAll.Add(new VarsProjectsAll()
                {
                    id = dataTableProjects.Rows[i][0].ToString(),
                    project = dataTableProjects.Rows[i][1].ToString(),
                    company = dataTableProjects.Rows[i][2].ToString(),
                    product = dataTableProjects.Rows[i][3].ToString(),
                    device = dataTableProjects.Rows[i][4].ToString(),
                    operators = dataTableProjects.Rows[i][5].ToString(),
                    explanation = dataTableProjects.Rows[i][6].ToString(),
                    dateCreated = dataTableProjects.Rows[i][7].ToString(),
                    dateDue = dataTableProjects.Rows[i][8].ToString(),
                    dateFinished = dataTableProjects.Rows[i][9].ToString(),
                    explanationFinished = dataTableProjects.Rows[i][10].ToString()
                });
            }
            listViewProjects.ItemsSource = projectsAll;
            localDbConnection.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredList.Clear();

            if (search.Text.Equals(""))
            {
                filteredList.AddRange(projectsAll);
            }
            else
            {
                foreach (VarsProjectsAll varsProjectAll in projectsAll)
                {

                    if (varsProjectAll.project.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.company.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.product.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.device.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.operators.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.explanation.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.explanationFinished.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateCreated.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateDue.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }

                    if (varsProjectAll.dateFinished.ToLower().Contains(search.Text.ToLower()))
                    {
                        filteredList.Add(varsProjectAll);
                        continue;
                    }
                }
            }
            listViewProjects.ItemsSource = filteredList.ToList();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
