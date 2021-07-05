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
    public partial class DiagnosticOPRForm : Form
    {
        DiagnosticOPRSQL DiagnosticOPRSQL_;
        DatabaseInterface DB;
        MaintenanceOPR _MaintenanceOPR;
        DiagnosticOPR _ParentDiagnosticOPR;
        DiagnosticOPR _DiagnosticOPR;
        Item _Item;
        Folder LastUsedFolder;
        List<MeasureOPR> _MeasureOPRList=new List<MeasureOPR> ();
        List<Missed_Fault_Item> Missed_Fault_Item_List = new List<Missed_Fault_Item>();

        List<MaintenanceTag > _FaultTagList = new List<MaintenanceTag>();
        List<DiagnosticFile> _DiagnosticOPRFileList = new List<DiagnosticFile>();
        List<DiagnosticOPRReport> _SubDiagnosticOPRList = new List<DiagnosticOPRReport>();


        ////////////////////////////////
        MenuItem OpenSubDiagnosticOPR_MenuItem;
        MenuItem AddSubDiagnosticOPR_MenuItem;
        MenuItem UpdateSubDiagnosticOPR_MenuItem;
        MenuItem DeleteSubDiagnosticOPR_MenuItem;

        MenuItem OpenMeasureOPR_MenuItem;
        MenuItem AddMeasureOPR_MenuItem;
        MenuItem UpdateMeasureOPR_MenuItem;
        MenuItem DeleteMeasureOPR_MenuItem;

        MenuItem Open_MissedFault_Item_MenuItem;
        MenuItem Add_MissedFault_Item_MenuItem;
        MenuItem Update_MissedFault_Item_MenuItem;
        MenuItem Delete_MissedFault_Item_MenuItem;

        MenuItem OpenTag_MenuItem;
        MenuItem AddFaultTag_MenuItem;
        MenuItem UpdateFaultTag_MenuItem;
        MenuItem DeleteFaultTag_MenuItem;

        MenuItem OpenFile_MenuItem;
        MenuItem AddFile_MenuItem;
        MenuItem SaveFile_MenuItem;
        MenuItem UpdateFileInfo_MenuItem;
        MenuItem DeleteFile_MenuItem;
        private bool Changed_;
        public bool Changed
        {
            get { return Changed_; }
        }
        public DiagnosticOPRForm(DatabaseInterface db,MaintenanceOPR MaintenanceOPR_, DiagnosticOPR ParentDiagnosticOPR_)
        {
            DB = db;
            DiagnosticOPRSQL_ = new DiagnosticOPRSQL(DB);
            _MaintenanceOPR = MaintenanceOPR_;
            _ParentDiagnosticOPR = ParentDiagnosticOPR_;
            InitializeComponent();
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انشاء";
            textBoxMaintenanceOPRID.Text = MaintenanceOPR_._Operation.OperationID.ToString();
            textBoxContactName.Text = MaintenanceOPR_._Contact.ContactName;
            if(ParentDiagnosticOPR_ !=null )
            {
                textBoxParentDiagnosticOPRID.Text = ParentDiagnosticOPR_.DiagnosticOPRID.ToString();
                textBoxParentDiagnosticOPRDesc.Text = ParentDiagnosticOPR_.Desc;
            }
            else
            {
                textBoxParentDiagnosticOPRID.Text = "-";
                textBoxParentDiagnosticOPRDesc.Text = "-";
            }
            this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
            this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);

            comboBoxNormal.SelectedIndex = 0;
            InitializeMenuItems();
            DiagnosticOPRReport. InitializeDiagnosticOPRListViewColumns (ref listViewSubDiagnosticOPR);
            textBoxID.Text = " - ";
           AdjustlistViewTag_ColumnsWidth();
        }

        public DiagnosticOPRForm(DatabaseInterface db, DiagnosticOPR DiagnosticOPR_, bool Edit)
        {
            try
            {
                DB = db;
                InitializeComponent();
                DiagnosticOPRSQL_ = new DiagnosticOPRSQL(DB);
                _DiagnosticOPR = DiagnosticOPR_;
                _MaintenanceOPR = _DiagnosticOPR._MaintenanceOPR;
                textBoxMaintenanceOPRID.Text = _MaintenanceOPR._Operation.OperationID.ToString();
                textBoxContactName.Text = _MaintenanceOPR._Contact.ContactName;

                if (_DiagnosticOPR.ParentDiagnosticOPRID == null)
                {
                    _ParentDiagnosticOPR = null;
                    textBoxParentDiagnosticOPRID.Text = "-";
                    textBoxParentDiagnosticOPRDesc.Text = "-";
                }
                else
                {
                    _ParentDiagnosticOPR = DiagnosticOPRSQL_.GetDiagnosticOPRINFO_BYID(Convert.ToUInt32(_DiagnosticOPR.ParentDiagnosticOPRID));
                    textBoxParentDiagnosticOPRID.Text = _ParentDiagnosticOPR.DiagnosticOPRID.ToString();
                    textBoxParentDiagnosticOPRDesc.Text = _ParentDiagnosticOPR.Desc;

                }

                DiagnosticOPRReport.InitializeDiagnosticOPRListViewColumns(ref listViewSubDiagnosticOPR);
                InitializeMenuItems();
                AdjustlistViewTag_ColumnsWidth();
                LoadForm(Edit);
            }
            catch (Exception ee)
            {

            }
           




        }
        public void InitializeMenuItems()
        {
            OpenSubDiagnosticOPR_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية الفحص", OpenSubDiagnosticOPR_MenuItem_Click );
            AddSubDiagnosticOPR_MenuItem  = new System.Windows.Forms.MenuItem("اضافة عملية فحص فرعية", AddSubDiagnosticOPR_MenuItem_Click);
            UpdateSubDiagnosticOPR_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditSubDiagnosticOPR_MenuItem_Click);
            DeleteSubDiagnosticOPR_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteSubDiagnosticOPR_MenuItem_Click);

            OpenMeasureOPR_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية القياس", OpenMeasureOPR_MenuItem_Click);
            AddMeasureOPR_MenuItem = new System.Windows.Forms.MenuItem("اضافة عملية قياس", AddMeasureOPR_MenuItem_Click);
            UpdateMeasureOPR_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditMeasureOPR_MenuItem_Click);
            DeleteMeasureOPR_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteMeasureOPR_MenuItem_Click);


            Open_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("عرض تفاصيل", Open_MissedFault_Item_MenuItem_Click);
            Add_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("اضافة عنصر مفقود او تالف", Add_MissedFault_Item_MenuItem_Click);
            Update_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", Edit_MissedFault_Item_MenuItem_Click);
            Delete_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("حذف", Delete_MissedFault_Item_MenuItem_Click);


            OpenTag_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل الرابط", OpenTag_MenuItem_Click);
            AddFaultTag_MenuItem = new System.Windows.Forms.MenuItem("ربط مع عطل", AddFaultTag_MenuItem_Click);
            UpdateFaultTag_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditTag_MenuItem_Click);
            DeleteFaultTag_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteTag_MenuItem_Click);


            OpenFile_MenuItem = new MenuItem("فتح الملف", OpenFile_MenuItem_Click);
            AddFile_MenuItem = new MenuItem("اضافة ملف", AddFile_MenuItem_Click);
            SaveFile_MenuItem = new MenuItem("حفظ الملف على وحدة تخزين", SaveFile_MenuItem_Click);
            UpdateFileInfo_MenuItem = new MenuItem("تعديل معلومات الملف", UpdateFileInfo_MenuItem_Click);
            DeleteFile_MenuItem = new MenuItem("حذف الملف", DeleteFile_MenuItem_Click);
        }
        public void LoadForm(bool Edit)
        {
            if (_DiagnosticOPR  == null) return;
            _DiagnosticOPRFileList  = new DiagnosticOPRFileSQL(DB).GetDiagnosticOPRFileList (_DiagnosticOPR );
            _MeasureOPRList = new MeasureOPRSQL(DB).GetMeasureOPRList(_DiagnosticOPR);
            _SubDiagnosticOPRList  = new DiagnosticOPRSQL(DB).GetSubDiagnosticOPRReportList(_MaintenanceOPR ,_DiagnosticOPR  );
            _FaultTagList   = new MaintenanceTagSQL(DB).Get_DiagnosticOPR_Tag_List (_DiagnosticOPR );
            Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
           

            buttonSave.Name = "buttonSave";
            buttonSave.Text = "حفظ";
            _Item = _DiagnosticOPR ._Item;
            textBoxID.Text = _DiagnosticOPR.DiagnosticOPRID.ToString();
            textBoxDesc .Text = _DiagnosticOPR.Desc ;
            textBoxLocation .Text = _DiagnosticOPR.Location ;
            dateTimePickerOPRDate.Value = _DiagnosticOPR.DiagnosticOPRDate;
            textBoxReport.Text = _DiagnosticOPR.Report;
            LoadItemData();
            RefreshDiagnosticOPRFilesItems(_DiagnosticOPRFileList);
            RefreshMeasureOPRList(_MeasureOPRList);
            RefreshTagList(_FaultTagList);
            RefreshMissed_FaultList(Missed_Fault_Item_List);
            DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, _SubDiagnosticOPRList);




            if (_DiagnosticOPR .Normal  == null) comboBoxNormal.SelectedIndex = 0;
            else if (Convert .ToBoolean ( _DiagnosticOPR.Normal) ) comboBoxNormal.SelectedIndex = 1;
            else comboBoxNormal.SelectedIndex = 2;
            

            if (Edit)
            {
         
                this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
                this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
                this.listViewDiagnosticFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewDiagnosticOPRFiles_MouseDoubleClick);
                this.listViewDiagnosticFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewDiagnosticOPRFiles_MouseDown);
                this.listViewSubDiagnosticOPR.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewSubDiagnosticOPR_MouseDoubleClick);
                this.listViewSubDiagnosticOPR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewSubDiagnosticOPR_MouseDown);
                this.listViewMeasureOPR.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewMeasureOPR_MouseDoubleClick);
                this.listViewMeasureOPR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewMeasureOPR_MouseDown);
                this.listViewTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewTag_MouseDoubleClick);
                this.listViewTags.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewTag_MouseDown);
                this.listViewMissedFaultItem.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MissedFault_Item_MouseDoubleClick);
                this.listViewMissedFaultItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView_MissedFault_Item_MouseDown);


                textBoxDesc.ReadOnly = false;
                textBoxLocation .ReadOnly = false;
                textBoxReport  .ReadOnly = false;
                textBoxItemID.ReadOnly = false;
                dateTimePickerOPRDate .Enabled  = true ;
                comboBoxNormal .Enabled  = true ;

            }
            else
            {
             
                buttonSave.Visible = false;
                textBoxDesc.ReadOnly = true ;
                textBoxLocation.ReadOnly = true;
                textBoxReport.ReadOnly = true;
                textBoxItemID.ReadOnly = true;
                dateTimePickerOPRDate.Enabled = false ;
                comboBoxNormal.Enabled = false ;
;

            }


        }
        private void buttonSave_Click(object sender, EventArgs e)
        {

            if (_MaintenanceOPR  == null) return;
            //ComboboxItem consumeunititem = (ComboboxItem)comboBoxConsumeUnt.SelectedItem;
            //ConsumeUnit _ConsumeUnit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunititem.Value);

            //ComboboxItem ComboboxItem_selltype = (ComboboxItem)comboBoxSellType.SelectedItem;
            //string SellType_ = ComboboxItem_selltype.Text;

            if (buttonSave.Name == "buttonAdd")
            {
                try
                {
                    uint? ParentID;
                    try
                    {
                        ParentID = _ParentDiagnosticOPR.DiagnosticOPRID;
                    }
                    catch
                    {
                        ParentID = null;
                    }
                    bool? Normal;
                    if (comboBoxNormal.SelectedIndex == 1) Normal = true ;
                    else if (comboBoxNormal.SelectedIndex == 2) Normal =false ;
                    else Normal  = null;
                    DateTime oprdate= dateTimePickerOPRDate .Value;

                    DiagnosticOPR DiagnosticOPR_ = DiagnosticOPRSQL_.AddDiagnosticOPR 
                        (_MaintenanceOPR ._Operation.OperationID,ParentID,oprdate , _Item, textBoxDesc .Text
                        , textBoxLocation .Text, Normal, textBoxReport .Text);
                    if (DiagnosticOPR_ != null)
                    {
                        _DiagnosticOPR  = DiagnosticOPR_;
                        MessageBox.Show("تم الاضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Changed_ = true;
                        this.Close();

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":تعذر انشاء عملية الصيانة " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                try
                {
                    if (_DiagnosticOPR    != null)
                    {
                        uint? ParentID;
                        try
                        {
                            ParentID = _ParentDiagnosticOPR.DiagnosticOPRID;
                        }
                        catch
                        {
                            ParentID = null;
                        }
                        bool? Normal;
                        if (comboBoxNormal.SelectedIndex == 1) Normal = true;
                        else if (comboBoxNormal.SelectedIndex == 2) Normal = false;
                        else Normal = null;
                        DateTime oprdate = dateTimePickerOPRDate.Value;
                        bool success = DiagnosticOPRSQL_.UpdateDiagnosticOPR 
                            (_DiagnosticOPR .DiagnosticOPRID,oprdate , _Item
                            , textBoxDesc.Text
                        , textBoxLocation.Text, Normal, textBoxReport.Text);
                        if (success == true)
                        {
                            MessageBox.Show("تم حفظ  بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _DiagnosticOPR  = DiagnosticOPRSQL_.GetDiagnosticOPRINFO_BYID (_DiagnosticOPR .DiagnosticOPRID);
                            this.Changed_ = true;
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
        private async void LoadItemData()
        {
            if (_Item !=null )
            {
                LastUsedFolder = _Item.folder;
                textBoxItemID.Text = _Item.ItemID.ToString();
                textBoxItemName.Text = _Item.ItemName;
                textBoxItemCompany.Text = _Item.ItemCompany;
                textBoxItemType.Text = _Item.folder.FolderName;
            }
            else
            {
                LastUsedFolder = null;
                textBoxItemID.Text = "";
                textBoxItemName.Text = "-";
                textBoxItemCompany.Text = "-";
                textBoxItemType.Text = "-";
            }
          

        }
       
        #region SubDiagnosticOPR
        private void DeleteSubDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewSubDiagnosticOPR .SelectedItems[0].Name);
                bool success = DiagnosticOPRSQL_.DeleteDiagnosticOPR (sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _SubDiagnosticOPRList  = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR,_DiagnosticOPR);
                    DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, _SubDiagnosticOPRList);

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
        private void EditSubDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewSubDiagnosticOPR.SelectedItems.Count > 0)
            {
                uint itemoutid = Convert.ToUInt32(listViewSubDiagnosticOPR.SelectedItems[0].Name);
                DiagnosticOPR DiagnosticOPR_ =DiagnosticOPRSQL_.GetDiagnosticOPRINFO_BYID (itemoutid);
                DiagnosticOPRForm DiagnosticOPRForm_ = new DiagnosticOPRForm (DB, DiagnosticOPR_, true);
                DiagnosticOPRForm_.ShowDialog();
                if (DiagnosticOPRForm_.Changed)
                {
                    _SubDiagnosticOPRList = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR, _DiagnosticOPR);
                    DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, _SubDiagnosticOPRList);
                }
                DiagnosticOPRForm_.Dispose();
            }
        }
        private void OpenSubDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {

            if (listViewSubDiagnosticOPR.SelectedItems.Count > 0)
            {
                uint itemoutid = Convert.ToUInt32(listViewSubDiagnosticOPR.SelectedItems[0].Name);
                DiagnosticOPR DiagnosticOPR_ = DiagnosticOPRSQL_.GetDiagnosticOPRINFO_BYID(itemoutid);
                DiagnosticOPRForm DiagnosticOPRForm_ = new DiagnosticOPRForm(DB, DiagnosticOPR_, false );
                DiagnosticOPRForm_.ShowDialog();
                if (DiagnosticOPRForm_.Changed)
                {
                    _SubDiagnosticOPRList = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR, _DiagnosticOPR);
                    DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, _SubDiagnosticOPRList);
                }
                DiagnosticOPRForm_.Dispose();
            }
        }
        private void AddSubDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {
            DiagnosticOPRForm DiagnosticOPRForm_ = new DiagnosticOPRForm(DB, _MaintenanceOPR,_DiagnosticOPR );
            DialogResult d = DiagnosticOPRForm_.ShowDialog();
            if (DiagnosticOPRForm_.Changed)
            {
                _SubDiagnosticOPRList = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR, _DiagnosticOPR);
                DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, _SubDiagnosticOPRList);
            }
            DiagnosticOPRForm_.Dispose();
        }
       
        private void listViewSubDiagnosticOPR_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewSubDiagnosticOPR .SelectedItems.Count > 0)
            {
                OpenSubDiagnosticOPR_MenuItem.PerformClick();
            }
        }
        private void listViewSubDiagnosticOPR_MouseDown(object sender, MouseEventArgs e)
        {
            listViewSubDiagnosticOPR.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewSubDiagnosticOPR.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenSubDiagnosticOPR_MenuItem 
                        , UpdateSubDiagnosticOPR_MenuItem, DeleteSubDiagnosticOPR_MenuItem, new MenuItem("-"), AddSubDiagnosticOPR_MenuItem };
                    listViewSubDiagnosticOPR.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddSubDiagnosticOPR_MenuItem  };
                    listViewSubDiagnosticOPR.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        private void listViewSubDiagnosticOPR_Resize(object sender, EventArgs e)
        {
            DiagnosticOPRReport.AdjustlistViewDiagnosticOPRColumnsWidth(ref listViewSubDiagnosticOPR);
        }

    
        #endregion
        #region MeasureOPR
        private void DeleteMeasureOPR_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewMeasureOPR.SelectedItems[0].Name);
                bool success = new MeasureOPRSQL(DB).DeleteMeasureOPR (sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _MeasureOPRList = new MeasureOPRSQL(DB).GetMeasureOPRList(_DiagnosticOPR );
                    RefreshMeasureOPRList(_MeasureOPRList);

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
        private void EditMeasureOPR_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMeasureOPR.SelectedItems.Count > 0)
            {
                try
                {
                    uint id = Convert.ToUInt32(listViewMeasureOPR.SelectedItems[0].Name);
                    MeasureOPR MeasureOPR_ = new MeasureOPRSQL (DB).GetMeasureOPRinfo_ByID(id);
                    MeasureOPRForm MeasureOPRForm_ = new MeasureOPRForm(DB, MeasureOPR_, true);
                    MeasureOPRForm_.ShowDialog();
   

                    if (MeasureOPRForm_.DialogResult==DialogResult.OK )
                    {
                        _MeasureOPRList = new MeasureOPRSQL(DB).GetMeasureOPRList(_DiagnosticOPR);
                        RefreshMeasureOPRList(_MeasureOPRList);
                    }
                    MeasureOPRForm_.Dispose();

                }
                catch
                {

                }
               
            }
        }
        private void OpenMeasureOPR_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMeasureOPR.SelectedItems.Count > 0)
            {
                try
                {
                    uint id = Convert.ToUInt32(listViewMeasureOPR.SelectedItems[0].Name);
                    MeasureOPR MeasureOPR_ = new MeasureOPRSQL(DB).GetMeasureOPRinfo_ByID(id);
                    MeasureOPRForm MeasureOPRForm_ = new MeasureOPRForm(DB, MeasureOPR_, false );
                    MeasureOPRForm_.ShowDialog();
                    if (MeasureOPRForm_.DialogResult == DialogResult.OK)
                    {
                        _MeasureOPRList = new MeasureOPRSQL(DB).GetMeasureOPRList(_DiagnosticOPR);
                        RefreshMeasureOPRList(_MeasureOPRList);
                    }
                    MeasureOPRForm_.Dispose();

                }
                catch
                {

                }

            }
        }
        private void AddMeasureOPR_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MeasureOPRForm MeasureOPRForm_ = new MeasureOPRForm(DB, _DiagnosticOPR );
                DialogResult d = MeasureOPRForm_.ShowDialog();
                if (MeasureOPRForm_.DialogResult==DialogResult.OK)
                {
                    _MeasureOPRList = new MeasureOPRSQL(DB).GetMeasureOPRList(_DiagnosticOPR);
                    RefreshMeasureOPRList(_MeasureOPRList);
                }
                MeasureOPRForm_.Dispose();
            }
            catch
            {

            }
            
        }
        private async void RefreshMeasureOPRList(List<MeasureOPR> MeasureOPRList)
        {

            listViewMeasureOPR.Items.Clear();
            for (int i = 0; i < MeasureOPRList.Count; i++)
            {
                ListViewItem ListViewItem_ = new ListViewItem(MeasureOPRList[i].Desc);
                ListViewItem_.Name = MeasureOPRList[i].MeasureID.ToString();
                ListViewItem_.SubItems.Add(MeasureOPRList[i].Value .ToString ());
                ListViewItem_.SubItems.Add(MeasureOPRList[i].MeasureUnit);
                if (MeasureOPRList[i].Normal == null)
                {
                    ListViewItem_.SubItems.Add("غير معروف");
                    ListViewItem_.BackColor = Color.LightYellow;
                }
                else if (MeasureOPRList[i].Normal == true)
                {
                    ListViewItem_.SubItems.Add("طبيعي");
                    ListViewItem_.BackColor = Color.LimeGreen ;
                }
                else
                {
                    ListViewItem_.SubItems.Add("غير طبيعي");
                    ListViewItem_.BackColor = Color.Orange ;
                }
                listViewMeasureOPR.Items.Add(ListViewItem_);

            }

        }
        private void listViewMeasureOPR_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewMeasureOPR.SelectedItems.Count > 0)
            {
                OpenMeasureOPR_MenuItem .PerformClick();
            }
        }
        private void listViewMeasureOPR_MouseDown(object sender, MouseEventArgs e)
        {
            listViewMeasureOPR.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewMeasureOPR.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenMeasureOPR_MenuItem , UpdateMeasureOPR_MenuItem , DeleteMeasureOPR_MenuItem , new MenuItem("-"), AddMeasureOPR_MenuItem };
                    listViewMeasureOPR.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddMeasureOPR_MenuItem  };
                    listViewMeasureOPR.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        #endregion


        #region Missed_Fault_Item
        private void Delete_MissedFault_Item_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من حذف العطل و جميع العمليات التابعة له؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewMissedFaultItem.SelectedItems[0].Name);
                bool success = new MissedFaultItemSQL(DB).DeleteMissed_Fault_Item(sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
                    RefreshMissed_FaultList( Missed_Fault_Item_List);

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
        private void Edit_MissedFault_Item_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMissedFaultItem.SelectedItems.Count > 0)
            {
                uint id = Convert.ToUInt32(listViewMissedFaultItem.SelectedItems[0].Name);
                Missed_Fault_Item Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID (id);
                MissedFault_Item_Form MissedFault_Item_Form_ = new MissedFault_Item_Form(DB, Missed_Fault_Item_, true);
                MissedFault_Item_Form_.ShowDialog();
                if (MissedFault_Item_Form_.Changed)
                {
                    Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
                    RefreshMissed_FaultList( Missed_Fault_Item_List);
                }
                MissedFault_Item_Form_.Dispose();
            }
        }
        private void Open_MissedFault_Item_MenuItem_Click(object sender, EventArgs e)
        {

            if (listViewMissedFaultItem.SelectedItems.Count > 0)
            {
                uint id = Convert.ToUInt32(listViewMissedFaultItem.SelectedItems[0].Name);
                Missed_Fault_Item Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID (id);
                MissedFault_Item_Form MissedFault_Item_Form_ = new MissedFault_Item_Form(DB, Missed_Fault_Item_, false);
                MissedFault_Item_Form_.ShowDialog();
                if (MissedFault_Item_Form_.Changed)
                {
                    Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
                    RefreshMissed_FaultList(Missed_Fault_Item_List);
                }
                MissedFault_Item_Form_.Dispose();
            }
        }
        private void Add_MissedFault_Item_MenuItem_Click(object sender, EventArgs e)
        {
            MissedFault_Item_Form MissedFault_Item_Form_ = new MissedFault_Item_Form(DB, _DiagnosticOPR );
            DialogResult d = MissedFault_Item_Form_.ShowDialog();
            if (MissedFault_Item_Form_.Changed)
            {
                Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
                RefreshMissed_FaultList( Missed_Fault_Item_List);
            }
            MissedFault_Item_Form_.Dispose();
        }
        internal async  void RefreshMissed_FaultList( List<Missed_Fault_Item> Missed_Fault_ItemList)
        {
            listViewMissedFaultItem.Items.Clear();
            for (int i = 0; i < Missed_Fault_ItemList.Count; i++)
            {
                string type = "";
                System.Windows.Forms.ListViewItem ListViewItem_ ;
             
                if (Missed_Fault_ItemList[i].Type == Missed_Fault_Item.FAULT_ITEM)
                {
                    ListViewItem_ = new System.Windows.Forms.ListViewItem("تالف");
                    ListViewItem_.BackColor = System.Drawing.Color.SandyBrown;
                }
                else
                {
                    ListViewItem_ = new System.Windows.Forms.ListViewItem("مفقود");
                    ListViewItem_.BackColor = System.Drawing.Color.PeachPuff;
                }
                ListViewItem_.Name = Missed_Fault_ItemList[i].ID.ToString();
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i]._Item.ItemID .ToString ());
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i]._Item.folder.FolderName);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i]._Item.ItemName);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i]._Item.ItemCompany);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i].Location);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i].Notes);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i].TagsCount.ToString());

                listViewMissedFaultItem.Items.Add(ListViewItem_);

            }


        }
        private void listView_MissedFault_Item_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewMissedFaultItem.SelectedItems.Count > 0)
            {
                Open_MissedFault_Item_MenuItem.PerformClick();
            }
        }
        private void listView_MissedFault_Item_MouseDown(object sender, MouseEventArgs e)
        {
            listViewMissedFaultItem.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewMissedFaultItem.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { Open_MissedFault_Item_MenuItem
                        , Update_MissedFault_Item_MenuItem, Delete_MissedFault_Item_MenuItem, new MenuItem("-"), Add_MissedFault_Item_MenuItem };
                    listViewMissedFaultItem.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { Add_MissedFault_Item_MenuItem };
                    listViewMissedFaultItem.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        private void listView_MissedFault_Item_Resize(object sender, EventArgs e)
        {
            //MaintenanceFaultReport .AdjustlistViewFaultReportOPRColumnsWidth (ref listViewSubDiagnosticOPR);
            AdjustlistView_MissedFault_ItemOPRColumnsWidth(listViewMissedFaultItem);
        }
        public async void AdjustlistView_MissedFault_ItemOPRColumnsWidth(ListView listview)
        {
            try
            {
                listview.Columns[0].Width = 60;
                listview.Columns[1].Width = 80;
                listview.Columns[2].Width = 125;
                listview.Columns[3].Width = 125;
                listview.Columns[4].Width = 125;
                listview.Columns[5].Width = 125;
                listview.Columns[6].Width = listview.Width - 880;
                listview.Columns[7].Width = 230;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("AdjustlistViewMissedFaultItemColumnsWidth" + Environment.NewLine + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        #endregion
        #region DiagnosticOPRFile
        public async void OpenItemFile()
        {
            try
            {
                uint fileid = Convert.ToUInt32(listViewDiagnosticFiles .SelectedItems[0].Name);

                if (!System.IO.Path.HasExtension(listViewDiagnosticFiles.SelectedItems[0].SubItems[0].Text))
                {
                    MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                string path = Application.StartupPath + "\\" + "OverLoadTemp";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                string fileName = path + "\\" + listViewDiagnosticFiles.SelectedItems[0].SubItems[0].Text + ".tmp" + System.IO.Path.GetExtension(listViewDiagnosticFiles.SelectedItems[0].SubItems[0].Text);

                byte[] filedata = (new DiagnosticOPRFileSQL (DB)).GetFileData(fileid);

                System.IO.File.WriteAllBytes(fileName, filedata);

                System.Diagnostics.Process p = System.Diagnostics.Process.Start(fileName);
                new System.Threading.Thread(delegate () {
                    kill_process(p, fileName);
                }).Start();

            }
            catch
            {

            }

        }
        public void kill_process(System.Diagnostics.Process p, string filename)
        {
            try
            {
                while (!p.HasExited)
                {
                    System.Threading.Thread.Sleep(100);
                }

                System.IO.File.Delete(filename);
            }
            catch { }

        }
        private void OpenFile_MenuItem_Click(object sender, EventArgs e)
        {
            OpenItemFile();
        }

        private void AddFile_MenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Reset();
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AddDiagnosticFile AddItemFile_ = new AddDiagnosticFile(DB, _DiagnosticOPR , openFileDialog1);
                AddItemFile_.ShowDialog();
                if (AddItemFile_.DialogResult == DialogResult.OK)
                {
                    MessageBox.Show("تم ادخال الملف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _DiagnosticOPRFileList = new DiagnosticOPRFileSQL(DB).GetDiagnosticOPRFileList(_DiagnosticOPR);
                    RefreshDiagnosticOPRFilesItems(_DiagnosticOPRFileList);
                }

            }
        }
        private void SaveFile_MenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowserDialog_ = new FolderBrowserDialog();
            FolderBrowserDialog_.Reset();
            //FolderBrowserDialog_. = Application.StartupPath;

            if (FolderBrowserDialog_.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    for (int i = 0; i < listViewDiagnosticFiles.SelectedItems.Count; i++)
                    {
                        uint fileid = Convert.ToUInt32(listViewDiagnosticFiles.SelectedItems[i].Name);
                        byte[] filedata = (new DiagnosticOPRFileSQL(DB)).GetFileData(fileid);
                        if (filedata == null) throw new Exception("الملف فارغ");
                        string f = FolderBrowserDialog_.SelectedPath + "\\" + listViewDiagnosticFiles.SelectedItems[i].SubItems[0].Text;
                        if (System.IO.File.Exists(f))
                        {
                            DialogResult d = MessageBox.Show("الملف " + f + " موجود بلفعل هل تريد استبداله !", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if (d != DialogResult.OK) continue;
                        }
                        System.IO.File.WriteAllBytes(f, filedata);
                    }
                    MessageBox.Show("تم ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }

            }
        }
        private void UpdateFileInfo_MenuItem_Click(object sender, EventArgs e)
        {
            DiagnosticFile DiagnosticFile_ = new DiagnosticFile(_DiagnosticOPR ,
                Convert.ToUInt32(listViewDiagnosticFiles.SelectedItems[0].Name)
                , listViewDiagnosticFiles.SelectedItems[0].SubItems[0].Text
                , listViewDiagnosticFiles.SelectedItems[0].SubItems[1].Text
                , -1, DateTime.Now);
            AddDiagnosticFile AddItemFile_ = new AddDiagnosticFile(DB, DiagnosticFile_);
            AddItemFile_.ShowDialog();
            if (AddItemFile_.DialogResult == DialogResult.OK)
            {
                MessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _DiagnosticOPRFileList = new DiagnosticOPRFileSQL(DB).GetDiagnosticOPRFileList(_DiagnosticOPR);
                RefreshDiagnosticOPRFilesItems(_DiagnosticOPRFileList);
            }
        }
        private void DeleteFile_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("هل انت متاكد من  حذف الملف؟" + listViewDiagnosticFiles .SelectedItems[0].SubItems[0].Text, "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (d != DialogResult.OK) return;


            try
            {
                uint fileid = Convert.ToUInt32(listViewDiagnosticFiles.SelectedItems[0].Name);
                if ((new DiagnosticOPRFileSQL(DB)).DeleteDiagnosticOPRFile (fileid))
                {
                    _DiagnosticOPRFileList = new DiagnosticOPRFileSQL(DB).GetDiagnosticOPRFileList(_DiagnosticOPR);
                    RefreshDiagnosticOPRFilesItems(_DiagnosticOPRFileList);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void RefreshDiagnosticOPRFilesItems(List<DiagnosticFile> DiagnosticFileList__)
        {

            try
            {
                listViewDiagnosticFiles.Items.Clear();

                for (int i = 0; i < DiagnosticFileList__.Count; i++)
                {
                    ListViewItem list_item = new ListViewItem(DiagnosticFileList__[i].FileName);
                    list_item.Name = DiagnosticFileList__[i].FileID.ToString();
                    list_item.SubItems.Add(DiagnosticFileList__[i].FileDescription);
                    list_item.SubItems.Add(DiagnosticFileList__[i].AddDate.ToString());
                    string size = "";
                    if (DiagnosticFileList__[i].FileSize < 1000)
                    {
                        size = DiagnosticFileList__[i].FileSize + " بايت ";
                    }
                    else if (DiagnosticFileList__[i].FileSize < 1000000)
                    {
                        size = DiagnosticFileList__[i].FileSize / 1000 + " كيلو بايت ";
                    }
                    else
                    {
                        size = DiagnosticFileList__[i].FileSize / 1000000 + " ميغا بايت ";
                    }

                    list_item.SubItems.Add(size);
                    listViewDiagnosticFiles.Items.Add(list_item);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        public void ConfifurelistViewDiagnosticOPRFilesColumnsWidth()
        {
            listViewDiagnosticFiles.Columns[2].Width = 200;
            listViewDiagnosticFiles.Columns[3].Width = 100;
            listViewDiagnosticFiles.Columns[0].Width = (listViewDiagnosticFiles .Width - 300) / 2;
            listViewDiagnosticFiles.Columns[1].Width = (listViewDiagnosticFiles.Width - 300) / 2;
        }
        private void listViewDiagnosticOPRFiles_Resize(object sender, EventArgs e)
        {
            ConfifurelistViewDiagnosticOPRFilesColumnsWidth();
        }
        private void listViewDiagnosticOPRFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenItemFile();
        }
        private void listViewDiagnosticOPRFiles_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listViewDiagnosticFiles .ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                foreach (ListViewItem item1 in listViewDiagnosticFiles.Items)
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
                    MenuItem[] mi1 = new MenuItem[] { OpenFile_MenuItem, SaveFile_MenuItem, UpdateFileInfo_MenuItem, DeleteFile_MenuItem, new MenuItem("-"), AddFile_MenuItem };
                    listViewDiagnosticFiles.ContextMenu = new ContextMenu(mi1);
                }
                else
                {
                    MenuItem[] mi = new MenuItem[] { AddFile_MenuItem };
                    listViewDiagnosticFiles.ContextMenu = new ContextMenu(mi);
                }


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
                string s = listViewTags.SelectedItems[0].Name;
                    uint sid = Convert.ToUInt32(listViewTags.SelectedItems[0].Name);
                    bool success = new MaintenanceTagSQL(DB).DeleteMaintenanceTag (sid);
                    if (success)
                    {
                        MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _FaultTagList = new MaintenanceTagSQL (DB).Get_DiagnosticOPR_Tag_List(_DiagnosticOPR);
                        RefreshTagList(_FaultTagList);

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
                    string s = listViewTags.SelectedItems[0].Name;
                        uint id = Convert.ToUInt32(listViewTags.SelectedItems[0].Name.Substring (1));
                        MaintenanceTag DiagnosticOPR_Fault_Tag_ = new MaintenanceTagSQL(DB).GetMaintenanceTaginfo_ByID(id);
                        DiagnosticOPR_Fault_TagForm DiagnosticOPR_Fault_TagForm_ = new DiagnosticOPR_Fault_TagForm(DB, DiagnosticOPR_Fault_Tag_,true );
                        DiagnosticOPR_Fault_TagForm_.ShowDialog();
                        if (DiagnosticOPR_Fault_TagForm_.DialogResult == DialogResult.OK)
                        {
                            _FaultTagList = new MaintenanceTagSQL(DB).Get_DiagnosticOPR_Tag_List(_DiagnosticOPR);
                            RefreshTagList(_FaultTagList);
                        }
                        DiagnosticOPR_Fault_TagForm_.Dispose();
                    
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
                    string s = listViewTags.SelectedItems[0].Name;
                        uint id = Convert.ToUInt32(listViewTags.SelectedItems[0].Name.Substring(1));
                        MaintenanceTag DiagnosticOPR_Fault_Tag_ = new MaintenanceTagSQL(DB).GetMaintenanceTaginfo_ByID(id);
                        DiagnosticOPR_Fault_TagForm DiagnosticOPR_Fault_TagForm_ = new DiagnosticOPR_Fault_TagForm(DB, DiagnosticOPR_Fault_Tag_,false );
                        DiagnosticOPR_Fault_TagForm_.ShowDialog();
                        if (DiagnosticOPR_Fault_TagForm_.DialogResult == DialogResult.OK)
                        {
                        _FaultTagList = new MaintenanceTagSQL(DB).Get_DiagnosticOPR_Tag_List(_DiagnosticOPR);
                        RefreshTagList(_FaultTagList);
                        }
                        DiagnosticOPR_Fault_TagForm_.Dispose();
                    

                }
                catch
                {

                }

            }
        }
        private void AddFaultTag_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiagnosticOPR_Fault_TagForm DiagnosticOPR_Fault_TagForm_ = new DiagnosticOPR_Fault_TagForm(DB, _DiagnosticOPR);
                DialogResult d = DiagnosticOPR_Fault_TagForm_.ShowDialog();
                if (DiagnosticOPR_Fault_TagForm_.DialogResult == DialogResult.OK)
                {
                    _FaultTagList = new MaintenanceTagSQL(DB).Get_DiagnosticOPR_Tag_List(_DiagnosticOPR);
                    RefreshTagList(_FaultTagList);
                }
                DiagnosticOPR_Fault_TagForm_.Dispose();
            }
            catch
            {

            }

        }
       
        private async void RefreshTagList(List<MaintenanceTag  > FaultTagList_)
        {
             
            listViewTags.Items.Clear();
            for (int i = 0; i < FaultTagList_.Count; i++)
            { 

                ListViewItem ListViewItem_ = new ListViewItem(FaultTagList_[i].TagID.ToString());
                ListViewItem_.Name = FaultTagList_[i].TagID .ToString();
                ListViewItem_.SubItems.Add(FaultTagList_[i]._MaintenanceFault.FaultID .ToString());
                ListViewItem_.SubItems.Add(FaultTagList_[i]._MaintenanceFault.FaultDesc);
                ListViewItem_.SubItems.Add(FaultTagList_[i].TagInfo );

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


                    MenuItem[] mi1 = new MenuItem[] { OpenTag_MenuItem, UpdateFaultTag_MenuItem, DeleteFaultTag_MenuItem, new MenuItem("-"), AddFaultTag_MenuItem };
                    listViewTags.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddFaultTag_MenuItem };
                    listViewTags.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        public async   void AdjustlistViewTag_ColumnsWidth()
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
