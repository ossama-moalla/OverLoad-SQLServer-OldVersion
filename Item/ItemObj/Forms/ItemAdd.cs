using ItemProject.ItemObj.ItemObjSQL;
using ItemProject.ItemObj.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ItemProject.ItemObj.Forms
{
    public partial class ItemAdd : Form
    {
        Folder folder;
        DatabaseInterface DB;
        Item Item_;
        public ItemAdd(DatabaseInterface db,Folder f)
        {
            InitializeComponent();
            DB = db;
            folder = f;
            TextBoxDefaultConsumeUnit.Text = folder.DefaultConsumeUnit;
            SetAutoCompleteTextBox();
            textBoxFolderName.Text = folder.FolderName;
        }
        public ItemAdd(DatabaseInterface db, Item Item__)
        {
            InitializeComponent();
            DB = db;
            SetAutoCompleteTextBox();
            Item_ = Item__;
            textBoxItemName.Text = Item_.ItemName;
            textBoxCompanyName.Text = Item_.ItemCompany;
            textBoxMarketCode.Text = Item_.MarketCode;
            TextBoxDefaultConsumeUnit.Text = Item_.DefaultConsumeUnit;
            textBoxFolderName.Text = Item_.folder .FolderName;
            Add.Name = "Update";
            Add.Text = "تعديل";


        }
        public void SetAutoCompleteTextBox()
        {
            textBoxItemName.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxItemName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection Item_AutoCompleteStringCollection_ = new AutoCompleteStringCollection();
            Item_AutoCompleteStringCollection_.AddRange(new ItemSQL(DB).GetAllItemsNameList().ToArray());
            textBoxItemName.AutoCompleteCustomSource = Item_AutoCompleteStringCollection_;


            textBoxCompanyName.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxCompanyName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection Company_AutoCompleteStringCollection_ = new AutoCompleteStringCollection();
            Company_AutoCompleteStringCollection_.AddRange(new ItemSQL(DB).GetAllCompaniesNameList().ToArray());
            textBoxCompanyName.AutoCompleteCustomSource = Company_AutoCompleteStringCollection_;
        }
        private void ADD_Click(object sender, EventArgs e)
        {
            ItemSQL itemsql = new ItemSQL(DB);
            if (Add.Name =="Add")
            {
               
                bool success = itemsql.CreateItem(folder, textBoxItemName.Text, textBoxCompanyName.Text, textBoxMarketCode.Text, TextBoxDefaultConsumeUnit.Text);
                if (success)
                {

                    this.DialogResult = DialogResult.OK; this.Close();
                }
            }
            else
            {
                bool success = itemsql.UpdateItem (Item_ , textBoxItemName.Text, textBoxCompanyName.Text, textBoxMarketCode.Text,  TextBoxDefaultConsumeUnit.Text);
                if (success)
                {

                    this.DialogResult = DialogResult.OK; this.Close();
                }

            }

        }
    }
}
