using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.Forms;
using ItemProject.ItemObj.ItemObjSQL;
using ItemProject.ItemObj.Objects;
using ItemProject.Trade.Forms.Container;
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
    public partial class ItemOUTForm : Form
    {

        List<SellType> selltypelist = new List<SellType>();
        List<ItemINSellPrice> ItemINSellPriceList = new List<ItemINSellPrice>();
        List<ConsumeUnit> ConsumUnitsList = new List<ConsumeUnit>();
        DatabaseInterface DB;
        ItemOUT _ItemOUT;
        Operation _Operation;
        Currency _Currency;

        Folder LastUsedFolder;
        private bool Changed_;

        ItemIN _TempItemIN;
        TradeStorePlace _TempPlace;
        public bool Changed
        {
            get { return Changed_; }
        }

        //SellType SelectedSellType;
        //ConsumeUnit SelectedConsumeUnit;

        public ItemOUTForm(DatabaseInterface db, Operation Operation_)
        {
            DB = db;
            InitializeComponent();
            selltypelist = new SellTypeSql(DB).GetSellTypeList();

            InitialzeDataGridViewItemSellPrices();
            LabelOperationInfo.Text = Operation.GetOperationItemOutDesc(Operation_.OperationType);

            _Operation = Operation_;
            LastUsedFolder = null;
            PanelSellInfo .Enabled = false;
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انشاء";
            _Currency = new OperationSQL(DB).GetOperationItemOUTCurrency(_Operation);
            


            textBoxExchangeRate.Text = _Currency.ExchangeRate.ToString();
            textBoxCurrency.Text = _Currency.CurrencyName;
            textBoxAmount.Text = "1";
            

        }
        public ItemOUTForm(DatabaseInterface db, ItemOUT ItemOUT_, bool Edit)
        {
            DB = db;
            InitializeComponent();
            selltypelist = new SellTypeSql(DB).GetSellTypeList();
            InitialzeDataGridViewItemSellPrices();

            _ItemOUT = ItemOUT_;
            _Operation  = _ItemOUT._Operation  ;

            LabelOperationInfo.Text = Operation.GetOperationItemOutDesc(_Operation.OperationType);
            _Currency = new OperationSQL(DB).GetOperationItemOUTCurrency(_Operation );
            textBoxExchangeRate.Text = _Currency .ExchangeRate.ToString();
            textBoxCurrency.Text = _Currency.CurrencyName;
            _TempItemIN = _ItemOUT._ItemIN;
            _TempPlace = _ItemOUT.Place;
            LoadForm(Edit );
            

        }
        //public void InitialzeDataGridViewItemSellPrices()
        //{
        //    //    try
        //    //    {

        //    //        SellTypeSql STS = new SellTypeSql(DB);
        //    //        selltypelist = STS.GetSellTypeList();
        //    //        for (int i = 0; i < selltypelist.Count; i++)
        //    //        {
        //    //            dataGridView1.Columns.Add(selltypelist[i].SellTypeID.ToString(), selltypelist[i].SellTypeName);
        //    //        }

        //    //        AdjustmentDatagridviewColumnsWidth();
        //    //        FillItemConsumeUnitsAndSellPrices();
        //    //    }

        //    //    catch
        //    //    {
        //    //        MessageBox.Show("فشل في جلب قائمة انماط البيع", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //    }
        //}

       
        private void FillComboBoxConsumeUnit(ConsumeUnit consumeunit)
        {
            if (_TempItemIN == null) return;
            comboBoxConsumeUnt .Items.Clear();
            int selected_index = 0;
            try
            {
                for (int i = 0; i < ConsumUnitsList .Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(ConsumUnitsList[i].ConsumeUnitName, ConsumUnitsList[i].ConsumeUnitID);
                    comboBoxConsumeUnt.Items.Add(item);
                    if (consumeunit != null && consumeunit.ConsumeUnitID == ConsumUnitsList[i].ConsumeUnitID) selected_index = i;
                }
                comboBoxConsumeUnt.SelectedIndex = selected_index;
            }
            catch
            { }

        }
       
        public void RefreshComponents()
        {
            try
            {
                if (_TempItemIN == null) return;
                PanelSellInfo.Enabled = true;
                ConsumeUnitSql CUS = new ConsumeUnitSql(DB);
                ConsumUnitsList = CUS.GetConsumeUnitList(_TempItemIN._Item);
                LastUsedFolder = _TempItemIN._Item.folder;
                textBoxItemState.Text = _TempItemIN._TradeState.TradeStateName;
                textBoxItemID.Text = _TempItemIN._Item.ItemID.ToString();
                textBoxItemName.Text = _TempItemIN._Item.ItemName;
                textBoxItemCompany.Text = _TempItemIN._Item.ItemCompany;
                textBoxItemType.Text = _TempItemIN._Item.folder.FolderName;

                textBoxOperationID.Text = _TempItemIN._Operation.OperationID.ToString();
                textBoxOperationType.Text =Operation .GetOperationName( _TempItemIN._Operation.OperationType);
                textBoxItemINID.Text = _TempItemIN.ItemINID.ToString();
                if (_TempPlace != null)
                    textBoxStorePlace.Text = _TempPlace.GetPlaceInfo();
                else
                    textBoxStorePlace.Text = "-";

                textBoxAvailableAmount.Text = new AvailableItemSQL(DB).GetAvailabeAmount_by_Place(_TempItemIN, _TempPlace).ToString();
                FillComboBoxConsumeUnit(null);

                ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_TempItemIN );

                FillItemConsumeUnitsAndSellPrices(ItemINSellPriceList);


            }
            catch (Exception ee)
            {
                MessageBox.Show("RefreshComponents:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }
        public async  void InitialzeDataGridViewItemSellPrices()
        {
            try
            {
                for (int i = 0; i < selltypelist.Count; i++)
                {
                    dataGridView1.Columns.Add(selltypelist[i].SellTypeID.ToString(), selltypelist[i].SellTypeName);

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
        public async void AdjustmentDatagridviewColumnsWidth()
        {
            int columnscount = dataGridView1.Columns.Count + 1;
            dataGridView1.RowHeadersWidth = dataGridView1.Width / columnscount;
            for (int i = 0; i < columnscount - 1; i++)
            {
                dataGridView1.Columns[i].Width = (dataGridView1.Width - 5) / columnscount;
            }

        }
        public void FillItemConsumeUnitsAndSellPrices(List<ItemINSellPrice> ItemINSellPriceList)
        {
            if (_TempItemIN  == null)
            {
                panelItemINSellPrices.Enabled = false;
                return;

            }
            panelItemINSellPrices.Enabled = true;
            dataGridView1.TopLeftHeaderCell.Value = _TempItemIN._TradeState .TradeStateName;

            dataGridView1.Rows.Clear();;
            //if (_Item == null) return;
            if (selltypelist.Count != 0)
            {


                for (int i = 0; i < ConsumUnitsList.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].HeaderCell.Value = ConsumUnitsList[i].ConsumeUnitName;
                    for (int j = 0; j < selltypelist.Count; j++)
                    {
                        try
                        {

                            List<ItemINSellPrice> dd = ItemINSellPriceList.Where(x => (x._ItemIN._Operation.OperationID == _TempItemIN ._Operation.OperationID && x.SellType_.SellTypeID == selltypelist[j].SellTypeID && x.ConsumeUnit_.ConsumeUnitID == ConsumUnitsList[i].ConsumeUnitID)).ToList();
                            if (dd.Count != 1) dataGridView1.Rows[i].Cells[j].Value = " - " + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);
                            else
                            {
                                double sellprice = System.Math.Round(Convert.ToDouble(dd[0].Price) * _Currency.ExchangeRate, 3);
                                dataGridView1.Rows[i].Cells[j].Value = sellprice.ToString() + " " + _Currency.CurrencySymbol.Replace(" ", string.Empty);

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
        public void LoadForm(bool Edit)
        {
            buttonSave.Name = "buttonSave";
            buttonSave.Text = "حفظ";

            PanelSellInfo .Enabled = true;

            if (Edit)
            {
                dataGridView1.Enabled = true;
                comboBoxConsumeUnt.Enabled = true;
                buttonAllAvailableItems.Enabled = true;
                buttonAvailableItemsINFolder.Enabled = true;
                buttonAvailableItemsINPlace.Enabled = true;
                textBoxAmount.ReadOnly = false;
                textBoxCost.ReadOnly = false;
                TextboxNotes.ReadOnly = false;
                //this.comboBoxSellType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSellType_SelectedIndexChanged);
                this.textBoxCost.TextChanged += new System.EventHandler(this.textBoxSellPrice_TextChanged);
                this.textBoxAmount.TextChanged += new System.EventHandler(this.textBoxSellPrice_TextChanged);
                //this.comboBoxConsumeUnt.SelectedIndexChanged += new System.EventHandler(this.comboBoxConsumeUnt_SelectedIndexChanged_1);

            }
            else
            {
                dataGridView1.Enabled = false ;
                comboBoxConsumeUnt.Enabled = false;
                buttonAllAvailableItems.Enabled = false;
                buttonAvailableItemsINFolder.Enabled = false;
                buttonAvailableItemsINPlace.Enabled = false;
                textBoxAmount.ReadOnly = true;
                textBoxCost.ReadOnly = true;
                TextboxNotes.ReadOnly = true;
            }
            ItemINSellPriceList = new ItemINSellPriceSql(DB).GetItemINPrices(_TempItemIN);

            RefreshComponents();

            linkLabelShowBuyOPR.Visible = true;
            textBoxAmount.Text = _ItemOUT  .Amount .ToString();
            textBoxCost.Text = _ItemOUT ._OUTValue .Value.ToString();
            

        }
        
        private void buttonSave_Click(object sender, EventArgs e)
        {

            if (_TempItemIN == null)
            {
                MessageBox.Show("يرجى تحديد المادة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double amount, cost;
            try
            {
                cost = Convert.ToDouble(textBoxCost .Text );
            }
            catch
            {
                MessageBox.Show("السعر يجب ان يكون رقم حقيقي " , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            try
            {
                amount  = Convert.ToDouble(textBoxAmount .Text);
            }
            catch
            {
                MessageBox.Show("الكمية يجب ان يكون رقم حقيقي " , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            ComboboxItem consumeunititem = (ComboboxItem)comboBoxConsumeUnt.SelectedItem;
            ConsumeUnit _ConsumeUnit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunititem.Value);

            //ComboboxItem ComboboxItem_selltype = (ComboboxItem)comboBoxSellType.SelectedItem;
            //string  SellType_ =  ComboboxItem_selltype.Text;

            if (buttonSave.Name == "buttonAdd")
            {
                try
                {

                    TradeState tradestate = _TempItemIN._TradeState;
                    ItemOUT ItemOUT_ = new ItemOUTSQL(DB).AddItemOUT(_Operation, _TempItemIN.ItemINID, _TempPlace
                        , amount , _ConsumeUnit, cost 
                        , TextboxNotes.Text);
                    if (ItemOUT_ != null)
                    {
                        _ItemOUT = ItemOUT_;
                        MessageBox.Show("تم الاضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Changed_ = true;
                        this.Close();

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":تعذر اخراج العنصر " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            else
            {
                try
                {
                    if (_ItemOUT != null)
                    {

                        bool success = new ItemOUTSQL(DB).UpdateItemOUT(_ItemOUT.ItemOUTID, _TempItemIN.ItemINID, _TempPlace
                            , amount , _ConsumeUnit, cost 
                        , TextboxNotes.Text);
                        if (success == true)
                        {
                            MessageBox.Show("تم حفظ  بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _ItemOUT = new ItemOUTSQL(DB).GetItemOUTINFO_BYID(_ItemOUT.ItemOUTID);
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
        private void buttonAllAvailableItems_Click(object sender, EventArgs e)
        {
            ItemObj.Forms.ShowAvailableItemSimpleForm ShowAvailableItemSimpleForm_ = new ItemObj.Forms.ShowAvailableItemSimpleForm(DB, true);
            DialogResult d1 = ShowAvailableItemSimpleForm_.ShowDialog();
            if (d1 == DialogResult.OK)
            {
                Item item_ = ShowAvailableItemSimpleForm_.ReturnItem;
                List<AvailbeItems_ItemINDetails> AvailbeItems_ItemINList = new AvailableItemSQL(DB).GetItemINList_ForAvailableItem(item_.ItemID);
                ItemIN ItemIN_;
    
                if (AvailbeItems_ItemINList.Count == 1)
                     ItemIN_ = AvailbeItems_ItemINList[0]._ItemIN;
                else
                {
                    ItemObj.Forms.AvailableItem_ItemIN_Form AvailableItem_ItemINS_Form = new ItemObj.Forms.AvailableItem_ItemIN_Form(DB, item_, true);
                    DialogResult d2 = AvailableItem_ItemINS_Form.ShowDialog();
                    if (d2 == DialogResult.OK)
                    {
                        ItemIN_ = AvailableItem_ItemINS_Form.ReturnItemIN;
                    }
                    else ItemIN_ = null;
                    AvailableItem_ItemINS_Form.Dispose();
                }
               
                if (ItemIN_ != null)
                {
                    
                    if (_Operation.OperationType == Operation.ASSEMBLAGE)
                    {
                        AssemblabgeOPR AssemblabgeOPR_ = new AssemblageSQL(DB).GetAssemblageOPR_INFO_BYID(_Operation.OperationID);
                        if (AssemblabgeOPR_._ItemIN.ItemINID == ItemIN_.ItemINID)
                        {
                            MessageBox.Show("العنصر المساهم بالتجميع يجب ان لا يكون هو نفسه العنصر المجمع ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    TradeStorePlace place = null;
                    List<ItemIN_StoreReport> iteminstore_placelist = new ItemINSQL(DB).GetItemIN_StoreReportList(ItemIN_ );
                    DialogResult d3;
                    
                    if (iteminstore_placelist.Count == 1)
                    {
                        place = iteminstore_placelist[0].Place;
                        d3 = DialogResult.OK;
                    }
                    else
                    {
                        ItemIN_StoreReport_Form ItemIN_StoreReport_Form_ = new ItemIN_StoreReport_Form(DB, ItemIN_);
                        DialogResult dd = ItemIN_StoreReport_Form_.ShowDialog();
                        if ( dd== DialogResult.OK)
                        {
                            place = ItemIN_StoreReport_Form_.ReturnPlace;
                            d3 = DialogResult.OK;
                        }
                        else
                            d3 = DialogResult.Cancel;


                        ItemIN_StoreReport_Form_.Dispose();
                    }
                    if (d3 == DialogResult.OK)
                    {

                        _TempPlace = place;
                        _TempItemIN = ItemIN_;
                        if (_Operation.OperationType == Operation.ASSEMBLAGE || _Operation.OperationType == Operation.DISASSEMBLAGE)
                        {
                            Currency tempcur = new OperationSQL(DB).GetOperationItemINCurrency(_TempItemIN._Operation);
                            Currency targcur = new OperationSQL(DB).GetOperationItemOUTCurrency(_Operation);
                            textBoxCost.Text = (_TempItemIN._INCost.Value * (targcur.ExchangeRate / tempcur.ExchangeRate)).ToString();
                        }
                        RefreshComponents();

                    }


                }

               
            }
            ShowAvailableItemSimpleForm_.Dispose();
        
        }

        private void buttonAvailableItemsINFolder_Click(object sender, EventArgs e)
        {
            ItemObj.Forms.AvailabeItemsForm AvailabeItemsForm_ = new ItemObj.Forms.AvailabeItemsForm(DB,LastUsedFolder , true);
            DialogResult d1 = AvailabeItemsForm_.ShowDialog();
            if (d1 == DialogResult.OK)
            {
                Item item_ = AvailabeItemsForm_.ReturnItem;
                List<AvailbeItems_ItemINDetails> AvailbeItems_ItemINList = new AvailableItemSQL(DB).GetItemINList_ForAvailableItem(item_.ItemID);
                ItemIN ItemIN_;
                if (AvailbeItems_ItemINList.Count == 1)
                    ItemIN_ = AvailbeItems_ItemINList[0]._ItemIN;
                else
                {
                    ItemObj.Forms.AvailableItem_ItemIN_Form AvailableItem_ItemINS_Form = new ItemObj.Forms.AvailableItem_ItemIN_Form(DB, item_, true);
                    DialogResult d2 = AvailableItem_ItemINS_Form.ShowDialog();
                    if (d2 == DialogResult.OK)
                    {
                        ItemIN_ = AvailableItem_ItemINS_Form.ReturnItemIN;
                    }
                    else ItemIN_ = null;
                    AvailableItem_ItemINS_Form.Dispose();
                }

                if (ItemIN_ != null)
                {

                    if (_Operation.OperationType == Operation.ASSEMBLAGE)
                    {
                        AssemblabgeOPR AssemblabgeOPR_ = new AssemblageSQL(DB).GetAssemblageOPR_INFO_BYID(_Operation.OperationID);
                        if (AssemblabgeOPR_._ItemIN.ItemINID == ItemIN_.ItemINID)
                        {
                            MessageBox.Show("العنصر المساهم بالتجميع يجب ان لا يكون هو نفسه العنصر المجمع ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    //if (_Operation.OperationType == Operation.DISASSEMBLAGE)
                    //{
                    //    DisAssemblabgeOPR DisAssemblabgeOPR_ = new DisAssemblageSQL(DB).GetDisAssemblageOPR_INFO_BYID(_Operation.OperationID);
                    //    if (DisAssemblabgeOPR_._ItemOUT._ItemIN.ItemINID == ItemIN_.ItemINID)
                    //    {
                    //        MessageBox.Show("العنصر الناتج عن التفكيك يجب ان لا يكون هو نفسه العنصر المفكك ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        return;
                    //    }
                    //}
                    TradeStorePlace place = null;
                    List<ItemIN_StoreReport> iteminstore_placelist = new ItemINSQL(DB).GetItemIN_StoreReportList(ItemIN_);
                    DialogResult d3;
                    if (iteminstore_placelist.Count == 1)
                    {
                        place = iteminstore_placelist[0].Place;
                        d3 = DialogResult.OK;
                    }
                    else
                    {
                        ItemIN_StoreReport_Form ItemIN_StoreReport_Form_ = new ItemIN_StoreReport_Form(DB, ItemIN_);
                        if (ItemIN_StoreReport_Form_.ShowDialog() == DialogResult.OK)
                        {
                            place = ItemIN_StoreReport_Form_.ReturnPlace;
                            d3 = DialogResult.OK;
                        }
                        else
                            d3 = DialogResult.Cancel;


                        ItemIN_StoreReport_Form_.Dispose();
                    }
                    if (d3 == DialogResult.OK)
                    {

                        _TempPlace = place;
                        _TempItemIN = ItemIN_;
                        if (_Operation.OperationType == Operation.ASSEMBLAGE || _Operation.OperationType == Operation.DISASSEMBLAGE)
                        {
                            Currency tempcur = new OperationSQL(DB).GetOperationItemINCurrency(_TempItemIN._Operation);
                            Currency targcur = new OperationSQL(DB).GetOperationItemOUTCurrency(_Operation);
                            textBoxCost.Text = (_TempItemIN._INCost.Value * (targcur.ExchangeRate / tempcur.ExchangeRate)).ToString();
                        }
                        RefreshComponents();

                    }

                }


            }
            AvailabeItemsForm_.Dispose();
        }

        private void buttonAvailableItemsINPlace_Click(object sender, EventArgs e)
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
                    ItemIN ItemIN_;
                    List<PlaceAvailbeItems_ItemINDetails> StoredItems = new AvailableItemSQL(DB).GetStoredItems_BuyOPRDetails(Place, item);
                    List<PlaceAvailbeItems_ItemINDetails> StoredItems_ItemIN = StoredItems.Where (x=>x.StoreType == TradeItemStore.ITEMIN_STORE_TYPE).ToList ();

                    if (StoredItems_ItemIN.Count ==1)
                    {
                        ItemIN_ = new ItemINSQL(DB).GetItemININFO_BYID(StoredItems_ItemIN[0].OprID);  
                    }
                    else
                    {
                        Container.PlaceItem_ItemINForm PlaceItemBuyOprForm_ = new Container.PlaceItem_ItemINForm(DB, Place, item, true);
                        DialogResult d3 = PlaceItemBuyOprForm_.ShowDialog();
                        if (d3 == DialogResult.OK)
                        {
                            if (_Operation.OperationType == Operation.ASSEMBLAGE)
                            {
                                AssemblabgeOPR AssemblabgeOPR_ = new AssemblageSQL(DB).GetAssemblageOPR_INFO_BYID(_Operation.OperationID);
                                if (AssemblabgeOPR_._ItemIN.ItemINID == PlaceItemBuyOprForm_.ReturnItemIN.ItemINID)
                                {
                                    MessageBox.Show("العنصر المساهم بالتجميع يجب ان لا يكون هو نفسه العنصر المجمع ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            ItemIN_ = PlaceItemBuyOprForm_.ReturnItemIN;

                        }
                        else ItemIN_ = null;
                       
                        PlaceItemBuyOprForm_.Dispose();

                    }
                    if (ItemIN_ != null)
                    {
                        _TempItemIN = ItemIN_; 
                        _TempPlace = Place;
                        if (_Operation.OperationType == Operation.ASSEMBLAGE || _Operation.OperationType == Operation.DISASSEMBLAGE)
                        {
                            Currency tempcur = new OperationSQL(DB).GetOperationItemINCurrency(_TempItemIN._Operation);
                            Currency targcur = new OperationSQL(DB).GetOperationItemOUTCurrency(_Operation);
                            textBoxCost.Text = (_TempItemIN._INCost.Value * (targcur.ExchangeRate / tempcur.ExchangeRate)).ToString();
                        }
                        RefreshComponents();
                    }
                }
                 
                   
                PlaceItemsForm_.Dispose();
            }
            showlocations.Dispose();
        }
    
        //private void comboBoxSellType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    RefreshPrice();
        //}
        //public void RefreshPrice()
        //{
        //    if (_TempItemIN == null    ) return;


        //    textBoxExchangeRate.Text = _Currency .ExchangeRate .ToString();
        //    try
        //    {
        //        ComboboxItem consumeunititem = (ComboboxItem)comboBoxConsumeUnt.SelectedItem;
        //        ConsumeUnit _ConsumeUnit;
        //        if (consumeunititem.Value == 0)
        //            _ConsumeUnit = new ConsumeUnit(0, _TempItemIN._Item .DefaultConsumeUnit, _TempItemIN._Item, 1);
        //        else
        //             _ConsumeUnit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunititem.Value);
        //        //ComboboxItem SellTypeitem = (ComboboxItem)comboBoxSellType .SelectedItem;
        //        //SellType _SellType = new SellTypeSql(DB).GetSellTypeinfo(SellTypeitem.Value);
        //        List<ItemINSellPrice> ff = ItemINSellPriceList.Where(x => (x._ItemIN.ItemINID  == _TempItemIN.ItemINID  && x.SellType_.SellTypeID == _SellType.SellTypeID && x.ConsumeUnit_.ConsumeUnitID == _ConsumeUnit.ConsumeUnitID)).ToList();
        //        if (ff.Count > 0)
        //        {
        //            double price = Convert.ToDouble(ff[0].Price);
        //            textBoxSellPrice.Text = System.Math.Round((price * _Currency.ReferenceFactor), 3).ToString();

        //        }
        //        else
        //        {
        //            textBoxSellPrice.Text = "-";
        //        }

        //    }
        //    catch(Exception ee)
        //    {
        //        MessageBox.Show("فشل جلب السعر"+ee.Message );
        //    }
        //}



        //private void comboBoxConsumeUnt_SelectedIndexChanged_1(object sender, EventArgs e)
        //{
        // RefreshPrice();
        //}

        private void textBoxSellPrice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double amount = Convert.ToDouble(textBoxAmount .Text );
                double sellprice = Convert.ToDouble(textBoxCost .Text);
                TextboxTotalValue.Text = (amount * sellprice).ToString()+_Currency .CurrencySymbol ;

            }
            catch
            {
                TextboxTotalValue.Text = "-";
            }
        }

        private void linkLabelShowBuyOPR_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ItemINForm BuyOprForm_ = new ItemINForm(DB, _TempItemIN, false );
            BuyOprForm_.ShowDialog();
        }

        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            AdjustmentDatagridviewColumnsWidth();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
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
                FillComboBoxConsumeUnit(SelectedConsumeUnit);
                TradeState TradeState_ = _TempItemIN._TradeState ;
                SellType SelectedSellType = new SellTypeSql(DB).GetSellTypeinfo(Convert.ToUInt32(dataGridView1.Columns[e.ColumnIndex].Name));


                double? price_ = new ItemINSellPriceSql(DB).GetPrice(_TempItemIN, SelectedSellType, SelectedConsumeUnit);
                if (price_ == null) MessageBox.Show("السعر غير مضبوط", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    double price = Convert.ToDouble(price_);
                    price = price * Convert.ToDouble(textBoxExchangeRate .Text );
                    
                    textBoxCost.Text =System.Math .Round ( price,2).ToString();
                }
            }catch
            {
               
            }
        }

       
    }
}
