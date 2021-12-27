namespace TimCodes.Mtd.Vat.App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LblSignedInAs = new System.Windows.Forms.Label();
            this.DataGridObligations = new System.Windows.Forms.DataGridView();
            this.BtnToggleSignin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridObligations)).BeginInit();
            this.SuspendLayout();
            // 
            // LblSignedInAs
            // 
            this.LblSignedInAs.AutoSize = true;
            this.LblSignedInAs.Location = new System.Drawing.Point(10, 15);
            this.LblSignedInAs.Name = "LblSignedInAs";
            this.LblSignedInAs.Size = new System.Drawing.Size(0, 15);
            this.LblSignedInAs.TabIndex = 1;
            this.LblSignedInAs.Visible = false;
            // 
            // DataGridObligations
            // 
            this.DataGridObligations.AllowUserToAddRows = false;
            this.DataGridObligations.AllowUserToDeleteRows = false;
            this.DataGridObligations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridObligations.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DataGridObligations.Location = new System.Drawing.Point(0, 41);
            this.DataGridObligations.Name = "DataGridObligations";
            this.DataGridObligations.ReadOnly = true;
            this.DataGridObligations.RowTemplate.Height = 25;
            this.DataGridObligations.Size = new System.Drawing.Size(800, 409);
            this.DataGridObligations.TabIndex = 2;
            // 
            // BtnToggleSignin
            // 
            this.BtnToggleSignin.Location = new System.Drawing.Point(712, 7);
            this.BtnToggleSignin.Name = "BtnToggleSignin";
            this.BtnToggleSignin.Size = new System.Drawing.Size(75, 23);
            this.BtnToggleSignin.TabIndex = 3;
            this.BtnToggleSignin.Text = "Sign In";
            this.BtnToggleSignin.UseVisualStyleBackColor = true;
            this.BtnToggleSignin.Click += new System.EventHandler(this.BtnToggleSignin_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnToggleSignin);
            this.Controls.Add(this.DataGridObligations);
            this.Controls.Add(this.LblSignedInAs);
            this.Name = "MainForm";
            this.Text = "Making Tax Digital (VAT)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridObligations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label LblSignedInAs;
        private DataGridView DataGridObligations;
        private Button BtnToggleSignin;
    }
}