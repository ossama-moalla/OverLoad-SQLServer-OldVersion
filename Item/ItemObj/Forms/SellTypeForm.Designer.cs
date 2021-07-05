namespace ItemProject.ItemObj.Forms
{
    partial class SellTypeForm
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
            this.listViewMeasureType = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.Close = new System.Windows.Forms.Button();
            this.labelMeasureType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listViewItemBuySellState = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewMeasureType
            // 
            this.listViewMeasureType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewMeasureType.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewMeasureType.FullRowSelect = true;
            this.listViewMeasureType.Location = new System.Drawing.Point(3, 3);
            this.listViewMeasureType.MultiSelect = false;
            this.listViewMeasureType.Name = "listViewMeasureType";
            this.listViewMeasureType.RightToLeftLayout = true;
            this.listViewMeasureType.Size = new System.Drawing.Size(200, 196);
            this.listViewMeasureType.TabIndex = 3;
            this.listViewMeasureType.UseCompatibleStateImageBehavior = false;
            this.listViewMeasureType.View = System.Windows.Forms.View.Details;
            this.listViewMeasureType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewMeasureType_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "انماط التسعير";
            this.columnHeader1.Width = 182;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.listViewMeasureType);
            this.panel1.Location = new System.Drawing.Point(12, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 204);
            this.panel1.TabIndex = 7;
            // 
            // Close
            // 
            this.Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close.Location = new System.Drawing.Point(206, 266);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(57, 29);
            this.Close.TabIndex = 5;
            this.Close.Text = "اغلاق";
            this.Close.UseVisualStyleBackColor = true;
            // 
            // labelMeasureType
            // 
            this.labelMeasureType.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelMeasureType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMeasureType.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMeasureType.Location = new System.Drawing.Point(12, 13);
            this.labelMeasureType.Name = "labelMeasureType";
            this.labelMeasureType.Size = new System.Drawing.Size(208, 30);
            this.labelMeasureType.TabIndex = 10;
            this.labelMeasureType.Text = "أنماط التسعير";
            this.labelMeasureType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(236, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 30);
            this.label1.TabIndex = 12;
            this.label1.Text = "حالات الشراء و البيع";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.listViewItemBuySellState);
            this.panel2.Location = new System.Drawing.Point(236, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(208, 204);
            this.panel2.TabIndex = 11;
            // 
            // listViewItemBuySellState
            // 
            this.listViewItemBuySellState.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listViewItemBuySellState.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewItemBuySellState.FullRowSelect = true;
            this.listViewItemBuySellState.Location = new System.Drawing.Point(3, 3);
            this.listViewItemBuySellState.MultiSelect = false;
            this.listViewItemBuySellState.Name = "listViewItemBuySellState";
            this.listViewItemBuySellState.RightToLeftLayout = true;
            this.listViewItemBuySellState.Size = new System.Drawing.Size(200, 196);
            this.listViewItemBuySellState.TabIndex = 3;
            this.listViewItemBuySellState.UseCompatibleStateImageBehavior = false;
            this.listViewItemBuySellState.View = System.Windows.Forms.View.Details;
            this.listViewItemBuySellState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewItemBuySellState_MouseDown);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "حالات الشراء و البيع";
            this.columnHeader2.Width = 177;
            // 
            // SellTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 307);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.labelMeasureType);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(472, 345);
            this.MinimumSize = new System.Drawing.Size(472, 345);
            this.Name = "SellTypeForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "اصناف القياس";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listViewMeasureType;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Label labelMeasureType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView listViewItemBuySellState;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}