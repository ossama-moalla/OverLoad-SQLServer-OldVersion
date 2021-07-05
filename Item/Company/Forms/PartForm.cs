﻿using ItemProject.Company.CompanySQL;
using ItemProject.Company.Objects;
using ItemProject.ItemObj.ItemObjSQL;
using ItemProject.ItemObj.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace ItemProject.Company.Forms
{
    public partial class PartForm : Form
    {
        uint? ParentPartID;
        Part Part_;
        PartSQL Partsql;
        DatabaseInterface DB;
        public PartForm(DatabaseInterface db,uint? pPart,string Partname)
        {
            InitializeComponent();
            DB = db;
            ParentPartID = pPart;
            Partsql = new PartSQL(DB);
            TextBoxInput.Text = Partname;
            this.TextBoxInput.SelectionStart = 0;
            this.TextBoxInput.SelectionLength = Partname.Length;
            TextBoxInput.Focus();
        }
        public PartForm(DatabaseInterface db, Part Part__,bool Edit)
        {
            InitializeComponent();
            DB = db;
            Partsql = new PartSQL(DB);
            Part_ = Part__;
   
            this.TextBoxInput.Text = Part_.PartName ;
            this.TextBoxInput.SelectionStart = 0;
            this.TextBoxInput.SelectionLength = Part_.PartName.Length;
            TextBoxInput.Focus();
            Add.Name = "Update";
            Add.Text = "تعديل";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (TextBoxInput.Text.Length == 0)
            { MessageBox.Show("اسم القسم يجب ان لا يكون فارغا"); return; }
            if (Add.Name == "Add")
            {
                bool r = Partsql.CreatePart(TextBoxInput.Text,dateTimePicker1.Value, ParentPartID);
                if (r == true)
                {
                    this.DialogResult = DialogResult.OK; this.Dispose();
                }
            }
           if(Add.Name =="Update")
            {
                bool r = Partsql.UpdatePart  (Part_ .PartID, TextBoxInput.Text,dateTimePicker1 .Value );
                if (r == true)
                {
                    this.DialogResult = DialogResult.OK; this.Dispose();
                }
            }
        }

 
    }
}
