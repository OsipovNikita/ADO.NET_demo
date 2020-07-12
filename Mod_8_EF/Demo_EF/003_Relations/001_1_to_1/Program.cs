using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace _001_1_to_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<OneToOneDbEntity>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OneToOneDbEntity>());

            using (OneToOneDbEntity db = new OneToOneDbEntity())
            {
                //Создание двух продуктов
                Product product1 = new Product { Name = "Coca Cola", Price = 20 };
                Product product2 = new Product { Name = "Pepsi", Price = 15 };

                //Добавления записей в таблицу.
                db.Products.Add(product1);
                db.Products.Add(product2);

                //или                
                //db.Products.AddRange(new List<Product> { product1, product2 });

                //Сохраняет все обновления в источнике данных.
                db.SaveChanges();

                //Создание заказа
                Order order1 = new Order { Customer = "Alex", Quantity = 3 , Product =  product2};
                Order order2 = new Order { Customer = "Nazar", Quantity = 2, Product = product1 };

                db.Orders.Add(order1);
                db.Orders.Add(order2);

                db.SaveChanges();

                //Выборка из БД
                var orders = db.Orders.ToList();

                //Вывод
                foreach (var item in orders)
                {
                    Console.WriteLine("{0}.{1} -> ({2}) Цена за шт: {3}$ - {4} шт.", 
                        item.Id, // 0
                        item.Customer, // 1
                        item.Product != null ? item.Product.Name : "X", // 2
                        item.Product != null ? Convert.ToString(item.Product.Price) : "X", // 3
                        item.Quantity); // 4
                }                
            }
        }
    }
}
