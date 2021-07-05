namespace ItemProject.Configuration
{
    partial class AddCurrencyForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCurrencyName = new System.Windows.Forms.TextBox();
            this.textBoxCurrencySymbol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxExhangeRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxRef = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(30, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "اسم العملة";
            // 
            // textBoxCurrencyName
            // 
            this.textBoxCurrencyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCurrencyName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBoxCurrencyName.Location = new System.Drawing.Point(33, 54);
            this.textBoxCurrencyName.Name = "textBoxCurrencyName";
            this.textBoxCurrencyName.Size = new System.Drawing.Size(225, 23);
            this.textBoxCurrencyName.TabIndex = 0;
            // 
            // textBoxCurrencySymbol
            // 
            this.textBoxCurrencySymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCurrencySymbol.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBoxCurrencySymbol.Location = new System.Drawing.Point(33, 113);
            this.textBoxCurrencySymbol.Name = "textBoxCurrencySymbol";
            this.textBoxCurrencySymbol.Size = new System.Drawing.Size(114, 23);
            this.textBoxCurrencySymbol.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(30, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "رمز العملة";
            // 
            // textBoxExhangeRate
            // 
            this.textBoxExhangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExhangeRate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBoxExhangeRate.Location = new System.Drawing.Point(33, 177);
            this.textBoxExhangeRate.Name = "textBoxExhangeRate";
            this.textBoxExhangeRate.Size = new System.Drawing.Size(114, 23);
            this.textBoxExhangeRate.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(30, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "سعر الصرف";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(30, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "بالنسبة";
            // 
            // textBoxRef
            // 
            this.textBoxRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRef.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBoxRef.Location = new System.Drawing.Point(33, 222);
            this.textBoxRef.Name = "textBoxRef";
            this.textBoxRef.ReadOnly = true;
            this.textBoxRef.Size = new System.Drawing.Size(225, 23);
            this.textBoxRef.TabIndex = 7;
            // 
            // buttonSave
            // 
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Location = new System.Drawing.Point(42, 276);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(105, 38);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "اضافة";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(153, 276);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(105, 38);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "اغلاق";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // AddCurrencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 326);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxRef);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxExhangeRate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCurrencySymbol);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxCurrencyName);
            this.Controls.Add(this.label1);
            this.Name = "AddCurrencyForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "AddCurrency";
            this.Load += new System.EventHandler(this.AddCurrency_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCurrencyName;
        private System.Windows.Forms.TextBox textBoxCurrencySymbol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxExhangeRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxRef;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
    }
}