#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

#endregion

namespace Example_6_6
{
class Program_6_6
{
   static void Main()
   {
      DataSet BookDataSet = new DataSet();

      BookDataSet.ReadXmlSchema("BookDataSet.xsd"); // Проверьте работу приложения без загрузки схемы
      BookDataSet.ReadXml("Books.xml");

      Console.WriteLine("Relations Created:");
      
      foreach (DataRelation xRelation in BookDataSet.Relations)
      {
         Console.WriteLine(xRelation.RelationName);
      }

      Console.WriteLine("Apress Books");
      Console.WriteLine("----------");
      Console.WriteLine();

      foreach (DataRow xRow in BookDataSet.Tables["Books"].Rows)
      {
         Console.WriteLine(xRow["Title"]);

         // Obtain child rows using the KeyTitleRef relation
         foreach (DataRow zRow in xRow.GetChildRows("KeyTitleRef"))
         {
            Console.WriteLine("  {0}", zRow["Rating"]);
         }
      }
   }
}
}
