using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace WorkingWithDataTablesCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "northwindDataSet1.Customers". При необходимости она может быть перемещена или удалена.
            this.customersTableAdapter.Fill(this.northwindDataSet1.Customers);
            CustomersDataGridView.DataSource = northwindDataSet1.Customers;
            // For this example we will turn off the ability to 
            // edit directly in a cell.
            CustomersDataGridView.MultiSelect = false;
            CustomersDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            CustomersDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;

        }

        private void FillTableButton_Click(object sender, EventArgs e)
        {
            sqlDataAdapter1.Fill(northwindDataSet1.Customers);
        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            // Create a new instance of a Customers row.
            northwindDataSet1.CustomersRow NewRow = (northwindDataSet1.CustomersRow)northwindDataSet1.Customers.NewRow();

            // Set the values for each column in the row.

            NewRow.CustomerID = "WINGT";
            NewRow.CompanyName = "Wingtip Toys";
            NewRow.ContactName = "Steve Lasker";
            NewRow.ContactTitle = "CEO";
            NewRow.Address = "1234 Main Street";
            NewRow.City = "Buffalo";
            NewRow.Region = "NY";
            NewRow.PostalCode = "98052";
            NewRow.Country = "USA";
            NewRow.Phone = "206-555-0111";
            NewRow.Fax = "206-555-0112";


            // Add the new row to the Rows collection of the Customers table. 
            try
            {
                northwindDataSet1.Customers.Rows.Add(NewRow);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Row Failed");
            }
        }

        private northwindDataSet1.CustomersRow  GetSelectedRow()
        {
            // Get the selected DataRow
            String SelectedCustomerID  = CustomersDataGridView.CurrentRow.Cells["CustomerID"].Value.ToString();
            // Using the SelectedCustomerID get the selected row.
            northwindDataSet1.CustomersRow SelectedRow  = northwindDataSet1.Customers.FindByCustomerID(SelectedCustomerID);

            return SelectedRow;
        }

        private void DeleteRowButton_Click(object sender, EventArgs e)
        {
            // Call the Delete method of the selected row to mark it as
            // deleted in the DataTable
            GetSelectedRow().Delete();

        }

        private void UpdateRowVersionDisplay()
        {
            // Display the Original and Current DataRowVersion
            // of the selected Cell
            try
            {
                CurrentDRVTextBox.Text = GetSelectedRow()[CustomersDataGridView.CurrentCell.OwningColumn.Name, DataRowVersion.Current].ToString();
            }
            catch (Exception ex)
            {
                CurrentDRVTextBox.Text = ex.Message;
            }

            try
            {
                OriginalDRVTextBox.Text = GetSelectedRow()[CustomersDataGridView.CurrentCell.OwningColumn.Name, DataRowVersion.Original].ToString();
            }
            catch (Exception ex)
            {
                OriginalDRVTextBox.Text = ex.Message;
            }

            // Display the current RowState of the selected row
            RowStateTextBox.Text = GetSelectedRow().RowState.ToString();
        }

        private void UpdateValueButton_Click(object sender, EventArgs e)
        {
            GetSelectedRow()[CustomersDataGridView.CurrentCell.OwningColumn.Name] = CellValueTextBox.Text;
            UpdateRowVersionDisplay();

        }

        private void CustomersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate the CellValueTextBox with the selected cell
            CellValueTextBox.Text = CustomersDataGridView.CurrentCell.Value.ToString();
            // Refresh the other text boxes
            UpdateRowVersionDisplay();

        }

        private void AcceptChangesButton_Click(object sender, EventArgs e)
        {
            GetSelectedRow().AcceptChanges();
            UpdateRowVersionDisplay();

        }

        private void RejectChangesButton_Click(object sender, EventArgs e)
        {
            GetSelectedRow().RejectChanges();
            UpdateRowVersionDisplay();

        }

 
    }
}