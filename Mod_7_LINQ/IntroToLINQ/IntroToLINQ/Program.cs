using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntroToLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
           m1();
         //  m2();
         //  m3();
         //  m4();
         //  mlet();
        }


        // Три этапа работы с LINQ Query
        private static void m1()
        {
            //  1. Источник данных: реализует интерфейс IEnumerable
            int[] numbers = { 0, 1, 22, 3, 4, 5, 6 };

            // 2. Создание запроса
            // numQuery is an IEnumerable<int>
            var numQuery =
                from num in numbers
                where (num % 2) == 0
                select num;

            Console.WriteLine(numQuery);
            // с помощью методов расширения
            var numQuery2 = numbers.Where(num => num % 2 == 0);

            // 3. Выполненте запроса
            foreach (int num in numQuery)
            {
                Console.Write("{0,1} ", num);
            }

            double f = numQuery2.Average();
            Console.WriteLine("\nAverage = " + f);

        }

        private static void m2()
        {
            // Data source.
            int[] scores = { 90, 71, 82, 93, 75, 82 };

            // Query Expression.
            IEnumerable<int> scoreQuery =               // query variable
                from score in scores                    // required
                where score > 80                        // optional
                orderby score descending                // optional
                select score;                           //must end with select or group

            // Execute the query to produce the results
            foreach (int testScore in scoreQuery)
            {
                Console.WriteLine(testScore);
            }

          //  scores[2] = 99;     // после создание запроса источник изменился - как это повлияет на выполнение запроса?
             
            int H23 =
                (from score in scores
                 select score)
                 .Max();                    // принудительное выполнение запроса
            Console.WriteLine("\nMax() --> {0} ", H23);

            int H32 =
                scoreQuery.Max();           // повторное выполнение запроса
            Console.WriteLine("\nMax() --> {0} ", H32);

        }

        private static void m3()
        { 
            int[] numbers = { 0, 1, 2, 3, 4, 5, 6 };

            var evenNumQuery = 
                from num in numbers
                where (num % 2) == 0
                select num;

            // выполнение запроса
            int evenNumCount = evenNumQuery.Count();
            int evenMax = evenNumQuery.Max();

            List<int> numQuery2 =
                (from num in numbers
                 where (num % 2) == 0
                 select num).ToList();      // Принудительное немедленное выполнение
                                            // 
            numbers[0] = 8;

            Console.WriteLine("\nCount() --> {0,1} ", evenNumCount);    // 4
            Console.WriteLine("\nMax() --> {0,1} ", evenMax);           // 6
            Console.WriteLine("\nMax() --> {0,1} ", numQuery2.Max());   // 8
            
        }


        private static void m4()
        {
            // Data source.
            int[] scores = { 90, 71, 82, 93, 75, 82 };

            // Query Expression.
            IEnumerable<string> highScoresQuery2 =
                from score in scores
                where score > 80
                orderby score descending
                select String.Format("The score is {0}", score);

            // Execute the query to produce the results
            foreach (string testScore in highScoresQuery2)
            {
                Console.WriteLine(testScore);
            }
        }

        private static void mlet()
        {
            List<Human> hs = new List<Human>()
            {
                new Human { Name = "Miko", Age = 88},
                new Human { Name = "Jiko", Age = 108}
            };

            var people = from h in hs
                         let name = "Mr. " + h.Name // let применяется для сохранения результатов выражения (вызов метода) в новую переменную диапазона
                         select new                 // select формирует новый тип, указывая свойства (атрибуты) нового типа
                         {
                             NameNew = name,
                             AgeNew = h.Age
                         };

            foreach (var testh in people)
            {
                Console.WriteLine(testh.NameNew);
            }
        }

        class Human
        {
            public string Name { get; set; }
            public int Age { get; set; }

        }
    }
}
