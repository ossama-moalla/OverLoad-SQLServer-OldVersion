namespace ItemProject.AccountingObj.Forms
{
    partial class EmployeeUserForm
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
            this.buttonSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxDisable = new System.Windows.Forms.ComboBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.labelusername = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxPasswordConfirm = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEmployeeID = new System.Windows.Forms.TextBox();
            this.textBoxEmployeeName = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(154, 12);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(114, 37);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "حفظ";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.LightGreen;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.buttonCancel);
            this.panel3.Controls.Add(this.buttonSave);
            this.panel3.Location = new System.Drawing.Point(12, 327);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(282, 64);
            this.panel3.TabIndex = 5;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(9, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(114, 37);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "اغلاق";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(215, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "الحالة";
            // 
            // comboBoxDisable
            // 
            this.comboBoxDisable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDisable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisable.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.comboBoxDisable.FormattingEnabled = true;
            this.comboBoxDisable.ItemHeight = 18;
            this.comboBoxDisable.Items.AddRange(new object[] {
            "ممكن",
            "معطل"});
            this.comboBoxDisable.Location = new System.Drawing.Point(100, 173);
            this.comboBoxDisable.Name = "comboBoxDisable";
            this.comboBoxDisable.Size = new System.Drawing.Size(165, 26);
            this.comboBoxDisable.TabIndex = 55;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUserName.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxUserName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.textBoxUserName.Location = new System.Drawing.Point(28, 29);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(236, 26);
            this.textBoxUserName.TabIndex = 0;
            // 
            // labelusername
            // 
            this.labelusername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelusername.AutoSize = true;
            this.labelusername.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelusername.Location = new System.Drawing.Point(159, 10);
            this.labelusername.Name = "labelusername";
            this.labelusername.Size = new System.Drawing.Size(103, 16);
            this.labelusername.TabIndex = 50;
            this.labelusername.Text = "اسم المستخدم";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.textBoxPasswordConfirm);
            this.panel2.Controls.Add(this.textBoxPassword);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.comboBoxDisable);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.textBoxUserName);
            this.panel2.Controls.Add(this.labelusername);
            this.panel2.Location = new System.Drawing.Point(12, 117);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(282, 204);
            this.panel2.TabIndex = 4;
            // 
            // textBoxPasswordConfirm
            // 
            this.textBoxPasswordConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPasswordConfirm.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPasswordConfirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPasswordConfirm.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.textBoxPasswordConfirm.Location = new System.Drawing.Point(29, 125);
            this.textBoxPasswordConfirm.Name = "textBoxPasswordConfirm";
            this.textBoxPasswordConfirm.Size = new System.Drawing.Size(236, 26);
            this.textBoxPasswordConfirm.TabIndex = 2;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPassword.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPassword.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.textBoxPassword.Location = new System.Drawing.Point(28, 77);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(236, 26);
            this.textBoxPassword.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(161, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 16);
            this.label4.TabIndex = 54;
            this.label4.Text = "تأكيد كلمة السر";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(190, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 52;
            this.label3.Text = "كلمة السر";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.NavajoWhite;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxEmployeeID);
            this.panel1.Controls.Add(this.textBoxEmployeeName);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Location = new System.Drawing.Point(12, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 107);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(219, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "الاسم";
            // 
            // textBoxEmployeeID
            // 
            this.textBoxEmployeeID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmployeeID.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxEmployeeID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEmployeeID.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.textBoxEmployeeID.Location = new System.Drawing.Point(147, 24);
            this.textBoxEmployeeID.Name = "textBoxEmployeeID";
            this.textBoxEmployeeID.ReadOnly = true;
            this.textBoxEmployeeID.Size = new System.Drawing.Size(117, 26);
            this.textBoxEmployeeID.TabIndex = 47;
            // 
            // textBoxEmployeeName
            // 
            this.textBoxEmployeeName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmployeeName.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxEmployeeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEmployeeName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.textBoxEmployeeName.Location = new System.Drawing.Point(29, 76);
            this.textBoxEmployeeName.Name = "textBoxEmployeeName";
            this.textBoxEmployeeName.ReadOnly = true;
            this.textBoxEmployeeName.Size = new System.Drawing.Size(236, 26);
            this.textBoxEmployeeName.TabIndex = 49;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(174, 6);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(84, 16);
            this.label23.TabIndex = 48;
            this.label23.Text = "رقم الموظف";
            // 
            // EmployeeUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 403);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "EmployeeUserForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "EmployeeUserForm";
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxDisable;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label labelusername;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxPasswordConfirm;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEmployeeID;
        private System.Windows.Forms.TextBox textBoxEmployeeName;
        private System.Windows.Forms.Label label23;
    }
}