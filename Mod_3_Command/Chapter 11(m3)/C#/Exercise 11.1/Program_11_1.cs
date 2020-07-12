#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

#endregion
// для выполнения необходимо создать базу данных на основе скрипта \Mod_3_Command\Chapter 11(m3)\SQL\CreateDatabase.sql
namespace Exercise_11_1
{
   class Program_11_1
   {
      private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDb;Initial Catalog=Test;Integrated Security=True";
      static void Main(string[] args)         
      {
         using (SqlConnection testConnection = new SqlConnection(connectionString))
         {
            SqlCommand myCommand = testConnection.CreateCommand();
            SqlTransaction myTransaction = null;
            try
            {
/*          Наиболее распространенная последовательность шагов, которые должны быть сделаны при разработке транзакционного приложения:
             1.	Открывается подключение к базе данных с помощью метода Open объекта подключения.
             2.	Начинается транзакция с помощью метода BeginTransaction объекта подключения. 
                Этот метод возвращает объект транзакции, который в дальнейшем используется для фиксации или отката транзакции. 
                Все изменения, выполненные любыми запросами до вызова метода BeginTransaction, фиксируются в базе данных сразу же после их выполнения.
             3.	В свойство Transaction объекта команды заносится объект транзакции, порожденный на шаге 2.
             4.	С помощью объекта команды выполняется команда SQL. Для этой цели можно использовать более одного объекта команды, 
                но только если в свойстве Transaction всех этих объектов указан допустимый объект транзакции.
             5.	Транзакция фиксируется или откатывается с помощью метода Commit или Rollback объекта транзакции.
             6.	Подключение к базе данных закрывается.
 */
               testConnection.Open();                                   // 1
               myTransaction = testConnection.BeginTransaction();       // 2
               myCommand.Transaction = myTransaction;                   // 3
               myCommand.CommandText = "Insert into CustomerProduct (CustomerID, ProductID) Values (2, 1)";     // 4
               myCommand.ExecuteNonQuery();
               myCommand.CommandText = "Update Customers Set AccountBalance = 96 Where CustomerID = 2";
               myCommand.ExecuteNonQuery();
               myTransaction.Commit();                                   // 5

               Console.WriteLine("Операция проведена успешно");
             }
            catch (System.Exception ex)
            {
                    if (testConnection.State != ConnectionState.Open)
                    {
                        Console.WriteLine("Failed to open a connection: " + ex.Message);
                    }
                    else
                    {
                        myTransaction.Rollback();                                // 5
                        throw ex;
                    }

                }
            finally
            {
               testConnection.Close();                                   // 6
            }
         }
      }
   }
}
