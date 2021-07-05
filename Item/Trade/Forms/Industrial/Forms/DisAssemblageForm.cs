using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.Objects;
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

namespace ItemProject.Trade.Forms.TradeForms
{
    public partial class DisAssemblageForm : Form
    {

        ItemObj.Objects.Folder LastUsedFolder;
        DisAssemblabgeOPR _DisAssemblabgeOPR;
        DatabaseInterface DB;
        ItemIN _TempItemIN;
        TradeStorePlace  _TempPlace;
        public bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }

        System.Windows.Forms.MenuItem OpenItemINMenuItem;
        System.Windows.Forms.MenuItem AddItemINMenuItem;
        System.Windows.Forms.MenuItem EditItemINMenuItem;
        System.Windows.Forms.MenuItem DeleteItemINMenuItem;

        List<ItemIN_ItemOUTReport   > ItemIN_ItemOUTReportList = new List<ItemIN_ItemOUTReport>();
        public DisAssemblageForm(DatabaseInterface db)
        {
            DB = db;
            InitializeComponent();


            _Changed = false;
            dateTimePicker_.Value = DateTime.Now;
            this.textboxEchangeRate.TextChanged += new System.EventHandler(this.textboxEchangeRate_TextChanged);
            this.comboBoxCurrency.SelectedIndexChanged += new System.EventHandler(this.comboBoxCurrency_SelectedIndexChanged);

            Currency currency = ProgramGeneralMethods.GetDefaultCurrency(DB);
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, currency);
            textboxEchangeRate.Text = currency.ExchangeRate.ToString();
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انشاء عملية تفكيك";
            panelDisassemplyItems.Enabled = false;
            InitializeMenuItems();
            AdjustListViewColumnsWidth();


        }
        public DisAssemblageForm(DatabaseInterface db,ItemIN ItemIN_,TradeStorePlace Place_)
        {
            DB = db;
            InitializeComponent();
            _TempItemIN = ItemIN_;
            _TempPlace = Place_;
            LoadItemINData();

            _Changed = false;
            dateTimePicker_.Value = DateTime.Now;
            this.textboxEchangeRate.TextChanged += new System.EventHandler(this.textboxEchangeRate_TextChanged);
            this.comboBoxCurrency.SelectedIndexChanged += new System.EventHandler(this.comboBoxCurrency_SelectedIndexChanged);

            Currency currency = ProgramGeneralMethods.GetDefaultCurrency(DB);
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, currency);
            textboxEchangeRate.Text = currency.ExchangeRate.ToString();
            buttonSave.Name = "buttonAdd";
            buttonSave.Text  = "انشاء عملية تفكيك";
            panelDisassemplyItems .Enabled = false;
            InitializeMenuItems();
            AdjustListViewColumnsWidth();
    

        }
        public DisAssemblageForm(DatabaseInterface db, DisAssemblabgeOPR DisAssemblabgeOPR_, bool edit)
        {
            InitializeComponent();
            DB = db;

            _DisAssemblabgeOPR = DisAssemblabgeOPR_;
            _TempItemIN = _DisAssemblabgeOPR._ItemOUT._ItemIN;
            _TempPlace = _DisAssemblabgeOPR._ItemOUT.Place ;
            _Changed = false;

            InitializeMenuItems();

            loadForm(edit);
            AdjustListViewColumnsWidth();


        }
       
         public void InitializeMenuItems()
        {
            OpenItemINMenuItem = new System.Windows.Forms.MenuItem("استعراض تفاصيل", OpenItemIN_MenuItem_Click);

            AddItemINMenuItem = new System.Windows.Forms.MenuItem("اضافة عنصر", AddItemIN_MenuItem_Click);
            EditItemINMenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditItemIN_MenuItem_Click);
            DeleteItemINMenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteItemIN_MenuItem_Click); ;

        }

        private void DeleteItemIN_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd != DialogResult.OK) return;
            string iteminid_Str = listView .SelectedItems[0].Name;
            uint iteminid = Convert.ToUInt32(iteminid_Str);
            bool Success = new ItemINSQL(DB).DeleteItemIN (iteminid);
            if (Success)
            {
                this._Changed = true;
                ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_DisAssemblabgeOPR ._Operation);
                RefreshBuyOperations(ItemIN_ItemOUTReportList);
            }
        }

        private void EditItemIN_MenuItem_Click(object sender, EventArgs e)
        {
            if ( listView.SelectedItems.Count > 0)
            {
                uint iteminid = Convert.ToUInt32(listView.SelectedItems[0].Name);
                ItemIN  itemin = new ItemINSQL (DB).GetItemININFO_BYID(iteminid);
                ItemINForm ItemINForm_ = new ItemINForm(DB, itemin ,true );
                ItemINForm_.ShowDialog();
                if (ItemINForm_.Changed)
                {
                    this._Changed = true;
                    ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_DisAssemblabgeOPR ._Operation);
                    RefreshBuyOperations(ItemIN_ItemOUTReportList);
                }
                ItemINForm_.Dispose();
                    
            }
        }
        private void OpenItemIN_MenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                uint iteminid = Convert.ToUInt32(listView.SelectedItems[0].Name);
                ItemIN  itemin = new ItemINSQL(DB).GetItemININFO_BYID(iteminid );
                ItemINForm ItemINForm_ = new ItemINForm(DB, itemin , false );
                ItemINForm_.ShowDialog();
                if (ItemINForm_.Changed)
                {
                    this._Changed = true;
                    ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_DisAssemblabgeOPR ._Operation);
                    RefreshBuyOperations(ItemIN_ItemOUTReportList);
                }
                ItemINForm_.Dispose();

            }
        }

        private void AddItemIN_MenuItem_Click(object sender, EventArgs e)
        {

            ItemINForm ItemINForm_ = new ItemINForm(DB, _DisAssemblabgeOPR ._Operation);
            DialogResult d = ItemINForm_.ShowDialog();
            if (ItemINForm_.Changed)
            {
                this._Changed  = true;
                ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_DisAssemblabgeOPR._Operation);
                RefreshBuyOperations(ItemIN_ItemOUTReportList);
            }
        }
        private void RefreshBuyOperations(List <ItemIN_ItemOUTReport   > ItemIN_ItemOUTReportList_)
        {
            listView.Items.Clear();
            if (_DisAssemblabgeOPR == null) return;
            ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxCurrency.SelectedItem;

 



            double totalcost = 0;

            for (int i = 0; i < ItemIN_ItemOUTReportList.Count; i++)
            {

                double buyprice = System.Math.Round(ItemIN_ItemOUTReportList[i]._ItemIN._INCost.Value  , 2);
                double total_buyprice = System.Math.Round(buyprice * ItemIN_ItemOUTReportList[i]._ItemIN.Amount, 3);
                totalcost = totalcost + total_buyprice;
                ListViewItem ListViewItem_ = new ListViewItem((listView.Items.Count + 1).ToString());
                //ListViewItem_.UseItemStyleForSubItems = false;
                ListViewItem_.Name = ItemIN_ItemOUTReportList[i]._ItemIN.ItemINID.ToString();
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList[i]._ItemIN._Item.ItemName);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList[i]._ItemIN._Item.ItemCompany);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList[i]._ItemIN._Item.folder.FolderName);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList[i]._ItemIN._TradeState.TradeStateName);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList[i]._ItemIN.Amount.ToString());
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList[i]._ItemIN._ConsumeUnit.ConsumeUnitName.ToString());
                ListViewItem_.SubItems.Add(buyprice.ToString() + " " + _DisAssemblabgeOPR ._Currency .CurrencySymbol.Replace(" ", string.Empty));
                ListViewItem_.SubItems.Add((total_buyprice).ToString() + " " + _DisAssemblabgeOPR._Currency.CurrencySymbol.Replace(" ", string.Empty));
                if (checkBoxShowDetails.Checked && listView.Name == "listViewDetails")
                {
                    double outamount = GetOUTAmount(ItemIN_ItemOUTReportList[i]);
                    if (outamount == 0)
                    {
                        ListViewItem_.SubItems.Add("-");
                        ListViewItem_.SubItems.Add("-");
                    }
                    else
                    {
                        ListViewItem_.SubItems.Add(outamount + ItemIN_ItemOUTReportList[i]._ItemIN._ConsumeUnit.ConsumeUnitName);
                        ListViewItem_.SubItems.Add(GetItemOUTsCost(ItemIN_ItemOUTReportList[i].ItemOUTList));
                    }
                    if (outamount == ItemIN_ItemOUTReportList[i]._ItemIN.Amount) ListViewItem_.BackColor = Color.Orange;
                    else
                        ListViewItem_.BackColor = Color.LimeGreen;
                }
                else ListViewItem_.BackColor = Color.LimeGreen;


                listView.Items.Add(ListViewItem_);

            }
            textBoxtotalvalue.Text = totalcost.ToString();


        }
        private string  GetItemOUTsCost(List <ItemOUT   > ItemOUTList)
        {
            if (ItemOUTList.Count == 0) return "-";
            string outcost = "";
            try
            {
                
                List<Currency> currencyList = ItemOUTList.Select(x =>new OperationSQL (DB).GetOperationItemOUTCurrency (x._Operation ) ).Distinct().ToList();
                
                for (int i = 0; i < currencyList.Count; i++)
                {
                    List<ItemOUT> tempoutlist = ItemOUTList.Where(x => new OperationSQL(DB).GetOperationItemOUTCurrency(x._Operation) == currencyList[i]).ToList();
                    for (int j = 0; j < tempoutlist.Count; j++)
                    {
                        //outcost = System.Math.Round((tempoutlist[j].Cost  * tempoutlist[j].Amount), 3) + currencyList[i].CurrencySymbol + " ";
                    }
                }
              
            }
            catch
            {
               
            }
            return outcost;

        }

        private double GetOUTAmount(ItemIN_ItemOUTReport ItemIN_ItemOUTReport_)
        {
            if (ItemIN_ItemOUTReport_.ItemOUTList .Count == 0) return 0;
            double amount = 0;
            try
            {
                
                    for (int j = 0; j < ItemIN_ItemOUTReport_.ItemOUTList .Count; j++)
                    {

                        amount = amount + System .Math .Round ((ItemIN_ItemOUTReport_.ItemOUTList[j].Amount *(ItemIN_ItemOUTReport_.ItemOUTList[j]._ConsumeUnit .Factor / ItemIN_ItemOUTReport_._ItemIN ._ConsumeUnit.Factor )),3);
  
                    }
             

            }
            catch
            {

            }
            return amount ;
        }
          public void loadForm(bool edit)
        {
            try
            {
                buttonSave.Name = "buttonSave";
                buttonSave.Text = "حفظ";
                panelDisassemplyItems.Enabled = true;
                if (_DisAssemblabgeOPR != null)
                {
                    ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_DisAssemblabgeOPR._Operation);
                    LoadItemINData();
                    textBoxcost.Text = _DisAssemblabgeOPR._ItemOUT._OUTValue.Value .ToString();

                    dateTimePicker_.Value = _DisAssemblabgeOPR.OprDate;

                   ProgramGeneralMethods.  FillComboBoxCurrency(ref comboBoxCurrency,DB, _DisAssemblabgeOPR._Currency);
                    textboxEchangeRate.Text = _DisAssemblabgeOPR.ExchangeRate.ToString();
                    textBoxItemSerial.Text = _DisAssemblabgeOPR.ItemSerial;
                    textBoxNotes.Text = _DisAssemblabgeOPR._ItemOUT.Notes;
                    labeldisassemplyinfo.Text = "عملية تفكيك رقم:" + _DisAssemblabgeOPR._Operation.OperationID.ToString();


                    RefreshBuyOperations(ItemIN_ItemOUTReportList);

                    if (edit)
                    {

                        panelDisassemplyItems.Enabled = false;
                        textboxEchangeRate.ReadOnly = false;
                        textBoxNotes.ReadOnly = false;
                        textBoxItemSerial.ReadOnly = false;
                        textBoxcost.ReadOnly = false;
                        dateTimePicker_.Enabled = true;
                        comboBoxCurrency.Enabled = true;
                        checkBoxRavageDisassmpledItem.Enabled = true;

                    }
                    else
                    {
                        this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
                        this.listView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDown);
                        ButtonGetItemIN_AvailableItems.Enabled = false;
                        buttonSelectWayLeft.Enabled = false;
                        buttonSelectWayRight.Enabled = false;
                        panelDisassemplyItems.Enabled = true;
                        textboxEchangeRate.ReadOnly = true;
                        textBoxNotes.ReadOnly = true;
                        textBoxItemSerial.ReadOnly = true;
                        textBoxcost.ReadOnly = true;
                        dateTimePicker_.Enabled = false;
                        comboBoxCurrency.Enabled = false;
                        checkBoxRavageDisassmpledItem.Enabled = false;


                    }


                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
           
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
           
            if(_TempItemIN ==null )
            {
                MessageBox.Show("يجب تحديد العنصر المراد تفكيكه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            ComboboxItem item = (ComboboxItem)comboBoxCurrency .SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(item.Value);
            if (buttonSave.Name == "buttonAdd")
            {
                try
                {
                    double exchangerate;
                    try
                    {
                        exchangerate = Convert.ToDouble(textboxEchangeRate.Text);
                    }
                    catch
                    {
                        MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                   
                    DisAssemblabgeOPR DisAssemblabgeOPR_ = new DisAssemblageSQL(DB).CreateAssemblageOPR
                        (dateTimePicker_.Value, _TempItemIN.ItemINID, _TempPlace
                        ,  textBoxItemSerial.Text, textBoxNotes.Text, currency, exchangerate, (checkBoxRavageDisassmpledItem.Checked ? true : false));
                    if (DisAssemblabgeOPR_ != null)
                    {
                        _DisAssemblabgeOPR = DisAssemblabgeOPR_;
                        _TempItemIN = _DisAssemblabgeOPR._ItemOUT._ItemIN;
                        _TempPlace = _DisAssemblabgeOPR._ItemOUT.Place;
                        MessageBox.Show("تم انشاء عملية التفكيك بنجاح ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this._Changed = true;
                        loadForm(false );

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":فشل انشاء العملية"+ee.Message , "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }

            else
            {
               try
                {
                    if (_DisAssemblabgeOPR != null)
                    {
                        double exchangerate;
                        try
                        {
                            exchangerate = Convert.ToDouble(textboxEchangeRate.Text);
                        }
                        catch
                        {
                            MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
              
                        bool success = new DisAssemblageSQL(DB).UpdateDisAssemblageOPR
                            (_DisAssemblabgeOPR._Operation.OperationID, dateTimePicker_.Value
                            , _TempItemIN.ItemINID, _TempPlace, textBoxNotes.Text
                            , textBoxItemSerial.Text, currency, exchangerate, (checkBoxRavageDisassmpledItem.Checked ? true : false));
                        if (success == true)
                        {
                            MessageBox.Show("تم الحفظ بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _DisAssemblabgeOPR = new DisAssemblageSQL(DB).GetDisAssemblageOPR_INFO_BYID(_DisAssemblabgeOPR._Operation.OperationID);
                            this._Changed = true;
                            loadForm(false );
                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":فشل تعديل العملية" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listView.SelectedItems.Count > 0)
            {
                OpenItemINMenuItem.PerformClick();
            }
        }
        private void listView_MouseDown(object sender, MouseEventArgs e)
        {
            listView.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listView.Items)
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


                        MenuItem[] mi1 = new MenuItem[] {OpenItemINMenuItem  ,EditItemINMenuItem  ,DeleteItemINMenuItem,new MenuItem ("-"), AddItemINMenuItem };
                        listView.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddItemINMenuItem  };
                    listView.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        private void listView_Resize(object sender, EventArgs e)
        {
            AdjustListViewColumnsWidth();
        }
        public void AdjustListViewColumnsWidth()
        {
            listView.Columns[0].Width = 80;
            
            for(int i=1;i<listView .Columns .Count;i++)
              listView.Columns[i].Width = ((listView.Width - 80) / (listView.Columns.Count-1 ))-1;
        }  
        private void comboBoxCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (_TempItemIN == null) return;
            ComboboxItem item = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(item.Value);
            if(currency .ReferenceCurrencyID ==null )
            {
                textboxEchangeRate.Text = "1";
                textboxEchangeRate.ReadOnly = true ;
            }
            else
            {
                textboxEchangeRate.Text = currency.ExchangeRate.ToString();
            }
        }

        private void textboxEchangeRate_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    double exchangerate = Convert.ToDouble(textboxEchangeRate.Text);
            //    double iteminoperation_exchangerate = new OperationSQL(DB).GetOperationItemINCurrency(_TempItemIN._Operation).ExchangeRate ;
            //    textBoxcost.Text = System.Math .Round ((_TempItemIN.Cost * (exchangerate  / iteminoperation_exchangerate)),3).ToString();
            //}
            //catch
            //{
            //    textBoxcost.Text = "";
            //}
        }

        private void buttonSelectItemIN_Click(object sender, EventArgs e)
        {
            if(ButtonGetItemIN_AvailableItems .Name == "ButtonGetItemIN_AvailableItems")
            {
                ItemObj.Forms.AvailableItemTopLevelForm AvailableItemTopLevelForm_ = new ItemObj.Forms.AvailableItemTopLevelForm(DB, LastUsedFolder, true);
                DialogResult d1 = AvailableItemTopLevelForm_.ShowDialog();
                if (d1 == DialogResult.OK)
                {
                    Item item_ = AvailableItemTopLevelForm_.ReturnItem;
                    ItemObj.Forms.AvailableItem_ItemIN_Form AvailableItem_ItemINS_Form = new ItemObj.Forms.AvailableItem_ItemIN_Form(DB, item_, true);
                    DialogResult d2 = AvailableItem_ItemINS_Form.ShowDialog();
                    if (d2 == DialogResult.OK)
                    {
                        ItemIN ItemIN_ = AvailableItem_ItemINS_Form.ReturnItemIN;
                        TradeStorePlace place = null;
                        ItemINForm ItemINForm_ = new ItemINForm(DB, ItemIN_, place);
                        DialogResult d3 = ItemINForm_.ShowDialog();
                        if (d3 == DialogResult.OK)
                        {
                            _TempPlace = ItemINForm_.ReturnPlace;
                            _TempItemIN = ItemIN_;

                            RefreshComponents();

                        }

                        ItemINForm_.Dispose();

                    }

                    AvailableItem_ItemINS_Form.Dispose();
                }
                AvailableItemTopLevelForm_.Dispose();
            }
            else
            {
                Container.ShowLocations showlocations = new Container.ShowLocations(DB, null, true);
                DialogResult d1 = showlocations.ShowDialog();
                if (d1 == DialogResult.OK)
                {
                    TradeStorePlace Place = showlocations.ReturnPlace;
                    showlocations.Dispose();
                    Container.PlaceItemsForm PlaceItemsForm_ = new Container.PlaceItemsForm(DB, Place, true);
                    DialogResult d2 = PlaceItemsForm_.ShowDialog();
                    if (d2 == DialogResult.OK)
                    {
                        Item item = PlaceItemsForm_.ReturnItem;
                        PlaceItemsForm_.Dispose();
                        Trade.Forms.Container.PlaceItem_ItemINForm PlaceItemItemINForm_ = new Container.PlaceItem_ItemINForm(DB, Place, item, true);
                        DialogResult d3 = PlaceItemItemINForm_.ShowDialog();
                        if (d3 == DialogResult.OK)
                        {
                            _TempItemIN = PlaceItemItemINForm_.ReturnItemIN;
                            PlaceItemItemINForm_.Dispose();
                            _TempPlace = Place;
                            RefreshComponents();

                        }
                        PlaceItemItemINForm_.Dispose();

                    }
                    PlaceItemsForm_.Dispose();
                }
                showlocations.Dispose();
            }
        
        }
        public void RefreshComponents()
        {
            if (_TempItemIN == null) return;
            Currency itemin_currency = new OperationSQL(DB).GetOperationItemINCurrency(_TempItemIN._Operation);
            ComboboxItem currencycomboitem = (ComboboxItem)comboBoxCurrency.SelectedItem;
            //double exchnagerate;
            //try
            //{
            //    exchnagerate = Convert.ToDouble(textboxEchangeRate.Text);
            //    textBoxcost.Text = ((exchnagerate / itemin_currency.ExchangeRate) * _TempItemIN._INCost .Value ).ToString();

            //}
            //catch
            //{
            //    textBoxcost.Text = "سعر الصرف غير مناسب";
            //}
            LoadItemINData();

        }
        private void LoadItemINData()
        {
            try
            {
                if (_TempItemIN != null)
                {
                    textBoxcost.Text = _TempItemIN._INCost.Value.ToString();
                    LastUsedFolder = _TempItemIN._Item.folder;
                    textBoxItemState.Text = _TempItemIN._TradeState.TradeStateName;
                    textBoxItemID.Text = _TempItemIN._Item.ItemID.ToString();
                    textBoxItemName.Text = _TempItemIN._Item.ItemName;
                    textBoxItemCompany.Text = _TempItemIN._Item.ItemCompany;
                    textBoxItemType.Text = _TempItemIN._Item.folder.FolderName;

                    textBoxOperationID.Text = _TempItemIN._Operation.OperationID.ToString();
                    textBoxOperationType.Text = _TempItemIN._Operation.OperationType.ToString();
                    textBoxItemINID.Text = _TempItemIN.ItemINID.ToString();

                    if (_TempPlace != null)
                        textBoxStorePlace.Text = _TempPlace.GetPlaceInfo();
                    else
                        textBoxStorePlace.Text = "-";

                    textBoxAvailableAmount.Text = new AvailableItemSQL(DB).GetAvailabeAmount_by_Place(_TempItemIN, _TempPlace).ToString();

                    checkBoxRavageDisassmpledItem.Visible = true;
                    textBoxItemSerial.Enabled = true;

                }
                else
                {
                    checkBoxRavageDisassmpledItem.Checked = false; ;
                    checkBoxRavageDisassmpledItem.Visible = true;
                    textBoxItemSerial.Text = "";
                    textBoxItemSerial.Enabled = false;
                }

            }
            catch
            {

            }

        }

        private void buttonSelectWayRight_Click(object sender, EventArgs e)
        {
            if(ButtonGetItemIN_AvailableItems .Name == "ButtonGetItemIN_StorePlace")
            {
                ButtonGetItemIN_AvailableItems.Name = "ButtonGetItemIN_AvailableItems";
                ButtonGetItemIN_AvailableItems.Text = "تحديد العنصر المفكك عن طريق البحث في العناصر المتوفرة";
            }
            else
            {
                ButtonGetItemIN_AvailableItems.Name = "ButtonGetItemIN_StorePlace";
                ButtonGetItemIN_AvailableItems.Text = "تحديد العنصر المفكك عن طريق البحث في أماكن التخزين";
            }
        }

        private void عرضالعناصرالخارجةمنالعناصرالداخلةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationItemsIN_ItemOutListForm OperationItemsIN_ItemOutListForm_ = new OperationItemsIN_ItemOutListForm(DB, _DisAssemblabgeOPR ._Operation);
            OperationItemsIN_ItemOutListForm_.ShowDialog();
        }
    }
}
