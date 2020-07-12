using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AccChanges
{
    /// <summary>
    /// Программное создание DataAdapter'a и настройка на обновление набора данных (поля FirstName) 
    /// </summary>
   public partial class Form1 : Form
   {
      private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True";
      private DataSet ds = null;

      public Form1()
      {
         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         using (SqlConnection testConnection = new SqlConnection(connectionString))
         {
            SqlDataAdapter sqlDA = new SqlDataAdapter("Select * from Customers",testConnection);

            SqlCommand myUpdateCommand = new SqlCommand("Update Customers Set FirstName = @FirstName where CustomerID = @CustomerID");

            SqlParameter param1 = new SqlParameter("@FirstName", SqlDbType.VarChar, 50);
            param1.SourceColumn = "FirstName";

            SqlParameter param2 = new SqlParameter("@CustomerID", SqlDbType.VarChar, 50);
            param2.SourceColumn = "CustomerID";

            myUpdateCommand.Parameters.AddRange(new SqlParameter[] {param1,param2}) ;
            myUpdateCommand.Connection = testConnection;

            sqlDA.UpdateCommand = myUpdateCommand;      // команда для обновления в источнике данных

            testConnection.Open();

            SqlTransaction myTransaction = testConnection.BeginTransaction();
            sqlDA.UpdateCommand.Transaction = myTransaction;
            sqlDA.AcceptChangesDuringUpdate = false;

            try
            {
               sqlDA.Update(ds);                        // обновление данных
               myTransaction.Commit();
               ds.AcceptChanges();                      // Сохранение изменений
                    
               dataGridView1.DataSource = ds.Tables[0]; // повторно отображаем в гриде
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }
         }
      }

      private void Form1_Load(object sender, EventArgs e)
      {
         using (SqlConnection testConnection = new SqlConnection(connectionString))
         {
            SqlDataAdapter sqlDA = new SqlDataAdapter("SELECT * FROM Customers", testConnection);
            ds = new DataSet();
            sqlDA.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
         }
      }
        /// <summary>
        /// DataAdapter может получать несколько результирующих наборов, возвращенных SelectCommand, 
        /// тогда в наборе DataSet создается несколько таблиц
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection testConnection = new SqlConnection(connectionString))
            {
                  string queryString = "SELECT * FROM Customers; SELECT * FROM Products";
                  SqlDataAdapter sqlDA = new SqlDataAdapter(queryString, testConnection);

             //   SqlDataAdapter sqlDA = new SqlDataAdapter();
             //   sqlDA.SelectCommand = new SqlCommand("Select * from Customers; Select * from Products", testConnection);
                ds = new DataSet();
                sqlDA.Fill(ds);
                int k = ds.Tables.Count;
                dataGridView1.DataSource = ds.Tables[1];        // отобразится вторая таблица - Products
            }
        }
    }
}