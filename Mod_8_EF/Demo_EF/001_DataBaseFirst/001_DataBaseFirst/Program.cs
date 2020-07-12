using System;
using System.Linq;

namespace _001_DataBaseFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DataBaseFirstDb db = new DataBaseFirstDb())
            {
                PersonalInfo personalInfo1 = new PersonalInfo { FirstName = "Polo", LastName = "Jo", Age = 16};
                PersonalInfo personalInfo2 = new PersonalInfo { FirstName = "Alex", LastName = "Dash", Age = 18 };

                db.PersonalInfoes.Add(personalInfo1);
                db.PersonalInfoes.Add(personalInfo2);
                db.SaveChanges();

                var personalInfoCollection = db.PersonalInfoes.ToList();

                foreach (var item in personalInfoCollection)
                {
                    Console.WriteLine("{0}.{1} {2} - {3}", item.Id, item.FirstName, item.LastName, item.Age);
                }
            }
        }
    }
}
