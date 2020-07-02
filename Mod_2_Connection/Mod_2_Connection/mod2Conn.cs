using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_2_Connection
{
    class Mod2Conn
    {
        static SqlConnection testConnection;
        static readonly string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True";
        /*
         Существуют два основных способа гарантировать, что соединения с базой данных
         будут освобождены после использования
        */
        static void Main()
        {
            M1();

            //M2();

            //M3();

            //GetConnectionStrings();
            //string cs = GetConnectionStringByName("DBConnect.Northwind");
            //Console.WriteLine("Строка подключения для DBConnect.Northwind: {0}", cs);

            //Mpool();
        }

        static void M1()
        {
            /*
             Первый вариант: применение блоков try...catch...finally, 
             гарантируется закрытие любых открытых соединений внутри блока finally.
                 */
            testConnection = new SqlConnection(cs);
            try
            {
                testConnection.Open();
                if (testConnection.State == ConnectionState.Open)
                {
                    Console.WriteLine("Successfully opened a connection");
                }

                //return; // имитация выброса
                                
            }
            catch (Exception)
            {
                if (testConnection.State != ConnectionState.Open)
                {
                    Console.WriteLine("Failed to open a connection");
                }
            }
            finally
            {
                // Closing a connection ensures connection pooling.
                if (testConnection.State == ConnectionState.Open)
                {
                    testConnection.Close();
                    Console.WriteLine("Successfully closed a connection");
                }
                testConnection.Dispose();
            }

            Console.WriteLine("Сделать что-то полезное");
        }

        static void M2()
        {
            // Вариант второй: оператор using
            using (testConnection = new SqlConnection(cs))
            {
                // Открыть соединение 
                testConnection.Open();
                if (testConnection.State == ConnectionState.Open)
                {
                    Console.WriteLine("Successfully opened conn_using");
                }
                // Сделать что-то полезное 
            }

            if (testConnection.State != ConnectionState.Open)
            {
                Console.WriteLine("Successfully closed a conn_using");
            }
        }

        static void M3()
        {
            /* Поставщики данных ADO.NET поддерживают объекты построителей строк подключения
             * (connection string builder object), которые позволяют устанавливать пары имя / значение 
             * с помощью строго типизированных свойств.
             * В ADO.NET этo решается с помощью класса ConnectionStringBuilder.*/

            SqlConnectionStringBuilder connstrBuilder = new SqlConnectionStringBuilder();
            connstrBuilder.DataSource = @"(LocalDB)\MSSQLLocalDB";
            connstrBuilder.InitialCatalog = "northwind";
            connstrBuilder.IntegratedSecurity = true;

            using (testConnection = new SqlConnection(connstrBuilder.ToString()))
            {
                try
                {
                    testConnection.Open();
                    if (testConnection.State == ConnectionState.Open)
                    {
                        Console.WriteLine("Connection successfully opened");
                        Console.WriteLine("Connection string used: " +
                            testConnection.ConnectionString);
                    }
                }
                catch (Exception)
                {
                    if (testConnection.State != ConnectionState.Open)
                    {
                        Console.WriteLine("Connection open failed");
                        Console.WriteLine("Connection string used: "
                            + testConnection.ConnectionString);
                    }
                }
            }
        }

        static void GetConnectionStrings()
        {
            /*
             Файл System.Configuration.dll не включается в проекты всех типов, 
             поэтому для использования классов конфигурации требуется добавить на него ссылку
             */
            Console.WriteLine("\nСписок всех строк соединения:");
            System.Configuration.ConnectionStringSettingsCollection settings =
                System.Configuration.ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (System.Configuration.ConnectionStringSettings cs in settings)
                {
                    Console.WriteLine("___");
                    Console.WriteLine(cs.Name);
                    Console.WriteLine(cs.ProviderName);
                    Console.WriteLine(cs.ConnectionString);
                }
            }
        }

        static string GetConnectionStringByName(string name)
        {
            // Извлечение строки соединения по имени
            string returnValue = null;

            System.Configuration.ConnectionStringSettings settings =
                System.Configuration.ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        static void Mpool()
        {
            // Пример отключения пула подключений (Pooling = False)
            /*
             При включенном пуле подключений (Pooling = True или по умолчанию) для обслуживания всех запросов
             задействуется лишь несколько подключений из пула
             */
            testConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True");
            long starticks = DateTime.Now.Ticks;
            for (int i = 0; i <= 1000; i++)
            {
                testConnection =
                new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = northwind; Integrated Security = True;Pooling = True");
                testConnection.Open();
                testConnection.Close();
            }
            long endticks = DateTime.Now.Ticks;
            Console.WriteLine("Time taken : " + (endticks - starticks) + " ticks.");
            testConnection.Dispose();
        }

    }
}
