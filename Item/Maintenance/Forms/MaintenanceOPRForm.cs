﻿using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.ItemObjSQL;
using ItemProject.ItemObj.Objects;
using ItemProject.Maintenance.MaintenanceSQL;
using ItemProject.Maintenance.Objects;
using ItemProject.Trade.Forms.TradeForms;
using ItemProject.Trade.Objects;
using ItemProject.Trade.TradeSQL;
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
    public partial class MaintenanceOPRForm : Form
    {
        System.Windows.Forms.MenuItem OpenItemOUTMenuItem;
        System.Windows.Forms.MenuItem AddItemOUTMenuItem;
        System.Windows.Forms.MenuItem EditItemOUTMenuItem;
        System.Windows.Forms.MenuItem DeleteItemOUTMenuItem;


        System.Windows.Forms.MenuItem OpenAccessory_MenuItem;
        System.Windows.Forms.MenuItem AddAccessory_MenuItem;
        System.Windows.Forms.MenuItem EditAccessory_MenuItem;
        System.Windows.Forms.MenuItem DeleteAccessory_MenuItem;

        MenuItem OpenDiagnosticOPR_MenuItem;
        MenuItem AddDiagnosticOPR_MenuItem;
        MenuItem UpdateDiagnosticOPR_MenuItem;
        MenuItem DeleteDiagnosticOPR_MenuItem;

        MenuItem OpenFault_MenuItem;
        MenuItem AddFault_MenuItem;
        MenuItem UpdateFault_MenuItem;
        MenuItem DeleteFault_MenuItem;

        MenuItem Open_MissedFault_Item_MenuItem;
        //MenuItem Add_MissedFault_Item_MenuItem;
        MenuItem Update_MissedFault_Item_MenuItem;
        MenuItem Delete_MissedFault_Item_MenuItem;

        DatabaseInterface DB;
        MaintenanceOPR _MaintenanceOPR;
        BillMaintenance _BillMaintenance;
        private Contact _Contact;
        Item _Item;
        TradeStorePlace _Place;
        Folder LastUsedFolder;
        DiagnosticOPRSQL DiagnosticOPRSQL_;
        List<DiagnosticOPRReport> DiagnosticOPRList = new List<DiagnosticOPRReport>();
        List<MaintenanceFaultReport > FaultReportList = new List<MaintenanceFaultReport>();
        List<Missed_Fault_Item> Missed_Fault_Item_List = new List<Missed_Fault_Item>();

        List<ItemOUT> _ItemOUTList = new List<ItemOUT>();
        List<MaintenanceOPR_Accessory> AccessoryList = new List<MaintenanceOPR_Accessory>();

        private bool Changed_;
        public bool Changed
        {
            get { return Changed_; }
        }
       
        public MaintenanceOPRForm(DatabaseInterface db,Contact Contact_)
        {
            DB = db;
            DiagnosticOPRSQL_ = new DiagnosticOPRSQL(DB);
            _Contact = Contact_;
            
             InitializeComponent();

            buttonBill_Create .Enabled  = false;
            buttonBill_Edit.Visible = false;
            buttonBill_Delete.Visible = false;
            buttonEndWork_Create.Enabled  = false;
            buttonEndWork_Edit.Visible = false;
            buttonEndWork_Delete.Visible = false;
            panelBIll.Enabled = false;
            panelEndWork.Enabled = false;
            tabControlData.Enabled = false ;
            if (_Contact!=null)
                textBoxContact.Text = _Contact.Get_Complete_ContactName_WithHeader();
            textBoxMaintenanceOPRID.Text = "-";
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انشاء";
            //LoadBillData();
            this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
            this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
            textBoxContact.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxContact_MouseDoubleClick);
            this.textBoxPlaceID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            this.textBoxPlaceID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxPlace_MouseDoubleClick);

            InitializeMenuItems();
            DiagnosticOPRReport.InitializeDiagnosticOPRListViewColumns(ref listViewSubDiagnosticOPR);
            MaintenanceFaultReport.InitializeFaultReportListViewColumns(ref listViewFault);
            Missed_Fault_Item.InitializeMissed_Fault_ListViewColumns(ref listViewMissedFaultItem);
            //AdjustListViewItemsOUTColumnsWidth();
            //panelFaults.Enabled = false;
            //panelDiagnosticOPR.Enabled = false;
        }
   
        public MaintenanceOPRForm(DatabaseInterface db, MaintenanceOPR MaintenanceOPR_, bool Edit)
        {
            DB = db;
            DiagnosticOPRSQL_ = new DiagnosticOPRSQL(DB);
            InitializeComponent();
            _MaintenanceOPR = MaintenanceOPR_;

            //AdjustListViewItemsOUTColumnsWidth();

            InitializeMenuItems();
            LoadForm(Edit);
            DiagnosticOPRReport.InitializeDiagnosticOPRListViewColumns (ref listViewSubDiagnosticOPR);
            MaintenanceFaultReport.InitializeFaultReportListViewColumns(ref listViewFault);
            Missed_Fault_Item.InitializeMissed_Fault_ListViewColumns(ref listViewMissedFaultItem);

        }
        public void LoadForm(bool Edit)
        {
            try
            {
                if (_MaintenanceOPR == null) return;
                _Contact = _MaintenanceOPR._Contact;
                _Item = _MaintenanceOPR._Item;
                _Place = _MaintenanceOPR.Place;
                _BillMaintenance = new BillMaintenanceSQL(DB).GetBillMaintenance_By_MaintenaceOPR(_MaintenanceOPR);
                textBoxContact.Text = _Contact.Get_Complete_ContactName_WithHeader();

                tabControlData.Enabled = true ;
                panelBIll.Enabled = true ;
                panelEndWork.Enabled = true ;
                LoadItemData();
                LoadBillData(Edit );
                LoadEndWorkData(Edit);
                GetSubData();
            buttonSave.Name = "buttonSave";
                buttonSave.Text = "حفظ";
               
                dateTimePickerEntryDate.Value = _MaintenanceOPR.EntryDate;
                textBoxMaintenanceOPRID.Text = _MaintenanceOPR._Operation.OperationID.ToString();
                textBoxItemSerial.Text = _MaintenanceOPR.ItemSerial;
                textBoxFaultDesc.Text = _MaintenanceOPR.FaultDesc;
                textBoxNotes.Text = _MaintenanceOPR.Notes;
                

                if (_MaintenanceOPR.Place != null)
                {

                    textBoxPlaceID.Text = _MaintenanceOPR.Place.PlaceID.ToString();
                    textBoxPlaceID.Name = _MaintenanceOPR.Place.PlaceID.ToString();
                    textBoxPlaceInfo.Text = _MaintenanceOPR.Place.GetPlaceInfo();
                }


                if (Edit)
                {
                    if (_Place != null)
                        buttonClearstoreinfo.Visible = true;
                    else
                        buttonClearstoreinfo.Visible = false;
                    //this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
                    this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
                    this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
                    this.textBoxPlaceID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
                    this.textBoxPlaceID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxPlace_MouseDoubleClick);
                    //this.listViewItemOUT.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewItemOUT_MouseDoubleClick);
                    //this.listViewItemOUT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewItemOUT_MouseDown);
                    this.listViewSubDiagnosticOPR.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewDiagnosticOPR_MouseDoubleClick);
                    this.listViewSubDiagnosticOPR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewDiagnosticOPR_MouseDown);
                    this.listViewFault.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewFault_MouseDoubleClick);
                    this.listViewFault.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewFault_MouseDown);

                    this.listViewMissedFaultItem.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MissedFault_Item_MouseDoubleClick);
                    this.listViewMissedFaultItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView_MissedFault_Item_MouseDown);


                }
                else
                {
             
                    buttonSave.Visible = false;
                    dateTimePickerEntryDate.Enabled = false;
                    textBoxPlaceID.ReadOnly = true;
                    textBoxItemID.ReadOnly = true;
                    textBoxItemSerial.ReadOnly = true;
                    textBoxFaultDesc.ReadOnly = true;
                    textBoxNotes.ReadOnly = true;
                    //comboBoxStatus.Enabled = false;
                    //checkBox1.Enabled = false;
                    //dateTimePickerDeliver.Enabled = false;

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(":حدث خطأ " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
        public void InitializeMenuItems()
        {
            //OpenItemOUTMenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية التركيب", OpenItemOUT_MenuItem_Click);
            //AddItemOUTMenuItem = new System.Windows.Forms.MenuItem("تركيب عنصر", AddItemOUT_MenuItem_Click);
            //EditItemOUTMenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditItemOUT_MenuItem_Click);
            //DeleteItemOUTMenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteItemOUT_MenuItem_Click);

            OpenAccessory_MenuItem = new System.Windows.Forms.MenuItem("عرض تفاصيل", OpenAccessory_MenuItem_Click);
            AddAccessory_MenuItem = new System.Windows.Forms.MenuItem("اضافة ملحق صيانة", AddAccessory_MenuItem_Click);
            EditAccessory_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditAccessory_MenuItem_Click);
            DeleteAccessory_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteAccessory_MenuItem_Click);

            OpenDiagnosticOPR_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية الفحص", OpenDiagnosticOPR_MenuItem_Click);
            AddDiagnosticOPR_MenuItem = new System.Windows.Forms.MenuItem("اضافة عملية فحص فرعية", AddDiagnosticOPR_MenuItem_Click);
            UpdateDiagnosticOPR_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditDiagnosticOPR_MenuItem_Click);
            DeleteDiagnosticOPR_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteDiagnosticOPR_MenuItem_Click);

            OpenFault_MenuItem = new System.Windows.Forms.MenuItem("عرض تفاصيل", OpenFault_MenuItem_Click);
            AddFault_MenuItem = new System.Windows.Forms.MenuItem("اضافة عطل", AddFault_MenuItem_Click);
            UpdateFault_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditFault_MenuItem_Click);
            DeleteFault_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteFault_MenuItem_Click);

            Open_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("عرض تفاصيل", Open_MissedFault_Item_MenuItem_Click);
            //Add_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("اضافة عنصر مفقود او تالف", Add_MissedFault_Item_MenuItem_Click);
            Update_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("تعديل ", Edit_MissedFault_Item_MenuItem_Click);
            Delete_MissedFault_Item_MenuItem = new System.Windows.Forms.MenuItem("حذف", Delete_MissedFault_Item_MenuItem_Click);

        }
        private void textBoxContact_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Trade.Forms.TradeContact.ShowContactsForm form = new Trade.Forms.TradeContact.ShowContactsForm(DB, true);
                DialogResult dd = form.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    _Contact = form.Contact_;
                    textBoxContact.Text = _Contact.GetContactTypeHeader() + ":" + _Contact.ContactName;
                }
            }
        }
     
        public async void GetSubData()
        {
            _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_MaintenanceOPR._Operation);
            AccessoryList = new MaintenanceAccessorySQL(DB).GetMaintenanceOPR_Accessories_List(_MaintenanceOPR);
            DiagnosticOPRList = new DiagnosticOPRSQL(DB).GetSubDiagnosticOPRReportList(_MaintenanceOPR, null);
            FaultReportList = new MaintenanceFaultSQL(DB).GetMaintenanceOPR_Report_Fault_List(_MaintenanceOPR);
            Missed_Fault_Item_List =new MissedFaultItemSQL  (DB).MaintenanceOPR_GetMissed_Fault_Item_List (_MaintenanceOPR);

            RefreshAccessories(AccessoryList);
            DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, DiagnosticOPRList);
            MaintenanceFaultReport.RefreshFaultReportList(ref listViewFault, FaultReportList);
            Missed_Fault_Item.RefreshMissed_FaultList(ref listViewMissedFaultItem, Missed_Fault_Item_List);
            //RefreshInstalledItems(_ItemOUTList);
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {

            if (_Item == null)
            {

                MessageBox.Show("يرجى تحديد المادة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_Item == null)
            {

                MessageBox.Show("يرجى تحديد الجهة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (buttonSave.Name == "buttonAdd")
            {
                try
                {
                   
                    MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).AddMaintenanceOPR
                        (dateTimePickerEntryDate.Value, _Contact.ContactID, _Item.ItemID, textBoxItemSerial.Text
                        , textBoxFaultDesc.Text, textBoxNotes.Text, _Place);
                    if (MaintenanceOPR_ != null)
                    {
                        _MaintenanceOPR = MaintenanceOPR_;
                        MessageBox.Show("تم الاضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Changed_ = true;
                        LoadForm(true );

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
                    if (_MaintenanceOPR  != null)
                    {
                        
                        bool success = new MaintenanceOPRSQL(DB).UpdateMaintenanceOPR
                            (_MaintenanceOPR._Operation.OperationID,dateTimePickerEntryDate.Value, _Contact.ContactID , _Item.ItemID
                            , textBoxItemSerial.Text
                        , textBoxFaultDesc.Text, textBoxNotes.Text, _Place);
                        if (success == true)
                        {
                            MessageBox.Show("تم حفظ  بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _MaintenanceOPR = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(_MaintenanceOPR._Operation.OperationID);
                            this.Changed_ = true;
                            LoadForm(true );
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
                    Item item__ = new ItemSQL(DB).GetItemInfoByID(itemid);
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
        private async  void LoadItemData()
        {
            LastUsedFolder = _Item.folder;
            textBoxItemID.Text = _Item.ItemID.ToString();
            textBoxItemName.Text = _Item.ItemName;
            textBoxItemCompany.Text = _Item.ItemCompany;
            textBoxItemType.Text = _Item.folder.FolderName;

        }
        //#region ItemOUT
        //private void listViewItemOUT_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left && listViewItemOUT.SelectedItems.Count > 0)
        //    {
        //        OpenItemOUTMenuItem.PerformClick();
        //    }
        //}
        //private void listViewItemOUT_MouseDown(object sender, MouseEventArgs e)
        //{
        //    listViewItemOUT.ContextMenu = null;
        //    bool match = false;
        //    ListViewItem listitem = new ListViewItem();
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    {
        //        foreach (ListViewItem item1 in listViewItemOUT.Items)
        //        {
        //            if (item1.Bounds.Contains(new Point(e.X, e.Y)))
        //            {
        //                match = true;
        //                listitem = item1;
        //                break;
        //            }
        //        }
        //        if (match)
        //        {


        //            MenuItem[] mi1 = new MenuItem[] { OpenItemOUTMenuItem, EditItemOUTMenuItem, DeleteItemOUTMenuItem, new MenuItem("-"), AddItemOUTMenuItem };
        //            listViewItemOUT.ContextMenu = new ContextMenu(mi1);


        //        }
        //        else
        //        {

        //            MenuItem[] mi = new MenuItem[] { AddItemOUTMenuItem };
        //            listViewItemOUT.ContextMenu = new ContextMenu(mi);

        //        }

        //    }

        //}
        //private void listViewItemsOUT_Resize(object sender, EventArgs e)
        //{
        //    AdjustListViewItemsOUTColumnsWidth();
        //}
        //public async  void AdjustListViewItemsOUTColumnsWidth()
        //{
        //    listViewItemOUT.Columns[0].Width = 80;
        //    int w= (listViewItemOUT.Width - 80) / 8 - 1;
        //    listViewItemOUT.Columns[1].Width = w;
        //    listViewItemOUT.Columns[2].Width = w;
        //    listViewItemOUT.Columns[3].Width = w;
        //    listViewItemOUT.Columns[4].Width = w;
        //    listViewItemOUT.Columns[5].Width = w;
        //    listViewItemOUT.Columns[6].Width = w;
        //    listViewItemOUT.Columns[7].Width = w;
        //    listViewItemOUT.Columns[8].Width = w;


        //}
        //private void DeleteItemOUT_MenuItem_Click(object sender, EventArgs e)
        //{

        //    try
        //    {

        //        DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        //        if (dd != DialogResult.OK) return;
        //        uint sid = Convert.ToUInt32(listViewItemOUT.SelectedItems[0].Name);
        //        bool success = new ItemOUTSQL(DB).DeleteItemOUT(sid);
        //        if (success)
        //        {
        //            MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_MaintenanceOPR ._Operation);
        //            RefreshInstalledItems(_ItemOUTList);

        //        }
        //        else
        //        {
        //            MessageBox.Show("فشل الحذف", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //        }
        //    }
        //    catch
        //    {

        //    }
        //}
        //private void EditItemOUT_MenuItem_Click(object sender, EventArgs e)
        //{
        //    if (listViewItemOUT.SelectedItems.Count > 0)
        //    {
        //        uint itemoutid = Convert.ToUInt32(listViewItemOUT.SelectedItems[0].Name);
        //        ItemOUT ItemOUT_ = new ItemOUTSQL(DB).GetItemOUTINFO_BYID(itemoutid);
        //        ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, ItemOUT_, true);
        //        ItemOUTForm_.ShowDialog();
        //        if (ItemOUTForm_.Changed)
        //        {
        //            _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_MaintenanceOPR._Operation);
        //            RefreshInstalledItems(_ItemOUTList);
        //        }
        //        ItemOUTForm_.Dispose();
        //    }
        //}
        //private void OpenItemOUT_MenuItem_Click(object sender, EventArgs e)
        //{
        //    if (listViewItemOUT.SelectedItems.Count > 0)
        //    {

        //        uint itemoutid = Convert.ToUInt32(listViewItemOUT.SelectedItems[0].Name);
        //        ItemOUT ItemOUT_ = new ItemOUTSQL(DB).GetItemOUTINFO_BYID(itemoutid);
        //        ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, ItemOUT_, false);
        //        ItemOUTForm_.ShowDialog();
        //        if (ItemOUTForm_.Changed)
        //        {
        //            _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_MaintenanceOPR._Operation);
        //            RefreshInstalledItems(_ItemOUTList);
        //        }
        //        ItemOUTForm_.Dispose();
        //    }
        //}
        //private async void RefreshInstalledItems(List<ItemOUT> ItemOUTList)
        //{

        //    listViewItemOUT.Items.Clear();
        //    double totalcost = 0;
        //    for (int i = 0; i < ItemOUTList.Count; i++)
        //    {
        //        double sellprice = ItemOUTList[i]._OUTValue .Value ;
        //        double total_sellprice = System.Math.Round(sellprice * ItemOUTList[i].Amount, 2);
        //        totalcost = totalcost + total_sellprice;
        //        ListViewItem ListViewItem_ = new ListViewItem((listViewItemOUT.Items.Count + 1).ToString());
        //        ListViewItem_.Name = ItemOUTList[i].ItemOUTID.ToString();
        //        ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.ItemName);
        //        ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.ItemCompany);
        //        ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.folder.FolderName);
        //        ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._TradeState.TradeStateName);
        //        ListViewItem_.SubItems.Add(ItemOUTList[i].Amount.ToString());
        //        ListViewItem_.SubItems.Add(ItemOUTList[i]._ConsumeUnit.ConsumeUnitName.ToString());


        //        ListViewItem_.SubItems.Add(sellprice.ToString() + " " + _BillMaintenance._Currency.CurrencySymbol.Replace(" ", string.Empty));
        //        ListViewItem_.SubItems.Add((total_sellprice).ToString() + " " + _BillMaintenance._Currency.CurrencySymbol.Replace(" ", string.Empty));
        //        ListViewItem_.SubItems.Add(ItemOUTList[i].Notes);
        //        ListViewItem_.BackColor = Color.Orange;
        //        listViewItemOUT.Items.Add(ListViewItem_);

        //    }


        //}

        //private void AddItemOUT_MenuItem_Click(object sender, EventArgs e)
        //{
        //    ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, _MaintenanceOPR._Operation);
        //    DialogResult d = ItemOUTForm_.ShowDialog();
        //    if (ItemOUTForm_.Changed)
        //    {
        //        _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_MaintenanceOPR._Operation);
        //        RefreshInstalledItems(_ItemOUTList);
        //    }
        //    ItemOUTForm_.Dispose();
        //}
        //#endregion
        #region AccessoryRegion
        private void DeleteAccessory_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                if (listViewAccessories.SelectedItems.Count == 0) return;
                    DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewAccessories .SelectedItems[0].Name);
                bool success = new MaintenanceAccessorySQL(DB).DeleteAccessory (sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AccessoryList = new MaintenanceAccessorySQL(DB).GetMaintenanceOPR_Accessories_List(_MaintenanceOPR);
                    RefreshAccessories(AccessoryList);

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
        private void EditAccessory_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewAccessories.SelectedItems.Count > 0)
            {
                uint accessoryid = Convert.ToUInt32(listViewAccessories.SelectedItems[0].Name);
                MaintenanceOPR_Accessory accessory = new MaintenanceAccessorySQL(DB).Get_Accessory_INFO_BYID(accessoryid);
                MaintenanceAccessoryForm MaintenanceAccessoryForm_ = new MaintenanceAccessoryForm(DB, accessory, true );
                MaintenanceAccessoryForm_.ShowDialog();
                if (MaintenanceAccessoryForm_.Changed)
                {
                    AccessoryList = new MaintenanceAccessorySQL(DB).GetMaintenanceOPR_Accessories_List(_MaintenanceOPR);
                    RefreshAccessories(AccessoryList);
                }
                MaintenanceAccessoryForm_.Dispose();
            }
        }
        private void OpenAccessory_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewAccessories.SelectedItems.Count > 0)
            {

                uint accessoryid = Convert.ToUInt32(listViewAccessories .SelectedItems[0].Name);
                MaintenanceOPR_Accessory  accessory = new MaintenanceAccessorySQL(DB).Get_Accessory_INFO_BYID(accessoryid);
                MaintenanceAccessoryForm MaintenanceAccessoryForm_ = new MaintenanceAccessoryForm(DB, accessory, false);
                MaintenanceAccessoryForm_.ShowDialog();
                if (MaintenanceAccessoryForm_.Changed)
                {
                    AccessoryList = new MaintenanceAccessorySQL(DB).GetMaintenanceOPR_Accessories_List(_MaintenanceOPR);
                    RefreshAccessories(AccessoryList);
                }
                MaintenanceAccessoryForm_.Dispose();
            }
        }
        private void AddAccessory_MenuItem_Click(object sender, EventArgs e)
        {
            MaintenanceAccessoryForm MaintenanceAccessoryForm_ = new MaintenanceAccessoryForm(DB, _MaintenanceOPR);
            DialogResult d = MaintenanceAccessoryForm_.ShowDialog();
            if (MaintenanceAccessoryForm_.Changed)
            {
                AccessoryList = new MaintenanceAccessorySQL(DB).GetMaintenanceOPR_Accessories_List(_MaintenanceOPR);
                RefreshAccessories(AccessoryList);
            }
            MaintenanceAccessoryForm_.Dispose();
        }
        private async void RefreshAccessories(List<MaintenanceOPR_Accessory> AccessoriesList_)
        {

            listViewAccessories .Items.Clear();
            double totalcost = 0;
            for (int i = 0; i < AccessoriesList_.Count; i++)
            {
                ListViewItem ListViewItem_ = new ListViewItem((listViewAccessories.Items.Count + 1).ToString());
                ListViewItem_.Name = AccessoriesList_[i].AccessoryID.ToString();
                ListViewItem_.SubItems.Add(AccessoriesList_[i]._Item.ItemName);
                ListViewItem_.SubItems.Add(AccessoriesList_[i]._Item.ItemCompany);
                ListViewItem_.SubItems.Add(AccessoriesList_[i]._Item.folder.FolderName);
                ListViewItem_.SubItems.Add(AccessoriesList_[i].ItemSerialNumber );
                if(AccessoriesList_[i].Place ==null )
                     ListViewItem_.SubItems.Add("");
                else
                     ListViewItem_.SubItems.Add(AccessoriesList_[i].Place .GetPlaceInfo());

                ListViewItem_.BackColor = Color.LimeGreen ;
                listViewAccessories.Items.Add(ListViewItem_);

            }


        }
        private void listViewAccessories_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewAccessories .SelectedItems.Count > 0)
            {
                OpenAccessory_MenuItem.PerformClick();
            }
        }
        private void listViewAccessories_MouseDown(object sender, MouseEventArgs e)
        {
            listViewAccessories.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewAccessories.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenAccessory_MenuItem, EditAccessory_MenuItem, DeleteAccessory_MenuItem, new MenuItem("-"), AddAccessory_MenuItem };
                    listViewAccessories.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddAccessory_MenuItem };
                    listViewAccessories.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        #endregion

        
     
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                try
                {
                    uint placeid = Convert.ToUInt32(textBoxPlaceID.Text);
                    TradeStorePlace place = new Trade.TradeSQL.TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(placeid);
                    if (place != null)
                    {
                        buttonClearstoreinfo.Visible = true;
                        _Place = place;
                        textBoxPlaceInfo.Text = _Place.GetPlaceInfo();
                        textBoxPlaceID.Text = _Place.PlaceID.ToString();
                        textBoxPlaceID.Name = _Place.PlaceID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على مكان التخزين", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    MessageBox.Show("يرجى ادخال عدد صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }
        private void textBoxPlace_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TradeStoreContainer container;
            try
            {
                container = new Trade.TradeSQL.TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(textBoxPlaceID.Name))._TradeStoreContainer;
            }
            catch
            {
                container = null;
            }

            Trade.Forms.Container.ShowLocations frm = new Trade.Forms.Container.ShowLocations(DB, container, true);
            DialogResult dd = frm.ShowDialog();
            if (dd == DialogResult.OK)
            {
                TradeStorePlace place = frm.ReturnPlace;
                buttonClearstoreinfo.Visible = true;
                _Place = place;
                textBoxPlaceInfo.Text = _Place.GetPlaceInfo();
                textBoxPlaceID.Text = _Place.PlaceID.ToString();
                textBoxPlaceID.Name = _Place.PlaceID.ToString();

            }
            frm.Dispose();
        }
        //private void buttonStore_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (buttonStore.Name == "buttonStore")
        //        {
        //            TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(textBoxPlaceID.Name));
        //            if (place == null)
        //            {
        //                MessageBox.Show("يرجى تحديد مكان التخزين", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }
        //            bool success = new TradeItemStoreSQL(DB).Store_Item_INPlace(place .PlaceID , _MaintenanceOPR ._Operation .OperationID ,TradeItemStore.MAINTENANCE_ITEM_STORE_TYPE, 1, null);
        //            if (success)
        //            {

        //                LoadStoreData();

        //            }
        //        }
        //        else
        //        {
        //            TradeStorePlace place = new TradeItemStoreSQL(DB).GetMaintenanceStorePlace(_MaintenanceOPR ._Operation .OperationID );
        //            bool success = new TradeItemStoreSQL(DB).UNStore_Item_INPlace(place.PlaceID  , _MaintenanceOPR._Operation.OperationID, TradeItemStore.MAINTENANCE_ITEM_STORE_TYPE);
        //            if (success)
        //            {
        //                LoadStoreData();

        //            }
        //        }


        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show("ضبط التخزين:" + ee.Message);
        //    }


        //}
        private void buttonClearstoreinfo_Click(object sender, EventArgs e)
        {
            buttonClearstoreinfo.Visible = false;
            _Place = null;
            textBoxPlaceInfo.Text = "";
            textBoxPlaceID.Text = "";
        }
     
       
        #region SubDiagnosticOPR
        private void DeleteDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewSubDiagnosticOPR.SelectedItems[0].Name);
                bool success = DiagnosticOPRSQL_.DeleteDiagnosticOPR(sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DiagnosticOPRList = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR, null );
                    DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, DiagnosticOPRList);

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
        private void EditDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewSubDiagnosticOPR.SelectedItems.Count > 0)
            {
                uint itemoutid = Convert.ToUInt32(listViewSubDiagnosticOPR.SelectedItems[0].Name);
                DiagnosticOPR DiagnosticOPR_ = DiagnosticOPRSQL_.GetDiagnosticOPRINFO_BYID(itemoutid);
                DiagnosticOPRForm DiagnosticOPRForm_ = new DiagnosticOPRForm(DB, DiagnosticOPR_, true);
                DiagnosticOPRForm_.ShowDialog();
                if (DiagnosticOPRForm_.Changed)
                {
                    DiagnosticOPRList = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR, null );
                    DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, DiagnosticOPRList);
                }
                DiagnosticOPRForm_.Dispose();
            }
        }
        private void OpenDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {

            if (listViewSubDiagnosticOPR.SelectedItems.Count > 0)
            {
                uint itemoutid = Convert.ToUInt32(listViewSubDiagnosticOPR.SelectedItems[0].Name);
                DiagnosticOPR DiagnosticOPR_ = DiagnosticOPRSQL_.GetDiagnosticOPRINFO_BYID(itemoutid);
                DiagnosticOPRForm DiagnosticOPRForm_ = new DiagnosticOPRForm(DB, DiagnosticOPR_, false);
                DiagnosticOPRForm_.ShowDialog();
                if (DiagnosticOPRForm_.Changed)
                {
                    DiagnosticOPRList = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR, null );
                    DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, DiagnosticOPRList);
                }
                DiagnosticOPRForm_.Dispose();
            }
        }
        private void AddDiagnosticOPR_MenuItem_Click(object sender, EventArgs e)
        {
            DiagnosticOPRForm DiagnosticOPRForm_ = new DiagnosticOPRForm(DB, _MaintenanceOPR, null );
            DialogResult d = DiagnosticOPRForm_.ShowDialog();
            if (DiagnosticOPRForm_.Changed)
            {
                DiagnosticOPRList = DiagnosticOPRSQL_.GetSubDiagnosticOPRReportList(_MaintenanceOPR, null );
                DiagnosticOPRReport.RefreshSubDiagnosticOPRList(ref listViewSubDiagnosticOPR, DiagnosticOPRList);
            }
            DiagnosticOPRForm_.Dispose();
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
        private void listViewDiagnosticOPR_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewSubDiagnosticOPR.SelectedItems.Count > 0)
            {
                OpenDiagnosticOPR_MenuItem.PerformClick();
            }
        }
        private void listViewDiagnosticOPR_MouseDown(object sender, MouseEventArgs e)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenDiagnosticOPR_MenuItem
                        , UpdateDiagnosticOPR_MenuItem, DeleteDiagnosticOPR_MenuItem, new MenuItem("-"), AddDiagnosticOPR_MenuItem };
                    listViewSubDiagnosticOPR.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddDiagnosticOPR_MenuItem };
                    listViewSubDiagnosticOPR.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        private void listViewSubDiagnosticOPR_Resize(object sender, EventArgs e)
        {
            DiagnosticOPRReport.AdjustlistViewDiagnosticOPRColumnsWidth(ref listViewSubDiagnosticOPR );
        }

        #endregion

        #region Fault
        private void DeleteFault_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من حذف العطل و جميع العمليات التابعة له؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewFault.SelectedItems[0].Name);
                bool success = new MaintenanceFaultSQL(DB).DeleteFault (sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FaultReportList = new MaintenanceFaultSQL(DB).GetMaintenanceOPR_Report_Fault_List(_MaintenanceOPR );
                    MaintenanceFaultReport.RefreshFaultReportList(ref listViewFault, FaultReportList);

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
        private void EditFault_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFault.SelectedItems.Count > 0)
            {
                uint faultid = Convert.ToUInt32(listViewFault .SelectedItems[0].Name);
                MaintenanceFault MaintenanceFault_ = new MaintenanceFaultSQL (DB).Get_Fault_INFO_BYID(faultid);
                FaultForm FaultForm_ = new FaultForm(DB, MaintenanceFault_, true);
                FaultForm_.ShowDialog();
                if (FaultForm_.Changed)
                {
                    FaultReportList = new MaintenanceFaultSQL(DB).GetMaintenanceOPR_Report_Fault_List(_MaintenanceOPR);
                    MaintenanceFaultReport.RefreshFaultReportList(ref listViewFault, FaultReportList);
                }
                FaultForm_.Dispose();
            }
        }
        private void OpenFault_MenuItem_Click(object sender, EventArgs e)
        {

            if (listViewFault.SelectedItems.Count > 0)
            {
                uint faultid = Convert.ToUInt32(listViewFault.SelectedItems[0].Name);
                MaintenanceFault MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(faultid);
                FaultForm FaultForm_ = new FaultForm(DB, MaintenanceFault_, false );
                FaultForm_.ShowDialog();
                if (FaultForm_.Changed)
                {
                    FaultReportList = new MaintenanceFaultSQL(DB).GetMaintenanceOPR_Report_Fault_List(_MaintenanceOPR);
                    MaintenanceFaultReport.RefreshFaultReportList(ref listViewFault, FaultReportList);
                }
                FaultForm_.Dispose();
            }
        }
        private void AddFault_MenuItem_Click(object sender, EventArgs e)
        {
            FaultForm FaultForm_ = new FaultForm(DB, _MaintenanceOPR);
            DialogResult d = FaultForm_.ShowDialog();
            if (FaultForm_.Changed)
            {
                FaultReportList = new MaintenanceFaultSQL(DB).GetMaintenanceOPR_Report_Fault_List(_MaintenanceOPR);
                MaintenanceFaultReport.RefreshFaultReportList(ref listViewFault , FaultReportList);
            }
            FaultForm_.Dispose();
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
        private void listViewFault_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewFault.SelectedItems.Count > 0)
            {
                OpenFault_MenuItem .PerformClick();
            }
        }
        private void listViewFault_MouseDown(object sender, MouseEventArgs e)
        {
            listViewFault .ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewFault.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenFault_MenuItem 
                        , UpdateFault_MenuItem, DeleteFault_MenuItem, new MenuItem("-"), AddFault_MenuItem };
                    listViewFault.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddFault_MenuItem };
                    listViewFault.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        private void listViewFault_Resize(object sender, EventArgs e)
        {
            //MaintenanceFaultReport .AdjustlistViewFaultReportOPRColumnsWidth (ref listViewSubDiagnosticOPR);
            AdjustlistViewFaultReportOPRColumnsWidth();
        }
        public async  void AdjustlistViewFaultReportOPRColumnsWidth()
        {
            try
            {
                listViewFault .Columns[0].Width = 60;
                listViewFault.Columns[1].Width = 100;
                listViewFault.Columns[2].Width = listViewFault.Width - 1010;
                listViewFault.Columns[3].Width = 110;
                listViewFault.Columns[4].Width = 110;
                listViewFault.Columns[5].Width = 110;
                listViewFault.Columns[6].Width = 130;
                listViewFault.Columns[7].Width = 190;
                listViewFault.Columns[8].Width = 190;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("AdjustlistViewFaultReportOPRColumnsWidth" + Environment.NewLine + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                bool success = new MissedFaultItemSQL (DB).DeleteMissed_Fault_Item (sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List  (_MaintenanceOPR);
                    RefreshMissed_FaultList(Missed_Fault_Item_List);
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
                Missed_Fault_Item Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID(id );
                MissedFault_Item_Form MissedFault_Item_Form_ = new MissedFault_Item_Form(DB, Missed_Fault_Item_, true);
                MissedFault_Item_Form_.ShowDialog();
                if (MissedFault_Item_Form_.Changed)
                {
                    Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
                    RefreshMissed_FaultList(Missed_Fault_Item_List);
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
                MissedFault_Item_Form MissedFault_Item_Form_ = new MissedFault_Item_Form(DB, Missed_Fault_Item_, false );
                MissedFault_Item_Form_.ShowDialog();
                if (MissedFault_Item_Form_.Changed)
                {
                    Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
                   RefreshMissed_FaultList( Missed_Fault_Item_List);
                }
                MissedFault_Item_Form_.Dispose();
            }
        }
        internal async void RefreshMissed_FaultList(List<Missed_Fault_Item> Missed_Fault_ItemList)
        {
            listViewMissedFaultItem.Items.Clear();
            for (int i = 0; i < Missed_Fault_ItemList.Count; i++)
            {

                System.Windows.Forms.ListViewItem ListViewItem_ = new System.Windows.Forms.ListViewItem(Missed_Fault_ItemList[i].ID.ToString());
                ListViewItem_.Name = Missed_Fault_ItemList[i].ID.ToString();
                if (Missed_Fault_ItemList[i].Type == Missed_Fault_Item.FAULT_ITEM)
                {
                    ListViewItem_.SubItems.Add("تالف");
                    ListViewItem_.BackColor = System.Drawing.Color.SandyBrown;
                }
                else
                {
                    ListViewItem_.SubItems.Add("مفقود");
                    ListViewItem_.BackColor = System.Drawing.Color.PeachPuff;
                }
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i]._Item.folder.FolderName);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i]._Item.ItemName);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i]._Item.ItemCompany);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i].Location);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i].Notes);
                ListViewItem_.SubItems.Add(Missed_Fault_ItemList[i].TagsCount.ToString());

                listViewMissedFaultItem.Items.Add(ListViewItem_);

            }


        }
        //private void Add_MissedFault_Item_MenuItem_Click(object sender, EventArgs e)
        //{
        //    MissedFault_Item_Form MissedFault_Item_Form_ = new MissedFault_Item_Form(DB, _MaintenanceOPR);
        //    DialogResult d = MissedFault_Item_Form_.ShowDialog();
        //    if (MissedFault_Item_Form_.Changed)
        //    {
        //        Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
        //        Missed_Fault_Item.RefreshMissed_FaultList(ref listViewMissedFaultItem, Missed_Fault_Item_List);
        //    }
        //    MissedFault_Item_Form_.Dispose();
        //}
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
        private void listView_MissedFault_Item_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewMissedFaultItem.SelectedItems.Count > 0)
            {
                Open_MissedFault_Item_MenuItem .PerformClick();
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
                        , Update_MissedFault_Item_MenuItem, Delete_MissedFault_Item_MenuItem };
                    listViewMissedFaultItem.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] {  };
                    listViewMissedFaultItem.ContextMenu = null ;

                }

            }

        }
        private void listView_MissedFault_Item_Resize(object sender, EventArgs e)
        {
            //MaintenanceFaultReport .AdjustlistViewFaultReportOPRColumnsWidth (ref listViewSubDiagnosticOPR);
            AdjustlistView_MissedFault_ItemOPRColumnsWidth( listViewMissedFaultItem);
        }
        public async void AdjustlistView_MissedFault_ItemOPRColumnsWidth( ListView listview)
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

        private void asdToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            SelecObjectForm ff = new SelecObjectForm("hojv");
            Missed_Fault_Item_List = new MissedFaultItemSQL(DB).MaintenanceOPR_GetMissed_Fault_Item_List(_MaintenanceOPR);
            Missed_Fault_Item.InitializeMissed_Fault_ListViewColumns(ref ff._listView);
            Missed_Fault_Item.RefreshMissed_FaultList(ref ff._listView  , Missed_Fault_Item_List);
            ff.adjustcolumns =f=> AdjustlistView_MissedFault_ItemOPRColumnsWidth( ff._listView );
            ff.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region BillMaintenance
        private void buttonBill_Create_Click(object sender, EventArgs e)
        {
            if (_BillMaintenance == null)
            {
                BillMaintenanceForm BillMaintenanceForm_ = new BillMaintenanceForm(DB, DateTime.Now, _MaintenanceOPR);
                BillMaintenanceForm_.ShowDialog();
                if (BillMaintenanceForm_.Changed)
                {
                    _BillMaintenance = new BillMaintenanceSQL(DB).GetBillMaintenance_By_MaintenaceOPR(_MaintenanceOPR);
                    LoadBillData(true);
                }
            }
            else
            {
                BillMaintenanceForm BillMaintenanceForm_ = new BillMaintenanceForm(DB, _BillMaintenance, false);
                BillMaintenanceForm_.ShowDialog();
                if (BillMaintenanceForm_.Changed)
                {
                    _BillMaintenance = new BillMaintenanceSQL(DB).GetBillMaintenance_By_MaintenaceOPR(_MaintenanceOPR);
                    LoadBillData(true);
                }
            }
        }
        private async void LoadBillData(bool Edit)
        {
            if (_BillMaintenance == null)
            {
                buttonBill_Create.Enabled = true;
                buttonBill_Create.Text = "انشاء فاتورة";
                buttonBill_Edit.Visible = false;
                buttonBill_Delete.Visible = false;
                textBoxBillCurrency.Text = "-";
                textBoxBillExchangeRte.Text = "-";
                textBoxBillValue.Text = "-";

            }
            else
            {
                textBoxBillCurrency.Text = _BillMaintenance._Currency.CurrencyName;
                textBoxBillExchangeRte.Text = _BillMaintenance.ExchangeRate.ToString();
                textBoxBillValue.Text = new OperationSQL(DB).Get_OperationValue(_BillMaintenance._Operation.OperationType, _BillMaintenance._Operation.OperationID).ToString()
                   +" "+ _BillMaintenance._Currency.CurrencySymbol;
                buttonBill_Create.Enabled = true;
                buttonBill_Create.Text = "استعراض";
                buttonBill_Edit.Visible = true;
                buttonBill_Delete.Visible = true;
            }
            if (!Edit)
            {
                buttonBill_Create.Visible = false;
                buttonBill_Edit.Visible = false;
                buttonBill_Delete.Visible = false;
            }

        }
     

        private void buttonBill_Edit_Click(object sender, EventArgs e)
        {
            BillMaintenanceForm BillMaintenanceForm_ = new BillMaintenanceForm(DB, _BillMaintenance, true);
            BillMaintenanceForm_.ShowDialog();
            if (BillMaintenanceForm_.Changed)
            {
                _BillMaintenance = new BillMaintenanceSQL(DB).GetBillMaintenance_By_MaintenaceOPR(_MaintenanceOPR);
                LoadBillData(true);
            }
        }

        private void buttonBill_Delete_Click(object sender, EventArgs e)
        {
            bool sussecc = new BillMaintenanceSQL(DB).DeleteBillMaintenance(_BillMaintenance._Operation.OperationID);
            if (sussecc )
            {
                _BillMaintenance = null ;
                LoadBillData(true);
            }
        }
        #endregion
        #region EndWork    
        private async void LoadEndWorkData(bool Edit)
        {
            if (_MaintenanceOPR._MaintenanceOPR_EndWork == null)
            {
                buttonEndWork_Create.Enabled = true;
                buttonEndWork_Create.Text = "انتهاء العمل";
                buttonEndWork_Edit.Visible = false;
                buttonEndWork_Delete.Visible = false;
                textBoxEndWorkDate.Text = "-";
                textBoxEndWorkResult.Text = "-";
                textBoxDeilverdate.Text = "-";
                textBoxEndWarrantyDate.Text = "-";
                textBoxEndWorkReport.Text = "-";

            }
            else
            {
                textBoxEndWorkDate.Text = _MaintenanceOPR._MaintenanceOPR_EndWork.EndWorkDate.ToShortDateString();
                if (_MaintenanceOPR._MaintenanceOPR_EndWork.Repaired)
                {
                    textBoxEndWorkResult.Text = "تم الاصلاح";
                    textBoxEndWorkResult.BackColor = Color.LimeGreen;
                }
                else
                {
                    textBoxEndWorkResult.Text = "لم يتم الاصلاح";
                    textBoxEndWorkResult.BackColor = Color.Orange;
                }
                if(_MaintenanceOPR._MaintenanceOPR_EndWork.DeliveredDate!=null)
                {
                    textBoxDeilverdate.Text =Convert .ToDateTime ( _MaintenanceOPR._MaintenanceOPR_EndWork.DeliveredDate).ToShortDateString();
                    textBoxDeilverdate.BackColor = Color.LimeGreen;
                }
                else
                {
                    textBoxDeilverdate.Text = "-";
                    textBoxDeilverdate.BackColor = Color.Orange;
                }
                if (_MaintenanceOPR._MaintenanceOPR_EndWork.EndwarrantyDate  != null)
                {
                    textBoxEndWarrantyDate.Text = Convert.ToDateTime(_MaintenanceOPR._MaintenanceOPR_EndWork.EndwarrantyDate).ToShortDateString ();
                    if (DateTime.Now > _MaintenanceOPR._MaintenanceOPR_EndWork.EndwarrantyDate)
                        textBoxEndWarrantyDate.BackColor = Color.Orange;
                    else
                        textBoxEndWarrantyDate.BackColor = Color.LimeGreen;
                }
                else 
                {
                    textBoxEndWarrantyDate.Text = "  لا يوجد ضمان";
                }


                textBoxEndWorkReport.Text = _MaintenanceOPR._MaintenanceOPR_EndWork.Report;
                buttonEndWork_Create.Enabled = true;
                buttonEndWork_Create.Text = "استعراض";
                buttonEndWork_Edit.Visible = true;
                buttonEndWork_Delete.Visible = true;
            }
            if (!Edit)
            {
                buttonEndWork_Create.Visible = false;
                buttonEndWork_Edit.Visible = false;
                buttonEndWork_Delete.Visible = false;
            }


        }
        private void buttonEndWork_Create_Click(object sender, EventArgs e)
        {
            if (_MaintenanceOPR._MaintenanceOPR_EndWork == null)
            {
                MaintenanceOPR_EndWorkForm MaintenanceOPR_EndWorkForm_ = new MaintenanceOPR_EndWorkForm(DB,  _MaintenanceOPR);
                MaintenanceOPR_EndWorkForm_.ShowDialog();
                if (MaintenanceOPR_EndWorkForm_.Changed)
                {
                    _MaintenanceOPR._MaintenanceOPR_EndWork = new MaintenanceOPRSQL(DB).Get_MaintenanceOPR_EndWork_ForMaintenanceOPR(_MaintenanceOPR._Operation.OperationID);
                    LoadEndWorkData(true);
                }
            }
            else
            {
                MaintenanceOPR_EndWorkForm MaintenanceOPR_EndWorkForm_ = new MaintenanceOPR_EndWorkForm(DB, _MaintenanceOPR._MaintenanceOPR_EndWork,false );
                MaintenanceOPR_EndWorkForm_.ShowDialog();
                if (MaintenanceOPR_EndWorkForm_.Changed)
                {
                    _MaintenanceOPR._MaintenanceOPR_EndWork = new MaintenanceOPRSQL(DB).Get_MaintenanceOPR_EndWork_ForMaintenanceOPR(_MaintenanceOPR._Operation.OperationID);
                    LoadEndWorkData(true);
                }
            }
        }
        private void buttonEndWork_Edit_Click(object sender, EventArgs e)
        {
            MaintenanceOPR_EndWorkForm MaintenanceOPR_EndWorkForm_ = new MaintenanceOPR_EndWorkForm(DB, _MaintenanceOPR._MaintenanceOPR_EndWork, true );
            MaintenanceOPR_EndWorkForm_.ShowDialog();
            if (MaintenanceOPR_EndWorkForm_.Changed)
            {
                _MaintenanceOPR._MaintenanceOPR_EndWork = new MaintenanceOPRSQL(DB).Get_MaintenanceOPR_EndWork_ForMaintenanceOPR(_MaintenanceOPR._Operation.OperationID);
                LoadEndWorkData(true);
            }
        }

        private void buttonEndWork_Delete_Click(object sender, EventArgs e)
        {
            bool sussecc = new MaintenanceOPRSQL(DB).Delete_MaintenanceOPREndWork(_MaintenanceOPR._Operation.OperationID);
            if (sussecc)
            {
                _MaintenanceOPR._MaintenanceOPR_EndWork = null;
                LoadEndWorkData(true);
            }
        }
        #endregion
    }
}
