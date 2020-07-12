#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient ;
#endregion

namespace Example_5_1
{
   class Program_5_1
   {
        static SqlConnection testConnection;
        static readonly string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True";

        static void Main(string[] args)
        {
            using (testConnection = new SqlConnection(cs))
            {
                /*
                 Метод ExecuteScalar() применяется в случаях, когда бывает необходимо вернуть 
                 единственный результат из оператора SQL.
                 Этот метод возвращает объект, который при необходимости может быть приведен к соответствующему типу
                */
                SqlCommand testCommand = new SqlCommand("Select count(*) from Customers",testConnection);
               testConnection.Open();
               int numResults = (int) testCommand.ExecuteScalar();

                Console.WriteLine("Total number of rows in Customers: " + numResults);
               testConnection.Close();
            } // testConnection.Dispose is called automatically
        }
   }
}