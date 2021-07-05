using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using ItemProject.ItemObj.Objects;
using ItemProject.AccountingObj.Objects;
using ItemProject.Trade.Objects;
using ItemProject.Trade.TradeSQL;
using System.Drawing;
using ItemProject.Trade.Forms;
using ItemProject.Maintenance.Objects;
using ItemProject.Maintenance.MaintenanceSQL;

namespace ItemProject.AccountingObj
{
    namespace AccountingSQL
    {
        public class MoneyAccountSQL
        {
            DatabaseInterface DB;

            public static class  MoneyOPR_DayReport_Table
            {
                public const string TableName = "[dbo].[Account_GetPays_DayReport_Details]";
                public const string Oprtime = "Oprtime";
                public const string Oprtype = "Oprtype";
                public const string OprID = "OprID";
                public const string OprOwner = "OprOwner";
                public const string Direction = "Direction";
                public const string Value = "Value";
                public const string Currency = "Currency";
                public const string ExchangeRate = "ExchangeRate";
                public const string RealValue = "RealValue";
            }
            public static class AccountOprTables
            {
                public const string AccountMoneyOverAll = "[dbo].[Account_GetAmountMoneyOverAll]";
                public const string AccountOprDayReport = "[dbo].[Account_GetPays_DayReport]";
                public const string AccountOprMonthReport = "[dbo].[Account_GetPays_MonthReport]";
                public const string AccountOprMonthReport_Details = "[dbo].[Account_GetPays_MonthReport_Details]";
                public const string AccountOprYearReport = "[dbo].[Account_GetPays_YearReport]";
                public const string AccountOprYearReport_Details = "[dbo].[Account_GetPays_YearReport_Details]";
                public const string AccountOprYearRangeReport = "[dbo].[Account_GetPays_YearRangeReport]";
                public const string AccountOprYearRangeReport_Details = "[dbo].[Account_GetPays_YearRangeReport_Details]";

            }
    
