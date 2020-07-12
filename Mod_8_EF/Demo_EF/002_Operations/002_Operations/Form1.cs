using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Windows.Forms;

namespace _002_Operations
{
    public partial class Form1 : Form
    {
        private ProductDB db;
        public Form1()
        {
            InitializeComponent();

            Database.SetInitializer(new DropCreateDatabaseAlways<ProductDB>());
            db = new ProductDB();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            db.Products.Load();
            dataGridView1.DataSource = db.Products.Local.ToBindingList();
            //tEXTbIND();
        }

        //Add Product
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty || textBox2.Text == String.Empty)
            {
                MessageBox.Show("Текстовые поля не заполнены !");
                return;
            }
            Product product = new Product
            {
                Name = textBox1.Text,
                Price = Convert.ToInt32(textBox2.Text)
            };

            db.Products.Add(product);
            db.SaveChanges();
            dataGridView1.Refresh();

            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
        }

        //Edit Product
        private void button2_Click(object sender, EventArgs e)
        {
            if (label4.Text == String.Empty) return;

            var id = Convert.ToInt32(label4.Text);
            var product = db.Products.Find(id);

            if (product == null) return;

            product.Name = textBox1.Text;
            product.Price = Convert.ToInt32(textBox2.Text);

            db.Entry(product).State = EntityState.Modified;
            db.Products.AddOrUpdate(product);

            db.SaveChanges();

            dataGridView1.Refresh();
        }

        //Delete Product
        private void button3_Click(object sender, EventArgs e)
        {
            if (label4.Text == String.Empty) return;

            var id = Convert.ToInt32(label4.Text);
            var product = db.Products.Find(id);

            db.Entry(product).State = EntityState.Deleted;
            db.Products.Remove(product);

            db.SaveChanges();

            dataGridView1.Refresh();
        }

        private void DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tEXTbIND();
        }

        public void tEXTbIND()
        {
            if (dataGridView1.CurrentRow == null) return;

            var product = dataGridView1.CurrentRow.DataBoundItem as Product;

            if (product == null) return;

            label4.Text = Convert.ToString(product.Id);
            textBox1.Text = product.Name;
            textBox2.Text = Convert.ToString(product.Price);
        }
    }
}
