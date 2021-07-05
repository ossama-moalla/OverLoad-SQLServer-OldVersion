using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.Company.Objects;
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
    public partial class PayOUTForm : Form
    {

        DatabaseInterface DB;
        Bill _Bill;
        EmployeePayOrder _EmployeePayOrder;
        PayOUT _PayOUT;

        public bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }
        bool Edit;
        public PayOUTForm(DatabaseInterface db, DateTime PayOUTDate_)
        {
            DB = db;
            InitializeComponent();
            this.Controls.Remove(panelOwner_INFO);
            int h = this.Size.Height;
            this.MinimumSize = new Size(panelPayOUTfo.Height + 40, panelPayOUTfo.Width + 140);
            this.Size = new Size(panelPayOUTfo.Height + 40, panelPayOUTfo.Width + 140);

            _Changed = false;
            dateTimePicker_.Value = PayOUTDate_;
            Currency defaultcurrency = ProgramGeneralMethods.GetDefaultCurrency(DB);
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, defaultcurrency);
            //Operation.FillComboBoxBillType_PayOUT(ref comboBoxOperationType, Operation.BILL_SELL);
            textBoxPayExchangeRate.Text = defaultcurrency.ExchangeRate.ToString();
            buttonSave.Name = "buttonAdd";

        }
        public PayOUTForm(DatabaseInterface db, DateTime PayOUTDate_, BillBuy Bill_)
        {
            DB = db;
            InitializeComponent();
            _Bill = Bill_;
            _EmployeePayOrder = null;
           _Changed = false;
            dateTimePicker_.Value = PayOUTDate_;
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _Bill._Currency);
            textBoxPayExchangeRate.Text = _Bill._Currency.ExchangeRate.ToString();
            buttonSave.Name = "buttonAdd";
            FillOwner_PanelINFO();
        }
        public PayOUTForm(DatabaseInterface db, DateTime PayOUTDate_, EmployeePayOrder EmployeePayOrder_)
        {
            DB = db;
            InitializeComponent();
            _Bill = null ;
            _EmployeePayOrder = EmployeePayOrder_;
            _Changed = false;
            dateTimePicker_.Value = PayOUTDate_;
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _EmployeePayOrder._Currency);
            textBoxPayExchangeRate.Text = _EmployeePayOrder._Currency.ExchangeRate.ToString();
            buttonSave.Name = "buttonAdd";
            FillOwner_PanelINFO();
        }

        public PayOUTForm(DatabaseInterface db, PayOUT PayOUT_, bool Edit_)
        {

            InitializeComponent();
            Edit = Edit_;
            DB = db;
            _PayOUT = PayOUT_;
            _Bill = PayOUT_._Bill;

            dateTimePicker_.Value = _PayOUT.PayOprDate; ;
            _Changed = false;
            FillOwner_PanelINFO();
            loadForm(Edit);
        }

        private async  void FillOwner_PanelINFO()
        {

            if (_Bill != null)
            {
                labelOwner.Text = "عائدة لـ " + Operation.GetOperationName(_Bill._Operation.OperationType);
                textBoxBillOUT_ID.Text = _Bill._Operation.OperationID.ToString();
                textBoxContact.Text = _Bill._Contact.Get_Complete_ContactName_WithHeader();

                textBoxBillOUTDate.Text = _Bill.BillDate.ToString("yyyy-mm-dd hh:mm");
                textBoxCurrency.Text = _Bill._Currency.CurrencyName;
                textBoxBillOUT_ExchangeRate.Text = _Bill.ExchangeRate.ToString();

                double billvalue, paid;
                billvalue = new OperationSQL(DB).Get_OperationValue(_Bill._Operation);
                paid = new OperationSQL(DB).Get_OperationPaysValue_UPON_OperationCurrency(_Bill._Operation);

                textBoxBillOUTValue.Text = billvalue.ToString() + " " + _Bill._Currency.CurrencySymbol;
                textBoxPaid.Text = paid.ToString() + " " + _Bill._Currency.CurrencySymbol;
                textBoxRemain.Text = (billvalue - paid).ToString() + " " + _Bill._Currency.CurrencySymbol;
                textBoxPayValue.Text = (billvalue - paid).ToString();
                textBoxPayValue.Focus();
            }
            else if (_EmployeePayOrder !=null )
            {
                labelOwner .Text = "عائدة لأمر صرف رقم: " +_EmployeePayOrder.PayOrderID.ToString ();
                textBoxBillOUT_ID.Text = _EmployeePayOrder.PayOrderID.ToString();
                textBoxContact.Text = _EmployeePayOrder._Employee .EmployeeName;

                textBoxBillOUTDate.Text = _EmployeePayOrder.PayOrderDate.ToString("yyyy-mm-dd hh:mm");
                textBoxCurrency.Text = _EmployeePayOrder._Currency.CurrencyName;
                textBoxBillOUT_ExchangeRate.Text = _EmployeePayOrder.ExchangeRate.ToString();

                double billvalue, paid;
                billvalue = new OperationSQL(DB).Get_OperationValue(Operation.Employee_PayOrder, _EmployeePayOrder.PayOrderID );
                paid = new OperationSQL(DB).Get_OperationPaysValue_UPON_OperationCurrency(Operation.Employee_PayOrder, _EmployeePayOrder.PayOrderID);

                textBoxBillOUTValue.Text = billvalue.ToString() + " " + _EmployeePayOrder._Currency.CurrencySymbol;
                textBoxPaid.Text = paid.ToString() + " " + _EmployeePayOrder._Currency.CurrencySymbol;
                textBoxRemain.Text = (billvalue - paid).ToString() + " " + _EmployeePayOrder._Currency.CurrencySymbol;
                textBoxPayValue.Text = (billvalue - paid).ToString();
                textBoxPayValue.Focus();
            }
            



        }





        public void loadForm(bool edit)
        {
            
            buttonSave.Name = "buttonSave";
            if (_PayOUT != null)
            {
                if (_PayOUT._Bill != null) FillOwner_PanelINFO();
                dateTimePicker_.Value = _PayOUT.PayOprDate;
                textBoxPayDesc.Text = _PayOUT.PayDescription;
                ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxCurrency, DB, _PayOUT._Currency);
                textBoxPayExchangeRate.Text = _PayOUT._Currency.ExchangeRate.ToString();
                textBoxPayValue.Text = _PayOUT.Value.ToString();
                TextboxNotes.Text = _PayOUT.Notes;
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
                textBoxPayExchangeRate.Text = defaultcurrency.ExchangeRate.ToString();
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
            Operation opr;
            if (_Bill != null) opr = _Bill._Operation;
            else if (_EmployeePayOrder != null) opr = new Operation(Operation.Employee_PayOrder, _EmployeePayOrder.PayOrderID);
            else opr = null;
            if (buttonSave.Name == "buttonAdd")
            {

                try
                {

                    bool Succes = new PayOUTSQL(DB).Add_PayOUT(dateTimePicker_.Value, opr, textBoxPayDesc.Text, payvalue, exchangerate, currency, TextboxNotes.Text);
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
                    bool Succes = new PayOUTSQL(DB).Update_PayOUT(_PayOUT.PayOprID, dateTimePicker_.Value, opr, textBoxPayDesc.Text, payvalue, exchangerate, currency, TextboxNotes.Text);
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

        private void PayOUTForm_Load(object sender, EventArgs e)
        {
    
                FillOwner_PanelINFO();
      
        }

        private void comboBoxCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem ComboboxItem_ = (ComboboxItem)comboBoxCurrency.SelectedItem;
                Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(ComboboxItem_.Value);
                textBoxPayExchangeRate.Text = currency.ExchangeRate.ToString();
            }
            catch
            {

            }
        }
    }
}
