using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace _002_1_to_M
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<OneToManyDbEntity>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OneToManyDbEntity>());


            using (OneToManyDbEntity db = new OneToManyDbEntity())
            {
                Product p1 = new Product { Name = "Nokia Lumia 930", Price = 9000 };
                Product p2 = new Product { Name = "Nokia Lumia 830", Price = 6000 };
                Product p3 = new Product { Name = "Samsung Galaxy S5", Price = 10000 };
                Product p4 = new Product { Name = "Samsung Galaxy S4", Price = 6000 };

                db.Products.AddRange(new List<Product> {p1, p2, p3, p4});
                db.SaveChanges();

                var products = db.Products.ToList();

                foreach (var item in products)
                {
                    Console.WriteLine("{0}.{1} - {2} ({3})", item.Id, item.Name, item.Price, item.Order != null ? item.Order.Customer : "Not Customer");
                }

                Console.WriteLine("-----------------------------------------");
                Console.ReadKey();

                Order order1 = new Order { Customer = "Nazar", Quantity = 2, Product = new List<Product> { p1, p3 }};

                Order order2 = new Order { Customer = "Alex", Quantity = 1, Product = new List<Product> { p2, p4 }};
               
                db.Orders.AddRange(new List<Order> { order1, order2 });
                db.SaveChanges();

                var products1 = db.Products.ToList();

                foreach (var item in products1)
                {
                    Console.WriteLine("{0}.{1} - {2} ({3})", item.Id, item.Name, item.Price, item.Order != null? item.Order.Customer: "Not Customer");
                }

                Console.WriteLine("-----------------------------------------");
                Console.ReadKey();

                var orders = db.Orders.ToList();

                foreach (var itemOrder in orders)
                {
                    Console.WriteLine("{0}.{1}", itemOrder.Id, itemOrder.Customer);

                    if (itemOrder.Product == null) continue;

                    foreach (var itemProd in itemOrder.Product)
                    {
                        Console.WriteLine("{0} - {1} * {2} = {3}", itemProd.Name, itemProd.Price, itemOrder.Quantity, itemProd.Price* itemOrder.Quantity);
                    }
                    Console.WriteLine("-----------------------------------------");
                }
                Console.ReadKey();
            }
        }
    }
}
