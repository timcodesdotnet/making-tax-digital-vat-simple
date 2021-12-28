namespace TimCodes.Mtd.Vat.App.Forms
{
    partial class VatReturnSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.DataGridSpreadsheet = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSpreadsheet)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridSpreadsheet
            // 
            this.DataGridSpreadsheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridSpreadsheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridSpreadsheet.Location = new System.Drawing.Point(0, 0);
            this.DataGridSpreadsheet.Name = "DataGridSpreadsheet";
            this.DataGridSpreadsheet.RowTemplate.Height = 25;
            this.DataGridSpreadsheet.Size = new System.Drawing.Size(1276, 537);
            this.DataGridSpreadsheet.TabIndex = 0;
            this.DataGridSpreadsheet.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridSpreadsheet_CellDoubleClick);
            // 
            // VatReturnSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 537);
            this.Controls.Add(this.DataGridSpreadsheet);
            this.Name = "VatReturnSelectionForm";
            this.Text = "VAT Return";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSpreadsheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenFileDialog FileDialog;
        private DataGridView DataGridSpreadsheet;
    }
}