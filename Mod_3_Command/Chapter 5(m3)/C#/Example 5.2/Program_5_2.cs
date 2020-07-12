#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient ;
#endregion

namespace Example_5_2
{
    class Program_5_2
    {
        static SqlConnection testConnection;
        static readonly string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True";

        static void Main(string[] args)
        {
            ReaderData();
            //ReaderDataParam();
            //ExecuteNonQueryExample();
            //StoredProcs();
        }

        static void ReaderData()
        {
            using (testConnection = new SqlConnection(cs))
            {
                string select = "SELECT ContactName,CompanyName FROM Customers";
                SqlCommand testCommand =
                    new SqlCommand(select, testConnection);
                testConnection.Open();
                /*
                 Метод ExecuteReader() выполняет команду и возвращает типизированный объект-читатель данных.
                 Возвращенный объект может применяться для итерации по возвращенным записям.
                 */
                SqlDataReader sqlDr = testCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (sqlDr.HasRows)
                {
                    while (sqlDr.Read())
                    {
                        Console.WriteLine("Contact: {0, 25}\t Company: {1}",
                        sqlDr[0], sqlDr.GetString(1));
                    }
                }
            } 
        }

        static void ReaderDataParam()
        {
            // Запрос с параметром - @pricePoint
            string queryString =
            "SELECT ProductID, UnitPrice, ProductName from dbo.products "
                + "WHERE UnitPrice > @pricePoint "
                    + "ORDER BY UnitPrice DESC;";

            // Установка параметру значения
            int paramValue = 5;

            using (SqlConnection connection = new SqlConnection(cs))
            {
                // Создание команды 
                SqlCommand command = new SqlCommand(queryString, connection);
                
                // Реализация связи команды и требуемого параметра 
                command.Parameters.AddWithValue("@pricePoint", paramValue);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("{0}\t{1,10}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void ExecuteNonQueryExample()
        {
            string select = "UPDATE Customers " +
                            "SET ContactName = 'Bob' " +
                            "WHERE ContactName = 'Bill'";

            using (testConnection = new SqlConnection(cs))
            {
                SqlCommand testCommand = new SqlCommand(select, testConnection);
                testConnection.Open();
                /*
                 Метод ExecuteNonQuery() используется для операторов UPDATE, INSERT или DELETE, 
                 где единственным возвращаемым значением является количество обработанных строк в виде значения типа int
.                */

                int rowsReturned = testCommand.ExecuteNonQuery();

                Console.WriteLine("{0} rows returned.", rowsReturned);
            }
        }

        // Перед выполнением требуется создать в БД хранимые процедуры с помощью скрипта StoredProcs.sql
        public static void StoredProcs()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // Generate the update command
                SqlCommand updateCommand = GenerateUpdateCommand(conn);

                // Generate the delete command
                SqlCommand deleteCommand = GenerateDeleteCommand(conn);

                // And the insert command
                SqlCommand insertCommand = GenerateInsertCommand(conn);

                DumpRegions(conn, "Regions prior to any stored procedure calls");

                // Insert a new region.
                // First set the @RegionDescription parameter to the new value to insert
                insertCommand.Parameters["@RegionDescription"].Value = "South West";

                // Then execute the command
                insertCommand.ExecuteNonQuery();

                // And then get the value returned from the stored proc
                int newRegionID = (int)insertCommand.Parameters["@RegionID"].Value;

                DumpRegions(conn, "Regions after inserting 'South West'");

                // Update the new region...
                updateCommand.Parameters[0].Value = newRegionID;
                updateCommand.Parameters[1].Value = "South Western England";
                updateCommand.ExecuteNonQuery();

                DumpRegions(conn, "Regions after updating 'South West' to 'South Western England'");

                // Delete the newly created record
                deleteCommand.Parameters["@RegionID"].Value = newRegionID;
                deleteCommand.ExecuteNonQuery();

                DumpRegions(conn, "Regions after deleting 'South Western England'");

                conn.Close();
            }
        }

        /// <summary>
        /// Create a command that will update a region record
        /// </summary>
        /// <param name="conn">The database connection</param>
        /// <returns>A SqlCommand</returns>
        private static SqlCommand GenerateUpdateCommand(SqlConnection conn)
        {
            SqlCommand aCommand = new SqlCommand("RegionUpdate", conn);

            aCommand.CommandType = CommandType.StoredProcedure;
            aCommand.Parameters.Add(new SqlParameter("@RegionID", SqlDbType.Int, 0, "RegionID"));
            aCommand.Parameters.Add(new SqlParameter("@RegionDescription", SqlDbType.NChar, 50, "RegionDescription"));
            aCommand.UpdatedRowSource = UpdateRowSource.None;

            return aCommand;
        }

        /// <summary>
        /// Create a command that will insert a region record
        /// </summary>
        /// <param name="conn">The database connection</param>
        /// <returns>A SqlCommand</returns>
        private static SqlCommand GenerateInsertCommand(SqlConnection conn)
        {
            SqlCommand aCommand = new SqlCommand("RegionInsert", conn);

            aCommand.CommandType = CommandType.StoredProcedure;
            aCommand.Parameters.Add(new SqlParameter("@RegionDescription", SqlDbType.NChar, 50, "RegionDescription"));
            aCommand.Parameters.Add(new SqlParameter("@RegionID", SqlDbType.Int, 0, ParameterDirection.Output,
                false, 0, 0, "RegionID", DataRowVersion.Default, null));
            aCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;

            return aCommand;
        }

        /// <summary>
        /// Create a command that will delete a region record
        /// </summary>
        /// <param name="conn">The database connection</param>
        /// <returns>A SqlCommand</returns>
        private static SqlCommand GenerateDeleteCommand(SqlConnection conn)
        {
            SqlCommand aCommand = new SqlCommand("RegionDelete", conn);

            aCommand.CommandType = CommandType.StoredProcedure;
            aCommand.Parameters.Add(new SqlParameter("@RegionID", SqlDbType.Int, 0, "RegionID"));
            aCommand.UpdatedRowSource = UpdateRowSource.None;

            return aCommand;
        }

        /// <summary>
        /// Dump out the region records within the database
        /// </summary>
        /// <param name="conn">Database Connection</param>
        /// <param name="message">A brief message to display</param>
        private static void DumpRegions(SqlConnection conn, string message)
        {
            SqlCommand aCommand = new SqlCommand("SELECT RegionID , RegionDescription From Region", conn);

            // Note the use of CommandBehaviour.KeyInfo.
            // If this is not set, the default seems to be CommandBehavior.CloseConnection,
            // which is an odd default if there ever was one.  Oh well.
            SqlDataReader aReader = aCommand.ExecuteReader(CommandBehavior.KeyInfo);

            Console.WriteLine(message);

            while (aReader.Read())
            {
                Console.WriteLine("  {0,-20} {1,-40}", aReader[0], aReader[1]);
            }

            aReader.Close();
        }




    }
}