using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationMy
{

    class Program
    {
        static void Main(string[] args)
        {

           Employ();
      
       //    lin();

      //      AddCusto();

        //    UpdateCust();

         //   Custom();

        //    DelCusto();

        //    Custom();
            
        //    Custo();
        }

        private static void DelCusto()
        {
            using (var data = new DataClassesMyNor2DataContext())
            {
                var deleteIndivCust = 
                    from cust in data.Customers
                    where cust.CustomerID == "AA85"
                    select cust;

              var deleteOrderIndivCust = 
                    from ord in data.Orders
                    where ord.CustomerID == "AA85"
                    select ord;

              foreach (var item in deleteOrderIndivCust)
              {
                  data.Orders.DeleteOnSubmit(item);

              }

                if (deleteIndivCust.Count() > 0)
                {
                    data.Customers.DeleteOnSubmit(deleteIndivCust.First());
                    data.SubmitChanges();
                }

            }
            
        }




        private static void Employ()
        {
            using (var data = new DataClassesMyNor2DataContext())
            {

                foreach (Employees employee in data.Employees)
                {
                    Console.WriteLine("{0,25} \t {1}", employee.Title, employee.Country);
                }
            }
        }

        private static void Custom()
        {
            Console.WriteLine("\nПросмотр заказчиков");
            using (var data = new DataClassesMyNor2DataContext())
            {

                foreach (Customers cust in data.Customers)
                {
                    Console.WriteLine("{0,25} \t {1} \t\t {2}", cust.ContactName,
                        cust.CompanyName, cust.City);
                }
            }
        }

        private static void Custo()
        {
           
            using (var data = new DataClassesMyNor2DataContext())
            {

                foreach (Customers customer in data.Customers)
                {
                    Console.WriteLine("{0},\t {1}", customer.CompanyName, customer.Country);

                    foreach (Orders order in customer.Orders)
                    {
                        Console.WriteLine("\t{0} {1:d}", order.OrderID, order.OrderDate);
                    }
                }
            }
        }


        private static void lin()
        {
            using (var data = new DataClassesMyNor2DataContext())
            {

                IQueryable<Employees> employeeQuery =
                    from emp in data.Employees
                    where emp.City == "London"
                    select emp;

                foreach (var emp in employeeQuery)
                {
                    Console.WriteLine(emp.Title);
                }
            }
        }


        private static void AddCusto()
        {

             Orders order1 = new Orders {
                        CustomerID = "AA85",
                        EmployeeID = 4,
                        OrderDate = DateTime.Now,
                        RequiredDate = DateTime.Now.AddDays(7),
                        ShipVia = 3,
                        Freight = new Decimal(24.66),
                        ShipName = "Lawn Wranglers",
                        ShipAddress = "1017 Maple Leaf Way",
                        ShipCity = "Ft. Worth",
                        ShipRegion = "TX",
                        ShipPostalCode = "76104",
                        ShipCountry = "USA"
                      };


            using (var data = new DataClassesMyNor2DataContext())
            {
                // Создание экземпляра сущностного объекта
                Customers cust = new Customers
                {
                    CustomerID = "AA85",
                    CompanyName = "ADONET",
                    ContactName = "Niko",
                    ContactTitle = "Owner",
                    Address = "1017 Maple Leaf",
                    City = "Moscow",
                    Region = "MC",
                    PostalCode = "555555",
                    Country = "Russia",
                    Phone = "+7(000)123-4567",
                    Fax = "None"
                    //,
                    //Orders = {
                    //  new Orders {
                    //    CustomerID = "AA85",
                    //    EmployeeID = 4,
                    //    OrderDate = DateTime.Now,
                    //    RequiredDate = DateTime.Now.AddDays(7),
                    //    ShipVia = 3,
                    //    Freight = new Decimal(24.66),
                    //    ShipName = "Lawn Wranglers",
                    //    ShipAddress = "1017 Maple Leaf Way",
                    //    ShipCity = "Ft. Worth",
                    //    ShipRegion = "TX",
                    //    ShipPostalCode = "76104",
                    //    ShipCountry = "USA"
                    //  },
                //}
                };

                data.Orders.InsertOnSubmit(order1);

                // 3. Добавление сущностного объекта в таблицу Customers
                data.Customers.InsertOnSubmit(cust);

                // 4. Вызов метода для записи в базу
                data.SubmitChanges();

                // Проверяем наличие вставленной записи
                Customers getcust = (from c in data.Customers
                                     where c.CustomerID == "AA85"
                                    select c).First<Customers>();

                Console.WriteLine("\n Новый заказчик {0} вставлен в базу данных",
                    getcust == null ? "не" : "успешно");
            }

        }

        private static void UpdateCust()
        {
            using (var data = new DataClassesMyNor2DataContext())
            {
                var cityNameQuery =
                    from cust in data.Customers
                    where cust.City.Contains("London")
                    select cust;

                foreach (var customer in cityNameQuery)
                {
                    if (customer.City == "London")
                    {
                        customer.City = "New London";
                    }
                }

            }
        }


        //private static void CustOrder()
        //{
        //    using (var data = new DataClassesMyNor2DataContext())
        //    {

        //        IQueryable<Customers> custQuery =
        //            from cust in data.Customers
        //           join ord in Orders on cust equals ord.
        //            select emp;

        //        foreach (var emp in employeeQuery)
        //        {
        //            Console.WriteLine(emp.Title);
        //        }
        //    }
        //}

    }

}


