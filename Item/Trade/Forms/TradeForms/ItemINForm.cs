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
    public partial class ItemINForm : Form
    {
        System.Windows.Forms.MenuItem OpenPlaceDetailsMenuItem;
        System.Windows.Forms.MenuItem UnStoreAmountMenuItem;
        System.Windows.Forms.MenuItem EditStoreAMountMenuItem;

        System.Windows.Forms.MenuItem setPriceMenuItem;
        System.Windows.Forms.MenuItem UnsetpriceMenuItem;

        System.Windows.Forms.MenuItem DisAssemblageMenuItem;
       
        List<SellType> selltypelist = new List<SellType>();
        List<TradeState> TradeStateList = new List<TradeState>();
        List<ItemINSellPrice> ItemINSellPriceList = new List<ItemINSellPrice>();
        List<ConsumeUnit> ConsumUnitsList = new List<ConsumeUnit>();
        DatabaseInterface DB;
        ItemIN _ItemIN;
        Operation _Operation;
        Currency _Currency;
         Item _Item;
        Folder LastUsedFolder;
        private bool Changed_;
        public bool Changed
        {
            get { return Changed_; }
        }
        //bool Edit;
        bool GetPlace;
        private TradeStorePlace  _ReturnPlace;
        public TradeStorePlace ReturnPlace
        {
            get
            {
                return _ReturnPlace;
            }
        }
        TradeStorePlace _TempStorePlace;
        public ItemINForm(DatabaseInterface db, Operation Operation_)
        {
            InitializeComponent();
            DB = db;
            OpenPlaceDetailsMenuItem = new System.Windows.Forms.MenuItem("عرض تفاصيل مكان التخزين", OpenPlaceDetails_MenuItem_Click);
            UnStoreAmountMenuItem = new System.Windows.Forms.MenuItem("الغاء تخزين ", UnStoreAmount_MenuItem_Click);
            EditStoreAMountMenuItem = new System.Windows.Forms.MenuItem("تعديل الكمية", EditStoreAMount_MenuItem_Click);
            setPriceMenuItem = new System.Windows.Forms.MenuItem("ضبط السعر", setprice_MenuItem_Click);
            UnsetpriceMenuItem = new System.Windows.Forms.MenuItem("الغاء ضبط السعر", Unsetprice_MenuItem_Click);
            DisAssemblageMenuItem  = new System.Windows.Forms.MenuItem("تفكيك عنصر", DisAssemblage_MenuItem_Click);


            TradeStateList = new TradeStateSQL(DB).GetTradeStateList();
            selltypelist = new SellTypeSql(DB).GetSellTypeList();

            _Operation = Operation_;
            _Currency = new OperationSQL(DB).GetOperationItemINCurrency(_Operation);
            LastUsedFolder = null;
            panelStoreAmount.Enabled = false;
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انشاء";

            textBoxExchangeRate.Text = _Currency.ExchangeRate  .ToString();
            textBoxCurrency.Text = _Currency.CurrencyName;
            FillComboBoxTradeState(null);
            comboBoxConsumeUnt.Enabled = false;
            textBoxAmount.Text = "0";
            textBoxBuyPrice.Text = "0";
            InitialzeDataGridViewItemSellPrices();
           
        }

         public ItemINForm(DatabaseInterface db,  ItemIN ItemIN_, bool? Edit_)
        {
            
            InitializeComponent();
            GetPlace = false;
            DB = db;
            TradeStateList = new TradeStateSQL(DB).GetTradeStateList();
            selltypelist = new SellTypeSql(DB).GetSellTypeList();
            InitialzeDataGridViewItemSellPrices();

            _ItemIN = ItemIN_;
            _Currency = new OperationSQL(DB).GetOperationItemINCurrency(_ItemIN._Operation) ;
            _Operation = _ItemIN._Operation;
            _Item = _ItemIN._Item;
            OpenPlaceDetailsMenuItem = new System.Windows.Forms.MenuItem("عرض تفاصيل مكان التخزين", OpenPlaceDetails_MenuItem_Click);
            UnStoreAmountMenuItem = new System.Windows.Forms.MenuItem("الغاء تخزين ", UnStoreAmount_MenuItem_Click);
            EditStoreAMountMenuItem = new System.Windows.Forms.MenuItem("تعديل الكمية", EditStoreAMount_MenuItem_Click); ;
            setPriceMenuItem = new System.Windows.Forms.MenuItem("ضبط السعر", setprice_MenuItem_Click);
            UnsetpriceMenuItem = new System.Windows.Forms.MenuItem("الغاء ضبط السعر", Unsetprice_MenuItem_Click);
            DisAssemblageMenuItem = new System.Windows.Forms.MenuItem("تفكيك عنصر", DisAssemblage_MenuItem_Click);
   

            
            textBoxExchangeRate.Text = _Currency .ExchangeRate .ToString();
            textBoxCurrency.Text =_Currency.CurrencyName;
            FillComboBoxConsumeUnit(_ItemIN ._ConsumeUnit);
            FillComboBoxTradeState (_ItemIN ._TradeState );
            LoadForm(Edit_);
 
            


        }
        public ItemINForm(DatabaseInterface db, ItemIN ItemIN_, TradeStorePlace Place)
        {
            InitializeComponent();
            panelHeader.Enabled = false;
            DB = db;
            
            GetPlace = true;
            TradeStateList = new TradeStateSQL(DB).GetTradeStateList();
            selltypelist = new SellTypeSql(DB).GetSellTypeList();
            InitialzeDataGridViewItemSellPrices();

            OpenPlaceDetailsMenuItem = new System.Windows.Forms.MenuItem("عرض تفاصيل مكان التخزين", OpenPlaceDetails_MenuItem_Click);
            UnStoreAmountMenuItem = new System.Windows.Forms.MenuItem("الغاء تخزين ", UnStoreAmount_MenuItem_Click);
            EditStoreAMountMenuItem = new System.Windows.Forms.MenuItem("تعديل الكمية", EditStoreAMount_MenuItem_Click); ;
            setPriceMenuItem = new System.Windows.Forms.MenuItem("ضبط السعر", setprice_MenuItem_Click);
            UnsetpriceMenuItem = new System.Windows.Forms.MenuItem("الغاء ضبط السعر", Unsetprice_MenuItem_Click);
            DisAssemblageMenuItem = new System.Windows.Forms.MenuItem("تفكيك عنصر", DisAssemblage_MenuItem_Click);

            labelItemInfo.Text = "معلومات العنصر";
            textBoxItemID.ReadOnly = true;
            
            _ItemIN = ItemIN_;
            _Operation  = _ItemIN ._Operation ;
            _Item = _ItemIN._Item;
            _Currency = new OperationSQL(DB).GetOperationItemINCurrency(_Operation);
            textBoxExchangeRate.Text = _Currency .ExchangeRate.ToString();
            textBoxCurrency.Text = _Currency.CurrencyName;
            FillComboBoxConsumeUnit(_ItemIN._ConsumeUnit);
            FillComboBoxTradeState(_ItemIN._TradeState);
            LoadForm(false );




        }
        public void LoadForm(bool? Edit)
        {

            

            _Item = _ItemIN ._Item;
            LastUsedFolder = _Item.folder;
            textBoxItemID.ReadOnly = true;
            comboBoxTradestate.Enabled = false;
            textBoxAmount.Text = _ItemIN .Amount.ToString();
            textBoxBuyPrice.Text = _ItemIN ._INCost.Value.ToString();
            LoadItemData();
            if(Edit ==null )
            {
                  buttonSave.Visible = false;
                panelStoreAmount.Visible  = false ;
                textBoxItemID.ReadOnly = true;
                textBoxAmount.ReadOnly = true;
                textBoxBuyPrice.ReadOnly = true;
                comboBoxConsumeUnt.Enabled = false;
                comboBoxTradestate.Enabled = false;
                TextboxNotes.ReadOnly = true;
            }
            else if(Edit==true   )
            {
                buttonSave.Name = "buttonSave";
                buttonSave.Text = "حفظ";
                this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
                this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
                this.textBoxAmount.TextChanged += new System.EventHandler(this.RefreshTotalCost_TextChanged);
                this.textBoxBuyPrice.TextChanged += new System.EventHandler(this.RefreshTotalCost_TextChanged);
                this.textBoxItemID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxItemID_KeyDown);
                this.textBoxItemID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxItem_MouseDoubleClick);
                panelStoreAmount.Enabled = false;
            }
            else
            {
                labelItemInfo.Text = "معلومات العنصر";
                textBoxItemID.ReadOnly = true;
                if (GetPlace )
                {
                    buttonSave.Name = "buttonGetPlace";
                    buttonSave.Text = "اختيار";
                    buttonSave.Visible = true ;
                }
                else buttonSave.Visible = false;

                this.textBoxPlaceID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
                this.textBoxPlaceID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxPlace_MouseDoubleClick);
                this.buttonStore.Click += new System.EventHandler(this.buttonStore_Click);
                //this.listViewItemStorePlace.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
                //this.listViewItemStorePlace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDown);
                panelStoreAmount.Enabled = true;
                textBoxItemID.ReadOnly = true;
                textBoxAmount.ReadOnly = true;
                textBoxBuyPrice.ReadOnly = true;
                comboBoxConsumeUnt.Enabled = false;
                comboBoxTradestate.Enabled = false;
                TextboxNotes.ReadOnly = true;
                

            }
            ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_ItemIN);

            FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);
           
            RefreshItemStorePlaces();


        }
        private void LoadItemData()
        {
            textBoxItemID.Text = _Item.ItemID.ToString();
            textBoxItemName.Text = _Item.ItemName;
            textBoxItemCompany.Text = _Item.ItemCompany;
            textBoxItemType.Text = _Item.folder.FolderName;
            FillComboBoxConsumeUnit(null);
            ConsumUnitsList = new ConsumeUnitSql(DB).GetConsumeUnitList(_Item);
            
        }
        public void FillItemConsumeUnitsAndSellPrices(List<ItemINSellPrice> ItemINSellPriceList)
        {
            if (_ItemIN == null)
            {
                panelItemINSellPrices.Enabled = false;
                panelItemINSellsProfit.Enabled = false;
                return;

            }
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

                            List<ItemINSellPrice> dd = ItemINSellPriceList.Where(x => (x._ItemIN._Operation .OperationID  == _ItemIN._Operation.OperationID && x.SellType_.SellTypeID == selltypelist[j].SellTypeID && x.ConsumeUnit_.ConsumeUnitID == ConsumUnitsList[i].ConsumeUnitID)).ToList();
                            if (dd.Count != 1) dataGridView1.Rows[i].Cells[j].Value = " - " + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);
                            else
                            {
                                double sellprice = System.Math.Round(Convert.ToDouble(dd[0].Price) * _Currency .ExchangeRate ,3) ;
                                dataGridView1.Rows[i].Cells[j].Value = sellprice .ToString() + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);
                                try
                                {
                                    double buyprice = Convert.ToDouble(textBoxBuyPrice.Text );
                                    double buyamount = Convert.ToDouble(textBoxAmount .Text);
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
      
        private void FillComboBoxConsumeUnit(ConsumeUnit  consumeunit)
        {
            if (_Item == null) return;
            comboBoxConsumeUnt .Items.Clear();
            comboBoxStoreConsumeUnit.Items.Clear();
            int selected_index = 0;
            try
            {
                List<ConsumeUnit > ConsumeUnitList = new ConsumeUnitSql (DB).GetConsumeUnitList(_Item );
                for (int i = 0; i < ConsumeUnitList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(ConsumeUnitList[i].ConsumeUnitName , ConsumeUnitList[i].ConsumeUnitID);
                    comboBoxConsumeUnt.Items.Add(item);
                    comboBoxStoreConsumeUnit.Items.Add(item);
                    if (consumeunit != null && consumeunit.ConsumeUnitID == ConsumeUnitList[i].ConsumeUnitID) selected_index = i;
                }
                comboBoxConsumeUnt.SelectedIndex = selected_index;
                comboBoxStoreConsumeUnit.SelectedIndex = 0;
                comboBoxConsumeUnt.Enabled = true;
            }
            catch
            { }

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
            if (buttonSave.Name == "buttonGetPlace")
            { SetReturnPlace(); return; }
            ComboboxItem consumeunititem = (ComboboxItem)comboBoxConsumeUnt .SelectedItem;
            ConsumeUnit _ConsumeUnit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunititem.Value);
            ComboboxItem tradestateitem = (ComboboxItem)comboBoxTradestate .SelectedItem;
            TradeState tradestate = new TradeStateSQL(DB).GetTradeStateBYID(tradestateitem.Value);
            if (buttonSave.Name == "buttonAdd")
            {
                if (_Item  == null)
                {
                    MessageBox.Show("يرجى تحديد العنصر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ItemIN itemin = new ItemINSQL   (DB).AddItemIN  (_Operation  ,_Item , tradestate 
                    , Convert.ToDouble (textBoxAmount .Text ), _ConsumeUnit, Convert.ToDouble (textBoxBuyPrice.Text)
                    ,TextboxNotes .Text  );
                if (itemin  != null)
                {
                    _ItemIN = itemin ;
                    MessageBox.Show("تم الاضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Changed_  = true;
                    LoadForm(false );

                }
            }

            else
            {

                if (_Operation  != null)
                {
                    if (_Item == null)
                    {
                        MessageBox.Show("يرجى تحديد العنصر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    bool success = new ItemINSQL (DB).UpdateItemIN (_ItemIN .ItemINID ,_Item, tradestate,
                        Convert.ToDouble (textBoxAmount.Text), _ConsumeUnit, Convert.ToDouble(textBoxBuyPrice.Text)
                    , TextboxNotes.Text);
                    if (success == true)
                    {
                        MessageBox.Show("تم حفظ  بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _ItemIN  = new ItemINSQL(DB).GetItemININFO_BYID(_ItemIN .ItemINID );
                        this.Changed_ = true;
                        this.Close();
                    }
                    else MessageBox.Show("لم يتم الحفظ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error );

                }
            }
        }
         private async  void RefreshTotalCost_TextChanged(object sender, EventArgs e)
        {
            double amount, buyprice;

            try
            {
                amount = Convert.ToDouble(textBoxAmount.Text);
                buyprice = Convert.ToDouble(textBoxBuyPrice.Text);

                TextboxTotalValue.Text = (amount * buyprice).ToString() + " " + _Currency.CurrencySymbol;
                FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList  );
            }
            catch { TextboxTotalValue.Text = " -- " + _Currency .CurrencySymbol; }
        }
        private async  void dataGridView1_Resize(object sender, EventArgs e)
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
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                try
                {
                    uint placeid = Convert.ToUInt32(textBoxPlaceID .Text);
                    TradeStorePlace  place = new TradeStorePlaceSQL (DB).GetTradeStorePlaceBYID (placeid);
                    if (place != null)
                    {

                        _TempStorePlace = place;
                        textBoxPlaceInfo.Text = _TempStorePlace.GetPlaceInfo();
                        textBoxPlaceID.Text = _TempStorePlace.PlaceID.ToString();
                        buttonClearStoreData.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على مكان التخزين", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message );
                    MessageBox.Show("يرجى ادخال عدد صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }
        
        private void textBoxPlace_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TradeStoreContainer container=null ;
            if (_TempStorePlace != null) container = _TempStorePlace._TradeStoreContainer;
            Container.ShowLocations frm = new Container.ShowLocations(DB, container, true);
            DialogResult dd= frm.ShowDialog();
            MessageBox.Show(dd.ToString());
            if(dd==DialogResult.OK)
            {
                _TempStorePlace = frm.ReturnPlace;
                textBoxPlaceInfo.Text = _TempStorePlace.GetPlaceInfo();
                textBoxPlaceID.Text = _TempStorePlace.PlaceID.ToString();
                buttonClearStoreData.Visible = true;
            }
        }
        private void buttonStore_Click(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem consumeunititem = (ComboboxItem)comboBoxConsumeUnt.SelectedItem;
                ConsumeUnit _ConsumeUnit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunititem.Value);

                double store_amount;
                try
                {
                   
                    store_amount = Convert.ToDouble(textBoxStoreAmount.Text);
                }catch
                {
                    MessageBox.Show("الكمية يجب ان تكون رقم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(_TempStorePlace ==null )
                {
                    MessageBox.Show("يرجى تحديد مكان التخزين", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bool success= new TradeItemStoreSQL(DB).Store_Item_INPlace( _TempStorePlace.PlaceID , _ItemIN.ItemINID,TradeItemStore.ITEMIN_STORE_TYPE, store_amount, _ConsumeUnit);
                if (success)
                {
                    RefreshItemStorePlaces();
                    textBoxStoreAmount.Text = "";  textBoxPlaceID.Text = "";textBoxPlaceInfo.Text = "";
                    buttonClearStoreData.Visible = false;
                }
           }
            catch
            {

            }
         
        }
        private void RefreshItemStorePlaces()
        {

            try
            {
                listViewItemStorePlace.Items.Clear();
                AvailableItemSQL AvailableItemSQL_ = new AvailableItemSQL(DB);
                double nonsotred = new TradeItemStoreSQL(DB).getNON_StoredAmount(_ItemIN);
                if (nonsotred > 0)
                {
                    panelStoreHeader.Enabled = true;
                    textBoxStoreAmount.Text = nonsotred.ToString();
                    ListViewItem ListViewItem_ = new ListViewItem();
                    ListViewItem_.Name = "null";
                    ListViewItem_.SubItems.Add("غير مخزن");
                    ListViewItem_.SubItems.Add(_ItemIN._ConsumeUnit.ConsumeUnitName);
                    ListViewItem_.SubItems.Add(nonsotred.ToString());
                    ListViewItem_.SubItems.Add(AvailableItemSQL_.GetSpentAmount_by_Place  (_ItemIN,null ).ToString ());
                    ListViewItem_.BackColor = Color.Orange;
                    listViewItemStorePlace.Items.Add(ListViewItem_);
                }
                else panelStoreHeader.Enabled = false;
                List<TradeItemStore> TradeItemStoreList = new List<TradeItemStore>();
                TradeItemStoreList = new TradeItemStoreSQL(DB).GetItemStoredPlaces(_ItemIN.ItemINID);
                for (int i = 0; i < TradeItemStoreList.Count; i++)
                {
                    ListViewItem ListViewItem_ = new ListViewItem(TradeItemStoreList[i]._TradeStorePlace.PlaceID.ToString());
                    ListViewItem_.Name = TradeItemStoreList[i]._TradeStorePlace.PlaceID.ToString();
                    ListViewItem_.SubItems.Add(TradeItemStoreList[i]._TradeStorePlace.GetPlaceInfo());
                    ListViewItem_.SubItems.Add(_ItemIN ._ConsumeUnit.ConsumeUnitName);
                    ListViewItem_.SubItems.Add(TradeItemStoreList[i].Amount.ToString());
                    ListViewItem_.SubItems.Add(AvailableItemSQL_.GetSpentAmount_by_Place(_ItemIN, TradeItemStoreList[i]._TradeStorePlace ).ToString ());

                    ListViewItem_.BackColor = Color.LimeGreen;
                    listViewItemStorePlace.Items.Add(ListViewItem_);

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("RefreshItemStorePlaces" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewItemStorePlace.SelectedItems.Count > 0)
            {
                if (!GetPlace)
                    OpenPlaceDetailsMenuItem.PerformClick();
                else
                    SetReturnPlace();

            }
        }
        private void SetReturnPlace()
        {
            if(listViewItemStorePlace .SelectedItems .Count ==1)
            {
                try
                {
                    if (listViewItemStorePlace.SelectedItems[0].Name == "null") 
                    _ReturnPlace = null;
                    else 
                    _ReturnPlace = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(listViewItemStorePlace.SelectedItems[0].Name) );

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("","",MessageBoxButtons.OK,MessageBoxIcon.Error );
                }
               
            }
        }
        private void listView_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {

                listViewItemStorePlace.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                foreach (ListViewItem item1 in listViewItemStorePlace.Items)
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

                    uint placeid;
                    MenuItem[] mi;
                    try
                    {
                        placeid = Convert.ToUInt32(listitem.Name);
                         mi = new MenuItem[] { OpenPlaceDetailsMenuItem, EditStoreAMountMenuItem, UnStoreAmountMenuItem,new MenuItem ("-"),DisAssemblageMenuItem  };
                    }
                    catch
                    {
                         mi = new MenuItem[] { DisAssemblageMenuItem  };
                    }

                    listViewItemStorePlace.ContextMenu = new ContextMenu(mi);


                }
                else
                {

                    //MenuItem[] mi = new MenuItem[] { AddBuyOprMenuItem };
                    //listViewItemStorePlace.ContextMenu = new ContextMenu(mi);

                }

            }
        }
        private void EditStoreAMount_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewItemStorePlace.SelectedItems.Count > 0)
                {
                    uint placeid;
                    try
                    {
                        placeid = Convert.ToUInt32(listViewItemStorePlace.SelectedItems[0].Name);
                    }
                    catch
                    {
                        return;
                    }

                    TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(placeid);
                    ItemProject.Forms.InputBox inp = new ItemProject.Forms.InputBox("تعديل كمية التخزين", "أدخل الكمية الجديدة");
                    DialogResult dd = inp.ShowDialog();
                    if (dd == DialogResult.OK)
                    {
                        double storeamount;
                        try
                        {
                            storeamount = Convert.ToDouble(inp.textBox1.Text);
                            bool success = new TradeItemStoreSQL(DB).UpdateItemAmountStored( place.PlaceID , _ItemIN.ItemINID, TradeItemStore.ITEMIN_STORE_TYPE, storeamount);
                            if (success) RefreshItemStorePlaces();
                        }
                        catch
                        {
                            MessageBox.Show("الكمية يجب ان تكون رقم حقيقي", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }


                }
            }
            catch
            {
                MessageBox.Show("فشل تعديل الكمية", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void UnStoreAmount_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewItemStorePlace.SelectedItems.Count > 0)
                {
                    uint placeid;
                    try
                    {
                        placeid = Convert.ToUInt32(listViewItemStorePlace.SelectedItems[0].Name);
                    }
                    catch
                    {
                        return;
                    }
                    DialogResult dd = MessageBox.Show("متأكد من الغاء التخزين", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dd == DialogResult.OK)
                    {
                        TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(placeid);
                        bool success = new TradeItemStoreSQL(DB).UNStore_Item_INPlace( place.PlaceID, _ItemIN.ItemINID, TradeItemStore.ITEMIN_STORE_TYPE);
                        if (success) RefreshItemStorePlaces();
                    }



                }
            }
            catch
            {
                MessageBox.Show("فشل تعديل الكمية", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void OpenPlaceDetails_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewItemStorePlace.SelectedItems.Count > 0)
                {
                    uint placeid;
                    try
                    {
                        placeid = Convert.ToUInt32(listViewItemStorePlace.SelectedItems[0].Name);
                    }
                    catch
                    {
                        return;
                    }
                    TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(placeid);

                    Container.PlaceItemsForm PlaceItemsForm_ = new Forms.Container.PlaceItemsForm(DB, place, false);
                    PlaceItemsForm_.ShowDialog();


                }
            }
            catch
            {
                MessageBox.Show("فشل تعديل الكمية", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void DisAssemblage_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TradeStorePlace place;
                try
                {

                    uint placeid = Convert.ToUInt32(listViewItemStorePlace.SelectedItems[0].Name);
                    place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(placeid);
                }
                catch
                {
                    place = null;
                }

                DisAssemblageForm DisAssemblageForm_ = new DisAssemblageForm(DB, _ItemIN,place );
                DisAssemblageForm_.ShowDialog();
            }
            catch
            {

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
                List <ItemINSellPrice> ff= ItemINSellPriceList.Where(x => (x._ItemIN.ItemINID == _ItemIN.ItemINID && x.SellType_.SellTypeID == Selected_SellType.SellTypeID && x.ConsumeUnit_.ConsumeUnitID == Selected_ConsumeUnit.ConsumeUnitID)).ToList();
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

                    new ItemINSellPriceSql(DB).SetItemINPrice(_ItemIN, SelectedConsumeUnit, SelectedSellType, price / _Currency .ExchangeRate );
                    ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_ItemIN);
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

        private void BuyOprForm_Load(object sender, EventArgs e)
        {
    
           
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
                bool success = new ItemINSellPriceSql(DB).UNSetBuyOPRPrice (_ItemIN,  Selected_ConsumeUnit, Selected_SellType);
                if (success)
                {
                    ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_ItemIN);
                    FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);
                }
            }
        }

        private void setprice_MenuItem_Click(object sender, EventArgs e)
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

                    new ItemINSellPriceSql(DB).SetItemINPrice(_ItemIN, Selected_ConsumeUnit, Selected_SellType, price / _Currency.ExchangeRate );
                    ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_ItemIN);
                    FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonClearStoreData_Click(object sender, EventArgs e)
        {
            _TempStorePlace = null;
            textBoxPlaceInfo.Text = "";
            textBoxPlaceID.Text = "";
            buttonClearStoreData.Visible = false;
        }

        private void buttonShowItemsOutReport_Click(object sender, EventArgs e)
        {
            try
            {
                ItemIN_ItemOutListForm ItemIN_ItemOutListForm_ = new ItemIN_ItemOutListForm(DB, _ItemIN);
                ItemIN_ItemOutListForm_.ShowDialog();
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("عرض تقرير العناصر الخارجة:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }


    }
}