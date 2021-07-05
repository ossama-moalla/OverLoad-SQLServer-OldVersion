using ItemProject.ItemObj.Objects;
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
    public partial class FaultForm : Form
    {

        DatabaseInterface DB;
        MaintenanceOPR _MaintenanceOPR;
        MaintenanceFault _MaintenanceFault;
        Folder LastUsedFolder;
        Item _Item;
        List<RepairOPR> RepairOPRList = new List<RepairOPR>();
        List<MaintenanceTag> FaultTagList_ = new List<MaintenanceTag>();


        System.Windows.Forms.MenuItem OpenRepairOPR_MenuItem;
        System.Windows.Forms.MenuItem AddRepairOPR_MenuItem;
        System.Windows.Forms.MenuItem EditRepairOPR_MenuItem;
        System.Windows.Forms.MenuItem DeleteRepairOPR_MenuItem;

        MenuItem OpenTag_MenuItem;
        MenuItem AddDiagnosticOPRTag_MenuItem;
        MenuItem AddMissedFaultItemTag_MenuItem;
        MenuItem UpdateTag_MenuItem;
        MenuItem DeleteTag_MenuItem;


        private bool Changed_;
        public bool Changed
        {
            get { return Changed_; }
        }
        public FaultForm(DatabaseInterface db, MaintenanceOPR MaintenanceOPR_)
        {

            DB = db;
            _MaintenanceOPR = MaintenanceOPR_;
            InitializeComponent();
            textBoxContact.Text = _MaintenanceOPR._Contact.Get_Complete_ContactName_WithHeader();
            textBoxMOPR.Text = _MaintenanceOPR._Operation.OperationID.ToString();
            dateTimePickerMainteneaceOPRDate.Value = _MaintenanceOPR.EntryDate ;

            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "اضافة";
            this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
            this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);

            OpenRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية التركيب", OpenRepairOPR_MenuItem_Click);
            AddRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("تركيب عنصر", AddRepairOPR_MenuItem_Click);
            EditRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditRepairOPR_MenuItem_Click);
            DeleteRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteRepairOPR_MenuItem_Click);

            OpenTag_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل الرابط", OpenTag_MenuItem_Click);
            AddDiagnosticOPRTag_MenuItem = new System.Windows.Forms.MenuItem("ربط مع عطل", AddDiagnosticOPRTag_MenuItem_Click);
            AddMissedFaultItemTag_MenuItem = new System.Windows.Forms.MenuItem("ربط مع عنصر مفقود او تالف", AddMissedFaultItemTag_MenuItem_Click);
            UpdateTag_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditTag_MenuItem_Click);
            DeleteTag_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteTag_MenuItem_Click);


            RepairOPR.InitializeRepairOPRListViewColumns(ref listViewRepairOPR);
            this.listViewRepairOPR.Resize += new System.EventHandler(this.listViewRepairOPR_Resize);

        }
        public FaultForm(DatabaseInterface db, MaintenanceFault MaintenanceFault_, bool Edit)
        {
            DB = db;
            _MaintenanceFault = MaintenanceFault_;
            _MaintenanceOPR = _MaintenanceFault._MaintenanceOPR ;
            InitializeComponent();
            OpenRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية الاصلاح", OpenRepairOPR_MenuItem_Click);
            AddRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("اضافة عملية اصلاح", AddRepairOPR_MenuItem_Click);
            EditRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditRepairOPR_MenuItem_Click);
            DeleteRepairOPR_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteRepairOPR_MenuItem_Click);

            OpenTag_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل الرابط", OpenTag_MenuItem_Click);
            AddDiagnosticOPRTag_MenuItem = new System.Windows.Forms.MenuItem("ربط مع عملية فحص", AddDiagnosticOPRTag_MenuItem_Click);
            AddMissedFaultItemTag_MenuItem = new System.Windows.Forms.MenuItem("ربط مع عنصر مفقود او تالف", AddMissedFaultItemTag_MenuItem_Click);

            UpdateTag_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditTag_MenuItem_Click);
            DeleteTag_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteTag_MenuItem_Click);


            RepairOPR.InitializeRepairOPRListViewColumns(ref listViewRepairOPR);
            this.listViewRepairOPR.Resize += new System.EventHandler(this.listViewRepairOPR_Resize);

            LoadForm(Edit);
        }
        public void LoadForm(bool Edit)
        {
            try
            {


                if (_MaintenanceFault == null) return;
                buttonSave.Name = "buttonSave";
                buttonSave.Text = "حفظ";
                _Item = _MaintenanceFault._Item;
                _MaintenanceOPR = _MaintenanceFault._MaintenanceOPR;
                FillComboBox(_MaintenanceFault.FaultDesc );
                dateTimePickerFaultDate.Value = _MaintenanceFault.FaultDate;
                textBoxContact.Text = _MaintenanceOPR._Contact.Get_Complete_ContactName_WithHeader();
                textBoxMOPR.Text = _MaintenanceOPR._Operation.OperationID.ToString();
                dateTimePickerMainteneaceOPRDate.Value = _MaintenanceOPR.EntryDate ;
                LoadItemData();
                RepairOPRList = new RepairOPRSQL(DB).GetFault_RepairOPR_List(_MaintenanceFault);
                FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List(_MaintenanceFault);

                RepairOPR.RefreshRepairOPRList(ref listViewRepairOPR, RepairOPRList);
                RefreshTagList(FaultTagList_);
                if (Edit)
                {
         
                    buttonSave.Visible = true;
                    textBoxItemID.ReadOnly = false;
                    comboBoxFaultDesc .Enabled  = true ;
                    dateTimePickerFaultDate .Enabled  = true ;



                }
                else
                {
                    this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
                    this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
                    this.listViewRepairOPR.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewRepairOPR_MouseDoubleClick);
                    this.listViewRepairOPR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewRepairOPR_MouseDown);

                    this.listViewTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewTag_MouseDoubleClick);
                    this.listViewTags.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewTag_MouseDown);


                    buttonSave.Visible = false;
                    TextBox textboxdesc = new TextBox();
                    textboxdesc.ReadOnly = true;
                    textboxdesc.Location = comboBoxFaultDesc.Location;
                    textboxdesc.Size = comboBoxFaultDesc.Size;
                    textboxdesc.Text = _MaintenanceFault.FaultDesc;
                    textboxdesc.Font = comboBoxFaultDesc.Font;
                    textboxdesc.BorderStyle = BorderStyle.FixedSingle;
                    textboxdesc .Anchor  = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                    panel2.Controls.Add(textboxdesc);
                    comboBoxFaultDesc.Visible = false;
                    textBoxItemID.ReadOnly = true;
                    comboBoxFaultDesc.Enabled = false ;
                    dateTimePickerFaultDate.Enabled = false ;

                }

            }
            catch (Exception ee)
            {
                MessageBox.Show("حصل خطأ اثناء تحميل الصفحة:" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void FillComboBox(string faultdesc)
        {
            comboBoxFaultDesc .Items.Clear();
            int selectedIndex = -1;
            List<string> faultdescList = new MaintenanceFaultSQL(DB).GetItem_FaultDescList(_Item );
            for (int i = 0; i < faultdescList.Count; i++)
            {
                if (faultdescList[i] == faultdesc) selectedIndex = i;
                comboBoxFaultDesc.Items.Add(faultdescList[i]);
            }
            comboBoxFaultDesc.SelectedIndex = selectedIndex;
        }
        private async void LoadItemData()
        {
            LastUsedFolder = _Item.folder;
            textBoxItemID.Text = _Item.ItemID.ToString();
            textBoxItemName.Text = _Item.ItemName;
            textBoxItemCompany.Text = _Item.ItemCompany;
            textBoxItemType.Text = _Item.folder.FolderName;

        }
        private void textBoxItemID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                try
                {
                    uint itemid = Convert.ToUInt32(textBoxItemID.Text);
                    Item item__ = new ItemObj.ItemObjSQL.ItemSQL(DB).GetItemInfoByID(itemid);
                    if (item__ != null)
                    {
                        _Item = item__;
                        LoadItemData();
                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على العنصر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    MessageBox.Show("يرجى ادخال عدد صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }
        private void textBoxItem_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ItemObj.Forms.SelectItem form = new ItemObj.Forms.SelectItem(DB, LastUsedFolder);
                DialogResult dd = form.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    _Item = form.ReturnItem;
                    LoadItemData();
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (_MaintenanceOPR == null) return;
            //ComboboxItem consumeunititem = (ComboboxItem)comboBoxConsumeUnt.SelectedItem;
            //ConsumeUnit _ConsumeUnit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunititem.Value);

            //ComboboxItem ComboboxItem_selltype = (ComboboxItem)comboBoxSellType.SelectedItem;
            //string SellType_ = ComboboxItem_selltype.Text;

            if (buttonSave.Name == "buttonAdd")
            {
                try
                {
                    if (_Item == null)
                    {

                        MessageBox.Show("يرجى تحديد المادة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MaintenanceFault MaintenanceFault_ =
                        new MaintenanceSQL.MaintenanceFaultSQL(DB).AddFault 
                        (_MaintenanceOPR._Operation.OperationID, _Item.ItemID , dateTimePickerFaultDate.Value ,comboBoxFaultDesc .Text 
                        ,textBoxReport .Text );
                    
                    if (MaintenanceFault_ != null)
                    {
                        _MaintenanceFault = MaintenanceFault_;
                        MessageBox.Show("تم الاضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Changed_ = true;
                        LoadForm(false);

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":تعذر اضافة العطل " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                try
                {
                    if (_MaintenanceFault != null)
                    {

                        bool success = new MaintenanceSQL.MaintenanceFaultSQL(DB).UpdateFault 
                            (_MaintenanceFault.FaultID, _Item.ItemID
                           , dateTimePickerFaultDate.Value, comboBoxFaultDesc.Text  .ToString () , textBoxReport.Text);
                        if (success == true)
                        {
                            MessageBox.Show("تم حفظ  بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //_MaintenanceFault = new MaintenanceSQL.MaintenanceAccessorySQL(DB).Get_Accessory_INFO_BYID(_MaintenanceFault.AccessoryID);
                            this.Changed_ = true;
                            LoadForm(false);
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

        #region RepairOPR
        private void DeleteRepairOPR_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من حذف بند الاصلاح و جميع العمليات التابعة له؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewRepairOPR .SelectedItems[0].Name);
                bool success = new RepairOPRSQL(DB).DeleteRepairOPR (sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RepairOPRList = new RepairOPRSQL(DB).GetFault_RepairOPR_List(_MaintenanceFault );
                    RepairOPR .RefreshRepairOPRList(ref listViewRepairOPR, RepairOPRList);

                }
                else
                {
                    MessageBox.Show("فشل الحذف", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("DeleteSubDiagnosticOPR_MenuItem_Click" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void EditRepairOPR_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewRepairOPR.SelectedItems.Count > 0)
            {
                uint faultid = Convert.ToUInt32(listViewRepairOPR.SelectedItems[0].Name);
                RepairOPR RepairOPR_ = new RepairOPRSQL(DB).Get_RepairOPR_INFO_BYID(faultid);
                RepairOPRForm RepairOPRForm_ = new RepairOPRForm(DB, RepairOPR_, true);
                RepairOPRForm_.ShowDialog();
                if (RepairOPRForm_.Changed)
                {
                    RepairOPRList = new RepairOPRSQL(DB).GetFault_RepairOPR_List(_MaintenanceFault);
                    RepairOPR.RefreshRepairOPRList(ref listViewRepairOPR, RepairOPRList);
                }
                RepairOPRForm_.Dispose();
            }
        }
        private void OpenRepairOPR_MenuItem_Click(object sender, EventArgs e)
        {

            if (listViewRepairOPR.SelectedItems.Count > 0)
            {
                uint faultid = Convert.ToUInt32(listViewRepairOPR.SelectedItems[0].Name);
                RepairOPR RepairOPR_ = new RepairOPRSQL(DB).Get_RepairOPR_INFO_BYID(faultid);
                RepairOPRForm RepairOPRForm_ = new RepairOPRForm(DB, RepairOPR_, false );
                RepairOPRForm_.ShowDialog();
                if (RepairOPRForm_.Changed)
                {
                    RepairOPRList = new RepairOPRSQL(DB).GetFault_RepairOPR_List(_MaintenanceFault);
                    RepairOPR.RefreshRepairOPRList(ref listViewRepairOPR, RepairOPRList);
                }
                RepairOPRForm_.Dispose();
            }
        }
        private void AddRepairOPR_MenuItem_Click(object sender, EventArgs e)
        {
            RepairOPRForm RepairOPRForm_ = new RepairOPRForm(DB, _MaintenanceFault);
            DialogResult d = RepairOPRForm_.ShowDialog();
            if (RepairOPRForm_.Changed)
            {
                RepairOPRList = new RepairOPRSQL(DB).GetFault_RepairOPR_List(_MaintenanceFault);
                RepairOPR.RefreshRepairOPRList(ref listViewRepairOPR, RepairOPRList);
            }
            RepairOPRForm_.Dispose();
        }
        //private async void RefreshDiagnosticOPRList(List<DiagnosticOPRReport> SubDiagnosticOPRReportList_)
        //{

        //    listViewSubDiagnosticOPR.Items.Clear();
        //    for (int i = 0; i < SubDiagnosticOPRReportList_.Count; i++)
        //    {
        //        ListViewItem ListViewItem_ = new ListViewItem(SubDiagnosticOPRReportList_[i]._DiagnosticOPR.DiagnosticOPRID.ToString());
        //        ListViewItem_.Name = SubDiagnosticOPRReportList_[i]._DiagnosticOPR.DiagnosticOPRID.ToString();
        //        ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i]._DiagnosticOPR.DiagnosticOPRDate.ToShortDateString());
        //        ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i]._DiagnosticOPR.Desc);
        //        ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i]._DiagnosticOPR.Location);
        //        if (SubDiagnosticOPRReportList_[i]._DiagnosticOPR._Item == null)
        //        {
        //            ListViewItem_.SubItems.Add("-");
        //            ListViewItem_.SubItems.Add("-");
        //            ListViewItem_.SubItems.Add("-");

        //        }
        //        else
        //        {
        //            ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i]._DiagnosticOPR._Item.folder.FolderName);
        //            ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i]._DiagnosticOPR._Item.ItemName);
        //            ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i]._DiagnosticOPR._Item.ItemCompany);

        //        }
        //        if (SubDiagnosticOPRReportList_[i]._DiagnosticOPR.Normal == null)
        //        {
        //            ListViewItem_.SubItems.Add("غير معروف");
        //            ListViewItem_.BackColor = Color.LightYellow;
        //        }
        //        else if (SubDiagnosticOPRReportList_[i]._DiagnosticOPR.Normal == true)
        //        {
        //            ListViewItem_.SubItems.Add("لا يوجد عطل");
        //            ListViewItem_.BackColor = Color.LimeGreen;
        //        }
        //        else
        //        {
        //            ListViewItem_.SubItems.Add(" يوجد عطل");
        //            ListViewItem_.BackColor = Color.Orange;
        //        }
        //        //ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i]._DiagnosticOPR.Report);
        //        ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i].MeasureOPR_Count.ToString());
        //        ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i].Files_Count.ToString());
        //        ListViewItem_.SubItems.Add(SubDiagnosticOPRReportList_[i].SubDiagnosticOPR_Count.ToString());

        //        listViewSubDiagnosticOPR.Items.Add(ListViewItem_);

        //    }



        //}
        private void listViewRepairOPR_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewRepairOPR .SelectedItems.Count > 0)
            {
                OpenRepairOPR_MenuItem.PerformClick();
            }
        }
        private void listViewRepairOPR_MouseDown(object sender, MouseEventArgs e)
        {
            listViewRepairOPR.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewRepairOPR.Items)
                {
                    if (item1.Bounds.Contains(new Point(e.X, e.Y)))
                    {
                        match = true;
                        listitem = item1;
                        break;
                    }
                }
                if (match)
                {


                    MenuItem[] mi1 = new MenuItem[] { OpenRepairOPR_MenuItem 
                        , EditRepairOPR_MenuItem, DeleteRepairOPR_MenuItem, new MenuItem("-"), AddRepairOPR_MenuItem };
                    listViewRepairOPR.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddRepairOPR_MenuItem  };
                    listViewRepairOPR.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        private void listViewRepairOPR_Resize(object sender, EventArgs e)
        {
            //MaintenanceFaultReport .AdjustlistViewFaultReportOPRColumnsWidth (ref listViewSubDiagnosticOPR);
            AdjustlistViewRepairOPRColumnsWidth();
        }
        public async void AdjustlistViewRepairOPRColumnsWidth()
        {
            try
            {
                listViewRepairOPR .Columns[0].Width = 60;
                listViewRepairOPR.Columns[1].Width = 100;
                listViewRepairOPR.Columns[2].Width = 150;
                listViewRepairOPR.Columns[3].Width = listViewRepairOPR.Width - 770;
                listViewRepairOPR.Columns[4].Width = 150;
                listViewRepairOPR.Columns[5].Width = 150;
                listViewRepairOPR.Columns[6].Width = 150;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("AdjustlistViewRepairOPRColumnsWidth" + Environment.NewLine + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Tags
        private void DeleteTag_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                    uint sid = Convert.ToUInt32(listViewTags.SelectedItems[0].Name);
                    bool success = new MaintenanceTagSQL(DB).DeleteMaintenanceTag(sid);
                    if (success)
                    {
                        MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List (_MaintenanceFault );
                        RefreshTagList(FaultTagList_);

                    }
                    else
                    {
                        MessageBox.Show("فشل الحذف", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                

            }
            catch
            {

            }
        }
        private void EditTag_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewTags.SelectedItems.Count > 0)
            {
                try
                {
                    string s = listViewTags.SelectedItems[0].Name.Substring(0, 1);
                    if (s == "D")
                    {
                        uint id = Convert.ToUInt32(listViewTags.SelectedItems[0].Name.Substring(1));
                        MaintenanceTag DiagnosticOPR_Fault_Tag_ = new MaintenanceTagSQL(DB).GetMaintenanceTaginfo_ByID(id);
                        DiagnosticOPR_Fault_TagForm DiagnosticOPR_Fault_TagForm_ = new DiagnosticOPR_Fault_TagForm(DB, DiagnosticOPR_Fault_Tag_, true);
                        DiagnosticOPR_Fault_TagForm_.ShowDialog();
                        if (DiagnosticOPR_Fault_TagForm_.DialogResult == DialogResult.OK)
                        {
                            FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List(_MaintenanceFault);
                            RefreshTagList(FaultTagList_);
                        }
                        DiagnosticOPR_Fault_TagForm_.Dispose();
                    }
                    else
                    {
                        uint id = Convert.ToUInt32(listViewTags.SelectedItems[0].Name.Substring(1));
                        MaintenanceTag Fault_MissedFaultItem_Tag_ = new MaintenanceTagSQL(DB).GetMaintenanceTaginfo_ByID (id);
                        Fault_MissedFaultItem_TagForm Fault_MissedFaultItem_TagForm_ = new Fault_MissedFaultItem_TagForm(DB, Fault_MissedFaultItem_Tag_, true);
                        Fault_MissedFaultItem_TagForm_.ShowDialog();
                        if (Fault_MissedFaultItem_TagForm_.DialogResult == DialogResult.OK)
                        {
                            FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List(_MaintenanceFault);
                            RefreshTagList(FaultTagList_);
                        }
                        Fault_MissedFaultItem_TagForm_.Dispose();
                    }
                }
                catch
                {

                }

            }
        }
        private void OpenTag_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewTags.SelectedItems.Count > 0)
            {
                try
                {
                    string s = listViewTags.SelectedItems[0].Name.Substring(0, 1);
                    if (s == "D")
                    {
                        uint id = Convert.ToUInt32(listViewTags.SelectedItems[0].Name.Substring(1));
                        MaintenanceTag  DiagnosticOPR_Fault_Tag_ = new MaintenanceTagSQL(DB).GetMaintenanceTaginfo_ByID (id);
                        DiagnosticOPR_Fault_TagForm DiagnosticOPR_Fault_TagForm_ = new DiagnosticOPR_Fault_TagForm(DB, DiagnosticOPR_Fault_Tag_, false);
                        DiagnosticOPR_Fault_TagForm_.ShowDialog();
                        if (DiagnosticOPR_Fault_TagForm_.DialogResult == DialogResult.OK)
                        {
                            FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List(_MaintenanceFault);
                            RefreshTagList(FaultTagList_);
                        }
                        DiagnosticOPR_Fault_TagForm_.Dispose();
                    }
                    else
                    {
                        uint id = Convert.ToUInt32(listViewTags.SelectedItems[0].Name.Substring(1));
                        MaintenanceTag Fault_MissedFaultItem_Tag_ = new MaintenanceTagSQL(DB).GetMaintenanceTaginfo_ByID (id);
                        Fault_MissedFaultItem_TagForm Fault_MissedFaultItem_TagForm_ = new Fault_MissedFaultItem_TagForm(DB, Fault_MissedFaultItem_Tag_, false );
                        Fault_MissedFaultItem_TagForm_.ShowDialog();
                        if (Fault_MissedFaultItem_TagForm_.DialogResult == DialogResult.OK)
                        {
                            FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List(_MaintenanceFault);
                            RefreshTagList(FaultTagList_);
                        }
                        Fault_MissedFaultItem_TagForm_.Dispose();
                    }

                }
                catch
                {

                }

            }
        }
        private void AddDiagnosticOPRTag_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiagnosticOPR_Fault_TagForm DiagnosticOPR_Fault_TagForm_ = new DiagnosticOPR_Fault_TagForm(DB, _MaintenanceFault );
                DialogResult d = DiagnosticOPR_Fault_TagForm_.ShowDialog();
                if (DiagnosticOPR_Fault_TagForm_.DialogResult == DialogResult.OK)
                {
                    FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List(_MaintenanceFault);
                    RefreshTagList(FaultTagList_);
                }
                DiagnosticOPR_Fault_TagForm_.Dispose();
            }
            catch
            {

            }

        }
        private void AddMissedFaultItemTag_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Fault_MissedFaultItem_TagForm Fault_MissedFaultItem_TagForm_ = new Fault_MissedFaultItem_TagForm(DB, _MaintenanceFault );
                DialogResult d = Fault_MissedFaultItem_TagForm_.ShowDialog();
                if (Fault_MissedFaultItem_TagForm_.DialogResult == DialogResult.OK)
                {
                    FaultTagList_ = new MaintenanceTagSQL(DB).Get_Fault_Tag_List(_MaintenanceFault);
                    RefreshTagList(FaultTagList_);
                }
                Fault_MissedFaultItem_TagForm_.Dispose();
            }
            catch
            {

            }


        }
        private async void RefreshTagList(List<MaintenanceTag> TagSummaryList)
        {

            listViewTags.Items.Clear();
            for (int i = 0; i < TagSummaryList.Count; i++)
            {
                
                ListViewItem ListViewItem_ = new ListViewItem(TagSummaryList[i].TagID.ToString ());
 
                ListViewItem_.Name =  TagSummaryList[i].TagID.ToString();
                ListViewItem_.SubItems.Add(TagSummaryList[i]._DiagnosticOPR .DiagnosticOPRID.ToString());
                ListViewItem_.SubItems.Add(TagSummaryList[i]._DiagnosticOPR.Desc );
                ListViewItem_.SubItems.Add(TagSummaryList[i].TagInfo);
                listViewTags.Items.Add(ListViewItem_);

            }

        }
        private void listViewTag_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewTags.SelectedItems.Count > 0)
            {
                OpenTag_MenuItem.PerformClick();
            }
        }
        private void listViewTag_MouseDown(object sender, MouseEventArgs e)
        {
            listViewTags.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewTags.Items)
                {
                    if (item1.Bounds.Contains(new Point(e.X, e.Y)))
                    {
                        match = true;
                        listitem = item1;
                        break;
                    }
                }
                if (match)
                {


                    MenuItem[] mi1 = new MenuItem[] { OpenTag_MenuItem, UpdateTag_MenuItem, DeleteTag_MenuItem, new MenuItem("-"), AddDiagnosticOPRTag_MenuItem, AddMissedFaultItemTag_MenuItem };
                    listViewTags.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddDiagnosticOPRTag_MenuItem, AddMissedFaultItemTag_MenuItem };
                    listViewTags.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        public async void AdjustlistViewTag_ColumnsWidth()
        {
            try
            {
                listViewTags.Columns[0].Width = 150;
                listViewTags.Columns[1].Width = 60;
                listViewTags.Columns[2].Width = 150;
                listViewTags.Columns[3].Width = listViewTags.Width - 380;

            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("AdjustlistViewMissed_Fault_ColumnsWidth" + Environment.NewLine + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void listViewTags_Resize(object sender, EventArgs e)
        {
            AdjustlistViewTag_ColumnsWidth();
        }
        #endregion

    }
}
