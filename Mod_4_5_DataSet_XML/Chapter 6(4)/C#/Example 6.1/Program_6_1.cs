#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data ;
using System.Diagnostics;

#endregion

namespace Example_6_1
{
    /// <summary>
    /// Касс DataTable не зависит от DataSet, в том смысле, что объекты DataTable можно использовать независимо от DataSet. 
    /// Класс DataTable содержит все методы, поддерживаемые DataSet.
    /// Для работы с одной таблицей в большинстве случаев лучше использовать объект DataTable.
    /// </summary>

    class Program_6_1
    {
        static void Main()
        {
            // Create the table
            DataTable productsTable = new DataTable("Products");
            
            // Класс DataColumn используется для определения имени и типа данных столбца в DataTable. 
            
            // Build the Products schema
            productsTable.Columns.Add("ID", typeof(System.Int32)) ;
            productsTable.Columns.Add("Name", typeof(System.String)) ;
            productsTable.Columns.Add("Category", typeof(System.Int32)) ;

            // Set up the ID column as the primary key
            /*
             В DataTable первичный ключ определяется как массив объектов DataColumn, 
             которые все вместе обеспечивают уникальность идентификатора для DataRow в DataTable.
             */
            productsTable.PrimaryKey = 
                new DataColumn[] { productsTable.Columns["ID"] };

            productsTable.Columns["ID"].AutoIncrement = true ;
            productsTable.Columns["ID"].AutoIncrementSeed = 1 ;
            productsTable.Columns["ID"].ReadOnly = true ;

            /*
             Для создания в DataTable новой строки, необходимо:
              1) вызвать метод DataTable.NewRow(), который возвращает объект DataRow, 
                удовлетворяющий текущей схеме DataTable,
              2) установить значение каждого столбца из DataRow,
              3) вызвать метод DataTable.Rows.Add(), передав в качестве 
                его единственного аргумента только что созданный объект DataRow
             */

            DataRow tempRow;
            for (int i = 0; i < 10; i++)
            {
                tempRow = productsTable.NewRow();                                   // 1
                Debug.WriteLine(tempRow.RowState);                  // Detached
                // Make every even row Caterham Seven de Dion
                if (Math.IEEERemainder(i, 2) == 0)
                {
                    tempRow["Name"] = "Caterham Seven de Dion #" + i.ToString() ;   // 2
                    tempRow["Category"] = 1;
                }
                else
                {
                    tempRow["Name"] = "Dodge Viper #" + i.ToString() ;
                    tempRow["Category"] = 2;
                }
                productsTable.Rows.Add(tempRow);                                    // 3
                Debug.WriteLine(tempRow.RowState);                  // Added
            }
            productsTable.WriteXml("productsTable.xml") ;
        }
    }
}
