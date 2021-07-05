using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.Maintenance.MaintenanceSQL;
using ItemProject.Maintenance.Objects;
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

namespace ItemProject.AccountingObj.Forms
{
    public partial class PayINForm : Form
    {
      
        DatabaseInterface DB;
        Bill _Bill;
        PayIN _PayIN;

        public bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }
        bool Edit;
        public PayINForm(DatabaseInterface db,DateTime PayINDate_)
        {
            DB = db;
            InitializeComponent();
            this.Controls.Remove(panelBillIN_INFO);
            int h = this.Size.Height;
            this.MinimumSize = new Size(panelPayInfo.Height + 40, panelPayInfo.Width + 140);
            this.Size = new Size(panelPayInfo.Height +40, panelPayInfo.Width + 140);
            
            _Changed = false;
            dateTimePicker_.Value = PayINDate_;
            Currency defaultcurrency = ProgramGeneralMethods.GetDefaultCurrency(DB);
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, defaultcurrency);
            //Operation.FillComboBoxBillType_PayIN(ref comboBoxOperationType, Operation.BILL_SELL);
            textBoxPayExchangeRate.Text = defaultcurrency.ExchangeRate .ToString();
            buttonSave.Name = "buttonAdd";

        }
        public PayINForm(DatabaseInterface db, DateTime PayINDate_,Bill Bill_)
        {
            DB = db;
            InitializeComponent();
            _Bill    = Bill_ ;
           
            _Changed = false;
            dateTimePicker_.Value = PayINDate_;
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _Bill._Currency);
            textBoxPayExchangeRate.Text = _Bill ._Currency.ExchangeRate .ToString();
            buttonSave.Name = "buttonAdd";

        }

        public PayINForm(DatabaseInterface db, PayIN PayIN_,bool Edit_)
        {

            InitializeComponent();
            Edit = Edit_;
            DB = db;
            _PayIN = PayIN_;
            _Bill   = PayIN_._Bill  ;
 
            dateTimePicker_.Value = _PayIN.PayOprDate; ;
            _Changed = false;
            loadForm(Edit);
        }

        private void FillBillIN_PanelINFO()
        {

            if (_Bill == null) return;
            labelOwner.Text = "عائدة لـ " + Operation.GetOperationName(_Bill ._Operation .OperationType);
                textBoxBillIN_ID.Text = _Bill._Operation.OperationID .ToString();
                textBoxContact.Text = _Bill._Contact.Get_Complete_ContactName_WithHeader();

                textBoxBillINDate.Text = _Bill.BillDate .ToString("yyyy-mm-dd hh:mm");
                textBoxCurrency.Text = _Bill._Currency.CurrencyName;
                textBoxBillIN_ExchangeRate.Text = _Bill.ExchangeRate.ToString();
               
                double billvalue , paid ;
            billvalue = new OperationSQL(DB).Get_OperationValue(_Bill._Operation);
            paid = new OperationSQL(DB).Get_OperationPaysValue_UPON_OperationCurrency(_Bill._Operation);
            
                textBoxBillINValue.Text = billvalue.ToString()+" "+_Bill._Currency .CurrencySymbol ;
                textBoxPaid.Text = paid.ToString() + " " + _Bill._Currency.CurrencySymbol;
            textBoxRemain.Text = (billvalue - paid).ToString() + " " + _Bill._Currency.CurrencySymbol;
            textBoxPayValue .Text = (billvalue - paid).ToString();
                textBoxPayValue.Focus();
        
  
            
        }
    
        
  

      
        public void loadForm(bool edit)
        {

             buttonSave.Name = "buttonSave";
            if (_PayIN  !=null)
            {
                if (_PayIN ._Bill !=null )FillBillIN_PanelINFO();
                dateTimePicker_.Value = _PayIN .PayOprDate;
                textBoxPayDesc.Text = _PayIN.PayDescription;
                ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _PayIN._Currency);
                textBoxPayExchangeRate.Text = _PayIN._Currency.ExchangeRate .ToString();
                textBoxPayValue.Text = _PayIN.Value.ToString();
                TextboxNotes.Text = _PayIN.Notes;
                if (!edit)
                {
                    dateTimePicker_.Enabled = false;
                    textBoxPayDesc.ReadOnly = true;
                    comboBoxCurrency.Enabled = false;
                    textBoxPayExchangeRate.ReadOnly = true;
                    textBoxPayValue.ReadOnly = true;
                    TextboxNotes.ReadOnly = true;
                    buttonSave.Visible = false;
                } 
                
            }
            else
            {
                Currency defaultcurrency = ProgramGeneralMethods.GetDefaultCurrency(DB);
                ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, defaultcurrency);
                textBoxPayExchangeRate.Text = defaultcurrency.ExchangeRate .ToString();
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            ComboboxItem item = (ComboboxItem)comboBoxCurrency.SelectedItem;
            Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(item.Value);
            double payvalue, exchangerate;
            try
            {
                payvalue = Convert.ToDouble(textBoxPayValue.Text);
            }
            catch
            {
                MessageBox.Show("قيمة الدفعة يجب ان تكون رقم حقيقي او صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                exchangerate = Convert.ToDouble(textBoxPayExchangeRate.Text);
            }
            catch
            {
                MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي او صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Operation opr = (_Bill == null ? null : _Bill._Operation);
            if (buttonSave.Name == "buttonAdd")
            {

                try
                {
                    
                    bool Succes = new PayINSQL(DB).Add_PayIN(dateTimePicker_.Value, opr, textBoxPayDesc.Text, payvalue, exchangerate, currency, TextboxNotes.Text);
                    if (Succes)
                    {
                        MessageBox.Show("تم اضافة الدفعة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this._Changed = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("buttonSave_Click:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }

            }

            else
            {
                try
                {
                    bool Succes = new PayINSQL(DB).Update_PayIN(_PayIN.PayOprID, dateTimePicker_.Value, opr, textBoxPayDesc.Text, payvalue, exchangerate, currency, TextboxNotes.Text);
                    if (Succes)
                    {
                        MessageBox.Show("تم تعديل الدفعة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this._Changed = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("buttonSave_Click:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }

            }
        }

        private void PayINForm_Load(object sender, EventArgs e)
        {
            if(_Bill  !=null )
            {
                FillBillIN_PanelINFO();
            }
        }

        private void comboBoxCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxCurrency.SelectedItem;
                Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);
                textBoxPayExchangeRate.Text = currency.ExchangeRate .ToString();
            }
            catch
            {

            }
        }
    }
}
