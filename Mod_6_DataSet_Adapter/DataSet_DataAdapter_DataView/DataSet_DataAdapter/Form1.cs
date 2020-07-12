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
        DataView productsDataView;
        decimal up;

        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Value = 10;
        }
        /// <summary>
        /// Теперь грид отображает не объект DataSet, а его "представление" - DataView.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = productsDataView;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            productsTableAdapter.Update(northwindDataSet.Products);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            up = numericUpDown1.Value;
            this.productsTableAdapter.Fill(this.northwindDataSet.Products, up);

            // Заполнение DataView
            productsDataView = new DataView(northwindDataSet.Products);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Настройка DataView для сортировки и фильтрации
            productsDataView.Sort = "ProductName";
            productsDataView.RowFilter = String.Format("UnitPrice < {0} ", numericUpDown1.Value);
            // Но надо помнить, что в данном случае верхний предел фильтра будет тем значением,
            // которое указано в конструкторе формы для numericUpDown
        }
    }
}
