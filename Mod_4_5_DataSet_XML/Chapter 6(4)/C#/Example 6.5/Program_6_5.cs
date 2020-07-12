#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

#endregion

namespace Example_6_5
{
   class Program
   {
        /// <summary>
        /// Строго типизированный DataSet — это класс DataSet, 
        /// сгенерированный с помощью XML-схемы, и, следовательно, 
        /// хорошо соответствующий этой конкретной XML-схеме
        /// </summary>
        /// <param name="args"></param>
        static void Main()
      {
         DataSet BookDataSet = new DataSet();

            /*
             XML-схема — это ХМL-документ, определяющий структуру других XML-документов 
             с помощью описания структур и типов элементов, которые могут быть использованы в этих документах
            */
            BookDataSet.ReadXmlSchema("BookDataSet.xsd"); // Типизированные объекты DataSet наследуют свою схему из файла .xsd и содержат явно типизированные коллекции

            BookDataSet.ReadXml("Books.xml");

         Console.WriteLine("Recent Books:");
         Console.WriteLine("-------------");

         foreach (DataRow xRow in BookDataSet.Tables["Books"].Rows)
         {
            Console.WriteLine("{0} by {1}", xRow["Title"], xRow["Publisher"]);
         }
      }
   }
}
