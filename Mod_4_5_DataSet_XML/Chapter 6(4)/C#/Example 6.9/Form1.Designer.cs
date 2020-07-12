namespace Example_6_9
{
   partial class Form1
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgBooks = new System.Windows.Forms.DataGridView();
            this.btnSumScores = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgBooks)).BeginInit();
            this.SuspendLayout();
            // 
            // dgBooks
            // 
            this.dgBooks.AllowUserToAddRows = false;
            this.dgBooks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgBooks.BackgroundColor = System.Drawing.Color.Lavender;
            this.dgBooks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.MidnightBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Lavender;
            this.dgBooks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.GhostWhite;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.MidnightBlue;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgBooks.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgBooks.Font = new System.Drawing.Font("Tahoma", 8F);
            this.dgBooks.GridColor = System.Drawing.Color.RoyalBlue;
            this.dgBooks.Location = new System.Drawing.Point(7, 4);
            this.dgBooks.Name = "dgBooks";
            this.dgBooks.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgBooks.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgBooks.Size = new System.Drawing.Size(443, 267);
            this.dgBooks.TabIndex = 0;
            // 
            // btnSumScores
            // 
            this.btnSumScores.Location = new System.Drawing.Point(368, 277);
            this.btnSumScores.Name = "btnSumScores";
            this.btnSumScores.Size = new System.Drawing.Size(75, 23);
            this.btnSumScores.TabIndex = 1;
            this.btnSumScores.Text = "Sum Scores";
            this.btnSumScores.Click += new System.EventHandler(this.btnSumScores_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(455, 307);
            this.Controls.Add(this.btnSumScores);
            this.Controls.Add(this.dgBooks);
            this.Name = "Form1";
            this.Text = "Annotated Typed Dataset Binding Example";
            ((System.ComponentModel.ISupportInitialize)(this.dgBooks)).EndInit();
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView dgBooks;
      private System.Windows.Forms.Button btnSumScores;
   }
}

