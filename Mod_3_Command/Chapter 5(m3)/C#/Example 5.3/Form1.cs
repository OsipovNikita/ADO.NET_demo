#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.Data.SqlClient ;
using System.Data.Common;
using System.Collections;

#endregion

namespace Example_5_3
{
    partial class Form1 : Form
    {
        static readonly string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True";

        private ArrayList dbRecordsHolder;
        public Form1()
        {
            InitializeComponent();
            dbRecordsHolder = new ArrayList();
        }

        private void btnPopulate_Click(object sender, EventArgs e)
        {
            using (SqlConnection testConnection = new SqlConnection(cs))
            {
                SqlCommand testCommand =
                    new SqlCommand("SELECT ContactName, CompanyName FROM Customers", testConnection);
                testConnection.Open();
                SqlDataReader sqlDr = testCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (sqlDr.HasRows)
                {
                    foreach (DbDataRecord rec in sqlDr)
                    {
                        dbRecordsHolder.Add(rec);
                    }
                }
            } // testConnection.Dispose is called automatically
        }

        private void btnDataBind_Click(object sender, EventArgs e)
        {
            myDataGrid.DataSource = dbRecordsHolder;
        }
    }
}