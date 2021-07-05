﻿using ItemProject.AccountingObj.AccountingSQL;
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
    public partial class BillBuyForm : Form
    {

        DatabaseInterface DB;
        Contact _Contact;
        BillBuy _BillBuy;
        public bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }
        double TotalCost;
        double PaysValue;
        System.Windows.Forms.MenuItem OpenBuyOprMenuItem;
        System.Windows.Forms.MenuItem AddBuyOprMenuItem;
        System.Windows.Forms.MenuItem EditBuyOprMenuItem;
        System.Windows.Forms.MenuItem DeleteBuyOprMenuItem;

        System.Windows.Forms.MenuItem OpenAdditionalClauseMenuItem;
        System.Windows.Forms.MenuItem AddAdditionalClauseMenuItem;
        System.Windows.Forms.MenuItem EditAdditionalClauseItem;
        System.Windows.Forms.MenuItem DeleteAdditionalClauseItem;

        System.Windows.Forms.MenuItem OpenPayOUT_MenuItem;
        System.Windows.Forms.MenuItem AddPayOUT_MenuItem;
        System.Windows.Forms.MenuItem EditPayOUT_MenuItem;
        System.Windows.Forms.MenuItem DeletePayOUT_MenuItem;

        List<ItemIN_ItemOUTReport> ItemIN_ItemOUTReportList = new List<ItemIN_ItemOUTReport>();
        List<BillAdditionalClause> _BillAdditionalClauseList = new List<BillAdditionalClause>();

        public BillBuyForm(DatabaseInterface db, DateTime BillOutDate_, Contact Contact_)
        {
            DB = db;
            TotalCost = 0;
            InitializeComponent();
            _Contact = Contact_;
            this.textBoxContact.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxContact_MouseDoubleClick);

            panelShowBillDataByCurrency.Visible = false;
            textBoxBillOUTExchangeRate.Enabled = true;
            comboBoxBillOUTCurrency.Enabled = true;
            panelShowBillDataByCurrency.Visible = false;
            _Changed = false;
            if (Contact_ != null) textBoxContact.Text = Contact_.Get_Complete_ContactName_WithHeader();
            dateTimePicker_.Value = BillOutDate_;
            FillComboBoxCurrency(ProgramGeneralMethods.GetDefaultCurrency(DB));
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "انشاء فاتورة";
            textBoxDiscount.Enabled = false;
            tabControl1 .Enabled = false;
            InitializeMenuItems();
            AdjustListViewColumnsWidth();
            listViewPays.Enabled = false;

        }
        public BillBuyForm(DatabaseInterface db, BillBuy BillBuy_, bool edit)
        {
            InitializeComponent();
            DB = db;
            _BillBuy = BillBuy_;
            _Changed = false;

            InitializeMenuItems();

            loadForm(edit);
            AdjustListViewColumnsWidth();


        }
        public void InitializeMenuItems()
        {
            OpenBuyOprMenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية شراء", OpenBuyOpr_MenuItem_Click);

            AddBuyOprMenuItem = new System.Windows.Forms.MenuItem("اضافة عملية شراء", AddBuyOpr_MenuItem_Click);
            EditBuyOprMenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditBuyOpr_MenuItem_Click);
            DeleteBuyOprMenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteBuyOpr_MenuItem_Click); ;

            AddAdditionalClauseMenuItem = new System.Windows.Forms.MenuItem("انشاء بند اضافي", AddAdditionalClause_MenuItem_Click);
            EditAdditionalClauseItem  = new System.Windows.Forms.MenuItem("تعديل ", EditAdditionalClause_MenuItem_Click);
            DeleteAdditionalClauseItem  = new System.Windows.Forms.MenuItem("حذف", DeleteAdditionalClause_MenuItem_Click); ;
            OpenAdditionalClauseMenuItem  = new System.Windows.Forms.MenuItem("فتح", OpenAdditionalClause_MenuItem_Click);

            OpenPayOUT_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية الدفع", OpenPayOUT_MenuItem_Click);
            AddPayOUT_MenuItem = new MenuItem("اضافة دفعة تابعة لهذه الفاتورة", AddPayOUT_MenuItem_Click);
            EditPayOUT_MenuItem = new MenuItem("تعديل", EditPayOUT_MenuItem_Click);
            DeletePayOUT_MenuItem = new MenuItem("حذف", DeletePayOUT_MenuItem_Click);

        }

        private void OpenPayOUT_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewPays.SelectedItems.Count > 0)
                {
                    uint payoutid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name);
                    PayOUT PayOUT_ = new PayOUTSQL(DB).GetPayOUT_INFO_BYID(payoutid);
                    AccountingObj.Forms.PayOUTForm PayOUTForm_ = new AccountingObj.Forms.PayOUTForm(DB, PayOUT_, false);
                    PayOUTForm_.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void DeletePayOUT_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dd != DialogResult.OK) return;
                uint payoutid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name );
                bool success = new PayOUTSQL(DB).Delete_PayOUT (payoutid);
                if (success)
                {
                    FillBillBuyPays();
                }
            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void EditPayOUT_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewPays.SelectedItems.Count > 0)
                {
                    uint payoutid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name);
                    PayOUT PayOUT_ = new PayOUTSQL(DB).GetPayOUT_INFO_BYID(payoutid);
                    AccountingObj.Forms.PayOUTForm PayOUTForm_ = new AccountingObj.Forms.PayOUTForm(DB, PayOUT_, true );
                    PayOUTForm_.ShowDialog();
                    if (PayOUTForm_.Changed)
                    {
                        FillBillBuyPays();

                    }
                }
               
               
            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void AddPayOUT_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PayOUTForm PayOUTForm_ = new PayOUTForm(DB, DateTime.Now ,_BillBuy);
                PayOUTForm_.ShowDialog();
                if (PayOUTForm_.DialogResult == DialogResult.OK)
                {
                    FillBillBuyPays();
                }
            }
            catch
            {
                MessageBox.Show("حدث خطا", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void DeleteBuyOpr_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dd = MessageBox.Show("هل أنت متأكد من حذف هذا البند", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd != DialogResult.OK) return;
            string iteminid_Str = listViewItemsIN.SelectedItems[0].Name;
            uint iteminid = Convert.ToUInt32(iteminid_Str);
            bool Success = new ItemINSQL(DB).DeleteItemIN(iteminid);
            if (Success)
            {
                ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_BillBuy._Operation);
                RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
            }
        }

        private void EditBuyOpr_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewItemsIN.SelectedItems.Count > 0)
            {
                uint iteminid = Convert.ToUInt32(listViewItemsIN.SelectedItems[0].Name);
                ItemIN itemin = new ItemINSQL(DB).GetItemININFO_BYID(iteminid);
                ItemINForm BuyOprForm_ = new ItemINForm(DB, itemin, true);
                BuyOprForm_.ShowDialog();
                if (BuyOprForm_.Changed)
                {
                    ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_BillBuy._Operation);
                    RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
                }
                BuyOprForm_.Dispose();

            }
        }
        private void OpenBuyOpr_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewItemsIN.SelectedItems.Count > 0)
            {
                uint iteminid = Convert.ToUInt32(listViewItemsIN.SelectedItems[0].Name);
                ItemIN itemin = new ItemINSQL(DB).GetItemININFO_BYID(iteminid);
                ItemINForm ItemINForm_ = new ItemINForm(DB, itemin, false);
                ItemINForm_.ShowDialog();
                if (ItemINForm_.Changed)
                {
                    ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_BillBuy._Operation);
                    RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
                }
                ItemINForm_.Dispose();

            }
        }

  

        private void AddBuyOpr_MenuItem_Click(object sender, EventArgs e)
        {

            ItemINForm BuyOprForm_ = new ItemINForm(DB, _BillBuy._Operation);
            DialogResult d = BuyOprForm_.ShowDialog();
            if (BuyOprForm_.Changed)
            {
                ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_BillBuy._Operation);
                RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
            }
        }
        private async  void RefreshAdditionalClause(List<BillAdditionalClause> AdditionalClauseList_)
        {
            listViewAdditionalClauses.Items.Clear();
            for (int i = 0; i < AdditionalClauseList_.Count; i++)
            {
                ListViewItem ListViewItem_ = new ListViewItem((listViewAdditionalClauses .Items.Count + 1).ToString());
                //ListViewItem_.UseItemStyleForSubItems = false;
                ListViewItem_.Name = AdditionalClauseList_[i].Description ;
                ListViewItem_.SubItems.Add(AdditionalClauseList_[i].Value .ToString() + " " + _BillBuy ._Currency.CurrencySymbol.Replace(" ", string.Empty));
                listViewAdditionalClauses.Items.Add(ListViewItem_);
            }
       }
        private async  void RefreshBuyOperations(List<ItemIN_ItemOUTReport> ItemIN_ItemOUTReportList_
            , List<BillAdditionalClause> AdditionalClauseList_)
        {
            RefreshAdditionalClause(AdditionalClauseList_);
            listViewItemsIN.Items.Clear();
            ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);
            if (_BillBuy == null) return;

            if (ItemIN_ItemOUTReportList_.Count == 0)
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
                Transtion_factor = (currency.ExchangeRate / _BillBuy.ExchangeRate);
                textBoxExchangeRate.Text = currency.ExchangeRate.ToString();
            }
            else
            {
                tempcurrency = _BillBuy._Currency;
                Transtion_factor = 1;
                textBoxExchangeRate.Text = currency.ExchangeRate.ToString();
            }




            double itemsin_value = 0;

            for (int i = 0; i < ItemIN_ItemOUTReportList_.Count; i++)
            {

                double buyprice = System.Math.Round(ItemIN_ItemOUTReportList_[i]._ItemIN._INCost .Value  * Transtion_factor, 3);
                double total_buyprice = System.Math.Round(buyprice * ItemIN_ItemOUTReportList_[i]._ItemIN.Amount, 3);
                itemsin_value = itemsin_value + total_buyprice;
                ListViewItem ListViewItem_ = new ListViewItem((listViewItemsIN.Items.Count + 1).ToString());
                //ListViewItem_.UseItemStyleForSubItems = false;
                ListViewItem_.Name = ItemIN_ItemOUTReportList_[i]._ItemIN.ItemINID.ToString();
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList_[i]._ItemIN._Item.ItemName);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList_[i]._ItemIN._Item.ItemCompany);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList_[i]._ItemIN._Item.folder.FolderName);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList_[i]._ItemIN._TradeState.TradeStateName);
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList_[i]._ItemIN.Amount.ToString());
                ListViewItem_.SubItems.Add(ItemIN_ItemOUTReportList_[i]._ItemIN._ConsumeUnit.ConsumeUnitName.ToString());
                ListViewItem_.SubItems.Add(buyprice.ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty));
                ListViewItem_.SubItems.Add((total_buyprice).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty));
                if (checkBoxShowDetails.Checked && listViewItemsIN.Name == "listViewDetails")
                {
                    double sellamount = GetSellsAmount(ItemIN_ItemOUTReportList_[i]);
                    if (sellamount == 0)
                    {
                        ListViewItem_.SubItems.Add("-");
                        ListViewItem_.SubItems.Add("-");
                    }
                    else
                    {
                        ListViewItem_.SubItems.Add(sellamount + ItemIN_ItemOUTReportList_[i]._ItemIN._ConsumeUnit.ConsumeUnitName);
                        ListViewItem_.SubItems.Add(GetItemOUTsCost(ItemIN_ItemOUTReportList_[i].ItemOUTList));
                    }
                    if (sellamount == ItemIN_ItemOUTReportList_[i]._ItemIN.Amount) ListViewItem_.BackColor = Color.Red ;
                    else
                        ListViewItem_.BackColor = Color.Orange  ;
                }
                else ListViewItem_.BackColor = Color.Orange ;


                listViewItemsIN.Items.Add(ListViewItem_);

            }

            double additionalcluses_cost = 0;
            listViewAdditionalClauses.Items.Clear();
            for (int i = 0; i < AdditionalClauseList_.Count; i++)
            {
                ListViewItem ListViewItem_ = new ListViewItem((listViewAdditionalClauses.Items.Count + 1).ToString());
                additionalcluses_cost = additionalcluses_cost +( AdditionalClauseList_[i].Value * Transtion_factor);
                ListViewItem_.Name = AdditionalClauseList_[i].ClauseID.ToString();
                ListViewItem_.SubItems.Add(AdditionalClauseList_ [i].Description);
                ListViewItem_.SubItems.Add(System.Math.Round((AdditionalClauseList_[i].Value * Transtion_factor),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty));
                listViewAdditionalClauses.Items.Add(ListViewItem_);
            }

            textBoxItemsinValue.Text = System.Math.Round(itemsin_value,3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty);
            textBoxAdditionalClausesValue.Text = System.Math.Round(additionalcluses_cost,3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty);
            TotalCost = additionalcluses_cost + itemsin_value;
            if (TotalCost > 0)
            {
                double discount;
                try
                {

                    discount = Convert.ToDouble(textBoxDiscount.Text) * Transtion_factor;
                    textBoxValue.Text = System.Math.Round(TotalCost,3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty);
                    textBoxDiscount.Enabled = true;
                    textBoxDiscount.Text = _BillBuy.Discount.ToString();
                    textBoxClearValue.Text = System.Math.Round((TotalCost - _BillBuy.Discount),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty);
                    textBoxPays.Text = System.Math.Round((PaysValue * Transtion_factor),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty); ;
                    textBoxRemain.Text = System.Math.Round((TotalCost - discount - PaysValue * Transtion_factor),3).ToString() + " " + tempcurrency.CurrencySymbol.Replace(" ", string.Empty); ;
                }
                catch
                {
                    textBoxClearValue.Text = "-";
                    textBoxRemain.Text = "-";
                }

            }


        }

        private string GetItemOUTsCost(List<ItemOUT> ItemOUTList)
        {
            if (ItemOUTList.Count == 0) return "-";
            string outcost = "";
            //try
            //{

            //    List<Currency> currencyList = ItemOUTList.Select(x => new OperationSQL(DB).GetOperationItemOUTCurrency(x._Operation)).Distinct().ToList();

            //    for (int i = 0; i < currencyList.Count; i++)
            //    {
            //        List<ItemOUT> tempoutlist = ItemOUTList.Where(x => new OperationSQL(DB).GetOperationItemOUTCurrency(x._Operation) == currencyList[i]).ToList();
            //        for (int j = 0; j < tempoutlist.Count; j++)
            //        {
            //            outcost = System.Math.Round((tempoutlist[j].Cost * tempoutlist[j].Amount), 3) + currencyList[i].CurrencySymbol + " ";
            //        }
            //    }

            //}
            //catch
            //{

            //}
            return outcost;

        }

        private double GetSellsAmount(ItemIN_ItemOUTReport ItemIN_ItemOUTReport_)
        {
            if (ItemIN_ItemOUTReport_.ItemOUTList.Count == 0) return 0;
            double amount = 0;
            try
            {

                for (int j = 0; j < ItemIN_ItemOUTReport_.ItemOUTList.Count; j++)
                {

                    amount = amount + System.Math.Round((ItemIN_ItemOUTReport_.ItemOUTList[j].Amount * (ItemIN_ItemOUTReport_.ItemOUTList[j]._ConsumeUnit.Factor / ItemIN_ItemOUTReport_._ItemIN._ConsumeUnit.Factor)), 3);

                }


            }
            catch
            {

            }
            return amount;
        }

        private void FillComboBoxCurrency(Currency currency)
        {
            comboBoxBillOUTCurrency.Items.Clear();
            comboBoxCurrency.Items.Clear();
            int selected_index = 0;
            try
            {
                List<Currency> CurrencyList = new CurrencySQL(DB).GetCurrencyList();
                for (int i = 0; i < CurrencyList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(CurrencyList[i].CurrencyName + "(" + CurrencyList[i].CurrencySymbol + ")", CurrencyList[i].CurrencyID);
                    comboBoxBillOUTCurrency.Items.Add(item);
                    comboBoxCurrency.Items.Add(item);
                    if (currency != null && currency.CurrencyID == CurrencyList[i].CurrencyID)
                        selected_index = i;
                }
                comboBoxBillOUTCurrency.SelectedIndex = selected_index;
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
                DialogResult dd = form.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    _Contact = form.Contact_;
                    textBoxContact.Text = _Contact.GetContactTypeHeader() + ":" + _Contact.ContactName;
                }
            }
        }
        public void loadForm(bool edit)
        {

            buttonSave.Name = "buttonSave";
            buttonSave.Text = "حفظ";
            textBoxDiscount.Enabled = false;
            tabControl1 .Enabled = true;
            if (_BillBuy != null)
            {
  
                ItemIN_ItemOUTReportList = new ItemINSQL(DB).GetItemIN_ItemOUTReport_List(_BillBuy._Operation);
                _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillBuy ._Operation);

                _Contact = _BillBuy._Contact;
                textBoxContact.Text = _BillBuy._Contact.Get_Complete_ContactName_WithHeader();
                dateTimePicker_.Value = _BillBuy.BillDate;

                FillComboBoxCurrency(_BillBuy._Currency);
                textBoxBillOUTExchangeRate.Text = _BillBuy.ExchangeRate.ToString();
                textBoxDescription.Text = _BillBuy.BillDescription;
                TextboxNotes.Text = _BillBuy.Notes;
                labelBillID.Text = "فاتورة شراء رقم:" + _BillBuy._Operation.OperationID.ToString();

                FillBillBuyPays();
                RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
                comboBoxCurrency.Enabled = false;
                if (edit)
                {
                    this.listViewItemsIN.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
                    this.listViewItemsIN.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDown);

                    this.listViewAdditionalClauses.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewAdditionalClause_MouseDoubleClick);
                    this.listViewAdditionalClauses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewAdditionalClause_MouseDown);

                    this.listViewPays.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewPays_MouseDoubleClick);
                    this.listViewPays.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewPays_MouseDown);


                    panelShowBillDataByCurrency.Visible = false;
                    
                    this.textBoxContact.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxContact_MouseDoubleClick);
                    textBoxDescription.ReadOnly = false;
                    dateTimePicker_.Enabled = true;
                    textBoxBillOUTExchangeRate.ReadOnly = false;
                    comboBoxBillOUTCurrency.Enabled = true;
                    TextboxNotes.ReadOnly = false;
                    textBoxDiscount.ReadOnly = false;
                  
                    buttonSave.Visible = true;
                }
                else
                {
                   
                    panelShowBillDataByCurrency.Visible = true;
                    textBoxDescription.ReadOnly = true;
                    dateTimePicker_.Enabled = false;
                    textBoxBillOUTExchangeRate.ReadOnly = true;
                    comboBoxBillOUTCurrency.Enabled = false;
                    TextboxNotes.ReadOnly = true;
                    textBoxDiscount.ReadOnly = true ;
                    buttonSave.Visible = false ;
                    this.checkBoxShowDetails.Visible = true;
                    this.checkBoxShowDetails.CheckedChanged += new System.EventHandler(this.checkBoxShowDetails_CheckedChanged);


                }
                listViewPays.Enabled = true;

            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            ComboboxItem item = (ComboboxItem)comboBoxBillOUTCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(item.Value);
            if (buttonSave.Name == "buttonAdd")
            {
                if (_Contact == null)
                {
                    MessageBox.Show("يرجى تحديد الجهة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double exchangerate;
                try
                {
                    exchangerate = Convert.ToDouble(textBoxBillOUTExchangeRate.Text);
                }
                catch
                {
                    MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                BillBuy billbuy = new BillBuySQL(DB).AddBillBuy(dateTimePicker_.Value, textBoxDescription.Text, _Contact, currency, exchangerate, 0, TextboxNotes.Text);
                if (billbuy != null)
                {
                    _BillBuy = billbuy;

                    MessageBox.Show("تم انشاء الفاتورة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this._Changed = true;
                    loadForm(true );

                }
            }

            else
            {
                if (_BillBuy != null)
                {
                    if (_Contact == null)
                    {
                        MessageBox.Show("يرجى تحديد الجهة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    double exchangerate;
                    try
                    {
                        exchangerate = Convert.ToDouble(textBoxBillOUTExchangeRate.Text);
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
                        if (discount > TotalCost)
                        {
                            MessageBox.Show("الخصم يجب ان يكون اقل من قيمة الفاتورة", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("الخصم يجب ان يكون رقم حقيقي", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    bool success = new BillBuySQL(DB).UpdateBillBuy(_BillBuy._Operation.OperationID, dateTimePicker_.Value, textBoxDescription.Text, _Contact, currency, exchangerate, discount, TextboxNotes.Text);
                    if (success == true)
                    {
                        MessageBox.Show("تم حفظ الفاتورة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _BillBuy = new BillBuySQL(DB).GetBillBuy_INFO_BYID(_BillBuy._Operation.OperationID);
                        this._Changed = true;
                        loadForm(true);
                    }
                }
            }
        }
        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewItemsIN.SelectedItems.Count > 0)
            {
                OpenBuyOprMenuItem.PerformClick();
            }
        }
        private void listView_MouseDown(object sender, MouseEventArgs e)
        {
            listViewItemsIN.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewItemsIN.Items)
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


                    MenuItem[] mi1 = new MenuItem[] { OpenBuyOprMenuItem, EditBuyOprMenuItem, DeleteBuyOprMenuItem, new MenuItem("-"), AddBuyOprMenuItem };
                    listViewItemsIN.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddBuyOprMenuItem };
                    listViewItemsIN.ContextMenu = new ContextMenu(mi);

                }

            }

        }



        private void listView_Resize(object sender, EventArgs e)
        {
            AdjustListViewColumnsWidth();
        }
        public void AdjustListViewColumnsWidth()
        {
            listViewItemsIN.Columns[0].Width = 80;

            for (int i = 1; i < listViewItemsIN.Columns.Count; i++)
                listViewItemsIN.Columns[i].Width = ((listViewItemsIN.Width - 80) / (listViewItemsIN.Columns.Count - 1)) - 1;
        }

        private void textBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double discount = Convert.ToDouble(textBoxDiscount.Text);
                textBoxClearValue.Text = (TotalCost - discount).ToString() + " " + _BillBuy._Currency.CurrencySymbol;
                textBoxRemain.Text = (TotalCost - discount - PaysValue).ToString() + " " + _BillBuy._Currency.CurrencySymbol;
            }
            catch
            {
                textBoxClearValue.Text = " - " + _BillBuy._Currency.CurrencySymbol;
                textBoxRemain.Text = " - " + _BillBuy._Currency.CurrencySymbol;
            }

        }

        private void comboBoxCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {

            RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCUrrentExchangeRate.Checked)
                comboBoxCurrency.Enabled = true;
            else comboBoxCurrency.Enabled = false;
            RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
        }

        private void comboBoxBillOUTCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxBillOUTCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);
            textBoxBillOUTExchangeRate.Text = currency.ExchangeRate.ToString();
        }

        private void checkBoxShowDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowDetails.Checked)
            {
                if (listViewItemsIN.Name != "listViewDetails")
                    ConvertListViewToDetails();

            }
            else
            {
                if (listViewItemsIN.Name != "listView")
                    ConvertListViewToSimple();

            }
            AdjustListViewColumnsWidth();
            RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
        }

        private void ConvertListViewToSimple()
        {
            if (listViewItemsIN.Name != "listView")
            {
                listViewItemsIN.Items.Clear();
                listViewItemsIN.Name = "listView";
                if (listViewItemsIN.Columns.Count > 9)
                {
                    listViewItemsIN.Columns.RemoveAt(10);
                    listViewItemsIN.Columns.RemoveAt(9);
                }

            }
        }

        private void ConvertListViewToDetails()
        {

            if (listViewItemsIN.Name != "listViewDetails")
            {
                listViewItemsIN.Items.Clear();
                listViewItemsIN.Name = "listViewDetails";
                if (listViewItemsIN.Columns.Count < 10)
                {

                    listViewItemsIN.Columns.Add("الكمية المباعة");
                    listViewItemsIN.Columns.Add("مبلغ المبيع");
                }
            }
        }
        private async  void FillBillBuyPays()
        {
            try
            {
                listViewPays.Items.Clear();
                if (_BillBuy == null)
                {
                    textBoxPays.Text = "-";
                    return;
                }

                List<PayOUT> Bill_Pays = new List<PayOUT>();
                Bill_Pays = new PayOUTSQL(DB).GetPaysOUT_List(_BillBuy._Operation);
                textBoxPays.Text = GetPaysReport(Bill_Pays);
                for (int i = 0; i < Bill_Pays.Count; i++)
                {
                    ListViewItem ListViewItem_ = new ListViewItem(Bill_Pays[i].PayOprID.ToString());
                    ListViewItem_.Name = Bill_Pays[i].PayOprID.ToString();
                    ListViewItem_.SubItems.Add(Bill_Pays[i].PayOprDate .ToShortDateString ());
                    ListViewItem_.SubItems.Add(Bill_Pays[i].PayDescription );
                    ListViewItem_.SubItems.Add(Bill_Pays[i].Value.ToString());
                    ListViewItem_.SubItems.Add(Bill_Pays[i]._Currency.CurrencyName);
                    ListViewItem_.SubItems.Add(Bill_Pays[i].ExchangeRate .ToString ());
                    ListViewItem_.BackColor = Color.Orange;
                    listViewPays.Items.Add(ListViewItem_);
                }

            }
            catch(Exception ee)
            {
                MessageBox.Show("FillBillBuyPays:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }

        private string GetPaysReport(List<PayOUT> bill_Pays)
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

                    List<PayOUT> temppaysList = bill_Pays.Where(x => x._Currency == currencyList[i]).ToList();
                    for (int j = 0; j < temppaysList.Count; j++)
                    {
                        double factor;
                        if (temppaysList[j]._Currency.CurrencyID != _BillBuy._Currency.CurrencyID)
                            factor = _BillBuy._Currency.ExchangeRate / temppaysList[j]._Currency.ExchangeRate;
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


            uint payoutid = Convert.ToUInt32(listViewPays.SelectedItems[0].Name);
            PayOUT PayOUT_ = new PayOUTSQL(DB).GetPayOUT_INFO_BYID(payoutid);
            AccountingObj.Forms.PayOUTForm PayOUTForm_ = new AccountingObj.Forms.PayOUTForm(DB, PayOUT_, false);
            PayOUTForm_.ShowDialog();

        }

        private void listViewPays_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewPays.Columns.Count; i++)
                listViewPays.Columns[i].Width = listViewPays.Width / 3;
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


                    MenuItem[] mi1 = new MenuItem[] { OpenPayOUT_MenuItem, EditPayOUT_MenuItem, DeletePayOUT_MenuItem, new MenuItem("-"), AddPayOUT_MenuItem };
                    listViewPays.ContextMenu = new ContextMenu(mi1);


                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddPayOUT_MenuItem };
                    listViewPays.ContextMenu = new ContextMenu(mi);

                }

            }
        }
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
                    _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillBuy._Operation);
                    RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);

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
                    _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillBuy._Operation);
                    RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
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
                    _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillBuy._Operation);
                    RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
                }
                BillAdditionalClauseForm_.Dispose();
            }
        }

        private void AddAdditionalClause_MenuItem_Click(object sender, EventArgs e)
        {
            BillAdditionalClauseForm BillAdditionalClauseForm_ = new BillAdditionalClauseForm(DB, _BillBuy._Operation);
            DialogResult d = BillAdditionalClauseForm_.ShowDialog();
            if (d == DialogResult.OK)
            {
                _BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses(_BillBuy._Operation);
                RefreshBuyOperations(ItemIN_ItemOUTReportList, _BillAdditionalClauseList);
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
        private void عرضتقريرالعناصرالخارجةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationItemsIN_ItemOutListForm OperationItemsIN_ItemOutListForm_ = new OperationItemsIN_ItemOutListForm(DB, _BillBuy._Operation);
            OperationItemsIN_ItemOutListForm_.ShowDialog();
        }

        private void طباعةالفاتورةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ItemIN> ItemINList = new ItemINSQL(DB).GetItemINList(_BillBuy._Operation);

            List<ItemProject.Reports.Objects.ItemIN_Report> ItemIN_ReportList 
                = Reports.Objects.ItemIN_Report.GetItemIN_ReportList(ItemINList);
            ItemProject.Reports.BillBuy_Print_Form BillBuy_Print_Form_
                = new Reports.BillBuy_Print_Form(ItemIN_ReportList);
            BillBuy_Print_Form_.ShowDialog();

        }
    }
}
