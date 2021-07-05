using ItemProject.AccountingObj.AccountingSQL;
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
    public partial class BillOUTReportForm : Form
    {
       
        //DatabaseInterface DB;
        //Contact _Contact;
        //BillOUT _BillOUT;
        //public bool _Changed;
        //public bool Changed
        //{
        //    get { return _Changed; }
        //}
        //double TotalCost;
        //System.Windows.Forms.MenuItem OpenBuyOprMenuItem;
        //System.Windows.Forms.MenuItem AddBuyOprMenuItem;
        //System.Windows.Forms.MenuItem EditBuyOprMenuItem;
        //System.Windows.Forms.MenuItem DeleteBuyOprMenuItem;

        //List<BuyOPR> _BuyOPRList = new List<BuyOPR>();

        //public BillOUTReportForm(DatabaseInterface db, BillOUT BillOUT_)
        //{
        //    InitializeComponent();
        //    DB = db;
        //    _BillOUT = BillOUT_;
        //    _Changed = false;

        //    InitializeMenuItems();
        //    loadForm( );


        //}
        // public void InitializeMenuItems()
        //{
        //    OpenBuyOprMenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل عملية شراء", OpenBuyOpr_MenuItem_Click);

        //    AddBuyOprMenuItem = new System.Windows.Forms.MenuItem("اضافة عملية شراء", AddBuyOpr_MenuItem_Click);
        //    EditBuyOprMenuItem = new System.Windows.Forms.MenuItem("تعديل ", EditBuyOpr_MenuItem_Click);
        //    DeleteBuyOprMenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteBuyOpr_MenuItem_Click); ;

        //}

        //private void DeleteBuyOpr_MenuItem_Click(object sender, EventArgs e)
        //{
        //    DialogResult dd = MessageBox.Show("هل أنت متأكد من حذف هذا البند", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        //    if (dd != DialogResult.OK) return;
        //    string buyoprid_Str = listView .SelectedItems[0].Name;
        //    uint buyoprid = Convert.ToUInt32(buyoprid_Str);
        //    bool Success = new BuyOPRSQL(DB).DeleteBuyOPR(buyoprid);
        //    if (Success)
        //    {
        //        _BuyOPRList = new BuyOPRSQL(DB).GetBuyOPRList(_BillOUT);
        //        RefreshBuyOperations(_BuyOPRList );
        //    }
        //}

        //private void EditBuyOpr_MenuItem_Click(object sender, EventArgs e)
        //{
        //    if ( listView.SelectedItems.Count > 0)
        //    {
        //        uint buyoprid = Convert.ToUInt32(listView.SelectedItems[0].Name);
        //        BuyOPR buyopr = new BuyOPRSQL(DB).GetBuyOPRINFO_BYID(buyoprid);
        //        ItemINForm BuyOprForm_ = new ItemINForm(DB, buyopr,true );
        //        BuyOprForm_.ShowDialog();
        //        if (BuyOprForm_.Changed)
        //        {
        //            _BuyOPRList = new BuyOPRSQL(DB).GetBuyOPRList(_BillOUT);
        //            RefreshBuyOperations(_BuyOPRList);
        //        }
        //        BuyOprForm_.Dispose();
                    
        //    }
        //}
        //private void OpenBuyOpr_MenuItem_Click(object sender, EventArgs e)
        //{
        //    if (listView.SelectedItems.Count > 0)
        //    {
        //        uint buyoprid = Convert.ToUInt32(listView.SelectedItems[0].Name);
        //        BuyOPR buyopr = new BuyOPRSQL(DB).GetBuyOPRINFO_BYID(buyoprid);
        //        ItemINForm BuyOprForm_ = new ItemINForm(DB, buyopr, false );
        //        BuyOprForm_.ShowDialog();
        //        if (BuyOprForm_.Changed)
        //        {
        //            _BuyOPRList = new BuyOPRSQL(DB).GetBuyOPRList(_BillOUT);
        //            RefreshBuyOperations(_BuyOPRList);
        //        }
        //        BuyOprForm_.Dispose();

        //    }
        //}

        //private void AddBuyOpr_MenuItem_Click(object sender, EventArgs e)
        //{
        //    ItemINForm BuyOprForm_ = new ItemINForm(DB, _BillOUT );
        //    DialogResult d = BuyOprForm_.ShowDialog();
        //    if (BuyOprForm_.Changed)
        //    {
        //        _BuyOPRList = new BuyOPRSQL(DB).GetBuyOPRList(_BillOUT);
        //        RefreshBuyOperations(_BuyOPRList);
        //    }
        //}

        //private void RefreshBuyOperations(List <BuyOPR > BuyOPRList)
        //{
        //    listView.Items.Clear();
        //    ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxCurrency .SelectedItem;
        //    Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);
        //    if (_BillOUT == null) return;
           
        //    if (BuyOPRList.Count == 0)
        //    {
        //        panelShowBillDataByCurrency.Enabled = false;
        //        return;
        //    }
        //    else panelShowBillDataByCurrency.Enabled = true ;
        //    double Transtion_factor;
        //    Currency tempcurrency;
        //    if (checkBoxCUrrentExchangeRate .Checked )
        //    {
        //        tempcurrency = currency;
        //         Transtion_factor = (currency.ExchangeRate / _BillOUT.ExchangeRate);
        //        textBoxExchangeRate.Text = currency.ExchangeRate.ToString();
        //    }
        //    else
        //    {
        //        tempcurrency = _BillOUT._Currency;
        //        Transtion_factor = 1;
        //        textBoxExchangeRate.Text = currency.ExchangeRate.ToString();
        //    }
          



        //   double totalcost = 0;
           
        //    for (int i=0;i< BuyOPRList.Count;i++)
        //    {

        //        double buyprice = System.Math.Round(BuyOPRList[i].BuyPrice * Transtion_factor,3);
        //        double total_buyprice = System.Math.Round(buyprice * BuyOPRList[i].Amount,3);
        //        totalcost = totalcost + total_buyprice;
        //        ListViewItem ListViewItem_ = new ListViewItem((listView .Items .Count +1).ToString ());
        //        ListViewItem_.Name = BuyOPRList[i].BuyOPRID.ToString();
        //        ListViewItem_.SubItems.Add(BuyOPRList[i]._Item.ItemName );
        //        ListViewItem_.SubItems.Add(BuyOPRList[i]._Item.ItemCompany );
        //        ListViewItem_.SubItems.Add(BuyOPRList[i]._Item.folder.FolderName);
        //        ListViewItem_.SubItems.Add(BuyOPRList[i]._TradeState .TradeStateName);
        //        ListViewItem_.SubItems.Add(BuyOPRList[i].Amount.ToString () );
        //        ListViewItem_.SubItems.Add(BuyOPRList[i]._ConsumeUnit .ConsumeUnitName.ToString());
        //        ListViewItem_.SubItems.Add(buyprice.ToString() + " " + tempcurrency .CurrencySymbol.Replace(" ", string.Empty));
        //        ListViewItem_.SubItems.Add((total_buyprice).ToString() + " " + tempcurrency .CurrencySymbol.Replace(" ", string.Empty));



        //        ListViewItem_.BackColor = Color.LimeGreen;
        //        listView.Items.Add(ListViewItem_);

        //    }
        //    TotalCost = totalcost;
        //    if (totalcost >0)
        //    {
        //        double discount;
        //        try
        //        {
        //            discount = Convert.ToDouble(textBoxDiscount.Text)*Transtion_factor ;
        //            textBoxValue.Text = totalcost .ToString() + " " + tempcurrency .CurrencySymbol.Replace(" ", string.Empty);
        //            textBoxDiscount.Enabled = true;
        //            textBoxDiscount.Text = _BillOUT.Discount.ToString();
        //            textBoxClearValue.Text = (totalcost - _BillOUT.Discount).ToString() + " " + tempcurrency .CurrencySymbol.Replace(" ", string.Empty);
        //        }
        //        catch
        //        {

        //        }

        //    }
 
        //    AdjustListViewColumnsWidth();
        //}

        //private void FillComboBoxCurrency(Currency currency)
        //{
        //    comboBoxCurrency.Items.Clear();
        //    int selected_index = 0;
        //    try
        //    {
        //        List<Currency> CurrencyList  = new CurrencySQL(DB).GetCurrencyList();
        //        for (int i = 0; i < CurrencyList.Count; i++)
        //        {
        //            ComboboxItem item = new ComboboxItem(CurrencyList[i].CurrencyName + "(" + CurrencyList[i].CurrencySymbol + ")", CurrencyList[i].CurrencyID);
        //            comboBoxCurrency.Items.Add(item);
        //                if (currency != null && currency.CurrencyID  == CurrencyList[i].CurrencyID )
        //                selected_index = i;
        //        }
        //        comboBoxCurrency.SelectedIndex = selected_index;
               
        //    }
        //    catch
        //    { }
      
        //}

        //private void textBoxContact_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        TradeContact.ShowContactsForm form = new TradeContact.ShowContactsForm(DB, true);
        //        DialogResult dd= form.ShowDialog();
        //        if (dd == DialogResult.OK)
        //        {
        //            _Contact = form.Contact_;
        //            textBoxContact.Text  =_Contact .GetContactTypeHeader  () +":"+ _Contact.ContactName;
        //        }
        //    }
        //}
        //public void loadForm()
        //{

        //    textBoxDiscount.Enabled = false;
        //    listView.Enabled = true;
        //    if(_BillOUT !=null)
        //    {
        //        _BuyOPRList = new BuyOPRSQL(DB).GetBuyOPRList(_BillOUT);
        //        _Contact = _BillOUT._Contact;
        //        textBoxContact.Text = _BillOUT ._Contact.ContactName;
        //        dateTimePicker_.Value = _BillOUT.BillOUTDate;
             
        //        FillComboBoxCurrency(_BillOUT._Currency);
        //        textBoxBillOUTExchangeRate .Text = _BillOUT.ExchangeRate.ToString ();
        //        textBoxDescription.Text = _BillOUT.BillDescription;
        //        TextboxNotes.Text = _BillOUT.Notes;
        //        labelBillID.Text = "فاتورة استيراد مواد رقم:" + _BillOUT.BillOUTID.ToString();
        //        RefreshBuyOperations(_BuyOPRList);
        //        //if (edit)
        //        //{
        //        //    panelShowBillDataByCurrency.Visible = false ;

        //        //    this.textBoxContact.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxContact_MouseDoubleClick);
        //        //    textBoxDescription.ReadOnly  = false ;
        //        //    dateTimePicker_.Enabled = true;
        //        //    textBoxBillOUTExchangeRate.ReadOnly  = false ;
        //        //    comboBoxBillOUTCurrency.Enabled = true;
        //        //    TextboxNotes.ReadOnly = false ;
        //        //}
        //        //else
        //        //{
        //        //    this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
        //        //    this.listView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDown);

        //        //    panelShowBillDataByCurrency.Visible = true;
        //        //    textBoxDescription.ReadOnly = true  ;
        //        //    dateTimePicker_.Enabled = false ;
        //        //    textBoxBillOUTExchangeRate.ReadOnly = true  ;
        //        //    comboBoxBillOUTCurrency.Enabled = false ;
        //        //    TextboxNotes.ReadOnly = true  ;

        //        //}
        //    }
        //}
        
        //private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left && listView.SelectedItems.Count > 0)
        //    {
        //        OpenBuyOprMenuItem.PerformClick();
        //    }
        //}
        //private void listView_MouseDown(object sender, MouseEventArgs e)
        //{
        //    listView.ContextMenu = null;
        //    bool match = false;
        //    ListViewItem listitem = new ListViewItem();
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    {
        //        foreach (ListViewItem item1 in listView.Items)
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


        //                MenuItem[] mi1 = new MenuItem[] {OpenBuyOprMenuItem ,AddBuyOprMenuItem  ,EditBuyOprMenuItem  ,DeleteBuyOprMenuItem };
        //                listView.ContextMenu = new ContextMenu(mi1);


        //        }
        //        else
        //        {

        //            MenuItem[] mi = new MenuItem[] { AddBuyOprMenuItem  };
        //            listView.ContextMenu = new ContextMenu(mi);

        //        }

        //    }

        //}

        //private void listView_Resize(object sender, EventArgs e)
        //{
        //    AdjustListViewColumnsWidth();
        //}
        //public void AdjustListViewColumnsWidth()
        //{
        //    listView.Columns[0].Width = 80;
            
        //    listView.Columns[1].Width = (listView.Width - 80) / 8 - 1;
        //    listView.Columns[2].Width = (listView.Width - 80) / 8 - 1;
        //    listView.Columns[3].Width = (listView.Width - 80) / 8 - 1;
        //    listView.Columns[4].Width = (listView.Width - 80) / 8 - 1;
        //    listView.Columns[5].Width = (listView.Width - 80) / 8 - 1;
        //    listView.Columns[6].Width = (listView.Width - 80) / 8 - 1;
        //    listView.Columns[7].Width = (listView.Width - 80) / 8 - 1;
        //    listView.Columns[8].Width = (listView.Width - 80) / 8 - 1;


        //}

        //private void textBoxDiscount_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        double discount = Convert.ToDouble(textBoxDiscount.Text);
        //        textBoxClearValue.Text = (TotalCost  - discount).ToString() + " " + _BillOUT._Currency.CurrencySymbol;
        //    }
        //    catch
        //    {
        //        textBoxClearValue.Text = " - " + _BillOUT._Currency.CurrencySymbol;
        //    }

        //}

        //private void comboBoxCurrency_SelectedIndexChanged(object sender, EventArgs e)
        //{
            
        //    RefreshBuyOperations(_BuyOPRList);
        //}

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    if(checkBoxCUrrentExchangeRate.Checked )
        //        comboBoxCurrency.Enabled = true;
        //    else comboBoxCurrency.Enabled = false ;
        //    RefreshBuyOperations(_BuyOPRList);
        //}

 
    }
}
