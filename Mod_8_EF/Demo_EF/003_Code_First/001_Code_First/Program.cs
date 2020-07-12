using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _001_Code_First
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ModelDb>());
            using (ModelDb db = new ModelDb())
            {
                // Создаем два объекта User
                User user1 = new User { Name = "Jon", Age = 33 };
                User user2 = new User { Name = "Michael", Age = 40 };

                // Добавляем в БД
                db.Users.Add(user1);
                db.Users.Add(user2);
                db.SaveChanges();
                Console.WriteLine("Объекты добавлены");

                // Получаем объекты из БД

                var users = db.Users;

                Console.WriteLine("Список Users:");
                foreach (User u in users)
                {
                    Console.WriteLine("{0}.{1} - {2}", u.Id, u.Name, u.Age);
                }
            }

            Console.Read();
        }
    }
}
