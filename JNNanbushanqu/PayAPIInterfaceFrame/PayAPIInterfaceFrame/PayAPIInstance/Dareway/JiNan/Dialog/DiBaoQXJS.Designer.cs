namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    partial class DiBaoQXJS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiBaoQXJS));
            this.axSDACard1 = new AxSDACARDLib.AxSDACard();
            ((System.ComponentModel.ISupportInitialize)(this.axSDACard1)).BeginInit();
            this.SuspendLayout();
            // 
            // axSDACard1
            // 
            this.axSDACard1.Enabled = true;
            this.axSDACard1.Location = new System.Drawing.Point(230, 41);
            this.axSDACard1.Name = "axSDACard1";
            this.axSDACard1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSDACard1.OcxState")));
            this.axSDACard1.Size = new System.Drawing.Size(100, 50);
            this.axSDACard1.TabIndex = 3;
            this.axSDACard1.Visible = false;
            // 
            // DiBaoQXJS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 114);
            this.ControlBox = false;
            this.Controls.Add(this.axSDACard1);
            this.Name = "DiBaoQXJS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "取消结算";
            this.Load += new System.EventHandler(this.DiBaoQXJS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axSDACard1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxSDACARDLib.AxSDACard axSDACard1;
    }
}