using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataViewExampleCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // Create a DataView for the Customers and 
        // Orders tables.
        DataView customersDataView;
        DataView ordersDataView;


        private void Form1_Load(object sender, EventArgs e)
        {
            // Load the Customers and Orders tables with data
            customersTableAdapter1.Fill(northwindDataSet1.Customers);
            ordersTableAdapter1.Fill(northwindDataSet1.Orders);

            // Set the DataViews to use the Customers and Orders tables
            customersDataView = new DataView(northwindDataSet1.Customers);
            ordersDataView = new DataView(northwindDataSet1.Orders);

            // Set the initial Sort order of the DataView
            customersDataView.Sort = "CustomerID";

            // Set the CustomersGrid to display the CustomersDataView
            CustomersGrid.DataSource = customersDataView;

        }

        private void SetDataViewPropertiesButton_Click(object sender, EventArgs e)
        {
            // C#
            customersDataView.Sort = SortTextBox.Text;
            customersDataView.RowFilter = FilterTextBox.Text;

        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            // Create a new row
            DataRowView newCustomerRow = customersDataView.AddNew();

            // Set a couple of column values
            newCustomerRow["CustomerID"] = "WINGT";
            newCustomerRow["CompanyName"] = "Wing Tip Toys";

            // Commit the row by explicitly ending the edit
            newCustomerRow.EndEdit();

        }

        private void GetOrdersButton_Click(object sender, EventArgs e)
        {
            // Get the CustomerID for the row selected in the CustomersGrid
            string selectedCustomerID =
               (string)CustomersGrid.SelectedCells[0].OwningRow.Cells["CustomerID"].Value;

            // Create a DataRowView and assign it the selected row.
            DataRowView selectedRow = customersDataView[customersDataView.Find(selectedCustomerID)];

            // Call the CreateChildView method to 
            // navigate the relationship and create a 
            // new DataView based on the related records.
            ordersDataView = selectedRow.CreateChildView
                (northwindDataSet1.Relations["FK_Orders_Customers"]);

            // Set the OrdersGrid to display the related Dataview
            OrdersGrid.DataSource = ordersDataView;

        }
    }
}