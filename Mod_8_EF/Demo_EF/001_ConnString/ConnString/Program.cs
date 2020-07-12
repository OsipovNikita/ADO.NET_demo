using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;

using System.Reflection;


namespace ConsoleApplication6
{
    class Program
    {
        static void Main(string[] args)
        {

            const string providerName = "System.Data.SqlClient";
            const string serverName = @"(LocalDB)\MSSQLLocalDB";
            string databasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                    @"\MyDatabaseConn.mdf";

            // Initialize the connection string builder for the
            // underlying provider.
            var sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                AttachDBFilename = databasePath,
                IntegratedSecurity = true
            };

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            var entityBuilder = new EntityConnectionStringBuilder
            {
                Provider = providerName,
                ProviderConnectionString = providerString,
                Metadata = @"res://*/MyModelDb.csdl|res://*/MyModelDb.ssdl|res://*/MyModelDb.msl"
            };

            Console.WriteLine(entityBuilder.ToString());

            using (var conn = new EntityConnection(entityBuilder.ToString()))
            {
                conn.Open();
                Console.WriteLine("Just testing the connection.");
                conn.Close();
            }

            Console.ReadKey();
        }
    }
}
