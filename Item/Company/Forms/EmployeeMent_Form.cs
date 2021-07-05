
using ItemProject.Company.CompanySQL;
using ItemProject.Company.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Company.Forms
{
    public partial class EmployeeMent_Form : Form
    {
        DatabaseInterface DB;
        Part _Part;
        EmployeeMent _EmployeeMent;
        Part LastUsedPart;

        List<EmployeeMent> EmployeeMentAssignReportList = new List<EmployeeMent>();


        MenuItem OpenEmployeeMentAssign_MenuItem;
        MenuItem UpdateEmployeeMentAssign_MenuItem;
        MenuItem DeleteEmployeeMentAssign_MenuItem;
        //private bool Changed_;
        //public bool Changed
        //{
        //    get { return Changed_; }
        //}
        public EmployeeMent_Form(DatabaseInterface db, Part Part_)
        {

            DB = db;
            _Part = Part_;
            InitializeComponent();
            if (_Part != null)
            {
                textBoxPartName.Text = _Part.PartName;
                textBoxPartID.Text = _Part.PartID.ToString();
                dateTimePickerPartCreateDate.Value = _Part.CreateDate;
            }
            else
            {
                textBoxPartName.Text = DB.COMPANY.PartName;
                textBoxPartID.Text = "0";
                dateTimePickerPartCreateDate.Visible = false;
            }
    
           
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "اضافة";
            labelEmployeementid.Visible = false ;
            textBoxEmployeeID.Visible = false;
            FillComboboxLevel(null);

        }
        public EmployeeMent_Form(DatabaseInterface db, EmployeeMent EmployeeMent_, bool Edit)
        {
            DB = db;
            InitializeComponent();
            _EmployeeMent = EmployeeMent_;
            _Part = _EmployeeMent._Part;
            if (_Part != null)
            {
                textBoxPartName.Text = _Part.PartName;
                textBoxPartID.Text = _Part.PartID.ToString();
                dateTimePickerPartCreateDate.Value = _Part.CreateDate;
            }
            else
            {
                textBoxPartName.Text = DB.COMPANY.PartName;
                textBoxPartID.Text = "0";
                dateTimePickerPartCreateDate.Visible = false;
            }




            LoadForm(Edit);
        }
        public void LoadForm(bool Edit)
        {
            try
            {
                if (_EmployeeMent  == null) return;
                OpenEmployeeMentAssign_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل ", OpenEmployeeMentAssign_MenuItem_Click);
               UpdateEmployeeMentAssign_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditEmployeeMentAssign_MenuItem_Click);
                DeleteEmployeeMentAssign_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteEmployeeMentAssign_MenuItem_Click);

                //EmployeeMentAssignReportList = new EmployeeMentAssignSQL(DB).Get_EmployeeMentAssignReport_List_ForEmployeeMent(_EmployeeMent);
                //RefreshEmployeeMentAssignReportList(EmployeeMentAssignReportList);
                buttonSave.Name = "buttonSave";
                buttonSave.Text = "حفظ";
                _Part = _EmployeeMent._Part;
                labelEmployeementid.Visible = true;
                textBoxEmployeeID.Visible = true ;
                textBoxEmployeeID.Text = _EmployeeMent.EmployeeMentID.ToString();
                textBoxEmployeementName.Text = _EmployeeMent.EmployeeMentName;
                dateTimePickerEmployeeCreateDate.Value = _EmployeeMent.CreateDate; ;
                FillComboboxLevel(_EmployeeMent.Level);
                textBoxEmployeeID.ReadOnly = true;


                if (Edit)
                {
                    textBoxEmployeementName.ReadOnly = false;
                    dateTimePickerEmployeeCreateDate.Enabled  = true  ;
                    comboBoxLevel.Enabled = true;
                }
                else
                {
                    this.listViewAssignReport.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewEmployeeMentAssign_MouseDoubleClick);
                    this.listViewAssignReport.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewEmployeeMentAssign_MouseDown);

                    textBoxEmployeementName.ReadOnly = true ;
                    dateTimePickerEmployeeCreateDate.Enabled = false ;
                    comboBoxLevel.Enabled = false ;
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show("حصل خطأ اثناء تحميل الصفحة:" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public async void FillComboboxLevel(EmployeeMentLevel EmployeeMentLevel_)
        {
            comboBoxLevel .Items.Clear();

            int selected_index = 0;
            try
            {
                List<EmployeeMentLevel> EmployeeMentLevelList = new EmployeeMentLevelSQL(DB).Get_EmployeeMentLevel_List();
                for (int i = 0; i < EmployeeMentLevelList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(EmployeeMentLevelList[i].LevelName , EmployeeMentLevelList[i].LevelID );
                    comboBoxLevel.Items.Add(item);
                    if (EmployeeMentLevel_ != null && EmployeeMentLevel_.LevelID  == EmployeeMentLevelList[i].LevelID ) selected_index = i;
                }
                comboBoxLevel.SelectedIndex = selected_index;

            }
            catch
            { }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            //if (_Part == null) return;
            //ComboboxItem consumeunititem = (ComboboxItem)comboBoxConsumeUnt.SelectedItem;
            //ConsumeUnit _ConsumeUnit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunititem.Value);

            //ComboboxItem ComboboxItem_selltype = (ComboboxItem)comboBoxSellType.SelectedItem;
            //string SellType_ = ComboboxItem_selltype.Text;
            ComboboxItem comboboxitem = (ComboboxItem)comboBoxLevel.SelectedItem;
            EmployeeMentLevel EmployeeMentLevel_ = new EmployeeMentLevel(comboboxitem.Value, comboboxitem.Text);

            if (buttonSave.Name == "buttonAdd")
            {
                try
                {

                  bool success =
                        new EmployeeMentSQL (DB).Add_EmployeeMent (textBoxEmployeementName.Text ,dateTimePickerEmployeeCreateDate .Value , EmployeeMentLevel_.LevelID , _Part);

                    if (success)
                    {
                        MessageBox.Show("تم الاضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult  = DialogResult.OK;
                        this.Close();

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":تعذر الاضافة " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                try
                {
                    if (_EmployeeMent  != null)
                    {

                        bool success =
                       new EmployeeMentSQL(DB).Update_EmployeeMent (_EmployeeMent.EmployeeMentID, textBoxEmployeementName.Text, dateTimePickerEmployeeCreateDate.Value, EmployeeMentLevel_.LevelID );
                        if (success == true)
                        {
                            MessageBox.Show("تم حفظ  بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //_MaintenanceFault = new MaintenanceSQL.MaintenanceAccessorySQL(DB).Get_Accessory_INFO_BYID(_MaintenanceFault.AccessoryID);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else MessageBox.Show("لم يتم الحفظ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":تعذرالحفظ  " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }


        }
        #region EmployeeMentAssigns
        private void DeleteEmployeeMentAssign_MenuItem_Click(object sender, EventArgs e)
        {

            //try
            //{

            //    DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //    if (dd != DialogResult.OK) return;
            //    string s = listViewAssignReport.SelectedItems[0].Name.Substring(0, 1);
            //    if (s == "D")
            //    {
            //        uint sid = Convert.ToUInt32(listViewAssignReport.SelectedItems[0].Name.Substring(1));
            //        bool success = new DiagnosticOPR_MissedFaultItem_EmployeeMentAssignSQL(DB).Delete_DiagnosticOPR_MissedFaultItem_EmployeeMentAssign (sid);
            //        if (success)
            //        {
            //            MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            EmployeeMentAssignReportList = new MaintenanceEmployeeMentAssignSQL(DB).GetMissedFaultItem_EmployeeMentAssignReportList(_EmployeeMent);
            //            RefreshEmployeeMentAssignReportList(EmployeeMentAssignReportList);

            //        }
            //        else
            //        {
            //            MessageBox.Show("فشل الحذف", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //        }
            //    }
            //    else if (s == "M")
            //    {
            //        uint sid = Convert.ToUInt32(listViewAssignReport.SelectedItems[0].Name.Substring(1));
            //        bool success = new Fault_MissedFaultItem_EmployeeMentAssignSQL(DB).Delete__Fault_MissedFaultItem_EmployeeMentAssign(sid);
            //        if (success)
            //        {
            //            MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            EmployeeMentAssignReportList = new MaintenanceEmployeeMentAssignSQL(DB).GetMissedFaultItem_EmployeeMentAssignReportList(_EmployeeMent);
            //            RefreshEmployeeMentAssignReportList(EmployeeMentAssignReportList);

            //        }
            //        else
            //        {
            //            MessageBox.Show("فشل الحذف", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //        }
            //    }

            //}
            //catch
            //{

            //}
        }
        private void EditEmployeeMentAssign_MenuItem_Click(object sender, EventArgs e)
        {
            //if (listViewAssignReport.SelectedItems.Count > 0)
            //{
            //    try
            //    {
            //        string s = listViewAssignReport.SelectedItems[0].Name.Substring(0, 1);
            //        if (s == "D")
            //        {
            //            uint id = Convert.ToUInt32(listViewAssignReport.SelectedItems[0].Name.Substring(1));
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssign DiagnosticOPR_MissedFaultItem_EmployeeMentAssign_ = new DiagnosticOPR_MissedFaultItem_EmployeeMentAssignSQL(DB).Get_DiagnosticOPR_MissedFaultItem_EmployeeMentAssign_info_ByID(id);
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_ = new DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm(DB, DiagnosticOPR_MissedFaultItem_EmployeeMentAssign_, true);
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_.ShowDialog();
            //            if (DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_.DialogResult == DialogResult.OK)
            //            {
            //                EmployeeMentAssignReportList = new MaintenanceEmployeeMentAssignSQL(DB).GetMissedFaultItem_EmployeeMentAssignReportList(_EmployeeMent);
            //                RefreshEmployeeMentAssignReportList(EmployeeMentAssignReportList);
            //            }
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_.Dispose();
            //        }
            //        else
            //        {
            //            uint id = Convert.ToUInt32(listViewAssignReport.SelectedItems[0].Name.Substring(1));
            //            Fault_MissedFaultItem_EmployeeMentAssign Fault_MissedFaultItem_EmployeeMentAssign_ = new Fault_MissedFaultItem_EmployeeMentAssignSQL(DB).Get_Fault_MissedFaultItem_EmployeeMentAssign_info_ByID(id);
            //            Fault_MissedFaultItem_EmployeeMentAssignForm Fault_MissedFaultItem_EmployeeMentAssignForm_ = new Fault_MissedFaultItem_EmployeeMentAssignForm(DB, Fault_MissedFaultItem_EmployeeMentAssign_, true);
            //            Fault_MissedFaultItem_EmployeeMentAssignForm_.ShowDialog();
            //            if (Fault_MissedFaultItem_EmployeeMentAssignForm_.DialogResult == DialogResult.OK)
            //            {
            //                EmployeeMentAssignReportList = new MaintenanceEmployeeMentAssignSQL(DB).GetMissedFaultItem_EmployeeMentAssignReportList(_EmployeeMent);
            //                RefreshEmployeeMentAssignReportList(EmployeeMentAssignReportList);
            //            }
            //            Fault_MissedFaultItem_EmployeeMentAssignForm_.Dispose();
            //        }
            //    }
            //    catch
            //    {

            //    }

            //}
        }
        private void OpenEmployeeMentAssign_MenuItem_Click(object sender, EventArgs e)
        {
            //if (listViewAssignReport.SelectedItems.Count > 0)
            //{
            //    try
            //    {
            //        string s = listViewAssignReport.SelectedItems[0].Name.Substring(0, 1);
            //        if (s == "D")
            //        {
            //            uint id = Convert.ToUInt32(listViewAssignReport.SelectedItems[0].Name.Substring(1));
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssign DiagnosticOPR_MissedFaultItem_EmployeeMentAssign_ = new DiagnosticOPR_MissedFaultItem_EmployeeMentAssignSQL(DB).Get_DiagnosticOPR_MissedFaultItem_EmployeeMentAssign_info_ByID(id);
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_ = new DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm(DB, DiagnosticOPR_MissedFaultItem_EmployeeMentAssign_, false );
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_.ShowDialog();
            //            if (DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_.DialogResult == DialogResult.OK)
            //            {
            //                EmployeeMentAssignReportList = new MaintenanceEmployeeMentAssignSQL(DB).GetMissedFaultItem_EmployeeMentAssignReportList(_EmployeeMent);
            //                RefreshEmployeeMentAssignReportList(EmployeeMentAssignReportList);
            //            }
            //            DiagnosticOPR_MissedFaultItem_EmployeeMentAssignForm_.Dispose();
            //        }
            //        else
            //        {
            //            uint id = Convert.ToUInt32(listViewAssignReport.SelectedItems[0].Name.Substring(1));
            //            Fault_MissedFaultItem_EmployeeMentAssign Fault_MissedFaultItem_EmployeeMentAssign_ = new Fault_MissedFaultItem_EmployeeMentAssignSQL(DB).Get_Fault_MissedFaultItem_EmployeeMentAssign_info_ByID(id);
            //            Fault_MissedFaultItem_EmployeeMentAssignForm Fault_MissedFaultItem_EmployeeMentAssignForm_ = new Fault_MissedFaultItem_EmployeeMentAssignForm(DB, Fault_MissedFaultItem_EmployeeMentAssign_, false);
            //            Fault_MissedFaultItem_EmployeeMentAssignForm_.ShowDialog();
            //            if (Fault_MissedFaultItem_EmployeeMentAssignForm_.DialogResult == DialogResult.OK)
            //            {
            //                EmployeeMentAssignReportList = new MaintenanceEmployeeMentAssignSQL(DB).GetMissedFaultItem_EmployeeMentAssignReportList(_EmployeeMent);
            //                RefreshEmployeeMentAssignReportList(EmployeeMentAssignReportList);
            //            }
            //            Fault_MissedFaultItem_EmployeeMentAssignForm_.Dispose();
            //        }

            //    }
            //    catch
            //    {

            //    }

            //}
        }
        private async void RefreshEmployeeMentAssignReportList(List<EmployeeMent > EmployeeMentAssignReportList_)
        {

            //listViewAssignReport .Items.Clear();
            //for (int i = 0; i < EmployeeMentAssignReportList_.Count; i++)
            //{
            //    string Header = "";
            //    Color color;
            //    string type = "";
            //    if (EmployeeMentAssignReportList_[i].EmployeeMentAssignType == MaintenanceEmployeeMentAssignSummary.DiagnosicOPR_MissedFaultItem_EmployeeMentAssign_Type)
            //    {
            //        type = "عملية فحص"; color = Color.Orange;
            //        Header = "D";
            //    }
            //    else if (EmployeeMentAssignReportList_[i].EmployeeMentAssignType == MaintenanceEmployeeMentAssignSummary.Fault_MissedFaultItem_EmployeeMentAssign_Type)
            //    {
            //        type = "عنصر مفقود او تالف";
            //        color = Color.Yellow;
            //        Header = "F";
            //    }
            //    else
            //    { type = ""; color = Color.White; }
            //    ListViewItem ListViewItem_ = new ListViewItem(type);
            //    ListViewItem_.BackColor = color;
            //    ListViewItem_.Name = Header + EmployeeMentAssignReportList_[i].EmployeeMentAssignID.ToString();
            //    ListViewItem_.SubItems.Add(EmployeeMentAssignReportList_[i].ID.ToString());
            //    ListViewItem_.SubItems.Add(EmployeeMentAssignReportList_[i].Desc);
            //    ListViewItem_.SubItems.Add(EmployeeMentAssignReportList_[i].EmployeeMentAssignINFO);
            //    listViewAssignReport.Items.Add(ListViewItem_);

            //}

        }
        private void listViewEmployeeMentAssign_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewAssignReport .SelectedItems.Count > 0)
            {
                OpenEmployeeMentAssign_MenuItem.PerformClick();
            }
        }
        private void listViewEmployeeMentAssign_MouseDown(object sender, MouseEventArgs e)
        {
            //listViewAssignReport.ContextMenu = null;
            //bool match = false;
            //ListViewItem listitem = new ListViewItem();
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    foreach (ListViewItem item1 in listViewAssignReport.Items)
            //    {
            //        if (item1.Bounds.Contains(new Point(e.X, e.Y)))
            //        {
            //            match = true;
            //            listitem = item1;
            //            break;
            //        }
            //    }
            //    if (match)
            //    {


            //        MenuItem[] mi1 = new MenuItem[] { OpenEmployeeMentAssign_MenuItem, UpdateEmployeeMentAssign_MenuItem, DeleteEmployeeMentAssign_MenuItem, new MenuItem("-"), add, AddFaultEmployeeMentAssign_MenuItem  };
            //        listViewAssignReport.ContextMenu = new ContextMenu(mi1);


            //    }
            //    else
            //    {

            //        MenuItem[] mi = new MenuItem[] { ad  };
            //        listViewAssignReport.ContextMenu = new ContextMenu(mi);

            //    }

            //}

        }
        public async void AdjustlistViewEmployeeMentAssign_ColumnsWidth()
        {
            try
            {
                listViewAssignReport.Columns[0].Width = 150;
                listViewAssignReport.Columns[1].Width = 60;
                listViewAssignReport.Columns[2].Width = 150;
                listViewAssignReport.Columns[3].Width = listViewAssignReport.Width - 380;

            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("AdjustlistViewEmployeeMentAssign_ColumnsWidth" + Environment.NewLine + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void listViewAssignReport_Resize(object sender, EventArgs e)
        {
            AdjustlistViewEmployeeMentAssign_ColumnsWidth();
        }
        #endregion

    }
}
