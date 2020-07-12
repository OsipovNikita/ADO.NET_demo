using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationEDM
{
    class Program
    {
        static void Main(string[] args)
        {
            // m1();
             //LazyLoading();
             //EagerLoading();
             ExplicitLoading();

        }

        public static void m1()
        {
            using (var data = new NorthwindEntities())
            {

                foreach (Employee employee in data.Employees)
                {
                    Console.WriteLine("{0,25} \t {1}", employee.Title, employee.Country);
                }
            }
        }


        /*
        Отложенная загрузка (lazy loading) заключается в том, что 
        Entity Framework автоматически загружает данные, при этом не загружая связанные данные. 
        Когда потребуются связанные данные Entity Framework создаст еще один запрос к базе данных
            */
        public static void LazyLoading()
        {
            NorthwindEntities context = new NorthwindEntities();

            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            // Загрузить всех покупателей
            List<Customer> customers = context.Customers
                .Where(c => c.City == "London")
                .ToList();      //  запрос к базе

            // Загрузить все их заказы
            List<Order> orders = customers.SelectMany(c => c.Orders)
                 .ToList();      // + запросы к базе данных

            foreach (var item in orders)
            {
                Console.WriteLine("{0,25} \t Дата заказа: {1}", item.Customer.ContactName, item.OrderDate);
            }

            Console.ReadLine();
        }


        /*
        Прямая загрузка данных (eager loading) позволяет указать в запросе какие связанные данные 
        нужно загрузить при выполнении запроса. Благодаря этому, когда в коде вы будете ссылаться 
        на связанную таблицу через навигационное свойство, SQL-запрос не будет направляться в базу данных,
        т.к. связанные данные уже будут загружены при первом запросе
        */
        public static void EagerLoading()
        {
            NorthwindEntities context = new NorthwindEntities();

            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            // Загрузить всех покупателей и связанные с ними заказы
            List<Customer> customers = context.Customers
                .Include("Orders")
                .Where(c => c.City == "London")
                .ToList();      // +1 запрос к базе



       //     Получить все их заказы
            List<Order> orders = customers.SelectMany(c => c.Orders)
                // Запрос к базе данных не выполняется,
                // т.к. данные уже были извлечены 
                // ранее с помощью прямой загрузки
                .ToList();

            foreach (var item in orders)
            {
                Console.WriteLine("{0}\t Дата заказа: {1},\t {2}",
                    item.Customer.ContactName, item.OrderDate, item.Employee.LastName);
            }

            //  Загрузить всех покупателей и связанные с ними заказы
            List<Order> orders2 = context.Orders
                .Include("Employee")
                .Include("Customer")
                .Where(c => c.ShipCity == "London")
                .ToList();      // +1 запрос к базе



        }

        /*
               явная загрузка (explicit loading) 
        Явная загрузка, как и отложенная загрузка, 
        не приводит к загрузке всех связанных данных в первом запросе. Но при этом, 
        в отличие от отложенной загрузки, при вызове навигационного свойства связанного класса,
        эта загрузка не приводит к автоматическому извлечению связанных данных, вы должны 
        явно вызвать метод Load(), если хотите загрузить связанные данные*/
        public static void ExplicitLoading()
        {
            NorthwindEntities context = new NorthwindEntities();

            // Загрузить одного покупателя
            Customer customer = context.Customers
                .Where(c => c.CustomerID == "AROUT")
                .FirstOrDefault();

            // Загрузить связанные с ним заказы с помощью явной загрузки

            // Можно проверить, были ли загружены ранее данные о заказах,
            // для этого покупателя, если нет, то используем явную загрузку.
            if (!context.Entry(customer)
                    .Collection(c => c.Orders).IsLoaded)

                context.Entry(customer)
                    .Collection(c => c.Orders)
                    .Load();


            if (customer != null && customer.Orders != null)
                Console.WriteLine("Покуматель: {0} ", customer.ContactName);
            foreach (var order in customer.Orders)
                    Console.WriteLine("Дата: {0}; \t номер {1}", order.OrderDate, order.OrderID);
        }

        // Справочно:

        // Вставка одного объекта
        public static void AddNewCustomer()
        {
            NorthwindEntities context = new NorthwindEntities();

            // Создать нового покупателя
            Customer customer = new Customer
            {
                // инициализация полей

            };

            // Добавить в DbSet
            context.Customers.Add(customer);

            // Сохранить изменения в базе данных
            context.SaveChanges();
        }

        // Вставка связанных объектов

        public static void AddNewOrder()
        {
            NorthwindEntities context = new NorthwindEntities();

            // Нужно извлечь сначала покупателя, 
            // которому добавляется заказ
            Customer ivan = context.Customers
                .Where(c => c.ContactName == "Иванов")
                .FirstOrDefault();

            // Создаем заказ
            Order order = new Order
            {
                // инициализация полей

                // Ссылка на покупателя в навигационном свойстве
                Customer = ivan
            };

            /*
             Если навигационное свойство имеет тип ссылки, то для вставки связанных данных 
             * нужно просто инициализировать это свойство. Если навигационное свойство имеет 
             * тип коллекции, то нужно использовать ее метод Add() для добавления новой записи.
             */
            context.Orders.Add(order);

            context.SaveChanges();
        }



    }
}
