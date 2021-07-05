using ItemProject.Maintenance.MaintenanceSQL;
using ItemProject.Maintenance.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Maintenance.Forms
{
    public partial class MaintenanceOPR_EndWorkForm : Form
    {
        MaintenanceOPR _MaintenanceOPR;
        MaintenanceOPR_EndWork _MaintenanceOPR_EndWork;
        DatabaseInterface DB;
        public bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }
        public MaintenanceOPR_EndWorkForm(DatabaseInterface db,  MaintenanceOPR MaintenanceOPR_)
        {
            DB = db;

            InitializeComponent();

            _MaintenanceOPR = MaintenanceOPR_;


            LoadMaintenanceOPRData();
            _Changed = false;


            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انهاء العمل";
            dateTimePickerDeliveDate.Enabled = false;
            dateTimePickerEndWarranty.Enabled = false;
            this.checkBoxDeliverdate.CheckedChanged += new System.EventHandler(this.checkBoxDeliverdate_CheckedChanged);
            this.checkBoxendwarrant.CheckedChanged += new System.EventHandler(this.checkBoxendwarrant_CheckedChanged);

        }
        public MaintenanceOPR_EndWorkForm(DatabaseInterface db, MaintenanceOPR_EndWork MaintenanceOPR_EndWork_, bool edit)
        {
            InitializeComponent();

            DB = db;
            try
            {
                _MaintenanceOPR = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(MaintenanceOPR_EndWork_.MaintenanceOPRID) ;
            }
            catch
            {
                MessageBox.Show("فشل الحصول على بيانات عملية الصيانة","",MessageBoxButtons.OK,MessageBoxIcon.Error );
            }
            _MaintenanceOPR_EndWork = MaintenanceOPR_EndWork_;
            _Changed = false;
            LoadMaintenanceOPRData();
            loadForm(edit);


        }
        private async void LoadMaintenanceOPRData()
        {

            textBoxItemID.Text = _MaintenanceOPR._Item.ItemID.ToString();
            textBoxItemName.Text = _MaintenanceOPR._Item.ItemName;
            textBoxItemCompany.Text = _MaintenanceOPR._Item.ItemCompany;
            textBoxItemType.Text = _MaintenanceOPR._Item.folder.FolderName;
            textBoxContact.Text = _MaintenanceOPR._Contact.Get_Complete_ContactName_WithHeader();
            textBoxMaintenenaceOPRID.Text = _MaintenanceOPR._Operation.OperationID.ToString();

        }
        public void loadForm(bool edit)
        {
            buttonSave.Name = "buttonSave";
            buttonSave.Text = "حفظ";

            if (_MaintenanceOPR_EndWork  != null)
            {

                dateTimePickerEndWorkDate.Value = _MaintenanceOPR_EndWork.EndWorkDate;
                if (_MaintenanceOPR_EndWork.Repaired)
                    comboBoxResult.SelectedIndex = 0;
                else
                    comboBoxResult.SelectedIndex = 1;

                if (_MaintenanceOPR_EndWork.DeliveredDate == null)
                {
                    dateTimePickerDeliveDate.Enabled = false;
                    checkBoxDeliverdate.Checked = false;
                }
                else
                {
                    checkBoxDeliverdate.Checked = true;
                    dateTimePickerDeliveDate.Value = Convert.ToDateTime(_MaintenanceOPR_EndWork.DeliveredDate);
                }
                if (_MaintenanceOPR_EndWork.EndwarrantyDate == null)
                {
                    checkBoxendwarrant.Checked = false;
                    dateTimePickerEndWarranty.Enabled = false;
                }
                else
                {
                    checkBoxendwarrant.Checked = true;
                    dateTimePickerEndWarranty.Value = Convert.ToDateTime(_MaintenanceOPR_EndWork.EndwarrantyDate);
                }
                textBoxReport.Text = _MaintenanceOPR_EndWork.Report;
                if (edit)
                {

                    dateTimePickerEndWorkDate.Enabled = true;
                    comboBoxResult.Enabled = true;
                    checkBoxDeliverdate.Enabled  = true ;
                    checkBoxendwarrant.Enabled = true;
                    textBoxReport .ReadOnly = false;
                    this.checkBoxDeliverdate.CheckedChanged += new System.EventHandler(this.checkBoxDeliverdate_CheckedChanged);
                    this.checkBoxendwarrant.CheckedChanged += new System.EventHandler(this.checkBoxendwarrant_CheckedChanged);

                }
                else
                {

                    dateTimePickerEndWorkDate.Enabled = false ;
                    comboBoxResult.Enabled = false ;
                    checkBoxDeliverdate.Enabled = false ;
                    checkBoxendwarrant.Enabled = false ;
                    textBoxReport.ReadOnly = true ;

                }
               
            }
        }

        private void checkBoxDeliverdate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDeliverdate.Checked)
                dateTimePickerDeliveDate.Enabled = true;
            else
                dateTimePickerDeliveDate.Enabled = false ;
        }

        private void checkBoxendwarrant_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxendwarrant.Checked)
                dateTimePickerEndWarranty.Enabled = true;
            else
                dateTimePickerEndWarranty.Enabled = false;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxResult .SelectedIndex<0)
            {
                MessageBox.Show("يجب تحديد النتيجة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool repaired;
            if (comboBoxResult.SelectedIndex == 0) repaired = true;
            else repaired = false;
            DateTime? deliverdate, endwarrantydate;
            if (checkBoxDeliverdate.Checked)
                deliverdate = dateTimePickerDeliveDate.Value;
            else
                deliverdate = null;
            if (checkBoxendwarrant.Checked)
                endwarrantydate  = dateTimePickerEndWarranty.Value;
            else
                endwarrantydate = null;
            if (buttonSave.Name == "buttonAdd")
            {


                bool success = new MaintenanceOPRSQL(DB).Create_MaintenanceOPREndWork (_MaintenanceOPR._Operation.OperationID, dateTimePickerEndWorkDate.Value
                    , repaired , deliverdate , endwarrantydate , textBoxReport.Text);
                if (success )
                {
                 MessageBox.Show("تم التنفيذ بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this._Changed = true;
                    this.Close();

                }
            }

            else
            {
               
                    bool success = new MaintenanceOPRSQL(DB).Update_MaintenanceOPREndWork(_MaintenanceOPR._Operation.OperationID, dateTimePickerEndWorkDate.Value
                    , repaired, deliverdate, endwarrantydate, textBoxReport.Text);
                    if (success == true)
                    {
                        MessageBox.Show("تم ألحفظ بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this._Changed = true;
                        this.Close();
                    }
   
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _Changed = false;
            this.Close();
        }
    }
}
