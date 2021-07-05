using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using ItemProject.AccountingObj.Objects;
using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.Trade.Objects;
using ItemProject.Trade.TradeSQL;
using ItemProject.Trade.Forms.TradeForms;
using ItemProject.Maintenance.MaintenanceSQL;
using ItemProject.Maintenance.Objects;
using ItemProject.Company.CompanySQL;
using ItemProject.Company.Objects;
using ItemProject.Company.Forms;

namespace ItemProject.AccountingObj.Forms
{
    public partial class MainWindowForm : Form
    {
        DatabaseInterface DB;
        DateAccount Report_;
        DateAccount MoneyAccount_;
        DateAccount SellsAccount_;
        DateAccount BuysAccount_;
        DateAccount MaintenanceOPRsAccount_;
        DateAccount PayOrdersAccount_;
        MoneyAccountSQL MoneyAccountSQL_;

        System.Windows.Forms.MenuItem Refresh_MenuItem;
        System.Windows.Forms.MenuItem CreateBillBuy_MenuItem;
        System.Windows.Forms.MenuItem OpenBillBuy_MenuItem;
        System.Windows.Forms.MenuItem EditBillBuy_MenuItem;
        System.Windows.Forms.MenuItem DeleteBillBuy_MenuItem;

        System.Windows.Forms.MenuItem CreateMaintenanceOPR_MenuItem;
        System.Windows.Forms.MenuItem OpenMaintenanceOPR_MenuItem;
        System.Windows.Forms.MenuItem EditMaintenanceOPR_MenuItem;
        System.Windows.Forms.MenuItem DeleteMaintenanceOPR_MenuItem;

        System.Windows.Forms.MenuItem CreatePayOrder_MenuItem;
        System.Windows.Forms.MenuItem OpenPayOrder_MenuItem;
        System.Windows.Forms.MenuItem EditPayOrder_MenuItem;
        System.Windows.Forms.MenuItem DeletePayOrder_MenuItem;

        System.Windows.Forms.MenuItem CreateBillSell_MenuItem;
        System.Windows.Forms.MenuItem OpenBillSell_MenuItem;
        System.Windows.Forms.MenuItem EditBillSell_MenuItem;
        System.Windows.Forms.MenuItem DeleteBillSell_MenuItem;




        System.Windows.Forms.MenuItem AddPayIN_MenuItem;
        System.Windows.Forms.MenuItem AddPayOUT_MenuItem;
        System.Windows.Forms.MenuItem AddExchangeOPR_MenuItem;
        System.Windows.Forms.MenuItem Open_MoneyOPR_MenuItem;
        System.Windows.Forms.MenuItem Edit_MoneyOPR_MenuItem;
        System.Windows.Forms.MenuItem Delete_MoneyOPR_MenuItem;

