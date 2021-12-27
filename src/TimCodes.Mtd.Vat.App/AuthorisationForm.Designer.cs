namespace TimCodes.Mtd.Vat.App
{
    partial class AuthorisationForm
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
            this.WebSignIn = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.WebSignIn)).BeginInit();
            this.SuspendLayout();
            // 
            // webSignIn
            // 
            this.WebSignIn.CreationProperties = null;
            this.WebSignIn.DefaultBackgroundColor = System.Drawing.Color.White;
            this.WebSignIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebSignIn.Location = new System.Drawing.Point(0, 0);
            this.WebSignIn.Name = "webSignIn";
            this.WebSignIn.Size = new System.Drawing.Size(847, 614);
            this.WebSignIn.TabIndex = 0;
            this.WebSignIn.ZoomFactor = 1D;
            this.WebSignIn.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.WebSignIn_NavigationCompleted);
            // 
            // AuthorisationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 614);
            this.Controls.Add(this.WebSignIn);
            this.Name = "AuthorisationForm";
            this.Text = "Making Tax Digital - Authorisation";
            ((System.ComponentModel.ISupportInitialize)(this.WebSignIn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 WebSignIn;
    }
}