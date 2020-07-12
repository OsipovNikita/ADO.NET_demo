#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace Exercise_11_1
{
   class Program
   {
       private static string connectionString = "Data Source=(localdb)\\v11.0;Initial Catalog=Test;Integrated Security=True";
      static void Main(string[] args)
      {
         using (SqlConnection testConnection = new SqlConnection(connectionString))
         {
            try
            {
                SqlCommand myCommand = testConnection.CreateCommand();

               SqlParameter param1 = new SqlParameter("@FirstName", SqlDbType.VarChar, 50);
               param1.Value = "Lion";

               SqlParameter param2 = new SqlParameter("@LastName", SqlDbType.VarChar, 50);
               param2.Value = "Good";

               SqlParameter param3 = new SqlParameter("@AccountBalance", SqlDbType.Float);
               param3.Value = 18.7;

               myCommand.Parameters.AddRange(new SqlParameter[] { param1, param2, param3 });

      //      SqlTransaction myTransaction = null;

               testConnection.Open();
     //          myTransaction = testConnection.BeginTransaction();
     //          myCommand.Transaction = myTransaction;
     //          myCommand.CommandText = "Insert into CustomerProduct (CustomerID, ProductID) Values (2, 1)";

               myCommand.CommandText = "Insert into Customers (FirstName, LastName, AccountBalance) VALUES (@FirstName, @LastName, @AccountBalance)";
               myCommand.ExecuteNonQuery();

   //            myCommand.CommandText = "Update Customers Set AccountBalance = 96 Where CustomerID = 2";
               myCommand.CommandText = "Update Customers Set AccountBalance = @AccountBalance Where CustomerID = 2";

          //     myCommand.CommandText = "Delete Products Where ProductID = 2";

               myCommand.ExecuteNonQuery();
   //            myTransaction.Commit();
            }
            catch (System.Exception ex)
            {
     //          myTransaction.Rollback();
     //          throw ex;
                Console.WriteLine(ex.Message);
            }
            finally
            {
               testConnection.Close();
            }
         }
      }
   }
}
