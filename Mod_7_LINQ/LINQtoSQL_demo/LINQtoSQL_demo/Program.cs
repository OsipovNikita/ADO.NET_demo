using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSQL_demo
{
    /*
 1. Добавить ссылки на сборку System.Data.Linq.dll и директивы using
 2. Добавить файл кода northwind.cs в проект
   */

    class Program
    {
        // static Northwnd db = new Northwnd(@"Data Source=(Localdb)\v11.0;Initial Catalog=northwind;Integrated Security=True");
        static Northwnd db = new Northwnd(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True");

        static void Main(string[] args)
        {
            //  m1();            // простой запрос - выбор (проекция)
            //  m11();          // с использованием строгой типизации
            //  m12();          // немедленное выполнение запроса
            //  m13();          // применяется группировка group
            //  m14();          // concat
            //  m2();           // связанный запрос
            //  m3();           // добавление нового объекта
            //  m4();           // обновление 
            //   m5();           // удаление
            //   m6("10249");    // вызов хранимой процедуры - список продуктов, включенных в заказ
            //   m7("ALFKI");    // вызов хранимой процедуры - история заказа для клиента
               m8();          // демонстрируется метод метод LoadWith

        }



        static void m1()
        {
            /* запрос для поиска клиентов из таблицы Customers базы данных, 
               находящихся в Лондоне */

            Table<Customer> Customers = db.GetTable<Customer>();

            IQueryable<Customer> custQuery = from cust in Customers
                                             where cust.City == "London"
                                             select cust;
            // или можно так:
           // var custQuery = Customers.Where(x => x.City == "London");

            foreach (Customer cust in custQuery)
            {
                Console.WriteLine("ID={0}, City={1}", cust.CustomerID,
                cust.City);
            }
            Console.ReadLine();

        }

        static void m11()
        {
            /*Задавая строгую типизацию объекта DataContext, можно избежать вызовов метода GetTable. 
             * Строго типизированные таблицы можно использовать в запросах только при 
             * использовании строго типизированного объекта DataContext.*/

            // Используется объект db, представляющий БД для извлечения требуемой таблицы
            var companyNameQuery =      // удобно использовать неявную типизацию
                            from cust in db.Customers
                            where cust.City == "London"
                            select cust.CompanyName;    // выбирается строка CompanyName для извлечения

            foreach (var cust in companyNameQuery)
            {
                Console.WriteLine(cust);
            }

            Console.ReadLine();
        }

        static void m12()
        {
            // вычисляется среднее для значений Freight из таблицы Orders
            Nullable<Decimal> averageFreight = 
                (from ord in db.Orders
                 select ord.Freight)
                 .Average();

            Console.WriteLine(averageFreight);
        }

        static void m13()
        {
            /* Используется оператор Average для поиска тех продуктов Products, цена единицы
товара для которых выше среднего значения цены единицы товара для содержащей их категории. 
               Затем выполняется отображение групп результатов  */

            var priceQuery = 
                from prod in db.Products
                group prod by prod.CategoryID into grouping
                select new
                {
                    grouping.Key,
                    ExpensiveProducts = 
                        from prod2 in grouping
                        where prod2.UnitPrice > grouping.Average(prod3 => prod3.UnitPrice)
                        select prod2
                };

            foreach (var grp in priceQuery)
            {
                Console.WriteLine(grp.Key);
                foreach (var listing in grp.ExpensiveProducts)
                {
                    Console.WriteLine(listing.ProductName);
                }
            }
        }

        static void m14()
        {
            // возвращение последовательности всех сопоставлений имен и номеров телефонов
            var infoQuery = 
                (from cust in db.Customers
                 select new { Name = cust.CompanyName, cust.Phone })
                 .Concat
                     (from emp in db.Employees
                      select new
                      {
                          Name = emp.FirstName + " " + emp.LastName,
                          Phone = emp.HomePhone
                      }
                      );

            foreach (var infoData in infoQuery)
            {
                Console.WriteLine("Name = {0}, Phone = {1}",
                infoData.Name, infoData.Phone);
            }
        }

        static void m2()
        {
            Table<Customer> Customers = db.GetTable<Customer>();
            Table<Order> Orders = db.GetTable<Order>();

            // Получение доступа к объектам "Order" с помощью объектов "Customer"
            var custQuery =
                        from cust in Customers
                        where cust.Orders.Any(/*x => x.ShipCountry.Contains("USA")*/)
                        select cust;

            foreach (var custObj in custQuery)
            {
                Console.WriteLine("ID={0}, Qty={1}", custObj.CustomerID,
                                                     custObj.Orders.Count);
            }
        }

        static void m3()
        {
            // Create the new Customer object.
            Customer newCust = new Customer();
            newCust.CompanyName = "AdventureWorks Cafe";
            newCust.CustomerID = "ADVCA";
            newCust.PostalCode = "55555";
            newCust.Phone = "555-555-5555";

            // Add the customer to the Customers table.
            db.Customers.InsertOnSubmit(newCust);
            db.SubmitChanges();

            foreach (var c in db.Customers.Where(cust => cust.CustomerID.Contains("CA")))
            {
                Console.WriteLine("{0}, {1}, {2}",
                c.CustomerID, c.CompanyName, c.Orders.Count);
            }
        }

        static void m4()
        {
            // Query for specific customer.
            // First() returns one object rather than a collection.
            var existingCust =
                            (from c in db.Customers
                             where c.CustomerID == "ADVCA"
                             select c)
                            .First();

            // Change the contact name of the customer.
            existingCust.CompanyName = "New Company";
            db.SubmitChanges();

            foreach (var c in db.Customers.Where(cust => cust.CustomerID.Contains("CA")))
            {
                Console.WriteLine("{0}, {1}, {2}",
                c.CustomerID, c.CompanyName, c.Orders.Count);
            }
        }

        static void m5()
        {
            //var existingCust =
            //(from c in db.Customers
            // where c.CustomerID == "ADVCA"
            // select c)
            //.First();

            //    db.Customers.DeleteOnSubmit(existingCust);
            //    db.SubmitChanges();

            // безопасный способ - в случае отсутствия удаляемого объекта:
            var deleteIndivCust = 
                from cust in db.Customers
                where cust.CustomerID == "ADVCA"
                select cust;

            if (deleteIndivCust.Count() > 0)
            {
                db.Customers.DeleteOnSubmit(deleteIndivCust.First());
                db.SubmitChanges();
            }


            foreach (var c in db.Customers.Where(cust => cust.CustomerID.Contains("CA")))
            {
                Console.WriteLine("{0}, {1}, {2}",
                c.CustomerID, c.CompanyName, c.Orders.Count);
            }
        }

        static void m6(string param)
        {
            var custquery = db.CustOrdersDetail(Convert.ToInt32(param));

            // Execute the stored procedure and display the results.
            string msg = "";
            foreach (CustOrdersDetailResult custOrdersDetail in custquery)
            {
                msg = msg + custOrdersDetail.ProductName + "\n";
            }
            if (msg == "")
                msg = "No results.";

            Console.WriteLine(msg);
        }

        static void m7(string param)
        {
            var custquery = db.CustOrderHist(param);
            string msg = "";

            foreach (CustOrderHistResult custOrdHist in custquery)
            {
                msg = msg + custOrdHist.ProductName + "\n";
            }

            Console.WriteLine(msg);

        }

        /// <summary>
        /// Используется метод LoadWith, чтобы указать, какие данные, 
        /// связанные с основными целевыми объектами, должны быть одновременно извлечены
        /// </summary>
        static void m8()
        {
            // извлекаются все заказы (Orders) для всех клиентов (Customers),
            // расположенных в Лондоне

            DataLoadOptions dlo = new DataLoadOptions();

            dlo.LoadWith<Customer>(c => c.Orders);
            db.LoadOptions = dlo;

            var londonCustomers =
                from cust in db.Customers
                where cust.City == "London"
                select cust;

            foreach (var custObj in londonCustomers)
            {
                Console.WriteLine(custObj.CustomerID);
            }
            Console.ReadLine();
        }

    }
}
