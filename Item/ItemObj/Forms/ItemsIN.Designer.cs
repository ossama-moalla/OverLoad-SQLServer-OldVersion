namespace ItemProject.ItemObj.Forms
{
    partial class ItemsIN
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.Item = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Owner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Cost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SellPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Notes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Item,
            this.ItemState,
            this.Amount,
            this.Owner,
            this.Cost,
            this.SellPrice,
            this.Notes});
            this.listView1.Location = new System.Drawing.Point(27, 74);
            this.listView1.Name = "listView1";
            this.listView1.RightToLeftLayout = true;
            this.listView1.Size = new System.Drawing.Size(726, 325);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Item
            // 
            this.Item.Text = "العنصر";
            this.Item.Width = 120;
            // 
            // ItemState
            // 
            this.ItemState.Text = "حالة العنصر";
            this.ItemState.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ItemState.Width = 102;
            // 
            // Amount
            // 
            this.Amount.Text = "الكمية";
            this.Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Owner
            // 
            this.Owner.Text = "المالك";
            this.Owner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Cost
            // 
            this.Cost.Text = "التكلفة";
            this.Cost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Cost.Width = 78;
            // 
            // SellPrice
            // 
            this.SellPrice.Text = "سعر المبيع";
            this.SellPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SellPrice.Width = 73;
            // 
            // Notes
            // 
            this.Notes.Text = "ملاحظات";
            this.Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Notes.Width = 204;
            // 
            // ItemsIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 417);
            this.Controls.Add(this.listView1);
            this.Name = "ItemsIN";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "ItemsIN";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Item;
        private System.Windows.Forms.ColumnHeader ItemState;
        private System.Windows.Forms.ColumnHeader Amount;
        private System.Windows.Forms.ColumnHeader Owner;
        private System.Windows.Forms.ColumnHeader Cost;
        private System.Windows.Forms.ColumnHeader SellPrice;
        private System.Windows.Forms.ColumnHeader Notes;
    }
}