            public MoneyAccountSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public string GetAccountMoneyOverAll()
            {
                string returnstring = "";
                DataTable table = DB.GetData("select * from " 
                    +AccountOprTables.AccountMoneyOverAll
                    + "()");
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string currency_symbol = table.Rows[i][0].ToString().Replace(" ",string.Empty );
                        returnstring = returnstring + " [ "
                            + table.Rows[i][1].ToString()
                        + currency_symbol + " ] ";
                       
                    }
                }
                else returnstring = "0";
                return "قيمة الصندوق :" + returnstring;
            }
            public List<PayCurrencyReport> GetPayReport_InDay(int year, int month, int day)
            {
                List<PayCurrencyReport> PayCurrencyReportList = new List<PayCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountOprTables.AccountOprDayReport
                        + "("
                        + year + ","
                        + month + ","
                        + day
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        uint currencyid = Convert.ToUInt32(table.Rows[i][0].ToString());
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(currencyid);
                        double payin_sell = Convert.ToDouble(table.Rows[i][2].ToString());
                        double payin_mainenance = Convert.ToDouble(table.Rows[i][3].ToString());
                        double payin_non = Convert.ToDouble(table.Rows[i][4].ToString());
                        double payin_Exchange = Convert.ToDouble(table.Rows[i][5].ToString());
                        double payout_buy = Convert.ToDouble(table.Rows[i][6].ToString());
                        double payout_emp = Convert.ToDouble(table.Rows[i][7].ToString());
                        double payout_non = Convert.ToDouble(table.Rows[i][8].ToString());
                        double payout_Exchange = Convert.ToDouble(table.Rows[i][9].ToString());
                        PayCurrencyReport paycurrency_report =
                            new PayCurrencyReport(Currency_, payin_sell, payin_mainenance, payin_non
                            , payin_Exchange, payout_buy , payout_emp, payout_non,payout_Exchange);
                        PayCurrencyReportList.Add(paycurrency_report);
                    }
                    return PayCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return PayCurrencyReportList;
                }
            }
            public List<PayCurrencyReport> GetPayReport_InMonth(int year, int month)
            {
                List<PayCurrencyReport> PayCurrencyReportList = new List<PayCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountOprTables.AccountOprMonthReport
                        + "("
                        + year + ","
                        + month 
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        uint currencyid = Convert.ToUInt32(table.Rows[i][0].ToString());
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(currencyid);
                        double payin_sell = Convert.ToDouble(table.Rows[i][2].ToString());
                        double payin_mainenance = Convert.ToDouble(table.Rows[i][3].ToString());
                        double payin_non = Convert.ToDouble(table.Rows[i][4].ToString());
                        double payin_Exchange = Convert.ToDouble(table.Rows[i][5].ToString());
                        double payout_buy = Convert.ToDouble(table.Rows[i][6].ToString());
                        double payout_emp = Convert.ToDouble(table.Rows[i][7].ToString());
                        double payout_non = Convert.ToDouble(table.Rows[i][8].ToString());
                        double payout_Exchange = Convert.ToDouble(table.Rows[i][9].ToString());
                        PayCurrencyReport paycurrency_report =
                            new PayCurrencyReport(Currency_, payin_sell, payin_mainenance, payin_non
                            , payin_Exchange, payout_buy, payout_emp, payout_non, payout_Exchange);
                        PayCurrencyReportList.Add(paycurrency_report);
                    }
                    return PayCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return PayCurrencyReportList;
                }
            }
            public List<PayCurrencyReport> GetPayReport_INYear(int year)
            {
                List<PayCurrencyReport> PayCurrencyReportList = new List<PayCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountOprTables.AccountOprYearReport
                        + "("
                        + year 
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        uint currencyid = Convert.ToUInt32(table.Rows[i][0].ToString());
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(currencyid);
                        double payin_sell = Convert.ToDouble(table.Rows[i][2].ToString());
                        double payin_mainenance = Convert.ToDouble(table.Rows[i][3].ToString());
                        double payin_non = Convert.ToDouble(table.Rows[i][4].ToString());
                        double payin_Exchange = Convert.ToDouble(table.Rows[i][5].ToString());
                        double payout_buy = Convert.ToDouble(table.Rows[i][6].ToString());
                        double payout_emp = Convert.ToDouble(table.Rows[i][7].ToString());
                        double payout_non = Convert.ToDouble(table.Rows[i][8].ToString());
                        double payout_Exchange = Convert.ToDouble(table.Rows[i][9].ToString());
                        PayCurrencyReport paycurrency_report =
                            new PayCurrencyReport(Currency_, payin_sell, payin_mainenance, payin_non
                            , payin_Exchange, payout_buy, payout_emp, payout_non, payout_Exchange);
                        PayCurrencyReportList.Add(paycurrency_report);
                    }
                    return PayCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return PayCurrencyReportList;
                }
            }
            public List<PayCurrencyReport> GetPayReport_betweenTwoYears(int year1, int year2)
            {
                List<PayCurrencyReport> PayCurrencyReportList = new List<PayCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountOprTables.AccountOprYearRangeReport
                        + "("
                        + year1 + ","
                        + year2 
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        uint currencyid = Convert.ToUInt32(table.Rows[i][0].ToString());
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(currencyid);
                        double payin_sell = Convert.ToDouble(table.Rows[i][2].ToString());
                        double payin_mainenance = Convert.ToDouble(table.Rows[i][3].ToString());
                        double payin_non = Convert.ToDouble(table.Rows[i][4].ToString());
                        double payin_Exchange = Convert.ToDouble(table.Rows[i][5].ToString());
                        double payout_buy = Convert.ToDouble(table.Rows[i][6].ToString());
                        double payout_emp = Convert.ToDouble(table.Rows[i][7].ToString());
                        double payout_non = Convert.ToDouble(table.Rows[i][8].ToString());
                        double payout_Exchange = Convert.ToDouble(table.Rows[i][9].ToString());
                        PayCurrencyReport paycurrency_report =
                            new PayCurrencyReport(Currency_, payin_sell, payin_mainenance, payin_non
                            , payin_Exchange, payout_buy, payout_emp, payout_non, payout_Exchange); PayCurrencyReportList.Add(paycurrency_report);
                    }
                    return PayCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return PayCurrencyReportList;
                }
            }
            public List<AccountOprReportDetail> GetAccountOprReport_Details_InDay(int year, int month, int day)
            {
                List<AccountOprReportDetail> AccountOprReportDetailList = new List<AccountOprReportDetail>();
                try
                {

                    DataTable table = new DataTable();
                    table = DB.GetData("select "
                     + MoneyOPR_DayReport_Table.Oprtime + ","
                     + MoneyOPR_DayReport_Table.Oprtype + ","
                     + MoneyOPR_DayReport_Table.Direction + ","
                     + MoneyOPR_DayReport_Table.OprID  + ","
                     + MoneyOPR_DayReport_Table.OprOwner + ","
                     + MoneyOPR_DayReport_Table.Value  + ","
                     + MoneyOPR_DayReport_Table.Currency + ","
                     + MoneyOPR_DayReport_Table.ExchangeRate  + ","
                     + MoneyOPR_DayReport_Table.RealValue 
                    + " from   "
                    + MoneyOPR_DayReport_Table.TableName
                    + " ( "
                    +year +","
                    + month  + ","
                    + day  
                    + ") order by "
                    + MoneyOPR_DayReport_Table.Oprtime 
                      );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DateTime accountoprdate = Convert.ToDateTime(table.Rows[i][0].ToString());
                        bool accountoprtype = Convert.ToBoolean(table.Rows[i][1].ToString());
                        bool Direction_ = Convert.ToBoolean(table.Rows[i][2].ToString());
                        int accountopr_id = Convert.ToInt32(table.Rows[i][3].ToString());
                        string owner = table.Rows[i][4].ToString();
                        double value = Convert.ToDouble(table.Rows[i][5].ToString());
                        string currency = table.Rows[i][6].ToString();
                        double exchangerate = Convert.ToDouble(table.Rows[i][7].ToString());
                        double realvalue = Convert.ToDouble(table.Rows[i][8].ToString());

                        AccountOprReportDetail accountopr_reportDetail
                            = new AccountOprReportDetail(accountoprdate, accountoprtype, Direction_, accountopr_id, owner
                            , value, currency, exchangerate ,realvalue );
                        AccountOprReportDetailList.Add(accountopr_reportDetail);
                    }
                    return AccountOprReportDetailList;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير دفعات اليوم التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return AccountOprReportDetailList;
                }
            }
            public List<AccountOprDayReportDetail> GetAccountOprReport_Details_InMonth(string year, string month)
            {
                List<AccountOprDayReportDetail> AccountOprDayReportDetailList = new List<AccountOprDayReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountOprTables.AccountOprMonthReport_Details
                        + "("
                        + year + ","
                        + month
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int dayid = Convert.ToInt32(table.Rows[i][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
                        int payin_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int payout_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        int exc_count = Convert.ToInt32(table.Rows[i][4].ToString());
                        string invalue = table.Rows[i][5].ToString();
                        double real_invalue = Convert.ToDouble(table.Rows[i][6].ToString());
                        string outvalue = table.Rows[i][7].ToString();
                        double real_outvalue = Convert.ToDouble(table.Rows[i][8].ToString());
                        AccountOprDayReportDetail accountoprdayreportDetail 
                            = new AccountOprDayReportDetail(dayid, daydate, payin_count , payout_count , exc_count , invalue,real_invalue , outvalue,real_outvalue );
                        AccountOprDayReportDetailList.Add(accountoprdayreportDetail);
                    }
                    return AccountOprDayReportDetailList;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return AccountOprDayReportDetailList;
                }
            }
            public List<AccountOprMonthReportDetail> GetAccountOprReport_Details_InYear(string year)
            {
                List<AccountOprMonthReportDetail> AccountOprMonthReportDetailList = new List<AccountOprMonthReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountOprTables.AccountOprYearReport_Details
                        + "("
                        + year 
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int year_month = Convert.ToInt32(table.Rows[i][0].ToString());
                        string monthname  = table.Rows[i][1].ToString();
                        int payin_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int payout_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        int exc_count = Convert.ToInt32(table.Rows[i][4].ToString());
                        string invalue = table.Rows[i][5].ToString();
                        double real_invalue = Convert.ToDouble(table.Rows[i][6].ToString());
                        string outvalue = table.Rows[i][7].ToString();
                        double real_outvalue = Convert.ToDouble(table.Rows[i][8].ToString());

                        AccountOprMonthReportDetail accountoprmonthreportDetail
                            = new AccountOprMonthReportDetail(year_month, monthname, payin_count, payout_count, exc_count, invalue, real_invalue, outvalue, real_outvalue);
                        AccountOprMonthReportDetailList.Add(accountoprmonthreportDetail);
                    }
                    return AccountOprMonthReportDetailList;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return AccountOprMonthReportDetailList;
                }
            }
            public List<AccountOprYearReportDetail> GetAccountOprReport_Details_InYearRange(string year1, string year2)
            {
                List<AccountOprYearReportDetail> AccountOprYearReportDetailist = new List<AccountOprYearReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountOprTables.AccountOprYearRangeReport_Details
                        + "("
                        + year1
                        +","
                        +year2
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int year= Convert.ToInt32(table.Rows[i][0].ToString());
                        int payin_count = Convert.ToInt32(table.Rows[i][1].ToString());
                        int payout_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int exc_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        string invalue = table.Rows[i][4].ToString();
                        double real_invalue = Convert.ToDouble(table.Rows[i][5].ToString());
                        string outvalue = table.Rows[i][6].ToString();
                        double real_outvalue = Convert.ToDouble(table.Rows[i][7].ToString());

                        AccountOprYearReportDetail accountopryearreportDetail
                            = new AccountOprYearReportDetail(year, payin_count, payout_count, exc_count, invalue,real_invalue, outvalue,real_outvalue);
                        AccountOprYearReportDetailist.Add(accountopryearreportDetail);
                    }
                    return AccountOprYearReportDetailist;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return AccountOprYearReportDetailist;
                }
            }

        }
        public class ReportBuysSQL
        {
            DatabaseInterface DB;
            public ReportBuysSQL(DatabaseInterface db)
            {
                DB = db;

            }

      
            #region BuyReports
            public static class Reports_Buys_DayReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Buy_GetDayReport_Details]";
                public const string Bill_Time = "Bill_Time";
                public const string Bill_ID = "Bill_ID";
                public const string Bill_Owner = "Bill_Owner";
                public const string ClauseS_Count = "ClauseS_Count";
                public const string Amount_IN = "Amount_IN";
                public const string Amount_Remain = "Amount_Remain";
                public const string BillValue = "BillValue";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string PaysAmount = "PaysAmount";
                public const string PaysRemain = "PaysRemain";
                public const string Bill_RealValue = "Bill_RealValue";
                public const string Bill_Pays_RealValue = "Bill_Pays_RealValue";
                public const string Bill_ItemsOut_Value = "Bill_ItemsOut_Value";
                public const string Bill_ItemsOut_RealValue = "Bill_ItemsOut_RealValue";
                public const string Bill_Pays_Return_Value = "Bill_Pays_Return_Value";
                public const string Bill_Pays_Return_RealValue = "Bill_Pays_Return_RealValue";
            }
            public static class Reports_Buys_MonthReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Buy_GetMonthReport_Details]";
                public const string DateDayNo = "DateDayNo";
                public const string Date_day = "Date_day";
                public const string Bills_Count = "Bills_Count";
                public const string Bills_Clause_Count = "Bills_Clause_Count";
                public const string Bills_Amounts_IN = "Bills_Amounts_IN";
                public const string Bills_Amounts_Remain = "Bills_Amounts_Remain";
                public const string Bills_Value = "Bills_Value";
                public const string Bills_Pays_Value = "Bills_Pays_Value";
                public const string Bills_Pays_Remain = "Bills_Pays_Remain";
                public const string Bills_Pays_Remain_UPON_BillsCurrency = "Bills_Pays_Remain_UPON_BillsCurrency";

                public const string Bills_RealValue  = "Bills_RealValue";
                public const string Bills_Pays_RealValue = "Bills_Pays_RealValue";
                public const string Bills_ItemsOut_Value = "Bills_ItemsOut_Value";
                public const string Bills_ItemsOut_RealValue = "Bills_ItemsOut_RealValue";
                public const string Bills_Pays_Return_Value = "Bills_Pays_Return_Value";
                public const string Bills_Pays_Return_RealValue = "Bills_Pays_Return_RealValue";

            }
            public static class Reports_Buys_YearReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Buy_GetYearReport_Details]";
                public const string Year_Month = "Year_Month";
                public const string Year_Month_Name = "Year_Month_Name";
                public const string Bills_Count = "Bills_Count";
                public const string Bills_Clause_Count = "Bills_Clause_Count";
                public const string Bills_Amounts_IN = "Bills_Amounts_IN";
                public const string Bills_Amounts_Remain = "Bills_Amounts_Remain";
                public const string Bills_Value = "Bills_Value";
                public const string Bills_Pays_Value = "Bills_Pays_Value";
                public const string Bills_Pays_Remain = "Bills_Pays_Remain";
                public const string Bills_Pays_Remain_UPON_BillsCurrency = "Bills_Pays_Remain_UPON_BillsCurrency";

                public const string Bills_RealValue = "Bills_RealValue";
                public const string Bills_Pays_RealValue = "Bills_Pays_RealValue";
                public const string Bills_ItemsOut_Value = "Bills_ItemsOut_Value";
                public const string Bills_ItemsOut_RealValue = "Bills_ItemsOut_RealValue";
                public const string Bills_Pays_Return_Value = "Bills_Pays_Return_Value";
                public const string Bills_Pays_Return_RealValue = "Bills_Pays_Return_RealValue";

            }
            public static class Reports_Buys_YearRangeReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Buy_GetYearRangeReport_Details]";
                public const string Year_NO = "Year_NO";
                public const string Bills_Count = "Bills_Count";
                public const string Bills_Clause_Count = "Bills_Clause_Count";
                public const string Bills_Amounts_IN = "Bills_Amounts_IN";
                public const string Bills_Amounts_Remain = "Bills_Amounts_Remain";
                public const string Bills_Value = "Bills_Value";
                public const string Bills_Pays_Value = "Bills_Pays_Value";
                public const string Bills_Pays_Remain = "Bills_Pays_Remain";
                public const string Bills_Pays_Remain_UPON_BillsCurrency = "Bills_Pays_Remain_UPON_BillsCurrency";

                public const string Bills_RealValue = "Bills_RealValue";
                public const string Bills_Pays_RealValue = "Bills_Pays_RealValue";
                public const string Bills_ItemsOut_Value = "Bills_ItemsOut_Value";
                public const string Bills_ItemsOut_RealValue = "Bills_ItemsOut_RealValue";
                public const string Bills_Pays_Return_Value = "Bills_Pays_Return_Value";
                public const string Bills_Pays_Return_RealValue = "Bills_Pays_Return_RealValue";


            }
            public List<Report_Buys_Day_ReportDetail> Get_Report_Buys_Day_ReportDetail(int year, int month, int day)
            {
                List<Report_Buys_Day_ReportDetail> List = new List<Report_Buys_Day_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Buys_DayReportDetails_Table.Bill_Time + ","
                   + Reports_Buys_DayReportDetails_Table.Bill_ID + ","
                   + Reports_Buys_DayReportDetails_Table.Bill_Owner + ","
                   + Reports_Buys_DayReportDetails_Table.ClauseS_Count + ","
                   + Reports_Buys_DayReportDetails_Table.Amount_IN + ","
                    + Reports_Buys_DayReportDetails_Table.Amount_Remain + ","
                    + Reports_Buys_DayReportDetails_Table.BillValue  + ","
                   + Reports_Buys_DayReportDetails_Table.CurrencyID + ","
                   + Reports_Buys_DayReportDetails_Table.ExchangeRate + ","
                   + Reports_Buys_DayReportDetails_Table.PaysAmount + ","
                   + Reports_Buys_DayReportDetails_Table.PaysRemain + ","
                   + Reports_Buys_DayReportDetails_Table.Bill_RealValue + ","
                   + Reports_Buys_DayReportDetails_Table.Bill_Pays_RealValue + ","
                   + Reports_Buys_DayReportDetails_Table.Bill_ItemsOut_Value + ","
                  + Reports_Buys_DayReportDetails_Table.Bill_ItemsOut_RealValue + ","
                  + Reports_Buys_DayReportDetails_Table.Bill_Pays_Return_Value + ","
                   + Reports_Buys_DayReportDetails_Table.Bill_Pays_Return_RealValue
                   + " from "
                   + Reports_Buys_DayReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month + ","
                   + day
                   + ")"
                   + " order by  "
                   + Reports_Buys_DayReportDetails_Table.Bill_Time
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime billdate = Convert.ToDateTime(table.Rows[i][0].ToString());
                        int billid = Convert.ToInt32(table.Rows[i][1].ToString());
                        string owner = table.Rows[i][2].ToString();
                        int clause_Count = Convert.ToInt32(table.Rows[i][3].ToString());
                        double  amontin = Convert.ToInt32(table.Rows[i][4].ToString());
                        double  amountremain = Convert.ToInt32(table.Rows[i][5].ToString());


                        double billvalue = Convert.ToDouble(table.Rows[i][6].ToString());
                        Currency currency = new CurrencySQL(DB)
                            .GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][7].ToString()));
                        double exchangerate = Convert.ToDouble(table.Rows[i][8].ToString());
                        string PaysAmount = table.Rows[i][9].ToString();
                        double paysremain = Convert.ToDouble(table.Rows[i][10].ToString()); ;
                        double billrealvalue = Convert.ToDouble(table.Rows[i][11].ToString()); ;
                        double bill_Pays_realvalue = Convert.ToDouble(table.Rows[i][12].ToString()); ;

                        string itemsoutvalue = table.Rows[i][13].ToString();
                        double itemsoutRealvalue = Convert.ToDouble(table.Rows[i][14].ToString());

                        string bill_pays_returns_value = table.Rows[i][15].ToString();
                        double bill_pays_returns_realvalue = Convert.ToDouble(table.Rows[i][16].ToString());

                        Report_Buys_Day_ReportDetail Report_Buys_Day_ReportDetail_
                            = new Report_Buys_Day_ReportDetail(billdate, billid,  owner,
                            clause_Count,amontin,amountremain , billvalue ,currency ,exchangerate 
                            ,PaysAmount ,paysremain ,billrealvalue ,bill_Pays_realvalue,itemsoutvalue , itemsoutRealvalue
                            , bill_pays_returns_value,bill_pays_returns_realvalue);
                        List.Add(Report_Buys_Day_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مشتريات اليوم التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_Buys_Month_ReportDetail Get_Report_Buys_Day_Report(int year, int month, int day)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Buys_MonthReportDetails_Table.DateDayNo + ","
                   + Reports_Buys_MonthReportDetails_Table.Date_day + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Clause_Count + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_IN  + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_RealValue 

                   + " from "
                   + Reports_Buys_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month
                   + ")"
                   + " where  "
                   + Reports_Buys_MonthReportDetails_Table.DateDayNo + "=" + day
                   );
                    if (table.Rows.Count == 1)
                    {
                        int dayno = Convert.ToInt32(table.Rows[0][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[0][1].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[0][3].ToString());
                        int amountin = Convert.ToInt32(table.Rows[0][4].ToString());
                        int amountremain = Convert.ToInt32(table.Rows[0][5].ToString());
                        string bills_value = table.Rows[0][6].ToString();
                        string bills_pays_value = table.Rows[0][7].ToString();
                        string bills_pays_remain = table.Rows[0][8].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][9].ToString());
                      double bills_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][11].ToString());
                        string bills_itemsout_value = table.Rows[0][12].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][13].ToString());
                        string bills_pays_returns_value = table.Rows[0][14].ToString();
                        double bills_pays_returns_realvalue = Convert.ToDouble(table.Rows[0][15].ToString());



                        Report_Buys_Month_ReportDetail Report_Buys_Month_ReportDetail_
                            = new Report_Buys_Month_ReportDetail(dayno, daydate, bills_count, bills_clause_count,amountin,amountremain, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_realvalue, bills_pays_realvalue,bills_itemsout_value
                            ,bills_itemsout_realvalue,bills_pays_returns_value,bills_pays_returns_realvalue);
                        return Report_Buys_Month_ReportDetail_;
                    }
                    else return null;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  المشتريات اليوم ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<Report_Buys_Month_ReportDetail> Get_Report_Buys_Month_ReportDetail(int year, int month)
            {
                List<Report_Buys_Month_ReportDetail> List = new List<Report_Buys_Month_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Buys_MonthReportDetails_Table.DateDayNo + ","
                   + Reports_Buys_MonthReportDetails_Table.Date_day + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Clause_Count + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_IN + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_RealValue

                   + " from "
                   + Reports_Buys_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month
                   + ")"
                   + " order by  "
                   + Reports_Buys_MonthReportDetails_Table.DateDayNo
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int dayno = Convert.ToInt32(table.Rows[i][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        int amountin = Convert.ToInt32(table.Rows[i][4].ToString());
                        int amountremain = Convert.ToInt32(table.Rows[i][5].ToString());
                        string bills_value = table.Rows[i][6].ToString();
                        string bills_pays_value = table.Rows[i][7].ToString();
                        string bills_pays_remain = table.Rows[i][8].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][9].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[i][10].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][11].ToString());
                        string bills_itemsout_value = table.Rows[i][12].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[i][13].ToString());
                        string bills_pays_returns_value = table.Rows[i][14].ToString();
                        double bills_pays_returns_realvalue = Convert.ToDouble(table.Rows[i][15].ToString());



                        Report_Buys_Month_ReportDetail Report_Buys_Month_ReportDetail_
                            = new Report_Buys_Month_ReportDetail(dayno, daydate, bills_count, bills_clause_count, amountin, amountremain, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_realvalue, bills_pays_realvalue, bills_itemsout_value
                            , bills_itemsout_realvalue, bills_pays_returns_value, bills_pays_returns_realvalue);
                        List.Add(Report_Buys_Month_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_Buys_Year_ReportDetail Get_Report_Buys_Month_Report(int year, int month)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Buys_YearReportDetails_Table.Year_Month + ","
                   + Reports_Buys_YearReportDetails_Table.Year_Month_Name + ","
                       + Reports_Buys_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Clause_Count + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_IN + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_RealValue
                   + " from "
                   + Reports_Buys_YearReportDetails_Table.TableName
                    + "("
                   + year
                   + ")"
                   + " where  "
                   + Reports_Buys_YearReportDetails_Table.Year_Month + "=" + month
                   );
                    if (table.Rows.Count == 1)
                    {
                        int monthno = Convert.ToInt32(table.Rows[0][0].ToString());
                        string monthname = table.Rows[0][1].ToString();
                        int bills_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[0][3].ToString());
                        int amountin = Convert.ToInt32(table.Rows[0][4].ToString());
                        int amountremain = Convert.ToInt32(table.Rows[0][5].ToString());
                        string bills_value = table.Rows[0][6].ToString();
                        string bills_pays_value = table.Rows[0][7].ToString();
                        string bills_pays_remain = table.Rows[0][8].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][9].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][11].ToString());
                        string bills_itemsout_value = table.Rows[0][12].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][13].ToString());
                        string bills_pays_returns_value = table.Rows[0][14].ToString();
                        double bills_pays_returns_realvalue = Convert.ToDouble(table.Rows[0][15].ToString());

                        return  new Report_Buys_Year_ReportDetail(monthno , monthname, bills_count, bills_clause_count, amountin, amountremain, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_realvalue, bills_pays_realvalue, bills_itemsout_value
                            , bills_itemsout_realvalue, bills_pays_returns_value, bills_pays_returns_realvalue);

                    }
                    else return null;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات السنة ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<Report_Buys_Year_ReportDetail> Get_Report_Buys_Year_ReportDetail(int year)
            {
                List<Report_Buys_Year_ReportDetail> List = new List<Report_Buys_Year_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Buys_YearReportDetails_Table.Year_Month  + ","
                   + Reports_Buys_YearReportDetails_Table.Year_Month_Name + ","
                       + Reports_Buys_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Clause_Count + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_IN + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_RealValue
                   + " from "
                   + Reports_Buys_YearReportDetails_Table.TableName
                    + "("
                   + year 
                   + ")"
                   + " order by  "
                   + Reports_Buys_YearReportDetails_Table.Year_Month
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int monthno = Convert.ToInt32(table.Rows[i][0].ToString());
                        string  monthname = table.Rows[i][1].ToString();
                        int bills_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        int amountin = Convert.ToInt32(table.Rows[i][4].ToString());
                        int amountremain = Convert.ToInt32(table.Rows[i][5].ToString());
                        string bills_value = table.Rows[i][6].ToString();
                        string bills_pays_value = table.Rows[i][7].ToString();
                        string bills_pays_remain = table.Rows[i][8].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][9].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[i][10].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][11].ToString());
                        string bills_itemsout_value = table.Rows[i][12].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[i][13].ToString());
                        string bills_pays_returns_value = table.Rows[i][14].ToString();
                        double bills_pays_returns_realvalue = Convert.ToDouble(table.Rows[i][15].ToString());


                        Report_Buys_Year_ReportDetail Report_Buys_Year_ReportDetail_
                            = new Report_Buys_Year_ReportDetail(monthno, monthname, bills_count, bills_clause_count, amountin, amountremain, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_realvalue, bills_pays_realvalue, bills_itemsout_value
                            , bills_itemsout_realvalue, bills_pays_returns_value, bills_pays_returns_realvalue);
                        List.Add(Report_Buys_Year_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_Buys_YearRange_ReportDetail Get_Report_Buys_Year_Report(int year)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select  "
                   + Reports_Buys_YearRangeReportDetails_Table.Year_NO + ","
                        + Reports_Buys_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Clause_Count + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_IN + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_RealValue
                   + " from "
                   + Reports_Buys_YearRangeReportDetails_Table.TableName
                    + "("
                   + (year - 5) + ","
                   + (year + 5)
                   + ")"
                   + " where  "
                   + Reports_Buys_YearRangeReportDetails_Table.Year_NO + "=" + year
                   );
                    if (table.Rows.Count == 1)
                    {
                        int yearno = Convert.ToInt32(table.Rows[0][0].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[0][1].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int amountin = Convert.ToInt32(table.Rows[0][3].ToString());
                        int amountremain = Convert.ToInt32(table.Rows[0][4].ToString());
                        string bills_value = table.Rows[0][5].ToString();
                        string bills_pays_value = table.Rows[0][6].ToString();
                        string bills_pays_remain = table.Rows[0][7].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][8].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[0][9].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());
                        string bills_itemsout_value = table.Rows[0][11].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][12].ToString());
                        string bills_pays_returns_value = table.Rows[0][13].ToString();
                        double bills_pays_returns_realvalue = Convert.ToDouble(table.Rows[0][14].ToString());



                        return  new Report_Buys_YearRange_ReportDetail(yearno, bills_count, bills_clause_count, amountin, amountremain, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_realvalue, bills_pays_realvalue, bills_itemsout_value
                            , bills_itemsout_realvalue, bills_pays_returns_value, bills_pays_returns_realvalue);

                    }
                    else return null;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  المشتريات  لسنة   "+ year , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<Report_Buys_YearRange_ReportDetail> Get_Report_Buys_YearRange_ReportDetail(int min_year, int max_year)
            {
                List<Report_Buys_YearRange_ReportDetail> List = new List<Report_Buys_YearRange_ReportDetail>();
                try
                {


                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Buys_YearRangeReportDetails_Table.Year_NO + ","
                       + Reports_Buys_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Clause_Count + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_IN + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Amounts_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Buys_MonthReportDetails_Table.Bills_Pays_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_ItemsOut_RealValue + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_Value + ","
                    + Reports_Buys_MonthReportDetails_Table.Bills_Pays_Return_RealValue
                   + " from "
                   + Reports_Buys_YearRangeReportDetails_Table.TableName
                    + "("
                   + min_year + ","
                   + max_year
                   + ")"
                   + " order by  "
                   + Reports_Buys_YearRangeReportDetails_Table.Year_NO
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int yearno = Convert.ToInt32(table.Rows[i][0].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[i][1].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int amountin = Convert.ToInt32(table.Rows[i][3].ToString());
                        int amountremain = Convert.ToInt32(table.Rows[i][4].ToString());
                        string bills_value = table.Rows[i][5].ToString();
                        string bills_pays_value = table.Rows[i][6].ToString();
                        string bills_pays_remain = table.Rows[i][7].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][8].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[i][9].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][10].ToString());
                        string bills_itemsout_value = table.Rows[i][11].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[i][12].ToString());
                        string bills_pays_returns_value = table.Rows[i][13].ToString();
                        double bills_pays_returns_realvalue = Convert.ToDouble(table.Rows[i][14].ToString());


                        Report_Buys_YearRange_ReportDetail Report_Buys_YearRange_ReportDetail_
                            = new Report_Buys_YearRange_ReportDetail(yearno , bills_count, bills_clause_count, amountin, amountremain, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_realvalue, bills_pays_realvalue, bills_itemsout_value
                            , bills_itemsout_realvalue, bills_pays_returns_value, bills_pays_returns_realvalue);
                        List.Add(Report_Buys_YearRange_ReportDetail_);
                    }
                    return List;
                }
                catch(Exception ee)
                {
                    MessageBox.Show(" فشل جلب تقرير  المشتريات  التفصيلي بين سنة "+min_year +"  و سنة " +max_year +" : "+ee.Message  , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            #endregion
            //public List<BillDayReportDetail> GetBillReport_Details_InMonth(string year, string month)
            //{
            //    List<BillDayReportDetail> BillDayReportDetailList = new List<BillDayReportDetail>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //            " select * from "
            //            + AccountBillTables.BillMonthReport_Details
            //            + "("
            //            + year + ","
            //            + month
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int dayid = Convert.ToInt32(table.Rows[i][0].ToString());
            //            DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
            //            int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
            //            string billin_Value = table.Rows[i][3].ToString();
            //            string billin_Pays_Value = table.Rows[i][4].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
            //            string billm_Value = table.Rows[i][6].ToString();
            //            string billm_Pays_Value = table.Rows[i][7].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
            //            string billout_Value = table.Rows[i][9].ToString();
            //            string billout_Pays_Value = table.Rows[i][10].ToString();

            //            BillDayReportDetail billreportdayDetail = new BillDayReportDetail(dayid, daydate, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillDayReportDetailList.Add(billreportdayDetail);
            //        }
            //        return BillDayReportDetailList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show("فشل جلب تقرير الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillDayReportDetailList;
            //    }
            //}
            //public List<BillMonthReportDetail> GetBillReport_Details_InYear(string year)
            //{
            //    List<BillMonthReportDetail> BillMonthReportDetailList = new List<BillMonthReportDetail>();
            //    try
            //    {
            //        DataTable table = DB.GetData(
            //            " select *from "
            //            + AccountBillTables.BillYearReport_Details
            //            + "("
            //            + year
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int monthid = Convert.ToInt32(table.Rows[i][0].ToString());
            //            string month = table.Rows[i][1].ToString();
            //            int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
            //            string billin_Value = table.Rows[i][3].ToString();
            //            string billin_Pays_Value = table.Rows[i][4].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
            //            string billm_Value = table.Rows[i][6].ToString();
            //            string billm_Pays_Value = table.Rows[i][7].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
            //            string billout_Value = table.Rows[i][9].ToString();
            //            string billout_Pays_Value = table.Rows[i][10].ToString();

            //            BillMonthReportDetail billmonthreportdetail = new BillMonthReportDetail(monthid, month, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillMonthReportDetailList.Add(billmonthreportdetail);
            //        }
            //        return BillMonthReportDetailList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show("فشل جلب تقرير السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillMonthReportDetailList;
            //    }
            //}
            //public List<BillYearReportDetail> GetBillReport_Details_InYearRange(string year1, string year2)
            //{
            //    List<BillYearReportDetail> BillYearReportDetailList = new List<BillYearReportDetail>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //            " select * from "
            //            + AccountBillTables.BillYearRangeReport_Details
            //            + "("
            //            + year1 + ","
            //            + year2
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int year = Convert.ToInt32(table.Rows[i][0].ToString());
            //            int billin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
            //            string billin_Value = table.Rows[i][2].ToString();
            //            string billin_Pays_Value = table.Rows[i][3].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][4].ToString());
            //            string billm_Value = table.Rows[i][5].ToString();
            //            string billm_Pays_Value = table.Rows[i][6].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][7].ToString());
            //            string billout_Value = table.Rows[i][8].ToString();
            //            string billout_Pays_Value = table.Rows[i][9].ToString();

            //            BillYearReportDetail BillYearReportDetail_ = new BillYearReportDetail(year, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillYearReportDetailList.Add(BillYearReportDetail_);
            //        }
            //        return BillYearReportDetailList;
            //    }
            //    catch (Exception ee)
            //    {
            //        MessageBox.Show(ee.Message);
            //        MessageBox.Show("فشل جلب تقرير السنوات التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillYearReportDetailList;
            //    }
            //}
        }
        public class ReportPayOrdersSQL
        {
            DatabaseInterface DB;
            public ReportPayOrdersSQL(DatabaseInterface db)
            {
                DB = db;

            }


            #region PayOrderReports
            public static class Reports_PayOrders_DayReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Employee_GetDayReport_Details]";
                public const string PayOrderTime = "PayOrderTime";
                public const string PayOrderType = "PayOrderType";
                public const string PayOrderID = "PayOrderID";
                public const string PayOrderDesc = "PayOrderDesc";
                public const string EmployeeID = "EmployeeID";
                public const string EmployeeName = "EmployeeName";
                public const string Value_ = "Value_";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";

                public const string PaysAmount = "PaysAmount";
                public const string PaysRemain = "PaysRemain";

                public const string RealValue = "RealValue";
                public const string RealPays = "RealPays";
               }
            public static class Reports_PayOrders_MonthReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Employee_GetMonthReport_Details]";
                public const string DateDayNo = "DateDayNo";
                public const string Date_day = "Date_day";
                public const string Salary_PayOrders_Count = "Salary_PayOrders_Count";
                public const string Other_PayOrders_Count = "Other_PayOrders_Count";
                public const string PayOrders_Value = "PayOrders_Value";
                public const string PayOrders_Pays_Value = "PayOrders_Pays_Value";
                public const string PayOrders_Pays_Remain = "PayOrders_Pays_Remain";
                public const string PayOrders_Pays_Remain_UPON_PayOrdersCurrency = "PayOrders_Pays_Remain_UPON_PayOrdersCurrency";
                public const string PayOrders_RealValue = "PayOrders_RealValue";
                public const string PayOrders_Pays_RealValue = "PayOrders_Pays_RealValue";

            }
            public static class Reports_PayOrders_YearReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Employee_GetYearReport_Details]";
                public const string Year_Month = "Year_Month";
                public const string Year_Month_Name = "Year_Month_Name";
                public const string Salary_PayOrders_Count = "Salary_PayOrders_Count";
                public const string Other_PayOrders_Count = "Other_PayOrders_Count";
                public const string PayOrders_Value = "PayOrders_Value";
                public const string PayOrders_Pays_Value = "PayOrders_Pays_Value";
                public const string PayOrders_Pays_Remain = "PayOrders_Pays_Remain";
                public const string PayOrders_Pays_Remain_UPON_PayOrdersCurrency = "PayOrders_Pays_Remain_UPON_PayOrdersCurrency";
                public const string PayOrders_RealValue = "PayOrders_RealValue";
                public const string PayOrders_Pays_RealValue = "PayOrders_Pays_RealValue";

            }
            public static class Reports_PayOrders_YearRangeReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Employee_GetYearRangeReport_Details]";
                public const string Year_NO = "Year_NO";
                public const string Salary_PayOrders_Count = "Salary_PayOrders_Count";
                public const string Other_PayOrders_Count = "Other_PayOrders_Count";
                public const string PayOrders_Value = "PayOrders_Value";
                public const string PayOrders_Pays_Value = "PayOrders_Pays_Value";
                public const string PayOrders_Pays_Remain = "PayOrders_Pays_Remain";
                public const string PayOrders_Pays_Remain_UPON_PayOrdersCurrency = "PayOrders_Pays_Remain_UPON_PayOrdersCurrency";
                public const string PayOrders_RealValue = "PayOrders_RealValue";
                public const string PayOrders_Pays_RealValue = "PayOrders_Pays_RealValue";


            }
            public List<Report_PayOrders_Day_ReportDetail> Get_Report_PayOrders_Day_ReportDetail(int year, int month, int day)
            {
                List<Report_PayOrders_Day_ReportDetail> List = new List<Report_PayOrders_Day_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_PayOrders_DayReportDetails_Table.PayOrderTime + ","
                   + Reports_PayOrders_DayReportDetails_Table.PayOrderType + ","
                   + Reports_PayOrders_DayReportDetails_Table.PayOrderID  + ","
                   + Reports_PayOrders_DayReportDetails_Table.PayOrderDesc  + ","
                   + Reports_PayOrders_DayReportDetails_Table.EmployeeID + ","
                    + Reports_PayOrders_DayReportDetails_Table.EmployeeName + ","
                    + Reports_PayOrders_DayReportDetails_Table.Value_  + ","
                   + Reports_PayOrders_DayReportDetails_Table.CurrencyID + ","
                   + Reports_PayOrders_DayReportDetails_Table.ExchangeRate + ","
                   + Reports_PayOrders_DayReportDetails_Table.PaysAmount + ","
                   + Reports_PayOrders_DayReportDetails_Table.PaysRemain + ","
                   + Reports_PayOrders_DayReportDetails_Table.RealValue + ","
                   + Reports_PayOrders_DayReportDetails_Table.RealPays 
                   + " from "
                   + Reports_PayOrders_DayReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month + ","
                   + day
                   + ")"
                   + " order by  "
                   + Reports_PayOrders_DayReportDetails_Table.PayOrderTime
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime payordertime = Convert.ToDateTime(table.Rows[i][0].ToString());
                        bool payordertype = Convert.ToBoolean (table.Rows[i][1].ToString());

                        uint payorderid = Convert.ToUInt32(table.Rows[i][2].ToString());
                        string payorderdesc = table.Rows[i][3].ToString();
                        uint employeeid = Convert.ToUInt32(table.Rows[i][4].ToString());
                        string employeename = table.Rows[i][5].ToString();

                        double payordervalue = Convert.ToDouble(table.Rows[i][6].ToString());
                        Currency currency = new CurrencySQL(DB)
                            .GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][7].ToString()));
                        double exchangerate = Convert.ToDouble(table.Rows[i][8].ToString());
                        string PaysAmount = table.Rows[i][9].ToString();
                        double paysremain = Convert.ToDouble(table.Rows[i][10].ToString()); ;
                        double Payorder_realvalue = Convert.ToDouble(table.Rows[i][11].ToString()); ;
                        double Payorder_Pays_realvalue = Convert.ToDouble(table.Rows[i][12].ToString()); ;

                        
                        Report_PayOrders_Day_ReportDetail Report_PayOrders_Day_ReportDetail_
                            = new Report_PayOrders_Day_ReportDetail(payordertime,payordertype ,payorderid 
                            ,payorderdesc ,employeeid ,employeename,payordervalue  ,currency ,exchangerate 
                            ,PaysAmount,paysremain ,Payorder_realvalue,Payorder_Pays_realvalue);
                        List.Add(Report_PayOrders_Day_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  أوامر صرف اليوم التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_PayOrders_Month_ReportDetail Get_Report_PayOrders_Day_Report(int year, int month, int day)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_PayOrders_MonthReportDetails_Table.DateDayNo + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Date_day + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Salary_PayOrders_Count  + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Other_PayOrders_Count  + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Value + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Value + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain_UPON_PayOrdersCurrency + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_RealValue + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_RealValue 

                   + " from "
                   + Reports_PayOrders_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month
                   + ")"
                   + " where  "
                   + Reports_PayOrders_MonthReportDetails_Table.DateDayNo + "=" + day
                   );
                    if (table.Rows.Count == 1)
                    {
                        int dayno = Convert.ToInt32(table.Rows[0][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[0][1].ToString());
                        int salarys_payorder_Count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int others_payorder_Count = Convert.ToInt32(table.Rows[0][3].ToString());
                        string payorders_value = table.Rows[0][4].ToString();
                        string payorders_pays_value = table.Rows[0][5].ToString();
                        string payorders_pays_remain = table.Rows[0][6].ToString();
                        double payorders_pays_remain_upon_payordercurrency = Convert.ToDouble(table.Rows[0][7].ToString());
                        double payorders_realvalue = Convert.ToDouble(table.Rows[0][8].ToString());
                        double payorders_pays_realvalue = Convert.ToDouble(table.Rows[0][9].ToString());



                        Report_PayOrders_Month_ReportDetail Report_PayOrders_Month_ReportDetail_
                            = new Report_PayOrders_Month_ReportDetail(dayno, daydate, salarys_payorder_Count, others_payorder_Count, payorders_value
                            , payorders_pays_value, payorders_pays_remain
                            , payorders_pays_remain_upon_payordercurrency, payorders_realvalue, payorders_pays_realvalue);
                        return Report_PayOrders_Month_ReportDetail_;
                    }
                    else return null;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  المشتريات اليوم ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<Report_PayOrders_Month_ReportDetail> Get_Report_PayOrders_Month_ReportDetail(int year, int month)
            {
                List<Report_PayOrders_Month_ReportDetail> List = new List<Report_PayOrders_Month_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_PayOrders_MonthReportDetails_Table.DateDayNo + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Date_day + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Salary_PayOrders_Count + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Other_PayOrders_Count + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Value + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Value + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain_UPON_PayOrdersCurrency + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_RealValue + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_RealValue

                   + " from "
                   + Reports_PayOrders_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month
                   + ")"
                   + " order by  "
                   + Reports_PayOrders_MonthReportDetails_Table.DateDayNo
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int dayno = Convert.ToInt32(table.Rows[i][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
                        int salarys_payorder_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int others_payorder_Count = Convert.ToInt32(table.Rows[i][3].ToString());
                        string payorders_value = table.Rows[i][4].ToString();
                        string payorders_pays_value = table.Rows[i][5].ToString();
                        string payorders_pays_remain = table.Rows[i][6].ToString();
                        double payorders_pays_remain_upon_payordercurrency = Convert.ToDouble(table.Rows[i][7].ToString());
                        double payorders_realvalue = Convert.ToDouble(table.Rows[i][8].ToString());
                        double payorders_pays_realvalue = Convert.ToDouble(table.Rows[i][9].ToString());
                      
                        Report_PayOrders_Month_ReportDetail Report_PayOrders_Month_ReportDetail_
                            = new Report_PayOrders_Month_ReportDetail(dayno, daydate, salarys_payorder_Count ,others_payorder_Count ,payorders_value
                            ,payorders_pays_value,payorders_pays_remain 
                            , payorders_pays_remain_upon_payordercurrency, payorders_realvalue, payorders_pays_realvalue);
                        List.Add(Report_PayOrders_Month_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_PayOrders_Year_ReportDetail Get_Report_PayOrders_Month_Report(int year, int month)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_PayOrders_YearReportDetails_Table.Year_Month + ","
                   + Reports_PayOrders_YearReportDetails_Table.Year_Month_Name + ","
                 + Reports_PayOrders_MonthReportDetails_Table.Salary_PayOrders_Count + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Other_PayOrders_Count + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Value + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Value + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain_UPON_PayOrdersCurrency + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_RealValue + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_RealValue
                   + " from "
                   + Reports_PayOrders_YearReportDetails_Table.TableName
                    + "("
                   + year
                   + ")"
                   + " where  "
                   + Reports_PayOrders_YearReportDetails_Table.Year_Month + "=" + month
                   );
                    if (table.Rows.Count == 1)
                    {
                        int monthno = Convert.ToInt32(table.Rows[0][0].ToString());
                        string monthname = table.Rows[0][1].ToString();
                        int salarys_payorder_Count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int others_payorder_Count = Convert.ToInt32(table.Rows[0][3].ToString());
                        string payorders_value = table.Rows[0][4].ToString();
                        string payorders_pays_value = table.Rows[0][5].ToString();
                        string payorders_pays_remain = table.Rows[0][6].ToString();
                        double payorders_pays_remain_upon_payordercurrency = Convert.ToDouble(table.Rows[0][7].ToString());
                        double payorders_realvalue = Convert.ToDouble(table.Rows[0][8].ToString());
                        double payorders_pays_realvalue = Convert.ToDouble(table.Rows[0][9].ToString());

                        return new Report_PayOrders_Year_ReportDetail(monthno, monthname, 
                            salarys_payorder_Count, others_payorder_Count, payorders_value
                            , payorders_pays_value, payorders_pays_remain
                            , payorders_pays_remain_upon_payordercurrency, payorders_realvalue, payorders_pays_realvalue);
            

                    }
                    else return null;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات السنة ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<Report_PayOrders_Year_ReportDetail> Get_Report_PayOrders_Year_ReportDetail(int year)
            {
                List<Report_PayOrders_Year_ReportDetail> List = new List<Report_PayOrders_Year_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_PayOrders_YearReportDetails_Table.Year_Month + ","
                   + Reports_PayOrders_YearReportDetails_Table.Year_Month_Name + ","
                     + Reports_PayOrders_MonthReportDetails_Table.Salary_PayOrders_Count + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Other_PayOrders_Count + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Value + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Value + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain_UPON_PayOrdersCurrency + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_RealValue + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_RealValue
                   + " from "
                   + Reports_PayOrders_YearReportDetails_Table.TableName
                    + "("
                   + year
                   + ")"
                   + " order by  "
                   + Reports_PayOrders_YearReportDetails_Table.Year_Month
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int monthno = Convert.ToInt32(table.Rows[i][0].ToString());
                        string monthname = table.Rows[i][1].ToString();
                        int salarys_payorder_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int others_payorder_Count = Convert.ToInt32(table.Rows[i][3].ToString());
                        string payorders_value = table.Rows[i][4].ToString();
                        string payorders_pays_value = table.Rows[i][5].ToString();
                        string payorders_pays_remain = table.Rows[i][6].ToString();
                        double payorders_pays_remain_upon_payordercurrency = Convert.ToDouble(table.Rows[i][7].ToString());
                        double payorders_realvalue = Convert.ToDouble(table.Rows[i][8].ToString());
                        double payorders_pays_realvalue = Convert.ToDouble(table.Rows[i][9].ToString());

                        Report_PayOrders_Year_ReportDetail Report_PayOrders_Year_ReportDetail_
                            = new Report_PayOrders_Year_ReportDetail(monthno, monthname, salarys_payorder_Count, others_payorder_Count, payorders_value
                            , payorders_pays_value, payorders_pays_remain
                            , payorders_pays_remain_upon_payordercurrency, payorders_realvalue, payorders_pays_realvalue);

                        List.Add(Report_PayOrders_Year_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_PayOrders_YearRange_ReportDetail Get_Report_PayOrders_Year_Report(int year)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select  "
                   + Reports_PayOrders_YearRangeReportDetails_Table.Year_NO + ","
                         + Reports_PayOrders_MonthReportDetails_Table.Salary_PayOrders_Count + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Other_PayOrders_Count + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Value + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Value + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain_UPON_PayOrdersCurrency + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_RealValue + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_RealValue
                   + " from "
                   + Reports_PayOrders_YearRangeReportDetails_Table.TableName
                    + "("
                   + (year - 5) + ","
                   + (year + 5)
                   + ")"
                   + " where  "
                   + Reports_PayOrders_YearRangeReportDetails_Table.Year_NO + "=" + year
                   );
                    if (table.Rows.Count == 1)
                    {
                        int yearno = Convert.ToInt32(table.Rows[0][0].ToString());
                        int salarys_payorder_Count = Convert.ToInt32(table.Rows[0][1].ToString());
                        int others_payorder_Count = Convert.ToInt32(table.Rows[0][2].ToString());
                        string payorders_value = table.Rows[0][3].ToString();
                        string payorders_pays_value = table.Rows[0][4].ToString();
                        string payorders_pays_remain = table.Rows[0][5].ToString();
                        double payorders_pays_remain_upon_payordercurrency = Convert.ToDouble(table.Rows[0][6].ToString());
                        double payorders_realvalue = Convert.ToDouble(table.Rows[0][7].ToString());
                        double payorders_pays_realvalue = Convert.ToDouble(table.Rows[0][8].ToString());


                        return new Report_PayOrders_YearRange_ReportDetail(yearno, salarys_payorder_Count, others_payorder_Count, payorders_value
                            , payorders_pays_value, payorders_pays_remain
                            , payorders_pays_remain_upon_payordercurrency, payorders_realvalue, payorders_pays_realvalue);

                    }
                    else return null;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  المشتريات  لسنة   " + year, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<Report_PayOrders_YearRange_ReportDetail> Get_Report_PayOrders_YearRange_ReportDetail(int min_year, int max_year)
            {
                List<Report_PayOrders_YearRange_ReportDetail> List = new List<Report_PayOrders_YearRange_ReportDetail>();
                try
                {


                    DataTable table = DB.GetData(
                   " select "
                   + Reports_PayOrders_YearRangeReportDetails_Table.Year_NO + ","
                        + Reports_PayOrders_MonthReportDetails_Table.Salary_PayOrders_Count + ","
                   + Reports_PayOrders_MonthReportDetails_Table.Other_PayOrders_Count + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Value + ","
                    + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Value + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_Remain_UPON_PayOrdersCurrency + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_RealValue + ","
                   + Reports_PayOrders_MonthReportDetails_Table.PayOrders_Pays_RealValue
                   + " from "
                   + Reports_PayOrders_YearRangeReportDetails_Table.TableName
                    + "("
                   + min_year + ","
                   + max_year
                   + ")"
                   + " order by  "
                   + Reports_PayOrders_YearRangeReportDetails_Table.Year_NO
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int yearno = Convert.ToInt32(table.Rows[i][0].ToString());
                        int salarys_payorder_Count = Convert.ToInt32(table.Rows[i][1].ToString());
                        int others_payorder_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        string payorders_value = table.Rows[i][3].ToString();
                        string payorders_pays_value = table.Rows[i][4].ToString();
                        string payorders_pays_remain = table.Rows[i][5].ToString();
                        double payorders_pays_remain_upon_payordercurrency = Convert.ToDouble(table.Rows[i][6].ToString());
                        double payorders_realvalue = Convert.ToDouble(table.Rows[i][7].ToString());
                        double payorders_pays_realvalue = Convert.ToDouble(table.Rows[i][8].ToString());

                        Report_PayOrders_YearRange_ReportDetail Report_PayOrders_YearRange_ReportDetail_
                            = new Report_PayOrders_YearRange_ReportDetail(yearno, salarys_payorder_Count, others_payorder_Count, payorders_value
                            , payorders_pays_value, payorders_pays_remain
                            , payorders_pays_remain_upon_payordercurrency, payorders_realvalue, payorders_pays_realvalue);
                        List.Add(Report_PayOrders_YearRange_ReportDetail_);
                    }
                    return List;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(" فشل جلب تقرير  المشتريات  التفصيلي بين سنة " + min_year + "  و سنة " + max_year + " : " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            #endregion
            //public List<BillDayReportDetail> GetBillReport_Details_InMonth(string year, string month)
            //{
            //    List<BillDayReportDetail> BillDayReportDetailList = new List<BillDayReportDetail>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //            " select * from "
            //            + AccountBillTables.BillMonthReport_Details
            //            + "("
            //            + year + ","
            //            + month
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int dayid = Convert.ToInt32(table.Rows[i][0].ToString());
            //            DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
            //            int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
            //            string billin_Value = table.Rows[i][3].ToString();
            //            string billin_Pays_Value = table.Rows[i][4].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
            //            string billm_Value = table.Rows[i][6].ToString();
            //            string billm_Pays_Value = table.Rows[i][7].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
            //            string billout_Value = table.Rows[i][9].ToString();
            //            string billout_Pays_Value = table.Rows[i][10].ToString();

            //            BillDayReportDetail billreportdayDetail = new BillDayReportDetail(dayid, daydate, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillDayReportDetailList.Add(billreportdayDetail);
            //        }
            //        return BillDayReportDetailList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show("فشل جلب تقرير الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillDayReportDetailList;
            //    }
            //}
            //public List<BillMonthReportDetail> GetBillReport_Details_InYear(string year)
            //{
            //    List<BillMonthReportDetail> BillMonthReportDetailList = new List<BillMonthReportDetail>();
            //    try
            //    {
            //        DataTable table = DB.GetData(
            //            " select *from "
            //            + AccountBillTables.BillYearReport_Details
            //            + "("
            //            + year
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int monthid = Convert.ToInt32(table.Rows[i][0].ToString());
            //            string month = table.Rows[i][1].ToString();
            //            int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
            //            string billin_Value = table.Rows[i][3].ToString();
            //            string billin_Pays_Value = table.Rows[i][4].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
            //            string billm_Value = table.Rows[i][6].ToString();
            //            string billm_Pays_Value = table.Rows[i][7].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
            //            string billout_Value = table.Rows[i][9].ToString();
            //            string billout_Pays_Value = table.Rows[i][10].ToString();

            //            BillMonthReportDetail billmonthreportdetail = new BillMonthReportDetail(monthid, month, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillMonthReportDetailList.Add(billmonthreportdetail);
            //        }
            //        return BillMonthReportDetailList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show("فشل جلب تقرير السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillMonthReportDetailList;
            //    }
            //}
            //public List<BillYearReportDetail> GetBillReport_Details_InYearRange(string year1, string year2)
            //{
            //    List<BillYearReportDetail> BillYearReportDetailList = new List<BillYearReportDetail>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //            " select * from "
            //            + AccountBillTables.BillYearRangeReport_Details
            //            + "("
            //            + year1 + ","
            //            + year2
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int year = Convert.ToInt32(table.Rows[i][0].ToString());
            //            int billin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
            //            string billin_Value = table.Rows[i][2].ToString();
            //            string billin_Pays_Value = table.Rows[i][3].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][4].ToString());
            //            string billm_Value = table.Rows[i][5].ToString();
            //            string billm_Pays_Value = table.Rows[i][6].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][7].ToString());
            //            string billout_Value = table.Rows[i][8].ToString();
            //            string billout_Pays_Value = table.Rows[i][9].ToString();

            //            BillYearReportDetail BillYearReportDetail_ = new BillYearReportDetail(year, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillYearReportDetailList.Add(BillYearReportDetail_);
            //        }
            //        return BillYearReportDetailList;
            //    }
            //    catch (Exception ee)
            //    {
            //        MessageBox.Show(ee.Message);
            //        MessageBox.Show("فشل جلب تقرير السنوات التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillYearReportDetailList;
            //    }
            //}
        }
        public class ReportSellsSQL
        {
            DatabaseInterface DB;
            //public static class AccountOperationReport_Columns
            //{
            //    public const string CurrencyID = "CurrencyID";
            //    public const string Operations_Count = "Operation_Count";
            //    public const string Operations_Value = "Operation_Value";
            //    public const string Operations_Pays_Amount = "Operation_Pays_Amount";
            //    public const string Operations_PaysValue_UPON_OperationCurrency = "Operations_PaysValue_UPON_OperationCurrency";

            //}
            //public static class AccountOperationReport_Tables
            //{
            //    public const string OperationDayReport = "[dbo].[Operation_Get_DayReport]";
            //    public const string OperationMonthReport = "[dbo].[Operation_Get_MonthReport]";
            //    public const string OperationYearReport = "[dbo].[Operation_Get_YearReport]";
            //    public const string OperationYearRangeReport = "[dbo].[Operation_Get_YearRangeReport]";

            //}
            //public static class AccountOperation_DayReportDetails_Table
            //{
            //    public const string TableName = "[dbo].[Operation_GetDayReport_Details]";
            //    public const string Operation_Time = "Operation_Time";
            //    public const string Operation_ID = "Operation_ID";
            //    public const string Operation_Owner = "Operation_Owner";
            //    public const string Operation_Info = "Operation_Info";
            //    public const string Operation_Value = "Operation_Value";
            //    public const string CurrencyID = "CurrencyID";
            //    public const string ExchangeRate = "ExchangeRate";
            //    public const string Pays_Count = "Pays_Count";
            //    public const string Pays_Amount = "Pays_Amount";
            //    public const string Pays_Values_UPON_OperationCurrency = "Pays_Values_UPON_OperationCurrency";
            //    public const string RealPaysValue = "RealPaysValue";
            //}

            public ReportSellsSQL(DatabaseInterface db)
            {
                DB = db;

            }

            //public List<OperationCurrencyReport> GetOperationReport_InDay(string year, string month, string day)
            //{
            //    List<OperationCurrencyReport> OperationCurrencyReportList = new List<OperationCurrencyReport>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //            " select "
            //            + AccountOperationReport_Columns.CurrencyID +","
            //            + AccountOperationReport_Columns.Operation_Count + ","
            //            + AccountOperationReport_Columns.Operation_Value + ","
            //            + AccountOperationReport_Columns.Operation_Pays_Amount + ","
            //            + AccountOperationReport_Columns.Operation_Remain
            //            + " from "
            //            + AccountOperationReport_Tables.OperationDayReport
            //            + "("
            //            + year + ","
            //            + month + ","
            //            + day
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            Currency Currency_ =new CurrencySQL (DB).GetCurrencyINFO_ByID ( Convert.ToUInt32(table.Rows[i][0].ToString()));
            //            int Operationin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
            //            double Operationin_Value = Convert.ToDouble(table.Rows[i][2].ToString());
            //            string  Operationin_Pays_amount = table.Rows[i][3].ToString();
            //            double remain = Convert.ToDouble (table.Rows[i][4].ToString());
            //             OperationCurrencyReport OperationreportDetail =
            //                new OperationCurrencyReport(Currency_, Operationin_Count, Operationin_Value
            //                , Operationin_Pays_amount, remain);

            //            OperationCurrencyReportList.Add(OperationreportDetail);
            //        }
            //        return OperationCurrencyReportList;
            //    }
            //    catch(Exception ee)
            //    {
            //        MessageBox.Show("GetOperationReport_InDay:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return OperationCurrencyReportList;
            //    }
            //}
            //public List<OperationCurrencyReport> GetOperationReport_InMonth(string year, string month)
            //{
            //    List<OperationCurrencyReport> OperationCurrencyReportList = new List<OperationCurrencyReport>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //          " select "
            //          + AccountOperationReport_Columns.CurrencyID + ","
            //          + AccountOperationReport_Columns.Operation_Count + ","
            //          + AccountOperationReport_Columns.Operation_Value + ","
            //          + AccountOperationReport_Columns.Operation_Pays_Amount + ","
            //          + AccountOperationReport_Columns.Operation_Remain
            //          + " from "
            //          + AccountOperationReport_Tables.OperationMonthReport
            //          + "("
            //            + year + ","
            //            + month
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][0].ToString()));
            //            int Operationin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
            //            double Operationin_Value = Convert.ToDouble(table.Rows[i][2].ToString());
            //            string Operationin_Pays_amount = table.Rows[i][3].ToString();
            //            double remain = Convert.ToDouble(table.Rows[i][4].ToString());
            //            OperationCurrencyReport OperationreportDetail =
            //               new OperationCurrencyReport(Currency_, Operationin_Count, Operationin_Value
            //               , Operationin_Pays_amount, remain);

            //            OperationCurrencyReportList.Add(OperationreportDetail);
            //        }
            //        return OperationCurrencyReportList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return OperationCurrencyReportList;
            //    }
            //}
            //public List<OperationCurrencyReport> GetOperationReport_InYear(string year)
            //{
            //    List<OperationCurrencyReport> OperationCurrencyReportList = new List<OperationCurrencyReport>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //          " select "
            //          + AccountOperationReport_Columns.CurrencyID + ","
            //          + AccountOperationReport_Columns.Operation_Count + ","
            //          + AccountOperationReport_Columns.Operation_Value + ","
            //          + AccountOperationReport_Columns.Operation_Pays_Amount + ","
            //          + AccountOperationReport_Columns.Operation_Remain
            //          + " from "
            //          + AccountOperationReport_Tables.OperationYearReport
            //            + "("
            //            + year
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][0].ToString()));
            //            int Operationin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
            //            double Operationin_Value = Convert.ToDouble(table.Rows[i][2].ToString());
            //            string Operationin_Pays_amount = table.Rows[i][3].ToString();
            //            double remain = Convert.ToDouble(table.Rows[i][4].ToString());
            //            OperationCurrencyReport OperationreportDetail =
            //               new OperationCurrencyReport(Currency_, Operationin_Count, Operationin_Value
            //               , Operationin_Pays_amount, remain);

            //            OperationCurrencyReportList.Add(OperationreportDetail);
            //        }
            //        return OperationCurrencyReportList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return OperationCurrencyReportList;
            //    }
            //}
            //public List<OperationCurrencyReport> GetOperationReport_BetweenTwoYears(string year1, string year2)
            //{
            //    List<OperationCurrencyReport> OperationCurrencyReportList = new List<OperationCurrencyReport>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //           " select "
            //           + AccountOperationReport_Columns.CurrencyID + ","
            //           + AccountOperationReport_Columns.Operation_Count + ","
            //           + AccountOperationReport_Columns.Operation_Value + ","
            //           + AccountOperationReport_Columns.Operation_Pays_Amount + ","
            //           + AccountOperationReport_Columns.Operation_Remain
            //           + " from "
            //           + AccountOperationReport_Tables.OperationYearRangeReport
            //             + "("
            //            + year1 + ","
            //            + year2
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][0].ToString()));
            //            int Operationin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
            //            double Operationin_Value = Convert.ToDouble(table.Rows[i][2].ToString());
            //            string Operationin_Pays_amount = table.Rows[i][3].ToString();
            //            double remain = Convert.ToDouble(table.Rows[i][4].ToString());
            //            OperationCurrencyReport OperationreportDetail =
            //               new OperationCurrencyReport(Currency_, Operationin_Count, Operationin_Value
            //               , Operationin_Pays_amount, remain);

            //            OperationCurrencyReportList.Add(OperationreportDetail);
            //        }
            //        return OperationCurrencyReportList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return OperationCurrencyReportList;
            //    }
            //}
            #region SellReports
            public static class Reports_Sells_DayReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Sell_GetDayReport_Details]";
                public const string Bill_Time = "Bill_Time";
                public const string Bill_ID = "Bill_ID";
                public const string SellType = "SellType";
                public const string Bill_Owner = "Bill_Owner";
                public const string ClauseS_Count = "ClauseS_Count";
                public const string ItemsOutValue = "ItemsOutValue";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string PaysCount = "PaysCount";
                public const string PaysAmount = "PaysAmount";
                public const string PaysRemain = "PaysRemain";
                public const string Source_ItemsIN_Cost_Details = "Source_ItemsIN_Cost_Details";
                public const string Source_ItemsIN_RealCost = "Source_ItemsIN_RealCost";
                public const string ItemOUT_RealValue = "ItemOUT_RealValue";
                public const string RealPaysValue = "RealPaysValue";
            }
            public static class Reports_Sells_MonthReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Sell_GetMonthReport_Details]";
                public const string DateDayNo = "DateDayNo";
                public const string Date_day = "Date_day";
                public const string Bills_Count = "Bills_Count";
                public const string Bills_Clause_Count = "Bills_Clause_Count";
                public const string Bills_Value = "Bills_Value";
                public const string Bills_Pays_Value = "Bills_Pays_Value";
                public const string Bills_Pays_Remain = "Bills_Pays_Remain";
                public const string Bills_Pays_Remain_UPON_BillsCurrency = "Bills_Pays_Remain_UPON_BillsCurrency";
                public const string Bills_ItemsIN_Value = "Bills_ItemsIN_Value";
                public const string Bills_ItemsIN_RealValue = "Bills_ItemsIN_RealValue";
                public const string Bills_RealValue = "Bills_RealValue";
                public const string Bills_Pays_RealValue = "Bills_Pays_RealValue";

            }
            public static class Reports_Sells_YearReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Sell_GetYearReport_Details]";
                public const string Year_Month = "Year_Month";
                public const string Year_Month_Name = "Year_Month_Name";
                public const string Bills_Count = "Bills_Count";
                public const string Bills_Clause_Count = "Bills_Clause_Count";
                public const string Bills_Value = "Bills_Value";
                public const string Bills_Pays_Value = "Bills_Pays_Value";
                public const string Bills_Pays_Remain = "Bills_Pays_Remain";
                public const string Bills_Pays_Remain_UPON_BillsCurrency = "Bills_Pays_Remain_UPON_BillsCurrency";
                public const string Bills_ItemsIN_Value = "Bills_ItemsIN_Value";
                public const string Bills_ItemsIN_RealValue = "Bills_ItemsIN_RealValue";
                public const string Bills_RealValue = "Bills_RealValue";
                public const string Bills_Pays_RealValue = "Bills_Pays_RealValue";

            }
            public static class Reports_Sells_YearRangeReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_Sell_GetYearRangeReport_Details]";
                public const string Year_NO = "Year_NO";
                public const string Bills_Count = "Bills_Count";
                public const string Bills_Clause_Count = "Bills_Clause_Count";
                public const string Bills_Value = "Bills_Value";
                public const string Bills_Pays_Value = "Bills_Pays_Value";
                public const string Bills_Pays_Remain = "Bills_Pays_Remain";
                public const string Bills_Pays_Remain_UPON_BillsCurrency = "Bills_Pays_Remain_UPON_BillsCurrency";
                public const string Bills_ItemsIN_Value = "Bills_ItemsIN_Value";
                public const string Bills_ItemsIN_RealValue = "Bills_ItemsIN_RealValue";
                public const string Bills_RealValue = "Bills_RealValue";
                public const string Bills_Pays_RealValue = "Bills_Pays_RealValue";

            }
            public List<Report_Sells_Day_ReportDetail > Get_Report_Sells_Day_ReportDetail(int year, int month, int day)
            {
                List<Report_Sells_Day_ReportDetail> List = new List<Report_Sells_Day_ReportDetail>();
                try
                {
                   
                         DataTable table = DB.GetData(
                        " select "
                        + Reports_Sells_DayReportDetails_Table.Bill_Time + ","
                        + Reports_Sells_DayReportDetails_Table.Bill_ID + ","
                        + Reports_Sells_DayReportDetails_Table.SellType + ","
                        + Reports_Sells_DayReportDetails_Table.Bill_Owner + ","
                        + Reports_Sells_DayReportDetails_Table.ClauseS_Count + ","
                        + Reports_Sells_DayReportDetails_Table.ItemsOutValue + ","
                        + Reports_Sells_DayReportDetails_Table.CurrencyID  + ","
                        + Reports_Sells_DayReportDetails_Table.ExchangeRate  + ","
                        + Reports_Sells_DayReportDetails_Table.PaysCount + ","
                        + Reports_Sells_DayReportDetails_Table.PaysAmount + ","
                        + Reports_Sells_DayReportDetails_Table.PaysRemain  + ","
                        + Reports_Sells_DayReportDetails_Table.Source_ItemsIN_Cost_Details + ","
                        + Reports_Sells_DayReportDetails_Table.Source_ItemsIN_RealCost + ","
                        + Reports_Sells_DayReportDetails_Table.ItemOUT_RealValue + ","
                        + Reports_Sells_DayReportDetails_Table.RealPaysValue 
                        + " from "
                        + Reports_Sells_DayReportDetails_Table.TableName 
                         + "("
                        + year + ","
                        + month + ","
                        + day
                        + ")"
                        + " order by  "
                        + Reports_Sells_DayReportDetails_Table.Bill_Time
                        );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime billdate = Convert.ToDateTime(table.Rows[i][0].ToString());
                        int billid = Convert.ToInt32(table.Rows[i][1].ToString());
                        string selltype = table.Rows[i][2].ToString();
                        string owner = table.Rows[i][3].ToString();
                        int clause_Count = Convert.ToInt32(table.Rows[i][4].ToString());
                        double items_outvalue = Convert.ToDouble(table.Rows[i][5].ToString());
                        Currency currency =new CurrencySQL (DB)
                            .GetCurrencyINFO_ByID (Convert.ToUInt32 ( table.Rows[i][6].ToString()));
                        double exchangerate = Convert.ToDouble(table.Rows[i][7].ToString());
                        int PaysCount = Convert.ToInt32(table.Rows[i][8].ToString());
                        string PaysAmount = table.Rows[i][9].ToString();
                        double remain= Convert.ToDouble(table.Rows[i][10].ToString()); ;
                        string bill_source_itemsin_cost = table.Rows[i][11].ToString();
                        double bill_source_itemsin_realcost= Convert.ToDouble(table.Rows[i][12].ToString());
                        double itemsout_realvalue= Convert.ToDouble(table.Rows[i][13].ToString());
                        double real_pays = Convert.ToDouble(table.Rows[i][14].ToString());

                        Report_Sells_Day_ReportDetail Report_Sells_Day_ReportDetail_
                            = new Report_Sells_Day_ReportDetail(billdate , billid ,selltype ,owner ,
                            clause_Count,items_outvalue ,currency ,exchangerate ,PaysCount ,PaysAmount ,remain 
                            ,bill_source_itemsin_cost,bill_source_itemsin_realcost ,itemsout_realvalue ,real_pays );
                        List .Add(Report_Sells_Day_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات اليوم التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_Sells_Month_ReportDetail Get_Report_Sells_Day_Report(int year, int month, int day)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Sells_MonthReportDetails_Table.DateDayNo + ","
                   + Reports_Sells_MonthReportDetails_Table.Date_day + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Clause_Count + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_ItemsIN_Value + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_ItemsIN_RealValue + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_RealValue

                   + " from "
                   + Reports_Sells_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month
                   + ")"
                   + " where  "
                   + Reports_Sells_MonthReportDetails_Table.DateDayNo+"="+day 
                   );
                    if(table.Rows .Count ==1)
                    {
                        int dayno = Convert.ToInt32(table.Rows[0][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[0][1].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[0][3].ToString());

                        string bills_value = table.Rows[0][4].ToString();
                        string bills_pays_value = table.Rows[0][5].ToString();
                        string bills_pays_remain = table.Rows[0][6].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][7].ToString());
                        string bills_itemsin_value = table.Rows[0][8].ToString();
                        double bills_itemsin_realvalue = Convert.ToDouble(table.Rows[0][9].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][11].ToString());



                        Report_Sells_Month_ReportDetail Report_Sells_Month_ReportDetail_
                            = new Report_Sells_Month_ReportDetail(dayno, daydate, bills_count, bills_clause_count, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsin_value, bills_itemsin_realvalue, bills_realvalue, bills_pays_realvalue);
                        return Report_Sells_Month_ReportDetail_;
                    }
                    else return null ;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات اليوم ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List <Report_Sells_Month_ReportDetail> Get_Report_Sells_Month_ReportDetail(int year, int month)
            {
                List<Report_Sells_Month_ReportDetail> List = new List<Report_Sells_Month_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Sells_MonthReportDetails_Table.DateDayNo  + ","
                   + Reports_Sells_MonthReportDetails_Table.Date_day + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Count + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Clause_Count + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Value + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_ItemsIN_Value + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_ItemsIN_RealValue + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_RealValue + ","
                   + Reports_Sells_MonthReportDetails_Table.Bills_Pays_RealValue 

                   + " from "
                   + Reports_Sells_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month 
                   + ")"
                   + " order by  "
                   + Reports_Sells_MonthReportDetails_Table.DateDayNo
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int dayno = Convert.ToInt32(table.Rows[i][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[i][3].ToString());

                        string bills_value = table.Rows[i][4].ToString();
                        string bills_pays_value = table.Rows[i][5].ToString();
                        string bills_pays_remain = table.Rows[i][6].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][7].ToString());
                        string bills_itemsin_value = table.Rows[i][8].ToString();
                        double bills_itemsin_realvalue = Convert.ToDouble(table.Rows[i][9].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[i][10].ToString());
                        double bills_pays_realvalue= Convert.ToDouble(table.Rows[i][11].ToString());



                        Report_Sells_Month_ReportDetail Report_Sells_Month_ReportDetail_
                            = new Report_Sells_Month_ReportDetail(dayno,daydate,bills_count,bills_clause_count,bills_value 
                            ,bills_pays_value,bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsin_value,bills_itemsin_realvalue,bills_realvalue ,bills_pays_realvalue );
                        List.Add(Report_Sells_Month_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_Sells_Year_ReportDetail Get_Report_Sells_Month_Report(int year, int month)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Sells_YearReportDetails_Table.Year_Month + ","
                   + Reports_Sells_YearReportDetails_Table.Year_Month_Name + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Count + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Clause_Count + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Value + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_ItemsIN_Value + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_ItemsIN_RealValue + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_RealValue + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_RealValue
                   + " from "
                   + Reports_Sells_YearReportDetails_Table.TableName
                    + "("
                   + year
                   + ")"
                   + " where  "
                   + Reports_Sells_YearReportDetails_Table.Year_Month+"="+month 
                   );
                    if(table.Rows.Count==1)
                    {
                        int monthno = Convert.ToInt32(table.Rows[0][0].ToString());
                        string monthname = table.Rows[0][1].ToString();
                        int bills_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[0][3].ToString());

                        string bills_value = table.Rows[0][4].ToString();
                        string bills_pays_value = table.Rows[0][5].ToString();
                        string bills_pays_remain = table.Rows[0][6].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][7].ToString());
                        string bills_itemsin_value = table.Rows[0][8].ToString();
                        double bills_itemsin_realvalue = Convert.ToDouble(table.Rows[0][9].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][11].ToString());



                        return new Report_Sells_Year_ReportDetail(monthno, monthname, bills_count, bills_clause_count, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsin_value, bills_itemsin_realvalue, bills_realvalue, bills_pays_realvalue);

                    }
                    else return null ;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات السنة ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
            }
            public List<Report_Sells_Year_ReportDetail> Get_Report_Sells_Year_ReportDetail(int year)
            {
                List<Report_Sells_Year_ReportDetail> List = new List<Report_Sells_Year_ReportDetail>();
                try
                {
    
                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Sells_YearReportDetails_Table.Year_Month  + ","
                   + Reports_Sells_YearReportDetails_Table.Year_Month_Name + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Count + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Clause_Count + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Value + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_ItemsIN_Value + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_ItemsIN_RealValue + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_RealValue + ","
                   + Reports_Sells_YearReportDetails_Table.Bills_Pays_RealValue
                   + " from "
                   + Reports_Sells_YearReportDetails_Table.TableName
                    + "("
                   + year
                   + ")"
                   + " order by  "
                   + Reports_Sells_YearReportDetails_Table.Year_Month 
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int monthno = Convert.ToInt32(table.Rows[i][0].ToString());
                        string  monthname = table.Rows[i][1].ToString();
                        int bills_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[i][3].ToString());

                        string bills_value = table.Rows[i][4].ToString();
                        string bills_pays_value = table.Rows[i][5].ToString();
                        string bills_pays_remain = table.Rows[i][6].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][7].ToString());
                        string bills_itemsin_value = table.Rows[i][8].ToString();
                        double bills_itemsin_realvalue = Convert.ToDouble(table.Rows[i][9].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[i][10].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][11].ToString());



                        Report_Sells_Year_ReportDetail Report_Sells_Month_ReportDetail_
                            = new Report_Sells_Year_ReportDetail(monthno, monthname, bills_count, bills_clause_count, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsin_value, bills_itemsin_realvalue, bills_realvalue, bills_pays_realvalue);
                        List.Add(Report_Sells_Month_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_Sells_YearRange_ReportDetail Get_Report_Sells_Year_Report(int year)
            {
                try
                {
                   
                    DataTable table = DB.GetData(
                   " select  "
                   + Reports_Sells_YearRangeReportDetails_Table.Year_NO + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Count + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Clause_Count + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Value + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_ItemsIN_Value + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_ItemsIN_RealValue + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_RealValue + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_RealValue
                   + " from "
                   + Reports_Sells_YearRangeReportDetails_Table.TableName
                    + "("
                   + (year -5) + ","
                   + (year +5)
                   + ")"
                   + " where  "
                   + Reports_Sells_YearRangeReportDetails_Table.Year_NO+"="+year 
                   );
                    if( table.Rows.Count==1)
                    {
                        int yearno = Convert.ToInt32(table.Rows[0][0].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[0][1].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[0][2].ToString());

                        string bills_value = table.Rows[0][3].ToString();
                        string bills_pays_value = table.Rows[0][4].ToString();
                        string bills_pays_remain = table.Rows[0][5].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][6].ToString());
                        string bills_itemsin_value = table.Rows[0][7].ToString();
                        double bills_itemsin_realvalue = Convert.ToDouble(table.Rows[0][8].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[0][9].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());



                        return new Report_Sells_YearRange_ReportDetail(yearno, bills_count, bills_clause_count, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsin_value, bills_itemsin_realvalue, bills_realvalue, bills_pays_realvalue);
                        
                    }
                    else return null ;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
            }
            public List<Report_Sells_YearRange_ReportDetail> Get_Report_Sells_YearRange_ReportDetail(int min_year,int max_year)
            {
                List<Report_Sells_YearRange_ReportDetail> List = new List<Report_Sells_YearRange_ReportDetail>();
                try
                {
     

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_Sells_YearRangeReportDetails_Table.Year_NO  + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Count + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Clause_Count + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Value + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_Value + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_Remain + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_ItemsIN_Value + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_ItemsIN_RealValue + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_RealValue + ","
                   + Reports_Sells_YearRangeReportDetails_Table.Bills_Pays_RealValue
                   + " from "
                   + Reports_Sells_YearRangeReportDetails_Table.TableName
                    + "("
                   + min_year+","
                   +max_year 
                   + ")"
                   + " order by  "
                   + Reports_Sells_YearRangeReportDetails_Table.Year_NO 
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int yearno = Convert.ToInt32(table.Rows[i][0].ToString());
                        int bills_count = Convert.ToInt32(table.Rows[i][1].ToString());
                        int bills_clause_count = Convert.ToInt32(table.Rows[i][2].ToString());

                        string bills_value = table.Rows[i][3].ToString();
                        string bills_pays_value = table.Rows[i][4].ToString();
                        string bills_pays_remain = table.Rows[i][5].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][6].ToString());
                        string bills_itemsin_value = table.Rows[i][7].ToString();
                        double bills_itemsin_realvalue = Convert.ToDouble(table.Rows[i][8].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[i][9].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][10].ToString());



                        Report_Sells_YearRange_ReportDetail Report_Sells_YearRange_ReportDetail_
                            = new Report_Sells_YearRange_ReportDetail(yearno , bills_count, bills_clause_count, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsin_value, bills_itemsin_realvalue, bills_realvalue, bills_pays_realvalue);
                        List.Add(Report_Sells_YearRange_ReportDetail_);
                    }
                    return List;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  مبيعات السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            #endregion
            //public List<BillDayReportDetail> GetBillReport_Details_InMonth(string year, string month)
            //{
            //    List<BillDayReportDetail> BillDayReportDetailList = new List<BillDayReportDetail>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //            " select * from "
            //            + AccountBillTables.BillMonthReport_Details
            //            + "("
            //            + year + ","
            //            + month
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int dayid = Convert.ToInt32(table.Rows[i][0].ToString());
            //            DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
            //            int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
            //            string billin_Value = table.Rows[i][3].ToString();
            //            string billin_Pays_Value = table.Rows[i][4].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
            //            string billm_Value = table.Rows[i][6].ToString();
            //            string billm_Pays_Value = table.Rows[i][7].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
            //            string billout_Value = table.Rows[i][9].ToString();
            //            string billout_Pays_Value = table.Rows[i][10].ToString();

            //            BillDayReportDetail billreportdayDetail = new BillDayReportDetail(dayid, daydate, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillDayReportDetailList.Add(billreportdayDetail);
            //        }
            //        return BillDayReportDetailList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show("فشل جلب تقرير الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillDayReportDetailList;
            //    }
            //}
            //public List<BillMonthReportDetail> GetBillReport_Details_InYear(string year)
            //{
            //    List<BillMonthReportDetail> BillMonthReportDetailList = new List<BillMonthReportDetail>();
            //    try
            //    {
            //        DataTable table = DB.GetData(
            //            " select *from "
            //            + AccountBillTables.BillYearReport_Details
            //            + "("
            //            + year
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int monthid = Convert.ToInt32(table.Rows[i][0].ToString());
            //            string month = table.Rows[i][1].ToString();
            //            int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
            //            string billin_Value = table.Rows[i][3].ToString();
            //            string billin_Pays_Value = table.Rows[i][4].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
            //            string billm_Value = table.Rows[i][6].ToString();
            //            string billm_Pays_Value = table.Rows[i][7].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
            //            string billout_Value = table.Rows[i][9].ToString();
            //            string billout_Pays_Value = table.Rows[i][10].ToString();

            //            BillMonthReportDetail billmonthreportdetail = new BillMonthReportDetail(monthid, month, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillMonthReportDetailList.Add(billmonthreportdetail);
            //        }
            //        return BillMonthReportDetailList;
            //    }
            //    catch
            //    {
            //        MessageBox.Show("فشل جلب تقرير السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillMonthReportDetailList;
            //    }
            //}
            //public List<BillYearReportDetail> GetBillReport_Details_InYearRange(string year1, string year2)
            //{
            //    List<BillYearReportDetail> BillYearReportDetailList = new List<BillYearReportDetail>();
            //    try
            //    {

            //        DataTable table = DB.GetData(
            //            " select * from "
            //            + AccountBillTables.BillYearRangeReport_Details
            //            + "("
            //            + year1 + ","
            //            + year2
            //            + ")");
            //        for (int i = 0; i < table.Rows.Count; i++)
            //        {
            //            int year = Convert.ToInt32(table.Rows[i][0].ToString());
            //            int billin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
            //            string billin_Value = table.Rows[i][2].ToString();
            //            string billin_Pays_Value = table.Rows[i][3].ToString();
            //            int billm_Count = Convert.ToInt32(table.Rows[i][4].ToString());
            //            string billm_Value = table.Rows[i][5].ToString();
            //            string billm_Pays_Value = table.Rows[i][6].ToString();
            //            int billout_Count = Convert.ToInt32(table.Rows[i][7].ToString());
            //            string billout_Value = table.Rows[i][8].ToString();
            //            string billout_Pays_Value = table.Rows[i][9].ToString();

            //            BillYearReportDetail BillYearReportDetail_ = new BillYearReportDetail(year, billin_Count, billin_Value, billin_Pays_Value
            //                , billm_Count, billm_Value, billm_Pays_Value
            //                , billout_Count, billout_Value, billout_Pays_Value);
            //            BillYearReportDetailList.Add(BillYearReportDetail_);
            //        }
            //        return BillYearReportDetailList;
            //    }
            //    catch (Exception ee)
            //    {
            //        MessageBox.Show(ee.Message);
            //        MessageBox.Show("فشل جلب تقرير السنوات التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return BillYearReportDetailList;
            //    }
            //}
        }
        public class ReportMaintenanceOPRsSQL
        {
            DatabaseInterface DB;
            public ReportMaintenanceOPRsSQL(DatabaseInterface db)
            {
                DB = db;

            }


            #region MaintenanceOPRReports
            public static class Reports_MaintenanceOPRs_DayReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_MaintenanceOPR_GetDayReport_Details]";
                public const string MaintenanceOPR_Time = "MaintenanceOPR_Time";
                public const string MaintenanceOPR_ID = "MaintenanceOPR_ID";
                public const string MaintenanceOPR_Owner = "MaintenanceOPR_Owner";
                public const string ItemID = "ItemID";
                public const string FalutDesc = "FalutDesc";
                public const string MaintenanceOPR_Endworkdate = "MaintenanceOPR_Endworkdate";
                public const string MaintenanceOPR_Rpaired = "MaintenanceOPR_Rpaired";
                public const string MaintenanceOPR_DeliverDate = "MaintenanceOPR_DeliverDate";
                public const string MaintenanceOPR_EndWarrantyDate = "MaintenanceOPR_EndWarrantyDate";
                public const string BillMaintenanceID = "BillMaintenanceID";
                public const string BillValue = "BillValue";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string PaysAmount = "PaysAmount";
                public const string PaysRemain = "PaysRemain";
                public const string Bill_ItemsOut_Value = "Bill_ItemsOut_Value";
                public const string Bill_ItemsOut_RealValue = "Bill_ItemsOut_RealValue";

                public const string Bill_RealValue = "Bill_RealValue";
                public const string Bill_Pays_RealValue = "Bill_Pays_RealValue";

            }
            public static class Reports_MaintenanceOPRs_MonthReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_MaintenanceOPR_GetMonthReport_Details]";
                public const string DateDayNo = "DateDayNo";
                public const string Date_day = "Date_day";
                public const string MaintenanceOPRs_Count = "MaintenanceOPRs_Count";
                public const string MaintenanceOPRs_EndWork_Count = "MaintenanceOPRs_EndWork_Count";
                public const string MaintenanceOPRs_Repaired_Count = "MaintenanceOPRs_Repaired_Count";
                public const string MaintenanceOPRs_Warranty_Count = "MaintenanceOPRs_Warranty_Count";
                public const string MaintenanceOPRs_Endarranty_Count = "MaintenanceOPRs_Endarranty_Count";
                public const string BillMaintenances_Count = "BillMaintenances_Count";
                public const string BillMaintenances_Value = "BillMaintenances_Value";
                public const string BillMaintenances_Pays_Value = "BillMaintenances_Pays_Value";
                public const string BillMaintenances_Pays_Remain = "BillMaintenances_Pays_Remain";

                public const string BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency = "BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency";

                public const string BillMaintenances_ItemsOut_Value = "BillMaintenances_ItemsOut_Value";
                public const string BillMaintenances_ItemsOut_RealValue = "BillMaintenances_ItemsOut_RealValue";
                public const string BillMaintenances_RealValue = "BillMaintenances_RealValue";
                public const string BillMaintenances_Pays_RealValue = "BillMaintenances_Pays_RealValue";


            }
            public static class Reports_MaintenanceOPRs_YearReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_MaintenanceOPR_GetYearReport_Details]";
                public const string Year_Month = "Year_Month";
                public const string Year_Month_Name = "Year_Month_Name";
                public const string MaintenanceOPRs_Count = "MaintenanceOPRs_Count";
                public const string MaintenanceOPRs_EndWork_Count = "MaintenanceOPRs_EndWork_Count";
                public const string MaintenanceOPRs_Repaired_Count = "MaintenanceOPRs_Repaired_Count";
                public const string MaintenanceOPRs_Warranty_Count = "MaintenanceOPRs_Warranty_Count";
                public const string MaintenanceOPRs_Endarranty_Count = "MaintenanceOPRs_Endarranty_Count";
                public const string BillMaintenances_Count = "BillMaintenances_Count";

                public const string BillMaintenances_Value = "BillMaintenances_Value";
                public const string BillMaintenances_Pays_Value = "BillMaintenances_Pays_Value";
                public const string BillMaintenances_Pays_Remain = "BillMaintenances_Pays_Remain";

                public const string BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency = "BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency";

                public const string BillMaintenances_ItemsOut_Value = "BillMaintenances_ItemsOut_Value";
                public const string BillMaintenances_ItemsOut_RealValue = "BillMaintenances_ItemsOut_RealValue";
                public const string BillMaintenances_RealValue = "BillMaintenances_RealValue";
                public const string BillMaintenances_Pays_RealValue = "BillMaintenances_Pays_RealValue";

            }
            public static class Reports_MaintenanceOPRs_YearRangeReportDetails_Table
            {

                public const string TableName = "[dbo].[Report_MaintenanceOPR_GetYearRangeReport_Details]";
                public const string Year_NO = "Year_NO";
                public const string MaintenanceOPRs_Count = "MaintenanceOPRs_Count";
                public const string MaintenanceOPRs_EndWork_Count = "MaintenanceOPRs_EndWork_Count";
                public const string MaintenanceOPRs_Repaired_Count = "MaintenanceOPRs_Repaired_Count";
                public const string MaintenanceOPRs_Warranty_Count = "MaintenanceOPRs_Warranty_Count";
                public const string MaintenanceOPRs_Endarranty_Count = "MaintenanceOPRs_Endarranty_Count";

                public const string BillMaintenances_Count = "BillMaintenances_Count";

                public const string BillMaintenances_Value = "BillMaintenances_Value";
                public const string BillMaintenances_Pays_Value = "BillMaintenances_Pays_Value";
                public const string BillMaintenances_Pays_Remain = "BillMaintenances_Pays_Remain";

                public const string BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency = "BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency";

                public const string BillMaintenances_ItemsOut_Value = "BillMaintenances_ItemsOut_Value";
                public const string BillMaintenances_ItemsOut_RealValue = "BillMaintenances_ItemsOut_RealValue";
                public const string BillMaintenances_RealValue = "BillMaintenances_RealValue";
                public const string BillMaintenances_Pays_RealValue = "BillMaintenances_Pays_RealValue";


            }
            public List<Report_MaintenanceOPRs_Day_ReportDetail> Get_Report_MaintenanceOPRs_Day_ReportDetail(int year, int month, int day)
            {
                List<Report_MaintenanceOPRs_Day_ReportDetail> List = new List<Report_MaintenanceOPRs_Day_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_Time + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_ID + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_Owner + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.ItemID + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.FalutDesc + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_Endworkdate + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_Rpaired + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_DeliverDate + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_EndWarrantyDate + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.BillMaintenanceID + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.BillValue + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.CurrencyID + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.ExchangeRate + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.PaysAmount + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.PaysRemain + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.Bill_ItemsOut_Value + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.Bill_ItemsOut_RealValue + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.Bill_RealValue + ","
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.Bill_Pays_RealValue

                   + " from "
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month + ","
                   + day
                   + ")"
                   + " order by  "
                   + Reports_MaintenanceOPRs_DayReportDetails_Table.MaintenanceOPR_Time
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime maintenanceopr_time = Convert.ToDateTime(table.Rows[i][0].ToString());
                        int maintenance_opr_id = Convert.ToInt32(table.Rows[i][1].ToString());
                        string owner = table.Rows[i][2].ToString();
                        Item Item_ = new ItemObj.ItemObjSQL.ItemSQL(DB)
                            .GetItemInfoByID(Convert.ToUInt32(table.Rows[i][3]));
                        string faultdesc = table.Rows[i][4].ToString();



                        DateTime? endworkdate, deliverdate, endwarrantry;
                        bool? Repaired;
                        uint? BillID;
                        Currency currency;
                        double? billvalue, exchangerate, paysremain, itemsoutRealvalue, billrealvalue, bill_Pays_realvalue;
                        string PaysAmount, itemsoutvalue;


                        try
                        {
                            endworkdate = Convert.ToDateTime(table.Rows[i][5]);
                            Repaired = Convert.ToBoolean(table.Rows[i][6]);
                        }
                        catch
                        {
                            endworkdate = null;
                            Repaired = null;
                        }

                        try
                        {
                            deliverdate = Convert.ToDateTime(table.Rows[i][7]);
                        }
                        catch
                        {
                            deliverdate = null;
                        }
                        try
                        {
                            endwarrantry = Convert.ToDateTime(table.Rows[i][8]);
                        }
                        catch
                        {
                            endwarrantry = null;
                        }
                        try
                        {
                 
                            BillID = Convert.ToUInt32(table.Rows[i][9]);
                            billvalue = Convert.ToDouble(table.Rows[i][10].ToString());
                            currency = new CurrencySQL(DB)
                               .GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][11].ToString()));
                            exchangerate = Convert.ToDouble(table.Rows[i][12].ToString());
                            PaysAmount = table.Rows[i][13].ToString();
                            paysremain = Convert.ToDouble(table.Rows[i][14].ToString()); ;

                            itemsoutvalue = table.Rows[i][15].ToString();
                            itemsoutRealvalue = Convert.ToDouble(table.Rows[i][16].ToString());
                            billrealvalue = Convert.ToDouble(table.Rows[i][17].ToString()); ;
                            bill_Pays_realvalue = Convert.ToDouble(table.Rows[i][18].ToString()); ;

                        }
                        catch
                        {
                            BillID = null;
                            billvalue = null;
                            currency = null;
                            exchangerate = null;
                            PaysAmount = null;
                            paysremain = null;

                            itemsoutvalue = null;
                            itemsoutRealvalue = null;
                            billrealvalue = null;
                            bill_Pays_realvalue = null;
                        }




                        Report_MaintenanceOPRs_Day_ReportDetail Report_MaintenanceOPRs_Day_ReportDetail_
                            = new Report_MaintenanceOPRs_Day_ReportDetail(maintenanceopr_time
                            , maintenance_opr_id, owner, Item_, faultdesc, endworkdate, Repaired, deliverdate
                            , endwarrantry, BillID, billvalue, currency, exchangerate, PaysAmount, paysremain,
                            itemsoutvalue, itemsoutRealvalue, billrealvalue, bill_Pays_realvalue);
                        List.Add(Report_MaintenanceOPRs_Day_ReportDetail_);
                    }
                    return List;
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_Day_ReportDetail: "+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public Report_MaintenanceOPRs_Month_ReportDetail Get_Report_MaintenanceOPRs_Day_Report(int year, int month, int day)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.DateDayNo + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.Date_day + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_EndWork_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Repaired_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Warranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_EndWork_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_Value + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_RealValue + ","
                     + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Count 
                   + " from "
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month
                   + ")"
                   + " where  "
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.DateDayNo + "=" + day
                   );
                    if (table.Rows.Count == 1)
                    {
                        int dayno = Convert.ToInt32(table.Rows[0][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[0][1].ToString());
                        int maintenanceopr_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int maintenanceopr_endwork_count = Convert.ToInt32(table.Rows[0][3].ToString());
                        int maintenanceopr_repaired_count = Convert.ToInt32(table.Rows[0][4].ToString());
                        int maintenanceopr_warranty_count = Convert.ToInt32(table.Rows[0][5].ToString());
                        int maintenanceopr_ENDwarranty_count = Convert.ToInt32(table.Rows[0][6].ToString());
                        string bills_value = table.Rows[0][7].ToString();
                        string bills_pays_value = table.Rows[0][8].ToString();
                        string bills_pays_remain = table.Rows[0][9].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][10].ToString());
                        string bills_itemsout_value = table.Rows[0][11].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][12].ToString());

                        double bills_realvalue = Convert.ToDouble(table.Rows[0][13].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][14].ToString());
                        int billscount = Convert.ToInt32(table.Rows[0][15].ToString());


                        Report_MaintenanceOPRs_Month_ReportDetail Report_MaintenanceOPRs_Month_ReportDetail_
                            = new Report_MaintenanceOPRs_Month_ReportDetail(dayno, daydate, maintenanceopr_count, maintenanceopr_endwork_count
                            , maintenanceopr_repaired_count, maintenanceopr_warranty_count, maintenanceopr_ENDwarranty_count, billscount, bills_value, bills_pays_value
                            , bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsout_value, bills_itemsout_realvalue
                            , bills_realvalue, bills_pays_realvalue);
                        return Report_MaintenanceOPRs_Month_ReportDetail_;
                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_Day_Report: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
            }
            public List<Report_MaintenanceOPRs_Month_ReportDetail> Get_Report_MaintenanceOPRs_Month_ReportDetail(int year, int month)
            {
                List<Report_MaintenanceOPRs_Month_ReportDetail> List = new List<Report_MaintenanceOPRs_Month_ReportDetail>();
                try
                {
                   
                    DataTable table = DB.GetData(
                   " select "
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.DateDayNo + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.Date_day + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_EndWork_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Repaired_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Warranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Endarranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_Value + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Count

                   + " from "
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.TableName
                    + "("
                   + year + ","
                   + month
                   + ")"
                   + " order by  "
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.DateDayNo
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int dayno = Convert.ToInt32(table.Rows[i][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows[i][1].ToString());
                        int maintenanceopr_count = Convert.ToInt32(table.Rows[i][2].ToString());

                        int maintenanceopr_endwork_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        int maintenanceopr_repaired_count = Convert.ToInt32(table.Rows[i][4].ToString());
                        int maintenanceopr_warranty_count = Convert.ToInt32(table.Rows[i][5].ToString());
                        int maintenanceopr_ENDwarranty_count = Convert.ToInt32(table.Rows[i][6].ToString());
                        string bills_value = table.Rows[i][7].ToString();
                        string bills_pays_value = table.Rows[i][8].ToString();
                        string bills_pays_remain = table.Rows[i][9].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][10].ToString());
                        string bills_itemsout_value = table.Rows[i][11].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[i][12].ToString());

                        double bills_realvalue = Convert.ToDouble(table.Rows[i][13].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][14].ToString());
                        int billscount = Convert.ToInt32(table.Rows[0][15].ToString());


                        Report_MaintenanceOPRs_Month_ReportDetail Report_MaintenanceOPRs_Month_ReportDetail_
                            = new Report_MaintenanceOPRs_Month_ReportDetail(dayno, daydate, maintenanceopr_count, maintenanceopr_endwork_count
                            , maintenanceopr_repaired_count, maintenanceopr_warranty_count, maintenanceopr_ENDwarranty_count, billscount, bills_value, bills_pays_value
                            , bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsout_value, bills_itemsout_realvalue
                            , bills_realvalue, bills_pays_realvalue);
                        List.Add(Report_MaintenanceOPRs_Month_ReportDetail_);
                    }
                    return List;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_Month_ReportDetail: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public Report_MaintenanceOPRs_Year_ReportDetail Get_Report_MaintenanceOPRs_Month_Report(int year, int month)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.Year_Month + ","
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.Year_Month_Name + ","
                       + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_EndWork_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Repaired_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Warranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Endarranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_Value + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_RealValue + ","
                                        + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Count
                   + " from "
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.TableName
                    + "("
                   + year
                   + ")"
                   + " where  "
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.Year_Month + "=" + month
                   );
                    if (table.Rows.Count == 1)
                    {
                        int monthno = Convert.ToInt32(table.Rows[0][0].ToString());
                        string monthname = table.Rows[0][1].ToString();
                        int maintenanceopr_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int maintenanceopr_endwork_count = Convert.ToInt32(table.Rows[0][3].ToString());
                        int maintenanceopr_repaired_count = Convert.ToInt32(table.Rows[0][4].ToString());
                        int maintenanceopr_warranty_count = Convert.ToInt32(table.Rows[0][5].ToString());
                        int maintenanceopr_ENDwarranty_count = Convert.ToInt32(table.Rows[0][6].ToString());
                        string bills_value = table.Rows[0][7].ToString();
                        string bills_pays_value = table.Rows[0][8].ToString();
                        string bills_pays_remain = table.Rows[0][9].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][10].ToString());
                        string bills_itemsout_value = table.Rows[0][11].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][12].ToString());

                        double bills_realvalue = Convert.ToDouble(table.Rows[0][13].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][14].ToString());

                        int billscount = Convert.ToInt32(table.Rows[0][15].ToString());

                        return new Report_MaintenanceOPRs_Year_ReportDetail(monthno, monthname, maintenanceopr_count, maintenanceopr_endwork_count
                            , maintenanceopr_repaired_count, maintenanceopr_warranty_count, maintenanceopr_ENDwarranty_count, billscount, bills_value, bills_pays_value
                            , bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsout_value, bills_itemsout_realvalue
                            , bills_realvalue, bills_pays_realvalue);

                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_Month_Report: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<Report_MaintenanceOPRs_Year_ReportDetail> Get_Report_MaintenanceOPRs_Year_ReportDetail(int year)
            {
                List<Report_MaintenanceOPRs_Year_ReportDetail> List = new List<Report_MaintenanceOPRs_Year_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.Year_Month + ","
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.Year_Month_Name + ","
                     + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_EndWork_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Repaired_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Warranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Endarranty_Count  + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_Value + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Count
                   + " from "
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.TableName
                    + "("
                   + year
                   + ")"
                   + " order by  "
                   + Reports_MaintenanceOPRs_YearReportDetails_Table.Year_Month
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int monthno = Convert.ToInt32(table.Rows[i][0].ToString());
                        string monthname = table.Rows[i][1].ToString();
                        int maintenanceopr_count = Convert.ToInt32(table.Rows[i][2].ToString());

                        int maintenanceopr_endwork_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        int maintenanceopr_repaired_count = Convert.ToInt32(table.Rows[i][4].ToString());
                        int maintenanceopr_warranty_count = Convert.ToInt32(table.Rows[i][5].ToString());
                        int maintenanceopr_ENDwarranty_count = Convert.ToInt32(table.Rows[i][6].ToString());
                        string bills_value = table.Rows[i][7].ToString();
                        string bills_pays_value = table.Rows[i][8].ToString();
                        string bills_pays_remain = table.Rows[i][9].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][10].ToString());
                        string bills_itemsout_value = table.Rows[i][11].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[i][12].ToString());

                        double bills_realvalue = Convert.ToDouble(table.Rows[i][13].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][14].ToString());

                        int billscount = Convert.ToInt32(table.Rows[0][15].ToString());


                        Report_MaintenanceOPRs_Year_ReportDetail Report_MaintenanceOPRs_Year_ReportDetail_
                            = new Report_MaintenanceOPRs_Year_ReportDetail(monthno, monthname, maintenanceopr_count, maintenanceopr_endwork_count
                            , maintenanceopr_repaired_count, maintenanceopr_warranty_count, maintenanceopr_ENDwarranty_count, billscount, bills_value, bills_pays_value
                            , bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsout_value, bills_itemsout_realvalue
                            , bills_realvalue, bills_pays_realvalue);
                        List.Add(Report_MaintenanceOPRs_Year_ReportDetail_);
                    }
                    return List;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_Year_ReportDetail: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List  ;
                }
            }
            public Report_MaintenanceOPRs_YearRange_ReportDetail Get_Report_MaintenanceOPRs_Year_Report(int year)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select  "
                   + Reports_MaintenanceOPRs_YearRangeReportDetails_Table.Year_NO + ","
                        + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_EndWork_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Repaired_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Warranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Endarranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_Value + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_RealValue + ","
                                        + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Count
                   + " from "
                   + Reports_MaintenanceOPRs_YearRangeReportDetails_Table.TableName
                    + "("
                   + (year - 5) + ","
                   + (year + 5)
                   + ")"
                   + " where  "
                   + Reports_MaintenanceOPRs_YearRangeReportDetails_Table.Year_NO + "=" + year
                   );
                    if (table.Rows.Count == 1)
                    {
                        int yearno = Convert.ToInt32(table.Rows[0][0].ToString());
                        int maintenanceopr_count = Convert.ToInt32(table.Rows[0][1].ToString());
                        int maintenanceopr_endwork_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int maintenanceopr_repaired_count = Convert.ToInt32(table.Rows[0][3].ToString());
                        int maintenanceopr_warranty_count = Convert.ToInt32(table.Rows[0][4].ToString());
                        int maintenanceopr_ENDwarranty_count = Convert.ToInt32(table.Rows[0][5].ToString());
                        string bills_value = table.Rows[0][6].ToString();
                        string bills_pays_value = table.Rows[0][7].ToString();
                        string bills_pays_remain = table.Rows[0][8].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][9].ToString());
                        string bills_itemsout_value = table.Rows[0][10].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][11].ToString());

                        double bills_realvalue = Convert.ToDouble(table.Rows[0][12].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][13].ToString());
                        int billscount = Convert.ToInt32(table.Rows[0][14].ToString());


                        return new Report_MaintenanceOPRs_YearRange_ReportDetail(yearno, maintenanceopr_count, maintenanceopr_endwork_count
                            , maintenanceopr_repaired_count, maintenanceopr_warranty_count, maintenanceopr_ENDwarranty_count, billscount, bills_value, bills_pays_value
                            , bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsout_value, bills_itemsout_realvalue
                            , bills_realvalue, bills_pays_realvalue);

                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_Year_Report: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
            }
            public List<Report_MaintenanceOPRs_YearRange_ReportDetail> Get_Report_MaintenanceOPRs_YearRange_ReportDetail(int min_year, int max_year)
            {
                List<Report_MaintenanceOPRs_YearRange_ReportDetail> List = new List<Report_MaintenanceOPRs_YearRange_ReportDetail>();
                try
                {


                    DataTable table = DB.GetData(
                   " select "
                   + Reports_MaintenanceOPRs_YearRangeReportDetails_Table.Year_NO + ","
                  + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_EndWork_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Repaired_Count + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Warranty_Count + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.MaintenanceOPRs_Endarranty_Count  + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Value + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency + ","
                   + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_Value + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_ItemsOut_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_RealValue + ","
                    + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Pays_RealValue + ","
                       + Reports_MaintenanceOPRs_MonthReportDetails_Table.BillMaintenances_Count
                   + " from "
                   + Reports_MaintenanceOPRs_YearRangeReportDetails_Table.TableName
                    + "("
                   + min_year + ","
                   + max_year
                   + ")"
                   + " order by  "
                   + Reports_MaintenanceOPRs_YearRangeReportDetails_Table.Year_NO
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int yearno = Convert.ToInt32(table.Rows[i][0].ToString());
                        int maintenanceopr_count = Convert.ToInt32(table.Rows[i][1].ToString());
                        int maintenanceopr_endwork_count = Convert.ToInt32(table.Rows[i][2].ToString());
                        int maintenanceopr_repaired_count = Convert.ToInt32(table.Rows[i][3].ToString());
                        int maintenanceopr_warranty_count = Convert.ToInt32(table.Rows[i][4].ToString());
                        int maintenanceopr_ENDwarranty_count = Convert.ToInt32(table.Rows[i][5].ToString());
                        string bills_value = table.Rows[i][6].ToString();
                        string bills_pays_value = table.Rows[i][7].ToString();
                        string bills_pays_remain = table.Rows[i][8].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[i][9].ToString());
                        string bills_itemsout_value = table.Rows[i][10].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[i][11].ToString());

                        double bills_realvalue = Convert.ToDouble(table.Rows[i][12].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[i][13].ToString());

                        int billscount = Convert.ToInt32(table.Rows[0][14].ToString());


                        Report_MaintenanceOPRs_YearRange_ReportDetail Report_MaintenanceOPRs_YearRange_ReportDetail_
                            = new Report_MaintenanceOPRs_YearRange_ReportDetail(yearno, maintenanceopr_count, maintenanceopr_endwork_count
                            , maintenanceopr_repaired_count, maintenanceopr_warranty_count, maintenanceopr_ENDwarranty_count,billscount , bills_value, bills_pays_value
                            , bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsout_value, bills_itemsout_realvalue
                            , bills_realvalue, bills_pays_realvalue);
                        List.Add(Report_MaintenanceOPRs_YearRange_ReportDetail_);
                    }
                    return List;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_YearRange_ReportDetail: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List ;
                }
            }
            #endregion

        }
        public class AccountBillSQL
        {
            DatabaseInterface DB;
            public static class AccountBillTables
            {
                public const string BillDayReport = "[dbo].[Bills_GetBills_DayReport]";
                public const string BillDayReport_Details = "[dbo].[Bills_GetBills_DayReport_Details]";
                public const string BillMonthReport = "[dbo].[Bills_GetBills_MonthReport]";
                public const string BillMonthReport_Details = "[dbo].[Bills_GetBills_MonthReport_Details]";
                public const string BillYearReport = "[dbo].[Bills_GetBills_YearReport]";
                public const string BillYearReport_Details = "[dbo].[Bills_GetBills_YearReport_Details]";
                public const string BillYearRangeReport = "[dbo].[Bills_GetBills_YearRangeReport]";
                public const string BillYearRangeReport_Details = "[dbo].[Bills_GetBills_YearRangeReport_Details]";

            }
 

            public AccountBillSQL(DatabaseInterface db)
            {
                DB = db;

            }
            
            public List<BillCurrencyReport> GetBillReport_InDay(string year, string month, string day)
            {
                List<BillCurrencyReport > BillCurrencyReportList = new List<BillCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountBillTables.BillDayReport
                        + "("
                        + year + ","
                        + month + ","
                        + day
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int currencyid = Convert.ToInt32(table.Rows [i][0].ToString ());
                        string currencyname = table.Rows[i][1].ToString();
                        int billin_Count= Convert.ToInt32(table.Rows[i][2].ToString());
                        double  billin_Value = Convert.ToDouble (table.Rows[i][3].ToString());
                        double billin_Pays_Value = Convert.ToDouble(table.Rows[i][4].ToString());
                        int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
                        double billm_Value = Convert.ToDouble(table.Rows[i][6].ToString());
                        double billm_Pays_Value = Convert.ToDouble(table.Rows[i][7].ToString());
                        int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
                        double billout_Value = Convert.ToDouble(table.Rows[i][9].ToString());
                        double billout_Pays_Value = Convert.ToDouble(table.Rows[i][10].ToString());
                        BillCurrencyReport billreportDetail =
                            new BillCurrencyReport (currencyid, currencyname, billin_Count , billin_Value, billin_Pays_Value
                            , billm_Count, billm_Value, billm_Pays_Value
                            , billout_Count, billout_Value, billout_Pays_Value);
                        BillCurrencyReportList.Add(billreportDetail);
                    }
                    return BillCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillCurrencyReportList;
                }
            }
            public List<BillCurrencyReport> GetBillReport_InMonth(string year, string month)
            {
                List<BillCurrencyReport> BillCurrencyReportList = new List<BillCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountBillTables.BillMonthReport
                        + "("
                        + year + ","
                        + month
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int currencyid = Convert.ToInt32(table.Rows[i][0].ToString());
                        string currencyname = table.Rows[i][1].ToString();
                        int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        double billin_Value = Convert.ToDouble(table.Rows[i][3].ToString());
                        double billin_Pays_Value = Convert.ToDouble(table.Rows[i][4].ToString());
                        int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
                        double billm_Value = Convert.ToDouble(table.Rows[i][6].ToString());
                        double billm_Pays_Value = Convert.ToDouble(table.Rows[i][7].ToString());
                        int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
                        double billout_Value = Convert.ToDouble(table.Rows[i][9].ToString());
                        double billout_Pays_Value = Convert.ToDouble(table.Rows[i][10].ToString());
                        BillCurrencyReport billreportDetail =
                            new BillCurrencyReport(currencyid, currencyname, billin_Count, billin_Pays_Value, billin_Pays_Value
                            , billm_Count, billm_Value, billm_Pays_Value
                            , billout_Count, billout_Value, billout_Pays_Value);
                        BillCurrencyReportList.Add(billreportDetail);
                    }
                    return BillCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillCurrencyReportList;
                }
            }
            public List<BillCurrencyReport> GetBillReport_InYear(string year)
            {
                List<BillCurrencyReport> BillCurrencyReportList = new List<BillCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountBillTables.BillYearReport
                        + "("
                        + year 
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int currencyid = Convert.ToInt32(table.Rows[i][0].ToString());
                        string currencyname = table.Rows[i][1].ToString();
                        int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        double billin_Value = Convert.ToDouble(table.Rows[i][3].ToString());
                        double billin_Pays_Value = Convert.ToDouble(table.Rows[i][4].ToString());
                        int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
                        double billm_Value = Convert.ToDouble(table.Rows[i][6].ToString());
                        double billm_Pays_Value = Convert.ToDouble(table.Rows[i][7].ToString());
                        int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
                        double billout_Value = Convert.ToDouble(table.Rows[i][9].ToString());
                        double billout_Pays_Value = Convert.ToDouble(table.Rows[i][10].ToString());
                        BillCurrencyReport billreportDetail =
                            new BillCurrencyReport(currencyid, currencyname, billin_Count, billin_Pays_Value, billin_Pays_Value
                            , billm_Count, billm_Value, billm_Pays_Value
                            , billout_Count, billout_Value, billout_Pays_Value);
                        BillCurrencyReportList.Add(billreportDetail);
                    }
                    return BillCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillCurrencyReportList;
                }
            }
            public List<BillCurrencyReport> GetBillReport_BetweenTwoYears(string year1, string year2)
            {
                List<BillCurrencyReport> BillCurrencyReportList = new List<BillCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountBillTables.BillYearRangeReport
                        + "("
                        + year1 + ","
                        + year2 
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int currencyid = Convert.ToInt32(table.Rows[i][0].ToString());
                        string currencyname = table.Rows[i][1].ToString();
                        int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        double billin_Value = Convert.ToDouble(table.Rows[i][3].ToString());
                        double billin_Pays_Value = Convert.ToDouble(table.Rows[i][4].ToString());
                        int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
                        double billm_Value = Convert.ToDouble(table.Rows[i][6].ToString());
                        double billm_Pays_Value = Convert.ToDouble(table.Rows[i][7].ToString());
                        int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
                        double billout_Value = Convert.ToDouble(table.Rows[i][9].ToString());
                        double billout_Pays_Value = Convert.ToDouble(table.Rows[i][10].ToString());
                        BillCurrencyReport billreportDetail =
                            new BillCurrencyReport(currencyid, currencyname, billin_Count, billin_Pays_Value, billin_Pays_Value
                            , billm_Count, billm_Value, billm_Pays_Value
                            , billout_Count, billout_Value, billout_Pays_Value);
                        BillCurrencyReportList.Add(billreportDetail);
                    }
                    return BillCurrencyReportList;
                }
                catch
                {
                    MessageBox.Show(" ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillCurrencyReportList;
                }
            }
            public List <BillReportDetail> GetBillReport_Details_InDay(string  year, string month, string day)
            {
                List<BillReportDetail> BillReportDetailList = new List<BillReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountBillTables.BillDayReport_Details
                        + "("
                        +year +","
                        + month  + ","
                        + day 
                        + ")"
                        + " order by Bill_Time"
                        );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime billdate = Convert.ToDateTime(table.Rows [i][0].ToString ());
                        int billid = Convert.ToInt32(table.Rows[i][1].ToString());
                        string billtype = table.Rows[i][2].ToString();
                        string desc = table.Rows[i][3].ToString();
                        string owner = table.Rows[i][4].ToString();
                        string operations = table.Rows[i][5].ToString();
                        string currency = table.Rows[i][6].ToString();
                        double value= Convert.ToDouble (table.Rows[i][7].ToString());
                        double paid = Convert.ToDouble(table.Rows[i][8].ToString());
                        double remain = Convert.ToDouble(table.Rows[i][9].ToString());
                        BillReportDetail billreportDetail = new BillReportDetail(billdate, billid, billtype, desc, owner, operations, currency, value, paid, remain);
                        BillReportDetailList.Add(billreportDetail);
                    }
                    return BillReportDetailList;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير  فواتير اليوم التفصيلي","خطأ",MessageBoxButtons.OK,MessageBoxIcon.Error );
                    return BillReportDetailList;
                }
            }
            public List<BillDayReportDetail> GetBillReport_Details_InMonth(string year, string month)
            {
                List<BillDayReportDetail> BillDayReportDetailList = new List<BillDayReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountBillTables.BillMonthReport_Details
                        + "("
                        + year + ","
                        + month
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int dayid = Convert.ToInt32(table.Rows[i][0].ToString());
                        DateTime daydate = Convert.ToDateTime(table.Rows [i][1].ToString ());
                        int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        string  billin_Value = table.Rows[i][3].ToString();
                        string billin_Pays_Value = table.Rows[i][4].ToString();
                        int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
                        string billm_Value = table.Rows[i][6].ToString();
                        string billm_Pays_Value = table.Rows[i][7].ToString();
                        int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
                        string billout_Value =table.Rows[i][9].ToString();
                        string billout_Pays_Value = table.Rows[i][10].ToString();

                        BillDayReportDetail billreportdayDetail = new BillDayReportDetail(dayid,daydate  , billin_Count  , billin_Value  , billin_Pays_Value
                            , billm_Count  , billm_Value, billm_Pays_Value
                            , billout_Count, billout_Value, billout_Pays_Value);
                        BillDayReportDetailList.Add(billreportdayDetail);
                    }
                    return BillDayReportDetailList;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير الشهر التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillDayReportDetailList;
                }
            }
            public List<BillMonthReportDetail> GetBillReport_Details_InYear(string year)
            {
                List<BillMonthReportDetail> BillMonthReportDetailList = new List<BillMonthReportDetail>();
                try
                {
                    DataTable table = DB.GetData(
                        " select *from "
                        + AccountBillTables.BillYearReport_Details
                        + "("
                        + year
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int monthid = Convert.ToInt32(table.Rows [i][0].ToString ());
                        string  month = table.Rows[i][1].ToString();
                        int billin_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        string billin_Value = table.Rows[i][3].ToString();
                        string billin_Pays_Value = table.Rows[i][4].ToString();
                        int billm_Count = Convert.ToInt32(table.Rows[i][5].ToString());
                        string billm_Value = table.Rows[i][6].ToString();
                        string billm_Pays_Value = table.Rows[i][7].ToString();
                        int billout_Count = Convert.ToInt32(table.Rows[i][8].ToString());
                        string billout_Value = table.Rows[i][9].ToString();
                        string billout_Pays_Value = table.Rows[i][10].ToString();

                        BillMonthReportDetail billmonthreportdetail = new BillMonthReportDetail(monthid , month, billin_Count, billin_Value, billin_Pays_Value
                            , billm_Count, billm_Value, billm_Pays_Value
                            , billout_Count, billout_Value, billout_Pays_Value);
                        BillMonthReportDetailList.Add(billmonthreportdetail);
                    }
                    return BillMonthReportDetailList;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير السنة التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillMonthReportDetailList;
                }
            }
            public List<BillYearReportDetail > GetBillReport_Details_InYearRange(string year1, string year2)
            {
                List<BillYearReportDetail> BillYearReportDetailList = new List<BillYearReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                        " select * from "
                        + AccountBillTables.BillYearRangeReport_Details
                        + "("
                        + year1  + ","
                        + year2
                        + ")");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int   year =Convert .ToInt32 ( table.Rows[i][0].ToString());
                        int billin_Count = Convert.ToInt32(table.Rows[i][1].ToString());
                        string billin_Value = table.Rows[i][2].ToString();
                        string billin_Pays_Value = table.Rows[i][3].ToString();
                        int billm_Count = Convert.ToInt32(table.Rows[i][4].ToString());
                        string billm_Value = table.Rows[i][5].ToString();
                        string billm_Pays_Value = table.Rows[i][6].ToString();
                        int billout_Count = Convert.ToInt32(table.Rows[i][7].ToString());
                        string billout_Value = table.Rows[i][8].ToString();
                        string billout_Pays_Value = table.Rows[i][9].ToString();

                        BillYearReportDetail BillYearReportDetail_ = new BillYearReportDetail(year, billin_Count, billin_Value, billin_Pays_Value
                            , billm_Count, billm_Value, billm_Pays_Value
                            , billout_Count, billout_Value, billout_Pays_Value);
                        BillYearReportDetailList.Add(BillYearReportDetail_);
                    }
                    return BillYearReportDetailList;
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    MessageBox.Show("فشل جلب تقرير السنوات التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillYearReportDetailList; 
                }
            }
           }
        public class CurrencySQL
        {
            DatabaseInterface DB;
            public static class CurrencyTable
            {
                public const string TableName = "Account_Currency";
                public const string CurrencyID = "CurrencyID";
                public const string CurrencyName = "CurrencyName";
                public const string CurrencySymbol = "CurrencySymbol";
                public const string ReferenceFactor = "ReferenceFactor";
                public const string ReferenceCurrencyID = "ReferenceCurrencyID";
                public const string Disable = "Disable";

            }
            public CurrencySQL(DatabaseInterface db)
            {
                DB = db;
                
            }
            public Currency GetCurrencyINFO_ByID(uint currencyid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + CurrencyTable.CurrencyName + ","
                     + CurrencyTable.CurrencySymbol + ","
                     + CurrencyTable.ReferenceFactor + ","
                     + CurrencyTable.ReferenceCurrencyID 
                    + " from   "
                    + CurrencyTable.TableName
                    + " where "
                    + CurrencyTable.CurrencyID + "=" + currencyid
                      );
                    if (t.Rows.Count == 1)
                    {
                        string name = t.Rows[0][0].ToString();
                        string symbol = t.Rows[0][1].ToString();
                        double referncefactor = Convert.ToDouble(t.Rows[0][2].ToString());
                        uint? RefCurrencyid;
                        try
                        {
                            RefCurrencyid = Convert.ToUInt32(t.Rows[0][3].ToString());
                        }
                        catch
                        {
                            RefCurrencyid = null;
                        }

                        return new Currency(currencyid, name, symbol, referncefactor, RefCurrencyid);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب بيانات العملة"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
                
            }
            public Currency GetReferenceCurrency()
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + CurrencyTable.CurrencyID  + ","
                     + CurrencyTable.CurrencyName + ","
                     + CurrencyTable.CurrencySymbol

                    + " from   "
                    + CurrencyTable.TableName
                    + " where "
                     + CurrencyTable.ReferenceCurrencyID+" is null"
                     +" and "
                     + CurrencyTable.ReferenceFactor  + "=1 "
                      );
                    if (t.Rows.Count == 1)
                    {
                        uint currencyid = Convert.ToUInt32(t.Rows[0][0].ToString());
                        string name = t.Rows[0][1].ToString();
                        string symbol = t.Rows[0][2].ToString();


                        return new Currency(currencyid, name, symbol, 1, null );

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلبالعملة المرجعية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
            public bool  AddCurrency(string name,string symbol,double referncefactor)
            {

                try
                {
                    DB.ExecuteSQLCommand(
                        " insert into "
                    + CurrencyTable.TableName 
                    + "("
                    + CurrencyTable.CurrencyName
                    + ","
                    + CurrencyTable.CurrencySymbol
                    + ","
                    + CurrencyTable.ReferenceFactor
                     + ","
                    + CurrencyTable.ReferenceCurrencyID 
                    + ")"
                    + "values"
                    + "("
                    + "'" + name + "'"
                    + ","
                      + "'" + symbol  + "'"
                    + ","
                    + referncefactor 
                     + ","
                    + GetReferenceCurrency ().CurrencyID
                    + ")"
                    );
                    DB.AddLog(
                             DatabaseInterface.Log.LogType.INSERT  
                             , DatabaseInterface.Log.Log_Target.Currency 
                             , ""
                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Currency
                            , ""
                          , false , ee.Message );
                    MessageBox.Show("AddCurrency:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateCurrency(uint currencyid,string newname, string symbol, double referncefactor)
            {
                   try
                {
                    DB.ExecuteSQLCommand( "update  "
                    + CurrencyTable.TableName
                    + " set "
                    + CurrencyTable.CurrencyName+"='"+newname +"',"
                    + CurrencyTable.CurrencySymbol + "='" + symbol + "',"
                    + CurrencyTable.ReferenceFactor + "="+referncefactor 
                    + " where "
                    + CurrencyTable.CurrencyID  +"="+currencyid
                    );
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Currency
                            , ""
                          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                           DatabaseInterface.Log.LogType.UPDATE
                           , DatabaseInterface.Log.Log_Target.Currency
                           , ""
                         , false ,ee.Message );
                    MessageBox.Show("UpdateCurrency"+ee.Message , "خطأ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteCurrency(uint currencyid)
            {
                try
                {
                    DB.ExecuteSQLCommand( "delete from   "
                    + CurrencyTable.TableName
                    + " where "
                    + CurrencyTable.CurrencyID + "=" + currencyid
                    );
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Currency
                            , ""
                          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                               DatabaseInterface.Log.LogType.DELETE 
                               , DatabaseInterface.Log.Log_Target.Currency
                               , ""
                             , false , ee.Message );
                    MessageBox.Show("DeleteCurrency", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            } 
            public List<Currency> GetCurrencyList()
            {
                List<Currency> currencyList = new List<Currency>();
                try
                {
                   
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + CurrencyTable.CurrencyID  + ","
                     + CurrencyTable.CurrencyName + ","
                     + CurrencyTable.CurrencySymbol + ","
                     + CurrencyTable.ReferenceFactor + ","
                     + CurrencyTable.ReferenceCurrencyID 
                    + " from   "
                    + CurrencyTable.TableName
                      );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint currencyid = Convert.ToUInt32(t.Rows[i][0].ToString()); 
                        string name = t.Rows[i][1].ToString();
                        string symbol = t.Rows[i][2].ToString().Replace (" ",string .Empty );
                        double referncefactor = Convert.ToDouble(t.Rows[i][3].ToString());
                        uint? RefCurrencyid ;
                        try
                        {
                            RefCurrencyid = Convert.ToUInt32(t.Rows[i][4].ToString());
                        }
                        catch
                        {
                            RefCurrencyid = null;
                        }
                        currencyList .Add ( new Currency(currencyid, name, symbol, referncefactor, RefCurrencyid));

                    }
                    return currencyList;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب قائمة العملات"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return currencyList;
                }

            }
         
         }
        public class ExchangeOPRSQL
        {

            DatabaseInterface DB;
            private static class ExchangeOPRTable
            {
                public const string TableName = "Account_ExchangeOpr";
                public const string ExchangeOprID = "ExchangeOprID";
                public const string ExchangeOprDate = "ExchangeOprDate";
                public const string SourceCurrencyID = "SourceCurrencyID";
                public const string SourceExchangeRate = "SourceExchangeRate";
                public const string OutMoneyValue = "OutMoneyValue";
                public const string TargetCurrencyID = "TargetCurrencyID";
                public const string TargetExchangeRate = "TargetExchangeRate";
                public const string Notes = "Notes";


            }
            public ExchangeOPRSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public ExchangeOPR GetExchangeOPR_INFO_BYID(uint exchangeoprid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ExchangeOPRTable.ExchangeOprDate
                        + ","
                        + ExchangeOPRTable.SourceCurrencyID
                        + ","
                        + ExchangeOPRTable.SourceExchangeRate
                        + ","
                        + ExchangeOPRTable.OutMoneyValue
                        + ","
                        + ExchangeOPRTable.TargetCurrencyID
                        + ","
                        + ExchangeOPRTable.TargetExchangeRate
                        + ","
                        + ExchangeOPRTable.Notes
                        + " from   "
                        + ExchangeOPRTable.TableName
                        + " where "
                        + ExchangeOPRTable.ExchangeOprID + "=" + exchangeoprid
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime exchangeoprdate = Convert.ToDateTime(t.Rows[0][0].ToString());
                        Currency sourcecurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                        double source_exchangerate = Convert.ToDouble(t.Rows[0][2].ToString());
                        double outmenyvalue = Convert.ToDouble(t.Rows[0][3].ToString());
                        Currency targetcurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][4].ToString()));
                        double target_exchangerate = Convert.ToDouble(t.Rows[0][5].ToString());
                        string notes = t.Rows[0][6].ToString();
                        return new ExchangeOPR(exchangeoprid, exchangeoprdate, sourcecurrency, source_exchangerate, outmenyvalue, targetcurrency, target_exchangerate, notes);

                    }
                    else
                        return null;
                }
                catch(Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
               
            }
            public bool Add_ExchageOPR(DateTime exchangeoprdate,Currency sourcecurrency,double source_exchangerate,double outmoneyvalue,Currency targetcurrency,double target_exchangerate,string notes)
            {
                try
                {
                    DB.ExecuteSQLCommand( " insert into "
                    + ExchangeOPRTable.TableName
                    + "("
                    + ExchangeOPRTable.ExchangeOprDate
                    + ","
                    + ExchangeOPRTable.SourceCurrencyID 
                    + ","
                     + ExchangeOPRTable.SourceExchangeRate
                    + ","
                    + ExchangeOPRTable.OutMoneyValue 
                    + ","
                    + ExchangeOPRTable.TargetCurrencyID
                    + ","
                    + ExchangeOPRTable.TargetExchangeRate 
                    + ","
                    + ExchangeOPRTable.Notes 
                    + ")"
                    + "values"
                    + "("
                    +"'" + exchangeoprdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + sourcecurrency .CurrencyID 
                    + ","
                    +source_exchangerate  
                    + ","
                    + outmoneyvalue 
                    + ","
                    + targetcurrency .CurrencyID
                    + ","
                    + target_exchangerate 
                    + ","
                    + "'"+  notes + "'"
                    + ")"
                    );
                    DB.AddLog(
                           DatabaseInterface.Log.LogType.INSERT 
                           , DatabaseInterface.Log.Log_Target.ExchangeOPR 
                           , ""
                         , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.INSERT
                          , DatabaseInterface.Log.Log_Target.ExchangeOPR 
                          , ""
                        , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_ExchageOPR(uint exchangeoprid,DateTime exchangeoprdate, Currency sourcecurrency, double source_exchangerate, double outmoneyvalue, Currency targetcurrency, double target_exchangerate, string notes)
            {
                try
                { 
                    DB.ExecuteSQLCommand( "update  "
                    + ExchangeOPRTable.TableName
                    + " set "
                    + ExchangeOPRTable.ExchangeOprDate  + "="+"'" + exchangeoprdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                     + ExchangeOPRTable.SourceCurrencyID + "=" + sourcecurrency.CurrencyID
                    + ","
                     + ExchangeOPRTable.SourceExchangeRate + "=" + source_exchangerate
                    + ","
                    + ExchangeOPRTable.OutMoneyValue   + "=" + outmoneyvalue  
                    + ","
                      + ExchangeOPRTable.TargetCurrencyID + "=" + targetcurrency.CurrencyID 
                    + ","
                     + ExchangeOPRTable.TargetExchangeRate + "=" + target_exchangerate
                    + ","
                    + ExchangeOPRTable.Notes   + "='" + notes + "'"
                    + " where "
                    + ExchangeOPRTable.ExchangeOprID + "=" + exchangeoprid
                    );
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.UPDATE 
                          , DatabaseInterface.Log.Log_Target.ExchangeOPR 
                          , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.UPDATE
                         , DatabaseInterface.Log.Log_Target.ExchangeOPR 
                         , ""
                       , false ,ee.Message );
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_ExchageOPR(uint exchangeoprid)
            {
                try
                {
                    DB.ExecuteSQLCommand( "delete from   "
                    + ExchangeOPRTable.TableName
                    + " where "
                    + ExchangeOPRTable.ExchangeOprID  + "=" + exchangeoprid 
                    );
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.DELETE 
                          , DatabaseInterface.Log.Log_Target.ExchangeOPR
                          , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.DELETE 
                         , DatabaseInterface.Log.Log_Target.ExchangeOPR
                         , ""
                       , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<ExchangeOPR> GetExchangeOPRList()
            {
                try
                {
                    List<ExchangeOPR> ExchangeOPRList = new List<ExchangeOPR >();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + ExchangeOPRTable.ExchangeOprID
                    + ","
                    + ExchangeOPRTable.ExchangeOprDate
                    + ","
                    + ExchangeOPRTable.SourceCurrencyID
                    + ","
                    + ExchangeOPRTable.SourceExchangeRate
                    + ","
                    + ExchangeOPRTable.OutMoneyValue
                    + ","
                    + ExchangeOPRTable.TargetCurrencyID
                    + ","
                    + ExchangeOPRTable.TargetExchangeRate
                    + ","
                    + ExchangeOPRTable.Notes
                    + " from   "
                    + ExchangeOPRTable.TableName
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint exchangeoprid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime exchangeoprdate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        Currency sourcecurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][2].ToString()));
                        double source_exchangerate = Convert.ToDouble(t.Rows[i][3].ToString());
                        double outmenyvalue = Convert.ToDouble(t.Rows[i][4].ToString());
                        Currency targetcurrency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        double target_exchangerate = Convert.ToDouble(t.Rows[i][6].ToString());
                        string notes = t.Rows[i][7].ToString();
                        ExchangeOPRList.Add (new ExchangeOPR(exchangeoprid, exchangeoprdate, sourcecurrency, source_exchangerate, outmenyvalue, targetcurrency, target_exchangerate, notes));

                    }
                    return ExchangeOPRList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        public class PayINSQL
        {

            DatabaseInterface DB;
            internal  static class PayINTable
            {
                public const string TableName = "Account_PayIN";
                public const string PayOprID = "PayOprID";
                public const string PayOprDate = "PayOprDate";
                public const string OperationType = "OperationType";
                public const string OperationID = "OperationID";
                public const string PayDescription = "PayDescription";
                public const string Value = "Value";
                public const string ExchangeRate = "ExchangeRate";
                public const string CurrencyID = "CurrencyID";
                public const string Notes = "Notes";
            }
            public PayINSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public PayIN GetPayIN_INFO_BYID(uint payinid)
            {
                DataTable t = new DataTable();
                
                t = DB.GetData("select "
                    + PayINTable.PayOprDate
                    + ","
                    + PayINTable.OperationType 
                    + ","
                    + PayINTable.OperationID  
                    + ","
                    + PayINTable.PayDescription 
                    + ","
                    + PayINTable.Value
                    + ","
                    + PayINTable.ExchangeRate
                    + ","
                    + PayINTable.CurrencyID 
                    + ","
                    + PayINTable.Notes
                    + " from   "
                    + PayINTable.TableName
                    + " where "
                    + PayINTable.PayOprID + "=" + payinid 
                  );
                if (t.Rows.Count == 1)
                {
                    DateTime payindate = Convert.ToDateTime(t.Rows[0][0].ToString());
                    Bill Bill_ = null;
                    try
                    {
                        uint operationtype = Convert.ToUInt32(t.Rows[0][1].ToString());
                        uint operationID = Convert.ToUInt32(t.Rows[0][2].ToString());
                        Operation  Operation_ = new Operation(operationtype, operationID);
                        Bill_ = new OperationSQL(DB).GetOperationBill(Operation_);
                    }
                    catch 
                    {
                        Bill_ = null;

                    }
                   
                    string description = t.Rows[0][3].ToString();
                    double value = Convert.ToDouble(t.Rows[0][4].ToString());
                    double exchangerate = Convert.ToDouble(t.Rows[0][5].ToString());
                    Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][6].ToString()));
                    string notes = t.Rows[0][7].ToString();
                    return new PayIN (payinid, payindate, Bill_, description , value, exchangerate, currency , notes);

                }
                else
                    return null;
            }
            public bool Add_PayIN(DateTime payindate, Operation Operation_, string description,  double value, double exchangerate, Currency currency, string notes)
            {
                try
                {

                    string operationtype_Str, operationid_Str;
                    if(Operation_ ==null )
                    {
                        operationtype_Str = "null";
                        operationid_Str = "null";
                    }
                    else
                    {
                        operationtype_Str = Operation_.OperationType .ToString(); 
                        operationid_Str = Operation_.OperationID.ToString();
                    }
                    DB.ExecuteSQLCommand( " insert into "
                    + PayINTable.TableName
                    + "("
                    + PayINTable.PayOprDate
                    + ","
                    + PayINTable.OperationType  
                    + ","
                    + PayINTable.OperationID  
                    + ","
                    + PayINTable.PayDescription 
                    + ","
                    + PayINTable.Value
                    + ","
                    + PayINTable.ExchangeRate
                    + ","
                    + PayINTable.CurrencyID 
                    + ","
                    + PayINTable.Notes
                    + ")"
                    + "values"
                    + "("
                     +"'" + payindate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + operationtype_Str
                    + ","
                    + operationid_Str 
                    + ","
                    + "'"+description +"'"
                    + ","
                    + value
                    + ","
                    + exchangerate
                    + ","
                    + currency .CurrencyID
                    + ","
                    + "'"+notes+"'"
                    + ")"
                    );
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.INSERT 
                         , DatabaseInterface.Log.Log_Target.PayIN 
                         , ""
                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.PayIN
                            , ""
                          , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_PayIN(uint payinid, DateTime payindate, Operation Operation_, string description, double value, double exchangerate, Currency currency, string notes)
            {
                try
                {
                    string operationtype_Str, operationid_Str;
                    if (Operation_ == null)
                    {
                        operationtype_Str = "=null";
                        operationid_Str = "=null";
                    }
                    else
                    {
                        operationtype_Str = Operation_.OperationType.ToString();
                        operationid_Str = Operation_.OperationID.ToString();
                    }
                   
                    DB.ExecuteSQLCommand( "update  "
                    + PayINTable.TableName
                    + " set "
                    +PayINTable .PayOprDate+"=" +"'" + payindate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + PayINTable.OperationType  + operationtype_Str
                    + ","
                    + PayINTable.OperationID  + operationid_Str
                    + ","
                    + PayINTable.PayDescription  + "='" + description+"'"
                    + ","
                    + PayINTable.Value + "=" + value 
                    + ","
                    + PayINTable.ExchangeRate  + "=" + exchangerate 
                    + ","
                    + PayINTable.Notes +"='" + notes  + "'"
                    + " where "
                    + PayINTable.PayOprID + "=" + payinid 
                    );
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.UPDATE 
                         , DatabaseInterface.Log.Log_Target.PayIN
                         , ""
                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.UPDATE
                         , DatabaseInterface.Log.Log_Target.PayIN
                         , ""
                       , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_PayIN:" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_PayIN(uint payinid)
            {
                try
                {
                    DB.ExecuteSQLCommand( "delete from   "
                    + PayINTable.TableName
                    + " where "
                    + PayINTable.PayOprID + "=" + payinid 
                    );
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.DELETE 
                         , DatabaseInterface.Log.Log_Target.PayIN
                         , ""
                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.DELETE
                         , DatabaseInterface.Log.Log_Target.PayIN
                         , ""
                       , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<PayIN > GetPayINList(Operation  opeartion)
            {
                try
                {
                    List<PayIN > payinList = new List<PayIN >();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + PayINTable.PayOprID 
                    + ","
                    + PayINTable.PayOprDate
                    + ","
                    + PayINTable.PayDescription
                    + ","
                    + PayINTable.Value
                    + ","
                    + PayINTable.ExchangeRate
                    + ","
                    + PayINTable.CurrencyID
                    + ","
                    + PayINTable.Notes
                    + " from   "
                    + PayINTable.TableName
                    + " where "
                    + PayINTable.OperationType   + "=" +opeartion.OperationType
                     + " and "
                    + PayINTable.OperationID + "=" + opeartion.OperationID 
                  );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint payinid =Convert .ToUInt32 ( t.Rows[i][0].ToString());
                        DateTime payindate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string description = t.Rows[i][2].ToString();
                        double value = Convert.ToDouble(t.Rows[i][3].ToString());
                        double exchangerate = Convert.ToDouble(t.Rows[i][4].ToString());
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        string notes = t.Rows[i][6].ToString();
                        payinList.Add ( new PayIN(payinid, payindate, new OperationSQL(DB).GetOperationBill(opeartion ), description, value, exchangerate, currency, notes));
                    }
                    return payinList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("", "" + ee.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
           
        }
        public class PayOUTSQL
        {

            DatabaseInterface DB;
            internal   static class PayOUTTable
            {
                public const string TableName = "Account_PayOUT";
                public const string PayOprID = "PayOprID";
                public const string PayOprDate = "PayOprDate";
                public const string OperationType = "OperationType";
                public const string OperationID = "OperationID";
                public const string PayDescription = "PayDescription";
                public const string Value = "Value";
                public const string ExchangeRate = "ExchangeRate";
                public const string CurrencyID = "CurrencyID";
                public const string Notes = "Notes";
            }
            public PayOUTSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public PayOUT  GetPayOUT_INFO_BYID(uint payoutid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                    + PayOUTTable.PayOprDate
                    + ","
                    + PayOUTTable.OperationType 
                    + ","
                     + PayOUTTable.OperationID 
                    + ","
                    + PayOUTTable.PayDescription
                    + ","
                    + PayOUTTable.Value
                    + ","
                    + PayOUTTable.ExchangeRate
                    + ","
                    + PayOUTTable.CurrencyID
                    + ","
                    + PayOUTTable.Notes
                    + " from   "
                    + PayOUTTable.TableName
                    + " where "
                    + PayOUTTable.PayOprID + "=" + payoutid
                  );
                if (t.Rows.Count == 1)
                {
                    DateTime payindate = Convert.ToDateTime(t.Rows[0][0].ToString());
                    Operation   operation;
                    try
                    {

                        uint operationtype = Convert.ToUInt32(t.Rows[0][1].ToString());
                        uint operationid = Convert.ToUInt32(t.Rows[0][2].ToString());
                        operation = new Operation(operationtype, operationid);

                    }
                    catch
                    {
                        operation = null;
                    }
                    string description = t.Rows[0][3].ToString();
                    double value = Convert.ToDouble(t.Rows[0][4].ToString());
                    double exchangerate = Convert.ToDouble(t.Rows[0][5].ToString());
                    Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][6].ToString()));
                    string notes = t.Rows[0][6].ToString();
                    return new PayOUT(payoutid, payindate, new OperationSQL(DB).GetOperationBill(operation ), description, value, exchangerate, currency, notes);
                }
                else
                    return null;
            }
            public bool Add_PayOUT(DateTime payoutdate, Operation operation, string description, double value, double exchangerate, Currency currency, string notes)
            {
                try
                {
                    string operationtype_str, operationid_str;
                    if(operation ==null )
                    {
                        operationtype_str = "null";
                        operationid_str = "null";
                    }
                    else
                    {
                        operationtype_str = operation.OperationType.ToString() ;
                        operationid_str = operation.OperationID .ToString();
                    }
                    
                    DB.ExecuteSQLCommand( " insert into "
                    + PayOUTTable.TableName
                    + "("
                    + PayOUTTable.PayOprDate
                    + ","
                    + PayOUTTable.OperationType 
                    + ","
                     + PayOUTTable.OperationID 
                    + ","
                    + PayOUTTable.PayDescription
                    + ","
                    + PayOUTTable.Value
                    + ","
                    + PayOUTTable.ExchangeRate
                    + ","
                    + PayOUTTable.CurrencyID
                    + ","
                    + PayOUTTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + "'" + payoutdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + operationtype_str
                    + ","
                    + operationid_str 
                    + ","
                    + "'" + description + "'"
                    + ","
                    + value
                    + ","
                    + exchangerate
                    + ","
                    + currency.CurrencyID
                    + ","
                    + "'" + notes + "'"
                    + ")"
                    );
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.INSERT 
                         , DatabaseInterface.Log.Log_Target.PayOUT
                         , ""
                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.INSERT
                        , DatabaseInterface.Log.Log_Target.PayOUT
                        , ""
                      , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add_PayOUT:" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_PayOUT(uint payoutid, DateTime payoutdate, Operation Operation_,  string description, double value, double exchangerate, Currency currency, string notes)
            {
                try
                {
                    string operationtype_Str, operationid_Str;
                    if (Operation_ == null)
                    {
                        operationtype_Str = "=null";
                        operationid_Str = "=null";
                    }
                    else
                    {
                        operationtype_Str = Operation_.OperationType.ToString();
                        operationid_Str = Operation_.OperationID.ToString();
                    }
                    DB.ExecuteSQLCommand( "update  "
                    + PayOUTTable.TableName
                    + " set "
                    + PayOUTTable.PayOprDate + "=" + "'" + payoutdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                      + ","
                    + PayOUTTable.OperationType + operationtype_Str
                    + ","
                    + PayOUTTable.OperationID + operationid_Str
                    + ","
                    + PayOUTTable.PayDescription + "='" + description + "'"
                    + ","
                    + PayOUTTable.Value + "=" + value
                    + ","
                    + PayOUTTable.ExchangeRate + "=" + exchangerate
                    + ","
                    + PayOUTTable.Notes + "='" + notes + "'"
                    + " where "
                    + PayOUTTable.PayOprID + "=" + payoutid 
                    );
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.UPDATE 
                        , DatabaseInterface.Log.Log_Target.PayOUT
                        , ""
                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.UPDATE
                       , DatabaseInterface.Log.Log_Target.PayOUT
                       , ""
                     , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_PayOUT:" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_PayOUT(uint payoutid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + PayOUTTable.TableName
                    + " where "
                    + PayOUTTable.PayOprID + "=" + payoutid
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE 
                       , DatabaseInterface.Log.Log_Target.PayOUT
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE
                      , DatabaseInterface.Log.Log_Target.PayOUT
                      , ""
                    , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("", "" + ee.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<PayOUT > GetPaysOUT_List(Operation Operation_)
            {
                try
                {
                    List<PayOUT> PayOUTList = new List<PayOUT>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + PayOUTTable.PayOprID
                    + ","
                    + PayOUTTable.PayOprDate
                    + ","
                    + PayOUTTable.PayDescription
                    + ","
                    + PayOUTTable.Value
                    + ","
                    + PayOUTTable.ExchangeRate
                    + ","
                    + PayOUTTable.CurrencyID
                    + ","
                    + PayOUTTable.Notes
                    + " from   "
                    + PayOUTTable.TableName
                    + " where "
                    + PayOUTTable.OperationType   + "=" + Operation_ .OperationType
                    + " and "
                    + PayOUTTable.OperationID  + "=" + Operation_.OperationID 
                  );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint payinid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime payindate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string description = t.Rows[i][2].ToString();
                        double value = Convert.ToDouble(t.Rows[i][3].ToString());
                        double exchangerate = Convert.ToDouble(t.Rows[i][4].ToString());
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        string notes = t.Rows[i][6].ToString();
                        PayOUTList.Add(new PayOUT(payinid, payindate, new OperationSQL(DB).GetOperationBill(Operation_), description, value, exchangerate, currency, notes));
                    }
                    return PayOUTList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("", "" + ee.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }

            
        }
    }
}
