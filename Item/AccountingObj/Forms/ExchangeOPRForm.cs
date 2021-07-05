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
    public partial class ExchangeOPRForm : Form
    {
      
        DatabaseInterface DB;
        ExchangeOPR _ExchangeOPR;

        public bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
        }
        bool Edit;
        public ExchangeOPRForm(DatabaseInterface db,DateTime ExchangeOPRDate_)
        {
            DB = db;
            InitializeComponent();
            _Changed = false;
            dateTimePicker_.Value = ExchangeOPRDate_;
            Currency defaultcurrency = ProgramGeneralMethods.GetDefaultCurrency(DB);
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxSourceCurrency, DB, defaultcurrency);
            ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxTargetCurrency, DB, defaultcurrency);
            buttonSave.Name = "buttonAdd";
            buttonSave.Text  = "انشاء";

        }


        public ExchangeOPRForm(DatabaseInterface db, ExchangeOPR ExchangeOPR_, bool Edit_)
        {

            InitializeComponent();
            Edit = Edit_;
            DB = db;
            _ExchangeOPR = ExchangeOPR_;
            dateTimePicker_.Value = _ExchangeOPR.ExchangeOprDate; ;
            _Changed = false;
            loadForm(Edit);
        }


        public void loadForm(bool edit)
        {

            buttonSave.Name = "buttonSave";
            buttonSave.Text = "حفظ";
            if (_ExchangeOPR != null)
            {
                dateTimePicker_.Value = _ExchangeOPR.ExchangeOprDate;
                ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxSourceCurrency, DB, _ExchangeOPR.SourceCurrency);
                ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxTargetCurrency, DB, _ExchangeOPR.TargetCurrency);

                textBoxSourceExchangeRate.Text = _ExchangeOPR.SourceCurrency.ExchangeRate.ToString();
                textBoxTargetExchangeRate.Text = _ExchangeOPR.TargetCurrency.ExchangeRate.ToString();

                textBoxMoneyOUTValue.Text = _ExchangeOPR.OutMoneyValue .ToString();
                TextboxNotes.Text = _ExchangeOPR .Notes;
                if (!edit)
                {
                    dateTimePicker_.Enabled = false;
                    comboBoxSourceCurrency.Enabled = false;
                    textBoxSourceExchangeRate.ReadOnly = true;
                    comboBoxTargetCurrency.Enabled = false;
                    textBoxTargetExchangeRate.ReadOnly = true;
                    textBoxMoneyOUTValue.ReadOnly = true;
                    TextboxNotes.ReadOnly = true;
                    buttonSave.Visible = false;
                }
                else
                {
                    dateTimePicker_.Enabled = true ;
                    comboBoxSourceCurrency.Enabled = true ;
                    textBoxSourceExchangeRate.ReadOnly = false ;
                    comboBoxTargetCurrency.Enabled = true ;
                    textBoxTargetExchangeRate.ReadOnly = false ;
                    textBoxMoneyOUTValue.ReadOnly = false ;
                    TextboxNotes.ReadOnly = false ;
                    buttonSave.Visible = true ;
                }

            }
            else
            {
                Currency defaultcurrency = ProgramGeneralMethods.GetDefaultCurrency(DB);
                ProgramGeneralMethods.FillComboBoxCurrency(ref comboBoxSourceCurrency, DB, defaultcurrency);
                textBoxSourceExchangeRate.Text = defaultcurrency.ExchangeRate.ToString();
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            ComboboxItem S_Currency_item = (ComboboxItem)comboBoxSourceCurrency.SelectedItem;
            Currency SourceCurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(S_Currency_item.Value);
            ComboboxItem T_Currency_item = (ComboboxItem)comboBoxTargetCurrency.SelectedItem;
            Currency TargetCurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(T_Currency_item.Value);
            double outmoneyvalue, source_exchangerate,target_exchangerate;
            try
            {
                outmoneyvalue = Convert.ToDouble(textBoxMoneyOUTValue.Text);
            }
            catch
            {
                MessageBox.Show("قيمة المبلغ الخارج يجب ان تكون رقم حقيقي او صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                source_exchangerate = Convert.ToDouble(textBoxSourceExchangeRate.Text);
                target_exchangerate = Convert.ToDouble(textBoxTargetExchangeRate.Text);

            }
            catch
            {
                MessageBox.Show("سعر الصرف يجب ان يكون رقم حقيقي او صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (buttonSave.Name == "buttonAdd")
            {

                try
                {
                    bool Succes = new ExchangeOPRSQL(DB).Add_ExchageOPR (dateTimePicker_.Value,SourceCurrency ,source_exchangerate,outmoneyvalue ,TargetCurrency,target_exchangerate , TextboxNotes.Text);
                    if (Succes)
                    {
                        MessageBox.Show("تم اضافة عملية الصرف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    bool Succes = new ExchangeOPRSQL(DB).Update_ExchageOPR(_ExchangeOPR.ExchangeOprID, dateTimePicker_.Value, SourceCurrency, source_exchangerate, outmoneyvalue, TargetCurrency, target_exchangerate, TextboxNotes.Text);
                    if (Succes)
                    {
                        MessageBox.Show("تم تعديل عملية الصرف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ValuesChanged(object sender, EventArgs e)
        {
            try
            {
                double source_exchangerate = Convert.ToDouble(textBoxSourceExchangeRate .Text );
                double target_exchangerate = Convert.ToDouble(textBoxTargetExchangeRate.Text);
                double exchangefactor = target_exchangerate / source_exchangerate;
                double money_out= Convert.ToDouble(textBoxMoneyOUTValue.Text);
                double inmoneyvalue = exchangefactor * money_out;
                textBoxExchangeFactor.Text = System.Math.Round (exchangefactor ,5).ToString ();
                textBoxMoneyINValue.Text = System.Math.Round(inmoneyvalue,2).ToString ();
            }
            catch
            {

                textBoxExchangeFactor.Text = "-";
                textBoxMoneyINValue.Text = "-";
            }
        }

        private void comboBoxSourceCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            Currency Sourcecurrency = null;
            if (comboBoxSourceCurrency.SelectedIndex >= 0)
            {
                ComboboxItem SourceComboboxItem_ = (ComboboxItem)comboBoxSourceCurrency.SelectedItem;
                Sourcecurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(SourceComboboxItem_.Value);
                textBoxSourceCurrencyName.Text = Sourcecurrency.CurrencyName;
                textBoxSourceExchangeRate.Text = Sourcecurrency.ExchangeRate.ToString();
                if (Sourcecurrency.ReferenceCurrencyID == null)
                    textBoxSourceExchangeRate.ReadOnly = true;
                else
                    textBoxSourceExchangeRate.ReadOnly = false ;
            }
            else
            {
                textBoxSourceCurrencyName.Text = "-";
                textBoxSourceExchangeRate.Text = "-";
            }
        }

        private void comboBoxTargetCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            Currency Targecurrency = null;
            if (comboBoxTargetCurrency.SelectedIndex >= 0)
            {
                ComboboxItem TargetComboboxItem_ = (ComboboxItem)comboBoxTargetCurrency.SelectedItem;
                Targecurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(TargetComboboxItem_.Value);
                textBoxTargetCurrencyName .Text = Targecurrency.CurrencyName;
                textBoxTargetExchangeRate.Text = Targecurrency.ExchangeRate.ToString();
                if (Targecurrency.ReferenceCurrencyID == null)
                    textBoxTargetExchangeRate.ReadOnly = true;
                else
                    textBoxTargetExchangeRate.ReadOnly = false ;
            }
            else
            {
                textBoxTargetCurrencyName.Text = "-";
                textBoxTargetExchangeRate.Text = "-";
            }
        }
    }
}