        MenuItem AddPayIN_BillSell_MenuItem;
        MenuItem AddPayIN_BillMaintenance_MenuItem;
        MenuItem AddPayOUT_BillBuy_MenuItem;
        MenuItem AddPayOUT_PayOrder_MenuItem;
        Currency ReferenceCurrency;
        public MainWindowForm(DatabaseInterface db)
        {
            InitializeComponent();
            DB = db;
            labelUser.Text = DB.GetUser_EmployeeName(); 
            ReferenceCurrency = new CurrencySQL(DB).GetReferenceCurrency();
            MoneyAccountSQL_ = new MoneyAccountSQL(DB);
            DateAccount  .YearRange yearrange = new DateAccount  .YearRange(DateTime.Today.Year-5, DateTime.Today.Year+5);

            MoneyAccount_ = new DateAccount (DB,yearrange,DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            SellsAccount_ = new DateAccount(DB, yearrange, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            BuysAccount_ = new DateAccount(DB, yearrange, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            PayOrdersAccount_ = new DateAccount(DB, yearrange, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            MaintenanceOPRsAccount_ = new DateAccount(DB, yearrange, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            Report_ = new DateAccount(DB, yearrange, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            Initialize_MenuItems();
            Refresh_ListViewMoneyDataDetails();
            Refresh_ListViewSells();
            Refresh_ListViewBuys();
            Refresh_ListViewMaintenanceOPRs();
            Refresh_ListViewPayOrders();
            TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();

         
            for(int i=0;i<dataGridView1.Columns .Count;i++)
              dataGridView1 .Columns[i].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            dataGridView1.TopLeftHeaderCell.Value = "العملة";

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Aqua;
            dataGridView1.Columns[4].HeaderCell.Style.BackColor = Color.LightGreen;
            dataGridView1.Columns[9].HeaderCell.Style.BackColor = Color.Orange ;
            dataGridView1.Columns[10].HeaderCell.Style.BackColor = Color.LightYellow;

            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Aqua;
            dataGridView1.TopLeftHeaderCell.Style.BackColor = Color.LightYellow;
            tabControl1.TabPages.RemoveAt(5);
        }
        public async void Initialize_MenuItems()
        {
            Refresh_MenuItem = new System.Windows.Forms.MenuItem("تحديث", Refresh_MenuItem_Click);

            CreateBillBuy_MenuItem = new System.Windows.Forms.MenuItem("انشاء فاتورة شراء", CreateBillBuy_MenuItem_Click);
            OpenBillBuy_MenuItem = new System.Windows.Forms.MenuItem("فتح", OpenBillBuy_MenuItem_Click);
            EditBillBuy_MenuItem = new System.Windows.Forms.MenuItem("تعديل", EditBillBuy_MenuItem_Click);
            DeleteBillBuy_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteBillBuy_MenuItem_Click);

            CreateMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("انشاء عملية صيانة", CreateMaintenanceOPR_MenuItem_Click);
            OpenMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("فتح", OpenMaintenanceOPR_MenuItem_Click);
            EditMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("تعديل", EditMaintenanceOPR_MenuItem_Click);
            DeleteMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteMaintenanceOPR_MenuItem_Click);



            CreatePayOrder_MenuItem = new System.Windows.Forms.MenuItem("انشاء أمر صرف", CreatePayOrder_MenuItem_Click);
            OpenPayOrder_MenuItem = new System.Windows.Forms.MenuItem("فتح", OpenPayOrder_MenuItem_Click);
            EditPayOrder_MenuItem = new System.Windows.Forms.MenuItem("تعديل", EditPayOrder_MenuItem_Click);
            DeletePayOrder_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeletePayOrder_MenuItem_Click);

            CreateBillSell_MenuItem = new MenuItem("انشاء فاتورة مبيع", CreateBillSell_MenuItem_Click);
            OpenBillSell_MenuItem = new System.Windows.Forms.MenuItem("فتح", OpenBillSell_MenuItem_Click);
            EditBillSell_MenuItem = new System.Windows.Forms.MenuItem("تعديل", EditBillSell_MenuItem_Click);
            DeleteBillSell_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteBillSell_MenuItem_Click);

            Open_MoneyOPR_MenuItem = new MenuItem("تعديل", Open_MoneyOPR_MenuItem_Click);
            Edit_MoneyOPR_MenuItem = new MenuItem("تعديل", Edit_MoneyOPR_MenuItem_Click);
            Delete_MoneyOPR_MenuItem = new MenuItem("حذف", Delete_MoneyOPR_MenuItem_Click);
            AddPayOUT_MenuItem = new System.Windows.Forms.MenuItem("اضافة دفعة خارجة من الصندوق", AddPayOUT_MenuItem_Click);
            AddPayIN_MenuItem = new MenuItem("اضافة دفعة واردة الى الصندوق", AddPayIN_MenuItem_Click);
            AddExchangeOPR_MenuItem = new MenuItem("اضافة عملية صرف", AddExchangeOPR_MenuItem_Click);


            AddPayOUT_BillBuy_MenuItem = new System.Windows.Forms.MenuItem("اضافة دفعة تابعة للفاتورة", AddPayOUT_BillBuy_MenuItem_Click);
            AddPayOUT_PayOrder_MenuItem = new System.Windows.Forms.MenuItem("اضافة دفعة تابعة لأامر الصرف", AddPayOUT_PayOrder_MenuItem_Click);

            AddPayIN_BillSell_MenuItem = new MenuItem("اضافة دفعة تابعة للفاتورة", AddPayIN_BillSell_MenuItem_Click);
            AddPayIN_BillMaintenance_MenuItem = new MenuItem("اضافة دفعة تابعة للفاتورة", AddPayIN_BillMaintenance_MenuItem_Click);

        }
        #region MoneyAccount
        public void AdjustmentDatagridviewColumnsWidth()
        {
            int columnscount = dataGridView1.Columns.Count + 1;
            int w = (dataGridView1.Width ) / columnscount; ;
            dataGridView1.RowHeadersWidth = w-2;
            for (int i = 0; i < columnscount - 1; i++) dataGridView1.Columns[i].Width = w;

        }

        public async void Refresh_ListViewMoneyDataDetails()
        {
            AccountLabelAccountDate.Text = MoneyAccount_.GetAccountDateString();
            Refresh_ListViewMoneyDataReport();
            dd();
            #region PaySection
            double realValue_in = 0, realValue_out = 0;
            if (MoneyAccount_.Day != -1)
            {
              
                AccountLabelAccountType.Text = "حساب اليوم";
                AccountLabelReport.Text = "تقرير حساب اليوم : " + MoneyAccount_.GetAccountDateString();
                #region PayDaySection
                ListViewMoneyDataDetails .Items.Clear();
            
                if (ListViewMoneyDataDetails.Name != "ListViewMoneyDataDetails_Day")
                {
                    AccountOprReportDetail.IntiliazeListView(ref ListViewMoneyDataDetails);

                }
                List<AccountOprReportDetail> accountopr_reportlist
                          = new MoneyAccountSQL(DB).GetAccountOprReport_Details_InDay( MoneyAccount_.Year, MoneyAccount_.Month, MoneyAccount_.Day);
                for (int i = 0; i < accountopr_reportlist.Count; i++)
                {
                   
                    string payopridstr = (accountopr_reportlist[i].OprType== AccountOprReportDetail.TYPE_PAY_OPR?"P":"E")
                        + (accountopr_reportlist[i].OprDirection== AccountOprReportDetail.DIRECTION_IN?"I":"O")
                        + accountopr_reportlist[i].OprID.ToString();
                    string payoprtype = "";
                    string Direction = "";

                    if (accountopr_reportlist[i].OprType == AccountOprReportDetail.TYPE_PAY_OPR)
                    {

                        payoprtype = "عملية دفع";
                    }
                    else
                    {
                        payoprtype = "عملية صرف";

                    }

                    if (accountopr_reportlist[i].OprDirection == AccountOprReportDetail.DIRECTION_IN)
                    {
                        Direction = "داخل الى الصندوق";
                        realValue_in += accountopr_reportlist[i].RealValue;
                    }
                    else
                    {
                        Direction = "خارج من الصندوق";
                        realValue_out += accountopr_reportlist[i].RealValue;

                    }
                    ListViewItem item = new ListViewItem(accountopr_reportlist[i].OprTime.ToShortTimeString());
                    item.Name = payopridstr;
                    item.SubItems.Add(payoprtype);
                    item.SubItems.Add(Direction);
                    item.SubItems.Add(accountopr_reportlist[i].OprID.ToString());
                  
                    item.SubItems.Add(accountopr_reportlist[i].Value.ToString()+" "
                        + accountopr_reportlist[i].Currency.ToString());
                    item.SubItems.Add(accountopr_reportlist[i].ExchangeRate .ToString());
                    item.SubItems.Add(accountopr_reportlist[i].RealValue .ToString()+" "+ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(accountopr_reportlist[i].OprOwner);
                    item.UseItemStyleForSubItems = false;
                    Color color;
                    if ( accountopr_reportlist[i].OprType == AccountOprReportDetail.TYPE_EXCHANGE_OPR) color = Color.LightGoldenrodYellow;
                    else if (accountopr_reportlist[i].OprType == AccountOprReportDetail.TYPE_PAY_OPR  &&
                        accountopr_reportlist[i].OprDirection  == AccountOprReportDetail.DIRECTION_OUT ) color = Color.Orange ;
                    else if (accountopr_reportlist[i].OprType == AccountOprReportDetail.TYPE_PAY_OPR  &&
                        accountopr_reportlist[i].OprDirection == AccountOprReportDetail.DIRECTION_IN) color = Color.LightGreen;
                    else color = Color.Orange;
                    //if (oprtypeDirectionColor == 0 && oprtypeColor == 0) color = Color.YellowGreen;
                    //else if (oprtypeDirectionColor == 1 && oprtypeColor == 0) color = Color.DarkOrange;
                    //else if (oprtypeDirectionColor == 0 && oprtypeColor == 1) color = Color.LightGreen;
                    //else color = Color.Orange;
                    //item.UseItemStyleForSubItems = false;
                    //item.SubItems[0].BackColor = color;
                    //item.SubItems[1].BackColor = color;
                    //item.SubItems[2].BackColor = color;
                    //item.SubItems[3].BackColor = color;
                    //item.SubItems[4].BackColor = color;
                    //item.SubItems[5].BackColor = color;
                    //item.SubItems[6].BackColor = color;
                    //item.SubItems[7].BackColor = color;
                    item.UseItemStyleForSubItems = true ;
                    item.BackColor = color;
  
                    ListViewMoneyDataDetails.Items.Add(item);

                }
              
                #endregion
            }
            else if (MoneyAccount_.Month != -1)
            {
                AccountLabelAccountType.Text = "حساب الشهر";
                AccountLabelReport.Text = "تقرير حساب الشهر : " + MoneyAccount_.GetAccountDateString();

                #region PayMonthSection
                ListViewMoneyDataDetails.Items.Clear();
                if (ListViewMoneyDataDetails.Name != "ListViewMoneyDataDetails_Month")
                {
                    AccountOprDayReportDetail.IntiliazeListView(ref ListViewMoneyDataDetails);
                }
                List<AccountOprDayReportDetail> accountoprdayeportlist
                                    = new MoneyAccountSQL(DB).GetAccountOprReport_Details_InMonth(MoneyAccount_.Year.ToString(), MoneyAccount_.Month.ToString());
                for (int i = 0; i < accountoprdayeportlist.Count; i++)
                {
                    ListViewItem item = new ListViewItem(accountoprdayeportlist[i].Date_day.ToShortDateString());
                    item.Name = accountoprdayeportlist[i].DateDayNo.ToString();
                    item.SubItems.Add(accountoprdayeportlist[i].PaysIN_Count.ToString());
                    item.SubItems.Add(accountoprdayeportlist[i].PaysOUT_Count.ToString());
                    item.SubItems.Add(accountoprdayeportlist[i].Exchange_Count.ToString());
                    item.SubItems.Add(accountoprdayeportlist[i].PaysIN_Value);
                    item.SubItems.Add(accountoprdayeportlist[i].PaysOUT_Value);
                    item.SubItems.Add(accountoprdayeportlist[i].PaysIN_Real_Value+" "+ReferenceCurrency.CurrencySymbol );
                    item.SubItems.Add(accountoprdayeportlist[i].PaysOUT_Real_Value + " " + ReferenceCurrency.CurrencySymbol);
                    realValue_in += accountoprdayeportlist[i].PaysIN_Real_Value;
                    realValue_out += accountoprdayeportlist[i].PaysOUT_Real_Value;
                    double clear_real_value = accountoprdayeportlist[i].PaysIN_Real_Value -
                        accountoprdayeportlist[i].PaysOUT_Real_Value;
                    item.SubItems.Add(clear_real_value + " " + ReferenceCurrency.CurrencySymbol);
                    //item.UseItemStyleForSubItems = false;
                    //item.SubItems[0].BackColor = Color.LightGray;
                    //item.SubItems[1].BackColor = Color.LightGreen;
                    //item.SubItems[2].BackColor = Color.Orange;
                    //item.SubItems[3].BackColor = Color.Yellow;
                    //item.SubItems[4].BackColor = Color.LightGreen;
                    //item.SubItems[5].BackColor = Color.Orange;
                    //item.SubItems[6].BackColor = Color.LightGreen;
                    //item.SubItems[7].BackColor = Color.Orange;
                    if (clear_real_value > 0)
                        item.BackColor = Color.LightGreen;
                    else if (clear_real_value < 0)
                        item.BackColor = Color.Orange;
                    else
                        item.BackColor = Color.LightYellow;
                    ListViewMoneyDataDetails.Items.Add(item);

                }
                #endregion
            }
            else if (MoneyAccount_.Year != -1)
            {
                AccountLabelAccountType.Text = "حساب السنة";
                AccountLabelReport.Text = "تقرير حساب السنة : " + MoneyAccount_.GetAccountDateString();

                #region PayYearSection
                ListViewMoneyDataDetails.Items.Clear();
                if (ListViewMoneyDataDetails.Name != "ListViewMoneyDataDetails_Year")
                {
                    AccountOprMonthReportDetail.IntiliazeListView(ref ListViewMoneyDataDetails);
                }
                List<AccountOprMonthReportDetail> accountoprmonthreportlist
                       = new MoneyAccountSQL(DB).GetAccountOprReport_Details_InYear(MoneyAccount_.Year.ToString());
                for (int i = 0; i < accountoprmonthreportlist.Count; i++)
                {
                    ListViewItem item = new ListViewItem(accountoprmonthreportlist[i].Year_Month_Name.ToString());
                    item.Name = accountoprmonthreportlist[i].Year_Month.ToString();
                    item.SubItems.Add(accountoprmonthreportlist[i].PaysIN_Count.ToString());
                    item.SubItems.Add(accountoprmonthreportlist[i].PaysOUT_Count.ToString());
                    item.SubItems.Add(accountoprmonthreportlist[i].Exchange_Count.ToString());
                    item.SubItems.Add(accountoprmonthreportlist[i].PaysIN_Value);
                    item.SubItems.Add(accountoprmonthreportlist[i].PaysOUT_Value);
                    item.SubItems.Add(accountoprmonthreportlist[i].PaysIN_Real_Value + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(accountoprmonthreportlist[i].PaysOUT_Real_Value + " " + ReferenceCurrency.CurrencySymbol);
                    realValue_in += accountoprmonthreportlist[i].PaysIN_Real_Value;
                    realValue_out += accountoprmonthreportlist[i].PaysOUT_Real_Value;
                    double clear_real_value = accountoprmonthreportlist[i].PaysIN_Real_Value -
                        accountoprmonthreportlist[i].PaysOUT_Real_Value;
                    item.SubItems.Add(clear_real_value + " " + ReferenceCurrency.CurrencySymbol);
                    //item.UseItemStyleForSubItems = false;
                    //item.SubItems[0].BackColor = Color.LightGray;
                    //item.SubItems[1].BackColor = Color.LightGreen;
                    //item.SubItems[2].BackColor = Color.Orange;
                    //item.SubItems[3].BackColor = Color.Yellow;
                    //item.SubItems[4].BackColor = Color.LightGreen;
                    //item.SubItems[5].BackColor = Color.Orange;
                    //item.SubItems[6].BackColor = Color.LightGreen;
                    //item.SubItems[7].BackColor = Color.Orange;
                    if (clear_real_value > 0)
                        item.BackColor = Color.LightGreen;
                    else if (clear_real_value < 0)
                        item.BackColor = Color.Orange;
                    else
                        item.BackColor = Color.LightYellow;
                    ListViewMoneyDataDetails.Items.Add(item);

                }
                #endregion
            }
            else
            {
                AccountLabelAccountType.Text = "حساب السنوات";
                AccountLabelReport.Text = "تقرير حساب السنوات : " + MoneyAccount_.GetAccountDateString();

                #region PayYearRangeSection
                ListViewMoneyDataDetails.Items.Clear();
                if (ListViewMoneyDataDetails.Name != "ListViewMoneyDataDetails_YearRange")
                {
                    AccountOprYearReportDetail.IntiliazeListView(ref ListViewMoneyDataDetails);
                }
                List<AccountOprYearReportDetail> accountopryearreportlist
                        = new MoneyAccountSQL(DB).GetAccountOprReport_Details_InYearRange(MoneyAccount_.YearRange_.min_year.ToString(), MoneyAccount_.YearRange_.max_year.ToString());
                for (int i = 0; i < accountopryearreportlist.Count; i++)
                {
                    ListViewItem item = new ListViewItem(accountopryearreportlist[i].AccountYear.ToString());
                    item.Name = accountopryearreportlist[i].AccountYear.ToString();
                    item.SubItems.Add(accountopryearreportlist[i].PaysIN_Count.ToString());
                    item.SubItems.Add(accountopryearreportlist[i].PaysOUT_Count.ToString());
                    item.SubItems.Add(accountopryearreportlist[i].Exchange_Count.ToString());
                    item.SubItems.Add(accountopryearreportlist[i].PaysIN_Value);
                    item.SubItems.Add(accountopryearreportlist[i].PaysOUT_Value);
                    item.SubItems.Add(accountopryearreportlist[i].PaysIN_Real_Value + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(accountopryearreportlist[i].PaysOUT_Real_Value + " " + ReferenceCurrency.CurrencySymbol);
                    realValue_in += accountopryearreportlist[i].PaysIN_Real_Value;
                    realValue_out += accountopryearreportlist[i].PaysOUT_Real_Value;
                    double clear_real_value = accountopryearreportlist[i].PaysIN_Real_Value -
                         accountopryearreportlist[i].PaysOUT_Real_Value;
                    item.SubItems.Add(clear_real_value + " " + ReferenceCurrency.CurrencySymbol);
                    //item.UseItemStyleForSubItems = false;
                    //item.SubItems[0].BackColor = Color.LightGray;
                    //item.SubItems[1].BackColor = Color.LightGreen;
                    //item.SubItems[2].BackColor = Color.Orange;
                    //item.SubItems[3].BackColor = Color.Yellow;
                    //item.SubItems[4].BackColor = Color.LightGreen;
                    //item.SubItems[5].BackColor = Color.Orange;
                    //item.SubItems[6].BackColor = Color.LightGreen;
                    //item.SubItems[7].BackColor = Color.Orange;
                    if (clear_real_value > 0)
                        item.BackColor = Color.LightGreen;
                    else if (clear_real_value < 0)
                        item.BackColor = Color.Orange;
                    else
                        item.BackColor = Color.LightYellow;
                    ListViewMoneyDataDetails.Items.Add(item);

                }
                #endregion
            }
            #endregion
            textBox_Real_In_Money.Text = realValue_in.ToString() + ReferenceCurrency.CurrencySymbol;
            textBox_Real_out_Money.Text = realValue_out.ToString() + ReferenceCurrency.CurrencySymbol;
            textBox_Real_Clear_value.Text = (realValue_in - realValue_out).ToString() + ReferenceCurrency.CurrencySymbol;
            if ((realValue_in - realValue_out) < 0)
                textBox_Real_Clear_value.BackColor = Color.Orange;
            else
                textBox_Real_Clear_value.BackColor = Color.LimeGreen;
        }
        public void dd()
        {
            dataGridView1.Rows.Clear();
        
            List<PayCurrencyReport> PayCurrencyReportList = new List<PayCurrencyReport>();
            if (MoneyAccount_.Day != -1)
            {
                PayCurrencyReportList = MoneyAccountSQL_.GetPayReport_InDay(MoneyAccount_.Year, MoneyAccount_.Month, MoneyAccount_.Day);
            }
            else if (this.MoneyAccount_.Month  != -1)
            {
                PayCurrencyReportList = MoneyAccountSQL_.GetPayReport_InMonth(MoneyAccount_.Year, MoneyAccount_.Month);
            }
            else if (this.MoneyAccount_.Year  != -1)
            {
                PayCurrencyReportList = MoneyAccountSQL_.GetPayReport_INYear(MoneyAccount_.Year);
            }
            else
            {
                PayCurrencyReportList
                    = MoneyAccountSQL_.GetPayReport_betweenTwoYears(MoneyAccount_.YearRange_.min_year, MoneyAccount_.YearRange_.max_year);
            }
            string In_money="",Out_Money="";

            for (int i = 0; i < PayCurrencyReportList.Count; i++)
            {
                dataGridView1.Rows.Add();
                double in_all = PayCurrencyReportList[i].PaysIN_Sell
                    + PayCurrencyReportList[i].PaysIN_Maintenance
                    + PayCurrencyReportList[i].PaysIN_NON
                    + PayCurrencyReportList[i].PaysIN_Exchange;
                double out_all = PayCurrencyReportList[i].PaysOUT_Buy
               + PayCurrencyReportList[i].PaysOUT_Emp
               + PayCurrencyReportList[i].PaysOUT_NON
               + PayCurrencyReportList[i].PaysOUT_Exchange;
                double clear_value = in_all - out_all;
                dataGridView1.Rows[i].HeaderCell.Value = PayCurrencyReportList[i]._Currency .CurrencyName;

                dataGridView1.Rows[i].Cells[0].Value = PayCurrencyReportList[i]. PaysIN_Sell.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[1].Value = PayCurrencyReportList[i].PaysIN_Maintenance.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[2].Value = PayCurrencyReportList[i].PaysIN_NON.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[3].Value = PayCurrencyReportList[i].PaysIN_Exchange.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[4].Value =in_all  + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;

                dataGridView1.Rows[i].Cells[5].Value = PayCurrencyReportList[i].PaysOUT_Buy.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[6].Value = PayCurrencyReportList[i].PaysOUT_Emp.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[7].Value = PayCurrencyReportList[i].PaysOUT_NON.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[8].Value = PayCurrencyReportList[i].PaysOUT_Exchange.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[9].Value = out_all + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;
                dataGridView1.Rows[i].Cells[10].Value = clear_value + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol;

                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.LightGreen;
                dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.LightGreen;
                dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.LightGreen;
                dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.LightGreen;
                dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.LightGreen;
                dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                dataGridView1.Rows[i].Cells[6].Style.BackColor = Color.Orange;
                dataGridView1.Rows[i].Cells[7].Style.BackColor = Color.Orange;
                dataGridView1.Rows[i].Cells[8].Style.BackColor = Color.Orange;
                dataGridView1.Rows[i].Cells[9].Style.BackColor = Color.Orange;
                if(clear_value >0)
                    dataGridView1.Rows[i].Cells[10].Style.BackColor = Color.LightGreen;
                else
                    dataGridView1.Rows[i].Cells[10].Style.BackColor = Color.Orange ;
                if(in_all >0)
                In_money += in_all + PayCurrencyReportList[i]._Currency.CurrencySymbol+" ";
                if (out_all > 0)
                    Out_Money += out_all +  PayCurrencyReportList[i]._Currency.CurrencySymbol;
                if (i != PayCurrencyReportList.Count - 1)
                {
                    if (out_all > 0)
                        Out_Money += " , ";
                    if (in_all > 0)
                        In_money += " , ";
                }
            }
            if (In_money.Length < 1)
                In_money = "-";
            if (Out_Money.Length < 1)
                Out_Money = "-";
            textBox_In_Money.Text = In_money.ToString();
            textBox_out_Money.Text = Out_Money.ToString();
        }
        public async void Refresh_ListViewMoneyDataReport()
        {
            //ListViewMoneyDataReport.Items.Clear();
            //List<PayCurrencyReport> PayCurrencyReportList = new List<PayCurrencyReport>();
            //if (MoneyAccount_.Day != -1)
            //{
            //    PayCurrencyReportList = MoneyAccountSQL_.GetPayReport_InDay(Convert .ToInt32 ( MoneyAccount_.Year), MoneyAccount_.Month), MoneyAccount_.Day ));
            //}
            //else if (this.MoneyAccount_ != -1)
            //{
            //    PayCurrencyReportList =  MoneyAccountSQL_.GetPayReport_InMonth(MoneyAccount_.Year), MoneyAccount_.Month));
            //}
            //else if (this.MoneyAccount_ != -1)
            //{
            //    PayCurrencyReportList =  MoneyAccountSQL_.GetPayReport_INYear(MoneyAccount_.Year));
            //}
            //else
            //{
            //    PayCurrencyReportList
            //        = MoneyAccountSQL_.GetPayReport_betweenTwoYears(MoneyAccount_.YearRange_.min_year), MoneyAccount_.YearRange_.max_year));
            //}
            //for (int i = 0; i < PayCurrencyReportList.Count; i++)
            //{
            //    ListViewItem item = new ListViewItem(PayCurrencyReportList[i]._Currency .CurrencyName);
            //    item.Name = PayCurrencyReportList[i]._Currency .CurrencyID.ToString();
            //    item.SubItems.Add(PayCurrencyReportList[i].PaysIN_Pays.ToString()+" "+ PayCurrencyReportList[i]._Currency .CurrencySymbol );
            //    item.SubItems.Add(PayCurrencyReportList[i].PaysIN_Exchange.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol);
            //    item.SubItems.Add(PayCurrencyReportList[i].PaysIN_ALL.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol);
            //    item.SubItems.Add(PayCurrencyReportList[i].PaysOUT_Pays.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol);
            //    item.SubItems.Add(PayCurrencyReportList[i].PaysOUT_Exchange.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol);
            //    item.SubItems.Add(PayCurrencyReportList[i].PaysOUT_ALL.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol);
            //    item.SubItems.Add(PayCurrencyReportList[i].ClearValue.ToString() + " " + PayCurrencyReportList[i]._Currency.CurrencySymbol);
            //    item.UseItemStyleForSubItems = false;
            //    item.SubItems[0].BackColor = Color.LightGray;
            //    item.SubItems[1].BackColor = Color.LightGreen;
            //    item.SubItems[2].BackColor = Color.LightGreen;
            //    item.SubItems[3].BackColor = Color.LightGreen;
            //    item.SubItems[4].BackColor = Color.Orange;
            //    item.SubItems[5].BackColor = Color.Orange;
            //    item.SubItems[6].BackColor = Color.Orange;
            //    item.SubItems[7].BackColor = Color.LightBlue;
            //    ListViewMoneyDataReport.Items.Add(item);

            //}
        }
        private void AccountBack_Click(object sender, EventArgs e)
        {
            if (MoneyAccount_.Year == -1) return;
            MoneyAccount_.Account_Date_UP();
            Refresh_ListViewMoneyDataDetails();
        }

        private void AccountButtonLeftRight_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool left;
            if (b.Name == "AccountButtonLeft") left = true;
            else left = false;

            if (MoneyAccount_.Day != -1)
            {
                if (left)
                {
                    if (MoneyAccount_.Day == DateTime.DaysInMonth(MoneyAccount_.Year, MoneyAccount_.Month))
                    {
                        if (MoneyAccount_.Month == 12)
                        { MoneyAccount_.Year++; MoneyAccount_.Month = 1; MoneyAccount_.Day = 1; }
                        else
                        { MoneyAccount_.Month++; MoneyAccount_.Day = 1; }

                    }
                    else MoneyAccount_.Day++;
                }
                else
                {
                    if (MoneyAccount_.Day == 1)
                    {

                        if (MoneyAccount_.Month == 1)
                        { MoneyAccount_.Year--; MoneyAccount_.Month = 12; }
                        else
                        { MoneyAccount_.Month--; }
                        MoneyAccount_.Day = DateTime.DaysInMonth(MoneyAccount_.Year, MoneyAccount_.Month);
                    }
                    else MoneyAccount_.Day--;
                }

            }
            else if (MoneyAccount_.Month != -1)
            {
                if (left)
                {
                    if (MoneyAccount_.Month == 12)
                    {
                        MoneyAccount_.Year++; MoneyAccount_.Month = 1;
                    }
                    else MoneyAccount_.Month++;
                }
                else
                {
                    if (MoneyAccount_.Month == 1)
                    {
                        MoneyAccount_.Year--; MoneyAccount_.Month = 12;
                    }
                    else MoneyAccount_.Month--;
                }
            }
            else if (MoneyAccount_.Year != -1)
            {
                if (left)
                {
                    MoneyAccount_.Year++;
                    MoneyAccount_.YearRange_.min_year++;
                    MoneyAccount_.YearRange_.max_year++;
                }
                else
                {
                    MoneyAccount_.Year--;
                    MoneyAccount_.YearRange_.min_year--;
                    MoneyAccount_.YearRange_.max_year--;
                }
            }
            else
            {
                if (left)
                {

                    MoneyAccount_.YearRange_.min_year += 10;
                    MoneyAccount_.YearRange_.max_year += 10;
                }
                else
                {
                    MoneyAccount_.YearRange_.min_year -= 10;
                    MoneyAccount_.YearRange_.max_year -= 10;
                }
            }
            Refresh_ListViewMoneyDataDetails();
        }

        public void ListViewMoneyDataDetailsAccountDown()
        {
            try
            {
                if (MoneyAccount_.Year == -1 || MoneyAccount_.Month == -1 || MoneyAccount_.Day == -1)
                {
                    MoneyAccount_.Account_Date_Down(Convert .ToInt32( ListViewMoneyDataDetails.SelectedItems[0].Name));
                    Refresh_ListViewMoneyDataDetails();
                }
                else
                {

                    Open_MoneyOPR_MenuItem.PerformClick();
                        //string s = ListViewMoneyDataDetails.SelectedItems[0].Name;
                        //if (s.Substring(0,1) == "P")
                        //{
                        //    if (s.Substring(3, 3) == "I")
                        //    {
                        //        uint payinid = Convert.ToUInt32(s.Substring(6));
                        //        PayIN PayIN_ = new PayINSQL(DB).GetPayIN_INFO_BYID(payinid);
                        //        PayINForm PayINForm_ = new PayINForm(DB, PayIN_, false);
                        //        PayINForm_.ShowDialog();
                        //        if (PayINForm_.Changed)
                        //        {
                        //            RefreshAccount();
                        //        }
                        //    }
                        //    else
                        //    {
                        //        uint payoutid = Convert.ToUInt32(s.Substring(6));
                        //        PayOUT PayOUT_ = new PayOUTSQL(DB).GetPayOUT_INFO_BYID(payoutid);
                        //        PayOUTForm PayOUTForm_ = new PayOUTForm(DB, PayOUT_, false);
                        //        PayOUTForm_.ShowDialog();
                        //        if (PayOUTForm_.Changed)
                        //        {
                        //            RefreshAccount();
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //    uint exchangeoprid = Convert.ToUInt32(s.Substring(6));
                        //    ExchangeOPR ExchangeOPR_ = new ExchangeOPRSQL(DB).GetExchangeOPR_INFO_BYID(exchangeoprid);
                        //    ExchangeOPRForm ExchangeOPRForm_ = new ExchangeOPRForm(DB, ExchangeOPR_, false);
                        //    ExchangeOPRForm_.ShowDialog();
                        //    if (ExchangeOPRForm_.Changed)
                        //    {
                        //        RefreshAccount();
                        //    }
                        //    ExchangeOPRForm_.Dispose();

                        //}
                    }

             
            }
            catch (Exception ee)
            {

            }


        }
        private void ListViewMoneyDataDetails_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListViewMoneyDataDetails.SelectedItems.Count > 0)
                ListViewMoneyDataDetailsAccountDown();
        }
        private void ListViewMoneyDataDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                ListViewMoneyDataDetailsAccountDown();
        }
        private void ListViewMoneyDataDetails_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ListViewMoneyDataDetails.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    foreach (ListViewItem item1 in ListViewMoneyDataDetails.Items)
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
                        if (MoneyAccount_ .Day != -1)
                        {

                            List<MenuItem> MenuItemList = new List<MenuItem>();
                            MenuItemList.Add(Refresh_MenuItem);
                            MenuItemList.Add(new MenuItem("-"));
                            MenuItemList.AddRange(new MenuItem[] { Edit_MoneyOPR_MenuItem, Delete_MoneyOPR_MenuItem });
                            MenuItemList.Add(new MenuItem("-"));
                             MenuItemList.AddRange(new MenuItem[] { AddPayIN_MenuItem, AddPayOUT_MenuItem, new MenuItem("-"), AddExchangeOPR_MenuItem });
                            ListViewMoneyDataDetails.ContextMenu = new ContextMenu(MenuItemList.ToArray());


                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            ListViewMoneyDataDetails.ContextMenu = new ContextMenu(mi1);


                        }


                    }
                    else
                    {
                        if (MoneyAccount_.Day != -1)
                        {
                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem, new MenuItem("-"), AddPayIN_MenuItem, AddPayOUT_MenuItem, new MenuItem("-"), AddExchangeOPR_MenuItem };
                            ListViewMoneyDataDetails.ContextMenu = new ContextMenu(mi1);
                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            ListViewMoneyDataDetails.ContextMenu = new ContextMenu(mi1);


                        }

                    }

                }
            }
        }
  
        public async  void IntializeListViewMoneyDataDetailsColumnsWidth()
        {

            if (MoneyAccount_.Day != -1)
            {
  
                
                ListViewMoneyDataDetails.Columns[0].Width = 100;//
                ListViewMoneyDataDetails.Columns[1].Width = 100;
                ListViewMoneyDataDetails.Columns[2].Width = 150;
                ListViewMoneyDataDetails.Columns[3].Width = 100;
                ListViewMoneyDataDetails.Columns[4].Width = 200;

                ListViewMoneyDataDetails.Columns[5].Width = 150;
                ListViewMoneyDataDetails.Columns[6].Width = 200;
                if (ListViewMoneyDataDetails.Width>1010)
                    ListViewMoneyDataDetails.Columns[7].Width = ListViewMoneyDataDetails.Width - 1005;
            }
            else
            {
                ListViewMoneyDataDetails.Columns[0].Width = 100;
                ListViewMoneyDataDetails.Columns[1].Width = 150;
                ListViewMoneyDataDetails.Columns[2].Width = 150;
                ListViewMoneyDataDetails.Columns[3].Width = 150;
                ListViewMoneyDataDetails.Columns[4].Width = (ListViewMoneyDataDetails.Width - 1005) / 2;
                ListViewMoneyDataDetails.Columns[5].Width = (ListViewMoneyDataDetails.Width - 1005) / 2;
                ListViewMoneyDataDetails.Columns[6].Width = 150;
                ListViewMoneyDataDetails.Columns[7].Width = 150;
                ListViewMoneyDataDetails.Columns[8].Width = 150;
            }
        }
       
        public async  void  IntializeListAccountListViewReport_ColumnsWidth()
        {
            //ListViewMoneyDataReport.Columns[0].Width = 100;
            //int w= (ListViewMoneyDataReport.Width - 101) / 7; ;
            //ListViewMoneyDataReport.Columns[1].Width = w;
            //ListViewMoneyDataReport.Columns[2].Width = w;
            //ListViewMoneyDataReport.Columns[3].Width = w;
            //ListViewMoneyDataReport.Columns[4].Width = w;
            //ListViewMoneyDataReport.Columns[5].Width = w;
            //ListViewMoneyDataReport.Columns[6].Width = w;
            //ListViewMoneyDataReport.Columns[7].Width = w;
        }
        private void AddExchangeOPR_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExchangeOPRForm ExchangeOPRForm_ = new ExchangeOPRForm(DB, GetSelectedDate());
                ExchangeOPRForm_.ShowDialog();
                if (ExchangeOPRForm_.Changed)
                {
                    Refresh_ListViewMoneyDataDetails();
                }


                TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            }
        }

        private void AddPayIN_MenuItem_Click(object sender, EventArgs e)
        {

            PayINForm PayINForm_ = new PayINForm(DB,MoneyAccount_ .GetDate());
            PayINForm_.ShowDialog();
            if (PayINForm_.DialogResult == DialogResult.OK)
            {
                Refresh_ListViewMoneyDataDetails();
            }


            TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();


        }

        private void AddPayOUT_MenuItem_Click(object sender, EventArgs e)
        {
            PayOUTForm PayOUTForm_ = new PayOUTForm(DB, MoneyAccount_.GetDate());
            PayOUTForm_.ShowDialog();
            if (PayOUTForm_.DialogResult == DialogResult.OK)
            {
                Refresh_ListViewMoneyDataDetails();
                TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
            }


          

        }

        private void Delete_MoneyOPR_MenuItem_Click(object sender, EventArgs e)
        {


            DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd != DialogResult.OK) return;
            string s = ListViewMoneyDataDetails.SelectedItems[0].Name;
            if (s.Substring(0, 1) == "P")
            {
                if (s.Substring(1, 1) == "I")
                {
                    uint payinid = Convert.ToUInt32(s.Substring(2));
                    bool success = new PayINSQL(DB).Delete_PayIN(payinid);
                    if (success)
                    {
                        Refresh_ListViewMoneyDataDetails();
                        TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
                    }
                }
                else
                {
                    uint payoutid = Convert.ToUInt32(s.Substring(2));
                    bool success = new PayOUTSQL(DB).Delete_PayOUT(payoutid);
                    if (success)
                    {
                        Refresh_ListViewMoneyDataDetails();
                        TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
                    }
                }


            }
            else
            {

                uint exchangeoprid = Convert.ToUInt32(s.Substring(2));
                bool success = new ExchangeOPRSQL(DB).Delete_ExchageOPR(exchangeoprid);
                if (success)
                {
                    Refresh_ListViewMoneyDataDetails();
                    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
                }

            }

        }

        private void Edit_MoneyOPR_MenuItem_Click(object sender, EventArgs e)
        {

            string s = ListViewMoneyDataDetails.SelectedItems[0].Name;
            if (s.Substring(0, 1) == "P")
            {
                if (s.Substring(1, 1) == "I")
                {
                    uint payinid = Convert.ToUInt32(s.Substring(2));
                    PayIN PayIN_ = new PayINSQL(DB).GetPayIN_INFO_BYID(payinid);
                    PayINForm PayINForm_ = new PayINForm(DB, PayIN_, true);
                    PayINForm_.ShowDialog();
                    if (PayINForm_.Changed)
                    {
                        Refresh_ListViewMoneyDataDetails();
                        TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();

                    }
                }
                else
                {
                    uint payoutid = Convert.ToUInt32(s.Substring(2));
                    PayOUT PayOUT_ = new PayOUTSQL(DB).GetPayOUT_INFO_BYID(payoutid);
                    PayOUTForm PayOUTForm_ = new PayOUTForm(DB, PayOUT_, true);
                    PayOUTForm_.ShowDialog();
                    if (PayOUTForm_.Changed)
                    {
                        Refresh_ListViewMoneyDataDetails();
                        TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();

                    }
                }


            }
            else
            {
                uint exchangeoprid = Convert.ToUInt32(s.Substring(2));
                ExchangeOPR ExchangeOPR_ = new ExchangeOPRSQL(DB).GetExchangeOPR_INFO_BYID(exchangeoprid);
                ExchangeOPRForm ExchangeOPRForm_ = new ExchangeOPRForm(DB, ExchangeOPR_, true);
                ExchangeOPRForm_.ShowDialog();
                if (ExchangeOPRForm_.Changed)
                {
                    Refresh_ListViewMoneyDataDetails();
                    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();

                }
                ExchangeOPRForm_.Dispose();

            }

        }
        private void Open_MoneyOPR_MenuItem_Click(object sender, EventArgs e)
        {

            string s = ListViewMoneyDataDetails.SelectedItems[0].Name;
            if (s.Substring(0, 1) == "P")
            {
                if (s.Substring(1, 1) == "I")
                {
                    uint payinid = Convert.ToUInt32(s.Substring(2));
                    PayIN PayIN_ = new PayINSQL(DB).GetPayIN_INFO_BYID(payinid);
                    PayINForm PayINForm_ = new PayINForm(DB, PayIN_, false );
                    PayINForm_.ShowDialog();
                    if (PayINForm_.Changed)
                    {
                        Refresh_ListViewMoneyDataDetails();
                        TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();

                    }
                }
                else
                {
                    uint payoutid = Convert.ToUInt32(s.Substring(2));
                    PayOUT PayOUT_ = new PayOUTSQL(DB).GetPayOUT_INFO_BYID(payoutid);
                    PayOUTForm PayOUTForm_ = new PayOUTForm(DB, PayOUT_, false );
                    PayOUTForm_.ShowDialog();
                    if (PayOUTForm_.Changed)
                    {
                        Refresh_ListViewMoneyDataDetails();
                        TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();

                    }
                }


            }
            else
            {
                uint exchangeoprid = Convert.ToUInt32(s.Substring(2));
                ExchangeOPR ExchangeOPR_ = new ExchangeOPRSQL(DB).GetExchangeOPR_INFO_BYID(exchangeoprid);
                ExchangeOPRForm ExchangeOPRForm_ = new ExchangeOPRForm(DB, ExchangeOPR_, false );
                ExchangeOPRForm_.ShowDialog();
                if (ExchangeOPRForm_.Changed)
                {
                    Refresh_ListViewMoneyDataDetails();
                    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();

                }
                ExchangeOPRForm_.Dispose();

            }

        }
        #endregion

        #region ReportBuys
        public async void Buys_FillReport()
        {

            BuysLabelAccountDate.Text = BuysAccount_.GetAccountDateString();



            if (BuysAccount_.Day != -1)
            {

                BuysLabelAccountType.Text = "حساب اليوم";
                BuysLabelReport.Text = "تقرير حساب اليوم : " + BuysAccount_.GetAccountDateString();
                #region DaySection

                Report_Buys_Month_ReportDetail Report_Buys_DayReport
                    = new ReportBuysSQL(DB).Get_Report_Buys_Day_Report(BuysAccount_.Year, BuysAccount_.Month, BuysAccount_.Day);
                textBoxBuys_AmountIN.Text = Report_Buys_DayReport.Amount_IN.ToString ();
                textBoxBuys_AmountRemain.Text = Report_Buys_DayReport.Amount_Remain.ToString ();
                textBoxBuys_Value.Text = Report_Buys_DayReport.Bills_Value;
                textBoxBuysPaysValue.Text = Report_Buys_DayReport.Bills_Pays_Value;
                textBoxBuysPaysRmain.Text = Report_Buys_DayReport.Bills_Pays_Remain;
                if (Report_Buys_DayReport.Bills_Pays_Remain_UPON_Bill_Currency > 0)
                    textBoxBuysPaysRmain.BackColor = Color.Orange;
                else
                    textBoxBuysPaysRmain.BackColor = Color.LimeGreen;
                textBoxBuyRealValue.Text = Report_Buys_DayReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBuys_RealPays.Text = Report_Buys_DayReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

                textBoxBuys_OutValue.Text = Report_Buys_DayReport.Bills_ItemsOut_Value;
                textBoxBuys_OutRealValue.Text = Report_Buys_DayReport.Bills_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBuys_Out_Pays.Text = Report_Buys_DayReport.Bills_Pays_Return_Value;
                textBoxBuys_Out_Pays_RealValue.Text = Report_Buys_DayReport.Bills_Pays_Return_RealValue + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else if (BuysAccount_.Month != -1)
            {
                BuysLabelAccountType.Text = "حساب الشهر";
                BuysLabelReport.Text = "تقرير حساب الشهر : " + BuysAccount_.GetAccountDateString();

                #region MonthSection

                Report_Buys_Year_ReportDetail Report_Buys_MonthReport
                     = new ReportBuysSQL(DB).Get_Report_Buys_Month_Report(BuysAccount_.Year, BuysAccount_.Month);
                textBoxBuys_AmountIN.Text = Report_Buys_MonthReport.Amount_IN.ToString();
                textBoxBuys_AmountRemain.Text = Report_Buys_MonthReport.Amount_Remain.ToString();
                textBoxBuys_Value.Text = Report_Buys_MonthReport.Bills_Value;
                textBoxBuysPaysValue.Text = Report_Buys_MonthReport.Bills_Pays_Value;
                textBoxBuysPaysRmain.Text = Report_Buys_MonthReport.Bills_Pays_Remain;
                if (Report_Buys_MonthReport.Bills_Pays_Remain_UPON_Bill_Currency > 0)
                    textBoxBuysPaysRmain.BackColor = Color.Orange;
                else
                    textBoxBuysPaysRmain.BackColor = Color.LimeGreen;
                textBoxBuyRealValue.Text = Report_Buys_MonthReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBuys_RealPays.Text = Report_Buys_MonthReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

                textBoxBuys_OutValue.Text = Report_Buys_MonthReport.Bills_ItemsOut_Value;
                textBoxBuys_OutRealValue.Text = Report_Buys_MonthReport.Bills_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBuys_Out_Pays.Text = Report_Buys_MonthReport.Bills_Pays_Return_Value;
                textBoxBuys_Out_Pays_RealValue.Text = Report_Buys_MonthReport.Bills_Pays_Return_RealValue + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else if (BuysAccount_.Year != -1)
            {
                BuysLabelAccountType.Text = "حساب السنة";
                BuysLabelReport.Text = "تقرير حساب السنة : " + BuysAccount_.GetAccountDateString();

                #region YearSection

                Report_Buys_YearRange_ReportDetail Report_Buys_YearReport
                   = new ReportBuysSQL(DB).Get_Report_Buys_Year_Report(BuysAccount_.Year);
                textBoxBuys_AmountIN.Text = Report_Buys_YearReport.Amount_IN.ToString();
                textBoxBuys_AmountRemain.Text = Report_Buys_YearReport.Amount_Remain.ToString();
                textBoxBuys_Value.Text = Report_Buys_YearReport.Bills_Value;
                textBoxBuysPaysValue.Text = Report_Buys_YearReport.Bills_Pays_Value;
                textBoxBuysPaysRmain.Text = Report_Buys_YearReport.Bills_Pays_Remain;
                if (Report_Buys_YearReport.Bills_Pays_Remain_UPON_Bill_Currency > 0)
                    textBoxBuysPaysRmain.BackColor = Color.Orange;
                else
                    textBoxBuysPaysRmain.BackColor = Color.LimeGreen;
                textBoxBuyRealValue.Text = Report_Buys_YearReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBuys_RealPays.Text = Report_Buys_YearReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

                textBoxBuys_OutValue.Text = Report_Buys_YearReport.Bills_ItemsOut_Value;
                textBoxBuys_OutRealValue.Text = Report_Buys_YearReport.Bills_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBuys_Out_Pays.Text = Report_Buys_YearReport.Bills_Pays_Return_Value;
                textBoxBuys_Out_Pays_RealValue.Text = Report_Buys_YearReport.Bills_Pays_Return_RealValue + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else
            {
                BuysLabelAccountType.Text = "حساب السنوات";
                BuysLabelReport.Text = "تقرير حساب السنوات : " + BuysAccount_.GetAccountDateString();

                #region YearRangeSection

                textBoxBuys_AmountIN.Text = "-";
                textBoxBuys_Value.Text = "-";
                textBoxBuysPaysValue.Text = "-";
                textBoxBuysPaysRmain.Text = "-";
                textBoxBuys_Out_Pays.Text = "-";
                textBoxBuyRealValue.Text = "-";
                textBoxBuys_OutValue.Text = "-";
                textBoxBuys_RealPays.Text = "-";
                #endregion
            }


        }
        public async void Refresh_ListViewBuys()
        {

            listViewBuys.Items.Clear();
            if (BuysAccount_.Day != -1)
            {

                #region DaySection


                if (listViewBuys.Name != "ListViewBuys_Day")
                {
                    Report_Buys_Day_ReportDetail.IntiliazeListView(ref listViewBuys);

                }
                List<Report_Buys_Day_ReportDetail> Report_Buys_Day_ReportDetail_List
                          = new ReportBuysSQL(DB).Get_Report_Buys_Day_ReportDetail(BuysAccount_.Year, BuysAccount_.Month, BuysAccount_.Day);
                for (int i = 0; i < Report_Buys_Day_ReportDetail_List.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_Buys_Day_ReportDetail_List[i].Bill_Time.ToShortTimeString());
                    item.Name = Report_Buys_Day_ReportDetail_List[i].Bill_ID.ToString();
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Bill_ID.ToString());
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Bill_Owner);
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].ClauseS_Count.ToString());
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Amount_IN .ToString());
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Amount_Remain.ToString());
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].BillValue .ToString() + " " + Report_Buys_Day_ReportDetail_List[i]._Currency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].ExchangeRate.ToString());
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].PaysAmount);
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].PaysRemain.ToString() + " " + Report_Buys_Day_ReportDetail_List[i]._Currency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Bill_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Bill_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Bill_ItemsOut_Value.ToString() );
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Bill_Pays_Return_Value);
                    item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].Bill_Pays_Return_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);

                    //-Report_Buys_Day_ReportDetail_List[i].Source_ItemsIN_RealCost) + " " + ReferenceCurrency.CurrencySymbol);
                    //item.SubItems.Add(Report_Buys_Day_ReportDetail_List[i].RealPaysValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);

                     item.UseItemStyleForSubItems = false;
                    if(Report_Buys_Day_ReportDetail_List[i].Amount_Remain ==0)
                        for (int j =4; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else
                        for (int j = 4; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    if (Report_Buys_Day_ReportDetail_List[i].PaysRemain != 0)
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;

                    if (Report_Buys_Day_ReportDetail_List[i].Bill_Pays_RealValue
                        > Report_Buys_Day_ReportDetail_List[i].Bill_Pays_Return_RealValue)
                        for (int j = 10; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Buys_Day_ReportDetail_List[i].Bill_Pays_RealValue
                        < Report_Buys_Day_ReportDetail_List[i].Bill_Pays_Return_RealValue)
                        for (int j = 10; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 10; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    listViewBuys.Items.Add(item);


                }
                #endregion
            }
            else if (BuysAccount_.Month != -1)
            {

                #region MonthSection
                if (listViewBuys.Name != "ListViewBuys_Month")
                {
                    Report_Buys_Month_ReportDetail.IntiliazeListView(ref listViewBuys);
                }
                List<Report_Buys_Month_ReportDetail> Report_Buys_Month_ReportDetailList
                                    = new ReportBuysSQL(DB).Get_Report_Buys_Month_ReportDetail(BuysAccount_.Year, BuysAccount_.Month);
                for (int i = 0; i < Report_Buys_Month_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_Buys_Month_ReportDetailList[i].DayDate.ToShortDateString());
                    item.Name = Report_Buys_Month_ReportDetailList[i].DayID.ToString();
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Count.ToString());
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Clause_Count.ToString());
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Amount_IN .ToString());
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Amount_Remain.ToString());
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Value.ToString());
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Pays_Value);
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Pays_Remain);

                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);

                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_ItemsOut_Value);
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Pays_Return_Value);
                    item.SubItems.Add(Report_Buys_Month_ReportDetailList[i].Bills_Pays_Return_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.UseItemStyleForSubItems = false;
                    if(Report_Buys_Month_ReportDetailList[i].Amount_IN > 0)
                    {
                        if (Report_Buys_Month_ReportDetailList[i].Amount_Remain == 0)
                            for (int j = 3; j <= 4; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 3; j <= 4; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                    }
                   else
                        for (int j = 3; j <= 4; j++)
                            item.SubItems[j].BackColor = Color.LightYellow ;
                    if(Report_Buys_Month_ReportDetailList[i].Bills_RealValue > 0)
                    {
                        if (Report_Buys_Month_ReportDetailList[i].Bills_Pays_Remain_UPON_Bill_Currency != 0)
                            for (int j = 5; j <= 7; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 5; j <= 7; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                    }
                    else
                        for (int j = 5; j <= 7; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    if (Report_Buys_Month_ReportDetailList[i].Bills_Pays_RealValue
                        > Report_Buys_Month_ReportDetailList[i].Bills_Pays_Return_RealValue)
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Buys_Month_ReportDetailList[i].Bills_Pays_RealValue
                        < Report_Buys_Month_ReportDetailList[i].Bills_Pays_Return_RealValue)
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    listViewBuys.Items.Add(item);

                }
                #endregion
            }
            else if (BuysAccount_.Year != -1)
            {

                #region YearSection
                if (listViewBuys.Name != "ListViewBuys_Year")
                {

                    Report_Buys_Year_ReportDetail.IntiliazeListView(ref listViewBuys);

                }

                List<Report_Buys_Year_ReportDetail> Report_Buys_Year_ReportDetailList
                           = new ReportBuysSQL(DB).Get_Report_Buys_Year_ReportDetail(BuysAccount_.Year);
                for (int i = 0; i < Report_Buys_Year_ReportDetailList.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_Buys_Year_ReportDetailList[i].MonthName);
                    item.Name = Report_Buys_Year_ReportDetailList[i].MonthNO.ToString();
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Count.ToString());
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Clause_Count.ToString());
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Amount_IN.ToString());
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Amount_Remain.ToString());
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Value.ToString());
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Pays_Value);
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Pays_Remain);

                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);

                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_ItemsOut_Value);
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Pays_Return_Value);
                    item.SubItems.Add(Report_Buys_Year_ReportDetailList[i].Bills_Pays_Return_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.UseItemStyleForSubItems = false;

                    if (Report_Buys_Year_ReportDetailList[i].Amount_IN > 0)
                    {
                        if (Report_Buys_Year_ReportDetailList[i].Amount_Remain == 0)
                            for (int j = 3; j <= 4; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 3; j <= 4; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                    }
                    else
                        for (int j = 3; j <= 4; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    if (Report_Buys_Year_ReportDetailList[i].Bills_RealValue > 0)
                    {
                        if (Report_Buys_Year_ReportDetailList[i].Bills_Pays_Remain_UPON_Bill_Currency != 0)
                            for (int j = 5; j <= 7; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 5; j <= 7; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                    }
                    else
                        for (int j = 5; j <= 7; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    if (Report_Buys_Year_ReportDetailList[i].Bills_Pays_RealValue
                        > Report_Buys_Year_ReportDetailList[i].Bills_Pays_Return_RealValue)
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Buys_Year_ReportDetailList[i].Bills_Pays_RealValue
                        < Report_Buys_Year_ReportDetailList[i].Bills_Pays_Return_RealValue)
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    listViewBuys.Items.Add(item);

                }
                #endregion
            }
            else
            {
                //BuysLabelAccountType.Text = "حساب السنوات";
                //BuysLabelReport.Text = "تقرير حساب السنوات : " + BuysAccount_.GetAccountDateString();

                #region YearRangeSection
                if (listViewBuys.Name != "ListViewBuys_YearRange")
                {
                    Report_Buys_YearRange_ReportDetail.IntiliazeListView(ref listViewBuys);
                }
                List<Report_Buys_YearRange_ReportDetail> Report_Buys_YearRange_ReportDetailList
                           = new ReportBuysSQL(DB).Get_Report_Buys_YearRange_ReportDetail(BuysAccount_.YearRange_.min_year, BuysAccount_.YearRange_.max_year);

                for (int i = 0; i < Report_Buys_YearRange_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_Buys_YearRange_ReportDetailList[i].YearNO.ToString());
                    item.Name = Report_Buys_YearRange_ReportDetailList[i].YearNO.ToString();
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Count.ToString());
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Clause_Count.ToString());
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Amount_IN.ToString());
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Amount_Remain.ToString());
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Value.ToString());
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_Value);
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_Remain);

                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);

                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_ItemsOut_Value);
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_Return_Value);
                    item.SubItems.Add(Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_Return_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.UseItemStyleForSubItems = false;

                    if (Report_Buys_YearRange_ReportDetailList[i].Amount_IN > 0)
                    {
                        if (Report_Buys_YearRange_ReportDetailList[i].Amount_Remain == 0)
                            for (int j = 3; j <= 4; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 3; j <= 4; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                    }
                    else
                        for (int j = 3; j <= 4; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    if (Report_Buys_YearRange_ReportDetailList[i].Bills_RealValue > 0)
                    {
                        if (Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_Remain_UPON_Bill_Currency != 0)
                            for (int j = 5; j <= 7; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 5; j <= 7; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                    }
                    else
                        for (int j = 5; j <= 7; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    if (Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_RealValue
                        > Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_Return_RealValue)
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_RealValue
                        < Report_Buys_YearRange_ReportDetailList[i].Bills_Pays_Return_RealValue)
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 8; j <= 13; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    listViewBuys.Items.Add(item);

                }

                #endregion
            }

            Buys_FillReport();


        }
        private void BuysBack_Click(object sender, EventArgs e)
        {
            if (BuysAccount_.Year == -1) return;
            BuysAccount_.Account_Date_UP();
            Refresh_ListViewBuys();
        }

        private void BuysButtonLeftRight_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool left;
            if (b.Name == "BuysButtonLeft") left = true;
            else left = false;

            if (BuysAccount_.Day != -1)
            {
                if (left)
                {
                    if (BuysAccount_.Day == DateTime.DaysInMonth(BuysAccount_.Year, BuysAccount_.Month))
                    {
                        if (BuysAccount_.Month == 12)
                        { BuysAccount_.Year++; BuysAccount_.Month = 1; BuysAccount_.Day = 1; }
                        else
                        { BuysAccount_.Month++; BuysAccount_.Day = 1; }

                    }
                    else BuysAccount_.Day++;
                }
                else
                {
                    if (BuysAccount_.Day == 1)
                    {

                        if (BuysAccount_.Month == 1)
                        { BuysAccount_.Year--; BuysAccount_.Month = 12; }
                        else
                        { BuysAccount_.Month--; }
                        BuysAccount_.Day = DateTime.DaysInMonth(BuysAccount_.Year, BuysAccount_.Month);
                    }
                    else BuysAccount_.Day--;
                }

            }
            else if (BuysAccount_.Month != -1)
            {
                if (left)
                {
                    if (BuysAccount_.Month == 12)
                    {
                        BuysAccount_.Year++; BuysAccount_.Month = 1;
                    }
                    else BuysAccount_.Month++;
                }
                else
                {
                    if (BuysAccount_.Month == 1)
                    {
                        BuysAccount_.Year--; BuysAccount_.Month = 12;
                    }
                    else BuysAccount_.Month--;
                }
            }
            else if (BuysAccount_.Year != -1)
            {
                if (left)
                {
                    BuysAccount_.Year++;
                    BuysAccount_.YearRange_.min_year++;
                    BuysAccount_.YearRange_.max_year++;
                }
                else
                {
                    BuysAccount_.Year--;
                    BuysAccount_.YearRange_.min_year--;
                    BuysAccount_.YearRange_.max_year--;
                }
            }
            else
            {
                if (left)
                {

                    BuysAccount_.YearRange_.min_year += 10;
                    BuysAccount_.YearRange_.max_year += 10;
                }
                else
                {
                    BuysAccount_.YearRange_.min_year -= 10;
                    BuysAccount_.YearRange_.max_year -= 10;
                }
            }
            Refresh_ListViewBuys();
        }

        public void ListViewBuysAccountDown()
        {
            try
            {
                if (BuysAccount_.Year == -1 || BuysAccount_.Month == -1 || BuysAccount_.Day == -1)
                {
                    BuysAccount_.Account_Date_Down(Convert.ToInt32(listViewBuys.SelectedItems[0].Name));
                    Refresh_ListViewBuys();
                }
                else
                {

                    OpenBillBuy_MenuItem.PerformClick();

                }


            }
            catch (Exception ee)
            {

            }


        }
        private void ListViewBuys_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewBuys.SelectedItems.Count > 0)
                ListViewBuysAccountDown();
        }
        private void ListViewBuys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                ListViewBuysAccountDown();
        }
        private void ListViewBuys_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listViewBuys.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    foreach (ListViewItem item1 in listViewBuys.Items)
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
                        if (BuysAccount_.Day != -1)
                        {

                            List<MenuItem> MenuItemList = new List<MenuItem>();
                            MenuItemList.Add(Refresh_MenuItem);
                            MenuItemList.Add(new MenuItem("-"));
                            MenuItemList.AddRange(new MenuItem[] {OpenBillBuy_MenuItem , EditBillBuy_MenuItem , DeleteBillBuy_MenuItem
                            , new MenuItem("-"),CreateBillBuy_MenuItem });
                            MenuItemList.AddRange(new MenuItem[] { new MenuItem("-"),AddPayOUT_BillBuy_MenuItem });
                            listViewBuys.ContextMenu = new ContextMenu(MenuItemList.ToArray());


                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewBuys.ContextMenu = new ContextMenu(mi1);


                        }


                    }
                    else
                    {
                        if (BuysAccount_.Day != -1)
                        {
                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem, new MenuItem("-"), CreateBillBuy_MenuItem};
                            listViewBuys.ContextMenu = new ContextMenu(mi1);
                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewBuys.ContextMenu = new ContextMenu(mi1);


                        }

                    }

                }
            }
        }

        public async void IntializeListViewBuysColumnsWidth()
        {

            if (BuysAccount_.Day != -1)
            {


                listViewBuys .Columns[0].Width = 75;//time
                listViewBuys.Columns[1].Width = 60;//id
                listViewBuys.Columns[2].Width = 100;//owner
                listViewBuys.Columns[3].Width = 60;//clause count
                listViewBuys.Columns[4].Width = 125;//amount in
                listViewBuys.Columns[5].Width = 125;//amount remain
                listViewBuys.Columns[6].Width = 100;//value
                listViewBuys.Columns[7].Width = 100;//exchangerate
                listViewBuys.Columns[8].Width = 100;//paid
                listViewBuys.Columns[9].Width = 100;//remain
                listViewBuys.Columns[10].Width = 140;//قيمة الفاتور الفعلية
                listViewBuys.Columns[11].Width = 150;// المدفوع الفعلي
                listViewBuys.Columns[12].Width = 140;//قيمة  الخارج
                listViewBuys.Columns[13].Width = 140;//عائدات الفاتورة
                listViewBuys.Columns[14].Width = 140;//القيمة العلية للعائدات

            }
            else
            {
                listViewBuys.Columns[0].Width = 100;//--
                listViewBuys.Columns[1].Width = 120;//bills count
                listViewBuys.Columns[2].Width = 115;//clause count
                listViewBuys.Columns[3].Width = 125;//bill value
                listViewBuys.Columns[4].Width = 125;//bills pays value
                listViewBuys.Columns[5].Width = 120;//remain
                listViewBuys.Columns[6].Width = 140;//item in value
                listViewBuys.Columns[7].Width = 115;//item in real value
                listViewBuys.Columns[8].Width = 145;//real value
                listViewBuys.Columns[9].Width = 115;//profit
                listViewBuys.Columns[10].Width = 125;//real p
            }
        }
        private void CreateBillBuy_MenuItem_Click(object sender, EventArgs e)
        {
            BillBuyForm BillOUTForm_ = new BillBuyForm(DB, GetSelectedDate(), null);
            BillOUTForm_.ShowDialog();
            if (BillOUTForm_.Changed)
            {
                RefreshAccount();
            }
        }
        private void OpenBillBuy_MenuItem_Click(object sender, EventArgs e)
        {
            uint billbuyid = Convert.ToUInt32(listViewBuys.SelectedItems [0].Name );
            BillBuy BillBuy_ = new BillBuySQL(DB).GetBillBuy_INFO_BYID(billbuyid);
            BillBuyForm BillOUTForm_ = new BillBuyForm(DB, BillBuy_, false );
            BillOUTForm_.ShowDialog();
            if (BillOUTForm_.Changed)
            {
                RefreshAccount();
            }
            BillOUTForm_.Dispose();
        }
        private void EditBillBuy_MenuItem_Click(object sender, EventArgs e)
        {
            uint billbuyid = Convert.ToUInt32(listViewBuys.SelectedItems[0].Name);
            BillBuy BillBuy_ = new BillBuySQL(DB).GetBillBuy_INFO_BYID(billbuyid);
            BillBuyForm BillOUTForm_ = new BillBuyForm(DB, BillBuy_, true );
            BillOUTForm_.ShowDialog();
            if (BillOUTForm_.Changed)
            {
                RefreshAccount();
            }
            BillOUTForm_.Dispose();
        }
        private void DeleteBillBuy_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف؟","",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning );
            if (dd != DialogResult.OK) return;
            uint billbuyid = Convert.ToUInt32(listViewBuys.SelectedItems[0].Name);
            bool success = new BillBuySQL(DB).DeleteBillBuy(billbuyid);
            if (success)
            {
                RefreshAccount();
            }

        }
        private void AddPayOUT_BillBuy_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewBuys .SelectedItems.Count == 1)
            {
                uint sid = Convert.ToUInt32(listViewBuys.SelectedItems[0].Name);
                BillBuy BillBuy_ = new BillBuySQL(DB).GetBillBuy_INFO_BYID(sid);
                PayOUTForm PayOUTForm_ = new PayOUTForm(DB, MoneyAccount_.GetDate(), BillBuy_);
                PayOUTForm_.ShowDialog();
                if (PayOUTForm_.DialogResult == DialogResult.OK)
                {
                    Refresh_ListViewMoneyDataDetails();
                    Refresh_ListViewBuys();
                    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
                }
            }

        }
        #endregion
        #region ReportMaintenanceOPRs
        public async void MaintenanceOPRs_FillReport()
        {

            MaintenanceOPRsLabelAccountDate.Text = MaintenanceOPRsAccount_.GetAccountDateString();



            if (MaintenanceOPRsAccount_.Day != -1)
            {

                MaintenanceOPRsLabelAccountType.Text = "حساب اليوم";
                MaintenanceOPRsLabelReport.Text = "تقرير حساب اليوم : " + MaintenanceOPRsAccount_.GetAccountDateString();
                #region DaySection

                Report_MaintenanceOPRs_Month_ReportDetail Report_MaintenanceOPRs_DayReport
                    = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Day_Report(MaintenanceOPRsAccount_.Year, MaintenanceOPRsAccount_.Month, MaintenanceOPRsAccount_.Day);
                textBoxMaintenanceOPRs_Count.Text = Report_MaintenanceOPRs_DayReport.MaintenanceOPRs_Count .ToString();
                textBoxMaintenanceOPRs_EndWorkCount.Text = Report_MaintenanceOPRs_DayReport.MaintenanceOPRs_EndWork_Count.ToString();
                textBoxMaintenanceOPRs_RepairedCount.Text = Report_MaintenanceOPRs_DayReport.MaintenanceOPRs_Repaired_Count.ToString(); ;
                textBoxMaintenanceOPRs_EndWarrantyCount.Text = Report_MaintenanceOPRs_DayReport.MaintenanceOPRs_EndWarranty_Count.ToString(); ;
                textBoxMaintenanceOPRs_Warrantycount.Text = Report_MaintenanceOPRs_DayReport.MaintenanceOPRs_Warranty_Count.ToString(); ;

                textBoxBillMaintenanceOPRs_Value.Text = Report_MaintenanceOPRs_DayReport.BillMaintenances_Value ;
                textBoxBillMaintenanceOPRs__PaysValue.Text = Report_MaintenanceOPRs_DayReport.BillMaintenances_Pays_Value;
                textBoxBillMaintenanceOPRs__PaysRmain.Text = Report_MaintenanceOPRs_DayReport.BillMaintenances_Pays_Remain;
                textBoxBillMaintenanceOPRs__ItemsOutValue.Text = Report_MaintenanceOPRs_DayReport.BillMaintenances_ItemsOut_Value;
                textBoxBillMaintenanceOPRs_ItemsOutRealValue.Text = Report_MaintenanceOPRs_DayReport.BillMaintenances_ItemsOut_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBillMaintenanceOPRs__RealValue.Text = Report_MaintenanceOPRs_DayReport.BillMaintenances_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBillMaintenanceOPRs__RealPays.Text = Report_MaintenanceOPRs_DayReport.BillMaintenances_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

              
                #endregion
            }
            else if (MaintenanceOPRsAccount_.Month != -1)
            {
                MaintenanceOPRsLabelAccountType.Text = "حساب الشهر";
                MaintenanceOPRsLabelReport.Text = "تقرير حساب الشهر : " + MaintenanceOPRsAccount_.GetAccountDateString();

                #region MonthSection

                Report_MaintenanceOPRs_Year_ReportDetail Report_MaintenanceOPRs_MonthReport
                     = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Month_Report(MaintenanceOPRsAccount_.Year, MaintenanceOPRsAccount_.Month);
                textBoxMaintenanceOPRs_Count.Text = Report_MaintenanceOPRs_MonthReport.MaintenanceOPRs_Count.ToString();
                textBoxMaintenanceOPRs_EndWorkCount.Text = Report_MaintenanceOPRs_MonthReport.MaintenanceOPRs_EndWork_Count.ToString();
                textBoxMaintenanceOPRs_RepairedCount.Text = Report_MaintenanceOPRs_MonthReport.MaintenanceOPRs_Repaired_Count.ToString(); ;
                textBoxMaintenanceOPRs_EndWarrantyCount.Text = Report_MaintenanceOPRs_MonthReport.MaintenanceOPRs_EndWarranty_Count.ToString(); ;
                textBoxMaintenanceOPRs_Warrantycount.Text = Report_MaintenanceOPRs_MonthReport.MaintenanceOPRs_Warranty_Count.ToString(); ;

                textBoxBillMaintenanceOPRs_Value.Text = Report_MaintenanceOPRs_MonthReport.BillMaintenances_Value;
                textBoxBillMaintenanceOPRs__PaysValue.Text = Report_MaintenanceOPRs_MonthReport.BillMaintenances_Pays_Value;
                textBoxBillMaintenanceOPRs__PaysRmain.Text = Report_MaintenanceOPRs_MonthReport.BillMaintenances_Pays_Remain;
                textBoxBillMaintenanceOPRs__ItemsOutValue.Text = Report_MaintenanceOPRs_MonthReport.BillMaintenances_ItemsOut_Value;
                textBoxBillMaintenanceOPRs_ItemsOutRealValue.Text = Report_MaintenanceOPRs_MonthReport.BillMaintenances_ItemsOut_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBillMaintenanceOPRs__RealValue.Text = Report_MaintenanceOPRs_MonthReport.BillMaintenances_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBillMaintenanceOPRs__RealPays.Text = Report_MaintenanceOPRs_MonthReport.BillMaintenances_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else if (MaintenanceOPRsAccount_.Year != -1)
            {
                MaintenanceOPRsLabelAccountType.Text = "حساب السنة";
                MaintenanceOPRsLabelReport.Text = "تقرير حساب السنة : " + MaintenanceOPRsAccount_.GetAccountDateString();

                #region YearSection

                Report_MaintenanceOPRs_YearRange_ReportDetail Report_MaintenanceOPRs_YearReport
                   = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Year_Report(MaintenanceOPRsAccount_.Year);
                textBoxMaintenanceOPRs_Count.Text = Report_MaintenanceOPRs_YearReport.MaintenanceOPRs_Count.ToString();
                textBoxMaintenanceOPRs_EndWorkCount.Text = Report_MaintenanceOPRs_YearReport.MaintenanceOPRs_EndWork_Count.ToString();
                textBoxMaintenanceOPRs_RepairedCount.Text = Report_MaintenanceOPRs_YearReport.MaintenanceOPRs_Repaired_Count.ToString(); ;
                textBoxMaintenanceOPRs_EndWarrantyCount.Text = Report_MaintenanceOPRs_YearReport.MaintenanceOPRs_EndWarranty_Count.ToString(); ;
                textBoxMaintenanceOPRs_Warrantycount.Text = Report_MaintenanceOPRs_YearReport.MaintenanceOPRs_Warranty_Count.ToString(); ;

                textBoxBillMaintenanceOPRs_Value.Text = Report_MaintenanceOPRs_YearReport.BillMaintenances_Value;
                textBoxBillMaintenanceOPRs__PaysValue.Text = Report_MaintenanceOPRs_YearReport.BillMaintenances_Pays_Value;
                textBoxBillMaintenanceOPRs__PaysRmain.Text = Report_MaintenanceOPRs_YearReport.BillMaintenances_Pays_Remain;
                textBoxBillMaintenanceOPRs__ItemsOutValue.Text = Report_MaintenanceOPRs_YearReport.BillMaintenances_ItemsOut_Value;
                textBoxBillMaintenanceOPRs_ItemsOutRealValue.Text = Report_MaintenanceOPRs_YearReport.BillMaintenances_ItemsOut_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBillMaintenanceOPRs__RealValue.Text = Report_MaintenanceOPRs_YearReport.BillMaintenances_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxBillMaintenanceOPRs__RealPays.Text = Report_MaintenanceOPRs_YearReport.BillMaintenances_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else
            {
                MaintenanceOPRsLabelAccountType.Text = "حساب السنوات";
                MaintenanceOPRsLabelReport.Text = "تقرير حساب السنوات : " + MaintenanceOPRsAccount_.GetAccountDateString();

                #region YearRangeSection
                textBoxMaintenanceOPRs_Count.Text = "-";
                textBoxMaintenanceOPRs_EndWorkCount.Text = "-";
                textBoxMaintenanceOPRs_RepairedCount.Text = "-";
                textBoxMaintenanceOPRs_EndWarrantyCount.Text = "-";
                textBoxMaintenanceOPRs_Warrantycount.Text = "-";

                textBoxBillMaintenanceOPRs_Value.Text = "-";
                textBoxBillMaintenanceOPRs__PaysValue.Text = "-";
                textBoxBillMaintenanceOPRs__PaysRmain.Text = "-";
                textBoxBillMaintenanceOPRs__ItemsOutValue.Text = "-";
                textBoxBillMaintenanceOPRs_ItemsOutRealValue.Text = "-";
                textBoxBillMaintenanceOPRs__RealValue.Text = "-";
                textBoxBillMaintenanceOPRs__RealPays.Text = "-";
                #endregion
            }


        }
        public async void Refresh_ListViewMaintenanceOPRs()
        {

            listViewMaintenanceOPRs.Items.Clear();
            if (MaintenanceOPRsAccount_.Day != -1)
            {

                #region DaySection


                if (listViewMaintenanceOPRs.Name != "ListViewMaintenanceOPRs_Day")
                {
                    Report_MaintenanceOPRs_Day_ReportDetail.IntiliazeListView(ref listViewMaintenanceOPRs);

                }
                List<Report_MaintenanceOPRs_Day_ReportDetail> Report_MaintenanceOPRs_Day_ReportDetail_List
                          = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Day_ReportDetail(MaintenanceOPRsAccount_.Year, MaintenanceOPRsAccount_.Month, MaintenanceOPRsAccount_.Day);
                for (int i = 0; i < Report_MaintenanceOPRs_Day_ReportDetail_List.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_Time .ToShortTimeString());
                    item.Name = Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_ID.ToString();
                    item.UseItemStyleForSubItems = false;
                    item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_ID .ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_Owner );
                    item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i]._Item.ItemName);
                    item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i]._Item.ItemCompany );
                    item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i]._Item.folder .FolderName);
                    item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].FalutDesc);
                    for (int j = 0; j <=6; j++)
                        item.SubItems[j].BackColor = Color.LightYellow ;
                    if (Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_Endworkdate == null)
                    {
                        item.SubItems.Add("في الصيانة");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        for (int j = 7; j < 11; j++)
                            item.SubItems[j].BackColor = Color.Orange;

                    }
                    else
                    {
                    
                        item.SubItems.Add(
                            Convert.ToDateTime(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_Endworkdate).ToShortDateString());
                        item.SubItems[7].BackColor = Color.LimeGreen;
                        bool repaired = Convert.ToBoolean(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_Rpaired);
                        if (repaired)
                        {
                            item.SubItems.Add("تم الاصلاح");
                            item.SubItems[8].BackColor = Color.LimeGreen;
                        }
                        else
                        {
                            item.SubItems.Add("لم يتم الاصلاح");
                            item.SubItems[8].BackColor = Color.Orange ;
                        }
                        try
                        {
                            item.SubItems.Add(
                                                  Convert.ToDateTime(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_DeliverDate).ToShortDateString());
                            item.SubItems[9].BackColor = Color.LimeGreen ;
                        }
                        catch
                        {
                            item.SubItems.Add("-");
                            item.SubItems[9].BackColor = Color.Orange;
                        }
                        try
                        {
                            item.SubItems.Add(
                                                     Convert.ToDateTime(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_EndWarrantyDate).ToShortDateString());

                            if(Convert.ToDateTime(Report_MaintenanceOPRs_Day_ReportDetail_List[i].MaintenanceOPR_EndWarrantyDate)>DateTime.Now )
                            item.SubItems[10].BackColor = Color.LimeGreen;
                            else
                                item.SubItems[10].BackColor = Color.Orange  ;
                        }
                        catch
                        {
                            item.SubItems.Add("لا يوجد ضمان");
                            item.SubItems[10].BackColor = Color.Yellow ;
                        }
                        

                    }
                    
                    try
                    {

                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].BillValue.ToString() + " " + Report_MaintenanceOPRs_Day_ReportDetail_List[i]._Currency.CurrencySymbol);
                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].ExchangeRate.ToString());
                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].PaysAmount);

                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].PaysRemain.ToString() + " " + Report_MaintenanceOPRs_Day_ReportDetail_List[i]._Currency.CurrencySymbol);
                        if (Report_MaintenanceOPRs_Day_ReportDetail_List[i].PaysRemain == 0)
                            for (int j = 11; j <= 13; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 11; j <= 13; j++)
                                item.SubItems[j].BackColor = Color.LimeGreen;
                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].Bill_ItemsOut_Value.ToString());
                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].Bill_ItemsOut_RealValue .ToString() + " " + ReferenceCurrency.CurrencySymbol);
                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].Bill_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                        item.SubItems.Add(Report_MaintenanceOPRs_Day_ReportDetail_List[i].Bill_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                        if(Report_MaintenanceOPRs_Day_ReportDetail_List[i].Bill_Pays_RealValue> Report_MaintenanceOPRs_Day_ReportDetail_List[i].Bill_RealValue)
                            for (int j = 16; j <= 18; j++)
                                item.SubItems[j].BackColor = Color.LimeGreen;
                        else
                            for (int j = 16; j <= 18; j++)
                                item.SubItems[j].BackColor = Color.Orange ;
                    }
                    catch
                    {
                        
                        item.SubItems.Add("غير منشاة");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        item.SubItems.Add("-");
                        for (int j = 11; j <= 18; j++)
                            item.SubItems[j].BackColor = Color.Orange ;
                    }
                  
                   
                    listViewMaintenanceOPRs.Items.Add(item);


                }
                #endregion
            }
            else if (MaintenanceOPRsAccount_.Month != -1)
            {

                #region MonthSection
                if (listViewMaintenanceOPRs.Name != "ListViewMaintenanceOPRs_Month")
                {
                    Report_MaintenanceOPRs_Month_ReportDetail.IntiliazeListView(ref listViewMaintenanceOPRs);
                }
                List<Report_MaintenanceOPRs_Month_ReportDetail> Report_MaintenanceOPRs_Month_ReportDetailList
                                    = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Month_ReportDetail(MaintenanceOPRsAccount_.Year, MaintenanceOPRsAccount_.Month);
                for (int i = 0; i < Report_MaintenanceOPRs_Month_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_MaintenanceOPRs_Month_ReportDetailList[i].DayDate.ToShortDateString());
                    item.Name = Report_MaintenanceOPRs_Month_ReportDetailList[i].DayID.ToString();
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Count .ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_EndWork_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Repaired_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Warranty_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_EndWarranty_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Pays_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Pays_Remain);

                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_ItemsOut_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);


                     item.UseItemStyleForSubItems = false;
                    if(Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Count>0)
                    {
                        if (Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Count ==
                        Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_EndWork_Count)
                        {
                            for (int j = 1; j <= 2; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            for (int j = 1; j <= 2; j++)
                                item.SubItems[j].BackColor = Color.Orange;

                        }
                        if (Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Count ==
                         Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Repaired_Count)
                        {
                            item.SubItems[3].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            item.SubItems[3].BackColor = Color.Orange;

                        }
                        if(Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Warranty_Count>0)
                        {
                            if (Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_Warranty_Count ==
                        Report_MaintenanceOPRs_Month_ReportDetailList[i].MaintenanceOPRs_EndWarranty_Count)
                            {
                                item.SubItems[4].BackColor = Color.LightGreen;
                                item.SubItems[5].BackColor = Color.LightGreen;
                            }
                            else
                            {
                                item.SubItems[4].BackColor = Color.Orange;
                                item.SubItems[5].BackColor = Color.Orange;

                            }
                        }
                        else
                        {
                            item.SubItems[4].BackColor = Color.LightYellow ;
                            item.SubItems[5].BackColor = Color.LightYellow;
                        }
                        
                    }
                    else
                        for (int j = 1; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.LightYellow ;
                    if (Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_RealValue > 0 ||
                        Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Pays_RealValue > 0)
                    {
                        if (Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency == 0)
                            for (int j =6; j <= 8; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else 
                            for (int j = 6; j <=8; j++)
                                item.SubItems[j].BackColor = Color.Orange ;
                    }
                    else
                    {
                        for (int j = 6; j <= 8; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    }
                    if (Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_RealValue>0
                        || Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Pays_RealValue>0)
                    {
                        if (Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_Pays_RealValue >
                           Report_MaintenanceOPRs_Month_ReportDetailList[i].BillMaintenances_RealValue)
                            for (int j = 9; j <= 12; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else
                            for (int j = 9; j <= 12; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                    }
                    else
                        for (int j = 9; j <= 12; j++)
                            item.SubItems[j].BackColor = Color.LightYellow ;


                    listViewMaintenanceOPRs.Items.Add(item);

                }
                #endregion
            }
            else if (MaintenanceOPRsAccount_.Year != -1)
            {

                #region YearSection
                if (listViewMaintenanceOPRs.Name != "ListViewMaintenanceOPRs_Year")
                {

                    Report_MaintenanceOPRs_Year_ReportDetail.IntiliazeListView(ref listViewMaintenanceOPRs);

                }

                List<Report_MaintenanceOPRs_Year_ReportDetail> Report_MaintenanceOPRs_Year_ReportDetailList
                           = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Year_ReportDetail(MaintenanceOPRsAccount_.Year);
                for (int i = 0; i < Report_MaintenanceOPRs_Year_ReportDetailList.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_MaintenanceOPRs_Year_ReportDetailList[i].MonthName);
                    item.Name = Report_MaintenanceOPRs_Year_ReportDetailList[i].MonthNO.ToString();
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_EndWork_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Repaired_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Warranty_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_EndWarranty_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Pays_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Pays_Remain);

                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_ItemsOut_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);


                    item.UseItemStyleForSubItems = false;
                    if (Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Count > 0)
                    {
                        if (Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Count ==
                        Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_EndWork_Count)
                        {
                            for (int j = 1; j <= 2; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            for (int j = 1; j <= 2; j++)
                                item.SubItems[j].BackColor = Color.Orange;

                        }
                        if (Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Count ==
                         Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Repaired_Count)
                        {
                            item.SubItems[3].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            item.SubItems[3].BackColor = Color.Orange;

                        }
                        if (Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Warranty_Count > 0)
                        {
                            if (Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_Warranty_Count ==
                        Report_MaintenanceOPRs_Year_ReportDetailList[i].MaintenanceOPRs_EndWarranty_Count)
                            {
                                item.SubItems[4].BackColor = Color.LightGreen;
                                item.SubItems[5].BackColor = Color.LightGreen;
                            }
                            else
                            {
                                item.SubItems[4].BackColor = Color.Orange;
                                item.SubItems[5].BackColor = Color.Orange;

                            }
                        }
                        else
                        {
                            item.SubItems[4].BackColor = Color.LightYellow;
                            item.SubItems[5].BackColor = Color.LightYellow;
                        }

                    }
                    else
                        for (int j = 1; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    if (Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_RealValue > 0 ||
                        Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Pays_RealValue > 0)
                    {
                        if (Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency == 0)
                            for (int j = 6; j <= 8; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else
                            for (int j = 6; j <= 8; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                    }
                    else
                    {
                        for (int j = 6; j <= 8; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    }
                    if (Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_RealValue > 0
                        || Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Pays_RealValue > 0)
                    {
                        if (Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_Pays_RealValue >
                           Report_MaintenanceOPRs_Year_ReportDetailList[i].BillMaintenances_RealValue)
                            for (int j = 9; j <= 12; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else
                            for (int j = 9; j <= 12; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                    }
                    else
                        for (int j = 9; j <= 12; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;


                    listViewMaintenanceOPRs.Items.Add(item);

                }
                #endregion
            }
            else
            {
                //MaintenanceOPRsLabelAccountType.Text = "حساب السنوات";
                //MaintenanceOPRsLabelReport.Text = "تقرير حساب السنوات : " + MaintenanceOPRsAccount_.GetAccountDateString();

                #region YearRangeSection
                if (listViewMaintenanceOPRs.Name != "ListViewMaintenanceOPRs_YearRange")
                {
                    Report_MaintenanceOPRs_YearRange_ReportDetail.IntiliazeListView(ref listViewMaintenanceOPRs);
                }
                List<Report_MaintenanceOPRs_YearRange_ReportDetail> Report_MaintenanceOPRs_YearRange_ReportDetailList
                           = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_YearRange_ReportDetail(MaintenanceOPRsAccount_.YearRange_.min_year, MaintenanceOPRsAccount_.YearRange_.max_year);

                for (int i = 0; i < Report_MaintenanceOPRs_YearRange_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].YearNO.ToString());
                    item.Name = Report_MaintenanceOPRs_YearRange_ReportDetailList[i].YearNO.ToString();
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_EndWork_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Repaired_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Warranty_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_EndWarranty_Count.ToString());
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Pays_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Pays_Remain);

                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_ItemsOut_Value);
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_ItemsOut_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);


                    item.UseItemStyleForSubItems = false;
                    if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Count > 0)
                    {
                        if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Count ==
                        Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_EndWork_Count)
                        {
                            for (int j = 1; j <= 2; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            for (int j = 1; j <= 2; j++)
                                item.SubItems[j].BackColor = Color.Orange;

                        }
                        if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Count ==
                         Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Repaired_Count)
                        {
                            item.SubItems[3].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            item.SubItems[3].BackColor = Color.Orange;

                        }
                        if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Warranty_Count > 0)
                        {
                            if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_Warranty_Count ==
                        Report_MaintenanceOPRs_YearRange_ReportDetailList[i].MaintenanceOPRs_EndWarranty_Count)
                            {
                                item.SubItems[4].BackColor = Color.LightGreen;
                                item.SubItems[5].BackColor = Color.LightGreen;
                            }
                            else
                            {
                                item.SubItems[4].BackColor = Color.Orange;
                                item.SubItems[5].BackColor = Color.Orange;

                            }
                        }
                        else
                        {
                            item.SubItems[4].BackColor = Color.LightYellow;
                            item.SubItems[5].BackColor = Color.LightYellow;
                        }

                    }
                    else
                        for (int j = 1; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_RealValue > 0 ||
                        Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Pays_RealValue > 0)
                    {
                        if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency == 0)
                            for (int j = 6; j <= 8; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else
                            for (int j = 6; j <= 8; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                    }
                    else
                    {
                        for (int j = 6; j <= 8; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    }
                    if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_RealValue > 0
                        || Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Pays_RealValue > 0)
                    {
                        if (Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_Pays_RealValue >
                           Report_MaintenanceOPRs_YearRange_ReportDetailList[i].BillMaintenances_RealValue)
                            for (int j = 9; j <= 12; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else
                            for (int j = 9; j <= 12; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                    }
                    else
                        for (int j = 9; j <= 12; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;


                    listViewMaintenanceOPRs.Items.Add(item);

                }

                #endregion
            }

            MaintenanceOPRs_FillReport();


        }
        private void MaintenanceOPRsBack_Click(object sender, EventArgs e)
        {
            if (MaintenanceOPRsAccount_.Year == -1) return;
            MaintenanceOPRsAccount_.Account_Date_UP();
            Refresh_ListViewMaintenanceOPRs();
        }

        private void MaintenanceOPRsButtonLeftRight_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool left;
            if (b.Name == "MaintenanceOPRsButtonLeft") left = true;
            else left = false;

            if (MaintenanceOPRsAccount_.Day != -1)
            {
                if (left)
                {
                    if (MaintenanceOPRsAccount_.Day == DateTime.DaysInMonth(MaintenanceOPRsAccount_.Year, MaintenanceOPRsAccount_.Month))
                    {
                        if (MaintenanceOPRsAccount_.Month == 12)
                        { MaintenanceOPRsAccount_.Year++; MaintenanceOPRsAccount_.Month = 1; MaintenanceOPRsAccount_.Day = 1; }
                        else
                        { MaintenanceOPRsAccount_.Month++; MaintenanceOPRsAccount_.Day = 1; }

                    }
                    else MaintenanceOPRsAccount_.Day++;
                }
                else
                {
                    if (MaintenanceOPRsAccount_.Day == 1)
                    {

                        if (MaintenanceOPRsAccount_.Month == 1)
                        { MaintenanceOPRsAccount_.Year--; MaintenanceOPRsAccount_.Month = 12; }
                        else
                        { MaintenanceOPRsAccount_.Month--; }
                        MaintenanceOPRsAccount_.Day = DateTime.DaysInMonth(MaintenanceOPRsAccount_.Year, MaintenanceOPRsAccount_.Month);
                    }
                    else MaintenanceOPRsAccount_.Day--;
                }

            }
            else if (MaintenanceOPRsAccount_.Month != -1)
            {
                if (left)
                {
                    if (MaintenanceOPRsAccount_.Month == 12)
                    {
                        MaintenanceOPRsAccount_.Year++; MaintenanceOPRsAccount_.Month = 1;
                    }
                    else MaintenanceOPRsAccount_.Month++;
                }
                else
                {
                    if (MaintenanceOPRsAccount_.Month == 1)
                    {
                        MaintenanceOPRsAccount_.Year--; MaintenanceOPRsAccount_.Month = 12;
                    }
                    else MaintenanceOPRsAccount_.Month--;
                }
            }
            else if (MaintenanceOPRsAccount_.Year != -1)
            {
                if (left)
                {
                    MaintenanceOPRsAccount_.Year++;
                    MaintenanceOPRsAccount_.YearRange_.min_year++;
                    MaintenanceOPRsAccount_.YearRange_.max_year++;
                }
                else
                {
                    MaintenanceOPRsAccount_.Year--;
                    MaintenanceOPRsAccount_.YearRange_.min_year--;
                    MaintenanceOPRsAccount_.YearRange_.max_year--;
                }
            }
            else
            {
                if (left)
                {

                    MaintenanceOPRsAccount_.YearRange_.min_year += 10;
                    MaintenanceOPRsAccount_.YearRange_.max_year += 10;
                }
                else
                {
                    MaintenanceOPRsAccount_.YearRange_.min_year -= 10;
                    MaintenanceOPRsAccount_.YearRange_.max_year -= 10;
                }
            }
            Refresh_ListViewMaintenanceOPRs();
        }

        public void ListViewMaintenanceOPRsAccountDown()
        {
            try
            {
                if (MaintenanceOPRsAccount_.Year == -1 || MaintenanceOPRsAccount_.Month == -1 || MaintenanceOPRsAccount_.Day == -1)
                {
                    MaintenanceOPRsAccount_.Account_Date_Down(Convert.ToInt32(listViewMaintenanceOPRs.SelectedItems[0].Name));
                    Refresh_ListViewMaintenanceOPRs();
                }
                else
                {

                    OpenMaintenanceOPR_MenuItem.PerformClick();

                }


            }
            catch (Exception ee)
            {

            }


        }
        private void ListViewMaintenanceOPRs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewMaintenanceOPRs.SelectedItems.Count > 0)
                ListViewMaintenanceOPRsAccountDown();
        }
        private void ListViewMaintenanceOPRs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                ListViewMaintenanceOPRsAccountDown();
        }
        private void ListViewMaintenanceOPRs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listViewMaintenanceOPRs.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    foreach (ListViewItem item1 in listViewMaintenanceOPRs.Items)
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
                        if (MaintenanceOPRsAccount_.Day != -1)
                        {
                            MaintenanceOPR MaintenanceOPR_ =new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID ( Convert.ToUInt32(listitem.Name ));
                            BillMaintenance BillMaintenance_ = new BillMaintenanceSQL(DB).GetBillMaintenance_By_MaintenaceOPR(MaintenanceOPR_);

                            List<MenuItem> MenuItemList = new List<MenuItem>();
                            MenuItemList.Add(Refresh_MenuItem);
                            MenuItemList.Add(new MenuItem("-"));
                            MenuItemList.AddRange(new MenuItem[] {OpenMaintenanceOPR_MenuItem , EditMaintenanceOPR_MenuItem , DeleteMaintenanceOPR_MenuItem
                            , new MenuItem("-"),CreateMaintenanceOPR_MenuItem });
                            if(BillMaintenance_ !=null )
                                MenuItemList.AddRange(new MenuItem[] { new MenuItem("-"), AddPayIN_BillMaintenance_MenuItem });
                            listViewMaintenanceOPRs.ContextMenu = new ContextMenu(MenuItemList.ToArray());


                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewMaintenanceOPRs.ContextMenu = new ContextMenu(mi1);


                        }


                    }
                    else
                    {
                        if (MaintenanceOPRsAccount_.Day != -1)
                        {
                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem, new MenuItem("-"), CreateMaintenanceOPR_MenuItem };
                            listViewMaintenanceOPRs.ContextMenu = new ContextMenu(mi1);
                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewMaintenanceOPRs.ContextMenu = new ContextMenu(mi1);


                        }

                    }

                }
            }
        }

        public async void IntializeListViewMaintenanceOPRsColumnsWidth()
        {

            if (MaintenanceOPRsAccount_.Day != -1)
            {


                listViewMaintenanceOPRs.Columns[0].Width = 75;//time
                listViewMaintenanceOPRs.Columns[1].Width = 60;//id
                listViewMaintenanceOPRs.Columns[2].Width = 100;//owner
                listViewMaintenanceOPRs.Columns[3].Width = 60;//clause count
                listViewMaintenanceOPRs.Columns[4].Width = 125;//amount in
                listViewMaintenanceOPRs.Columns[5].Width = 125;//amount remain
                listViewMaintenanceOPRs.Columns[6].Width = 100;//value
                listViewMaintenanceOPRs.Columns[7].Width = 100;//exchangerate
                listViewMaintenanceOPRs.Columns[8].Width = 100;//paid
                listViewMaintenanceOPRs.Columns[9].Width = 100;//remain
                listViewMaintenanceOPRs.Columns[10].Width = 140;//قيمة الفاتور الفعلية
                listViewMaintenanceOPRs.Columns[11].Width = 150;// المدفوع الفعلي
                listViewMaintenanceOPRs.Columns[12].Width = 140;//قيمة  الخارج
                listViewMaintenanceOPRs.Columns[13].Width = 140;//عائدات الفاتورة
                listViewMaintenanceOPRs.Columns[14].Width = 140;//القيمة العلية للعائدات

            }
            else
            {
                listViewMaintenanceOPRs.Columns[0].Width = 100;//--
                listViewMaintenanceOPRs.Columns[1].Width = 120;//bills count
                listViewMaintenanceOPRs.Columns[2].Width = 115;//clause count
                listViewMaintenanceOPRs.Columns[3].Width = 125;//bill value
                listViewMaintenanceOPRs.Columns[4].Width = 125;//bills pays value
                listViewMaintenanceOPRs.Columns[5].Width = 120;//remain
                listViewMaintenanceOPRs.Columns[6].Width = 140;//item in value
                listViewMaintenanceOPRs.Columns[7].Width = 115;//item in real value
                listViewMaintenanceOPRs.Columns[8].Width = 145;//real value
                listViewMaintenanceOPRs.Columns[9].Width = 115;//profit
                listViewMaintenanceOPRs.Columns[10].Width = 125;//real p
            }
        }
        private void CreateMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB,null);
            MaintenanceOPRForm_.ShowDialog();
            if (MaintenanceOPRForm_.Changed)
            {
                Refresh_ListViewMaintenanceOPRs();
            }
        }
        private void OpenMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            uint MaintenanceOPRid = Convert.ToUInt32(listViewMaintenanceOPRs.SelectedItems[0].Name);
            MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(MaintenanceOPRid);
            Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB, MaintenanceOPR_, false);
            MaintenanceOPRForm_.ShowDialog();
            if (MaintenanceOPRForm_.Changed)
            {
                Refresh_ListViewMaintenanceOPRs();
            }
            MaintenanceOPRForm_.Dispose();
        }
        private void EditMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            uint MaintenanceOPRid = Convert.ToUInt32(listViewMaintenanceOPRs.SelectedItems[0].Name);
            MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(MaintenanceOPRid);
            Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB, MaintenanceOPR_, true );
            MaintenanceOPRForm_.ShowDialog();
            if (MaintenanceOPRForm_.Changed)
            {
                Refresh_ListViewMaintenanceOPRs();
            }
            MaintenanceOPRForm_.Dispose();
        }
        private void DeleteMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd != DialogResult.OK) return;
            uint MaintenanceOPRid = Convert.ToUInt32(listViewMaintenanceOPRs.SelectedItems[0].Name);
            bool success = new MaintenanceOPRSQL(DB).DeleteMaintenanceOPR (MaintenanceOPRid);
            if (success)
            {
                Refresh_ListViewMaintenanceOPRs();
            }

        }
        private void AddPayIN_BillMaintenance_MenuItem_Click(object sender, EventArgs e)
        {

            if (listViewMaintenanceOPRs.SelectedItems.Count == 1)
            {
                uint sid = Convert.ToUInt32(listViewMaintenanceOPRs .SelectedItems[0].Name);

                MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(sid);
                BillMaintenance BillMaintenance_ = new BillMaintenanceSQL(DB).GetBillMaintenance_By_MaintenaceOPR(MaintenanceOPR_);
                PayINForm PayINForm_ = new PayINForm(DB, MoneyAccount_.GetDate(), BillMaintenance_);
                PayINForm_.ShowDialog();
                if (PayINForm_.DialogResult == DialogResult.OK)
                {
                    Refresh_ListViewMaintenanceOPRs();
                    Refresh_ListViewMoneyDataDetails();
                    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
                }
            }


          


        }
       
        #endregion
        #region ReportSells
        public async  void Sells_FillReport()
        {

            SellsLabelAccountDate.Text = SellsAccount_.GetAccountDateString();



            if (SellsAccount_.Day != -1)
            {

                SellsLabelAccountType.Text = "حساب اليوم";
                SellsLabelReport.Text = "تقرير حساب اليوم : " + SellsAccount_.GetAccountDateString();
                #region DaySection
               
                Report_Sells_Month_ReportDetail Report_Sells_DayReport
                    = new ReportSellsSQL(DB).Get_Report_Sells_Day_Report(SellsAccount_.Year, SellsAccount_.Month, SellsAccount_.Day);
                textBoxSells_ItemsINValue.Text = Report_Sells_DayReport.Bills_ItemsIN_Value;
                textBoxSells_Value.Text = Report_Sells_DayReport.Bills_Value;
                textBoxSellsPaysValue.Text = Report_Sells_DayReport.Bills_Pays_Value;
                textBoxSellsPaysRmain.Text = Report_Sells_DayReport.Bills_Pays_Remain;
                textBoxSells_ItemsIn_RealValue.Text = Report_Sells_DayReport.Bills_ItemsIN_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSellRealValue.Text = Report_Sells_DayReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSells_RealProfit.Text =System.Math.Round ( (Report_Sells_DayReport.Bills_RealValue - Report_Sells_DayReport.Bills_ItemsIN_RealValue),2).ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSells_RealPays.Text = Report_Sells_DayReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else if (SellsAccount_.Month != -1)
            {
                SellsLabelAccountType.Text = "حساب الشهر";
                SellsLabelReport.Text = "تقرير حساب الشهر : " + SellsAccount_.GetAccountDateString();

                #region MonthSection
              
                Report_Sells_Year_ReportDetail Report_Sells_MonthReport
                     = new ReportSellsSQL(DB).Get_Report_Sells_Month_Report(SellsAccount_.Year, SellsAccount_.Month);
                textBoxSells_ItemsINValue.Text = Report_Sells_MonthReport.Bills_ItemsIN_Value;
                textBoxSells_Value.Text = Report_Sells_MonthReport.Bills_Value;
                textBoxSellsPaysValue.Text = Report_Sells_MonthReport.Bills_Pays_Value;
                textBoxSellsPaysRmain.Text = Report_Sells_MonthReport.Bills_Pays_Remain;
                textBoxSells_ItemsIn_RealValue.Text = Report_Sells_MonthReport.Bills_ItemsIN_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSellRealValue.Text = Report_Sells_MonthReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSells_RealProfit.Text = System.Math.Round((Report_Sells_MonthReport.Bills_RealValue - Report_Sells_MonthReport.Bills_ItemsIN_RealValue), 2).ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSells_RealPays.Text = Report_Sells_MonthReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else if (SellsAccount_.Year != -1)
            {
                SellsLabelAccountType.Text = "حساب السنة";
                SellsLabelReport.Text = "تقرير حساب السنة : " + SellsAccount_.GetAccountDateString();

                #region YearSection

                Report_Sells_YearRange_ReportDetail Report_Sells_YearReport
                   = new ReportSellsSQL(DB).Get_Report_Sells_Year_Report(SellsAccount_.Year);
                textBoxSells_ItemsINValue.Text = Report_Sells_YearReport.Bills_ItemsIN_Value;
                textBoxSells_Value.Text = Report_Sells_YearReport.Bills_Value;
                textBoxSellsPaysValue.Text = Report_Sells_YearReport.Bills_Pays_Value;
                textBoxSellsPaysRmain.Text = Report_Sells_YearReport.Bills_Pays_Remain;
                textBoxSells_ItemsIn_RealValue.Text = Report_Sells_YearReport.Bills_ItemsIN_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSellRealValue.Text = Report_Sells_YearReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSells_RealProfit.Text = System.Math.Round((Report_Sells_YearReport.Bills_RealValue - Report_Sells_YearReport.Bills_ItemsIN_RealValue), 2).ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxSells_RealPays.Text = Report_Sells_YearReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else
            {
                SellsLabelAccountType.Text = "حساب السنوات";
                SellsLabelReport.Text = "تقرير حساب السنوات : " + SellsAccount_.GetAccountDateString();

                #region YearRangeSection
               
                textBoxSells_ItemsINValue.Text = "-";
                textBoxSells_Value.Text = "-";
                textBoxSellsPaysValue.Text = "-";
                textBoxSellsPaysRmain.Text = "-";
                textBoxSells_ItemsIn_RealValue.Text = "-";
                textBoxSellRealValue.Text = "-";
                textBoxSells_RealProfit.Text = "-";
                textBoxSells_RealPays.Text = "-";
                #endregion
            }


        }
        public async void Refresh_ListViewSells()
        {

            listViewSells.Items.Clear();
            if (SellsAccount_.Day != -1)
            {

                #region DaySection


                if (listViewSells.Name != "ListViewSells_Day")
                {
                    Report_Sells_Day_ReportDetail.IntiliazeListView(ref listViewSells);

                }
                List<Report_Sells_Day_ReportDetail> Report_Sells_Day_ReportDetail_List
                          = new ReportSellsSQL(DB).Get_Report_Sells_Day_ReportDetail(SellsAccount_.Year, SellsAccount_.Month, SellsAccount_.Day);

                for (int i = 0; i < Report_Sells_Day_ReportDetail_List.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_Sells_Day_ReportDetail_List[i].Bill_Time.ToShortTimeString());
                    item.Name = Report_Sells_Day_ReportDetail_List[i].Bill_ID.ToString();
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].Bill_ID.ToString());
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].SellType);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].Bill_Owner);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].ClauseS_Count.ToString());
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].ItemsOutValue.ToString() + " " + Report_Sells_Day_ReportDetail_List[i]._Currency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].ExchangeRate.ToString());
                    //item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].PaysCount.ToString());
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].PaysAmount);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].PaysRemain.ToString() + " " + Report_Sells_Day_ReportDetail_List[i]._Currency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].Source_ItemsIN_Cost_Details);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].Source_ItemsIN_RealCost.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].ItemsOut_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add((Report_Sells_Day_ReportDetail_List[i].ItemsOut_RealValue
                        - Report_Sells_Day_ReportDetail_List[i].Source_ItemsIN_RealCost) + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Day_ReportDetail_List[i].RealPaysValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                    item.UseItemStyleForSubItems = false;
                    if(Report_Sells_Day_ReportDetail_List[i].ItemsOutValue> 0 
                        || Report_Sells_Day_ReportDetail_List[i].RealPaysValue  > 0)
                    {
                        if (Report_Sells_Day_ReportDetail_List[i].PaysRemain != 0)
                            for (int j = 4; j <= 8; j++)
                                item.SubItems[j].BackColor = Color.Orange;
                        else
                            for (int j = 4; j <= 8; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                    }
                   else
                    {
                        for (int j = 4; j <= 8; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    }

                    if (Report_Sells_Day_ReportDetail_List[i].Source_ItemsIN_RealCost
                        >Report_Sells_Day_ReportDetail_List[i].ItemsOut_RealValue)
                        for (int j = 9; j <= 12; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Sells_Day_ReportDetail_List[i].Source_ItemsIN_RealCost
                       <Report_Sells_Day_ReportDetail_List[i].ItemsOut_RealValue)
                        for (int j = 9; j <= 12; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 9; j <= 12; j++)
                            item.SubItems[j].BackColor = Color.LightYellow ;

                    if (Report_Sells_Day_ReportDetail_List[i].Source_ItemsIN_RealCost
                        > Report_Sells_Day_ReportDetail_List[i].RealPaysValue )
                            item.SubItems[13].BackColor = Color.Orange;
                    else if (Report_Sells_Day_ReportDetail_List[i].Source_ItemsIN_RealCost
                       < Report_Sells_Day_ReportDetail_List[i].RealPaysValue )
                            item.SubItems[13].BackColor = Color.LightGreen;
                    else
                            item.SubItems[13].BackColor = Color.LightYellow;
                    listViewSells.Items.Add(item);


                }
                #endregion
            }
            else if (SellsAccount_.Month != -1)
            {

                #region MonthSection
                if (listViewSells.Name != "ListViewSells_Month")
                {
                    Report_Sells_Month_ReportDetail.IntiliazeListView(ref listViewSells);
                }
                List<Report_Sells_Month_ReportDetail> Report_Sells_Month_ReportDetailList
                                    = new ReportSellsSQL(DB).Get_Report_Sells_Month_ReportDetail(SellsAccount_.Year, SellsAccount_.Month);
                for (int i = 0; i < Report_Sells_Month_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_Sells_Month_ReportDetailList[i].DayDate.ToShortDateString());
                    item.Name = Report_Sells_Month_ReportDetailList[i].DayID.ToString();
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_Count.ToString());
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_Clause_Count.ToString());
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_Value.ToString());
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_Pays_Value);
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_Pays_Remain);
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_ItemsIN_Value);
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_ItemsIN_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add((Report_Sells_Month_ReportDetailList[i].Bills_RealValue - Report_Sells_Month_ReportDetailList[i].Bills_ItemsIN_RealValue) + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Month_ReportDetailList[i].Bills_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);

                    item.UseItemStyleForSubItems = false;
                    if (Report_Sells_Month_ReportDetailList[i].Bills_RealValue > 0
                        || Report_Sells_Month_ReportDetailList[i].Bills_Pays_RealValue > 0)
                    {
                        if (Report_Sells_Month_ReportDetailList[i].Bills_Pays_Remain_UPON_BillsCurrency == 0
                            )
                            for (int j = 3; j <= 5; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else 
                            for (int j = 3; j <= 5; j++)
                                item.SubItems[j].BackColor = Color.Orange;

                         
                    }
                    else
                    {
                        for (int j =3; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.LightYellow ;
                    }



                    if (Report_Sells_Month_ReportDetailList[i].Bills_ItemsIN_RealValue
                         > Report_Sells_Month_ReportDetailList[i].Bills_RealValue)
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Sells_Month_ReportDetailList[i].Bills_ItemsIN_RealValue
                       < Report_Sells_Month_ReportDetailList[i].Bills_RealValue)
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    if (Report_Sells_Month_ReportDetailList[i].Bills_ItemsIN_RealValue
                        > Report_Sells_Month_ReportDetailList[i].Bills_Pays_RealValue)
                        item.SubItems[10].BackColor = Color.Orange;
                    else if (Report_Sells_Month_ReportDetailList[i].Bills_ItemsIN_RealValue
                       < Report_Sells_Month_ReportDetailList[i].Bills_Pays_RealValue)
                        item.SubItems[10].BackColor = Color.LightGreen;
                    else
                        item.SubItems[10].BackColor = Color.LightYellow;

                    listViewSells.Items.Add(item);

                }
                #endregion
            }
            else if (SellsAccount_.Year != -1)
            {

                #region YearSection
                if (listViewSells.Name != "ListViewSells_Year")
                {

                    Report_Sells_Year_ReportDetail.IntiliazeListView(ref listViewSells);

                }

                List<Report_Sells_Year_ReportDetail> Report_Sells_Year_ReportDetailList
                           = new ReportSellsSQL(DB).Get_Report_Sells_Year_ReportDetail(SellsAccount_.Year);
                for (int i = 0; i < Report_Sells_Year_ReportDetailList.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_Sells_Year_ReportDetailList[i].MonthName);
                    item.Name = Report_Sells_Year_ReportDetailList[i].MonthNO.ToString();
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_Count.ToString());
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_Clause_Count.ToString());
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_Value.ToString());
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_Pays_Value);
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_Pays_Remain);
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_ItemsIN_Value);
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_ItemsIN_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add((Report_Sells_Year_ReportDetailList[i].Bills_RealValue - Report_Sells_Year_ReportDetailList[i].Bills_ItemsIN_RealValue) + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_Year_ReportDetailList[i].Bills_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);

                    item.UseItemStyleForSubItems = false;
                    if (Report_Sells_Year_ReportDetailList[i].Bills_RealValue > 0
                        || Report_Sells_Year_ReportDetailList[i].Bills_Pays_RealValue > 0)
                    {
                        if (Report_Sells_Year_ReportDetailList[i].Bills_Pays_Remain_UPON_BillsCurrency == 0)
                            for (int j = 3; j <= 5; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else
                            for (int j = 3; j <= 5; j++)
                                item.SubItems[j].BackColor = Color.Orange;


                    }
                    else
                    {
                        for (int j = 3; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    }



                    if (Report_Sells_Year_ReportDetailList[i].Bills_ItemsIN_RealValue
                         > Report_Sells_Year_ReportDetailList[i].Bills_RealValue)
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Sells_Year_ReportDetailList[i].Bills_ItemsIN_RealValue
                       < Report_Sells_Year_ReportDetailList[i].Bills_RealValue)
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    if (Report_Sells_Year_ReportDetailList[i].Bills_ItemsIN_RealValue
                        > Report_Sells_Year_ReportDetailList[i].Bills_Pays_RealValue)
                        item.SubItems[10].BackColor = Color.Orange;
                    else if (Report_Sells_Year_ReportDetailList[i].Bills_ItemsIN_RealValue
                       < Report_Sells_Year_ReportDetailList[i].Bills_Pays_RealValue)
                        item.SubItems[10].BackColor = Color.LightGreen;
                    else
                        item.SubItems[10].BackColor = Color.LightYellow;


                    listViewSells.Items.Add(item);

                }
                #endregion
            }
            else
            {
                //SellsLabelAccountType.Text = "حساب السنوات";
                //SellsLabelReport.Text = "تقرير حساب السنوات : " + SellsAccount_.GetAccountDateString();

                #region YearRangeSection
                if (listViewSells.Name != "ListViewSells_YearRange")
                {
                    Report_Sells_YearRange_ReportDetail.IntiliazeListView(ref listViewSells);
                }
                List<Report_Sells_YearRange_ReportDetail> Report_Sells_YearRange_ReportDetailList
                           = new ReportSellsSQL(DB).Get_Report_Sells_YearRange_ReportDetail(SellsAccount_.YearRange_.min_year, SellsAccount_.YearRange_.max_year);

                for (int i = 0; i < Report_Sells_YearRange_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_Sells_YearRange_ReportDetailList[i].YearNO.ToString());
                    item.Name = Report_Sells_YearRange_ReportDetailList[i].YearNO.ToString();
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_Count.ToString());
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_Clause_Count.ToString());
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_Value.ToString());
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_Pays_Value);
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_Pays_Remain);
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_ItemsIN_Value);
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_ItemsIN_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(System.Math.Round((Report_Sells_YearRange_ReportDetailList[i].Bills_RealValue - Report_Sells_YearRange_ReportDetailList[i].Bills_ItemsIN_RealValue), 2) + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_Sells_YearRange_ReportDetailList[i].Bills_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);

                    item.UseItemStyleForSubItems = false;
                    if (Report_Sells_YearRange_ReportDetailList[i].Bills_RealValue > 0
                        || Report_Sells_YearRange_ReportDetailList[i].Bills_Pays_RealValue > 0)
                    {
                        if (Report_Sells_YearRange_ReportDetailList[i].Bills_Pays_Remain_UPON_BillsCurrency == 0)
                            for (int j = 3; j <= 5; j++)
                                item.SubItems[j].BackColor = Color.LightGreen;
                        else
                            for (int j = 3; j <= 5; j++)
                                item.SubItems[j].BackColor = Color.Orange;


                    }
                    else
                    {
                        for (int j = 3; j <= 5; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;
                    }



                    if (Report_Sells_YearRange_ReportDetailList[i].Bills_ItemsIN_RealValue
                         > Report_Sells_YearRange_ReportDetailList[i].Bills_RealValue)
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.Orange;
                    else if (Report_Sells_YearRange_ReportDetailList[i].Bills_ItemsIN_RealValue
                       < Report_Sells_YearRange_ReportDetailList[i].Bills_RealValue)
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.LightGreen;
                    else
                        for (int j = 6; j <= 9; j++)
                            item.SubItems[j].BackColor = Color.LightYellow;

                    if (Report_Sells_YearRange_ReportDetailList[i].Bills_ItemsIN_RealValue
                        > Report_Sells_YearRange_ReportDetailList[i].Bills_Pays_RealValue)
                        item.SubItems[10].BackColor = Color.Orange;
                    else if (Report_Sells_YearRange_ReportDetailList[i].Bills_ItemsIN_RealValue
                       < Report_Sells_YearRange_ReportDetailList[i].Bills_Pays_RealValue)
                        item.SubItems[10].BackColor = Color.LightGreen;
                    else
                        item.SubItems[10].BackColor = Color.LightYellow;


                    listViewSells.Items.Add(item);

                }

                #endregion
            }

            Sells_FillReport();


        }
        private void SellsBack_Click(object sender, EventArgs e)
        {


            if (SellsAccount_ .Year == -1) return;
            SellsAccount_.Account_Date_UP();
            Refresh_ListViewSells();
        }

        private void SellsButtonLeftRight_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool left;
            if (b.Name == "SellsButtonLeft") left = true;
            else left = false;

            if (SellsAccount_.Day != -1)
            {
                if (left)
                {
                    if (SellsAccount_.Day == DateTime.DaysInMonth(SellsAccount_.Year, SellsAccount_.Month))
                    {
                        if (SellsAccount_.Month == 12)
                        { SellsAccount_.Year++; SellsAccount_.Month = 1; SellsAccount_.Day = 1; }
                        else
                        { SellsAccount_.Month++; SellsAccount_.Day = 1; }

                    }
                    else SellsAccount_.Day++;
                }
                else
                {
                    if (SellsAccount_.Day == 1)
                    {

                        if (SellsAccount_.Month == 1)
                        { SellsAccount_.Year--; SellsAccount_.Month = 12; }
                        else
                        { SellsAccount_.Month--; }
                        SellsAccount_.Day = DateTime.DaysInMonth(SellsAccount_.Year, SellsAccount_.Month);
                    }
                    else SellsAccount_.Day--;
                }

            }
            else if (SellsAccount_.Month != -1)
            {
                if (left)
                {
                    if (SellsAccount_.Month == 12)
                    {
                        SellsAccount_.Year++; SellsAccount_.Month = 1;
                    }
                    else SellsAccount_.Month++;
                }
                else
                {
                    if (SellsAccount_.Month == 1)
                    {
                        SellsAccount_.Year--; SellsAccount_.Month = 12;
                    }
                    else SellsAccount_.Month--;
                }
            }
            else if (SellsAccount_.Year != -1)
            {
                if (left)
                {
                    SellsAccount_.Year++;
                    SellsAccount_.YearRange_.min_year++;
                    SellsAccount_.YearRange_.max_year++;
                }
                else
                {
                    SellsAccount_.Year--;
                    SellsAccount_.YearRange_.min_year--;
                    SellsAccount_.YearRange_.max_year--;
                }
            }
            else
            {
                if (left)
                {

                    SellsAccount_.YearRange_.min_year += 10;
                    SellsAccount_.YearRange_.max_year += 10;
                }
                else
                {
                    SellsAccount_.YearRange_.min_year -= 10;
                    SellsAccount_.YearRange_.max_year -= 10;
                }
            }
            Refresh_ListViewSells();
        }

        public void ListViewSellsAccountDown()
        {
            try
            {
                if (SellsAccount_.Year == -1 || SellsAccount_.Month == -1 || SellsAccount_.Day == -1)
                {
                    SellsAccount_.Account_Date_Down(Convert.ToInt32(listViewSells .SelectedItems[0].Name));
                    Refresh_ListViewSells ();
                }
                else
                {

                    OpenBillSell_MenuItem.PerformClick();
                }


            }
            catch (Exception ee)
            {

            }


        }
        private void ListViewSells_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewSells .SelectedItems.Count > 0)
                ListViewSellsAccountDown();
        }
        private void ListViewSells_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                ListViewSellsAccountDown();
        }
        private void ListViewSells_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listViewSells.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    foreach (ListViewItem item1 in listViewSells.Items)
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
                        if (SellsAccount_.Day != -1)
                        {

                            List<MenuItem> MenuItemList = new List<MenuItem>();
                            MenuItemList.Add(Refresh_MenuItem);
                            MenuItemList.Add(new MenuItem("-"));
                            MenuItemList.AddRange(new MenuItem[] {OpenBillSell_MenuItem , EditBillSell_MenuItem , DeleteBillSell_MenuItem
                            , new MenuItem("-"),CreateBillSell_MenuItem });
                            MenuItemList.AddRange(new MenuItem[] { new MenuItem("-"), AddPayIN_BillSell_MenuItem });
                            listViewSells .ContextMenu = new ContextMenu(MenuItemList.ToArray());


                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewSells.ContextMenu = new ContextMenu(mi1);


                        }


                    }
                    else
                    {
                        if (SellsAccount_.Day != -1)
                        {
                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem, new MenuItem("-"), CreateBillSell_MenuItem };
                            listViewSells.ContextMenu = new ContextMenu(mi1);
                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewSells.ContextMenu = new ContextMenu(mi1);


                        }

                    }

                }
            }
        }

        public async void IntializeListViewSellsColumnsWidth()
        {

            if (SellsAccount_.Day != -1)
            {


                listViewSells .Columns[0].Width = 75;//time
                listViewSells.Columns[1].Width = 60;//id
                listViewSells.Columns[2].Width = 75;//selltype
                listViewSells.Columns[3].Width = 100;//owner
                listViewSells.Columns[4].Width = 75;//clause count
                listViewSells.Columns[5].Width = 90;//value
                listViewSells.Columns[6].Width = 100;//exchangerate
                listViewSells.Columns[7].Width = 130;//paid
                listViewSells.Columns[8].Width = 90;//remain
                listViewSells.Columns[9].Width = 100;//item in cost
                listViewSells.Columns[10].Width = 130;//real item in cost
                listViewSells.Columns[11].Width = 130;//real items out cost
                listViewSells.Columns[12].Width = 130;//real profit value
                listViewSells.Columns[13].Width = 130;//real pays value
            }
            else
            {
                listViewSells.Columns[0].Width = 100;//--
                listViewSells.Columns[1].Width = 120;//bills count
                listViewSells.Columns[2].Width = 115;//clause count
                listViewSells.Columns[3].Width = 125;//bill value
                listViewSells.Columns[4].Width = 125;//bills pays value
                listViewSells.Columns[5].Width = 120;//remain
                listViewSells.Columns[6].Width = 140;//item in value
                listViewSells.Columns[7].Width = 115;//item in real value
                listViewSells.Columns[8].Width = 145;//real value
                listViewSells.Columns[9].Width = 115;//profit
                listViewSells.Columns[10].Width = 125;//real p
            }
        }
        private void CreateBillSell_MenuItem_Click(object sender, EventArgs e)
        {
            BillSellForm BillINForm_ = new BillSellForm(DB, GetSelectedDate(), null);
            BillINForm_.ShowDialog();
            if (BillINForm_.Changed)
            {
                Refresh_ListViewSells();
            }
        }
        private void OpenBillSell_MenuItem_Click(object sender, EventArgs e)
        {
            uint billSellid = Convert.ToUInt32(listViewSells.SelectedItems[0].Name);
            BillSell BillSell_ = new BillSellSQL(DB).GetBillSell_INFO_BYID(billSellid);
            BillSellForm BillOUTForm_ = new BillSellForm(DB, BillSell_, false);
            BillOUTForm_.ShowDialog();
            if (BillOUTForm_.Changed)
            {
                Refresh_ListViewSells();
            }
            BillOUTForm_.Dispose();
        }
        private void EditBillSell_MenuItem_Click(object sender, EventArgs e)
        {
            uint billSellid = Convert.ToUInt32(listViewSells.SelectedItems[0].Name);
            BillSell BillSell_ = new BillSellSQL(DB).GetBillSell_INFO_BYID(billSellid);
            BillSellForm BillOUTForm_ = new BillSellForm(DB, BillSell_, true);
            BillOUTForm_.ShowDialog();
            if (BillOUTForm_.Changed)
            {
                Refresh_ListViewSells();
            }
            BillOUTForm_.Dispose();
        }
        private void DeleteBillSell_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd != DialogResult.OK) return;
            uint billSellid = Convert.ToUInt32(listViewSells.SelectedItems[0].Name);
            bool success = new BillSellSQL(DB).DeleteBillSell(billSellid);
            if (success)
            {
                Refresh_ListViewSells();
            }

        }
        private void AddPayIN_BillSell_MenuItem_Click(object sender, EventArgs e)
        {
            if(listViewSells.SelectedItems.Count ==1)
            {
                uint sid = Convert.ToUInt32(listViewSells.SelectedItems[0].Name);
                BillSell BillSell_ = new BillSellSQL(DB).GetBillSell_INFO_BYID(sid);
                PayINForm PayINForm_ = new PayINForm(DB, MoneyAccount_.GetDate(), BillSell_);
                PayINForm_.ShowDialog();
                if (PayINForm_.DialogResult == DialogResult.OK)
                {
                    Refresh_ListViewMoneyDataDetails();
                    Refresh_ListViewSells();
                    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
                }
            }
            

        }
        #endregion
        #region ReportPayOrders
        public async void PayOrders_FillReport()
        {

            PayOrdersLabelAccountDate.Text = PayOrdersAccount_.GetAccountDateString();



            if (PayOrdersAccount_.Day != -1)
            {

                PayOrdersLabelAccountType.Text = "حساب اليوم";
                PayOrdersLabelReport.Text = "تقرير حساب اليوم : " + PayOrdersAccount_.GetAccountDateString();
                #region DaySection

                Report_PayOrders_Month_ReportDetail Report_PayOrders_DayReport
                    = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Day_Report(PayOrdersAccount_.Year, PayOrdersAccount_.Month, PayOrdersAccount_.Day);
                textBoxPayOrders_SalaryCount.Text = Report_PayOrders_DayReport.Salary_PayOrders_Count .ToString();
                textBoxPayOrders_OthersCount.Text = Report_PayOrders_DayReport.Other_PayOrders_Count .ToString();
                textBoxPayOrders_Value.Text = Report_PayOrders_DayReport.PayOrders_Value;
                textBoxPayOrdersPaysValue.Text = Report_PayOrders_DayReport.PayOrders_Pays_Value;
                textBoxPayOrdersPaysRmain.Text = Report_PayOrders_DayReport.PayOrders_Pays_Remain;

                textBoxPayOrderRealValue.Text = Report_PayOrders_DayReport.PayOrders_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxPayOrders_RealPays.Text = Report_PayOrders_DayReport.PayOrders_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

               
                #endregion
            }
            else if (PayOrdersAccount_.Month != -1)
            {
                PayOrdersLabelAccountType.Text = "حساب الشهر";
                PayOrdersLabelReport.Text = "تقرير حساب الشهر : " + PayOrdersAccount_.GetAccountDateString();

                #region MonthSection

                Report_PayOrders_Year_ReportDetail Report_PayOrders_MonthReport
                     = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Month_Report(PayOrdersAccount_.Year, PayOrdersAccount_.Month);
                textBoxPayOrders_SalaryCount.Text = Report_PayOrders_MonthReport.Salary_PayOrders_Count.ToString();
                textBoxPayOrders_OthersCount.Text = Report_PayOrders_MonthReport.Other_PayOrders_Count.ToString();
                textBoxPayOrders_Value.Text = Report_PayOrders_MonthReport.PayOrders_Value;
                textBoxPayOrdersPaysValue.Text = Report_PayOrders_MonthReport.PayOrders_Pays_Value;
                textBoxPayOrdersPaysRmain.Text = Report_PayOrders_MonthReport.PayOrders_Pays_Remain;

                textBoxPayOrderRealValue.Text = Report_PayOrders_MonthReport.PayOrders_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxPayOrders_RealPays.Text = Report_PayOrders_MonthReport.PayOrders_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else if (PayOrdersAccount_.Year != -1)
            {
                PayOrdersLabelAccountType.Text = "حساب السنة";
                PayOrdersLabelReport.Text = "تقرير حساب السنة : " + PayOrdersAccount_.GetAccountDateString();

                #region YearSection

                Report_PayOrders_YearRange_ReportDetail Report_PayOrders_YearReport
                   = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Year_Report(PayOrdersAccount_.Year);
                textBoxPayOrders_SalaryCount.Text = Report_PayOrders_YearReport.Salary_PayOrders_Count.ToString();
                textBoxPayOrders_OthersCount.Text = Report_PayOrders_YearReport.Other_PayOrders_Count.ToString();
                textBoxPayOrders_Value.Text = Report_PayOrders_YearReport.PayOrders_Value;
                textBoxPayOrdersPaysValue.Text = Report_PayOrders_YearReport.PayOrders_Pays_Value;
                textBoxPayOrdersPaysRmain.Text = Report_PayOrders_YearReport.PayOrders_Pays_Remain;

                textBoxPayOrderRealValue.Text = Report_PayOrders_YearReport.PayOrders_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                textBoxPayOrders_RealPays.Text = Report_PayOrders_YearReport.PayOrders_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
                #endregion
            }
            else
            {
                PayOrdersLabelAccountType.Text = "حساب السنوات";
                PayOrdersLabelReport.Text = "تقرير حساب السنوات : " + PayOrdersAccount_.GetAccountDateString();

                #region YearRangeSection

                textBoxPayOrders_SalaryCount.Text = "-";
                textBoxPayOrders_OthersCount.Text = "-";
                textBoxPayOrders_Value.Text = "-";
                textBoxPayOrdersPaysValue.Text = "-";
                textBoxPayOrdersPaysRmain.Text = "-";
                textBoxPayOrderRealValue.Text = "-";
                textBoxPayOrders_RealPays.Text = "-";
                #endregion
            }


        }
        public async void Refresh_ListViewPayOrders()
        {

            listViewPayOrders.Items.Clear();
            if (PayOrdersAccount_.Day != -1)
            {

                #region DaySection


                if (listViewPayOrders.Name != "ListViewPayOrders_Day")
                {
                    Report_PayOrders_Day_ReportDetail.IntiliazeListView(ref listViewPayOrders);

                }
                List<Report_PayOrders_Day_ReportDetail> Report_PayOrders_Day_ReportDetail_List
                          = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Day_ReportDetail(PayOrdersAccount_.Year, PayOrdersAccount_.Month, PayOrdersAccount_.Day);
                for (int i = 0; i < Report_PayOrders_Day_ReportDetail_List.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_PayOrders_Day_ReportDetail_List[i].PayOrder_Time.ToShortTimeString());
                    item.Name = Report_PayOrders_Day_ReportDetail_List[i].PayOrderID.ToString();
                    if (Report_PayOrders_Day_ReportDetail_List[i].PayOrderType == Report_PayOrders_Day_ReportDetail.TYPE_SALARY_PAY_ODER)
                        item.SubItems.Add("تابع لامر صرف راتب");
                    else
                        item.SubItems.Add("أمر صرف مستقل");
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].PayOrderID.ToString ());
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].EmployeeName);
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].PayOrderDesc );
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].Value .ToString()
                       +" " + Report_PayOrders_Day_ReportDetail_List[i]._Currency.CurrencySymbol );
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].ExchangeRate  .ToString ());
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].PaysAmount);
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].PaysRemain.ToString() + " " + Report_PayOrders_Day_ReportDetail_List[i]._Currency.CurrencySymbol);
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].RealPays.ToString() + " " + ReferenceCurrency.CurrencySymbol);
                   
                    //-Report_PayOrders_Day_ReportDetail_List[i].Source_ItemsIN_RealCost) + " " + ReferenceCurrency.CurrencySymbol);
                    //item.SubItems.Add(Report_PayOrders_Day_ReportDetail_List[i].RealPaysValue.ToString() + " " + ReferenceCurrency.CurrencySymbol);

                    item.UseItemStyleForSubItems = false;

                    //if (Report_PayOrders_Day_ReportDetail_List[i].PaysRemain != 0)
                    //    for (int j = 0; j <= 8; j++)
                    //        item.SubItems[j].BackColor = Color.Orange;
                    //else
                    //    for (int j = 0; j <= 8; j++)
                    //        item.SubItems[j].BackColor = Color.LightGreen;

                    //if (Report_PayOrders_Day_ReportDetail_List[i].Bill_Pays_RealValue
                    //    > Report_PayOrders_Day_ReportDetail_List[i].Bill_Pays_Return_RealValue)
                    //    for (int j = 9; j <= 13; j++)
                    //        item.SubItems[j].BackColor = Color.Orange;
                    //else if (Report_PayOrders_Day_ReportDetail_List[i].Bill_Pays_RealValue
                    //    < Report_PayOrders_Day_ReportDetail_List[i].Bill_Pays_Return_RealValue)
                    //    for (int j = 9; j <= 13; j++)
                    //        item.SubItems[j].BackColor = Color.LightGreen;
                    //else
                    //    for (int j = 9; j <= 13; j++)
                    //        item.SubItems[j].BackColor = Color.Yellow;
                    listViewPayOrders.Items.Add(item);


                }
                #endregion
            }
            else if (PayOrdersAccount_.Month != -1)
            {

                #region MonthSection
                if (listViewPayOrders.Name != "ListViewPayOrders_Month")
                {
                    Report_PayOrders_Month_ReportDetail.IntiliazeListView(ref listViewPayOrders);
                }
                List<Report_PayOrders_Month_ReportDetail> Report_PayOrders_Month_ReportDetailList
                                    = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Month_ReportDetail(PayOrdersAccount_.Year, PayOrdersAccount_.Month);
                for (int i = 0; i < Report_PayOrders_Month_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_PayOrders_Month_ReportDetailList[i].DayDate.ToShortDateString());
                    item.Name = Report_PayOrders_Month_ReportDetailList[i].DayID.ToString();
                    item.SubItems.Add(Report_PayOrders_Month_ReportDetailList[i].Salary_PayOrders_Count .ToString());
                    item.SubItems.Add(Report_PayOrders_Month_ReportDetailList[i].Other_PayOrders_Count .ToString());
                    item.SubItems.Add(Report_PayOrders_Month_ReportDetailList[i].PayOrders_Value);
                    item.SubItems.Add(Report_PayOrders_Month_ReportDetailList[i].PayOrders_Pays_Value);
                    item.SubItems.Add(Report_PayOrders_Month_ReportDetailList[i].PayOrders_Pays_Remain);

                    item.SubItems.Add(Report_PayOrders_Month_ReportDetailList[i].PayOrders_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_PayOrders_Month_ReportDetailList[i].PayOrders_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);

                    item.UseItemStyleForSubItems = false;
                    //if (Report_PayOrders_Month_ReportDetailList[i].Bills_RealValue > 0 ||
                    //    Report_PayOrders_Month_ReportDetailList[i].Bills_Pays_RealValue > 0)
                    //{
                    //    if (Report_PayOrders_Month_ReportDetailList[i].Bills_Pays_Remain_UPON_BillsCurrency == 0)
                    //        for (int j = 0; j <= 5; j++)
                    //            item.SubItems[j].BackColor = Color.LightGreen;
                    //    else if (Report_PayOrders_Month_ReportDetailList[i].Bills_Pays_Remain_UPON_BillsCurrency < 0)
                    //        for (int j = 0; j <= 5; j++)
                    //            item.SubItems[j].BackColor = Color.LightSkyBlue;
                    //    else
                    //        for (int j = 0; j <= 5; j++)
                    //            item.SubItems[j].BackColor = Color.Orange;

                    //    if (Report_PayOrders_Month_ReportDetailList[i].Bills_Pays_RealValue >
                    //       Report_PayOrders_Month_ReportDetailList[i].Bills_ItemsIN_RealValue)
                    //        for (int j = 6; j <= 10; j++)
                    //            item.SubItems[j].BackColor = Color.LightGreen;
                    //    else
                    //        for (int j = 6; j <= 10; j++)
                    //            item.SubItems[j].BackColor = Color.Orange;
                    //}
                    //else
                    //{
                    //    for (int j = 0; j <= 10; j++)
                    //        item.SubItems[j].BackColor = Color.LightYellow;
                    //}

                    listViewPayOrders.Items.Add(item);

                }
                #endregion
            }
            else if (PayOrdersAccount_.Year != -1)
            {

                #region YearSection
                if (listViewPayOrders.Name != "ListViewPayOrders_Year")
                {

                    Report_PayOrders_Year_ReportDetail.IntiliazeListView(ref listViewPayOrders);

                }

                List<Report_PayOrders_Year_ReportDetail> Report_PayOrders_Year_ReportDetailList
                           = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Year_ReportDetail(PayOrdersAccount_.Year);
                for (int i = 0; i < Report_PayOrders_Year_ReportDetailList.Count; i++)
                {

                    ListViewItem item = new ListViewItem(Report_PayOrders_Year_ReportDetailList[i].MonthName);
                    item.Name = Report_PayOrders_Year_ReportDetailList[i].MonthNO.ToString();
                    item.SubItems.Add(Report_PayOrders_Year_ReportDetailList[i].Salary_PayOrders_Count.ToString());
                    item.SubItems.Add(Report_PayOrders_Year_ReportDetailList[i].Other_PayOrders_Count.ToString());
                    item.SubItems.Add(Report_PayOrders_Year_ReportDetailList[i].PayOrders_Value);
                    item.SubItems.Add(Report_PayOrders_Year_ReportDetailList[i].PayOrders_Pays_Value);
                    item.SubItems.Add(Report_PayOrders_Year_ReportDetailList[i].PayOrders_Pays_Remain);

                    item.SubItems.Add(Report_PayOrders_Year_ReportDetailList[i].PayOrders_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_PayOrders_Year_ReportDetailList[i].PayOrders_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.UseItemStyleForSubItems = false;
                    //if (Report_PayOrders_Year_ReportDetailList[i].Bills_RealValue > 0)
                    //{
                    //    if (Report_PayOrders_Year_ReportDetailList[i].Bills_Pays_Remain_UPON_BillsCurrency == 0)
                    //        for (int j = 0; j <= 5; j++)
                    //            item.SubItems[j].BackColor = Color.LightGreen;
                    //    else
                    //        for (int j = 0; j <= 5; j++)
                    //            item.SubItems[j].BackColor = Color.Orange;
                    //    if (Report_PayOrders_Year_ReportDetailList[i].Bills_Pays_RealValue >
                    //  Report_PayOrders_Year_ReportDetailList[i].Bills_ItemsIN_RealValue)
                    //        for (int j = 6; j <= 10; j++)
                    //            item.SubItems[j].BackColor = Color.LightGreen;
                    //    else
                    //        for (int j = 6; j <= 10; j++)
                    //            item.SubItems[j].BackColor = Color.Orange;
                    //}
                    //else
                    //{
                    //    for (int j = 0; j <= 10; j++)
                    //        item.SubItems[j].BackColor = Color.LightYellow;
                    //}

                    listViewPayOrders.Items.Add(item);

                }
                #endregion
            }
            else
            {
                //PayOrdersLabelAccountType.Text = "حساب السنوات";
                //PayOrdersLabelReport.Text = "تقرير حساب السنوات : " + PayOrdersAccount_.GetAccountDateString();

                #region YearRangeSection
                if (listViewPayOrders.Name != "ListViewPayOrders_YearRange")
                {
                    Report_PayOrders_YearRange_ReportDetail.IntiliazeListView(ref listViewPayOrders);
                }
                List<Report_PayOrders_YearRange_ReportDetail> Report_PayOrders_YearRange_ReportDetailList
                           = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_YearRange_ReportDetail(PayOrdersAccount_.YearRange_.min_year, PayOrdersAccount_.YearRange_.max_year);

                for (int i = 0; i < Report_PayOrders_YearRange_ReportDetailList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Report_PayOrders_YearRange_ReportDetailList[i].YearNO.ToString());
                    item.Name = Report_PayOrders_YearRange_ReportDetailList[i].YearNO.ToString();
                    item.SubItems.Add(Report_PayOrders_YearRange_ReportDetailList[i].Salary_PayOrders_Count.ToString());
                    item.SubItems.Add(Report_PayOrders_YearRange_ReportDetailList[i].Other_PayOrders_Count.ToString());
                    item.SubItems.Add(Report_PayOrders_YearRange_ReportDetailList[i].PayOrders_Value);
                    item.SubItems.Add(Report_PayOrders_YearRange_ReportDetailList[i].PayOrders_Pays_Value);
                    item.SubItems.Add(Report_PayOrders_YearRange_ReportDetailList[i].PayOrders_Pays_Remain);

                    item.SubItems.Add(Report_PayOrders_YearRange_ReportDetailList[i].PayOrders_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.SubItems.Add(Report_PayOrders_YearRange_ReportDetailList[i].PayOrders_Pays_RealValue + " " + ReferenceCurrency.CurrencySymbol);
                    item.UseItemStyleForSubItems = false;
                    //if (Report_PayOrders_YearRange_ReportDetailList[i].Bills_RealValue > 0)
                    //{
                    //    if (Report_PayOrders_YearRange_ReportDetailList[i].Bills_Pays_Remain_UPON_BillsCurrency == 0)
                    //        for (int j = 0; j <= 5; j++)
                    //            item.SubItems[j].BackColor = Color.LightGreen;
                    //    else
                    //        for (int j = 0; j <= 5; j++)
                    //            item.SubItems[j].BackColor = Color.Orange;
                    //    if (Report_PayOrders_YearRange_ReportDetailList[i].Bills_Pays_RealValue >
                    //  Report_PayOrders_YearRange_ReportDetailList[i].Bills_ItemsIN_RealValue)
                    //        for (int j = 6; j <= 10; j++)
                    //            item.SubItems[j].BackColor = Color.LightGreen;
                    //    else
                    //        for (int j = 6; j <= 10; j++)
                    //            item.SubItems[j].BackColor = Color.Orange;
                    //}
                    //else
                    //{
                    //    for (int j = 0; j <= 10; j++)
                    //        item.SubItems[j].BackColor = Color.LightYellow;
                    //}

                    listViewPayOrders.Items.Add(item);

                }

                #endregion
            }

            PayOrders_FillReport();


        }
        private void PayOrdersBack_Click(object sender, EventArgs e)
        {
            if (PayOrdersAccount_.Year == -1) return;
            PayOrdersAccount_.Account_Date_UP();
            Refresh_ListViewPayOrders();
        }

        private void PayOrdersButtonLeftRight_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool left;
            if (b.Name == "PayOrdersButtonLeft") left = true;
            else left = false;

            if (PayOrdersAccount_.Day != -1)
            {
                if (left)
                {
                    if (PayOrdersAccount_.Day == DateTime.DaysInMonth(PayOrdersAccount_.Year, PayOrdersAccount_.Month))
                    {
                        if (PayOrdersAccount_.Month == 12)
                        { PayOrdersAccount_.Year++; PayOrdersAccount_.Month = 1; PayOrdersAccount_.Day = 1; }
                        else
                        { PayOrdersAccount_.Month++; PayOrdersAccount_.Day = 1; }

                    }
                    else PayOrdersAccount_.Day++;
                }
                else
                {
                    if (PayOrdersAccount_.Day == 1)
                    {

                        if (PayOrdersAccount_.Month == 1)
                        { PayOrdersAccount_.Year--; PayOrdersAccount_.Month = 12; }
                        else
                        { PayOrdersAccount_.Month--; }
                        PayOrdersAccount_.Day = DateTime.DaysInMonth(PayOrdersAccount_.Year, PayOrdersAccount_.Month);
                    }
                    else PayOrdersAccount_.Day--;
                }

            }
            else if (PayOrdersAccount_.Month != -1)
            {
                if (left)
                {
                    if (PayOrdersAccount_.Month == 12)
                    {
                        PayOrdersAccount_.Year++; PayOrdersAccount_.Month = 1;
                    }
                    else PayOrdersAccount_.Month++;
                }
                else
                {
                    if (PayOrdersAccount_.Month == 1)
                    {
                        PayOrdersAccount_.Year--; PayOrdersAccount_.Month = 12;
                    }
                    else PayOrdersAccount_.Month--;
                }
            }
            else if (PayOrdersAccount_.Year != -1)
            {
                if (left)
                {
                    PayOrdersAccount_.Year++;
                    PayOrdersAccount_.YearRange_.min_year++;
                    PayOrdersAccount_.YearRange_.max_year++;
                }
                else
                {
                    PayOrdersAccount_.Year--;
                    PayOrdersAccount_.YearRange_.min_year--;
                    PayOrdersAccount_.YearRange_.max_year--;
                }
            }
            else
            {
                if (left)
                {

                    PayOrdersAccount_.YearRange_.min_year += 10;
                    PayOrdersAccount_.YearRange_.max_year += 10;
                }
                else
                {
                    PayOrdersAccount_.YearRange_.min_year -= 10;
                    PayOrdersAccount_.YearRange_.max_year -= 10;
                }
            }
            Refresh_ListViewPayOrders();
        }

        public void ListViewPayOrdersAccountDown()
        {
            try
            {
                if (PayOrdersAccount_.Year == -1 || PayOrdersAccount_.Month == -1 || PayOrdersAccount_.Day == -1)
                {
                    PayOrdersAccount_.Account_Date_Down(Convert.ToInt32(listViewPayOrders.SelectedItems[0].Name));
                    Refresh_ListViewPayOrders();
                }
                else
                {

                    OpenPayOrder_MenuItem.PerformClick();

                }


            }
            catch (Exception ee)
            {

            }


        }
        private void ListViewPayOrders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listViewPayOrders.SelectedItems .Count >0)
                ListViewPayOrdersAccountDown();
        }
        private void ListViewPayOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                ListViewPayOrdersAccountDown();
        }
        private void ListViewPayOrders_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listViewPayOrders.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    foreach (ListViewItem item1 in listViewPayOrders.Items)
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
                        if (PayOrdersAccount_.Day != -1)
                        {

                            List<MenuItem> MenuItemList = new List<MenuItem>();
                            MenuItemList.Add(Refresh_MenuItem);
                            MenuItemList.Add(new MenuItem("-"));
                            MenuItemList.AddRange(new MenuItem[] {OpenPayOrder_MenuItem , EditPayOrder_MenuItem , DeletePayOrder_MenuItem
                            , new MenuItem("-"),CreatePayOrder_MenuItem });
                            MenuItemList.AddRange(new MenuItem[] { new MenuItem("-"), AddPayOUT_PayOrder_MenuItem });
                            listViewPayOrders.ContextMenu = new ContextMenu(MenuItemList.ToArray());


                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewPayOrders.ContextMenu = new ContextMenu(mi1);


                        }


                    }
                    else
                    {
                        if (PayOrdersAccount_.Day != -1)
                        {
                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem, new MenuItem("-"), CreatePayOrder_MenuItem };
                            listViewPayOrders.ContextMenu = new ContextMenu(mi1);
                        }
                        else
                        {

                            MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
                            listViewPayOrders.ContextMenu = new ContextMenu(mi1);


                        }

                    }

                }
            }
        }

        public async void IntializeListViewPayOrdersColumnsWidth()
        {

            if (PayOrdersAccount_.Day != -1)
            {


                listViewPayOrders.Columns[0].Width = 75;//time
                listViewPayOrders.Columns[1].Width = 125;//type
                listViewPayOrders.Columns[2].Width = 100;//id
                listViewPayOrders.Columns[3].Width = 200;//owner
                listViewPayOrders.Columns[4].Width = 200;//desc
                listViewPayOrders.Columns[5].Width = 125;//value
                listViewPayOrders.Columns[6].Width = 100;//exchangerate
                listViewPayOrders.Columns[7].Width = 125;//paid
                listViewPayOrders.Columns[8].Width = 125;//remain
                listViewPayOrders.Columns[9].Width = 100;//real value
                listViewPayOrders.Columns[10].Width = 100;//real paid

            }
            else
            {
                listViewPayOrders.Columns[0].Width = 100;//daydate
                listViewPayOrders.Columns[1].Width = 175;//salary count
                listViewPayOrders.Columns[2].Width = 175;//other count

                listViewPayOrders.Columns[3].Width = 140;// value
                listViewPayOrders.Columns[4].Width = 140;//paid
                listViewPayOrders.Columns[5].Width = 140;//remain

                listViewPayOrders.Columns[6].Width = 125;//real value
                listViewPayOrders.Columns[7].Width = 125;//real pays value
            }
        }
        private void CreatePayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SelecObjectForm SelecObjectForm_ = new SelecObjectForm("اختر موظف");
                List<Company.Objects.EmployeesReport> EmployeesReportList = new Company.CompanySQL.CompanyReportSQL(DB).GetEmployeesReportList();
                Company.Objects.EmployeesReport.InitializeListView(ref SelecObjectForm_._listView);
                Company.Objects.EmployeesReport.RefreshEmployeesReportList(ref SelecObjectForm_._listView, EmployeesReportList);
                SelecObjectForm_.adjustcolumns = f => Company.Objects.EmployeesReport.AdjustlistViewEmployeesColumnsWidth(ref SelecObjectForm_._listView);
                DialogResult dd = SelecObjectForm_.ShowDialog();
                if (dd != DialogResult.OK) return;
                Employee _Employee = new EmployeeSQL(DB).GetEmployeeInforBYID(SelecObjectForm_.ReturnID);
                EmployeePayOrderForm EmployeePayOrderForm_ = new EmployeePayOrderForm(DB, _Employee);
                EmployeePayOrderForm_.ShowDialog();
                if (EmployeePayOrderForm_.DialogResult == DialogResult.OK)
                {
                    Refresh_ListViewPayOrders();
                }
                EmployeePayOrderForm_.Dispose();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Create_EmployeePayOrder_MenuItem_Click" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OpenPayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewPayOrders.SelectedItems.Count > 0)
                {

                    uint sid = Convert.ToUInt32(listViewPayOrders.SelectedItems[0].Name);
                    EmployeePayOrder EmployeePayOrder_ = new EmployeePayOrderSQL(DB).GetPayOrder_INFO_BYID(sid);
                    EmployeePayOrderForm EmployeePayOrderForm_ = new EmployeePayOrderForm(DB, EmployeePayOrder_, false );
                    EmployeePayOrderForm_.ShowDialog();
                    if (EmployeePayOrderForm_.DialogResult == DialogResult.OK)
                    {
                        Refresh_ListViewPayOrders();
                    }

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Edit_EmployeePayOrder_MenuItem_Click" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void EditPayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewPayOrders.SelectedItems.Count > 0)
                {
       
                    uint sid = Convert.ToUInt32(listViewPayOrders.SelectedItems[0].Name);
                    EmployeePayOrder EmployeePayOrder_ = new EmployeePayOrderSQL(DB).GetPayOrder_INFO_BYID(sid);
                    EmployeePayOrderForm EmployeePayOrderForm_ = new EmployeePayOrderForm(DB, EmployeePayOrder_, true);
                    EmployeePayOrderForm_.ShowDialog();
                    if (EmployeePayOrderForm_.DialogResult == DialogResult.OK)
                    {
                        Refresh_ListViewPayOrders();
                    }

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Edit_EmployeePayOrder_MenuItem_Click" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeletePayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd != DialogResult.OK) return;
            uint PayOrderid = Convert.ToUInt32(listViewPayOrders.SelectedItems[0].Name);
            bool success = new EmployeePayOrderSQL (DB).Delete_PayOrder (PayOrderid);
            if (success)
            {
                Refresh_ListViewPayOrders();
            }

        }
        private void AddPayOUT_PayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewPayOrders .SelectedItems.Count == 1)
            {
                uint sid = Convert.ToUInt32(listViewPayOrders.SelectedItems[0].Name);
                Company.Objects.EmployeePayOrder EmployeePayOrder_ = new Company.CompanySQL.EmployeePayOrderSQL(DB).GetPayOrder_INFO_BYID(sid);
                PayOUTForm PayOUTForm_ = new PayOUTForm(DB, MoneyAccount_.GetDate(), EmployeePayOrder_);
                PayOUTForm_.ShowDialog();
                if (PayOUTForm_.DialogResult == DialogResult.OK)
                {
                    Refresh_ListViewMoneyDataDetails();
                    Refresh_ListViewPayOrders();
                    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
                }
            }




        }
        #endregion
        #region General
        private void tabPage1_Resize(object sender, EventArgs e)
        {
            AdjustmentDatagridviewColumnsWidth();
            IntializeListViewMoneyDataDetailsColumnsWidth();
            IntializeListViewSellsColumnsWidth();
            IntializeListAccountListViewReport_ColumnsWidth();
            IntializeListViewPayOrdersColumnsWidth();
        }
        private void Refresh_MenuItem_Click(object sender, EventArgs e)
        {
            Refresh_ListViewMaintenanceOPRs();
            Refresh_ListViewMoneyDataDetails();
            Refresh_ListViewPayOrders();
            Refresh_ListViewSells();
            Refresh_ListViewBuys();
            TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
        }
        private void RefreshAccount()
        {
            //TextBoxAccountMoney.Text = Account_.GetAccountMoneyOverAll();
            //MoneyAccount_ .GetAccountDetails(ref ListViewAccountDataDetails);
            //Account_.GetAccountReport(ref AccountListViewReport);
        }
        #endregion




     

       

      
        private DateTime GetSelectedDate()
        {
            //try
            //{
            //    if (Account_.Day != -1)
            //        return (new DateTime(Account_.Year), Account_.Month), Account_.Day), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            //    else if (Account_.Month != -1)
            //    {
            //        int day = ListViewAccountDataDetails.SelectedItems[0].Name);
            //        return (new DateTime(Account_.Year), Account_.Month), day));
            //    }
            //    else if (Account_.Year != -1)
            //    {
            //        int month = ListViewAccountDataDetails.SelectedItems[0].Name);
            //        return (new DateTime(Account_.Year), month, 1));
            //    }
            //    else
            //    {
            //        int year = ListViewAccountDataDetails.SelectedItems[0].Name);
            //        return (new DateTime(year, 1, 1));
            //    }
            //}
            //catch
            //{
            //    return DateTime.Now;
            //}
            return DateTime.Now;
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
               IntializeListViewMoneyDataDetailsColumnsWidth();
            IntializeListAccountListViewReport_ColumnsWidth();
            AdjustmentDatagridviewColumnsWidth();
            IntializeListViewPayOrdersColumnsWidth();
            this.tabPage1.Resize += new System.EventHandler(this.tabPage1_Resize);
        }

        #region ToolStripMenuItem
        private void ضبطالبرنامجToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration.ConfigurationForm ConfigurationForm_ = new Configuration.ConfigurationForm(DB);
                ConfigurationForm_.ShowDialog();
            }
            catch(Exception ee)
            {
                MessageBox.Show(""+ee.Message ,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void ادارةالعناصرToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ItemObj.Forms.ShowItemsForm ShowItemsForm_ = new ItemObj.Forms.ShowItemsForm(DB, null);
                ShowItemsForm_.Show();
            }
            catch(Exception ee)
            {
                MessageBox.Show(""+ee.Message ,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void ادارةالعملاءToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Trade.Forms.TradeContact.ShowContactsForm ShowContactsForm_ = new Trade.Forms.TradeContact.ShowContactsForm(DB, false);
                ShowContactsForm_.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ادارةالمستودعToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Trade.Forms.Container.ShowLocations ShowLocations_ = new Trade.Forms.Container.ShowLocations(DB, null, false);
                ShowLocations_.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void عرضالموادالمتوفرةحسبالصنفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ItemProject.ItemObj.Forms.AvailabeItemsForm AvailabeItemsForm_= new ItemProject.ItemObj.Forms.AvailabeItemsForm(DB, null, false);
                AvailabeItemsForm_.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void عرضكلالموادالمتوفرةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ItemProject.ItemObj.Forms.ShowAvailableItemSimpleForm ShowAvailableItemSimpleForm_ = new ItemProject.ItemObj.Forms.ShowAvailableItemSimpleForm(DB, false);
                ShowAvailableItemSimpleForm_.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void الصيانةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Maintenance.Forms.MaintenanceOPRForm MaintenanceForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB,null);
                //MaintenanceForm_.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void التفكيكوالتجميعToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IndustrialForm IndustrialForm_ = new IndustrialForm(DB);
            IndustrialForm_.Show();
        }

   

        private void الوظائفوالموظفينToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Company.Forms.CompanyManagmentForm CompanyManagmentForm_ = new Company.Forms.CompanyManagmentForm(DB);
            CompanyManagmentForm_.Show();
        }
        private void فاتورةشراءToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BillBuyForm BillOUTForm_ = new BillBuyForm(DB, GetSelectedDate(), null);
            BillOUTForm_.ShowDialog();
            if (BillOUTForm_.Changed)
            {
                RefreshAccount();
            }
        }

        private void عملياتالصيانةالغيرمنتهيةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maintenance.Forms.MaintenanceOPR_NotFinish_Form MaintenanceOPR_NotFinish_Form_ = new Maintenance.Forms.MaintenanceOPR_NotFinish_Form(DB);
            MaintenanceOPR_NotFinish_Form_.Show();
        }

        private void ادارةحساباتالمستخدمينToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowEmployeeUserAccountsForm ShowEmployeeUserAccountsForm_ = new ShowEmployeeUserAccountsForm(DB);
            ShowEmployeeUserAccountsForm_.Show();
        }

        private void تسجيلخروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
                DB.LogOut();
            this.Close();
        }

        private void عرضالسجلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogForm ShowLogForm_ = new ShowLogForm(DB);
            ShowLogForm_.Show();
        }



        #endregion
        //#region ReportReports
        //public async void Reports_FillReport()
        //{

        //    ReportsLabelAccountDate.Text = Report_.GetAccountDateString();



        //    if (Report_.Day != -1)
        //    {

        //        ReportsLabelAccountType.Text = "حساب اليوم";
        //        #region DaySection

        //        Report_Buys_Month_ReportDetail Report_Buys_Month_ReportDetail_
        //            = new ReportBuysSQL(DB).Get_Report_Buys_Day_Report(Report_.Year, Report_.Month, Report_.Day);
        //        textBoxReport_BillBuys_Count.Text = Report_Buys_Month_ReportDetail_.Bills_Count.ToString();
        //        textBoxReport_BillBuys_Value.Text = Report_Buys_Month_ReportDetail_.Bills_Value.ToString();
        //        textBoxReport_BillBuys_Paid.Text = Report_Buys_Month_ReportDetail_.Bills_Pays_Value;
        //        textBoxReport_BillBuys_UnPaid.Text = Report_Buys_Month_ReportDetail_.Bills_Pays_Remain;
        //        textBoxReport_BillBuys_RealValue.Text = Report_Buys_Month_ReportDetail_.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillBuys_RealPaid.Text = Report_Buys_Month_ReportDetail_.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_Sells_Month_ReportDetail Report_Sells_Month_ReportDetail_
        //            = new ReportSellsSQL(DB).Get_Report_Sells_Day_Report(Report_.Year, Report_.Month, Report_.Day);
        //        textBoxReport_BillSells_Count.Text = Report_Sells_Month_ReportDetail_.Bills_Count.ToString();
        //        textBoxReport_BillSells_Value.Text = Report_Sells_Month_ReportDetail_.Bills_Value.ToString();
        //        textBoxReport_BillSells_Paid.Text = Report_Sells_Month_ReportDetail_.Bills_Pays_Value;
        //        textBoxReport_BillSells_UnPaid.Text = Report_Sells_Month_ReportDetail_.Bills_Pays_Remain;
        //        textBoxReport_BillSells_RealValue.Text = Report_Sells_Month_ReportDetail_.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillSells_RealPaid.Text = Report_Sells_Month_ReportDetail_.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_MaintenanceOPRs_Month_ReportDetail Report_MaintenanceOPRs_Month_ReportDetail_
        //           = new ReportMaintenanceOPRsSQL (DB).Get_Report_MaintenanceOPRs_Day_Report(Report_.Year, Report_.Month, Report_.Day);
        //        textBoxReport_BillMaintenances_Count.Text = Report_MaintenanceOPRs_Month_ReportDetail_.BillMaintenances_Count .ToString();
        //        textBoxReport_BillMaintenances_Value.Text = Report_MaintenanceOPRs_Month_ReportDetail_.BillMaintenances_Value.ToString();
        //        textBoxReport_BillMaintenances_Paid.Text = Report_MaintenanceOPRs_Month_ReportDetail_.BillMaintenances_Pays_Value;
        //        textBoxReport_BillMaintenances_UNPaid.Text = Report_MaintenanceOPRs_Month_ReportDetail_.BillMaintenances_Pays_Remain;
        //        textBoxReport_BillMaintenances_RealValue.Text = Report_MaintenanceOPRs_Month_ReportDetail_.BillMaintenances_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillMaintenances_RealPaid.Text = Report_MaintenanceOPRs_Month_ReportDetail_.BillMaintenances_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_PayOrders_Month_ReportDetail Report_PayOrders_Month_ReportDetail_
        //           = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Day_Report(Report_.Year, Report_.Month, Report_.Day);
        //        textBoxReport_PayOrder_Count.Text = (Report_PayOrders_Month_ReportDetail_.Salary_PayOrders_Count+
        //           Report_PayOrders_Month_ReportDetail_.Other_PayOrders_Count).ToString();
        //        textBoxReport_PayOrder_Value.Text = Report_PayOrders_Month_ReportDetail_.PayOrders_Value.ToString();
        //        textBoxReport_PayOrder_Paid.Text = Report_PayOrders_Month_ReportDetail_.PayOrders_Pays_Value;
        //        textBoxReport_PayOrder_UNPaid.Text = Report_PayOrders_Month_ReportDetail_.PayOrders_Pays_Remain;
        //        textBoxReport_PayOrder_RealValue.Text = Report_PayOrders_Month_ReportDetail_.PayOrders_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_PayOrder_RealPaid.Text = Report_PayOrders_Month_ReportDetail_.PayOrders_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        #endregion
        //    }
        //    else if (Report_.Month != -1)
        //    {
        //        ReportsLabelAccountType.Text = "حساب الشهر";

        //        #region MonthSection

        //        Report_Buys_Year_ReportDetail Report_Buys_MonthReport
        //             = new ReportBuysSQL(DB).Get_Report_Buys_Month_Report(Report_.Year, Report_.Month);
        //        textBoxReport_BillBuys_Count.Text = Report_Buys_MonthReport.Bills_Count.ToString();
        //        textBoxReport_BillBuys_Value.Text = Report_Buys_MonthReport.Bills_Value.ToString();
        //        textBoxReport_BillBuys_Paid.Text = Report_Buys_MonthReport.Bills_Pays_Value;
        //        textBoxReport_BillBuys_UnPaid.Text = Report_Buys_MonthReport.Bills_Pays_Remain;
        //        textBoxReport_BillBuys_RealValue.Text = Report_Buys_MonthReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillBuys_RealPaid.Text = Report_Buys_MonthReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_Sells_Year_ReportDetail Report_Sells_MonthReport
        //            = new ReportSellsSQL(DB).Get_Report_Sells_Month_Report(Report_.Year, Report_.Month);
        //        textBoxReport_BillSells_Count.Text = Report_Sells_MonthReport.Bills_Count.ToString();
        //        textBoxReport_BillSells_Value.Text = Report_Sells_MonthReport.Bills_Value.ToString();
        //        textBoxReport_BillSells_Paid.Text = Report_Sells_MonthReport.Bills_Pays_Value;
        //        textBoxReport_BillSells_UnPaid.Text = Report_Sells_MonthReport.Bills_Pays_Remain;
        //        textBoxReport_BillSells_RealValue.Text = Report_Sells_MonthReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillSells_RealPaid.Text = Report_Sells_MonthReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_MaintenanceOPRs_Year_ReportDetail Report_MaintenanceOPRs_Month_Report
        //           = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Month_Report(Report_.Year, Report_.Month);
        //        textBoxReport_BillMaintenances_Count.Text = Report_MaintenanceOPRs_Month_Report.BillMaintenances_Count.ToString();
        //        textBoxReport_BillMaintenances_Value.Text = Report_MaintenanceOPRs_Month_Report.BillMaintenances_Value.ToString();
        //        textBoxReport_BillMaintenances_Paid.Text = Report_MaintenanceOPRs_Month_Report.BillMaintenances_Pays_Value;
        //        textBoxReport_BillMaintenances_UNPaid.Text = Report_MaintenanceOPRs_Month_Report.BillMaintenances_Pays_Remain;
        //        textBoxReport_BillMaintenances_RealValue.Text = Report_MaintenanceOPRs_Month_Report.BillMaintenances_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillMaintenances_RealPaid.Text = Report_MaintenanceOPRs_Month_Report.BillMaintenances_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_PayOrders_Year_ReportDetail Report_PayOrders_Month_Report
        //           = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Month_Report(Report_.Year, Report_.Month);
        //        textBoxReport_PayOrder_Count.Text = (Report_PayOrders_Month_Report.Salary_PayOrders_Count +
        //           Report_PayOrders_Month_Report.Other_PayOrders_Count).ToString();
        //        textBoxReport_PayOrder_Value.Text = Report_PayOrders_Month_Report.PayOrders_Value.ToString();
        //        textBoxReport_PayOrder_Paid.Text = Report_PayOrders_Month_Report.PayOrders_Pays_Value;
        //        textBoxReport_PayOrder_UNPaid.Text = Report_PayOrders_Month_Report.PayOrders_Pays_Remain;
        //        textBoxReport_PayOrder_RealValue.Text = Report_PayOrders_Month_Report.PayOrders_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_PayOrder_RealPaid.Text = Report_PayOrders_Month_Report.PayOrders_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        #endregion
        //    }
        //    else if (Report_.Year != -1)
        //    {
        //        ReportsLabelAccountType.Text = "حساب السنة";

        //        #region YearSection

        //        Report_Buys_YearRange_ReportDetail Report_Reports_YearReport
        //           = new ReportBuysSQL(DB).Get_Report_Buys_Year_Report(Report_.Year);
        //        textBoxReport_BillBuys_Count.Text = Report_Reports_YearReport.Bills_Count.ToString();
        //        textBoxReport_BillBuys_Value.Text = Report_Reports_YearReport.Bills_Value.ToString();
        //        textBoxReport_BillBuys_Paid.Text = Report_Reports_YearReport.Bills_Pays_Value;
        //        textBoxReport_BillBuys_UnPaid.Text = Report_Reports_YearReport.Bills_Pays_Remain;
        //        textBoxReport_BillBuys_RealValue.Text = Report_Reports_YearReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillBuys_RealPaid.Text = Report_Reports_YearReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;


        //        Report_Sells_YearRange_ReportDetail Report_Sells_YearReport
        //            = new ReportSellsSQL(DB).Get_Report_Sells_Year_Report(Report_.Year);
        //        textBoxReport_BillSells_Count.Text = Report_Sells_YearReport.Bills_Count.ToString();
        //        textBoxReport_BillSells_Value.Text = Report_Sells_YearReport.Bills_Value.ToString();
        //        textBoxReport_BillSells_Paid.Text = Report_Sells_YearReport.Bills_Pays_Value;
        //        textBoxReport_BillSells_UnPaid.Text = Report_Sells_YearReport.Bills_Pays_Remain;
        //        textBoxReport_BillSells_RealValue.Text = Report_Sells_YearReport.Bills_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillSells_RealPaid.Text = Report_Sells_YearReport.Bills_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_MaintenanceOPRs_YearRange_ReportDetail Report_MaintenanceOPRs_Year_Report
        //           = new ReportMaintenanceOPRsSQL(DB).Get_Report_MaintenanceOPRs_Year_Report(Report_.Year);
        //        textBoxReport_BillMaintenances_Count.Text = Report_MaintenanceOPRs_Year_Report.BillMaintenances_Count.ToString();
        //        textBoxReport_BillMaintenances_Value.Text = Report_MaintenanceOPRs_Year_Report.BillMaintenances_Value.ToString();
        //        textBoxReport_BillMaintenances_Paid.Text = Report_MaintenanceOPRs_Year_Report.BillMaintenances_Pays_Value;
        //        textBoxReport_BillMaintenances_UNPaid.Text = Report_MaintenanceOPRs_Year_Report.BillMaintenances_Pays_Remain;
        //        textBoxReport_BillMaintenances_RealValue.Text = Report_MaintenanceOPRs_Year_Report.BillMaintenances_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_BillMaintenances_RealPaid.Text = Report_MaintenanceOPRs_Year_Report.BillMaintenances_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;

        //        Report_PayOrders_YearRange_ReportDetail Report_PayOrders_Year_Report
        //           = new ReportPayOrdersSQL(DB).Get_Report_PayOrders_Year_Report(Report_.Year);
        //        textBoxReport_PayOrder_Count.Text = (Report_PayOrders_Year_Report.Salary_PayOrders_Count +
        //           Report_PayOrders_Year_Report.Other_PayOrders_Count).ToString();
        //        textBoxReport_PayOrder_Value.Text = Report_PayOrders_Year_Report.PayOrders_Value.ToString();
        //        textBoxReport_PayOrder_Paid.Text = Report_PayOrders_Year_Report.PayOrders_Pays_Value;
        //        textBoxReport_PayOrder_UNPaid.Text = Report_PayOrders_Year_Report.PayOrders_Pays_Remain;
        //        textBoxReport_PayOrder_RealValue.Text = Report_PayOrders_Year_Report.PayOrders_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        textBoxReport_PayOrder_RealPaid.Text = Report_PayOrders_Year_Report.PayOrders_Pays_RealValue.ToString() + " " + ReferenceCurrency.CurrencySymbol;
        //        #endregion
        //    }
        //    else
        //    {
        //        //ReportsLabelAccountType.Text = "حساب السنوات";
        //        //ReportsLabelReport.Text = "تقرير حساب السنوات : " + Report_.GetAccountDateString();

        //        //#region YearRangeSection

        //        //textBoxReports_AmountIN.Text = "-";
        //        //textBoxReports_Value.Text = "-";
        //        //textBoxReportsPaysValue.Text = "-";
        //        //textBoxReportsPaysRmain.Text = "-";
        //        //textBoxReports_Out_Pays.Text = "-";
        //        //textBoxReportRealValue.Text = "-";
        //        //textBoxReports_OutValue.Text = "-";
        //        //textBoxReports_RealPays.Text = "-";
        //        //#endregion
        //    }


        //}

        //private void ReportsBack_Click(object sender, EventArgs e)
        //{
        //    if (Report_.Year == -1) return;
        //    Report_.Account_Date_UP();
        //    Reports_FillReport();
        //}

        //private void ReportsButtonLeftRight_Click(object sender, EventArgs e)
        //{
        //    Button b = (Button)sender;
        //    bool left;
        //    if (b.Name == "ReportsButtonLeft") left = true;
        //    else left = false;

        //    if (Report_.Day != -1)
        //    {
        //        if (left)
        //        {
        //            if (Report_.Day == DateTime.DaysInMonth(Report_.Year, Report_.Month))
        //            {
        //                if (Report_.Month == 12)
        //                { Report_.Year++; Report_.Month = 1; Report_.Day = 1; }
        //                else
        //                { Report_.Month++; Report_.Day = 1; }

        //            }
        //            else Report_.Day++;
        //        }
        //        else
        //        {
        //            if (Report_.Day == 1)
        //            {

        //                if (Report_.Month == 1)
        //                { Report_.Year--; Report_.Month = 12; }
        //                else
        //                { Report_.Month--; }
        //                Report_.Day = DateTime.DaysInMonth(Report_.Year, Report_.Month);
        //            }
        //            else Report_.Day--;
        //        }

        //    }
        //    else if (Report_.Month != -1)
        //    {
        //        if (left)
        //        {
        //            if (Report_.Month == 12)
        //            {
        //                Report_.Year++; Report_.Month = 1;
        //            }
        //            else Report_.Month++;
        //        }
        //        else
        //        {
        //            if (Report_.Month == 1)
        //            {
        //                Report_.Year--; Report_.Month = 12;
        //            }
        //            else Report_.Month--;
        //        }
        //    }
        //    else if (Report_.Year != -1)
        //    {
        //        if (left)
        //        {
        //            Report_.Year++;
        //            Report_.YearRange_.min_year++;
        //            Report_.YearRange_.max_year++;
        //        }
        //        else
        //        {
        //            Report_.Year--;
        //            Report_.YearRange_.min_year--;
        //            Report_.YearRange_.max_year--;
        //        }
        //    }
        //    else
        //    {
        //        if (left)
        //        {

        //            Report_.YearRange_.min_year += 10;
        //            Report_.YearRange_.max_year += 10;
        //        }
        //        else
        //        {
        //            Report_.YearRange_.min_year -= 10;
        //            Report_.YearRange_.max_year -= 10;
        //        }
        //    }
        //    Reports_FillReport();
        //}

        //public void ListViewReportsAccountDown()
        //{
        //    try
        //    {
        //        if (Report_.Year == -1 || Report_.Month == -1 || Report_.Day == -1)
        //        {
        //            Report_.Account_Date_Down(Convert.ToInt32(listViewReports.SelectedItems[0].Name));
        //            Refresh_ListViewReports();
        //        }
        //        else
        //        {

        //            OpenBillReport_MenuItem.PerformClick();

        //        }


        //    }
        //    catch (Exception ee)
        //    {

        //    }


        //}
        //private void ListViewReports_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (listViewReports.SelectedItems.Count > 0)
        //        ListViewReportsAccountDown();
        //}
        //private void ListViewReports_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyValue == 13)
        //        ListViewReportsAccountDown();
        //}
        //private void ListViewReports_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    {
        //        listViewReports.ContextMenu = null;
        //        bool match = false;
        //        ListViewItem listitem = new ListViewItem();
        //        if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //        {
        //            foreach (ListViewItem item1 in listViewReports.Items)
        //            {
        //                if (item1.Bounds.Contains(new Point(e.X, e.Y)))
        //                {
        //                    match = true;
        //                    listitem = item1;
        //                    break;
        //                }
        //            }

        //            if (match)
        //            {
        //                if (Report_.Day != -1)
        //                {

        //                    List<MenuItem> MenuItemList = new List<MenuItem>();
        //                    MenuItemList.Add(Refresh_MenuItem);
        //                    MenuItemList.Add(new MenuItem("-"));
        //                    MenuItemList.AddRange(new MenuItem[] {OpenBillReport_MenuItem , EditBillReport_MenuItem , DeleteBillReport_MenuItem
        //                    , new MenuItem("-"),CreateBillReport_MenuItem });
        //                    MenuItemList.AddRange(new MenuItem[] { new MenuItem("-"), AddBillReport_PayOUT_MenuItem });
        //                    listViewReports.ContextMenu = new ContextMenu(MenuItemList.ToArray());


        //                }
        //                else
        //                {

        //                    MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
        //                    listViewReports.ContextMenu = new ContextMenu(mi1);


        //                }


        //            }
        //            else
        //            {
        //                if (Report_.Day != -1)
        //                {
        //                    MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem, new MenuItem("-"), CreateBillReport_MenuItem };
        //                    listViewReports.ContextMenu = new ContextMenu(mi1);
        //                }
        //                else
        //                {

        //                    MenuItem[] mi1 = new MenuItem[] { Refresh_MenuItem };
        //                    listViewReports.ContextMenu = new ContextMenu(mi1);


        //                }

        //            }

        //        }
        //    }
        //}

        //public async void IntializeListViewReportsColumnsWidth()
        //{

        //    if (Report_.Day != -1)
        //    {


        //        listViewReports.Columns[0].Width = 75;//time
        //        listViewReports.Columns[1].Width = 60;//id
        //        listViewReports.Columns[2].Width = 100;//owner
        //        listViewReports.Columns[3].Width = 60;//clause count
        //        listViewReports.Columns[4].Width = 125;//amount in
        //        listViewReports.Columns[5].Width = 125;//amount remain
        //        listViewReports.Columns[6].Width = 100;//value
        //        listViewReports.Columns[7].Width = 100;//exchangerate
        //        listViewReports.Columns[8].Width = 100;//paid
        //        listViewReports.Columns[9].Width = 100;//remain
        //        listViewReports.Columns[10].Width = 140;//قيمة الفاتور الفعلية
        //        listViewReports.Columns[11].Width = 150;// المدفوع الفعلي
        //        listViewReports.Columns[12].Width = 140;//قيمة  الخارج
        //        listViewReports.Columns[13].Width = 140;//عائدات الفاتورة
        //        listViewReports.Columns[14].Width = 140;//القيمة العلية للعائدات

        //    }
        //    else
        //    {
        //        listViewReports.Columns[0].Width = 100;//--
        //        listViewReports.Columns[1].Width = 120;//bills count
        //        listViewReports.Columns[2].Width = 115;//clause count
        //        listViewReports.Columns[3].Width = 125;//bill value
        //        listViewReports.Columns[4].Width = 125;//bills pays value
        //        listViewReports.Columns[5].Width = 120;//remain
        //        listViewReports.Columns[6].Width = 140;//item in value
        //        listViewReports.Columns[7].Width = 115;//item in real value
        //        listViewReports.Columns[8].Width = 145;//real value
        //        listViewReports.Columns[9].Width = 115;//profit
        //        listViewReports.Columns[10].Width = 125;//real p
        //    }
        //}
        //private void CreateBillReport_MenuItem_Click(object sender, EventArgs e)
        //{
        //    BillReportForm BillOUTForm_ = new BillReportForm(DB, GetSelectedDate(), null);
        //    BillOUTForm_.ShowDialog();
        //    if (BillOUTForm_.Changed)
        //    {
        //        RefreshAccount();
        //    }
        //}
        //private void OpenBillReport_MenuItem_Click(object sender, EventArgs e)
        //{
        //    uint billReportid = Convert.ToUInt32(listViewReports.SelectedItems[0].Name);
        //    BillReport BillReport_ = new BillReportSQL(DB).GetBillReport_INFO_BYID(billReportid);
        //    BillReportForm BillOUTForm_ = new BillReportForm(DB, BillReport_, false);
        //    BillOUTForm_.ShowDialog();
        //    if (BillOUTForm_.Changed)
        //    {
        //        RefreshAccount();
        //    }
        //    BillOUTForm_.Dispose();
        //}
        //private void EditBillReport_MenuItem_Click(object sender, EventArgs e)
        //{
        //    uint billReportid = Convert.ToUInt32(listViewReports.SelectedItems[0].Name);
        //    BillReport BillReport_ = new BillReportSQL(DB).GetBillReport_INFO_BYID(billReportid);
        //    BillReportForm BillOUTForm_ = new BillReportForm(DB, BillReport_, true);
        //    BillOUTForm_.ShowDialog();
        //    if (BillOUTForm_.Changed)
        //    {
        //        RefreshAccount();
        //    }
        //    BillOUTForm_.Dispose();
        //}
        //private void DeleteBillReport_MenuItem_Click(object sender, EventArgs e)
        //{
        //    DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        //    if (dd != DialogResult.OK) return;
        //    uint billReportid = Convert.ToUInt32(listViewReports.SelectedItems[0].Name);
        //    bool success = new BillReportSQL(DB).DeleteBillReport(billReportid);
        //    if (success)
        //    {
        //        RefreshAccount();
        //    }

        //}
        //private void AddBillReport_PayOUT_MenuItem_Click(object sender, EventArgs e)
        //{
        //    //PayOUTForm PayOUTForm_ = new PayOUTForm(DB, MoneyAccount_.GetDate());
        //    //PayOUTForm_.ShowDialog();
        //    //if (PayOUTForm_.DialogResult == DialogResult.OK)
        //    //{
        //    //    Refresh_ListViewMoneyDataDetails();
        //    //    TextBoxAccountMoney.Text = MoneyAccountSQL_.GetAccountMoneyOverAll();
        //    //}




        //}
        //#endregion



    }


}
