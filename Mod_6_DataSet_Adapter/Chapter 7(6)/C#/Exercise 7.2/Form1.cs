#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

#endregion

namespace Exercise_7_2
{
   partial class Form1 : Form
   {
      private DataTable userTable;

      public Form1()
      {
         InitializeComponent();
         userTable = new DataTable();
      }

      private void buttonFillData_Click(object sender, EventArgs e)
      {
        string connectionString =
         @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=SSPI;";

         using (SqlConnection testConnection = new SqlConnection(connectionString))
         {
            SqlCommand testCommand = testConnection.CreateCommand();
            testCommand.CommandText = "Select FirstName, LastName from userTable";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(testCommand);
            
            dataAdapter.Fill(userTable);
         } // testConnection.Dispose called automatically.
      }

      private void buttonBind_Click(object sender, EventArgs e)
      {
         datagridView.DataSource = userTable;
      }
   }
}