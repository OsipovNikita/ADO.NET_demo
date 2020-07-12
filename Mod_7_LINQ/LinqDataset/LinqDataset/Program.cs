using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDataset
{
    /*
     1.Добавьте ссылки на сборки System.Core, System.Data и System.Data.DataSetExtensions
     2.Добавьте директивы using для System.Data и System.LINQ
    
    */
    class Program
    {

        static void Main(string[] args)
        {
            DataSet BookDataSet = new DataSet();
            BookDataSet.ReadXmlSchema("BookDataSet.xsd");
            BookDataSet.ReadXml("Books.xml");

            DataTable books = BookDataSet.Tables["Books"];
            DataTable bookReviews = BookDataSet.Tables["BookReviews"];

            Console.WriteLine("Recent Books:");
            Console.WriteLine("-------------");

            foreach (DataRow xRow in books.Rows)
            {
                Console.WriteLine("{0} by {1}", xRow["Title"], xRow["Publisher"]);
            }


            Console.WriteLine("\n1-------------");
            // декларативный синтаксис запроса
         
          IEnumerable<DataRow> query = 
                                      from bks in books.AsEnumerable()
                                      where bks.Field<long>("BookID") == 1
                                      select bks;

          Console.WriteLine("Books Names:");
          foreach (DataRow p in query)
          {
              Console.WriteLine("{0} by Publisher: {1}", p.Field<string>("Title"), p.Field<string>("Publisher"));
          }

            Console.WriteLine("\n11-------------");
            // используется для возврата последовательности, состоящей только из названий книг

            IEnumerable<string> query11 =
                                        from bks in books.AsEnumerable()
                                        select bks.Field<string>("Title");

            Console.WriteLine("Books Title:");
            foreach (string p in query11)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("\n2-------------");
            /* прямые вызовы методов LINQ
             с передачей им лямбда-выражений в качестве параметров*/

          var query2 = books.AsEnumerable().
                          Select(bks => new
                          {
                              bksTitle = bks.Field<string>("Title"),
                              bksPublisher = bks.Field<string>("Publisher"),
                          });

          Console.WriteLine("Books Names:");
          foreach (var bookInfo in query2)
          {
              Console.WriteLine("book name: {0}, Publisher: {1}.",
                  bookInfo.bksTitle, bookInfo.bksTitle);
          }


            Console.WriteLine("\n3-------------");
            // выполнение двух запросов
 
          IEnumerable<DataRow> query3 =
                          from bksrev in bookReviews.AsEnumerable()
                    //    where bksrev.Field<long>("BookID") == 1
                          select bksrev;

          Console.WriteLine("Books Reviews:");
          foreach (DataRow p in query)
          {
              foreach (DataRow p3 in query3)
              {
                  Console.WriteLine("{0} ------- {1}", p.Field<string>("Title"), p3.Field<string>("Review"));
              }
          }



          Console.WriteLine("\n4-------------");
            // перекрестные запросы между таблицами с помощью соединений
            var query4 =
                  from bk in books.AsEnumerable()
                  join bkrev in bookReviews.AsEnumerable() on bk.Field<long>("BookID") equals bkrev.Field<long>("BookID")
                  //  where bk.Field<long>("BookID") == 1
                  select new
                  {
                      title = bk.Field<string>("Title"),
                      prev = bkrev.Field<string>("Review")
                  };

          Console.WriteLine("Books Reviews Join:");
          foreach (var p in query4)
          {
              Console.WriteLine("book: {0}, Review: {1}.", p.title, p.prev);
          }

            Console.WriteLine("\n5-------------");
            // несколько from (по итогам тоже, что и в запросе 4)
            var query5 =
                  from bk in books.AsEnumerable()
                  from bkrev in bookReviews.AsEnumerable()
                  where bk.Field<long>("BookID") == bkrev.Field<long>("BookID")
                  select new
                  {
                      title = bk.Field<string>("Title"),
                      prev = bkrev.Field<string>("Review")
                  };

            Console.WriteLine("Books Reviews:");
            foreach (var p in query5)
            {
                Console.WriteLine("book: {0}, Review: {1}.", p.title, p.prev);
            }

        }
    }
}
