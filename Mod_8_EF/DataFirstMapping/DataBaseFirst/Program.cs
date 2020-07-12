using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            m1();
            m2();
        }

        private static void m1()
        {
            using (var data = new SchoolEntities())
            {

                foreach (var item in data.People)
                {
                    Console.WriteLine("{0,20} \t {1}", item.FirstName, item.LastName);
                }
            }
        }

        private static void m2()
        {
            using (var data = new SchoolEntities())
            {
                var instrQuery = data.People.OfType<Instructor>();

                foreach (var item in instrQuery)
                {
                    Console.WriteLine("{0,10} \t {1,10} \t {2}", item.FirstName, item.LastName, item.Location);
                }
            }
        }
    }
}
