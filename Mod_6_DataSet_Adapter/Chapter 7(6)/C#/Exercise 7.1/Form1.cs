using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Exercise_7_1
{
    /// <summary>
    /// Выполните скрипт Mod_6_DataSet_Adapter\Chapter 7(6)\SQL\CreateTableDatabaseTest.Sql
    /// для добавления требуемых для проектов этого решения таблиц
    /// Добавьте несколько строк в таблицу UserTable "в ручную" с помощью Обозревателя серверов
    /// </summary>
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void bindingNavigatorSaveItem_Click(object sender, EventArgs e)
		{
			if (this.Validate())
			{
				this.userTableBindingSource.EndEdit();
				this.userTableTableAdapter.Update(this.testDataSet.UserTable);
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(this, "Validation errors occurred.", "Save", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
			}

		}

		private void Form1_Load(object sender, EventArgs e)
		{
            // Эта строка кода загружает данные в таблицу testDataSet.UserTable
            this.userTableTableAdapter.Fill(this.testDataSet.UserTable);

		}
	}
}