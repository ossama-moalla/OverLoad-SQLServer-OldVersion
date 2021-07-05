namespace ItemProject.Trade.Forms.TradeForms
{
    partial class BillOUTReportForm
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
            this.panelListView = new System.Windows.Forms.Panel();
            this.listView = new System.Windows.Forms.ListView();
            this.columnNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnItemCompany = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnItemFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnItemstate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnConsumeUnit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnItemPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTotalPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxClearValue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxDiscount = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.TextboxNotes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxContact = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelBillID = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker_ = new System.Windows.Forms.DateTimePicker();
            this.textBoxBillOUTExchangeRate = new System.Windows.Forms.TextBox();
            this.labelExchangeRate = new System.Windows.Forms.Label();
            this.panelShowBillDataByCurrency = new System.Windows.Forms.Panel();
            this.comboBoxCurrency = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxExchangeRate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxCUrrentExchangeRate = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxCurrency = new System.Windows.Forms.TextBox();
            this.panelListView.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelShowBillDataByCurrency.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelListView
            // 
            this.panelListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelListView.Controls.Add(this.listView);
            this.panelListView.Location = new System.Drawing.Point(9, 175);
            this.panelListView.Name = "panelListView";
            this.panelListView.Size = new System.Drawing.Size(1150, 325);
            this.panelListView.TabIndex = 0;
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnNo,
            this.columnItem,
            this.columnItemCompany,
            this.columnItemFolder,
            this.columnItemstate,
            this.columnAmount,
            this.columnConsumeUnit,
            this.columnItemPrice,
            this.columnTotalPrice});
            this.listView.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(2, 5);
            this.listView.Name = "listView";
            this.listView.RightToLeftLayout = true;
            this.listView.Size = new System.Drawing.Size(1141, 315);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            //this.listView.Resize += new System.EventHandler(this.listView_Resize);
            // 
            // columnNo
            // 
            this.columnNo.Text = "متسلسل";
            this.columnNo.Width = 80;
            // 
            // columnItem
            // 
            this.columnItem.Text = "العنصر";
            this.columnItem.Width = 140;
            // 
            // columnItemCompany
            // 
            this.columnItemCompany.Text = "الشركة";
            this.columnItemCompany.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnItemCompany.Width = 124;
            // 
            // columnItemFolder
            // 
            this.columnItemFolder.Text = "الصنف";
            this.columnItemFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnItemFolder.Width = 112;
            // 
            // columnItemstate
            // 
            this.columnItemstate.Text = "حالة العنصر";
            this.columnItemstate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnItemstate.Width = 125;
            // 
            // columnAmount
            // 
            this.columnAmount.Text = "الكمية";
            this.columnAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnAmount.Width = 85;
            // 
            // columnConsumeUnit
            // 
            this.columnConsumeUnit.Text = "الوحدة";
            this.columnConsumeUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnConsumeUnit.Width = 100;
            // 
            // columnItemPrice
            // 
            this.columnItemPrice.Text = "السعر المفرد";
            this.columnItemPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnItemPrice.Width = 225;
            // 
            // columnTotalPrice
            // 
            this.columnTotalPrice.Text = "الاجمالي";
            this.columnTotalPrice.Width = 210;
            // 
            // panel3
            // 
            this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.textBoxClearValue);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.textBoxValue);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.textBoxDiscount);
            this.panel3.Controls.Add(this.buttonClose);
            this.panel3.Location = new System.Drawing.Point(230, 506);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(893, 128);
            this.panel3.TabIndex = 13;
            // 
            // textBoxClearValue
            // 
            this.textBoxClearValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxClearValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxClearValue.Location = new System.Drawing.Point(100, 31);
            this.textBoxClearValue.Multiline = true;
            this.textBoxClearValue.Name = "textBoxClearValue";
            this.textBoxClearValue.ReadOnly = true;
            this.textBoxClearValue.Size = new System.Drawing.Size(235, 31);
            this.textBoxClearValue.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(741, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 16);
            this.label7.TabIndex = 7;
            this.label7.Text = "قيمة الفاتورة";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(498, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 16);
            this.label8.TabIndex = 9;
            this.label8.Text = "الخصم";
            // 
            // textBoxValue
            // 
            this.textBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxValue.Location = new System.Drawing.Point(598, 31);
            this.textBoxValue.Multiline = true;
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.ReadOnly = true;
            this.textBoxValue.Size = new System.Drawing.Size(226, 31);
            this.textBoxValue.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(283, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 16);
            this.label9.TabIndex = 11;
            this.label9.Text = "الصافي";
            // 
            // textBoxDiscount
            // 
            this.textBoxDiscount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDiscount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDiscount.Location = new System.Drawing.Point(413, 31);
            this.textBoxDiscount.Multiline = true;
            this.textBoxDiscount.Name = "textBoxDiscount";
            this.textBoxDiscount.Size = new System.Drawing.Size(131, 31);
            this.textBoxDiscount.TabIndex = 8;
            this.textBoxDiscount.Text = "0";
            //this.textBoxDiscount.TextChanged += new System.EventHandler(this.textBoxDiscount_TextChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonClose.Location = new System.Drawing.Point(446, 81);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(114, 37);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "اغلاق";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // TextboxNotes
            // 
            this.TextboxNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextboxNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextboxNotes.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextboxNotes.Location = new System.Drawing.Point(397, 108);
            this.TextboxNotes.Name = "TextboxNotes";
            this.TextboxNotes.ReadOnly = true;
            this.TextboxNotes.Size = new System.Drawing.Size(186, 26);
            this.TextboxNotes.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(524, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "ملاحظات";
            // 
            // textBoxContact
            // 
            this.textBoxContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxContact.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxContact.Location = new System.Drawing.Point(839, 60);
            this.textBoxContact.Name = "textBoxContact";
            this.textBoxContact.ReadOnly = true;
            this.textBoxContact.Size = new System.Drawing.Size(275, 26);
            this.textBoxContact.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1074, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "الجهة";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(781, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "التاريخ";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDescription.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(839, 108);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.Size = new System.Drawing.Size(275, 26);
            this.textBoxDescription.TabIndex = 6;
            // 
            // labelBillID
            // 
            this.labelBillID.BackColor = System.Drawing.Color.Aquamarine;
            this.labelBillID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelBillID.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelBillID.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBillID.Location = new System.Drawing.Point(0, 0);
            this.labelBillID.Name = "labelBillID";
            this.labelBillID.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelBillID.Size = new System.Drawing.Size(1145, 36);
            this.labelBillID.TabIndex = 10;
            this.labelBillID.Text = "فاتورة استيراد مواد";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(777, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "العملة";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1068, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "الوصف";
            // 
            // dateTimePicker_
            // 
            this.dateTimePicker_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_.CalendarFont = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_.CustomFormat = " yyyy/MM/dd      mm:hh";
            this.dateTimePicker_.Enabled = false;
            this.dateTimePicker_.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.dateTimePicker_.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_.Location = new System.Drawing.Point(614, 108);
            this.dateTimePicker_.Name = "dateTimePicker_";
            this.dateTimePicker_.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dateTimePicker_.Size = new System.Drawing.Size(209, 26);
            this.dateTimePicker_.TabIndex = 15;
            // 
            // textBoxBillOUTExchangeRate
            // 
            this.textBoxBillOUTExchangeRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBillOUTExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBillOUTExchangeRate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBillOUTExchangeRate.Location = new System.Drawing.Point(397, 63);
            this.textBoxBillOUTExchangeRate.Name = "textBoxBillOUTExchangeRate";
            this.textBoxBillOUTExchangeRate.ReadOnly = true;
            this.textBoxBillOUTExchangeRate.Size = new System.Drawing.Size(189, 26);
            this.textBoxBillOUTExchangeRate.TabIndex = 16;
            // 
            // labelExchangeRate
            // 
            this.labelExchangeRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExchangeRate.AutoSize = true;
            this.labelExchangeRate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExchangeRate.Location = new System.Drawing.Point(397, 41);
            this.labelExchangeRate.Name = "labelExchangeRate";
            this.labelExchangeRate.Size = new System.Drawing.Size(192, 16);
            this.labelExchangeRate.TabIndex = 18;
            this.labelExchangeRate.Text = "سعر الصرف عند انشاء الفاتورة";
            // 
            // panelShowBillDataByCurrency
            // 
            this.panelShowBillDataByCurrency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelShowBillDataByCurrency.Controls.Add(this.checkBoxCUrrentExchangeRate);
            this.panelShowBillDataByCurrency.Controls.Add(this.label1);
            this.panelShowBillDataByCurrency.Controls.Add(this.textBoxExchangeRate);
            this.panelShowBillDataByCurrency.Controls.Add(this.label10);
            this.panelShowBillDataByCurrency.Controls.Add(this.comboBoxCurrency);
            this.panelShowBillDataByCurrency.Location = new System.Drawing.Point(3, 44);
            this.panelShowBillDataByCurrency.Name = "panelShowBillDataByCurrency";
            this.panelShowBillDataByCurrency.Size = new System.Drawing.Size(388, 90);
            this.panelShowBillDataByCurrency.TabIndex = 19;
            // 
            // comboBoxCurrency
            // 
            this.comboBoxCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCurrency.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.comboBoxCurrency.FormattingEnabled = true;
            this.comboBoxCurrency.Location = new System.Drawing.Point(161, 59);
            this.comboBoxCurrency.Name = "comboBoxCurrency";
            this.comboBoxCurrency.Size = new System.Drawing.Size(206, 26);
            this.comboBoxCurrency.TabIndex = 23;
            //this.comboBoxCurrency.SelectedIndexChanged += new System.EventHandler(this.comboBoxCurrency_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(323, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 16);
            this.label10.TabIndex = 24;
            this.label10.Text = "العملة";
            // 
            // textBoxExchangeRate
            // 
            this.textBoxExchangeRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExchangeRate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExchangeRate.Location = new System.Drawing.Point(31, 59);
            this.textBoxExchangeRate.Name = "textBoxExchangeRate";
            this.textBoxExchangeRate.ReadOnly = true;
            this.textBoxExchangeRate.Size = new System.Drawing.Size(101, 26);
            this.textBoxExchangeRate.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 26;
            this.label1.Text = "سعر الصرف";
            // 
            // checkBoxCUrrentExchangeRate
            // 
            this.checkBoxCUrrentExchangeRate.AutoSize = true;
            this.checkBoxCUrrentExchangeRate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkBoxCUrrentExchangeRate.Location = new System.Drawing.Point(70, 3);
            this.checkBoxCUrrentExchangeRate.Name = "checkBoxCUrrentExchangeRate";
            this.checkBoxCUrrentExchangeRate.Size = new System.Drawing.Size(313, 20);
            this.checkBoxCUrrentExchangeRate.TabIndex = 28;
            this.checkBoxCUrrentExchangeRate.Text = "عرض قيمة الفاتورة حسب جدول الصرف الحالي";
            this.checkBoxCUrrentExchangeRate.UseVisualStyleBackColor = true;
            //this.checkBoxCUrrentExchangeRate.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.textBoxCurrency);
            this.panel2.Controls.Add(this.panelShowBillDataByCurrency);
            this.panel2.Controls.Add(this.labelExchangeRate);
            this.panel2.Controls.Add(this.textBoxBillOUTExchangeRate);
            this.panel2.Controls.Add(this.dateTimePicker_);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.labelBillID);
            this.panel2.Controls.Add(this.textBoxDescription);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBoxContact);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.TextboxNotes);
            this.panel2.Location = new System.Drawing.Point(12, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1147, 148);
            this.panel2.TabIndex = 1;
            // 
            // textBoxCurrency
            // 
            this.textBoxCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurrency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCurrency.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCurrency.Location = new System.Drawing.Point(614, 60);
            this.textBoxCurrency.Name = "textBoxCurrency";
            this.textBoxCurrency.ReadOnly = true;
            this.textBoxCurrency.Size = new System.Drawing.Size(209, 26);
            this.textBoxCurrency.TabIndex = 29;
            // 
            // BillOUTReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Moccasin;
            this.ClientSize = new System.Drawing.Size(1171, 637);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelListView);
            this.Name = "BillOUTReportForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TradeInAddForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelListView.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelShowBillDataByCurrency.ResumeLayout(false);
            this.panelShowBillDataByCurrency.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelListView;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnItem;
        private System.Windows.Forms.ColumnHeader columnItemstate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBoxClearValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDiscount;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ColumnHeader columnAmount;
        private System.Windows.Forms.ColumnHeader columnItemPrice;
        private System.Windows.Forms.ColumnHeader columnConsumeUnit;
        private System.Windows.Forms.ColumnHeader columnItemFolder;
        private System.Windows.Forms.ColumnHeader columnItemCompany;
        private System.Windows.Forms.ColumnHeader columnNo;
        private System.Windows.Forms.ColumnHeader columnTotalPrice;
        private System.Windows.Forms.TextBox TextboxNotes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxContact;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelBillID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker_;
        private System.Windows.Forms.TextBox textBoxBillOUTExchangeRate;
        private System.Windows.Forms.Label labelExchangeRate;
        private System.Windows.Forms.Panel panelShowBillDataByCurrency;
        private System.Windows.Forms.CheckBox checkBoxCUrrentExchangeRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxExchangeRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxCurrency;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxCurrency;
    }
}