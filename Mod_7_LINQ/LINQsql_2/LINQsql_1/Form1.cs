using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LINQsql_1
{
   
    public partial class Form1 : Form
    {
        DataContext db ; 
        public Form1()
        {
           
            InitializeComponent(); 
            db = new DataContext(sqlConnection1);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //            DataContext db = new DataContext
            //(@"Data Source=(local);Initial Catalog=Northwind;Integrated Security=True");

            // загружаются данные из двух таблиц
            var custQuery =
                          from cust in db.GetTable<Customer>()
                          where cust.Orders.Any()
                          select cust;

            foreach (var custObj in custQuery)
            {
                ListViewItem item =
                    listView1.Items.Add(custObj.CustomerID.ToString());
                item.SubItems.Add(custObj.City.ToString());
                item.SubItems.Add(custObj.Orders.Count.ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Region custnew = new Region();

            custnew.RegionID = int.Parse(txtCustomerCode.Text);
            custnew.RegionDescription = txtCusromerCity.Text;

            db.GetTable<Region>().InsertOnSubmit(custnew);
            db.SubmitChanges();

            //var results = from c in db.GetTable<Region>()
            //              select c;
            // можно записать короче
            var results = db.GetTable<Region>();

            foreach (var c in results)
                MessageBox.Show(c.ToString(), "Содержимое таблицы Region");
        }
    }



    
}
