using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.Forms;
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

namespace ItemProject.Configuration
{
    public partial class ConfigurationForm : Form
    {
        DatabaseInterface DB;
        Button SelectedButton;

        MenuItem AddCurrency;
        MenuItem EditCurrency;
        MenuItem DeleteCurrency;
        MenuItem SetDefaultCurrency;

        MenuItem AddSellType;
        MenuItem EditSellType;
        MenuItem DeleteSellType;

        MenuItem AddTradeState;
        MenuItem EditTradeState;
        MenuItem DeleteTradeState;
        public ConfigurationForm(DatabaseInterface db)
        {
            DB = db;
            InitializeComponent();
            AddCurrency = new System.Windows.Forms.MenuItem("اضافة عملة", AddCurrency_MenuItem_Click);
            EditCurrency= new System.Windows.Forms.MenuItem("تعديل العملة", EditCurrency_MenuItem_Click); ;
            DeleteCurrency  = new System.Windows.Forms.MenuItem("حذف العملة", DeleteCurrency_MenuItem_Click); ;
            SetDefaultCurrency = new System.Windows.Forms.MenuItem("ضبط كافتراضي", SetDefaultCurrency_MenuItem_Click); ;


            AddSellType = new System.Windows.Forms.MenuItem("اضافة نمط بيع", AddSellType_MenuItem_Click);
            EditSellType = new System.Windows.Forms.MenuItem("تعديل نط البيع", EditSellType_MenuItem_Click); ;
            DeleteSellType = new System.Windows.Forms.MenuItem("حذف نمط البيع", DeleteSellType_MenuItem_Click); ;

            AddTradeState  = new System.Windows.Forms.MenuItem("اضافة جديد", AddTradeState_MenuItem_Click );
            EditTradeState  = new System.Windows.Forms.MenuItem("تعديل", EditTradeState_MenuItem_Click); ;
            DeleteTradeState  = new System.Windows.Forms.MenuItem("حذف ", DeleteTradeState_MenuItem_Click); ;

        }

   
        #region TradeStateMenuItem
        private void DeleteTradeState_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewData.SelectedItems.Count == 1)
                {
                    DialogResult dd = MessageBox.Show("هل انت متأكد من الحذف", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dd == DialogResult.OK)
                    {
                        uint stateid = Convert.ToUInt32(listViewData.SelectedItems[0].Name);
                        bool success = new TradeStateSQL (DB).DeleteTradestate (stateid);
                        if (success) buttonTradeStateSetting.PerformClick();
                    }

                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void EditTradeState_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewData.SelectedItems.Count == 1)
            {
                uint tradestateid = Convert.ToUInt32(listViewData.SelectedItems[0].Name);
                InputBox InputBox_ = new InputBox("تعديل حالة بيع شراء", "أدخل وصف حالة العنصر ", listViewData.SelectedItems[0].Text );
                DialogResult dd = InputBox_.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    bool success = new TradeStateSQL(DB).UpdateTradeState (tradestateid, InputBox_.textBox1.Text);
                    if (success) buttonTradeStateSetting.PerformClick();
                }
            }

        }

        private void AddTradeState_MenuItem_Click(object sender, EventArgs e)
        {
            InputBox InputBox_ = new InputBox("اضافة حالة بيع شراء", "أدخل وصف حالة العنصر");
            DialogResult dd = InputBox_.ShowDialog();
            if (dd == DialogResult.OK)
            {
                bool success = new TradeStateSQL (DB).AddTradeState (InputBox_.textBox1.Text);
                if (success) buttonTradeStateSetting.PerformClick();
            }
        }
        #endregion
        #region SellTypeMenuItem
        private void DeleteSellType_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewData.SelectedItems.Count == 1)
                {
                    DialogResult dd = MessageBox.Show("هل انت متأكد من الحذف", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dd == DialogResult.OK)
                    {
                        uint selltypeid = Convert.ToUInt32(listViewData.SelectedItems[0].Name);
                        bool success = new SellTypeSql(DB).DeleteSellType(selltypeid);
                        if (success) buttonSellTypeSetting.PerformClick();
                    }

                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void EditSellType_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewData.SelectedItems.Count == 1)
            {
                uint selltypeid = Convert.ToUInt32(listViewData.SelectedItems[0].Name);
                InputBox InputBox_ = new InputBox("تعديل نمط بيع", "أدخل اسم نمط البيع الجديد", listViewData.SelectedItems[0].Text );
                DialogResult dd = InputBox_.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    bool success = new SellTypeSql(DB).UpdateSellType (selltypeid,InputBox_.textBox1.Text);
                    if (success) buttonSellTypeSetting.PerformClick();
                }
            }

        }

        private void AddSellType_MenuItem_Click(object sender, EventArgs e)
        {
            InputBox InputBox_ = new InputBox("اضافة نمط بيع","أدخل اسم نمط البيع");
            DialogResult dd = InputBox_.ShowDialog();
            if (dd == DialogResult.OK)
            {
                bool success = new SellTypeSql(DB).AddSellType(InputBox_.textBox1 .Text );
                if(success ) buttonSellTypeSetting .PerformClick();
            }
        }
        #endregion
        #region CurrencyMenuItem
        private void SetDefaultCurrency_MenuItem_Click(object sender, EventArgs e)
        {
           try
            {
                if (listViewData.SelectedItems.Count == 1)
                {
                    uint currencyid = Convert.ToUInt32(listViewData.SelectedItems[0].Name);
                    bool success= ProgramGeneralMethods.SetDefaultCurrency(currencyid);
                    if(success ) buttonCurrencySetting.PerformClick();
                }
                
            }catch
            {

            }
        }
        private void DeleteCurrency_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewData.SelectedItems.Count == 1)
                {
                    DialogResult dd = MessageBox.Show("هل انت متأكد من الحذف", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dd == DialogResult.OK)
                    {
                        uint currencyid = Convert.ToUInt32(listViewData.SelectedItems[0].Name);
                        CurrencySQL currencysql = new CurrencySQL(DB);
                        Currency currency = currencysql.GetCurrencyINFO_ByID(currencyid);
                        if (currency.ReferenceCurrencyID == null)
                        {
                            MessageBox.Show("غير مسموح حذف او تعديل بيانات العملة المرجعية", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        bool success = currencysql.DeleteCurrency(currencyid);
                        if(success)buttonCurrencySetting.PerformClick();
                    }

                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void EditCurrency_MenuItem_Click(object sender, EventArgs e)
        {
            if(listViewData .SelectedItems .Count ==1)
            {
                uint currencyid = Convert.ToUInt32(listViewData.SelectedItems[0].Name );
                Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(currencyid);
                if(currency .ReferenceCurrencyID ==null )
                {
                    MessageBox.Show("غير مسموح تعديل بيانات العملة المرجعية","",MessageBoxButtons.OK ,MessageBoxIcon.Error );
                    return;
                }
                AddCurrencyForm AddCurrencyForm_ = new AddCurrencyForm(DB, currency);
                DialogResult dd = AddCurrencyForm_.ShowDialog();
                if (dd == DialogResult.OK) buttonCurrencySetting.PerformClick();
            }
           
        }

        private void AddCurrency_MenuItem_Click(object sender, EventArgs e)
        {
            AddCurrencyForm AddCurrencyForm_ = new AddCurrencyForm(DB);
            DialogResult dd = AddCurrencyForm_.ShowDialog();
            if (dd == DialogResult.OK) buttonCurrencySetting.PerformClick();
        }
        #endregion

        public void OptimizeListViewColumns()
        {
            for (int i = 0; i < listViewData .Columns  .Count; i++)
            {
                listViewData.Columns[i].Width = listViewData.Width / listViewData.Columns.Count;
            }
       }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            buttonCurrencySetting.PerformClick();
        }

        private void listView_MouseDown(object sender, MouseEventArgs e)
        {
            listViewData.ContextMenu = null;
            bool match = false;
            ListViewItem listitem = new ListViewItem();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem item1 in listViewData.Items)
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
                    try
                    {
                        if (SelectedButton == buttonCurrencySetting)
                        {
                            MenuItem[] mi1 = new MenuItem[] { SetDefaultCurrency ,AddCurrency, EditCurrency, DeleteCurrency };
                            listViewData.ContextMenu = new ContextMenu(mi1);
                        }
                        else if (SelectedButton == buttonSellTypeSetting )
                        {
                            MenuItem[] mi1 = new MenuItem[] { AddSellType , EditSellType , DeleteSellType  };
                            listViewData.ContextMenu = new ContextMenu(mi1);
                        }
                        else if (SelectedButton == buttonTradeStateSetting )
                        {
                            MenuItem[] mi1 = new MenuItem[] { AddTradeState, EditTradeState, DeleteTradeState };
                            listViewData.ContextMenu = new ContextMenu(mi1);
                        }

                    }
                    catch
                    {
                        return;
                    }



                }
                else
                {
                    if (SelectedButton == buttonCurrencySetting)
                    {
                        MenuItem[] mi1 = new MenuItem[] { AddCurrency };
                        listViewData.ContextMenu = new ContextMenu(mi1);
                    }
                    else if (SelectedButton == buttonSellTypeSetting)
                    {
                        MenuItem[] mi1 = new MenuItem[] { AddSellType };
                        listViewData.ContextMenu = new ContextMenu(mi1);
                    }
                    else if (SelectedButton == buttonTradeStateSetting)
                    {
                        MenuItem[] mi1 = new MenuItem[] { AddTradeState  };
                        listViewData.ContextMenu = new ContextMenu(mi1);
                    }

                }

            }
        }
        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewData.SelectedItems.Count > 0)
            {
                //OpenPlaceDetailsMenuItem.PerformClick();
            }
        }

        private void buttonCurrencySetting_Click(object sender, EventArgs e)
        {
            SelectedButton = buttonCurrencySetting;
            labelHeader.Text = "ضبط العملات";
            try
            {
                Currency defaultcurrency = ProgramGeneralMethods.GetDefaultCurrency(DB);
                listViewData.Items.Clear();
                listViewData.Columns.Clear();
                listViewData.ContextMenu = null;
                List<Currency> CurrencyList = new CurrencySQL(DB).GetCurrencyList();
                listViewData.Columns.Add("اسم العملة" );
                listViewData.Columns.Add("رمز العملة");
                listViewData.Columns.Add("سعر الصرف");
                listViewData.Columns.Add("", 50);
                for (int i=0;i<CurrencyList .Count;i++)
                {
                    ListViewItem ListViewItem_ = new ListViewItem(CurrencyList[i].CurrencyName);
                    ListViewItem_.Name = CurrencyList[i].CurrencyID.ToString();
                    ListViewItem_.SubItems.Add(CurrencyList[i].CurrencySymbol );
                    ListViewItem_.SubItems.Add(CurrencyList[i].ExchangeRate.ToString());
                   if(defaultcurrency !=null )
                    if (CurrencyList[i].CurrencyID == defaultcurrency.CurrencyID)
                        ListViewItem_.SubItems.Add("افتراضية");
                    listViewData.Items.Add(ListViewItem_);
                }
                OptimizeListViewColumns();
        }
            catch (Exception ee)
            {
                MessageBox.Show("buttonCurrencySetting_Click"+ee.Message );
            }
}

        private void buttonSellTypeSetting_Click(object sender, EventArgs e)
        {

            SelectedButton = buttonSellTypeSetting ;
            labelHeader.Text = "ضبط أنماط البيع";
            try
            {
                listViewData.Items.Clear();
                listViewData.Columns.Clear();
                listViewData.ContextMenu = null;
                List<SellType > SellTypeList = new SellTypeSql(DB).GetSellTypeList();
                listViewData.Columns.Add("اسم نمط البيع");
                for (int i = 0; i < SellTypeList.Count; i++)
                {
                    ListViewItem ListViewItem_ = new ListViewItem(SellTypeList[i].SellTypeName);
                    ListViewItem_.Name = SellTypeList[i].SellTypeID .ToString();
                    listViewData.Items.Add(ListViewItem_);
                }
                OptimizeListViewColumns();
            }
            catch (Exception ee)
            {

            }
        }

        private void buttonTradeStateSetting_Click(object sender, EventArgs e)
        {
            SelectedButton = buttonTradeStateSetting ;
            labelHeader.Text = "ضبط حالات بيع شراء العنصر";
            try
            {
                listViewData.Items.Clear();
                listViewData.Columns.Clear();
                listViewData.ContextMenu = null;
                List<TradeState > TradeStateList = new TradeStateSQL(DB).GetTradeStateList();
                listViewData.Columns.Add("وصف حالة العنصر");
                for (int i = 0; i < TradeStateList.Count; i++)
                {
                    ListViewItem ListViewItem_ = new ListViewItem(TradeStateList[i].TradeStateName );
                    ListViewItem_.Name = TradeStateList[i].TradeStateID.ToString();
                    listViewData.Items.Add(ListViewItem_);
                }
                OptimizeListViewColumns();
            }
            catch (Exception ee)
            {

            }
        }
    }
}
