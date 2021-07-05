using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Forms;
using ItemProject.AccountingObj.Objects;
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
    public partial class BillSellForm : Form
    {
        
        DatabaseInterface DB;
        Contact _Contact;
        BillSell _BillSell;
        public bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }
        System.Windows.Forms.MenuItem OpenPayIN_MenuItem;
        System.Windows.Forms.MenuItem AddPayIN_MenuItem;
        System.Windows.Forms.MenuItem EditPayIN_MenuItem;
        System.Windows.Forms.MenuItem DeletePayIN_MenuItem;


        System.Windows.Forms.MenuItem OpenItemOUTMenuItem;
        System.Windows.Forms.MenuItem AddItemOUTMenuItem;
        System.Windows.Forms.MenuItem EditItemOUTMenuItem;
        System.Windows.Forms.MenuItem DeleteItemOUTMenuItem;

        System.Windows.Forms.MenuItem OpenAdditionalClauseMenuItem;
        System.Windows.Forms.MenuItem AddAdditionalClauseMenuItem;
        System.Windows.Forms.MenuItem EditAdditionalClauseItem;
        System.Windows.Forms.MenuItem DeleteAdditionalClauseItem;
        List<ItemOUT > _ItemOUTList = new List<ItemOUT>();
        List<BillAdditionalClause> _BillAdditionalClauseList = new List<BillAdditionalClause>();
        List<SellType> selltypelist = new List<SellType>();
        double TotalCost;
        double PaysValue;
        public BillSellForm(DatabaseInterface db,DateTime BillINDate_,Contact Contact_)
        {
            DB = db;

            InitializeComponent();
            _Contact = Contact_;
            selltypelist = new SellTypeSql(DB).GetSellTypeList();
            FillComboBoxSellType(null);
            TotalCost = 0;
            this.textBoxContact.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxContact_MouseDoubleClick);

            panelShowBillDataByCurrency.Visible = false;
            textBoxBillINExchangeRate.Enabled = true;
            comboBoxBillINCurrency.Enabled = true;
            panelShowBillDataByCurrency.Visible = false;

            _Changed = false;
            if (Contact_ != null) textBoxContact.Text = Contact_.Get_Complete_ContactName_WithHeader();
            dateTimePicker_.Value = BillINDate_;
            FillComboBoxCurrency(ProgramGeneralMethods.GetDefaultCurrency (DB) );
            buttonSave.Name = "buttonAdd";
            buttonSave.Text  = "انشاء فاتورة";
            textBoxDiscount.Enabled = false;
            panelSellOPRs .Enabled = false;
            InitializeMenuItems();


        }
        public BillSellForm(DatabaseInterface db, BillSell BillSell_, bool edit)
        {
            try
            {
                InitializeComponent();

                DB = db;
                _BillSell = BillSell_;
                selltypelist = new SellTypeSql(DB).GetSellTypeList();
                ProgramGeneralMethods.FillComboBoxSellType (ref comboBoxSellType ,DB, BillSell_._SellType );
                _Changed = false;
                InitializeMenuItems();
                loadForm(edit);
            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message );
            }
         


        }
         public void InitializeMenuItems()
        {
            OpenItemOUTMenuItem  = new System.Windows.Forms.MenuItem("فتح تفاصيل ", OpenItemOUT_MenuItem_Click);
            AddItemOUTMenuItem  = new System.Windows.Forms.MenuItem("اضافة مادة", AddItemOUT_MenuItem_Click);
            EditItemOUTMenuItem  = new System.Windows.Forms.MenuItem("تعديل ", EditItemOUT_MenuItem_Click);
            DeleteItemOUTMenuItem  = new System.Windows.Forms.MenuItem("حذف", DeleteItemOUT_MenuItem_Click); ;

            AddAdditionalClauseMenuItem  = new System.Windows.Forms.MenuItem("انشاء بند اضافي", AddAdditionalClause_MenuItem_Click);
            EditAdditionalClauseItem  = new System.Windows.Forms.MenuItem("تعديل ", EditAdditionalClause_MenuItem_Click);
            DeleteAdditionalClauseItem  = new System.Windows.Forms.MenuItem("حذف", DeleteAdditionalClause_MenuItem_Click); ;
            OpenAdditionalClauseMenuItem  = new System.Windows.Forms.MenuItem("فتح", OpenAdditionalClause_MenuItem_Click);

            AddPayIN_MenuItem = new MenuItem("اضافة دفعة تابعة لهذه الفاتورة", AddPayIN_MenuItem_Click);
            EditPayIN_MenuItem = new MenuItem("تعديل", EditPayIN_MenuItem_Click);
            DeletePayIN_MenuItem = new MenuItem("حذف", DeletePayIN_MenuItem_Click);
            OpenPayIN_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية الدفع", OpenPayIN_MenuItem_Click);

        }

        private void OpenPayIN_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewPays.SelectedItems.Count > 0)
                {
                    uint PayINid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name);
                    PayIN PayIN_ = new PayINSQL(DB).GetPayIN_INFO_BYID(PayINid);
                    AccountingObj.Forms.PayINForm PayINForm_ = new AccountingObj.Forms.PayINForm(DB, PayIN_, false);
                    PayINForm_.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void DeletePayIN_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint PayINid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name);
                bool success = new PayINSQL(DB).Delete_PayIN(PayINid);
                if (success)
                {
                    FillBillSellPays();
                }
            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void EditPayIN_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewPays.SelectedItems.Count > 0)
                {
                    uint PayINid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name);
                    PayIN PayIN_ = new PayINSQL(DB).GetPayIN_INFO_BYID(PayINid);
                    AccountingObj.Forms.PayINForm PayINForm_ = new AccountingObj.Forms.PayINForm(DB, PayIN_, true );
                    PayINForm_.ShowDialog();
                    if (PayINForm_.Changed)
                    {
                        FillBillSellPays();

                    }
                }


            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void AddPayIN_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PayINForm PayINForm_ = new PayINForm(DB, DateTime.Now,_BillSell);
                PayINForm_.ShowDialog();
                if (PayINForm_.DialogResult == DialogResult.OK)
                {
                    FillBillSellPays();
                }
            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }




        private void RefreshSellOperations(List <ItemOUT > ItemOUTList,List <BillAdditionalClause > AdditionalClauseList)
        {

            listViewItemOUT.Items.Clear();
            ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);

            if (ItemOUTList.Count == 0)
            {
                panelShowBillDataByCurrency.Enabled = false;
                return;
            }
            else panelShowBillDataByCurrency.Enabled = true;
            double Transtion_factor;
            Currency tempcurrency;
            if (checkBoxCUrrentExchangeRate.Checked)
            {
                tempcurrency = currency;
                Transtion_factor = (currency.ExchangeRate  / _BillSell.ExchangeRate);
                textBoxExchangeRate.Text = currency.ExchangeRate .ToString();
            }
            else
            {
                tempcurrency = _BillSell._Currency;
                Transtion_factor = 1;
                textBoxExchangeRate.Text = currency.ExchangeRate .ToString();
            }




            double itemsout_value = 0;

   
            for (int i=0;i< ItemOUTList.Count;i++)
            {
                double sellprice = ItemOUTList[i]._OUTValue .Value   * Transtion_factor; 
                double total_sellprice = sellprice * ItemOUTList[i].Amount;
                itemsout_value = itemsout_value + total_sellprice;
                ListViewItem ListViewItem_ = new ListViewItem((listViewItemOUT .Items .Count +1).ToString ());
                ListViewItem_.Name = ItemOUTList[i].ItemOUTID .ToString();
                ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN  ._Item .ItemName );
                ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.ItemCompany );
                ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._Item.folder .FolderName );
                ListViewItem_.SubItems.Add(ItemOUTList[i]._ItemIN._TradeState .TradeStateName );
                ListViewItem_.SubItems.Add(ItemOUTList[i].Amount.ToString());
                ListViewItem_.SubItems.Add(ItemOUTList[i]._ConsumeUnit.ConsumeUnitName.ToString());


                ListViewItem_.SubItems.Add(System.Math.Round(sellprice, 3).ToString() + " " + tempcurrency  .CurrencySymbol.Replace (" ",string.Empty ));
                ListViewItem_.SubItems.Add(System.Math.Round(total_sellprice, 3).ToString ()+" "+tempcurrency   .CurrencySymbol.Replace(" ", string.Empty));
                ListViewItem_.SubItems.Add(ItemOUTList[i].Notes);
                ListViewItem_.BackColor = Color.LimeGreen ;
                listViewItemOUT.Items.Add(ListViewItem_);

            }
            double additionalclauses_value = 0;
            listViewAdditionalClauses.Items.Clear();
            for (int i = 0; i < AdditionalClauseList.Count; i++)
            {
                ListViewItem ListViewItem_ = new ListViewItem((listViewAdditionalClauses.Items.Count + 1).ToString());
                additionalclauses_value = additionalclauses_value + (AdditionalClauseList[i].Value * Transtion_factor);
                ListViewItem_.Name = AdditionalClauseList[i].ClauseID .ToString ();
                ListViewItem_.SubItems.Add(AdditionalClauseList[i].Description);
               ListViewItem_.SubItems.Add(System.Math.Round ((AdditionalClauseList[i].Value * Transtion_factor),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty));
                listViewAdditionalClauses.Items.Add(ListViewItem_);
            }
            textBoxItemsOUTValue.Text =System.Math .Round ( itemsout_value,3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty); 
            textBoxAdditionalClausesvALUE.Text = System.Math.Round(additionalclauses_value,3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty);
            TotalCost = additionalclauses_value+itemsout_value;
            if (TotalCost > 0)
            {
                double discount;
                try
                {
                    discount = Convert.ToDouble(textBoxDiscount.Text) * Transtion_factor;
                    textBoxValue.Text = System.Math.Round(TotalCost,3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty);
                    textBoxDiscount.Enabled = true;
                    textBoxDiscount.Text = _BillSell.Discount.ToString();
                    textBoxClearValue.Text = System.Math.Round((TotalCost - _BillSell.Discount),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty);
                    textBoxPays.Text = System.Math.Round((PaysValue * Transtion_factor),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty); ;
                    textBoxRemain.Text = System.Math.Round((TotalCost - discount - PaysValue * Transtion_factor),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty); ;
                }
                catch
                {
                    textBoxClearValue.Text = "-";
                    textBoxRemain.Text = "-";

                }

            }

            AdjustListViewColumnsWidth();
        }

        private void FillComboBoxCurrency(Currency currency)
        {
            comboBoxCurrency.Items.Clear();
            int selected_index = 0;
            try
            {
                List<Currency> CurrencyList  = new CurrencySQL(DB).GetCurrencyList();
                for (int i = 0; i < CurrencyList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(CurrencyList[i].CurrencyName + "(" + CurrencyList[i].CurrencySymbol + ")", CurrencyList[i].CurrencyID);
                    comboBoxBillINCurrency.Items.Add(item);
                    comboBoxCurrency.Items.Add(item);
                        if (currency != null && currency.CurrencyID  == CurrencyList[i].CurrencyID ) selected_index = i;
                }
                comboBoxBillINCurrency.SelectedIndex = selected_index;
                comboBoxCurrency.SelectedIndex = selected_index;
            }
            catch
            { }
      
        }

        private void textBoxContact_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TradeContact.ShowContactsForm form = new TradeContact.ShowContactsForm(DB, true);
                DialogResult dd= form.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    _Contact = form.Contact_;
                    textBoxContact.Text  =_Contact .GetContactTypeHeader  () +":"+ _Contact.ContactName;
                }
            }
        }
        public void loadForm(bool edit)
        {
            buttonSave.Name = "buttonSave";
            buttonSave.Text = "حفظ";
            panelSellOPRs.Enabled = true ;
            textBoxDiscount.Enabled = false;
            if (_BillSell != null)
            {
                
                _ItemOUTList  = new ItemOUTSQL(DB).GetItemOUTList(_BillSell._Operation);
                _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillSell._Operation);
                _Contact = _BillSell._Contact;
                textBoxContact.Text = _BillSell._Contact.Get_Complete_ContactName_WithHeader();
                dateTimePicker_.Value = _BillSell.BillDate  ;

                FillComboBoxCurrency(_BillSell._Currency);
                textBoxBillINExchangeRate.Text = _BillSell.ExchangeRate.ToString();
                textBoxDescription.Text = _BillSell.BillDescription;
                TextboxNotes.Text = _BillSell.Notes;
                labelBillID.Text = "فاتورة مبيع رقم:" + _BillSell._Operation.OperationID  .ToString();
                FillBillSellPays();
                RefreshSellOperations(_ItemOUTList , _BillAdditionalClauseList);
                comboBoxCurrency.Enabled = false;
                if (edit)
                {
                    this.listViewItemOUT.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewItemOUT_MouseDoubleClick);
                    this.listViewItemOUT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewItemOUT_MouseDown);
                    this.listViewAdditionalClauses .MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewAdditionalClause_MouseDoubleClick);
                    this.listViewAdditionalClauses .MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewAdditionalClause_MouseDown);

                    this.listViewPays.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewPays_MouseDoubleClick);
                    this.listViewPays.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewPays_MouseDown);

                    panelShowBillDataByCurrency.Visible = false;
                    comboBoxSellType.Enabled = true;
                    this.textBoxContact.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxContact_MouseDoubleClick);
                    textBoxDescription.ReadOnly = false;
                    dateTimePicker_.Enabled = true;
                    textBoxBillINExchangeRate.ReadOnly = false;
                    comboBoxBillINCurrency.Enabled = true;
                    TextboxNotes.ReadOnly = false;
                   
                    textBoxDiscount.ReadOnly = false;
                    buttonSave.Visible = true;
                }
                else
                {
     
                 comboBoxSellType.Enabled = false ;
                    panelShowBillDataByCurrency.Visible = true;
           
                    textBoxDescription.ReadOnly = true;
                    dateTimePicker_.Enabled = false;
                    textBoxBillINExchangeRate.ReadOnly = true;
                    comboBoxBillINCurrency.Enabled = false;
                    TextboxNotes.ReadOnly = true;
                    textBoxDiscount.ReadOnly = true ;
                    buttonSave.Visible = false ;

                }
                listViewPays.Enabled = true;
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            ComboboxItem ComboboxItem_selltype = (ComboboxItem)comboBoxSellType.SelectedItem;
            SellType SellType_ = new SellTypeSql(DB).GetSellTypeinfo(ComboboxItem_selltype.Value);

            ComboboxItem item = (ComboboxItem)comboBoxBillINCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(item.Value);
            if (buttonSave .Name == "buttonAdd")
            {
                if(_Contact ==null )
                {
                    MessageBox.Show("يرجى تحديد الجهة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double exchangerate;
                try
                {
                    exchangerate = Convert.ToDouble(textBoxBillINExchangeRate .Text );
                }
                catch
                {
                    MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                BillSell billsell = new BillSellSQL(DB).AddBillSell (dateTimePicker_.Value, textBoxDescription.Text,
                    SellType_,_Contact, currency, exchangerate, 0, TextboxNotes.Text);
                if (billsell  != null)
                {
                    _BillSell = billsell ;

                    MessageBox.Show("تم انشاء الفاتورة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this._Changed = true;
                    loadForm(true  );

                }
            }
      
            else
            {
                if(_BillSell != null )
                {
                    if (_Contact == null)
                    {
                        MessageBox.Show("يرجى تحديد الجهة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    double exchangerate;
                    try
                    {
                        exchangerate = Convert.ToDouble(textBoxBillINExchangeRate.Text);
                    }
                    catch
                    {
                        MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    double discount;
                    try
                    {

                        discount = Convert.ToDouble(textBoxDiscount.Text);
                        if(discount >TotalCost   )
                        {
                            MessageBox.Show("الخصم يجب ان يكون اقل من قيمة الفاتورة","",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("الخصم يجب ان يكون رقم حقيقي", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    bool success = new BillSellSQL (DB).UpdateBillIN (_BillSell._Operation.OperationID  , dateTimePicker_.Value
                        , textBoxDescription.Text, SellType_, _Contact, currency, exchangerate, discount, TextboxNotes.Text);
                    if (success == true)
                    {
                        MessageBox.Show("تم حفظ الفاتورة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _BillSell = new BillSellSQL (DB).GetBillSell_INFO_BYID(_BillSell._Operation.OperationID  );
                        this._Changed = true;
                        loadForm(true   );
                    }
                }
            }
        }

        #region ItemOUT
        private void DeleteItemOUT_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewItemOUT.SelectedItems[0].Name);
                bool success = new ItemOUTSQL(DB).DeleteItemOUT(sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_BillSell._Operation);
                    RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);

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
            if (listViewItemOUT.SelectedItems.Count > 0)
            {
                uint itemoutid = Convert.ToUInt32(listViewItemOUT.SelectedItems[0].Name);
                ItemOUT ItemOUT_ = new ItemOUTSQL(DB).GetItemOUTINFO_BYID(itemoutid);
                ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, ItemOUT_, true);
                ItemOUTForm_.ShowDialog();
                if (ItemOUTForm_.Changed)
                {
                    _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_BillSell._Operation);
                    RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
                }
                ItemOUTForm_.Dispose();
            }
        }
        private void OpenItemOUT_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewItemOUT.SelectedItems.Count > 0)
            {

                uint itemoutid = Convert.ToUInt32(listViewItemOUT.SelectedItems[0].Name);
                ItemOUT ItemOUT_ = new ItemOUTSQL(DB).GetItemOUTINFO_BYID(itemoutid);
                ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, ItemOUT_, false);
                ItemOUTForm_.ShowDialog();
                if (ItemOUTForm_.Changed)
                {
                    _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_BillSell._Operation);
                    RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
                }
                ItemOUTForm_.Dispose();
            }
        }

        private void AddItemOUT_MenuItem_Click(object sender, EventArgs e)
        {
            ItemOUTForm ItemOUTForm_ = new ItemOUTForm(DB, _BillSell._Operation);
            DialogResult d = ItemOUTForm_.ShowDialog();
            if (ItemOUTForm_.Changed)
            {
                _ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(_BillSell._Operation);
                RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
            }
            ItemOUTForm_.Dispose();
        }
        private void listViewItemOUT_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewItemOUT.SelectedItems.Count > 0)
            {
                OpenItemOUTMenuItem .PerformClick();
            }
        }
        private void listViewItemOUT_MouseDown(object sender, MouseEventArgs e)
        {
            listViewItemOUT.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewItemOUT.Items)
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


                        MenuItem[] mi1 = new MenuItem[] {OpenItemOUTMenuItem , EditItemOUTMenuItem, DeleteItemOUTMenuItem,new MenuItem ("-"), AddItemOUTMenuItem };
                        listViewItemOUT.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddItemOUTMenuItem };
                    listViewItemOUT.ContextMenu = new ContextMenu(mi);

                }

            }

        }
        #endregion
        #region AdditionalClause
        private void DeleteAdditionalClause_MenuItem_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dd = MessageBox.Show("هل انت متاكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint sid = Convert.ToUInt32(listViewAdditionalClauses.SelectedItems[0].Name);
                bool success = new BillAdditionalClauseSQL(DB).DeleteBillAdditionalClause(sid);
                if (success)
                {
                    MessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillSell._Operation);
                    RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);

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

        private void EditAdditionalClause_MenuItem_Click(object sender, EventArgs e)
        {

            if (listViewAdditionalClauses.SelectedItems.Count > 0)
            {

                uint clauseid = Convert.ToUInt32(listViewAdditionalClauses.SelectedItems[0].Name);
                BillAdditionalClause BillAdditionalClause_ = new BillAdditionalClauseSQL(DB).Get_BillAdditionalClause_INFO_BYID(clauseid);
                BillAdditionalClauseForm BillAdditionalClauseForm_ = new BillAdditionalClauseForm(DB, BillAdditionalClause_, true);
                DialogResult d = BillAdditionalClauseForm_.ShowDialog();
                if (d == DialogResult.OK)
                {
                    _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillSell._Operation);
                    RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
                }
                BillAdditionalClauseForm_.Dispose();
            }
        }
        private void OpenAdditionalClause_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewAdditionalClauses.SelectedItems.Count > 0)
            {

                uint clauseid = Convert.ToUInt32(listViewAdditionalClauses.SelectedItems[0].Name);
                BillAdditionalClause BillAdditionalClause_ = new BillAdditionalClauseSQL(DB).Get_BillAdditionalClause_INFO_BYID(clauseid);
                BillAdditionalClauseForm BillAdditionalClauseForm_ = new BillAdditionalClauseForm(DB, BillAdditionalClause_, false);
                DialogResult d = BillAdditionalClauseForm_.ShowDialog();
                if (d == DialogResult.OK)
                {
                    _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillSell._Operation);
                    RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
                }
                BillAdditionalClauseForm_.Dispose();
            }
        }

        private void AddAdditionalClause_MenuItem_Click(object sender, EventArgs e)
        {
            BillAdditionalClauseForm BillAdditionalClauseForm_ = new BillAdditionalClauseForm(DB, _BillSell._Operation);
            DialogResult d = BillAdditionalClauseForm_.ShowDialog();
            if (d == DialogResult.OK)
            {
                _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillSell._Operation);
                RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
            }
            BillAdditionalClauseForm_.Dispose();
        }
        private void listViewAdditionalClause_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewAdditionalClauses.SelectedItems.Count > 0)
            {
                OpenAdditionalClauseMenuItem.PerformClick();
            }
        }
        private void listViewAdditionalClause_MouseDown(object sender, MouseEventArgs e)
        {
            listViewAdditionalClauses.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewAdditionalClauses.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenAdditionalClauseMenuItem, EditAdditionalClauseItem, DeleteAdditionalClauseItem, new MenuItem("-"), AddAdditionalClauseMenuItem };
                    listViewAdditionalClauses.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddAdditionalClauseMenuItem  };
                    listViewAdditionalClauses.ContextMenu = new ContextMenu(mi);

                }

            }

        }

        #endregion
        private void listView_Resize(object sender, EventArgs e)
        {
            AdjustListViewColumnsWidth();
        }
        public void AdjustListViewColumnsWidth()
        {
            listViewItemOUT.Columns[0].Width = 80;
            
            listViewItemOUT.Columns[1].Width = (listViewItemOUT.Width - 80) /8 - 1;
            listViewItemOUT.Columns[2].Width = (listViewItemOUT.Width - 80) /8 - 1;
            listViewItemOUT.Columns[3].Width = (listViewItemOUT.Width - 80) / 8 - 1;
            listViewItemOUT.Columns[4].Width = (listViewItemOUT.Width - 80) / 8 - 1;
            listViewItemOUT.Columns[5].Width = (listViewItemOUT.Width - 80) /8 - 1;
            listViewItemOUT.Columns[6].Width = (listViewItemOUT.Width - 80) / 8 - 1;
            listViewItemOUT.Columns[7].Width = (listViewItemOUT.Width - 80) / 8 - 1;
            listViewItemOUT.Columns[8].Width = (listViewItemOUT.Width - 80) / 8 - 1;


        }

        private void textBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {


                double discount = Convert.ToDouble(textBoxDiscount.Text);
                textBoxClearValue.Text = (TotalCost   - discount).ToString() + " " + _BillSell  ._Currency.CurrencySymbol;
            }
            catch
            {
                textBoxClearValue.Text = " - " + _BillSell._Currency.CurrencySymbol;
            }

        }

        private void comboBoxCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {

            RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCUrrentExchangeRate.Checked)
                comboBoxCurrency.Enabled = true;
            else comboBoxCurrency.Enabled = false;
            RefreshSellOperations(_ItemOUTList, _BillAdditionalClauseList);
        }

        private void comboBoxBillINCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxBillINCurrency .SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);
            textBoxBillINExchangeRate.Text = currency.ExchangeRate .ToString();
        }

        private void FillBillSellPays()
        {
            try
            {
                listViewPays.Items.Clear();
                if (_BillSell == null)
                {
                    textBoxPays.Text = "-";
                    return;
                }

                List<PayIN> Bill_Pays = new List<PayIN>();
                Bill_Pays = new PayINSQL(DB).GetPayINList(_BillSell ._Operation);
                textBoxPays.Text = GetPaysReport(Bill_Pays);
                for (int i = 0; i < Bill_Pays.Count; i++)
                {
                    ListViewItem ListViewItem_ = new ListViewItem(Bill_Pays[i].PayOprID.ToString());
                    ListViewItem_.Name = Bill_Pays[i].PayOprID.ToString();
                    ListViewItem_.SubItems.Add(Bill_Pays[i].PayOprDate.ToShortDateString());
                    ListViewItem_.SubItems.Add(Bill_Pays[i].PayDescription);
                    ListViewItem_.SubItems.Add(Bill_Pays[i].Value.ToString());
                    ListViewItem_.SubItems.Add(Bill_Pays[i]._Currency.CurrencyName);
                    ListViewItem_.SubItems.Add(Bill_Pays[i].ExchangeRate.ToString());
                    ListViewItem_.BackColor = Color.Orange;
                    listViewPays.Items.Add(ListViewItem_);
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show("FillBillSellPays:" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }

        private string GetPaysReport(List<PayIN > bill_Pays)
        {
            PaysValue = 0;
            if (bill_Pays.Count == 0)
            {
                PaysValue = 0; return "-";
            }
            string pays = "";
            try
            {
                List<Currency> currencyList = bill_Pays.Select(x => x._Currency).Distinct().ToList();

                for (int i = 0; i < currencyList.Count; i++)
                {

                    List<PayIN > temppaysList = bill_Pays.Where(x => x._Currency == currencyList[i]).ToList();
                    for (int j = 0; j < temppaysList.Count; j++)
                    {
                        double factor;
                        if (temppaysList[j]._Currency.CurrencyID != _BillSell._Currency.CurrencyID)
                            factor = _BillSell._Currency.ExchangeRate  / temppaysList[j]._Currency.ExchangeRate ;
                        else
                            factor = 1;
                        PaysValue = PaysValue + temppaysList[j].Value * factor;
                        pays = System.Math.Round((temppaysList[j].Value), 3) + currencyList[i].CurrencySymbol + " ";
                    }
                }

            }
            catch
            {

            }
            return pays;
        }

        private void listViewPays_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            uint payinid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name);
            PayIN PayIN_ = new PayINSQL(DB).GetPayIN_INFO_BYID(payinid );
            AccountingObj.Forms.PayINForm PayINForm_ = new AccountingObj.Forms.PayINForm(DB, PayIN_ , false);
            PayINForm_.ShowDialog();

        }

        private void listViewPays_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewPays.Columns.Count; i++)
                listViewPays.Columns[i].Width = listViewPays.Width / 3;
        }
        private void FillComboBoxSellType(string   SellType_)
        {
            comboBoxSellType.Items.Clear();
            int selected_index = 0;
            try
            {

                for (int i = 0; i < selltypelist.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(selltypelist[i].SellTypeName, selltypelist[i].SellTypeID);
                    comboBoxSellType.Items.Add(item);
                    if(SellType_!=null)
                    if (SellType_  == selltypelist[i].SellTypeName) selected_index = i;
                }
                comboBoxSellType.SelectedIndex = selected_index;
            }
            catch (Exception ee)
            {
                MessageBox.Show("FillComboBoxSellType" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

 
        private void listViewPays_MouseDown(object sender, MouseEventArgs e)
        {
            listViewPays.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewPays.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenPayIN_MenuItem, EditPayIN_MenuItem, DeletePayIN_MenuItem, new MenuItem("-"), AddPayIN_MenuItem };
                    listViewPays.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddPayIN_MenuItem };
                    listViewPays.ContextMenu = new ContextMenu(mi);

                }

            }
        }
    }
}
