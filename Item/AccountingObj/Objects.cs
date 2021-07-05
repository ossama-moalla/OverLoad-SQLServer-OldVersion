using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.Maintenance.Objects;
using ItemProject.Trade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.AccountingObj
{
    namespace Objects
    {
        public class DateAccount
        {
            public class YearRange
            {
                public int min_year;
                public int max_year;
                public YearRange(int y1, int y2)
                {
                    if (y1 > y2)
                    {
                        min_year = y2;
                        max_year = y1;
                    }
                    else
                    {
                        min_year = y1;
                        max_year = y2;
                    }
                }

            }
            public YearRange YearRange_;
            public int Year;
            public int Month;
            public int Day;
            private DatabaseInterface DB;
            public DateAccount(DatabaseInterface db, YearRange YearRange__, int year, int month, int day)
            {
                DB = db;
                YearRange_ = YearRange__;
                Year = year;
                Month = month;
                Day = day;
            }
      
   
            public string GetAccountDateString()
            {
                string returnstring = "";
                if (Day != -1)
                    returnstring = "["+Day.ToString()+ "] \\ [" + Month.ToString() + "] \\ [" + Year.ToString()+" ]";

                else if (Month != -1)
                {
                    returnstring = "[" + Month.ToString() + " ] [ " + Year.ToString() + " ]";
                }
                else if (Year != -1)
                {
                    returnstring = Year.ToString();
                }
                else
                {
                    returnstring = YearRange_.max_year.ToString() + "-" + YearRange_.min_year.ToString();
                }
                return returnstring;
            }
            
            public void Account_Date_UP()
            {
                if (this.Day != -1) this.Day = -1;
                else if (this.Month != -1) this.Month = -1;
                else if (this.Year != -1) this.Year = -1;
                else return;
            }
            public void Account_Date_Down(int value)
            {
                if (this.Year == -1)
                {
                    if (Year < 1990 && Year > 2200) return;
                    Year = value;
                }
                else if (Month == -1)
                {
                    if (Month < 1 && Month > 12) return;
                    Month = value;
                }
                else if (Day == -1)
                {
                    
                    if (Day < 1 && Day > DateTime.DaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(Month))) return;
                    Day = value;

                }
                else return;
            }

            public void GetAccountDetails(ref ListView listview)
            {
                int x = 5;
                switch (x)
                {

                    case 1:
                        listview.Items.Clear();
                        #region BillSection
                        if (this.Day != null)
                        {
                            #region BillDayReport
                            if (listview.Name != "ListViewBillDay")
                            {
                                BillReportDetail.IntiliazeListView(ref listview);
                            }

                            List<BillReportDetail> billreportdetail
                                = new AccountBillSQL(DB).GetBillReport_Details_InDay(this.Year.ToString(), this.Month.ToString(), this.Day.ToString());
                            for (int i = 0; i < billreportdetail.Count; i++)
                            {
                                ListViewItem item = new ListViewItem(billreportdetail[i].BillDate.ToShortTimeString());
                                if (billreportdetail[i]._BillType == "صيانة")
                                {
                                    item.Name = 'M' + billreportdetail[i].BillID.ToString();
                                    item.BackColor = Color.PaleGoldenrod;
                                }
                                else if (billreportdetail[i]._BillType == "شراء")
                                {
                                    item.Name = 'O' + billreportdetail[i].BillID.ToString();
                                    item.BackColor = Color.Orange;
                                }
                                else
                                {

                                    item.Name = 'I' + billreportdetail[i].BillID.ToString();
                                    item.BackColor = Color.LightGreen;
                                }
                                item.SubItems.Add(billreportdetail[i].BillID.ToString());
                                item.SubItems.Add(billreportdetail[i]._BillType.ToString());
                                item.SubItems.Add(billreportdetail[i].BillDescription.ToString());
                                item.SubItems.Add(billreportdetail[i].BillOwner.ToString());
                                item.SubItems.Add(billreportdetail[i].BillOperations.ToString());

                                item.SubItems.Add(billreportdetail[i]._Currency);
                                item.SubItems.Add(billreportdetail[i].BillValue.ToString());
                                item.SubItems.Add(billreportdetail[i].Paid.ToString());
                                item.SubItems.Add(billreportdetail[i].Remain.ToString());

                                listview.Items.Add(item);

                            }
                            #endregion
                        }

                        else if (this.Month != null)
                        {
                            #region BillMonthReport
                            if (listview.Name != "ListViewBillMonth")
                            {
                                BillDayReportDetail.IntiliazeListView(ref listview);
                            }
                            List<BillDayReportDetail> BillDayReportDetailList
                                    = new AccountBillSQL(DB).GetBillReport_Details_InMonth(this.Year.ToString(), this.Month.ToString());
                            for (int i = 0; i < BillDayReportDetailList.Count; i++)
                            {
                                ListViewItem item = new ListViewItem(BillDayReportDetailList[i].DayDate.ToShortDateString());
                                item.UseItemStyleForSubItems = false;
                                item.Name = BillDayReportDetailList[i].DayID.ToString();
                                item.SubItems[0].BackColor = Color.LightGray;
                                item.SubItems.Add(BillDayReportDetailList[i].BillINCount.ToString());
                                item.SubItems.Add(BillDayReportDetailList[i].BillINValue);
                                item.SubItems.Add(BillDayReportDetailList[i].BillIN_PaysValue);
                                item.SubItems.Add(BillDayReportDetailList[i].BillMaintenanceCount.ToString());
                                item.SubItems.Add(BillDayReportDetailList[i].BillMaintenanceValue);
                                item.SubItems.Add(BillDayReportDetailList[i].BillMaintenance_PaysValue);
                                item.SubItems.Add(BillDayReportDetailList[i].BillOUTCount.ToString());
                                item.SubItems.Add(BillDayReportDetailList[i].BillOUTValue);
                                item.SubItems.Add(BillDayReportDetailList[i].BillOUT_PaysValue);
                                item.SubItems[1].BackColor = Color.LightGreen;
                                item.SubItems[2].BackColor = Color.LightGreen;
                                item.SubItems[3].BackColor = Color.LightGreen;
                                item.SubItems[4].BackColor = Color.PaleGoldenrod;
                                item.SubItems[5].BackColor = Color.PaleGoldenrod;
                                item.SubItems[6].BackColor = Color.PaleGoldenrod;
                                item.SubItems[7].BackColor = Color.Orange;
                                item.SubItems[8].BackColor = Color.Orange;
                                item.SubItems[9].BackColor = Color.Orange;
                                listview.Items.Add(item);

                            }
                            #endregion
                        }
                        else if (this.Year != null)
                        {
                            #region BillYearReport
                            if (listview.Name != "ListViewBillYear")
                            {
                                BillMonthReportDetail.IntiliazeListView(ref listview);
                            }
                            List<BillMonthReportDetail> BillMonthReportDetailList
                                    = new AccountBillSQL(DB).GetBillReport_Details_InYear(this.Year.ToString());
                            for (int i = 0; i < BillMonthReportDetailList.Count; i++)
                            {
                                ListViewItem item = new ListViewItem(BillMonthReportDetailList[i].Month);
                                item.UseItemStyleForSubItems = false;
                                item.Name = BillMonthReportDetailList[i].MonthID.ToString();
                                item.SubItems[0].BackColor = Color.LightGray;
                                item.SubItems.Add(BillMonthReportDetailList[i].BillINCount.ToString());
                                item.SubItems.Add(BillMonthReportDetailList[i].BillINValue);
                                item.SubItems.Add(BillMonthReportDetailList[i].BillIN_PaysValue);
                                item.SubItems.Add(BillMonthReportDetailList[i].BillMaintenanceCount.ToString());
                                item.SubItems.Add(BillMonthReportDetailList[i].BillMaintenanceValue);
                                item.SubItems.Add(BillMonthReportDetailList[i].BillMaintenance_PaysValue);
                                item.SubItems.Add(BillMonthReportDetailList[i].BillOUTCount.ToString());
                                item.SubItems.Add(BillMonthReportDetailList[i].BillOUTValue);
                                item.SubItems.Add(BillMonthReportDetailList[i].BillOUT_PaysValue);
                                item.SubItems[1].BackColor = Color.LightGreen;
                                item.SubItems[2].BackColor = Color.LightGreen;
                                item.SubItems[3].BackColor = Color.LightGreen;
                                item.SubItems[4].BackColor = Color.PaleGoldenrod;
                                item.SubItems[5].BackColor = Color.PaleGoldenrod;
                                item.SubItems[6].BackColor = Color.PaleGoldenrod;
                                item.SubItems[7].BackColor = Color.Orange;
                                item.SubItems[8].BackColor = Color.Orange;
                                item.SubItems[9].BackColor = Color.Orange;
                                listview.Items.Add(item);

                            }
                            #endregion
                        }
                        else
                        {
                            #region BillRangeYearReport
                            if (listview.Name != "ListViewBillYearRange")
                            {
                                BillYearReportDetail.IntiliazeListView(ref listview);
                            }
                            List<BillYearReportDetail> BillYearReportDetailList
                                    = new AccountBillSQL(DB).GetBillReport_Details_InYearRange(this.YearRange_.min_year.ToString(), this.YearRange_.max_year.ToString());
                            for (int i = 0; i < BillYearReportDetailList.Count; i++)
                            {
                                ListViewItem item = new ListViewItem(BillYearReportDetailList[i].Year.ToString());
                                item.UseItemStyleForSubItems = false;
                                item.Name = BillYearReportDetailList[i].Year.ToString();
                                item.SubItems[0].BackColor = Color.LightGray;
                                item.SubItems.Add(BillYearReportDetailList[i].BillINCount.ToString());
                                item.SubItems.Add(BillYearReportDetailList[i].BillINValue);
                                item.SubItems.Add(BillYearReportDetailList[i].BillIN_PaysValue);
                                item.SubItems.Add(BillYearReportDetailList[i].BillMaintenanceCount.ToString());
                                item.SubItems.Add(BillYearReportDetailList[i].BillMaintenanceValue);
                                item.SubItems.Add(BillYearReportDetailList[i].BillMaintenance_PaysValue);
                                item.SubItems.Add(BillYearReportDetailList[i].BillOUTCount.ToString());
                                item.SubItems.Add(BillYearReportDetailList[i].BillOUTValue);
                                item.SubItems.Add(BillYearReportDetailList[i].BillOUT_PaysValue);
                                item.SubItems[1].BackColor = Color.LightGreen;
                                item.SubItems[2].BackColor = Color.LightGreen;
                                item.SubItems[3].BackColor = Color.LightGreen;
                                item.SubItems[4].BackColor = Color.PaleGoldenrod;
                                item.SubItems[5].BackColor = Color.PaleGoldenrod;
                                item.SubItems[6].BackColor = Color.PaleGoldenrod;
                                item.SubItems[7].BackColor = Color.Orange;
                                item.SubItems[8].BackColor = Color.Orange;
                                item.SubItems[9].BackColor = Color.Orange;
                                listview.Items.Add(item);

                            }
                            #endregion
                        }
                        break;
                    #endregion
                    case 2:
                        #region PaySection
                        //if (this.Day != null)
                        //{
                            //#region PayDaySection
                            //listview.Items.Clear();
                            //if (listview.Name != "ListViewPayDay")
                            //{
                            //    AccountOprReportDetail.IntiliazeListView(ref listview);

                            //}
                            //List<AccountOprReportDetail> accountopr_reportlist
                            //          = new AccountOprSQL(DB).GetAccountOprReport_Details_InDay(this.Year.ToString(), this.Month.ToString(), this.Day.ToString());
                            //for (int i = 0; i < accountopr_reportlist.Count; i++)
                            //{
                            //    string payopridstr = accountopr_reportlist[i].OprType
                            //        + accountopr_reportlist[i].OprDirection
                            //        + accountopr_reportlist[i].OprID.ToString();
                            //    string payoprtype = "";
                            //    string Direction = "";
                            //    int oprtypeColor = 1; ;
                            //    if (accountopr_reportlist[i].OprType == "PAY")
                            //    {

                            //        payoprtype = "عملية دفع";
                            //    }
                            //    else
                            //    {
                            //        payoprtype = "عملية صرف";
                            //        oprtypeColor = 0;
                            //    }
                            //    string oprdirection = accountopr_reportlist[i].OprDirection.Replace(" ", string.Empty);
                            //    int oprtypeDirectionColor;
                            //    if (oprdirection == "IN")
                            //    {
                            //        Direction = "داخل الى الصندوق";
                            //        oprtypeDirectionColor = 0;
                            //    }
                            //    else
                            //    {
                            //        Direction = "خارج من الصندوق";
                            //        oprtypeDirectionColor = 1;
                            //    }
                            //    ListViewItem item = new ListViewItem(accountopr_reportlist[i].OprTime.ToShortTimeString());
                            //    item.Name = payopridstr;
                            //    item.SubItems.Add(payoprtype);
                            //    item.SubItems.Add(Direction);
                            //    item.SubItems.Add(accountopr_reportlist[i].OprID.ToString());
                            //    item.SubItems.Add(accountopr_reportlist[i].OprDescription);
                            //    item.SubItems.Add(accountopr_reportlist[i].Value.ToString());
                            //    item.SubItems.Add(accountopr_reportlist[i].Currency.ToString());
                            //    item.SubItems.Add(accountopr_reportlist[i].Details.ToString());
                            //    item.UseItemStyleForSubItems = false;
                            //    Color color;
                            //    if (oprtypeDirectionColor == 0 && oprtypeColor == 0) color = Color.YellowGreen;
                            //    else if (oprtypeDirectionColor == 1 && oprtypeColor == 0) color = Color.DarkOrange;
                            //    else if (oprtypeDirectionColor == 0 && oprtypeColor == 1) color = Color.LightGreen;
                            //    else color = Color.Orange;
                            //    item.UseItemStyleForSubItems = false;
                            //    item.SubItems[0].BackColor = color;
                            //    item.SubItems[1].BackColor = color;
                            //    item.SubItems[2].BackColor = color;
                            //    item.SubItems[3].BackColor = color;
                            //    item.SubItems[4].BackColor = color;
                            //    item.SubItems[5].BackColor = color;
                            //    item.SubItems[6].BackColor = color;
                            //    item.SubItems[7].BackColor = color;
                            //    listview.Items.Add(item);

                            //}
                            //#endregion
                        //}
                        //else if (this.Month != null)
                        //{
                        //    #region PayMonthSection
                        //    listview.Items.Clear();
                        //    if (listview.Name != "ListViewPayMonth")
                        //    {
                        //        AccountOprDayReportDetail.IntiliazeListView(ref listview);
                        //    }
                        //    List<AccountOprDayReportDetail> accountoprdayeportlist
                        //                        = new AccountOprSQL(DB).GetAccountOprReport_Details_InMonth(this.Year.ToString(), this.Month.ToString());
                        //    for (int i = 0; i < accountoprdayeportlist.Count; i++)
                        //    {
                        //        ListViewItem item = new ListViewItem(accountoprdayeportlist[i].Date_day.ToShortDateString());
                        //        item.Name = accountoprdayeportlist[i].DateDayNo.ToString();
                        //        item.SubItems.Add(accountoprdayeportlist[i].PaysIN_Count.ToString());
                        //        item.SubItems.Add(accountoprdayeportlist[i].PaysOUT_Count.ToString());
                        //        item.SubItems.Add(accountoprdayeportlist[i].Exchange_Count.ToString());
                        //        item.SubItems.Add(accountoprdayeportlist[i].PaysIN_Value);
                        //        item.SubItems.Add(accountoprdayeportlist[i].PaysOUT_Value);
                        //        item.UseItemStyleForSubItems = false;
                        //        item.SubItems[0].BackColor = Color.LightGray;
                        //        item.SubItems[1].BackColor = Color.LightGreen;
                        //        item.SubItems[2].BackColor = Color.Orange;
                        //        item.SubItems[3].BackColor = Color.Yellow;
                        //        item.SubItems[4].BackColor = Color.LightGreen;
                        //        item.SubItems[5].BackColor = Color.Orange;
                        //        listview.Items.Add(item);

                        //    }
                        //    #endregion
                        //}
                        //else if (this.Year != null)
                        //{
                        //    #region PayYearSection
                        //    listview.Items.Clear();
                        //    if (listview.Name != "ListViewPayYear")
                        //    {
                        //        AccountOprMonthReportDetail.IntiliazeListView(ref listview);
                        //    }
                        //    List<AccountOprMonthReportDetail> accountoprmonthreportlist
                        //           = new AccountOprSQL(DB).GetAccountOprReport_Details_InYear(this.Year.ToString());
                        //    for (int i = 0; i < accountoprmonthreportlist.Count; i++)
                        //    {
                        //        ListViewItem item = new ListViewItem(accountoprmonthreportlist[i].Year_Month_Name.ToString());
                        //        item.Name = accountoprmonthreportlist[i].Year_Month.ToString();
                        //        item.SubItems.Add(accountoprmonthreportlist[i].PaysIN_Count.ToString());
                        //        item.SubItems.Add(accountoprmonthreportlist[i].PaysOUT_Count.ToString());
                        //        item.SubItems.Add(accountoprmonthreportlist[i].Exchange_Count.ToString());
                        //        item.SubItems.Add(accountoprmonthreportlist[i].PaysIN_Value);
                        //        item.SubItems.Add(accountoprmonthreportlist[i].PaysOUT_Value.ToString());
                        //        item.UseItemStyleForSubItems = false;
                        //        item.SubItems[0].BackColor = Color.LightGray;
                        //        item.SubItems[1].BackColor = Color.LightGreen;
                        //        item.SubItems[2].BackColor = Color.Orange;
                        //        item.SubItems[3].BackColor = Color.Yellow;
                        //        item.SubItems[4].BackColor = Color.LightGreen;
                        //        item.SubItems[5].BackColor = Color.Orange;
                        //        listview.Items.Add(item);

                        //    }
                        //    #endregion
                        //}
                        //else
                        //{
                        //    #region PayYearRangeSection
                        //    listview.Items.Clear();
                        //    if (listview.Name != "ListViewPayYearRange")
                        //    {
                        //        AccountOprYearReportDetail.IntiliazeListView(ref listview);
                        //    }
                        //    List<AccountOprYearReportDetail> accountopryearreportlist
                        //            = new AccountOprSQL(DB).GetAccountOprReport_Details_InYearRange(this.YearRange_.min_year.ToString(), this.YearRange_.max_year.ToString());
                        //    for (int i = 0; i < accountopryearreportlist.Count; i++)
                        //    {
                        //        ListViewItem item = new ListViewItem(accountopryearreportlist[i].AccountYear.ToString());
                        //        item.Name = accountopryearreportlist[i].AccountYear.ToString();
                        //        item.SubItems.Add(accountopryearreportlist[i].PaysIN_Count.ToString());
                        //        item.SubItems.Add(accountopryearreportlist[i].PaysOUT_Count.ToString());
                        //        item.SubItems.Add(accountopryearreportlist[i].Exchange_Count.ToString());
                        //        item.SubItems.Add(accountopryearreportlist[i].PaysIN_Value);
                        //        item.SubItems.Add(accountopryearreportlist[i].PaysOUT_Value.ToString());
                        //        item.UseItemStyleForSubItems = false;
                        //        item.SubItems[0].BackColor = Color.LightGray;
                        //        item.SubItems[1].BackColor = Color.LightGreen;
                        //        item.SubItems[2].BackColor = Color.Orange;
                        //        item.SubItems[3].BackColor = Color.Yellow;
                        //        item.SubItems[4].BackColor = Color.LightGreen;
                        //        item.SubItems[5].BackColor = Color.Orange;
                        //        listview.Items.Add(item);

                        //    }
                        //    #endregion
                        //}
                        #endregion
                        break;
                    default:
                        listview.Items.Clear();
                        break;
                }
            }

            internal DateTime GetDate()
            {
                if (this.Year != -1 && this.Month != -1 && this.Day != -1)
                    return new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(Day)
                        ,DateTime .Now .Hour , DateTime.Now.Minute, DateTime.Now.Second );
                else return DateTime.Now;
            }

            public void IntializeListViewColumnsWidth(ref ListView listview)
            {
                //    if (this.Day != null)
                //    {
                //        if (ShowType == (int)ShowAccountType.Bill)
                //        {
                //            BillReportDetail.IntializeListViewColumnsWidth(ref listview);

                //        }
                //        else
                //        {
                //            AccountOprReportDetail.IntializeListViewColumnsWidth(ref listview);

                //        }
                //    }
                //    else
                //    {
                //        if (ShowType == (int)ShowAccountType.Bill)
                //        {
                //            BillMonthReportDetail .IntializeListViewColumnsWidth(ref listview);
                //        }
                //        else
                //        {
                //            AccountOprMonthReportDetail .IntializeListViewColumnsWidth(ref listview);
                //        }
                //    }
            }
            public void GetAccountReport(ref ListView listview)
            {
                //switch (ShowType)
                //{
                //    case (int)ShowAccountType.Bill:
                //        listview.Items.Clear();
                //        if (listview.Name != "ListViewBillsReport")
                //        {
                //            listview.Name = "ListViewBillsReport";
                //            listview.Columns.Clear();
                //            listview.Columns.Add("العملة");
                //            listview.Columns.Add("عدد فواتير المبيع");
                //            listview.Columns.Add("قيمتها");
                //            listview.Columns.Add("محصلة المدفوع");
                //            listview.Columns.Add("عدد فواتير الصيانة");
                //            listview.Columns.Add("قيمتها");
                //            listview.Columns.Add("محصلة المدفوع");
                //            listview.Columns.Add("عدد فواتير الشراء");
                //            listview.Columns.Add("قيمتها");
                //            listview.Columns.Add("محصلة المدفوع");
                //            IntializeListViewReportColumnsWidth(ref listview);

                //        }
                //        List<BillCurrencyReport> BillCurrencyReportList = new List<BillCurrencyReport>();
                //        if (this.Day != null)
                //        {
                //            BillCurrencyReportList = new AccountBillSQL(DB).GetBillReport_InDay(this .Year .ToString (),this .Month .ToString (),this .Day .ToString ());
                //        }
                //        else if (this.Month != null)
                //        {
                //            BillCurrencyReportList = new AccountBillSQL(DB).GetBillReport_InMonth(this.Year.ToString(), this.Month.ToString());
                //        }
                //        else if (this.Year != null)
                //        {
                //            BillCurrencyReportList = new AccountBillSQL(DB).GetBillReport_InYear(this.Year.ToString());
                //        }
                //        else
                //        {
                //            BillCurrencyReportList 
                //                = new AccountBillSQL(DB).GetBillReport_BetweenTwoYears(this.YearRange_.min_year.ToString(), this.YearRange_.max_year .ToString());
                //        }
                //        for (int i = 0; i < BillCurrencyReportList.Count; i++)
                //        {
                //            ListViewItem item = new ListViewItem(BillCurrencyReportList[i].CurrencyName);
                //            item.Name = BillCurrencyReportList[i].CurrencyID.ToString();
                //            item.SubItems.Add(BillCurrencyReportList[i].BillINCount.ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillINValue.ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillIN_PaysValue.ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillMaintenanceCount .ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillMaintenanceValue.ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillMaintenance_PaysValue.ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillOUTCount  .ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillOUTValue .ToString());
                //            item.SubItems.Add(BillCurrencyReportList[i].BillOUT_PaysValue .ToString());
                //            item.UseItemStyleForSubItems = false;
                //            item.SubItems[0].BackColor = Color.LightGray;
                //            item.SubItems[1].BackColor = Color.LightGreen;
                //            item.SubItems[2].BackColor = Color.LightGreen;
                //            item.SubItems[3].BackColor = Color.LightGreen;
                //            item.SubItems[4].BackColor = Color.PaleGoldenrod;
                //            item.SubItems[5].BackColor = Color.PaleGoldenrod;
                //            item.SubItems[6].BackColor = Color.PaleGoldenrod;
                //            item.SubItems[7].BackColor = Color.Orange;
                //            item.SubItems[8].BackColor = Color.Orange;
                //            item.SubItems[9].BackColor = Color.Orange;
                //            listview.Items.Add(item);

                //        }

                //        break;
                //    case (int)ShowAccountType.Pay:
                //        listview.Items.Clear();
                //        if (listview.Name != "ListViewPaysReport")
                //        {
                //            listview.Name = "ListViewPaysReport";
                //            listview.Columns.Clear();
                //            listview.Columns.Add("العملة");
                //            listview.Columns.Add("داخل عمليات الدفع");
                //            listview.Columns.Add("داخل عمليات الصرف");
                //            listview.Columns.Add("اجمالي الداخل");
                //            listview.Columns.Add("خارج عمليات الدفع");
                //            listview.Columns.Add("خارج عمليات الصرف");
                //            listview.Columns.Add("اجمالي الخارج");
                //            listview.Columns.Add("الصافي");
                //            IntializeListViewReportColumnsWidth(ref listview);

                //        }
                //        List<PayCurrencyReport > PayCurrencyReportList = new List<PayCurrencyReport>();
                //        if (this.Day != null)
                //        {
                //            PayCurrencyReportList = new AccountOprSQL(DB).GetPayReport_InDay(this.Year.ToString(), this.Month.ToString(), this.Day.ToString());
                //        }
                //        else if (this.Month != null)
                //        {
                //            PayCurrencyReportList = new AccountOprSQL(DB).GetPayReport_InMonth(this.Year.ToString(), this.Month.ToString());
                //        }
                //        else if (this.Year != null)
                //        {
                //            PayCurrencyReportList = new AccountOprSQL(DB).GetPayReport_INYear(this.Year.ToString());
                //        }
                //        else
                //        {
                //            PayCurrencyReportList
                //                = new AccountOprSQL(DB).GetPayReport_betweenTwoYears(this.YearRange_.min_year.ToString(), this.YearRange_.max_year.ToString());
                //        }
                //        for (int i = 0; i < PayCurrencyReportList.Count; i++)
                //        {
                //            ListViewItem item = new ListViewItem(PayCurrencyReportList[i].CurrencyName);
                //            item.Name = PayCurrencyReportList[i].CurrencyID.ToString();
                //            item.SubItems.Add(PayCurrencyReportList[i].PaysIN_Pays.ToString());
                //            item.SubItems.Add(PayCurrencyReportList[i].PaysIN_Exchange.ToString());
                //            item.SubItems.Add(PayCurrencyReportList[i].PaysIN_ALL.ToString());
                //            item.SubItems.Add(PayCurrencyReportList[i].PaysOUT_Pays.ToString());
                //            item.SubItems.Add(PayCurrencyReportList[i].PaysOUT_Exchange.ToString());
                //            item.SubItems.Add(PayCurrencyReportList[i].PaysOUT_ALL.ToString());
                //            item.SubItems.Add(PayCurrencyReportList[i].ClearValue .ToString());
                //            item.UseItemStyleForSubItems = false;
                //            item.SubItems[0].BackColor = Color.LightGray;
                //            item.SubItems[1].BackColor = Color.LightGreen;
                //            item.SubItems[2].BackColor = Color.LightGreen;
                //            item.SubItems[3].BackColor = Color.LightGreen;
                //            item.SubItems[4].BackColor = Color.Orange;
                //            item.SubItems[5].BackColor = Color.Orange;
                //            item.SubItems[6].BackColor = Color.Orange;
                //            item.SubItems[7].BackColor = Color.LightBlue ;
                //            listview.Items.Add(item);

                //        }
                //        break;
                //    default:
                //        listview.Items.Clear();
                //        break;
                //}
            }
            public void IntializeListViewReportColumnsWidth(ref ListView listview)
            {
                //switch (ShowType)
                //{
                //    case (int)ShowAccountType.Bill:
                //        listview.Columns[0].Width = 100;
                //        listview.Columns[1].Width = 135;
                //        listview.Columns[2].Width = (listview.Width - 515) / 6;
                //        listview.Columns[3].Width = (listview.Width - 515) / 6;

                //        listview.Columns[4].Width = 135;
                //        listview.Columns[5].Width = (listview.Width - 515) / 6;
                //        listview.Columns[6].Width = (listview.Width - 515) / 6;

                //        listview.Columns[7].Width = 135;
                //        listview.Columns[8].Width = (listview.Width - 515) / 6;
                //        listview.Columns[9].Width = (listview.Width - 515) / 6;
                //        break;
                //    case (int)ShowAccountType.Pay:
                //        listview.Columns[0].Width = 100;
                //        listview.Columns[1].Width = (listview.Width - 100) / 7;
                //        listview.Columns[2].Width = (listview.Width - 100) / 7;
                //        listview.Columns[3].Width = (listview.Width - 100) / 7;
                //        listview.Columns[4].Width = (listview.Width - 100) / 7;
                //        listview.Columns[5].Width = (listview.Width - 100) / 7;
                //        listview.Columns[6].Width = (listview.Width - 100) / 7;
                //        listview.Columns[7].Width = (listview.Width - 100) / 7;
                //        break;
                //    default:
                //        break;
                //}
            }
        }
        public class Currency
        {
            public uint CurrencyID;
            public string CurrencyName;
            public string CurrencySymbol;
            public double ExchangeRate;
            public uint? ReferenceCurrencyID;

            public Currency(uint CurrencyID_, string CurrencyName_, string CurrencySymbol_, double ExchangeRate_, uint? ReferenceCurrencyID_)
            {
                CurrencyID = CurrencyID_;
                CurrencyName = CurrencyName_;
                CurrencySymbol = CurrencySymbol_;
                ExchangeRate = ExchangeRate_;
                ReferenceCurrencyID = ReferenceCurrencyID_;
            }

        }
        public class ExchangeOPR
        {
            public uint ExchangeOprID;
            public DateTime ExchangeOprDate;
            public Currency SourceCurrency;
            public double SourceExchangeRate; 
            public double OutMoneyValue;
            public Currency TargetCurrency;
            public double TargetExchangeRate;
            public string Notes;
            public ExchangeOPR(uint ExchangeOprID_, DateTime ExchangeOprDate_,
                Currency SourceCurrency_, double SourceExchangeRate_, double OutMoneyValue_,  Currency TargetCurrency_, double TargetExchangeRate_, string Notes_)
            {
                ExchangeOprID = ExchangeOprID_;
                ExchangeOprDate = ExchangeOprDate_;
                SourceCurrency = SourceCurrency_;
                SourceExchangeRate  = SourceExchangeRate_;
                OutMoneyValue = OutMoneyValue_;
                TargetCurrency = TargetCurrency_;
                TargetExchangeRate = TargetExchangeRate_;
                Notes = Notes_;
            }
        }
        public class PayIN
        {
            public uint PayOprID;
            public DateTime PayOprDate;
            public Bill _Bill;
            public string PayDescription;
            public double Value;
            public Currency _Currency;
            public double ExchangeRate;
            public string Notes;

            public PayIN(uint PayOprID_, DateTime PayOprDate_, Bill Bill_, string PayDescription_, double Value_, double ExchangeRate_, Currency Currency_, string Notes_)
            {
                PayOprID = PayOprID_;
                PayOprDate = PayOprDate_;
                _Bill = Bill_;
                PayDescription = PayDescription_;
                Value = Value_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                Notes = Notes_;
            }
           

        }
        public class PayOUT
        {
            public uint PayOprID;
            public DateTime PayOprDate;
            public Bill _Bill;
            public string PayDescription;
            public double Value;
            public Currency _Currency;
            public double ExchangeRate;
            public string Notes;

            public PayOUT(uint PayOprID_, DateTime PayOprDate_, Bill Bill_, string PayDescription_, double Value_, double ExchangeRate_, Currency Currency_, string Notes_)
            {
                PayOprID = PayOprID_;
                PayOprDate = PayOprDate_;
                _Bill = Bill_;
                PayDescription = PayDescription_;
                Value = Value_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                Notes = Notes_;
            }

        }
        public class BillReportDetail
        {
            public DateTime BillDate;
            public int BillID;
            public string _BillType;
            public string BillDescription;
            public string BillOwner;
            public string BillOperations;
            public string   _Currency;
            public double BillValue;
            public double Paid;
            public double Remain;
            public BillReportDetail(DateTime BillDate_, int BillID_,
                string BillType_,
             string BillDescription_,
             string BillOwner_,
             string BillOperations_,
             string   Currency_,
             double BillValue_,
             double Paid_,
             double Remain_)
            {
                _BillType = BillType_;
                BillDate = BillDate_;
                BillID = BillID_;
                BillDescription = BillDescription_;
                BillOwner = BillOwner_;
                BillOperations = BillOperations_;
                _Currency = Currency_;
                BillValue = BillValue_;
                Paid = Paid_;
                Remain = Remain_;

            }
            public static void IntiliazeListView(ref ListView listview)
            {
               try
                {
                    listview.Name = "ListViewBillDay";
                    listview.Columns.Clear(); 
                    listview.Columns.Add("الوقت");
                    listview.Columns.Add("رقم الفاتورة");
                    listview.Columns.Add("طبيعة الفاتورة");
                    listview.Columns.Add("وصف الفاتورة");
                    listview.Columns.Add("الفاتورة باسم");
                    listview.Columns.Add("عمليات الفاتورة");
                    listview.Columns.Add("العملة");
                    listview.Columns.Add("قيمة الفاتورة");
                    listview.Columns.Add("محصلة المدفوع");
                    listview.Columns.Add("الباقي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch(Exception ee)
                {
                    MessageBox.Show("BillReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "",MessageBoxButtons.OK,MessageBoxIcon.Error );
                }
              
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {
                
                try
                {
                    listview.Columns[0].Width = 75;
                    listview.Columns[1].Width = 100;
                    listview.Columns[2].Width = 125;
                    listview.Columns[3].Width = (listview.Width - 725) / 3;
                    listview.Columns[4].Width = (listview.Width - 725) / 3;
                    listview.Columns[5].Width = (listview.Width - 725) / 3;
                    listview.Columns[6].Width = 100;
                    listview.Columns[7].Width = 100;
                    listview.Columns[8].Width = 125;
                    listview.Columns[9].Width = 100;
     
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public class BillDayReportDetail
        {
            public int DayID;
            public DateTime DayDate;
            public int BillINCount;
            public string BillINValue;
            public string BillIN_PaysValue;
            public int BillMaintenanceCount;
            public string BillMaintenanceValue;
            public string BillMaintenance_PaysValue;
            public int BillOUTCount;
            public string BillOUTValue;
            public string BillOUT_PaysValue;
            public BillDayReportDetail(
                    int DayID_,
                    DateTime DayDate_,
             int BillINCount_,
             string BillINValue_,
             string BillIN_PaysValue_,
             int BillMaintenanceCount_,
             string BillMaintenanceValue_,
             string BillMaintenance_PaysValue_,
             int BillOUTCount_,
             string BillOUTValue_,
             string BillOUT_PaysValue_)
            {
                DayID  = DayID_;
                DayDate = DayDate_;
                BillINCount = BillINCount_;
                BillINValue = BillINValue_;
                BillIN_PaysValue = BillIN_PaysValue_;
                BillMaintenanceCount = BillMaintenanceCount_;
                BillMaintenanceValue = BillMaintenanceValue_;
                BillMaintenance_PaysValue = BillMaintenance_PaysValue_;
                BillOUTCount = BillOUTCount_; ;
                BillOUTValue = BillOUTValue_;
                BillOUT_PaysValue = BillOUT_PaysValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {
   
                try
                {
                    listview.Name = "ListViewBillMonth";
                    listview.Columns.Clear();
                    listview.Columns.Add("اليوم");
                    listview.Columns.Add("عدد فواتير المبيع");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("عدد فواتير الصيانة");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("عدد فواتير الشراء");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("المدفوع");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch(Exception ee)
                {
                    MessageBox.Show("BillDayReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public  static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;
                    listview.Columns[1].Width = 135;
                    listview.Columns[2].Width = (listview.Width - 515) / 6;
                    listview.Columns[3].Width = (listview.Width - 515) / 6;

                    listview.Columns[4].Width = 135;
                    listview.Columns[5].Width = (listview.Width - 515) / 6;
                    listview.Columns[6].Width = (listview.Width - 515) / 6;

                    listview.Columns[7].Width = 135;
                    listview.Columns[8].Width = (listview.Width - 515) / 6;
                    listview.Columns[9].Width = (listview.Width - 515) / 6;

                }
                catch(Exception ee)
                {
                    MessageBox.Show("BillDayReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class BillMonthReportDetail
        {
            public int MonthID;
            public string Month;
            public int BillINCount;
            public string BillINValue;
            public string BillIN_PaysValue;
            public int BillMaintenanceCount;
            public string BillMaintenanceValue;
            public string BillMaintenance_PaysValue;
            public int BillOUTCount;
            public string BillOUTValue;
            public string BillOUT_PaysValue;
            public BillMonthReportDetail(
                 int MonthID_,
                    string Month_,
             int BillINCount_,
             string BillINValue_,
             string BillIN_PaysValue_,
             int BillMaintenanceCount_,
             string BillMaintenanceValue_,
             string BillMaintenance_PaysValue_,
             int BillOUTCount_,
             string BillOUTValue_,
             string BillOUT_PaysValue_
                )
            {
                MonthID = MonthID_;
                Month = Month_;
                BillINCount = BillINCount_;
                BillINValue = BillINValue_;
                BillIN_PaysValue = BillIN_PaysValue_;
                BillMaintenanceCount = BillMaintenanceCount_;
                BillMaintenanceValue = BillMaintenanceValue_;
                BillMaintenance_PaysValue = BillMaintenance_PaysValue_;
                BillOUTCount = BillOUTCount_; ;
                BillOUTValue = BillOUTValue_;
                BillOUT_PaysValue = BillOUT_PaysValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {
   
                try
                {
                    listview.Name = "ListViewBillYear";
                    listview.Columns.Clear();
                    listview.Columns.Add("الشهر");
                    listview.Columns.Add("عدد فواتير المبيع");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("عدد فواتير الصيانة");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("عدد فواتير الشراء");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("المدفوع");
                    IntializeListViewColumnsWidth(ref listview);

                }
                catch(Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;
                    listview.Columns[1].Width = 135;
                    listview.Columns[2].Width = (listview.Width - 515) / 6;
                    listview.Columns[3].Width = (listview.Width - 515) / 6;

                    listview.Columns[4].Width = 135;
                    listview.Columns[5].Width = (listview.Width - 515) / 6;
                    listview.Columns[6].Width = (listview.Width - 515) / 6;

                    listview.Columns[7].Width = 135;
                    listview.Columns[8].Width = (listview.Width - 515) / 6;
                    listview.Columns[9].Width = (listview.Width - 515) / 6;

                }
                catch(Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntializeListViewColumnsWidth"+Environment.NewLine+ee.Message  , "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class BillYearReportDetail
        {

            public int Year;
            public int BillINCount;
            public string BillINValue;
            public string BillIN_PaysValue;
            public int BillMaintenanceCount;
            public string BillMaintenanceValue;
            public string BillMaintenance_PaysValue;
            public int BillOUTCount;
            public string BillOUTValue;
            public string BillOUT_PaysValue;
            public BillYearReportDetail(
                    int Year_,
             int BillINCount_,
             string BillINValue_,
             string BillIN_PaysValue_,
             int BillMaintenanceCount_,
             string BillMaintenanceValue_,
             string BillMaintenance_PaysValue_,
             int BillOUTCount_,
             string BillOUTValue_,
             string BillOUT_PaysValue_)
            {
                Year = Year_;
                BillINCount = BillINCount_;
                BillINValue = BillINValue_;
                BillIN_PaysValue = BillIN_PaysValue_;
                BillMaintenanceCount = BillMaintenanceCount_;
                BillMaintenanceValue = BillMaintenanceValue_;
                BillMaintenance_PaysValue = BillMaintenance_PaysValue_;
                BillOUTCount = BillOUTCount_; ;
                BillOUTValue = BillOUTValue_;
                BillOUT_PaysValue = BillOUT_PaysValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {
                listview.Name = "ListViewBillYearRange";
                listview.Columns.Clear();
                listview.Columns.Add("السنة");
                listview.Columns.Add("عدد فواتير المبيع");
                listview.Columns.Add("القيمة");
                listview.Columns.Add("المدفوع");
                listview.Columns.Add("عدد فواتير الصيانة");
                listview.Columns.Add("القيمة");
                listview.Columns.Add("المدفوع");
                listview.Columns.Add("عدد فواتير الشراء");
                listview.Columns.Add("القيمة");
                listview.Columns.Add("المدفوع");
                IntializeListViewColumnsWidth(ref listview);
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {
                listview.Columns[0].Width = 100;
                listview.Columns[1].Width = 135;
                listview.Columns[2].Width = (listview.Width - 515) / 6;
                listview.Columns[3].Width = (listview.Width - 515) / 6;

                listview.Columns[4].Width = 135;
                listview.Columns[5].Width = (listview.Width - 515) / 6;
                listview.Columns[6].Width = (listview.Width - 515) / 6;

                listview.Columns[7].Width = 135;
                listview.Columns[8].Width = (listview.Width - 515) / 6;
                listview.Columns[9].Width = (listview.Width - 515) / 6;
            }

        }
        public class BillCurrencyReport
        {
            public int CurrencyID;
            public string CurrencyName;
            public int BillINCount;
            public double  BillINValue;
            public double BillIN_PaysValue;
            public int BillMaintenanceCount;
            public double BillMaintenanceValue;
            public double BillMaintenance_PaysValue;
            public int BillOUTCount;
            public double BillOUTValue;
            public double BillOUT_PaysValue;
            public BillCurrencyReport(
                  int CurrencyID_,
             string CurrencyName_,
             int BillINCount_,
             double BillINValue_,
             double BillIN_PaysValue_,
             int BillMaintenanceCount_,
             double BillMaintenanceValue_,
             double BillMaintenance_PaysValue_,
             int BillOUTCount_,
             double BillOUTValue_,
             double BillOUT_PaysValue_)
            {
                 CurrencyID= CurrencyID_;
             CurrencyName= CurrencyName_;
                BillINCount = BillINCount_;
                BillINValue = BillINValue_;
                BillIN_PaysValue = BillIN_PaysValue_;
                BillMaintenanceCount = BillMaintenanceCount_;
                BillMaintenanceValue = BillMaintenanceValue_;
                BillMaintenance_PaysValue = BillMaintenance_PaysValue_;
                BillOUTCount = BillOUTCount_; ;
                BillOUTValue = BillOUTValue_;
                BillOUT_PaysValue = BillOUT_PaysValue_;
            }
        }

        #region Report_Buy
        public class Report_Buys_Day_ReportDetail
        {

            public DateTime Bill_Time;
            public int Bill_ID;
            public string Bill_Owner;
            public int ClauseS_Count;
            public double Amount_IN;
            public double Amount_Remain;
            public double BillValue;
            public Currency _Currency;
            public double ExchangeRate;
            public string PaysAmount;
            public double PaysRemain;
            public double Bill_RealValue;
            public double Bill_Pays_RealValue;
            public string Bill_ItemsOut_Value;
            public double Bill_ItemsOut_RealValue;
            public string  Bill_Pays_Return_Value;
            public double Bill_Pays_Return_RealValue;

            public Report_Buys_Day_ReportDetail(DateTime Bill_Time_,
             int Bill_ID_,
             string Bill_Owner_,
             int ClauseS_Count_,
             double Amount_IN_,
             double Amount_Remain_,
             double BillValue_,
             Currency Currency_,
             double ExchangeRate_,
             string PaysAmount_,
             double PaysRemain_,
             double Bill_RealValue_,
             double Bill_Pays_RealValue_,
             string Bill_ItemsOut_Value_,
             double Bill_ItemsOut_RealValue_,
             string  Bill_Pays_Return_Value_,
             double Bill_Pays_Return_RealValue_
               )
            {
            Bill_Time= Bill_Time_;
            Bill_ID= Bill_ID_;
            Bill_Owner= Bill_Owner_;
            ClauseS_Count= ClauseS_Count_;
            Amount_IN= Amount_IN_;
            Amount_Remain= Amount_Remain_;
            BillValue= BillValue_;
            _Currency= Currency_;
            ExchangeRate= ExchangeRate_;
            PaysAmount= PaysAmount_;
            PaysRemain= PaysRemain_;
            Bill_RealValue= Bill_RealValue_;
            Bill_Pays_RealValue= Bill_Pays_RealValue_;
            Bill_ItemsOut_Value= Bill_ItemsOut_Value_;
            Bill_ItemsOut_RealValue = Bill_ItemsOut_RealValue_;
            Bill_Pays_Return_Value = Bill_Pays_Return_Value_;
            Bill_Pays_Return_RealValue= Bill_Pays_Return_RealValue_;

        }
        public static void IntiliazeListView(ref ListView listview)
            {
                try
                {
                    listview.Name = "ListViewBuysDay";
                    listview.Columns.Clear();
                    listview.Columns.Add("الوقت");
                    listview.Columns.Add("الرقم");
                    listview.Columns.Add("باسم");
                    listview.Columns.Add("البنود");
                    listview.Columns.Add("اجمالي الكميات");
                    listview.Columns.Add("الكمية المتبقية");
                    listview.Columns.Add("قيمة الفاتورة");
                    listview.Columns.Add("سعر الصرف");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("الباقي");
                    listview.Columns.Add("قيمة الفاتور الفعلية");
                    listview.Columns.Add(" المدفوع الفعلي");
                    listview.Columns.Add("قيمة  الخارج");
                    listview.Columns.Add(" عائدات الفاتورة");
                    listview.Columns.Add("العائدات الفعلية");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Report_Buy_Day_Detail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 75;//time
                    listview.Columns[1].Width = 60;//id
                    listview.Columns[2].Width = 100;//owner
                    listview.Columns[3].Width = 60;//clause count
                    listview.Columns[4].Width = 125;//amount in
                    listview.Columns[5].Width = 125;//amount remain
                    listview.Columns[6].Width = 100;//value
                    listview.Columns[7].Width = 100;//exchangerate
                    listview.Columns[8].Width = 100;//paid
                    listview.Columns[9].Width = 100;//remain
                    listview.Columns[10].Width = 140;//قيمة الفاتور الفعلية
                    listview.Columns[11].Width = 150;// المدفوع الفعلي
                    listview.Columns[12].Width = 140;//قيمة  الخارج
                    listview.Columns[13].Width = 140;//عائدات الفاتورة
                    listview.Columns[14].Width = 140;//القيمة العلية للعائدات

                }
                catch (Exception ee)
                {
                    MessageBox.Show("Report_Buy_Day_Detail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_Buys_Month_ReportDetail
        {

            public int DayID;
            public DateTime DayDate;
            public int Bills_Count;
            public int Bills_Clause_Count;
            public double Amount_IN;
            public double Amount_Remain;
            public string Bills_Value;
  
            public string Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double Bills_Pays_Remain_UPON_Bill_Currency;

            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public string Bills_ItemsOut_Value;
            public double Bills_ItemsOut_RealValue;
            public string Bills_Pays_Return_Value;
            public double Bills_Pays_Return_RealValue;
            public Report_Buys_Month_ReportDetail(
                    int DayID_,
                    DateTime DayDate_,
              int Bills_Count_,
            int Bills_Clause_Count_,
             double Amount_IN_,
             double Amount_Remain_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,

             double Bills_Pays_Remain_UPON_Bill_Currency_,

             double Bills_RealValue_,
             double Bills_Pays_RealValue_,
             string Bills_ItemsOut_Value_,
             double Bills_ItemsOut_RealValue_,
             string Bills_Pays_Return_Value_,
             double Bills_Pays_Return_RealValue_)
            {
                DayID = DayID_;
                DayDate = DayDate_;
                Bills_Count= Bills_Count_;
             Bills_Clause_Count= Bills_Clause_Count_;
             Amount_IN= Amount_IN_;
            Amount_Remain= Amount_Remain_;
            Bills_Value= Bills_Value_;

            Bills_Pays_Value= Bills_Pays_Value_;
            Bills_Pays_Remain= Bills_Pays_Remain_;
            Bills_Pays_Remain_UPON_Bill_Currency= Bills_Pays_Remain_UPON_Bill_Currency_;

             Bills_RealValue= Bills_RealValue_;
            Bills_Pays_RealValue= Bills_Pays_RealValue_;
           Bills_ItemsOut_Value= Bills_ItemsOut_Value_;
            Bills_ItemsOut_RealValue= Bills_ItemsOut_RealValue_;
           Bills_Pays_Return_Value= Bills_Pays_Return_Value_;
            Bills_Pays_Return_RealValue= Bills_Pays_Return_RealValue_;
        }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewBuys_Month";
                    listview.Columns.Clear();
                    listview.Columns.Add("اليوم");
                    listview.Columns.Add("العدد الكلي");
                    listview.Columns.Add("اجمالي البنود");
                    listview.Columns.Add("الكمية الداخلة");
                    listview.Columns.Add("الكمية المتبقية");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    listview.Columns.Add("قيمة الخارج");
                    listview.Columns.Add(" الخارج الفعلي");
                    listview.Columns.Add("قيمة العائدات");
                    listview.Columns.Add(" العائد الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//daydate
                    listview.Columns[1].Width = 120;//bills count
                    listview.Columns[2].Width = 115;//clause count
                    listview.Columns[3].Width = 125;//amount in
                    listview.Columns[4].Width = 125;//amount remain
                    listview.Columns[5].Width = 120;//bill value
                    listview.Columns[6].Width = 140;//paid
                    listview.Columns[7].Width = 115;//remain

                    listview.Columns[8].Width = 125;//real value
                    listview.Columns[9].Width = 125;//real pays value
                    listview.Columns[10].Width = 125;//out value
                    listview.Columns[11].Width = 125;//out real value
                    listview.Columns[12].Width = 125;//pays return value
                    listview.Columns[13].Width = 125;//pays return  real value


                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_Buys_Year_ReportDetail
        {

            public int MonthNO;
            public string MonthName;
            public int Bills_Count;
            public int Bills_Clause_Count;
            public double Amount_IN;
            public double Amount_Remain;
            public string Bills_Value;

            public string Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double Bills_Pays_Remain_UPON_Bill_Currency;

            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public string Bills_ItemsOut_Value;
            public double Bills_ItemsOut_RealValue;
            public string Bills_Pays_Return_Value;
            public double Bills_Pays_Return_RealValue;

            public Report_Buys_Year_ReportDetail(
                    int MOnthNO_,
                    string MonthName_,
               int Bills_Count_,
            int Bills_Clause_Count_,
             double Amount_IN_,
             double Amount_Remain_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,

             double Bills_Pays_Remain_UPON_Bill_Currency_,

             double Bills_RealValue_,
             double Bills_Pays_RealValue_,
             string Bills_ItemsOut_Value_,
             double Bills_ItemsOut_RealValue_,
             string Bills_Pays_Return_Value_,
             double Bills_Pays_Return_RealValue_)
            {
                MonthNO = MOnthNO_;
                MonthName = MonthName_;
                Bills_Count = Bills_Count_;
                Bills_Clause_Count = Bills_Clause_Count_;
                Amount_IN = Amount_IN_;
                Amount_Remain = Amount_Remain_;

                Bills_Value = Bills_Value_;
                Bills_Pays_Value = Bills_Pays_Value_;
                Bills_Pays_Remain = Bills_Pays_Remain_;
                Bills_Pays_Remain_UPON_Bill_Currency = Bills_Pays_Remain_UPON_Bill_Currency_;

                Bills_RealValue = Bills_RealValue_;
                Bills_Pays_RealValue = Bills_Pays_RealValue_;
                Bills_ItemsOut_Value = Bills_ItemsOut_Value_;
                Bills_ItemsOut_RealValue = Bills_ItemsOut_RealValue_;
                Bills_Pays_Return_Value = Bills_Pays_Return_Value_;
                Bills_Pays_Return_RealValue = Bills_Pays_Return_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewBuys_Year";
                    listview.Columns.Clear();
                    listview.Columns.Add("الشهر");
                    listview.Columns.Add("العدد الكلي");
                    listview.Columns.Add("اجمالي البنود");
                    listview.Columns.Add("الكمية الداخلة");
                    listview.Columns.Add("الكمية المتبقية");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    listview.Columns.Add("قيمة الخارج");
                    listview.Columns.Add(" الخارج الفعلي");
                    listview.Columns.Add("قيمة العائدات");
                    listview.Columns.Add(" العائد الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//month
                    listview.Columns[1].Width = 120;//bills count
                    listview.Columns[2].Width = 115;//clause count
                    listview.Columns[3].Width = 125;//amount in
                    listview.Columns[4].Width = 125;//amount remain
                    listview.Columns[5].Width = 120;//bill value
                    listview.Columns[6].Width = 140;//paid
                    listview.Columns[7].Width = 115;//remain

                    listview.Columns[8].Width = 125;//real value
                    listview.Columns[9].Width = 125;//real pays value
                    listview.Columns[10].Width = 125;//out value
                    listview.Columns[11].Width = 125;//out real value
                    listview.Columns[12].Width = 125;//pays return value
                    listview.Columns[13].Width = 125;//pays return  real value


                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_Buys_YearRange_ReportDetail
        {

            public int YearNO;
            public int Bills_Count;
            public int Bills_Clause_Count;
            public double Amount_IN;
            public double Amount_Remain;
            public string Bills_Value;

            public string Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double Bills_Pays_Remain_UPON_Bill_Currency;

            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public string Bills_ItemsOut_Value;
            public double Bills_ItemsOut_RealValue;
            public string Bills_Pays_Return_Value;
            public double Bills_Pays_Return_RealValue;
            public Report_Buys_YearRange_ReportDetail(
                    int YearNO_,
              int Bills_Count_,
            int Bills_Clause_Count_,
             double Amount_IN_,
             double Amount_Remain_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,

             double Bills_Pays_Remain_UPON_Bill_Currency_,

             double Bills_RealValue_,
             double Bills_Pays_RealValue_,
             string Bills_ItemsOut_Value_,
             double Bills_ItemsOut_RealValue_,
             string Bills_Pays_Return_Value_,
             double Bills_Pays_Return_RealValue_)
            {
                YearNO = YearNO_;

                Bills_Count = Bills_Count_;
                Bills_Clause_Count = Bills_Clause_Count_;
                Amount_IN = Amount_IN_;
                Amount_Remain = Amount_Remain_;
                Bills_Value = Bills_Value_;

                Bills_Pays_Value = Bills_Pays_Value_;
                Bills_Pays_Remain = Bills_Pays_Remain_;
                Bills_Pays_Remain_UPON_Bill_Currency = Bills_Pays_Remain_UPON_Bill_Currency_;

                Bills_RealValue = Bills_RealValue_;
                Bills_Pays_RealValue = Bills_Pays_RealValue_;
                Bills_ItemsOut_Value = Bills_ItemsOut_Value_;
                Bills_ItemsOut_RealValue = Bills_ItemsOut_RealValue_;
                Bills_Pays_Return_Value = Bills_Pays_Return_Value_;
                Bills_Pays_Return_RealValue = Bills_Pays_Return_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewBuys_YearRange";
                    listview.Columns.Clear();
                    listview.Columns.Add("السنة");
                    listview.Columns.Add("العدد الكلي");
                    listview.Columns.Add("اجمالي البنود");
                    listview.Columns.Add("الكمية الداخلة");
                    listview.Columns.Add("الكمية المتبقية");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    listview.Columns.Add("قيمة الخارج");
                    listview.Columns.Add(" الخارج الفعلي");
                    listview.Columns.Add("قيمة العائدات");
                    listview.Columns.Add(" العائد الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearRangeReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//year
                    listview.Columns[1].Width = 120;//bills count
                    listview.Columns[2].Width = 115;//clause count
                    listview.Columns[3].Width = 125;//amount in
                    listview.Columns[4].Width = 125;//amount remain
                    listview.Columns[5].Width = 120;//bill value
                    listview.Columns[6].Width = 140;//paid
                    listview.Columns[7].Width = 115;//remain

                    listview.Columns[8].Width = 125;//real value
                    listview.Columns[9].Width = 125;//real pays value
                    listview.Columns[10].Width = 125;//out value
                    listview.Columns[11].Width = 125;//out real value
                    listview.Columns[12].Width = 125;//pays return value
                    listview.Columns[13].Width = 125;//pays return  real value


                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearRangeReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        #endregion
        #region Report_MaintenanceOPR

        public class Report_MaintenanceOPRs_Day_ReportDetail
        {

            public DateTime MaintenanceOPR_Time;
            public int MaintenanceOPR_ID;
            public string MaintenanceOPR_Owner;
            public ItemObj.Objects.Item _Item;
            public string FalutDesc;
            public DateTime? MaintenanceOPR_Endworkdate;
            public bool? MaintenanceOPR_Rpaired;
            public DateTime? MaintenanceOPR_DeliverDate;
            public DateTime? MaintenanceOPR_EndWarrantyDate;
            public uint? BillMaintenanceID;
            public double? BillValue;
            public Currency _Currency;
            public double? ExchangeRate;
            public string PaysAmount;
            public double? PaysRemain;
            public string Bill_ItemsOut_Value;
            public double? Bill_ItemsOut_RealValue;
            public double? Bill_RealValue;
            public double? Bill_Pays_RealValue;





            public Report_MaintenanceOPRs_Day_ReportDetail(
                DateTime MaintenanceOPR_Time_,
             int MaintenanceOPR_ID_,
             string MaintenanceOPR_Owner_,
             ItemObj.Objects.Item Item_,
             string FalutDesc_,
             DateTime? MaintenanceOPR_Endworkdate_,
             bool? MaintenanceOPR_Rpaired_,
             DateTime? MaintenanceOPR_DeliverDate_,
             DateTime? MaintenanceOPR_EndWarrantyDate_,
             uint? BillMaintenanceID_,
              double? BillValue_,
             Currency Currency_,
             double? ExchangeRate_,
             string PaysAmount_,
             double? PaysRemain_,
            string Bill_ItemsOut_Value_,
             double? Bill_ItemsOut_RealValue_,
             double? Bill_RealValue_,
             double? Bill_Pays_RealValue_


               )
            {
                MaintenanceOPR_Time = MaintenanceOPR_Time_;
                MaintenanceOPR_ID = MaintenanceOPR_ID_;
                MaintenanceOPR_Owner = MaintenanceOPR_Owner_;
                _Item = Item_;
                FalutDesc = FalutDesc_;
                MaintenanceOPR_Endworkdate = MaintenanceOPR_Endworkdate_;
                MaintenanceOPR_Rpaired = MaintenanceOPR_Rpaired_;
                MaintenanceOPR_DeliverDate = MaintenanceOPR_DeliverDate_;
                MaintenanceOPR_EndWarrantyDate = MaintenanceOPR_EndWarrantyDate_;
                BillMaintenanceID = BillMaintenanceID_;
                BillValue = BillValue_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                PaysAmount = PaysAmount_;
                PaysRemain = PaysRemain_;
                Bill_ItemsOut_Value = Bill_ItemsOut_Value_;
                Bill_ItemsOut_RealValue = Bill_ItemsOut_RealValue_;
                Bill_RealValue = Bill_RealValue_;
                Bill_Pays_RealValue = Bill_Pays_RealValue_;


            }
            public static void IntiliazeListView(ref ListView listview)
            {
                try
                {
                  
                    listview.Name = "ListViewMaintenanceOPRs_Day";
                    listview.Columns.Clear();
                    listview.Columns.Add("الوقت");
                    listview.Columns.Add("الرقم");
                    listview.Columns.Add("باسم");
                    listview.Columns.Add("الموديل");
                    listview.Columns.Add("الشركة");
                    listview.Columns.Add("الصنف");
                    listview.Columns.Add("وصف العطل");
                    listview.Columns.Add(" انتهاء العمل");
                    listview.Columns.Add("الاصلاح");
                    listview.Columns.Add(" تسليم الجهاز");
                    listview.Columns.Add(" انتهاء الكفالة");
                    listview.Columns.Add("قيمة الفاتورة");
                    listview.Columns.Add(" سعر الصرف");
                    listview.Columns.Add(" المدفوع ");
                    listview.Columns.Add(" المتبقي ");
                    listview.Columns.Add("قيمة  المواد ");
                    listview.Columns.Add("قيمة  المواد  الفعلية");
                    listview.Columns.Add(" قيمة الفاتورة الفعلية");
                    listview.Columns.Add(" المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Report_MaintenanceOPR_Day_Detail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 75;//time
                    listview.Columns[1].Width = 60;//id
                    listview.Columns[2].Width = 100;//owner
                    listview.Columns[3].Width = 125;//model
                    listview.Columns[4].Width = 125;//company
                    listview.Columns[5].Width = 125;//type
                    listview.Columns[6].Width = 125;//fault desc
                    listview.Columns[7].Width = 125;//end work date
                    listview.Columns[8].Width = 125;//repair
                    listview.Columns[9].Width = 125;//deliver date
                    listview.Columns[10].Width = 125;//end warranty date
                    listview.Columns[11].Width = 125;//قيمة الفاتورة
                    listview.Columns[12].Width = 125;//سعر الصرف
                    listview.Columns[13].Width = 125;//المدفوع
                    listview.Columns[14].Width = 125;//المتبقي
                    listview.Columns[15].Width = 155;//قيمة  الخارج
                    listview.Columns[16].Width = 160;//الخارج الفعلي 
                    listview.Columns[17].Width = 155;//قيمة الفاتور الفعلية
                    listview.Columns[18].Width = 150;// المدفوع الفعلي

                }
                catch (Exception ee)
                {
                    MessageBox.Show("Report_MaintenanceOPR_Day_Detail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_MaintenanceOPRs_Month_ReportDetail
        {

            public int DayID;
            public DateTime DayDate;
            public int MaintenanceOPRs_Count;
            public int MaintenanceOPRs_EndWork_Count;
            public int MaintenanceOPRs_Repaired_Count;
            public int MaintenanceOPRs_Warranty_Count;
            public int MaintenanceOPRs_EndWarranty_Count;

            public int BillMaintenances_Count;
            public string BillMaintenances_Value;
            public string BillMaintenances_Pays_Value;
            public string BillMaintenances_Pays_Remain;
            public double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency;

            public string BillMaintenances_ItemsOut_Value;
            public double BillMaintenances_ItemsOut_RealValue;
            public double BillMaintenances_RealValue;
            public double BillMaintenances_Pays_RealValue;
            public Report_MaintenanceOPRs_Month_ReportDetail(
                    int DayID_,
                    DateTime DayDate_,
               int MaintenanceOPRs_Count_,
             int MaintenanceOPRs_EndWork_Count_,
             int MaintenanceOPRs_Repaired_Count_,
             int MaintenanceOPRs_Warranty_Count_,
             int MaintenanceOPRs_EndWarranty_Count_,

               int BillMaintenances_Count_,
             string BillMaintenances_Value_,
             string BillMaintenances_Pays_Value_,
             string BillMaintenances_Pays_Remain_,
             double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_,

             string BillMaintenances_ItemsOut_Value_,
             double BillMaintenances_ItemsOut_RealValue_,
             double BillMaintenances_RealValue_,
             double BillMaintenances_Pays_RealValue_)
            {
                DayID = DayID_;
                DayDate = DayDate_;
                MaintenanceOPRs_Count = MaintenanceOPRs_Count_;
                MaintenanceOPRs_EndWork_Count = MaintenanceOPRs_EndWork_Count_;
                MaintenanceOPRs_Repaired_Count = MaintenanceOPRs_Repaired_Count_;
                MaintenanceOPRs_Warranty_Count = MaintenanceOPRs_Warranty_Count_;
                MaintenanceOPRs_EndWarranty_Count = MaintenanceOPRs_EndWarranty_Count_;

                BillMaintenances_Count = BillMaintenances_Count_;
                BillMaintenances_Value = BillMaintenances_Value_;
                BillMaintenances_Pays_Value = BillMaintenances_Pays_Value_;
                BillMaintenances_Pays_Remain = BillMaintenances_Pays_Remain_;
                BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency = BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_;

                BillMaintenances_ItemsOut_Value = BillMaintenances_ItemsOut_Value_;
                BillMaintenances_ItemsOut_RealValue = BillMaintenances_ItemsOut_RealValue_;
                BillMaintenances_RealValue = BillMaintenances_RealValue_;
                BillMaintenances_Pays_RealValue = BillMaintenances_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewMaintenanceOPRs_Month";
                    listview.Columns.Clear();
                    listview.Columns.Add("اليوم");
                    listview.Columns.Add("عدد عمليات الصيانة");
                    listview.Columns.Add("ما تم انهاءه");
                    listview.Columns.Add("ماتم اصلاحه");
                    listview.Columns.Add("عدد المكفول");
                    listview.Columns.Add("المنتهي كفالته");
                    listview.Columns.Add(" قمة فواتير الصيانة");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("قيمة المواد المركبة");
                    listview.Columns.Add("قيمة المواد المركبة الفعلية");
                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//daydate
                    listview.Columns[1].Width = 120;//عدد عمليات الصيانة
                    listview.Columns[2].Width = 115;//ما تم انهائه
                    listview.Columns[3].Width = 125;//ما تم اصلاحه
                    listview.Columns[4].Width = 125;//عدد المكفول
                    listview.Columns[5].Width = 120;//المنتهي كفالته
                    listview.Columns[6].Width = 140;//قيمة فواتير الصيانة
                    listview.Columns[7].Width = 115;//المدفوع

                    listview.Columns[8].Width = 125;//المتبقي
                    listview.Columns[9].Width = 125;//قيمة العناصر المركبة
                    listview.Columns[10].Width = 125;//قيمة المركب الفعلي
                    listview.Columns[11].Width = 125;//قيمة الفواتير الفعلية
                    listview.Columns[12].Width = 125;//قيمة المدفوع الفعلي



                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_MaintenanceOPRs_Year_ReportDetail
        {

            public int MonthNO;
            public string MonthName;
            public int MaintenanceOPRs_Count;
            public int MaintenanceOPRs_EndWork_Count;
            public int MaintenanceOPRs_Repaired_Count;
            public int MaintenanceOPRs_Warranty_Count;
            public int MaintenanceOPRs_EndWarranty_Count;

            public int BillMaintenances_Count;
            public string BillMaintenances_Value;
            public string BillMaintenances_Pays_Value;
            public string BillMaintenances_Pays_Remain;
            public double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency;

            public string BillMaintenances_ItemsOut_Value;
            public double BillMaintenances_ItemsOut_RealValue;
            public double BillMaintenances_RealValue;
            public double BillMaintenances_Pays_RealValue;

            public Report_MaintenanceOPRs_Year_ReportDetail(
                    int MOnthNO_,
                    string MonthName_,
                 int MaintenanceOPRs_Count_,
             int MaintenanceOPRs_EndWork_Count_,
             int MaintenanceOPRs_Repaired_Count_,
             int MaintenanceOPRs_Warranty_Count_,
             int MaintenanceOPRs_EndWarranty_Count_,

               int BillMaintenances_Count_,
             string BillMaintenances_Value_,
             string BillMaintenances_Pays_Value_,
             string BillMaintenances_Pays_Remain_,
             double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_,

             string BillMaintenances_ItemsOut_Value_,
             double BillMaintenances_ItemsOut_RealValue_,
             double BillMaintenances_RealValue_,
             double BillMaintenances_Pays_RealValue_)
            {
                MonthNO = MOnthNO_;
                MonthName = MonthName_;
                MaintenanceOPRs_Count = MaintenanceOPRs_Count_;
                MaintenanceOPRs_EndWork_Count = MaintenanceOPRs_EndWork_Count_;
                MaintenanceOPRs_Repaired_Count = MaintenanceOPRs_Repaired_Count_;
                MaintenanceOPRs_Warranty_Count = MaintenanceOPRs_Warranty_Count_;
                MaintenanceOPRs_EndWarranty_Count = MaintenanceOPRs_EndWarranty_Count_;

                BillMaintenances_Count = BillMaintenances_Count_;
                BillMaintenances_Value = BillMaintenances_Value_;
                BillMaintenances_Pays_Value = BillMaintenances_Pays_Value_;
                BillMaintenances_Pays_Remain = BillMaintenances_Pays_Remain_;
                BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency = BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_;

                BillMaintenances_ItemsOut_Value = BillMaintenances_ItemsOut_Value_;
                BillMaintenances_ItemsOut_RealValue = BillMaintenances_ItemsOut_RealValue_;
                BillMaintenances_RealValue = BillMaintenances_RealValue_;
                BillMaintenances_Pays_RealValue = BillMaintenances_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewMaintenanceOPRs_Year";
                    listview.Columns.Clear();
                    listview.Columns.Add("الشهر");
                    listview.Columns.Add("عدد عمليات الصيانة");
                    listview.Columns.Add("ما تم انهاءه");
                    listview.Columns.Add("ماتم اصلاحه");
                    listview.Columns.Add("عدد المكفول");
                    listview.Columns.Add("المنتهي كفالته");
                    listview.Columns.Add(" قمة فواتير الصيانة");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("قيمة المواد المركبة");
                    listview.Columns.Add("قيمة المواد المركبة الفعلية");
                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//month
                    listview.Columns[1].Width = 120;//عدد عمليات الصيانة
                    listview.Columns[2].Width = 115;//ما تم انهائه
                    listview.Columns[3].Width = 125;//ما تم اصلاحه
                    listview.Columns[4].Width = 125;//عدد المكفول
                    listview.Columns[5].Width = 120;//المنتهي كفالته
                    listview.Columns[6].Width = 140;//قيمة فواتير الصيانة
                    listview.Columns[7].Width = 115;//المدفوع

                    listview.Columns[8].Width = 125;//المتبقي
                    listview.Columns[9].Width = 125;//قيمة العناصر المركبة
                    listview.Columns[10].Width = 125;//قيمة المركب الفعلي
                    listview.Columns[11].Width = 125;//قيمة الفواتير الفعلية
                    listview.Columns[12].Width = 125;//قيمة المدفوع الفعلي


                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_MaintenanceOPRs_YearRange_ReportDetail
        {

            public int YearNO;
            public int MaintenanceOPRs_Count;
            public int MaintenanceOPRs_EndWork_Count;
            public int MaintenanceOPRs_Repaired_Count;
            public int MaintenanceOPRs_Warranty_Count;
            public int MaintenanceOPRs_EndWarranty_Count;

            public int BillMaintenances_Count;
            public string BillMaintenances_Value;
            public string BillMaintenances_Pays_Value;
            public string BillMaintenances_Pays_Remain;
            public double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency;

            public string BillMaintenances_ItemsOut_Value;
            public double BillMaintenances_ItemsOut_RealValue;
            public double BillMaintenances_RealValue;
            public double BillMaintenances_Pays_RealValue;
            public Report_MaintenanceOPRs_YearRange_ReportDetail(
                    int YearNO_,
                 int MaintenanceOPRs_Count_,
             int MaintenanceOPRs_EndWork_Count_,
             int MaintenanceOPRs_Repaired_Count_,
             int MaintenanceOPRs_Warranty_Count_,
             int MaintenanceOPRs_EndWarranty_Count_,
              int BillMaintenances_Count_,
            string BillMaintenances_Value_,
             string BillMaintenances_Pays_Value_,
             string BillMaintenances_Pays_Remain_,
             double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_,

             string BillMaintenances_ItemsOut_Value_,
             double BillMaintenances_ItemsOut_RealValue_,
             double BillMaintenances_RealValue_,
             double BillMaintenances_Pays_RealValue_)
            {
                YearNO = YearNO_;

                MaintenanceOPRs_Count = MaintenanceOPRs_Count_;
                MaintenanceOPRs_EndWork_Count = MaintenanceOPRs_EndWork_Count_;
                MaintenanceOPRs_Repaired_Count = MaintenanceOPRs_Repaired_Count_;
                MaintenanceOPRs_Warranty_Count = MaintenanceOPRs_Warranty_Count_;
                MaintenanceOPRs_EndWarranty_Count = MaintenanceOPRs_EndWarranty_Count_;

                BillMaintenances_Count = BillMaintenances_Count_;
                BillMaintenances_Value = BillMaintenances_Value_;
                BillMaintenances_Pays_Value = BillMaintenances_Pays_Value_;
                BillMaintenances_Pays_Remain = BillMaintenances_Pays_Remain_;
                BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency = BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_;

                BillMaintenances_ItemsOut_Value = BillMaintenances_ItemsOut_Value_;
                BillMaintenances_ItemsOut_RealValue = BillMaintenances_ItemsOut_RealValue_;
                BillMaintenances_RealValue = BillMaintenances_RealValue_;
                BillMaintenances_Pays_RealValue = BillMaintenances_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewMaintenanceOPRs_YearRange";
                    listview.Columns.Clear();
                    listview.Columns.Add("السنة");
                    listview.Columns.Add("عدد عمليات الصيانة");
                    listview.Columns.Add("ما تم انهاءه");
                    listview.Columns.Add("ماتم اصلاحه");
                    listview.Columns.Add("عدد المكفول");
                    listview.Columns.Add("المنتهي كفالته");
                    listview.Columns.Add(" قمة فواتير الصيانة");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("قيمة المواد المركبة");
                    listview.Columns.Add("قيمة المواد المركبة الفعلية");
                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearRangeReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//year
                    listview.Columns[1].Width = 120;//عدد عمليات الصيانة
                    listview.Columns[2].Width = 115;//ما تم انهائه
                    listview.Columns[3].Width = 125;//ما تم اصلاحه
                    listview.Columns[4].Width = 125;//عدد المكفول
                    listview.Columns[5].Width = 120;//المنتهي كفالته
                    listview.Columns[6].Width = 140;//قيمة فواتير الصيانة
                    listview.Columns[7].Width = 115;//المدفوع

                    listview.Columns[8].Width = 125;//المتبقي
                    listview.Columns[9].Width = 125;//قيمة العناصر المركبة
                    listview.Columns[10].Width = 125;//قيمة المركب الفعلي
                    listview.Columns[11].Width = 125;//قيمة الفواتير الفعلية
                    listview.Columns[12].Width = 125;//قيمة المدفوع الفعلي

                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearRangeReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        #endregion
        #region Report_Sell
        public class Report_Sells_Day_ReportDetail
        {

            public DateTime Bill_Time;
            public int Bill_ID;
            public string SellType;
            public string Bill_Owner;
            public  int ClauseS_Count;
            public double ItemsOutValue;
            public Currency _Currency;
            public double ExchangeRate;
            public int PaysCount;
            public string  PaysAmount;
            public double PaysRemain;
            public  string Source_ItemsIN_Cost_Details;
            public double Source_ItemsIN_RealCost;
            public double ItemsOut_RealValue;
            public double RealPaysValue;
            public Report_Sells_Day_ReportDetail(  DateTime Bill_Time_,
             int Bill_ID_,
             string SellType_,
             string Bill_Owner_,
             int ClauseS_Count_,
             double ItemsOutValue_,
             Currency Currency_,
             double ExchangeRate_,
             int PaysCount_,
             string PaysAmount_,
             double PaysRemain_,
             string Source_ItemsIN_Cost_Details_,
             double Source_ItemsIN_RealCost_,
             double ItemsOut_RealValue_,
             double RealPaysValue_
               )
            {
                Bill_Time= Bill_Time_;
             Bill_ID= Bill_ID_;
             SellType= SellType_;
              Bill_Owner= Bill_Owner_;
              ClauseS_Count= ClauseS_Count_;
              ItemsOutValue= ItemsOutValue_;
              _Currency= Currency_;
              ExchangeRate= ExchangeRate_;
              PaysCount= PaysCount_;
              PaysAmount= PaysAmount_;
              PaysRemain= PaysRemain_;
              Source_ItemsIN_Cost_Details= Source_ItemsIN_Cost_Details_;
              Source_ItemsIN_RealCost= Source_ItemsIN_RealCost_;
              ItemsOut_RealValue= ItemsOut_RealValue_;
              RealPaysValue= RealPaysValue_;
        }
            public static void IntiliazeListView(ref ListView listview)
            {
                try
                {
                    listview.Name = "ListViewSellsDay";
                    listview.Columns.Clear();
                    listview.Columns.Add("الوقت");
                    listview.Columns.Add("الرقم");
                    listview.Columns.Add("نمط البيع");
                    listview.Columns.Add("باسم");
                    listview.Columns.Add("البنود");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("سعر الصرف");
                    //listview.Columns.Add("عدد الدفعات");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("الباقي");
                    listview.Columns.Add("تكلفة المواد");
                    listview.Columns.Add("التكلفة الفعلية");
                    listview.Columns.Add("المباع الفعلي");
                    listview.Columns.Add("الربح الفعلي");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("OperationReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 75;//time
                    listview.Columns[1].Width = 60;//id
                    listview.Columns[2].Width = 75;//selltype
                    listview.Columns[3].Width = 100;//owner
                    listview.Columns[4].Width = 65;//clause count
                    listview.Columns[5].Width = 90;//value
                    listview.Columns[6].Width = 100;//exchangerate
                    listview.Columns[7].Width = 130;//paid
                    listview.Columns[8].Width = 90;//remain
                    listview.Columns[9].Width = 100;//item in cost
                    listview.Columns[10].Width = 130;//real item in cost
                    listview.Columns[11].Width = 130;//real items out cost
                    listview.Columns[12].Width = 130;//profit value
                    listview.Columns[13].Width = 130;//real pays value

                }
                catch (Exception ee)
                {
                    MessageBox.Show("OperationReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        
        }
        public class Report_Sells_Month_ReportDetail
        {

            public int DayID;
            public DateTime DayDate;
            public int Bills_Count;
            public int Bills_Clause_Count;
            public string Bills_Value;
            public string  Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double  Bills_Pays_Remain_UPON_BillsCurrency;
            public string Bills_ItemsIN_Value;
            public double  Bills_ItemsIN_RealValue;
            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public Report_Sells_Month_ReportDetail(
                    int DayID_,
                    DateTime DayDate_,
              int Bills_Count_,
             int Bills_Clause_Count_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,
             double Bills_Pays_Remain_UPON_BillsCurrency_,
             string Bills_ItemsIN_Value_,
             double Bills_ItemsIN_RealValue_,
             double Bills_RealValue_,
             double Bills_Pays_RealValue_)
            {
                DayID = DayID_;
                DayDate = DayDate_;
                  Bills_Count = Bills_Count_;
              Bills_Clause_Count= Bills_Clause_Count_;
              Bills_Value= Bills_Value_;
              Bills_Pays_Value= Bills_Pays_Value_;
              Bills_Pays_Remain= Bills_Pays_Remain_;
                Bills_Pays_Remain_UPON_BillsCurrency = Bills_Pays_Remain_UPON_BillsCurrency_;
              Bills_ItemsIN_Value = Bills_ItemsIN_Value_;
              Bills_ItemsIN_RealValue= Bills_ItemsIN_RealValue_;
              Bills_RealValue= Bills_RealValue_;
              Bills_Pays_RealValue= Bills_Pays_RealValue_;
        }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewSells_Month";
                    listview.Columns.Clear();
                    listview.Columns.Add("اليوم");
                    listview.Columns.Add("اجمالي الفواتير");
                    listview.Columns.Add("اجمالي البنود");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add("اجمالي المدفوع");
                    listview.Columns.Add("المتبقي");
                    listview.Columns.Add("كلفة المواد المباعة");
                    listview.Columns.Add("الكلفة الفعلية");
                    listview.Columns.Add("قيمة الفواتير الفعلية");
                    listview.Columns.Add("الربح الفعلي");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//daydate
                    listview.Columns[1].Width = 120;//bills count
                    listview.Columns[2].Width = 115;//clause count
                    listview.Columns[3].Width = 125;//bill value
                    listview.Columns[4].Width = 125;//bills pays value
                    listview.Columns[5].Width = 120;//remain
                    listview.Columns[6].Width = 140;//item in value
                    listview.Columns[7].Width = 115;//item in real value
                    listview.Columns[8].Width = 145;//real value
                    listview.Columns[9].Width = 115;//profit
                    listview.Columns[10].Width = 125;//real p


                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillMonthReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_Sells_Year_ReportDetail
        {

            public int MonthNO;
            public string MonthName;
            public int Bills_Count;
            public int Bills_Clause_Count;
            public string Bills_Value;
            public string Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double Bills_Pays_Remain_UPON_BillsCurrency;
            public string Bills_ItemsIN_Value;
            public double Bills_ItemsIN_RealValue;
            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public Report_Sells_Year_ReportDetail(
                    int MOnthNO_,
                    string MonthName_,
              int Bills_Count_,
             int Bills_Clause_Count_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,
             double Bills_Pays_Remain_UPON_BillsCurrency_,
             string Bills_ItemsIN_Value_,
             double Bills_ItemsIN_RealValue_,
             double Bills_RealValue_,
             double Bills_Pays_RealValue_)
            {
                MonthNO = MOnthNO_;
                MonthName = MonthName_;
                Bills_Count = Bills_Count_;
                Bills_Clause_Count = Bills_Clause_Count_;
                Bills_Value = Bills_Value_;
                Bills_Pays_Value = Bills_Pays_Value_;
                Bills_Pays_Remain = Bills_Pays_Remain_;
                Bills_Pays_Remain_UPON_BillsCurrency = Bills_Pays_Remain_UPON_BillsCurrency_;
                Bills_ItemsIN_Value = Bills_ItemsIN_Value_;
                Bills_ItemsIN_RealValue = Bills_ItemsIN_RealValue_;
                Bills_RealValue = Bills_RealValue_;
                Bills_Pays_RealValue = Bills_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewSells_Year";
                    listview.Columns.Clear();
                    //listview.Columns.Add("رقم الشهر");
                    listview.Columns.Add("الشهر");
                    listview.Columns.Add("اجمالي الفواتير");
                    listview.Columns.Add("اجمالي البنود");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add("اجمالي المدفوع");
                    listview.Columns.Add("المتبقي");
                    listview.Columns.Add("كلفة المواد المباعة");
                    listview.Columns.Add("الكلفة الفعلية");
                    listview.Columns.Add("قيمة الفواتير الفعلية");
                    listview.Columns.Add("الربح الفعلي");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//month
                    listview.Columns[1].Width = 120;//bills count
                    listview.Columns[2].Width = 115;//clause count
                    listview.Columns[3].Width = 125;//bill value
                    listview.Columns[4].Width = 125;//bills pays value
                    listview.Columns[5].Width = 120;//remain
                    listview.Columns[6].Width = 140;//item in value
                    listview.Columns[7].Width = 115;//item in real value
                    listview.Columns[8].Width = 145;//real value
                    listview.Columns[9].Width = 115;//profit
                    listview.Columns[10].Width = 125;//real p


                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_Sells_YearRange_ReportDetail
        {

            public int YearNO;
            public int Bills_Count;
            public int Bills_Clause_Count;
            public string Bills_Value;
            public string Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double Bills_Pays_Remain_UPON_BillsCurrency;
            public string Bills_ItemsIN_Value;
            public double Bills_ItemsIN_RealValue;
            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public Report_Sells_YearRange_ReportDetail(
                    int YearNO_,
              int Bills_Count_,
             int Bills_Clause_Count_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,
             double Bills_Pays_Remain_UPON_BillsCurrency_,
             string Bills_ItemsIN_Value_,
             double Bills_ItemsIN_RealValue_,
             double Bills_RealValue_,
             double Bills_Pays_RealValue_)
            {
                YearNO = YearNO_;
 
                Bills_Count = Bills_Count_;
                Bills_Clause_Count = Bills_Clause_Count_;
                Bills_Value = Bills_Value_;
                Bills_Pays_Value = Bills_Pays_Value_;
                Bills_Pays_Remain = Bills_Pays_Remain_;
                Bills_Pays_Remain_UPON_BillsCurrency = Bills_Pays_Remain_UPON_BillsCurrency_;
                Bills_ItemsIN_Value = Bills_ItemsIN_Value_;
                Bills_ItemsIN_RealValue = Bills_ItemsIN_RealValue_;
                Bills_RealValue = Bills_RealValue_;
                Bills_Pays_RealValue = Bills_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewSells_YearRange";
                    listview.Columns.Clear();
                    listview.Columns.Add("السنة");
                    listview.Columns.Add("اجمالي الفواتير");
                    listview.Columns.Add("اجمالي البنود");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add("اجمالي المدفوع");
                    listview.Columns.Add("المتبقي");
                    listview.Columns.Add("كلفة المواد المباعة");
                    listview.Columns.Add("الكلفة الفعلية");
                    listview.Columns.Add("قيمة الفواتير الفعلية");
                    listview.Columns.Add("الربح الفعلي");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearRangeReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//year
                    listview.Columns[1].Width = 120;//bills count
                    listview.Columns[2].Width = 115;//clause count
                    listview.Columns[3].Width = 125;//bill value
                    listview.Columns[4].Width = 125;//bills pays value
                    listview.Columns[5].Width = 120;//remain
                    listview.Columns[6].Width = 140;//item in value
                    listview.Columns[7].Width = 115;//item in real value
                    listview.Columns[8].Width = 145;//real value
                    listview.Columns[9].Width = 115;//profit
                    listview.Columns[10].Width = 125;//real p


                }
                catch (Exception ee)
                {
                    MessageBox.Show("BillYearRangeReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class OperationCurrencyReport
        {
            public Currency _Currency;
            public int OperationsCount;
            public double OperationsValue;
            public string  Operations_PaysAmount;
            public double  Operations_RemainValue;

            public OperationCurrencyReport(
             Currency Currency_,
             int OperationsCount_,
             double OperationsValue_,
             string Operations_PaysAmount_,
             double Operations_RemainValue_)
            {
                _Currency= Currency_;
             OperationsCount= OperationsCount_;
             OperationsValue= OperationsValue_;
            Operations_PaysAmount= Operations_PaysAmount_;
            Operations_RemainValue= Operations_RemainValue_;
            }
        }
        #endregion
        #region Report_PayOrder
        public class Report_PayOrders_Day_ReportDetail
        {

            public const bool TYPE_SALARY_PAY_ODER = false;
            public const bool TYPE_PAY_ODER = true;

            public DateTime PayOrder_Time;
            public bool PayOrderType;
            public uint PayOrderID;
            
            public string PayOrderDesc;
            public uint EmployeeID;
            public string EmployeeName;
            public Currency _Currency;
            public double ExchangeRate;
            public double Value;
            public string PaysAmount;
            public double PaysRemain;
            public double RealValue;
            public double RealPays;
            public Report_PayOrders_Day_ReportDetail(
                 DateTime PayOrder_Time_,
                bool PayOrderType_,
                uint PayOrderID_,
            
             string PayOrderDesc_,
             uint EmployeeID_,
             string EmployeeName_,
                          double Value_,
             Currency Currency_,
            double ExchangeRate_,

             string PaysAmount_,
             double PaysRemain_,
             double RealValue_,
             double RealPays_)
            {
                PayOrder_Time = PayOrder_Time_;
                PayOrderType = PayOrderType_;
                PayOrderID = PayOrderID_;
                PayOrderDesc = PayOrderDesc_;
                EmployeeID = EmployeeID_;
                EmployeeName = EmployeeName_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                Value = Value_;
                PaysAmount = PaysAmount_;
                PaysRemain = PaysRemain_;
                RealValue = RealValue_;
                RealPays = RealPays_;
            }

            public static void IntiliazeListView(ref ListView listview)
            {
                try
                {
                    listview.Name = "ListViewEmployeesDay";
                    listview.Columns.Clear();
                    listview.Columns.Add("الوقت");
                    listview.Columns.Add("طبيعة الأمر");
                    listview.Columns.Add("الرقم");
                    listview.Columns.Add("الموظف");
                    listview.Columns.Add("الوصف");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("سعر الصرف");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("الباقي");
                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");

                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("OperationReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 75;//time
                    listview.Columns[1].Width = 125;//type
                    listview.Columns[2].Width = 100;//id
                    listview.Columns[3].Width = 200;//owner
                    listview.Columns[4].Width = 200;//desc
                    listview.Columns[5].Width = 125;//value
                    listview.Columns[6].Width = 100;//exchangerate
                    listview.Columns[7].Width = 125;//paid
                    listview.Columns[8].Width = 125;//remain
                    listview.Columns[9].Width = 100;//real value
                    listview.Columns[10].Width = 100;//real paid


                }
                catch (Exception ee)
                {
                    MessageBox.Show("OperationReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_PayOrders_Month_ReportDetail
        {

            public int DayID;
            public DateTime DayDate;
            public int Salary_PayOrders_Count;
            public int Other_PayOrders_Count;
            public string PayOrders_Value;
            public string PayOrders_Pays_Value;
            public string PayOrders_Pays_Remain;

            public double PayOrders_Pays_Remain_UPON_PayOrdersCurrency;
            public double PayOrders_RealValue;
            public double PayOrders_Pays_RealValue;


            public Report_PayOrders_Month_ReportDetail(
                    int DayID_,
                    DateTime DayDate_,
                int Salary_PayOrders_Count_,
             int Other_PayOrders_Count_,
             string PayOrders_Value_,
             string PayOrders_Pays_Value_,
             string PayOrders_Pays_Remain_,

             double PayOrders_Pays_Remain_UPON_PayOrdersCurrency_,
             double PayOrders_RealValue_,
             double PayOrders_Pays_RealValue_)
            {
                DayID = DayID_;
                DayDate = DayDate_;
                Salary_PayOrders_Count = Salary_PayOrders_Count_;
                Other_PayOrders_Count = Other_PayOrders_Count_;
                PayOrders_Value = PayOrders_Value_;
                PayOrders_Pays_Value = PayOrders_Pays_Value_;
                PayOrders_Pays_Remain = PayOrders_Pays_Remain_;

                PayOrders_Pays_Remain_UPON_PayOrdersCurrency = PayOrders_Pays_Remain_UPON_PayOrdersCurrency_;
                PayOrders_RealValue = PayOrders_RealValue_;
                PayOrders_Pays_RealValue = PayOrders_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewPayOrders_Month";
                    listview.Columns.Clear();
                    listview.Columns.Add("اليوم");
                    listview.Columns.Add("عدد اوامر صرف الرواتب");
                    listview.Columns.Add("عدد اوامر الصرف الاخرى");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("PayOrderMonthReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//daydate
                    listview.Columns[1].Width = 175;//salary count
                    listview.Columns[2].Width = 175;//other count

                    listview.Columns[3].Width = 140;// value
                    listview.Columns[4].Width = 140;//paid
                    listview.Columns[5].Width = 140;//remain

                    listview.Columns[6].Width = 125;//real value
                    listview.Columns[7].Width = 125;//real pays value

                }
                catch (Exception ee)
                {
                    MessageBox.Show("PayOrderMonthReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_PayOrders_Year_ReportDetail
        {

            public int MonthNO;
            public string MonthName;
            public int Salary_PayOrders_Count;
            public int Other_PayOrders_Count;
            public string PayOrders_Value;
            public string PayOrders_Pays_Value;
            public string PayOrders_Pays_Remain;

            public double PayOrders_Pays_Remain_UPON_PayOrdersCurrency;
            public double PayOrders_RealValue;
            public double PayOrders_Pays_RealValue;

            public Report_PayOrders_Year_ReportDetail(
                    int MOnthNO_,
                    string MonthName_,
              int Salary_PayOrders_Count_,
             int Other_PayOrders_Count_,
             string PayOrders_Value_,
             string PayOrders_Pays_Value_,
             string PayOrders_Pays_Remain_,

             double PayOrders_Pays_Remain_UPON_PayOrdersCurrency_,
             double PayOrders_RealValue_,
             double PayOrders_Pays_RealValue_)
            {
                MonthNO = MOnthNO_;
                MonthName = MonthName_;
                Salary_PayOrders_Count = Salary_PayOrders_Count_;
                Other_PayOrders_Count = Other_PayOrders_Count_;
                PayOrders_Value = PayOrders_Value_;
                PayOrders_Pays_Value = PayOrders_Pays_Value_;
                PayOrders_Pays_Remain = PayOrders_Pays_Remain_;

                PayOrders_Pays_Remain_UPON_PayOrdersCurrency = PayOrders_Pays_Remain_UPON_PayOrdersCurrency_;
                PayOrders_RealValue = PayOrders_RealValue_;
                PayOrders_Pays_RealValue = PayOrders_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewPayOrders_Month";
                    listview.Columns.Clear();
                    listview.Columns.Add("اليوم");
                    listview.Columns.Add("عدد اوامر صرف الرواتب");
                    listview.Columns.Add("عدد اوامر الصرف الاخرى");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("PayOrderMonthReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//daydate
                    listview.Columns[1].Width = 175;//salary count
                    listview.Columns[2].Width = 175;//other count

                    listview.Columns[3].Width = 120;// value
                    listview.Columns[4].Width = 140;//paid
                    listview.Columns[5].Width = 115;//remain

                    listview.Columns[6].Width = 125;//real value
                    listview.Columns[7].Width = 125;//real pays value

                }
                catch (Exception ee)
                {
                    MessageBox.Show("PayOrderMonthReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Report_PayOrders_YearRange_ReportDetail
        {

            public int YearNO;
            public int Salary_PayOrders_Count;
            public int Other_PayOrders_Count;
            public string PayOrders_Value;
            public string PayOrders_Pays_Value;
            public string PayOrders_Pays_Remain;

            public double PayOrders_Pays_Remain_UPON_PayOrdersCurrency;
            public double PayOrders_RealValue;
            public double PayOrders_Pays_RealValue;
            public Report_PayOrders_YearRange_ReportDetail(
                    int YearNO_,
            int Salary_PayOrders_Count_,
             int Other_PayOrders_Count_,
             string PayOrders_Value_,
             string PayOrders_Pays_Value_,
             string PayOrders_Pays_Remain_,

             double PayOrders_Pays_Remain_UPON_PayOrdersCurrency_,
             double PayOrders_RealValue_,
             double PayOrders_Pays_RealValue_)
            {
                YearNO = YearNO_;

                Salary_PayOrders_Count = Salary_PayOrders_Count_;
                Other_PayOrders_Count = Other_PayOrders_Count_;
                PayOrders_Value = PayOrders_Value_;
                PayOrders_Pays_Value = PayOrders_Pays_Value_;
                PayOrders_Pays_Remain = PayOrders_Pays_Remain_;

                PayOrders_Pays_Remain_UPON_PayOrdersCurrency = PayOrders_Pays_Remain_UPON_PayOrdersCurrency_;
                PayOrders_RealValue = PayOrders_RealValue_;
                PayOrders_Pays_RealValue = PayOrders_Pays_RealValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {

                try
                {
                    listview.Name = "ListViewPayOrders_Month";
                    listview.Columns.Clear();
                    listview.Columns.Add("اليوم");
                    listview.Columns.Add("عدد اوامر صرف الرواتب");
                    listview.Columns.Add("عدد اوامر الصرف الاخرى");
                    listview.Columns.Add("القيمة الكلية");
                    listview.Columns.Add(" المدفوع");
                    listview.Columns.Add("المتبقي");

                    listview.Columns.Add("القيمة الفعلية");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("PayOrderMonthReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 100;//daydate
                    listview.Columns[1].Width = 175;//salary count
                    listview.Columns[2].Width = 175;//other count

                    listview.Columns[3].Width = 120;// value
                    listview.Columns[4].Width = 140;//paid
                    listview.Columns[5].Width = 115;//remain

                    listview.Columns[6].Width = 125;//real value
                    listview.Columns[7].Width = 125;//real pays value

                }
                catch (Exception ee)
                {
                    MessageBox.Show("PayOrderMonthReportDetail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        #endregion

        #region AccountMoney
        public class PayCurrencyReport
        {

            public Currency _Currency;
            public double PaysIN_Sell;
            public double PaysIN_Maintenance;
            public double PaysIN_NON;
            public double PaysIN_Exchange;
            public double PaysOUT_Buy;
            public double PaysOUT_Emp;
            public double PaysOUT_NON;
            public double PaysOUT_Exchange;

            public PayCurrencyReport(
                  Currency Currency_,
              double PaysIN_Sell_,
             double PaysIN_Maintenance_,
             double PaysIN_NON_,
             double PaysIN_Exchange_,
             double PaysOUT_Buy_,
             double PaysOUT_Emp_,
             double PaysOUT_NON_,
             double PaysOUT_Exchange_)
            {
                _Currency = Currency_;
                PaysIN_Sell= PaysIN_Sell_;
             PaysIN_Maintenance= PaysIN_Maintenance_;
            PaysIN_NON= PaysIN_NON_;
             PaysIN_Exchange= PaysIN_Exchange_;
             PaysOUT_Buy= PaysOUT_Buy_;
            PaysOUT_Emp= PaysOUT_Emp_;
             PaysOUT_NON= PaysOUT_NON_;
            PaysOUT_Exchange= PaysOUT_Exchange_;
          }
        }
      
        public class AccountOprReportDetail
        {
            public const bool DIRECTION_IN = false;
            public const bool DIRECTION_OUT = true ;

            public const bool TYPE_PAY_OPR = false ;
            public const bool TYPE_EXCHANGE_OPR = true ;

            public DateTime OprTime;
            public bool  OprType;
            public bool OprDirection;
            public int OprID;
            public string OprOwner;

            public double  Value;
            public string Currency;
            public double ExchangeRate;
            public double RealValue;
            public AccountOprReportDetail(
             DateTime OprTime_,
             bool   OprType_,
             bool  OprDirection_,
             int OprID_,
             string OprOwner_,
             double Value_,
             string Currency_,
             double ExchangeRate_,
             double RealValue_
             )
            {

                OprTime = OprTime_;
                OprType = OprType_;
                OprID= OprID_;
                OprOwner= OprOwner_;
                OprDirection= OprDirection_;
                 Value = Value_;
                Currency= Currency_;
                ExchangeRate = ExchangeRate_;
                RealValue = RealValue_;
        }
            public static void IntiliazeListView(ref ListView listview)
            {
                listview.Name = "ListViewMoneyDataDetails_Day";
   
                listview.Columns.Clear();
                listview.Columns.Add("الوقت");
                listview.Columns.Add("التصنيف");
                listview.Columns.Add("الاتجاه");
                listview.Columns.Add("المعرف");

                listview.Columns.Add("القيمة");
                listview.Columns.Add("سعر الصرف");
                listview.Columns.Add("القيمة الفعلية");
                listview.Columns.Add("عائدة لـ");
                for (int i = 0; i < listview.Columns.Count; i++)
                    listview.Columns[i].TextAlign = HorizontalAlignment.Center;
                IntializeListViewColumnsWidth(ref listview);
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {
                //MessageBox.Show("f");
                listview.Columns[0].Width = 100;//
                listview.Columns[1].Width = 100;
                listview.Columns[2].Width = 150;
                listview.Columns[3].Width = 100;
                listview.Columns[4].Width = 200;

                listview.Columns[5].Width = 150;
                listview.Columns[6].Width = 200;

                listview.Columns[7].Width = listview.Width - 905;
            }


        }
        public class AccountOprDayReportDetail
        {
            public int DateDayNo;
            public DateTime Date_day;
            public int PaysIN_Count;
            public int PaysOUT_Count;
            public int Exchange_Count;
            public string PaysIN_Value;
            public double PaysIN_Real_Value;
            public string PaysOUT_Value;
            public double PaysOUT_Real_Value;
            public AccountOprDayReportDetail(
                           int DateDayNo_,
             DateTime Date_day_,
             int PaysIN_Count_,
             int PaysOUT_Count_,
             int Exchange_Count_,
             string PaysIN_Value_,
             double PaysIN_Real_Value_,
             string PaysOUT_Value_,
             double PaysOUT_Real_Value_)
            {
                DateDayNo= DateDayNo_;
                Date_day= Date_day_;
                PaysIN_Count= PaysIN_Count_;
                PaysOUT_Count= PaysOUT_Count_;
                Exchange_Count= Exchange_Count_;
                PaysIN_Value= PaysIN_Value_;
                PaysIN_Real_Value = PaysIN_Real_Value_;
                PaysOUT_Value = PaysOUT_Value_;
                PaysOUT_Real_Value= PaysOUT_Real_Value_;
        }
            public static void IntiliazeListView(ref ListView listview)
            {
                listview.Name = "ListViewMoneyDataDetails_Month";
                listview.Columns.Clear();
                listview.Columns.Add("اليوم");
                listview.Columns.Add("عدد الدفعات الداخلة");
                listview.Columns.Add("عدد الدفعات الخارجة");
                listview.Columns.Add("عدد عمليات الصرف");
                listview.Columns.Add(" الداخل الى الصندوق");
                listview.Columns.Add(" الخارج من الصندوق");
                listview.Columns.Add(" قيمة الداخل الفعلية");
                listview.Columns.Add(" قيمة الخارج الفعلية");
                listview.Columns.Add(" الصافي الفعلي");
                for(int i=0;i<listview .Columns.Count;i++)
                    listview.Columns [i].TextAlign= HorizontalAlignment.Center ;
 

                IntializeListViewColumnsWidth(ref listview);
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {
                listview.Columns[0].Width = 100;
                listview.Columns[1].Width = 150;
                listview.Columns[2].Width = 150;
                listview.Columns[3].Width = 150;
                listview.Columns[4].Width = (listview.Width - 1005) / 2;
                listview.Columns[5].Width = (listview.Width - 1005) / 2;
                listview.Columns[6].Width = 150;
                listview.Columns[7].Width = 150;
                listview.Columns[8].Width = 150;
            }



        }
        public class AccountOprMonthReportDetail
        {
            public int Year_Month;
            public string Year_Month_Name;
            public int PaysIN_Count;
            public int PaysOUT_Count;
            public int Exchange_Count;
            public string PaysIN_Value;
            public double PaysIN_Real_Value;
            public string PaysOUT_Value;
            public double PaysOUT_Real_Value;
            public AccountOprMonthReportDetail(
                int Year_Month_,
             string Year_Month_Name_,
            int PaysIN_Count_,
             int PaysOUT_Count_,
             int Exchange_Count_,
             string PaysIN_Value_,
             double PaysIN_Real_Value_,
             string PaysOUT_Value_,
             double PaysOUT_Real_Value_)
            {
                Year_Month  = Year_Month_;
                Year_Month_Name = Year_Month_Name_;
                PaysIN_Count = PaysIN_Count_;
                PaysOUT_Count = PaysOUT_Count_;
                Exchange_Count = Exchange_Count_;
                PaysIN_Value = PaysIN_Value_;
                PaysIN_Real_Value = PaysIN_Real_Value_;
                PaysOUT_Value = PaysOUT_Value_;
                PaysOUT_Real_Value = PaysOUT_Real_Value_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {
                listview.Name = "ListViewMoneyDataDetails_Year";
                listview.Columns.Clear();
                listview.Columns.Add("الشهر");
                listview.Columns.Add("عدد الدفعات الداخلة");
                listview.Columns.Add("عدد الدفعات الخارجة");
                listview.Columns.Add("عدد عمليات الصرف");
                listview.Columns.Add(" الداخل الى الصندوق");
                listview.Columns.Add(" الخارج من الصندوق");
                listview.Columns.Add(" قيمة الداخل الفعلية");
                listview.Columns.Add(" قيمة الخارج الفعلية");
                listview.Columns.Add(" الصافي الفعلي");
                for (int i = 0; i < listview.Columns.Count; i++)
                    listview.Columns[i].TextAlign = HorizontalAlignment.Center;
                IntializeListViewColumnsWidth(ref listview);
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {
                listview.Columns[0].Width = 100;
                listview.Columns[1].Width = 150;
                listview.Columns[2].Width = 150;
                listview.Columns[3].Width = 150;
                listview.Columns[4].Width = (listview.Width - 1005) / 2;
                listview.Columns[5].Width = (listview.Width - 1005) / 2;
                listview.Columns[6].Width = 150;
                listview.Columns[7].Width = 150;
                listview.Columns[8].Width = 150;
            }



        }
        public class AccountOprYearReportDetail
        {

            public int AccountYear ;
            public int PaysIN_Count;
            public int PaysOUT_Count;
            public int Exchange_Count;
            public string PaysIN_Value;
            public double PaysIN_Real_Value;
            public string PaysOUT_Value;
            public double PaysOUT_Real_Value;
            public AccountOprYearReportDetail(
                int AccountYear_,
             int PaysIN_Count_,
             int PaysOUT_Count_,
             int Exchange_Count_,
             string PaysIN_Value_,
             double PaysIN_Real_Value_,
             string PaysOUT_Value_,
             double PaysOUT_Real_Value_)
            {
                AccountYear = AccountYear_;
                PaysIN_Count = PaysIN_Count_;
                PaysOUT_Count = PaysOUT_Count_;
                Exchange_Count = Exchange_Count_;
                PaysIN_Value = PaysIN_Value_;
                PaysIN_Real_Value = PaysIN_Real_Value_;
                PaysOUT_Value = PaysOUT_Value_;
                PaysOUT_Real_Value = PaysOUT_Real_Value_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {
                listview.Name = "ListViewMoneyDataDetails_Range";
                listview.Columns.Clear();
                listview.Columns.Add("السنة");
                listview.Columns.Add("عدد الدفعات الداخلة");
                listview.Columns.Add("عدد الدفعات الخارجة");
                listview.Columns.Add("عدد عمليات الصرف");
                listview.Columns.Add(" الداخل الى الصندوق");
                listview.Columns.Add(" الخارج من الصندوق");
                listview.Columns.Add(" قيمة الداخل الفعلية");
                listview.Columns.Add(" قيمة الخارج الفعلية");
                listview.Columns.Add(" الصافي الفعلي");
                for (int i = 0; i < listview.Columns.Count; i++)
                    listview.Columns[i].TextAlign = HorizontalAlignment.Center;
                IntializeListViewColumnsWidth(ref listview);
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {
                listview.Columns[0].Width = 100;
                listview.Columns[1].Width = 150;
                listview.Columns[2].Width = 150;
                listview.Columns[3].Width = 150;
                listview.Columns[4].Width = (listview.Width - 1005) / 2;
                listview.Columns[5].Width = (listview.Width - 1005) / 2;
                listview.Columns[6].Width = 150;
                listview.Columns[7].Width = 150;
                listview.Columns[8].Width = 150;
            }



        }
        #endregion
        #region ContactReport
        public class Contact_Pays_ReportDetail
        {
            public const bool DIRECTION_IN = false;
            public const bool DIRECTION_OUT = true;

            public uint OperationType;
            public uint OperationID;
            public bool PayDirection;
            public DateTime PayDate;
            public int PayID;
            public double Value;
            public string Currency;
            public double ExchangeRate;
            public double RealValue;

            public Contact_Pays_ReportDetail(
             uint OperationType_,
             uint OperationID_,
             bool PayDirection_,
             DateTime PayDate_,
             int PayID_,
             double Value_,
             string Currency_,
             double ExchangeRate_,
             double RealValue_
             )
            {
                OperationType= OperationType_;
                OperationID= OperationID_;
                PayDirection= PayDirection_;
                PayDate= PayDate_;
                PayID= PayID_;
                Value= Value_;
                Currency= Currency_;
                ExchangeRate= ExchangeRate_;
                RealValue= RealValue_;
        }
            public static void IntiliazeListView(ref ListView listview)
            {

                listview.Columns.Clear();
                listview.Columns.Add("التاريخ");
                listview.Columns.Add("الاتجاه");
                listview.Columns.Add("المعرف");

                listview.Columns.Add("القيمة");
                listview.Columns.Add("سعر الصرف");
                listview.Columns.Add("القيمة الفعلية");
                listview.Columns.Add("عائدة لـ");
                for (int i = 0; i < listview.Columns.Count; i++)
                    listview.Columns[i].TextAlign = HorizontalAlignment.Center;
                IntializeListViewColumnsWidth(ref listview);
            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {
                //MessageBox.Show("f");
                listview.Columns[0].Width = 100;//
                listview.Columns[1].Width = 100;
                listview.Columns[2].Width = 150;
                listview.Columns[3].Width = 100;
                listview.Columns[4].Width = 200;

                listview.Columns[5].Width = 150;
                listview.Columns[6].Width = 200;
            }


        }
        public class Contact_PayCurrencyReport
        {

            public Currency _Currency;
            public double PaysIN_Sell;
            public double PaysIN_Maintenance;

            public double PaysOUT_Buy;


            public Contact_PayCurrencyReport(
                  Currency Currency_,
              double PaysIN_Sell_,
             double PaysIN_Maintenance_,

             double PaysOUT_Buy_)
            {
                _Currency = Currency_;
                PaysIN_Sell = PaysIN_Sell_;
                PaysIN_Maintenance = PaysIN_Maintenance_;
                PaysOUT_Buy = PaysOUT_Buy_;
            }
        }

        public class Contact_Buys_ReportDetail
        {

            public DateTime Bill_Date;
            public int Bill_ID;
            public int ClauseS_Count;
            public double Amount_IN;
            public double Amount_Remain;
            public double BillValue;
            public Currency _Currency;
            public double ExchangeRate;
            public string PaysAmount;
            public double PaysRemain;
            public double Bill_RealValue;
            public double Bill_Pays_RealValue;
            public string Bill_ItemsOut_Value;
            public double Bill_ItemsOut_RealValue;
            public string Bill_Pays_Return_Value;
            public double Bill_Pays_Return_RealValue;

            public Contact_Buys_ReportDetail(DateTime Bill_Date_,
             int Bill_ID_,
             int ClauseS_Count_,
             double Amount_IN_,
             double Amount_Remain_,
             double BillValue_,
             Currency Currency_,
             double ExchangeRate_,
             string PaysAmount_,
             double PaysRemain_,
             double Bill_RealValue_,
             double Bill_Pays_RealValue_,
             string Bill_ItemsOut_Value_,
             double Bill_ItemsOut_RealValue_,
             string Bill_Pays_Return_Value_,
             double Bill_Pays_Return_RealValue_
               )
            {
                Bill_Date = Bill_Date_;
                Bill_ID = Bill_ID_;
                ClauseS_Count = ClauseS_Count_;
                Amount_IN = Amount_IN_;
                Amount_Remain = Amount_Remain_;
                BillValue = BillValue_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                PaysAmount = PaysAmount_;
                PaysRemain = PaysRemain_;
                Bill_RealValue = Bill_RealValue_;
                Bill_Pays_RealValue = Bill_Pays_RealValue_;
                Bill_ItemsOut_Value = Bill_ItemsOut_Value_;
                Bill_ItemsOut_RealValue = Bill_ItemsOut_RealValue_;
                Bill_Pays_Return_Value = Bill_Pays_Return_Value_;
                Bill_Pays_Return_RealValue = Bill_Pays_Return_RealValue_;

            }
            public static void IntiliazeListView(ref ListView listview)
            {
                try
                {
                    listview.Name = "ListViewBuysDay";
                    listview.Columns.Clear();
                    listview.Columns.Add("التاريخ");
                    listview.Columns.Add("الرقم");
                    listview.Columns.Add("البنود");
                    listview.Columns.Add("اجمالي الكميات");
                    listview.Columns.Add("الكمية المتبقية");
                    listview.Columns.Add("قيمة الفاتورة");
                    listview.Columns.Add("سعر الصرف");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("الباقي");
                    listview.Columns.Add("قيمة الفاتور الفعلية");
                    listview.Columns.Add(" المدفوع الفعلي");
                    listview.Columns.Add("قيمة  الخارج");
                    listview.Columns.Add(" عائدات الفاتورة");
                    listview.Columns.Add("العائدات الفعلية");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Report_Buy_Day_Detail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 75;//time
                    listview.Columns[1].Width = 60;//id
                    listview.Columns[2].Width = 60;//clause count
                    listview.Columns[3].Width = 125;//amount in
                    listview.Columns[4].Width = 125;//amount remain
                    listview.Columns[5].Width = 100;//value
                    listview.Columns[6].Width = 100;//exchangerate
                    listview.Columns[7].Width = 100;//paid
                    listview.Columns[8].Width = 100;//remain
                    listview.Columns[9].Width = 140;//قيمة الفاتور الفعلية
                    listview.Columns[10].Width = 150;// المدفوع الفعلي
                    listview.Columns[11].Width = 140;//قيمة  الخارج
                    listview.Columns[12].Width = 140;//عائدات الفاتورة
                    listview.Columns[13].Width = 140;//القيمة العلية للعائدات

                }
                catch (Exception ee)
                {
                    MessageBox.Show("Contact_Buy_Detail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Contact_Buys_Report
        {

            public int Bills_Count;
            public double Amount_IN;
            public double Amount_Remain;
            public string Bills_Value;

            public string Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double Bills_Pays_Remain_UPON_Bill_Currency;

            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public string Bills_ItemsOut_Value;
            public double Bills_ItemsOut_RealValue;
            public string Bills_Pays_Return_Value;
            public double Bills_Pays_Return_RealValue;
            public Contact_Buys_Report(
              int Bills_Count_,
             double Amount_IN_,
             double Amount_Remain_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,

             double Bills_Pays_Remain_UPON_Bill_Currency_,

             double Bills_RealValue_,
             double Bills_Pays_RealValue_,
             string Bills_ItemsOut_Value_,
             double Bills_ItemsOut_RealValue_,
             string Bills_Pays_Return_Value_,
             double Bills_Pays_Return_RealValue_)
            {
                Bills_Count = Bills_Count_;
                Amount_IN = Amount_IN_;
                Amount_Remain = Amount_Remain_;
                Bills_Value = Bills_Value_;

                Bills_Pays_Value = Bills_Pays_Value_;
                Bills_Pays_Remain = Bills_Pays_Remain_;
                Bills_Pays_Remain_UPON_Bill_Currency = Bills_Pays_Remain_UPON_Bill_Currency_;

                Bills_RealValue = Bills_RealValue_;
                Bills_Pays_RealValue = Bills_Pays_RealValue_;
                Bills_ItemsOut_Value = Bills_ItemsOut_Value_;
                Bills_ItemsOut_RealValue = Bills_ItemsOut_RealValue_;
                Bills_Pays_Return_Value = Bills_Pays_Return_Value_;
                Bills_Pays_Return_RealValue = Bills_Pays_Return_RealValue_;
            }
           
        }
        public class Contact_Sells_ReportDetail
        {

            public DateTime Bill_Date;
            public int Bill_ID;
            public string SellType;
            public int ClauseS_Count;
            public double ItemsOutValue;
            public Currency _Currency;
            public double ExchangeRate;
            public int PaysCount;
            public string PaysAmount;
            public double PaysRemain;
            public string Source_ItemsIN_Cost_Details;
            public double Source_ItemsIN_RealCost;
            public double ItemsOut_RealValue;
            public double RealPaysValue;
            public Contact_Sells_ReportDetail(DateTime Bill_Date_,
             int Bill_ID_,
             string SellType_,
             int ClauseS_Count_,
             double ItemsOutValue_,
             Currency Currency_,
             double ExchangeRate_,
             int PaysCount_,
             string PaysAmount_,
             double PaysRemain_,
             string Source_ItemsIN_Cost_Details_,
             double Source_ItemsIN_RealCost_,
             double ItemsOut_RealValue_,
             double RealPaysValue_
               )
            {
                Bill_Date = Bill_Date_;
                Bill_ID = Bill_ID_;
                SellType = SellType_;
                ClauseS_Count = ClauseS_Count_;
                ItemsOutValue = ItemsOutValue_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                PaysCount = PaysCount_;
                PaysAmount = PaysAmount_;
                PaysRemain = PaysRemain_;
                Source_ItemsIN_Cost_Details = Source_ItemsIN_Cost_Details_;
                Source_ItemsIN_RealCost = Source_ItemsIN_RealCost_;
                ItemsOut_RealValue = ItemsOut_RealValue_;
                RealPaysValue = RealPaysValue_;
            }
            public static void IntiliazeListView(ref ListView listview)
            {
                try
                {
                    listview.Name = "ListViewSellsDay";
                    listview.Columns.Clear();
                    listview.Columns.Add("التاريخ");
                    listview.Columns.Add("الرقم");
                    listview.Columns.Add("نمط البيع");
                    listview.Columns.Add("البنود");
                    listview.Columns.Add("القيمة");
                    listview.Columns.Add("سعر الصرف");
                    //listview.Columns.Add("عدد الدفعات");
                    listview.Columns.Add("المدفوع");
                    listview.Columns.Add("الباقي");
                    listview.Columns.Add("تكلفة المواد");
                    listview.Columns.Add("التكلفة الفعلية");
                    listview.Columns.Add("المباع الفعلي");
                    listview.Columns.Add("الربح الفعلي");
                    listview.Columns.Add("المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("OperationReportDetail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 75;//time
                    listview.Columns[1].Width = 60;//id
                    listview.Columns[2].Width = 75;//selltype
                    listview.Columns[3].Width = 65;//clause count
                    listview.Columns[4].Width = 90;//value
                    listview.Columns[5].Width = 100;//exchangerate
                    listview.Columns[6].Width = 130;//paid
                    listview.Columns[7].Width = 90;//remain
                    listview.Columns[8].Width = 100;//item in cost
                    listview.Columns[9].Width = 130;//real item in cost
                    listview.Columns[10].Width = 130;//real items out cost
                    listview.Columns[11].Width = 130;//profit value
                    listview.Columns[12].Width = 130;//real pays value

                }
                catch (Exception ee)
                {
                    MessageBox.Show("Contact_Sell_Report:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Contact_Sells_Report
        {

            public int Bills_Count;
            public string Bills_Value;
            public string Bills_Pays_Value;
            public string Bills_Pays_Remain;
            public double Bills_Pays_Remain_UPON_BillsCurrency;
            public string Bills_ItemsIN_Value;
            public double Bills_ItemsIN_RealValue;
            public double Bills_RealValue;
            public double Bills_Pays_RealValue;
            public Contact_Sells_Report(
              int Bills_Count_,
             string Bills_Value_,
             string Bills_Pays_Value_,
             string Bills_Pays_Remain_,
             double Bills_Pays_Remain_UPON_BillsCurrency_,
             string Bills_ItemsIN_Value_,
             double Bills_ItemsIN_RealValue_,
             double Bills_RealValue_,
             double Bills_Pays_RealValue_)
            {

                Bills_Count = Bills_Count_;
                Bills_Value = Bills_Value_;
                Bills_Pays_Value = Bills_Pays_Value_;
                Bills_Pays_Remain = Bills_Pays_Remain_;
                Bills_Pays_Remain_UPON_BillsCurrency = Bills_Pays_Remain_UPON_BillsCurrency_;
                Bills_ItemsIN_Value = Bills_ItemsIN_Value_;
                Bills_ItemsIN_RealValue = Bills_ItemsIN_RealValue_;
                Bills_RealValue = Bills_RealValue_;
                Bills_Pays_RealValue = Bills_Pays_RealValue_;
            }
        
        }
        public class Contact_MaintenanceOPRs_ReportDetail
        {

            public DateTime MaintenanceOPR_Date;
            public int MaintenanceOPR_ID;
            public ItemObj.Objects.Item _Item;
            public string FalutDesc;
            public DateTime? MaintenanceOPR_Endworkdate;
            public bool? MaintenanceOPR_Rpaired;
            public DateTime? MaintenanceOPR_DeliverDate;
            public DateTime? MaintenanceOPR_EndWarrantyDate;
            public uint? BillMaintenanceID;
            public double? BillValue;
            public Currency _Currency;
            public double? ExchangeRate;
            public string PaysAmount;
            public double? PaysRemain;
            public string Bill_ItemsOut_Value;
            public double? Bill_ItemsOut_RealValue;
            public double? Bill_RealValue;
            public double? Bill_Pays_RealValue;





            public Contact_MaintenanceOPRs_ReportDetail(
                DateTime MaintenanceOPR_Date_,
             int MaintenanceOPR_ID_,
             ItemObj.Objects.Item Item_,
             string FalutDesc_,
             DateTime? MaintenanceOPR_Endworkdate_,
             bool? MaintenanceOPR_Rpaired_,
             DateTime? MaintenanceOPR_DeliverDate_,
             DateTime? MaintenanceOPR_EndWarrantyDate_,
             uint? BillMaintenanceID_,
              double? BillValue_,
             Currency Currency_,
             double? ExchangeRate_,
             string PaysAmount_,
             double? PaysRemain_,
            string Bill_ItemsOut_Value_,
             double? Bill_ItemsOut_RealValue_,
             double? Bill_RealValue_,
             double? Bill_Pays_RealValue_


               )
            {
                MaintenanceOPR_Date = MaintenanceOPR_Date_;
                MaintenanceOPR_ID = MaintenanceOPR_ID_;
                _Item = Item_;
                FalutDesc = FalutDesc_;
                MaintenanceOPR_Endworkdate = MaintenanceOPR_Endworkdate_;
                MaintenanceOPR_Rpaired = MaintenanceOPR_Rpaired_;
                MaintenanceOPR_DeliverDate = MaintenanceOPR_DeliverDate_;
                MaintenanceOPR_EndWarrantyDate = MaintenanceOPR_EndWarrantyDate_;
                BillMaintenanceID = BillMaintenanceID_;
                BillValue = BillValue_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                PaysAmount = PaysAmount_;
                PaysRemain = PaysRemain_;
                Bill_ItemsOut_Value = Bill_ItemsOut_Value_;
                Bill_ItemsOut_RealValue = Bill_ItemsOut_RealValue_;
                Bill_RealValue = Bill_RealValue_;
                Bill_Pays_RealValue = Bill_Pays_RealValue_;


            }
            public static void IntiliazeListView(ref ListView listview)
            {
                try
                {
                    listview.Columns.Clear();
                    listview.Columns.Add("التاريخ");
                    listview.Columns.Add("الرقم");
                    listview.Columns.Add("الموديل");
                    listview.Columns.Add("الشركة");
                    listview.Columns.Add("الصنف");
                    listview.Columns.Add("وصف العطل");
                    listview.Columns.Add(" انتهاء العمل");
                    listview.Columns.Add("الاصلاح");
                    listview.Columns.Add(" تسليم الجهاز");
                    listview.Columns.Add(" انتهاء الكفالة");
                    listview.Columns.Add("قيمة الفاتورة");
                    listview.Columns.Add(" سعر الصرف");
                    listview.Columns.Add(" المدفوع ");
                    listview.Columns.Add(" المتبقي ");
                    listview.Columns.Add("قيمة  المواد ");
                    listview.Columns.Add("قيمة  المواد  الفعلية");
                    listview.Columns.Add(" قيمة الفاتورة الفعلية");
                    listview.Columns.Add(" المدفوع الفعلي");
                    IntializeListViewColumnsWidth(ref listview);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Report_MaintenanceOPR_Day_Detail:IntiliazeListView" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            public static void IntializeListViewColumnsWidth(ref ListView listview)
            {

                try
                {
                    listview.Columns[0].Width = 75;//time
                    listview.Columns[1].Width = 60;//id
                    listview.Columns[2].Width = 125;//model
                    listview.Columns[3].Width = 125;//company
                    listview.Columns[4].Width = 125;//type
                    listview.Columns[5].Width = 125;//fault desc
                    listview.Columns[6].Width = 125;//end work date
                    listview.Columns[7].Width = 125;//repair
                    listview.Columns[8].Width = 125;//deliver date
                    listview.Columns[9].Width = 125;//end warranty date
                    listview.Columns[10].Width = 125;//قيمة الفاتورة
                    listview.Columns[11].Width = 125;//سعر الصرف
                    listview.Columns[12].Width = 125;//المدفوع
                    listview.Columns[13].Width = 125;//المتبقي
                    listview.Columns[14].Width = 155;//قيمة  الخارج
                    listview.Columns[15].Width = 160;//الخارج الفعلي 
                    listview.Columns[16].Width = 155;//قيمة الفاتور الفعلية
                    listview.Columns[17].Width = 150;// المدفوع الفعلي

                }
                catch (Exception ee)
                {
                    MessageBox.Show("Contact_MaintenanceOPR_Day_Detail:IntializeListViewColumnsWidth" + Environment.NewLine + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public class Contact_MaintenanceOPRs_Report
        {

            public int MaintenanceOPRs_Count;
            public int MaintenanceOPRs_EndWork_Count;
            public int MaintenanceOPRs_Repaired_Count;
            public int MaintenanceOPRs_Warranty_Count;
            public int MaintenanceOPRs_EndWarranty_Count;

            public int BillMaintenances_Count;
            public string BillMaintenances_Value;
            public string BillMaintenances_Pays_Value;
            public string BillMaintenances_Pays_Remain;
            public double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency;

            public string BillMaintenances_ItemsOut_Value;
            public double BillMaintenances_ItemsOut_RealValue;
            public double BillMaintenances_RealValue;
            public double BillMaintenances_Pays_RealValue;
            public Contact_MaintenanceOPRs_Report(
               int MaintenanceOPRs_Count_,
             int MaintenanceOPRs_EndWork_Count_,
             int MaintenanceOPRs_Repaired_Count_,
             int MaintenanceOPRs_Warranty_Count_,
             int MaintenanceOPRs_EndWarranty_Count_,

               int BillMaintenances_Count_,
             string BillMaintenances_Value_,
             string BillMaintenances_Pays_Value_,
             string BillMaintenances_Pays_Remain_,
             double BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_,

             string BillMaintenances_ItemsOut_Value_,
             double BillMaintenances_ItemsOut_RealValue_,
             double BillMaintenances_RealValue_,
             double BillMaintenances_Pays_RealValue_)
            {
                MaintenanceOPRs_Count = MaintenanceOPRs_Count_;
                MaintenanceOPRs_EndWork_Count = MaintenanceOPRs_EndWork_Count_;
                MaintenanceOPRs_Repaired_Count = MaintenanceOPRs_Repaired_Count_;
                MaintenanceOPRs_Warranty_Count = MaintenanceOPRs_Warranty_Count_;
                MaintenanceOPRs_EndWarranty_Count = MaintenanceOPRs_EndWarranty_Count_;

                BillMaintenances_Count = BillMaintenances_Count_;
                BillMaintenances_Value = BillMaintenances_Value_;
                BillMaintenances_Pays_Value = BillMaintenances_Pays_Value_;
                BillMaintenances_Pays_Remain = BillMaintenances_Pays_Remain_;
                BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency = BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency_;

                BillMaintenances_ItemsOut_Value = BillMaintenances_ItemsOut_Value_;
                BillMaintenances_ItemsOut_RealValue = BillMaintenances_ItemsOut_RealValue_;
                BillMaintenances_RealValue = BillMaintenances_RealValue_;
                BillMaintenances_Pays_RealValue = BillMaintenances_Pays_RealValue_;
            }
       

        }
        #endregion
    }
}
