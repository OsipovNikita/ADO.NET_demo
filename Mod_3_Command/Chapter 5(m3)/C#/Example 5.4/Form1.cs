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

namespace Example_5_4
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

        // // асинхронная версия через Begin/End
        private void btnPopulate_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=SSPI;Asynchronous Processing=true";
            SqlConnection testConnection = new SqlConnection(connectionString);
            
            SqlCommand testCommand =
                new SqlCommand("Select * from Customers", testConnection);
            testConnection.Open();

            AsyncCallback callback = new AsyncCallback(DataReaderIsReady);
            IAsyncResult asyncresult = testCommand.BeginExecuteReader(callback, testCommand);
        }

        private void DataReaderIsReady(IAsyncResult result)
        {
            MessageBox.Show("Results Load Complete","I'm Done");
            SqlCommand testCommand = (SqlCommand)result.AsyncState;

            SqlDataReader sqlDr = testCommand.EndExecuteReader(result);
            if (sqlDr.HasRows)
            {
                foreach (DbDataRecord rec in sqlDr)
                {
                    dbRecordsHolder.Add(rec);
                }
            }
            sqlDr.Close();
            testCommand.Connection.Dispose();
        }

        private void btnDataBind_Click(object sender, EventArgs e)
        {
            myDataGrid.DataSource = dbRecordsHolder;
        }

        // асинхронная версия с помощью async/await
        private async void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection testConnection = new SqlConnection(cs))
            {
                SqlCommand testCommand =
                    new SqlCommand("SELECT ContactName, CompanyName FROM Customers", testConnection);
                testConnection.Open();
                SqlDataReader sqlDr = await testCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                if (sqlDr.HasRows)
                {
                    foreach (DbDataRecord rec in sqlDr)
                    {
                        dbRecordsHolder.Add(rec);
                    }
                }
            } // testConnection.Dispose is called automatically
        }
    }
}