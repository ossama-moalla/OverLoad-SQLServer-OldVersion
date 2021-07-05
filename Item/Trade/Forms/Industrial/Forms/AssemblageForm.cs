using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.ItemObjSQL;
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
    public partial class AssemblageForm : Form
    {
        System.Windows.Forms.MenuItem OpenItemOUTMenuItem;
        System.Windows.Forms.MenuItem AddItemOUTMenuItem;
        System.Windows.Forms.MenuItem EditItemOUTMenuItem;
        System.Windows.Forms.MenuItem DeleteItemOUTMenuItem;

        System.Windows.Forms.MenuItem setPriceMenuItem; 
        System.Windows.Forms.MenuItem  UnsetpriceMenuItem ;




        List<ItemOUT> _ItemOUTList = new List<ItemOUT>();

        List<TradeState> TradeStateList = new List<TradeState>();
        List<ItemINSellPrice> ItemINSellPriceList = new List<ItemINSellPrice>();
        List<SellType> selltypelist = new List<SellType>();
        List<ConsumeUnit > ConsumUnitsList = new List<ConsumeUnit >();

        DatabaseInterface DB;
        AssemblabgeOPR _AssemblabgeOPR;
        //Currency _Currency;
         Item _Item;
        Folder LastUsedFolder;
        private bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }
        bool Edit;

        //TradeStorePlace _TempStorePlace;

        public AssemblageForm(DatabaseInterface db)
        {

            InitializeComponent();
            InitializeMenuItems();
            DB = db;

            AdjustListViewColumnsWidth();
            TradeStateList = new TradeStateSQL(DB).GetTradeStateList();
            selltypelist = new SellTypeSql(DB).GetSellTypeList();

            LastUsedFolder = null;
            panelStoreAmount.Enabled = false;
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انشاء";
            Currency  _Currency = ProgramGeneralMethods.GetDefaultCurrency(DB);
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _Currency);
            textBoxExchangeRate.Text = _Currency.ExchangeRate  .ToString();
            FillComboBoxTradeState(null);
            textBoxBuyPrice.Text = "0";
            InitialzeDataGridViewItemSellPrices();
            this.listView.Resize += new System.EventHandler(this.listView_Resize);

        }

        public AssemblageForm(DatabaseInterface db,  AssemblabgeOPR AssemblabgeOPR_, bool Edit_)
        {
            InitializeComponent();
            InitializeMenuItems();
            AdjustListViewColumnsWidth();
            Edit = Edit_;
            DB = db;
            TradeStateList = new TradeStateSQL(DB).GetTradeStateList();
            selltypelist = new SellTypeSql(DB).GetSellTypeList();

            InitialzeDataGridViewItemSellPrices();

            _AssemblabgeOPR  = AssemblabgeOPR_;
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _AssemblabgeOPR._Currency );
            _Item = _AssemblabgeOPR._ItemIN._Item;
     
            if (Edit )
            {
                this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
                this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
            }
            else
            {
                labelItemInfo.Text = "معلومات العنصر";
                textBoxItemID.ReadOnly = true;
            }

            
            textBoxExchangeRate.Text = _AssemblabgeOPR .ExchangeRate .ToString();
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _AssemblabgeOPR._Currency);
            FillComboBoxTradeState (_AssemblabgeOPR. _ItemIN ._TradeState );
            LoadForm(Edit );
            this.listView.Resize += new System.EventHandler(this.listView_Resize);




        }
        public void InitializeMenuItems()
        {
            OpenItemOUTMenuItem = new System.Windows.Forms.MenuItem("عرض التافصيل", OpenItemOUT_MenuItem_Click);

            AddItemOUTMenuItem = new System.Windows.Forms.MenuItem("اضافة عنصر مساهم في عملية التجميع", AddItemOUT_MenuItem_Click);
            EditItemOUTMenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditItemOUT_MenuItem_Click);
            DeleteItemOUTMenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteItemOUT_MenuItem_Click); ;

            setPriceMenuItem = new System.Windows.Forms.MenuItem("ضبط السعر", setprice_MenuItem_Click);
            UnsetpriceMenuItem = new System.Windows.Forms.MenuItem("الغاء ضبط السعر", Unsetprice_MenuItem_Click);


        }
        public void LoadForm(bool edit)
        {
            try
            {
                buttonSave.Name = "buttonSave";
                buttonSave.Text = "حفظ";
                if (_AssemblabgeOPR == null) return;

                _Item = _AssemblabgeOPR._ItemIN._Item;

                FillComboBoxConsumeUnit(null);
                LastUsedFolder = _Item.folder;
                
               

                //textBoxBuyPrice.Text = _AssemblabgeOPR._ItemIN.Cost.ToString();
                LoadItemData();
                ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_AssemblabgeOPR._ItemIN);

                FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);
                LoadStoreData();
                _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_AssemblabgeOPR._Operation);
                RefreshAssemblageItemOutLIst(_ItemOUTList);
                if (edit)
                {
                    this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
                    this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
                    panelStoreAmount.Enabled = false;
                    textBoxItemSerial.ReadOnly = false;
                    dateTimePicker_.Enabled = true ;
                    textBoxItemID.ReadOnly = false ;
                    textBoxBuyPrice.ReadOnly = false ;
                    comboBoxCurrency.Enabled = true ;
                    comboBoxTradestate.Enabled = true ;
                    TextboxNotes.ReadOnly = false ;
                    textBoxExchangeRate.ReadOnly = false;
                }
                else
                {

                    this.textBoxPlaceID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
                    this.textBoxPlaceID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxPlace_MouseDoubleClick);
                    this.listView .MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
                    this.listView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDown);
                    panelStoreAmount.Enabled = true;
                    textBoxItemSerial.ReadOnly = true ;
                    dateTimePicker_.Enabled = false;
                    textBoxItemID.ReadOnly = true;
                    textBoxBuyPrice.ReadOnly = true;
                    comboBoxCurrency.Enabled = false;
                    comboBoxTradestate.Enabled = false;
                    TextboxNotes.ReadOnly = true;
                    textBoxExchangeRate.ReadOnly = true ;

                }

  
            }
            catch(Exception ee)
            {
                MessageBox.Show("LoadingForm:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }




        }
        private void LoadItemData()
        {
            textBoxItemID.Text = _Item.ItemID.ToString();
            textBoxItemName.Text = _Item.ItemName;
            textBoxItemCompany.Text = _Item.ItemCompany;
            textBoxItemType.Text = _Item.folder.FolderName;
            ConsumUnitsList = new ConsumeUnitSql(DB).GetConsumeUnitList(_Item);



        }
        public void FillItemConsumeUnitsAndSellPrices(List<ItemINSellPrice> ItemINSellPriceList)
        {
            if (_AssemblabgeOPR . _ItemIN == null)
            {
                panelItemINSellPrices.Enabled = false;
                panelItemINSellsProfit.Enabled = false;
                return;

            }
            ComboboxItem comboboxitem_curency = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency _Currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(comboboxitem_curency.Value);
            panelItemINSellPrices.Enabled = true ;
            panelItemINSellsProfit.Enabled = true ;
            ComboboxItem comboboxitem = (ComboboxItem)comboBoxTradestate.SelectedItem;
            dataGridView1.TopLeftHeaderCell.Value = comboboxitem.Text;

            dataGridView1.Rows.Clear();
            dataGridViewProfit.Rows.Clear();
            //if (_Item == null) return;
            if ( selltypelist.Count != 0)
            {


                for (int i = 0; i < ConsumUnitsList.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridViewProfit.Rows.Add();
                    dataGridView1.Rows[i].HeaderCell.Value = ConsumUnitsList[i].ConsumeUnitName;
                    dataGridViewProfit.Rows[i].HeaderCell.Value = ConsumUnitsList[i].ConsumeUnitName;
                    for (int j = 0; j < selltypelist.Count; j++)
                    {
                        try
                        {

                            List<ItemINSellPrice> dd = ItemINSellPriceList.Where(x => (x._ItemIN._Operation .OperationID  == _AssemblabgeOPR . _ItemIN._Operation.OperationID && x.SellType_.SellTypeID == selltypelist[j].SellTypeID && x.ConsumeUnit_.ConsumeUnitID == ConsumUnitsList[i].ConsumeUnitID)).ToList();
                            if (dd.Count != 1) dataGridView1.Rows[i].Cells[j].Value = " - " + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);
                            else
                            {
                                double sellprice = System.Math.Round(Convert.ToDouble(dd[0].Price) * _Currency .ExchangeRate ,3) ;
                                dataGridView1.Rows[i].Cells[j].Value = sellprice .ToString() + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);
                                try
                                {
                                    double buyprice = Convert.ToDouble(textBoxBuyPrice.Text );
                                    double buyamount = 1;
                                    double profit = (sellprice - buyprice) * buyamount;
                                    dataGridViewProfit.Rows[i].Cells[j].Value = System.Math.Round(profit , 3).ToString() + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);

                                }
                                catch
                                {
                                    dataGridViewProfit.Rows[i].Cells[j].Value = " - " + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);
                                }
                                
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(" gg " + ee.Message);
                        }
                    }
                    //    double? price = new ItemSellPriceSql(DB).GetPrice(_Item, TradeState_, selltypelist[j], ConsumUnitsList[i]);
                    //    if (price == null) dataGridView1.Rows[i].Cells[j].Value = " - " + " " + _BillOUT._Currency.CurrencySymbol.Replace (" ",string.Empty );

                    //    else dataGridView1.Rows[i].Cells[j].Value = System.Math.Round((Convert.ToDouble(price) * _BillOUT .ExchangeRate), 3).ToString() + " " + _BillOUT._Currency .CurrencySymbol.Replace(" ", string.Empty);
                    //}


                }
            }

        }
        public void InitialzeDataGridViewItemSellPrices()
        {
            try
            {
                for (int i = 0; i < selltypelist.Count; i++)
                {
                    dataGridView1.Columns.Add(selltypelist[i].SellTypeID.ToString(), selltypelist[i].SellTypeName);
                    dataGridViewProfit .Columns.Add(selltypelist[i].SellTypeID.ToString(), selltypelist[i].SellTypeName);

                }
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Aqua;
                dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Aqua;
                dataGridView1.TopLeftHeaderCell.Style.BackColor = Color.Orange;
                AdjustmentDatagridviewColumnsWidth();
            }

            catch
            {
                MessageBox.Show("فشل في جلب قائمة انماط البيع", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void AdjustmentDatagridviewColumnsWidth()
        {
            int columnscount = dataGridView1.Columns.Count + 1;
            dataGridView1.RowHeadersWidth = dataGridView1.Width / columnscount;
            dataGridViewProfit.RowHeadersWidth = dataGridView1.Width / columnscount;
            for (int i = 0; i < columnscount - 1; i++)
            {
                dataGridView1.Columns[i].Width = (dataGridView1.Width - 5) / columnscount;
                dataGridViewProfit.Columns[i].Width = (dataGridView1.Width - 5) / columnscount;
            }

        }
      
       
        private void FillComboBoxTradeState(TradeState tradestate)
        {
            comboBoxTradestate.Items.Clear();
            int selected_index = 0;
                try
                {
                    List<TradeState> TradeStateList = new TradeStateSQL(DB).GetTradeStateList();
                    for (int i = 0; i < TradeStateList.Count; i++)
                    {
                        ComboboxItem item = new ComboboxItem(TradeStateList[i].TradeStateName , TradeStateList[i].TradeStateID);
                        comboBoxTradestate.Items.Add(item);
                        if (tradestate != null && tradestate.TradeStateID == TradeStateList[i].TradeStateID) selected_index = i;
                    }
                    comboBoxTradestate.SelectedIndex = selected_index;

                }
                catch
                { }

        }
       
        private void textBoxItem_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ItemObj.Forms.SelectItem form = new ItemObj.Forms.SelectItem(DB, LastUsedFolder);
                DialogResult dd = form.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    _Item  = form.ReturnItem ;
                    LoadItemData();
                }
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (_Item == null)
            {
                MessageBox.Show("يرجى تحديد العنصر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double Exchangerate_;
            try
            {
                Exchangerate_ = Convert.ToDouble(textBoxExchangeRate.Text);
            }
            catch
            {
                MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ComboboxItem Currency_ComboboxItem = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Currency_ComboboxItem.Value);
            ComboboxItem tradestateitem = (ComboboxItem)comboBoxTradestate.SelectedItem;
            TradeState tradestate = new TradeStateSQL(DB).GetTradeStateBYID(tradestateitem.Value);
            AssemblageSQL AssemblageSQL_ = new AssemblageSQL(DB);
            if (buttonSave.Name == "buttonAdd")
            {

                AssemblabgeOPR AssemblabgeOPR_ = AssemblageSQL_.CreateAssemblageOPR 
                    (dateTimePicker_ .Value  ,_Item , tradestate 
                    , Convert.ToDouble (textBoxBuyPrice.Text)
                    ,TextboxNotes .Text  ,textBoxItemSerial.Text ,Currency_ ,Exchangerate_ );
                if (AssemblabgeOPR_ != null)
                {
                    _AssemblabgeOPR = AssemblabgeOPR_;
                    MessageBox.Show("تم انشاء عملية التجميع بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this._Changed   = true;
                    LoadForm(false );

                }
            }

            else
            {

                if (_AssemblabgeOPR  != null)
                {
                  

                    bool success = AssemblageSQL_.UpdateAssemblageOPR 
                        (_AssemblabgeOPR ._Operation.OperationID,dateTimePicker_ .Value,
                        _AssemblabgeOPR._ItemIN .ItemINID ,_Item, tradestate,
                         Convert.ToDouble(textBoxBuyPrice.Text)
                    , TextboxNotes.Text,textBoxItemSerial .Text ,Currency_ ,Exchangerate_);
                    if (success == true)
                    {
                        MessageBox.Show("تم حفظ  بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _AssemblabgeOPR   = AssemblageSQL_.GetAssemblageOPR_INFO_BYID (_AssemblabgeOPR  ._Operation.OperationID );
                        this._Changed = true;
                        LoadForm(false);
                    }
                    else MessageBox.Show("لم يتم الحفظ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error );

                }
            }
        }
        
        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            AdjustmentDatagridviewColumnsWidth();
        }
        private void textBoxItemID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue ==13)
            {
                try
                {
                    uint itemid = Convert.ToUInt32(textBoxItemID.Text);
                    Item item__ = new ItemSQL(DB).GetItemInfoByID(itemid);
                    if(item__!=null)
                    {
                        _Item = item__;
                        LoadItemData();
                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على العنصر","خطأ",MessageBoxButtons.OK,MessageBoxIcon.Error );
                    }
                }
                catch
                {
                    MessageBox.Show("يرجى ادخال عدد صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }   
        private void buttonStore_Click(object sender, EventArgs e)
        {
            try
            {
                if(buttonStore .Name == "buttonStore")
                {
                    TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert .ToUInt32 (textBoxPlaceID .Name ));
                    if (place == null)
                    {
                        MessageBox.Show("يرجى تحديد مكان التخزين", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    bool success = new TradeItemStoreSQL(DB).Store_Item_INPlace( place.PlaceID , _AssemblabgeOPR._ItemIN.ItemINID, TradeItemStore.ITEMIN_STORE_TYPE, 1, null);
                    if (success)
                    {
                  
                        LoadStoreData();

                    }
                }
                else
                {
                    List<TradeItemStore> placelist = new TradeItemStoreSQL(DB).GetItemStoredPlaces(_AssemblabgeOPR._ItemIN.ItemINID);
                    bool success = new TradeItemStoreSQL(DB).UNStore_Item_INPlace ( placelist[0]._TradeStorePlace.PlaceID , _AssemblabgeOPR._ItemIN.ItemINID,TradeItemStore.ITEMIN_STORE_TYPE);
                    if (success)
                    {
                        LoadStoreData();

                    }
                }

                
           }
            catch(Exception ee)
            {
                MessageBox.Show("ضبط التخزين:"+ee.Message );
            }
         
        }
        private void FillComboBoxConsumeUnit(ConsumeUnit consumeunit)
        {
            if (_Item == null) return;
            comboBoxStoreConsumeUnit.Items.Clear();
            int selected_index = 0;
            try
            {
                List<ConsumeUnit> ConsumeUnitList = new ConsumeUnitSql(DB).GetConsumeUnitList(_Item);
                for (int i = 0; i < ConsumeUnitList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(ConsumeUnitList[i].ConsumeUnitName, ConsumeUnitList[i].ConsumeUnitID);
                    comboBoxStoreConsumeUnit.Items.Add(item);
                    if (consumeunit != null && consumeunit.ConsumeUnitID == ConsumeUnitList[i].ConsumeUnitID) selected_index = i;
                }
                comboBoxStoreConsumeUnit.SelectedIndex = 0;
            }
            catch
            { }

        }
        private void LoadStoreData()
        {

            try
            {
                List<TradeItemStore> TradeItemStoreList = new TradeItemStoreSQL(DB).GetItemStoredPlaces(_AssemblabgeOPR._ItemIN.ItemINID);
                buttonClearstoreinfo.Visible = false;
                if (TradeItemStoreList.Count  == 0)
                {
                    buttonStore.Name = "buttonStore";
                    buttonStore.Text = "ضبط التخزين";
                    textBoxPlaceID.Name  = "textBoxPlaceID";
                    textBoxPlaceID.Text = "";
                    textBoxPlaceInfo.Text  = "";

                    buttonStore.Visible = false;
                    textBoxPlaceID.Enabled  = true ;
                }
                else
                {
                    textBoxPlaceID.Enabled = false;
                    buttonStore.Name = "buttonUnStore";
                    buttonStore.Text = "الغاء التخزين";
                    buttonStore.Visible = true;
                    textBoxPlaceID.Text = TradeItemStoreList[0]._TradeStorePlace.PlaceID.ToString();
                    textBoxPlaceID.Name  = TradeItemStoreList[0]._TradeStorePlace.PlaceID.ToString();

                    textBoxPlaceInfo.Text = TradeItemStoreList[0]._TradeStorePlace.GetPlaceInfo();
                }

                
            }
            catch (Exception ee)
            {
                MessageBox.Show("Loading Store Info" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listView.SelectedItems.Count > 0)
            {
                OpenItemOUTMenuItem.PerformClick();
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


                    MenuItem[] mi1 = new MenuItem[] { OpenItemOUTMenuItem, EditItemOUTMenuItem, DeleteItemOUTMenuItem, new MenuItem("-"), AddItemOUTMenuItem };
                    listView.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddItemOUTMenuItem };
                    listView.ContextMenu = new ContextMenu(mi);

                }

            }

        }
           
        
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                ConsumeUnit Selected_ConsumeUnit = null;
                for (int i = 0; i < ConsumUnitsList.Count; i++)
                {
                    if (ConsumUnitsList[i].ConsumeUnitName.Equals(dataGridView1.Rows[e.RowIndex].HeaderCell.Value))
                    {
                        Selected_ConsumeUnit = ConsumUnitsList[i];
                        break;
                    }
                }
                ComboboxItem tradestate_item = (ComboboxItem)comboBoxTradestate.SelectedItem;
                TradeState TradeState_ = new TradeState(tradestate_item.Value, tradestate_item.Text);
                SellType Selected_SellType = new SellTypeSql(DB).GetSellTypeinfo(Convert.ToUInt32(dataGridView1.Columns[e.ColumnIndex].Name));
                List <ItemINSellPrice> ff= ItemINSellPriceList.Where(x => (x._ItemIN.ItemINID ==_AssemblabgeOPR . _ItemIN.ItemINID && x.SellType_.SellTypeID == Selected_SellType.SellTypeID && x.ConsumeUnit_.ConsumeUnitID == Selected_ConsumeUnit.ConsumeUnitID)).ToList();
                ContextMenu m = new ContextMenu();
                if (ff.Count >0)
                {               
                    m.MenuItems.Add(setPriceMenuItem );
                    m.MenuItems.Add(UnsetpriceMenuItem);
                }
                else
                {
                    m.MenuItems.Add(setPriceMenuItem);
                }
                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                m.Show(dataGridView1, new Point(e.X, e.Y));

            }
        }
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ComboboxItem comboboxitem_curency = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency _Currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(comboboxitem_curency.Value);
            ConsumeUnit SelectedConsumeUnit = null;
            for (int i = 0; i < ConsumUnitsList.Count; i++)
            {
                if (ConsumUnitsList[i].ConsumeUnitName.Equals(dataGridView1.Rows[e.RowIndex].HeaderCell.Value))
                {
                    SelectedConsumeUnit = ConsumUnitsList[i];
                    break;
                }
            }
            ComboboxItem tradestate_item = (ComboboxItem)comboBoxTradestate.SelectedItem;
            TradeState TradeState_ = new TradeState(tradestate_item.Value, tradestate_item.Text);
            SellType SelectedSellType = new SellTypeSql(DB).GetSellTypeinfo(Convert.ToUInt32(dataGridView1.Columns[e.ColumnIndex].Name));
          Trade.Forms.PriceInputBox inp = new Trade.Forms.PriceInputBox("ضبط السعر", SelectedSellType.SellTypeName, TradeState_.TradeStateName , SelectedConsumeUnit.ConsumeUnitName, _Currency .CurrencyName, "");
            inp.ShowDialog();
            if (inp.DialogResult == DialogResult.OK)
            {
                try
                {
                    double price = Convert.ToDouble(inp.Price);
                    if (price < 0)
                    {
                        MessageBox.Show("السعر يجب ان يكون اكبر من الصفر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    new ItemINSellPriceSql(DB).SetItemINPrice(_AssemblabgeOPR . _ItemIN, SelectedConsumeUnit, SelectedSellType, price / _Currency .ExchangeRate );
                    ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_AssemblabgeOPR._ItemIN);
                    FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            //dataGridView1.BeginEdit(true);

            ////optionally set the EditMode before you call BeginEdit
            //dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

    

        //private void comboBoxTradestate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillItemConsumeUnitsAndSellPrices(ItemSellPriceList);
        //}
        private void Unsetprice_MenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dd = MessageBox.Show("هل انت متاكد من الغاء ضبط السعر", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd == DialogResult.OK)
            {
                ConsumeUnit Selected_ConsumeUnit = null;
                for (int i = 0; i < ConsumUnitsList.Count; i++)
                {
                    if (ConsumUnitsList[i].ConsumeUnitName.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].HeaderCell.Value))
                    {
                        Selected_ConsumeUnit = ConsumUnitsList[i];
                        break;
                    }
                }
                SellType Selected_SellType = new SellTypeSql(DB).GetSellTypeinfo(Convert.ToUInt32(dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name));
                bool success = new ItemINSellPriceSql(DB).UNSetBuyOPRPrice (_AssemblabgeOPR._ItemIN,  Selected_ConsumeUnit, Selected_SellType);
                if (success)
                {
                    ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_AssemblabgeOPR._ItemIN);
                    FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);
                }
            }
        }

        private void setprice_MenuItem_Click(object sender, EventArgs e)
        {
            ComboboxItem comboboxitem_curency = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency _Currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(comboboxitem_curency.Value);
            ConsumeUnit Selected_ConsumeUnit = null;
            for (int i = 0; i < ConsumUnitsList.Count; i++)
            {
                if (ConsumUnitsList[i].ConsumeUnitName.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].HeaderCell.Value))
                {
                    Selected_ConsumeUnit = ConsumUnitsList[i];
                    break;
                }
            }
            ComboboxItem tradestate_item = (ComboboxItem)comboBoxTradestate.SelectedItem;
            TradeState TradeState_ = new TradeState(tradestate_item.Value, tradestate_item.Text);
            SellType Selected_SellType = new SellTypeSql(DB).GetSellTypeinfo(Convert.ToUInt32(dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name));
            Trade.Forms.PriceInputBox inp = new Trade.Forms.PriceInputBox("ضبط السعر", Selected_SellType.SellTypeName, TradeState_.TradeStateName, Selected_ConsumeUnit.ConsumeUnitName, _Currency.CurrencyName, "");
            inp.ShowDialog();
            if (inp.DialogResult == DialogResult.OK)
            {
                try
                {
                    double price = Convert.ToDouble(inp.Price);
                    if (price < 0)
                    {
                        MessageBox.Show("السعر يجب ان يكون اكبر من الصفر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    new ItemINSellPriceSql(DB).SetItemINPrice(_AssemblabgeOPR._ItemIN, Selected_ConsumeUnit, Selected_SellType, price / _Currency.ExchangeRate );
                    ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_AssemblabgeOPR._ItemIN);
                    FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

   
        private void DeleteItemOUT_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listView.SelectedItems[0].Name);
                bool success = new ItemOUTSQL(DB).DeleteItemOUT(sid);
                if (success)
                {
                    this._Changed = true;
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_AssemblabgeOPR._Operation);
                    RefreshAssemblageItemOutLIst(_ItemOUTList);

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

        private void EditItemOUT_MenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                uint itemoutid = Convert.ToUInt32(listView.SelectedItems[0].Name);
                ItemOUT ItemOUT_ = new ItemOUTSQL(DB).GetItemOUTINFO_BYID(itemoutid);
                ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, ItemOUT_, true);
                ItemOUTForm_.ShowDialog();
                if (ItemOUTForm_.Changed)
                {
                    this._Changed = true;
                    _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_AssemblabgeOPR._Operation);
                    RefreshAssemblageItemOutLIst(_ItemOUTList);
                }
                ItemOUTForm_.Dispose();
            }
        }
        private void OpenItemOUT_MenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {

                uint itemoutid = Convert.ToUInt32(listView.SelectedItems[0].Name);
                ItemOUT ItemOUT_ = new ItemOUTSQL(DB).GetItemOUTINFO_BYID(itemoutid);
                ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, ItemOUT_, false);
                ItemOUTForm_.ShowDialog();
                if (ItemOUTForm_.Changed)
                {
                    this._Changed = true;
                    _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_AssemblabgeOPR._Operation);
                    RefreshAssemblageItemOutLIst(_ItemOUTList);
                }
                ItemOUTForm_.Dispose();
            }
        }

        private void AddItemOUT_MenuItem_Click(object sender, EventArgs e)
        {
            ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, _AssemblabgeOPR._Operation);
            DialogResult d = ItemOUTForm_.ShowDialog();
            if (ItemOUTForm_.Changed)
            {
                this._Changed = true;
                _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_AssemblabgeOPR._Operation);
                RefreshAssemblageItemOutLIst(_ItemOUTList);
            }
            ItemOUTForm_.Dispose();
        }
        private void RefreshAssemblageItemOutLIst(List<ItemOUT> ItemOUTList)
        {

            listView.Items.Clear();
            ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);

           



            //double totalcost = 0;


            //for (int i = 0; i < ItemOUTList.Count; i++)
            //{
            //    double sellprice = ItemOUTList[i].Cost ;
            //    double total_sellprice = System.Math.Round(sellprice * ItemOUTList[i].Amount, 3);
            //    totalcost = totalcost + total_sellprice;
            //    ListViewItem ListViewItem_ = new ListViewItem((listView.Items.Count + 1).ToString());
            //    ListViewItem_.Name = ItemOUTList[i].ItemOUTID.ToString();
            //    ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.ItemName);
            //    ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.ItemCompany);
            //    ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.folder.FolderName);
            //    ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._TradeState.TradeStateName);
            //    ListViewItem_.SubItems.Add(ItemOUTList[i].Amount.ToString());
            //    ListViewItem_.SubItems.Add(ItemOUTList[i]._ConsumeUnit.ConsumeUnitName.ToString());


            //    ListViewItem_.SubItems.Add(sellprice.ToString() + " " + currency.CurrencySymbol.Replace(" ", string.Empty));
            //    ListViewItem_.SubItems.Add((total_sellprice).ToString() + " " + currency.CurrencySymbol.Replace(" ", string.Empty));
            //    ListViewItem_.SubItems.Add(ItemOUTList[i].Notes);
            //    ListViewItem_.BackColor = Color.Orange;
            //    listView.Items.Add(ListViewItem_);

            //}

            // textBoxtotalcost .Text = totalcost.ToString() + " " + currency.CurrencySymbol.Replace(" ", string.Empty);

        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                try
                {
                    uint placeid = Convert.ToUInt32(textBoxPlaceID.Text);
                    TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(placeid);
                    if (place != null)
                    {

                        textBoxPlaceInfo.Text = place.GetPlaceInfo();
                        textBoxPlaceID.Text = place.PlaceID.ToString();
                        textBoxPlaceID.Name  = place.PlaceID.ToString();
                        buttonClearstoreinfo.Visible = true;
                        buttonStore.Visible = true ;
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
                container = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert .ToUInt32 (textBoxPlaceID .Name ))._TradeStoreContainer;
            }catch
            {
                container = null;
            }

            Container.ShowLocations frm = new Container.ShowLocations(DB, container, true);
            DialogResult dd = frm.ShowDialog();
            if (dd == DialogResult.OK)
            {
                 TradeStorePlace place = frm.ReturnPlace;
                textBoxPlaceInfo.Text = place.GetPlaceInfo();
                textBoxPlaceID.Text = place.PlaceID.ToString();
                textBoxPlaceID.Name = place.PlaceID.ToString();
                buttonStore.Visible = true;
                buttonClearstoreinfo.Visible = true;

            }
            frm.Dispose();
        }
        private void buttonClearstoreinfo_Click(object sender, EventArgs e)
        {
            buttonClearstoreinfo.Visible = false ;
            textBoxPlaceID.Name = "textBoxPlaceID";
            LoadStoreData();
        }
        private void listView_Resize(object sender, EventArgs e)
        {
            AdjustListViewColumnsWidth();
        }
        public void AdjustListViewColumnsWidth()
        {
            listView.Columns[0].Width = 80;

            listView.Columns[1].Width = (listView.Width - 80) / 8 - 1;
            listView.Columns[2].Width = (listView.Width - 80) / 8 - 1;
            listView.Columns[3].Width = (listView.Width - 80) / 8 - 1;
            listView.Columns[4].Width = (listView.Width - 80) / 8 - 1;
            listView.Columns[5].Width = (listView.Width - 80) / 8 - 1;
            listView.Columns[6].Width = (listView.Width - 80) / 8 - 1;
            listView.Columns[7].Width = (listView.Width - 80) / 8 - 1;
            listView.Columns[8].Width = (listView.Width - 80) / 8 - 1;


        }

        private void عرضالعناصرالخارجةمنالعنصرالناتجعنهذهالعمليةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemIN_ItemOutListForm ItemIN_ItemOutListForm_ = new ItemIN_ItemOutListForm(DB, _AssemblabgeOPR._ItemIN);
            ItemIN_ItemOutListForm_.ShowDialog();
        }
    }
}