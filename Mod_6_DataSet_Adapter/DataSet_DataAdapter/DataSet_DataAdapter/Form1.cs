using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSet_DataAdapter
{
    public partial class Form1 : Form
    {
        decimal up;
        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Value = 10;
        }

        /// <summary>
        /// Каждая таблица в наборе данных имеет связанный объект TableAdapter.
        /// Адаптер таблицы используется для заполнения набора данных и, 
        /// при необходимости, для отправки команд в базу данных
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            up = numericUpDown1.Value;
            this.productsTableAdapter.Fill(this.northwindDataSet.Products, up);
        }

        /// <summary>
        /// При внесении изменений в записи в наборе данных сведения об этих изменениях сохраняются до их фиксации.
        /// Изменения фиксируются при вызове метода AcceptChanges набора данных или таблицы данных, а 
        /// также при вызове метода Update адаптеров TableAdapter или адаптера данных
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Метод HasChanges набора данных возвращает true, если в наборе данных были внесены изменения.
            // Определив, что измененные строки существуют, можно вызвать метод GetChanges DataSet или DataTable,
            // чтобы вернуть набор измененных строк
            // Можно также проверить, какие типы изменений были сделаны в наборе данных, передав значение из
            // перечисления DataRowState в метод HasChanges
            if (northwindDataSet.HasChanges(DataRowState.Modified))
            {
                // в объект DataTable возвращается набор измененных строк
                DataTable dtb = northwindDataSet.Products.GetChanges(/*DataRowState.Modified*/);
                // изменения сохраняются в файл
                dtb.WriteXml("productsTable.xml");
            }
            else
            {
                // No changed rows were detected, add appropriate code.
            }

            productsTableAdapter.Update(northwindDataSet.Products);
        }
    }
}
