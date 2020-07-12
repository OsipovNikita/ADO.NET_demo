#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace Exercise_11_2
{
    /// <summary>
    /// Уровень изоляции — это степень видимости внутри транзакции изменений, выполненных за пределами этой транзакции. 
    /// Он определяет, насколько чувствительна транзакция к изменениям, выполненным другими транзакциями. 
    /// </summary>
    class Program_11_2
   {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDb;Initial Catalog=Test;Integrated Security=True";

        static void Main(string[] args)
        {

         SqlConnection connection1 = new SqlConnection(connectionString);
         SqlConnection connection2 = new SqlConnection(connectionString);

         SqlCommand command1 = connection1.CreateCommand();
         SqlCommand command2 = connection2.CreateCommand();

         try
         {
         connection1.Open();
         connection2.Open();

         SqlTransaction transaction1 = connection1.BeginTransaction(IsolationLevel.ReadCommitted);      // 1
         command1.Transaction = transaction1;

         SqlTransaction transaction2 = connection2.BeginTransaction(IsolationLevel.ReadUncommitted);    // 2
         command2.Transaction = transaction2;

         SqlDataReader myReader;

            command1.CommandText = 
               "INSERT INTO CUSTOMERS (FIRSTNAME, LASTNAME, ACCOUNTBALANCE) VALUES ('Bat', 'Man', 100)";
            command1.ExecuteNonQuery();                                                                 // 3

            command2.CommandText = 
               "SELECT FIRSTNAME, LASTNAME from CUSTOMERS where FIRSTNAME = 'Bat'";
            myReader = command2.ExecuteReader();                                                        // 4

            Console.WriteLine("Results when the transaction is midway:");
            if (!myReader.HasRows)
               Console.WriteLine("No Rows Found");
            while (myReader.Read())
            {
               Console.WriteLine("FirstName: " + myReader[0] + " and LastName: " + myReader[1]);
            }
            myReader.Close();

            transaction1.Rollback();

            command2.CommandText = 
               "SELECT FIRSTNAME, LASTNAME from CUSTOMERS where FIRSTNAME = 'Bat'";
            myReader = command2.ExecuteReader();                                                        // 5

            Console.WriteLine("Results when the transaction is rolled back:");
            if (!myReader.HasRows)
               Console.WriteLine("No Rows Found");
            while (myReader.Read())
            {
               Console.WriteLine("FirstName: " + myReader[0] + " and LastName: " + myReader[1]);
            }
            myReader.Close();
         }
         catch (System.Exception ex)
         {
            Console.WriteLine(ex.ToString());
         }
         finally
         {
            connection1.Dispose();
            connection2.Dispose();
         }
         Console.Read();
      }
   }
}
/*
 Тестирование работы приложения

    Эксперимент № 1
1.	Открывается подключение к локальной базе данных Test и начинается транзакция. 
    Уровень изоляции этой транзакции устанавливается по умолчанию: ReadCommitted.
2.	Открывается еще одно подключение к базе данных и начинается еще одна транзакция. 
    Но для этой транзакции устанавливается уровень изоляции ReadUncommitted.
3.	Из первой транзакции вставляется строка в таблицу Customers.
4.	Без фиксации первой транзакции из второй транзакции выполняется чтение этой строки с уровнем изоляции ReadUncommitted. 
    Результаты выводятся на консоль: 
        Results when the transaction is midway:
        FirstName: Bat and LastName: Man
    Увидим, что хотя первая транзакция еще не закончена, вторая транзакция читает записи, вставленные первой, 
    а это потенциально проблематично и рисковано.
5.	Первая транзакция откатывается, и из второй транзакции запускается тот же запрос, чтобы снова выбрать 
   только что вставленного (но уже отмененного) покупателя:
        Results when the transaction is rolled back:
        No Rows Found   
   Мы увидим, что результат того же самого запроса будет совершенно другим, а это доказывает, 
   что ReadUncommitted не является надежным уровнем изоляции в тех случаях, когда требуется согласованность данных.

    Эксперимент № 2
1. Измените уровень изоляции первой транзакции на Serializable, а затем запустить программу снова
    Вы увидите, что программа зависнет на выполнении команды SELECT:
     это происходит потому, что уровень изоляции Serializable блокирует все дальнейшие чтения из источника данных 
     (строка, страница или таблица), пока первая транзакция не завершится (фиксацией или откатом).

    Эксперимент № 3
1. Установите уровень изоляции обеих транзакций в Snapshot
2. Запустите приложение. Должно появиться сообщение об исключении.
   Эта ошибка появилась потому, что по умолчанию этот уровень изоляции запрещен. 
3. Разрешите это уровень изоляции, выполнив команду:
      ALTER DATABASE TEST SET ALLOW_SNAPSHOT_ISOLATION ON
4. Запустите приложение еще раз, вы должны увидеть следующие результаты:
    Results when the transaction is midway:
    No Rows Found
    Results when the transaction is rolled back:
    No Rows Found

Этот уровень изоляции уменьшает вероятность установки блокировки строк, 
сохраняя копию данных, которые одно приложение может читать, в то время как другое модифицирует эти же данные
*/
