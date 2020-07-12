#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient ;

#endregion

namespace Exercise_11_3
{
    /// <summary>
    /// Точки сохранения (savepoint) — это маркеры, выполняющие роль закладок. 
    /// Во время выполнения транзакции можно пометить какую-либо точку,
    /// и затем выполнить откат к этой точке вместо полного отката всей транзакции
    /// </summary>
    class Program_11_3
   {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDb;Initial Catalog=Test;Integrated Security=True";

        static void Main()
      {
         using (SqlConnection testConnection = new SqlConnection(connectionString))
         {
            SqlCommand testCommand = testConnection.CreateCommand();
            testConnection.Open();
            
            SqlTransaction myTransaction = testConnection.BeginTransaction();
            testCommand.Transaction = myTransaction;

            try
            {
               testCommand.CommandText = 
                  "Insert into Customers (FirstName, LastName, AccountBalance) Values ('Bat','Man',100)";
               testCommand.ExecuteNonQuery();
               myTransaction.Save("firstCustomer");                     // точка сохранения

               testCommand.CommandText = 
                  "Insert into Customers (FirstName, LastName, AccountBalance) Values ('The','Joker',100)";
               testCommand.ExecuteNonQuery();

               myTransaction.Rollback("firstCustomer");                 // откат к точке сохранения

               testCommand.CommandText = 
                  "Insert into Customers (FirstName, LastName, AccountBalance) Values ('Robin','Sidekick',100)";
               testCommand.ExecuteNonQuery();
               myTransaction.Commit();                                  // фиксация изменений

               testCommand.CommandText = "Select * from Customers";
               SqlDataReader sqlDa = testCommand.ExecuteReader();

               while (sqlDa.Read())
               {
                  Console.WriteLine(
                     " FirstName: " + sqlDa["FirstName"] + 
                     " LastName = " + sqlDa["LastName"] + 
                     " AccountBalance = " + sqlDa["AccountBalance"]);
               }
               sqlDa.Close();
            }
            catch (System.Exception ex)
            {
               Console.WriteLine(ex.ToString());
            }
            testConnection.Close();
         } // testConnection.Dispose is called automatically.
      }
   }
}

/*
 FirstName: Capt. LastName = Kirk AccountBalance = 100
 FirstName: Super LastName = Man AccountBalance = 96
 FirstName: Bat LastName = Man AccountBalance = 100
 FirstName: Robin LastName = Sidekick AccountBalance = 100
 */
