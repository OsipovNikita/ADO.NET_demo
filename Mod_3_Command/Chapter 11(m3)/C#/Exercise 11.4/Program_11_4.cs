#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

#endregion

namespace Exercise_11_4
{
    // для выполнения необходимо создать две базы данных на основе скрипта \Mod_3_Command\Chapter 11(m3)\SQL\CreateDistributedDatabase.sql
    /// <summary>
    /// Код примера заносит две соответствующие друг другу операции вставки в одну транзакцию. 
    /// То есть если операция добавления денег пошла неудачно, 
    /// то необходимо выполнить откат и для ранее вставленной строки снятия суммы
    /// </summary>
    class Program_11_4
   {
      private static string connectionString1 = @"Data Source=(LocalDB)\MSSQLLocalDb;Initial Catalog=Credits;Integrated Security=True";
      private static string connectionString2 = @"Data Source=(LocalDB)\MSSQLLocalDb;Initial Catalog=Debits;Integrated Security=True";

        static void Main()
        {
            int f = CreateTransactionScope(
                connectionString1, connectionString2,
                "Insert into Credits(CreditAmount) Values (100)",
                "Insert into Debits(DebitAmount) Values (100)"
                );

        }
        /// <summary>
        /// Распределенная транзакция — это транзакция, влияющая на несколько ресурсов. 
        /// Для фиксации распределенной транзакции все участники должны гарантировать, 
        /// что любое изменение данных будет постоянным.
        /// На платформе.NET Framework распределенные транзакции управляются с помощью API-интерфейса 
        /// пространства имен System.Transactions
        /// </summary>
        /// <param name="connectString1"> первое подключение</param>
        /// <param name="connectString2"> второе подключение</param>
        /// <param name="commandText1"> первая команда </param>
        /// <param name="commandText2"> вторая команда</param>
        /// <returns></returns>

        static public int CreateTransactionScope(string connectString1, string connectString2,
    string commandText1, string commandText2)
        {
            // Инициализирует возвращение значения ноль и создает  StringWriter для отображения 
            // результата.
            int returnValue = 0;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                // Создать TransactionScope чторбы выполнить кооманды, гарантирующие
                // что обе коммандлы могут либо завершиться или отмениться как единая единая 
                // рабочая единица.
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection1 = new SqlConnection(connectString1))
                    {
                        // Открытие соединения автоматом включает его в
                        // TransactionScope как облегченную транзакцию.
                        connection1.Open();

                        // Создать объект SqlCommand и выполнить первую команду.
                        SqlCommand command1 = new SqlCommand(commandText1, connection1);
                        returnValue = command1.ExecuteNonQuery();
                        writer.WriteLine("Записи изменены по command1: {0}", returnValue);

                        // Если программа дошла до этой точки, это означает, что 
                        // команда command1 выполнена успешно. Путем вложения
                        //  оператора блока using для connection2 внутри соединения connection1, 
                        // вы сохраняете сервер и сетевые ресурсы когда  connection2 открыто
                        // только и есть шанс завершить транзакцию.   
                        using (SqlConnection connection2 = new SqlConnection(connectString2))
                        {
                            // Транзакция будет продолжена до полной рпаспределенной транзакции
                            // когда соединение connection2 открыто.
                            connection2.Open();

                            // Выполнить вторую команду во второй БД.
                            returnValue = 0;
                            SqlCommand command2 = new SqlCommand(commandText2, connection2);
                            returnValue = command2.ExecuteNonQuery();
                            writer.WriteLine("Записи будут изменены по command2: {0}", returnValue);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("Транзакция отменена с ошибкой: {0}", ex.Message);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(writer.ToString());

            return returnValue;
        }

    }
}
