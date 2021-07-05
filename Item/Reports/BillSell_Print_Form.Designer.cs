namespace ItemProject.Reports
{
    partial class BillSell_Print_Form
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.ItemOUT_ReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewerBillSell = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.ItemOUT_ReportBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ItemOUT_ReportBindingSource
            // 
            this.ItemOUT_ReportBindingSource.DataSource = typeof(ItemProject.Reports.Objects.ItemOUT_Report);
            // 
            // reportViewerBillSell
            // 
            this.reportViewerBillSell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.ItemOUT_ReportBindingSource;
            this.reportViewerBillSell.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewerBillSell.LocalReport.ReportEmbeddedResource = "ItemProject.Reports.ReportBillSell.rdlc";
            this.reportViewerBillSell.Location = new System.Drawing.Point(2, 5);
            this.reportViewerBillSell.Name = "reportViewerBillSell";
            this.reportViewerBillSell.Size = new System.Drawing.Size(628, 399);
            this.reportViewerBillSell.TabIndex = 0;
            // 
            // BillSell_Print_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 406);
            this.Controls.Add(this.reportViewerBillSell);
            this.Name = "BillSell_Print_Form";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "طباعة فاتورة  مبيع";
            this.Load += new System.EventHandler(this.BillSell_Print_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ItemOUT_ReportBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewerBillSell;
        private System.Windows.Forms.BindingSource ItemOUT_ReportBindingSource;
    }
}