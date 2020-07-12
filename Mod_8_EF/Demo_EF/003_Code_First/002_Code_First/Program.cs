using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _002_Code_First
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MyModel db = new MyModel())
            {
                var users = db.Users.ToList();

                foreach (var user in users)
                {
                    Console.WriteLine("{0}.{1} - {2}", user.Id, user.Name, user.Age);
                }
            }
        }
    }
}
