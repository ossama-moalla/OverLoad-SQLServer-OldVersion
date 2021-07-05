using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.ItemObjSQL;
using ItemProject.ItemObj.Objects;
using ItemProject.Maintenance.Objects;
using ItemProject.Trade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Trade
{
    namespace TradeSQL
    {
        public class OperationSQL
        {
            public static class OperationFunctionsSQL
            {
                public const string OperationValue_Function = "dbo.Operation_GetOperation_Value";
                public const string OperationPaysValue_UPON_OperationCurrency_Function = "dbo.Operation_GetPays_Value_UPON_OperationCurrency";
            }
            DatabaseInterface DB;
            public OperationSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public Currency GetOperationItemINCurrency(Operation operation)
            {
                try
                {

                    switch (operation .OperationType )
                    {
                        
                        case Operation.BILL_BUY:
                            BillBuy billbuy = new BillBuySQL(DB).GetBillBuy_INFO_BYID(operation.OperationID);
                            return new Currency(billbuy._Currency.CurrencyID, billbuy._Currency.CurrencyName
                                , billbuy._Currency.CurrencySymbol, billbuy.ExchangeRate, billbuy._Currency.ReferenceCurrencyID);
                        case Operation.ASSEMBLAGE:
                            return ProgramGeneralMethods.GetDefaultCurrency(DB);

                        case Operation.DISASSEMBLAGE:
                            return ProgramGeneralMethods.GetDefaultCurrency(DB);

                        default:
                            throw new Exception("جلب عملة تكلفة ادخال العنصر: العملية غير صحيحة");
                           

                    }


                }
                catch(Exception ee)
                {
                    throw new Exception("فشل جلب عملة العملية المصدر" +ee.Message );
                }

            }
            public Currency GetOperation_BillAdditionalClause_Currency(Operation operation)
            {
                try
                {

                    switch (operation.OperationType)
                    {
                        case Operation.BILL_BUY:
                            BillBuy billbuy = new BillBuySQL(DB).GetBillBuy_INFO_BYID(operation.OperationID);
                            return new Currency(billbuy._Currency.CurrencyID, billbuy._Currency.CurrencyName
                                , billbuy._Currency.CurrencySymbol, billbuy.ExchangeRate, billbuy._Currency.ReferenceCurrencyID);
                        case Operation.BILL_SELL:
                            BillSell BillSell_ = new BillSellSQL(DB).GetBillSell_INFO_BYID(operation.OperationID);
                            return new Currency(BillSell_._Currency.CurrencyID, BillSell_._Currency.CurrencyName
                                , BillSell_._Currency.CurrencySymbol, BillSell_.ExchangeRate, BillSell_._Currency.ReferenceCurrencyID);

                        case Operation.BILL_MAINTENANCE:
                            BillMaintenance BillMaintenance_ = new Maintenance.MaintenanceSQL.BillMaintenanceSQL(DB).GetBillMaintenance_INFO_BYID(operation.OperationID);
                            return new Currency(BillMaintenance_._Currency.CurrencyID, BillMaintenance_._Currency.CurrencyName
                                , BillMaintenance_._Currency.CurrencySymbol, BillMaintenance_.ExchangeRate, BillMaintenance_._Currency.ReferenceCurrencyID);

                       
                        default:
                            throw new Exception("فشل جلب عملة الفاتورة : عملية غير صحيحة");

                    }


                }
                catch
                {
                    throw new Exception("فشل جلب عملة العملية المصدر");
                }

            }
            public Currency GetOperationItemOUTCurrency(Operation operation)
            {
                try
                {

                    switch (operation.OperationType)
                    {
                        case Operation.BILL_SELL:
                            BillSell BillSell_ = new BillSellSQL(DB).GetBillSell_INFO_BYID(operation.OperationID);
                            return new Currency(BillSell_._Currency.CurrencyID, BillSell_._Currency.CurrencyName
                                , BillSell_._Currency.CurrencySymbol, BillSell_.ExchangeRate, BillSell_._Currency.ReferenceCurrencyID);

                        case Operation.BILL_MAINTENANCE:
                            BillMaintenance BillMaintenance_ = new Maintenance.MaintenanceSQL.BillMaintenanceSQL(DB).GetBillMaintenance_INFO_BYID(operation.OperationID);
                            return new Currency(BillMaintenance_._Currency.CurrencyID, BillMaintenance_._Currency.CurrencyName
                                , BillMaintenance_._Currency.CurrencySymbol, BillMaintenance_.ExchangeRate, BillMaintenance_._Currency.ReferenceCurrencyID);

                        case Operation.REPAIROPR:
                            return ProgramGeneralMethods.GetDefaultCurrency(DB);

                        case Operation.ASSEMBLAGE:
                            return ProgramGeneralMethods.GetDefaultCurrency(DB);

                        case Operation.DISASSEMBLAGE:
                            return ProgramGeneralMethods.GetDefaultCurrency(DB);

                        case Operation.RAVAGE:
                            return ProgramGeneralMethods.GetDefaultCurrency(DB);
                        default:
                            throw new Exception("جلب عملة تكلفة اخراج العنصر: العملية غير صحيحة");

                    }


                }
                catch
                {
                    throw new Exception("فشل جلب عملة العملية المصدر");
                }

            }
            public Bill GetOperationBill(Operation operation)
            {
                try
                {
                    switch (operation.OperationType)
                    {
                        case Operation.BILL_BUY:
                            BillBuy billbuy = new BillBuySQL(DB).GetBillBuy_INFO_BYID(operation.OperationID);
                            return billbuy;
                       case Operation.BILL_SELL:
                             BillSell BillSell_ = new BillSellSQL(DB).GetBillSell_INFO_BYID(operation.OperationID);
                            return BillSell_;
                        case Operation.BILL_MAINTENANCE:
                             BillMaintenance BillMaintenance_ = new Maintenance.MaintenanceSQL.BillMaintenanceSQL(DB).GetBillMaintenance_INFO_BYID(operation.OperationID);
                            return BillMaintenance_;
                        default:
                            return null;


                    }
                }
                catch
                {
                    return null;
                }
            }
            public double  Get_OperationValue(uint OperationType,uint OperationID)
            {
                try
                {

                    DataTable t = DB.GetData(
                         "select "
                         +OperationFunctionsSQL.OperationValue_Function
                         +"("
                    + OperationType
                    +","
                    + OperationID
                     + ")"
                     );
                    return Convert.ToDouble(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_OperationValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }

            }
            public double Get_OperationPaysValue_UPON_OperationCurrency(uint OperationType, uint OperationID)
            {
                try
                {
                    DataTable t = DB.GetData(
                         "select "
                         + OperationFunctionsSQL.OperationPaysValue_UPON_OperationCurrency_Function
                         + "("
                    + OperationType
                    + ","
                    + OperationID
                     + ")"
                     );
                    return Convert.ToDouble(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_OperationValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }

            }
            public double Get_OperationValue(Operation Operation_)
            {
                try
                {

                    DataTable t = DB.GetData(
                         "select "
                         + OperationFunctionsSQL.OperationValue_Function
                         + "("
                    + Operation_. OperationType
                    + ","
                    + Operation_.OperationID
                     + ")"
                     );
                    return Convert.ToDouble(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_OperationValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }

            }
            public double Get_OperationPaysValue_UPON_OperationCurrency(Operation Operation_)
            {
                try
                {
                    DataTable t = DB.GetData(
                         "select "
                         + OperationFunctionsSQL.OperationPaysValue_UPON_OperationCurrency_Function
                         + "("
                    + Operation_.OperationType
                    + ","
                    + Operation_.OperationID
                     + ")"
                     );
                    return Convert.ToDouble(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_OperationValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }

            }
        }
        public class BillAdditionalClauseSQL
        {
            DatabaseInterface DB;
            private static class BillAdditionalClauseTable
            {
                public const string TableName = "Trade_Bill_AdditionalClause";
                public const string ClauseID = "ClauseID";
                public const string OperationType = "OperationType";
                public const string OperationID = "OperationID";
                public const string Description = "Description";
                public const string Value_ = "Value_";


            }
            public BillAdditionalClauseSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public BillAdditionalClause Get_BillAdditionalClause_INFO_BYID(uint ClauseID)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + BillAdditionalClauseTable.OperationType + ","
                    + BillAdditionalClauseTable.OperationID + ","
                    + BillAdditionalClauseTable.Description + ","
                    + BillAdditionalClauseTable.Value_
                    + " from   "
                    + BillAdditionalClauseTable.TableName
                    + " where "
                    + BillAdditionalClauseTable.ClauseID + "=" + ClauseID
                      );
                    if (t.Rows.Count == 1)
                    {
                        uint operation_type = Convert.ToUInt32(t.Rows[0][0]);
                        uint operation_id = Convert.ToUInt32(t.Rows[0][1]);
                        string desc = t.Rows[0][2].ToString();
                        double value = Convert.ToDouble(t.Rows[0][3].ToString());

                        return new BillAdditionalClause(new Operation(operation_type, operation_id), ClauseID,
                            desc, value);

                    }
                    else
                        return null;
                }
                catch(Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_BillAdditionalClause_INFO_BYID:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
               
            }
            public bool  AddBillAdditionalClause(Operation Operation_,string description, double Value)
            {
                try
                {


                    DataTable t = DB.GetData(" insert into "
                    + BillAdditionalClauseTable.TableName
                    + "("
                    + BillAdditionalClauseTable.OperationType
                    + ","
                    + BillAdditionalClauseTable.OperationID
                    + ","
                    + BillAdditionalClauseTable.Description 
                    + ","
                    + BillAdditionalClauseTable.Value_ 
                    
                    + ")"
                    + "values"
                    + "("
                   + Operation_.OperationType
                    + ","
                    + Operation_.OperationID
                    + ","
                    +"'"+description +"'"
                    + ","
                    + Value         
                    + ")"
                    );
  


                    DB.AddLog(
                          DatabaseInterface.Log.LogType.INSERT
                          , DatabaseInterface.Log.Log_Target.Trade_BillAdditionalClause
                          , ""
                          , true, "");

                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_BillAdditionalClause
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("AddBillAdditionalClause:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool UpdateBillAdditionalClause(uint ClauseID, string description, double Value)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + BillAdditionalClauseTable.TableName
                    + " set "
   
                    + BillAdditionalClauseTable.Description  + "='" + description + "'"
                    + ","
                    + BillAdditionalClauseTable.Value_  + "=" + Value 
                   
                    + " where "
                    + BillAdditionalClauseTable.ClauseID  + "=" + ClauseID
                    );
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.UPDATE
                        , DatabaseInterface.Log.Log_Target.Trade_BillAdditionalClause 
                         , ""
                         , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE
                            , DatabaseInterface.Log.Log_Target.Trade_BillAdditionalClause 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("UpdateBillAdditionalClause:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteBillAdditionalClause(uint clauseID)
            {
                try
                {
      
                    DB.ExecuteSQLCommand("delete from   "
                    + BillAdditionalClauseTable.TableName
                    + " where "
                    + BillAdditionalClauseTable.ClauseID  + "=" + clauseID
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE
                       , DatabaseInterface.Log.Log_Target.Trade_BillAdditionalClause 
                        , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE
                            , DatabaseInterface.Log.Log_Target.Trade_BillAdditionalClause 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("DeleteBillAdditionalClause:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List <BillAdditionalClause > GetBill_AdditionalClauses(Operation Operation_)
            {
                List<BillAdditionalClause> List = new List<BillAdditionalClause>();
                try
                {
                    DataTable  t = DB.GetData("select "
                    + BillAdditionalClauseTable.ClauseID  + ","
                    + BillAdditionalClauseTable.Description + ","
                    + BillAdditionalClauseTable.Value_
                    + " from   "
                    + BillAdditionalClauseTable.TableName
                    + " where "
                    + BillAdditionalClauseTable.OperationType + "=" + Operation_.OperationType
                     + " and "
                    + BillAdditionalClauseTable.OperationID + "=" + Operation_.OperationID 
                      );
                    for(int i=0;i< t.Rows.Count;i++)
                    {
                        uint ClauseID = Convert.ToUInt32(t.Rows[i][0]);
                        string desc = t.Rows[i][1].ToString();
                        double value = Convert.ToDouble(t.Rows[i][2].ToString());

                        List .Add ( new BillAdditionalClause(Operation_, ClauseID,
                            desc, value));

                    }
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBill_AdditionalClauses:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
                return List;
            }
        }
        public class BillBuySQL
        {
            DatabaseInterface DB;
            private static class BillBuyTable
            {
                public const string TableName = "Trade_BillBuy";
                public const string BillBuyID = "BillBuyID";
                public const string BillDate = "BillDate";
                public const string BillDescription = "BillDescription";
                public const string ContactID = "ContactID";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string Discount = "Discount";
                public const string Notes = "Notes";

            }
            public BillBuySQL(DatabaseInterface db)
            {
                DB = db;

            }
            public BillBuy GetBillBuy_INFO_BYID(uint billid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                + BillBuyTable.BillDate+","
                + BillBuyTable.BillDescription + ","
                + BillBuyTable.ContactID + ","
                + BillBuyTable.CurrencyID + ","
                + BillBuyTable.ExchangeRate  + ","
                + BillBuyTable.Discount + ","
                + BillBuyTable.Notes 
                + " from   "
                + BillBuyTable.TableName
                + " where "
                + BillBuyTable.BillBuyID + "=" + billid 
                  );
                if (t.Rows.Count == 1)
                {
                    DateTime billdate = Convert.ToDateTime(t.Rows [0][0]);
                    string desc = t.Rows[0][1].ToString ();
                    Contact  Contact_ = new ContactSQL(DB).GetContactInforBYID (Convert.ToUInt32(t.Rows[0][2].ToString()));
                    Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][3].ToString()));
                    double exchangerate = Convert.ToDouble(t.Rows[0][4].ToString());
                    double discount =Convert .ToDouble ( t.Rows[0][5].ToString());
                    string notes = t.Rows[0][6].ToString();
                    return new BillBuy  (billid  ,billdate  ,desc ,Contact_,Currency_,exchangerate ,discount ,notes );

                }
                else
                    return null;
            }
            public BillBuy   AddBillBuy( DateTime billdate,string description,Contact contact,Currency currency,double ExchangeRate,double discount,string notes)
            {
                try
                {
                   

                    DataTable t= DB.GetData (" insert into "
                    + BillBuyTable.TableName
                    + "("
                    + BillBuyTable.BillDate  
                    + ","
                    + BillBuyTable.BillDescription 
                    + ","
                    + BillBuyTable.ContactID 
                    + ","
                    + BillBuyTable.CurrencyID 
                    + ","
                    + BillBuyTable.ExchangeRate 
                    + ","
                    + BillBuyTable.Discount 
                    + ","
                    + BillBuyTable.Notes 
                    + ")"
                    + "values"
                    + "("
                    + "'" + billdate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'"+description +"'" 
                    + ","
                    +contact .ContactID 
                    + ","
                    + currency .CurrencyID 
                    + ","
                     +ExchangeRate 
                    + ","
                    + discount 
                    + ","
                    + "'"+notes +"'"
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint billbuyid = Convert.ToUInt32(t.Rows [0][0].ToString ());

                    
                    DB.AddLog(
                                                    DatabaseInterface.Log.LogType.INSERT
                                                    , DatabaseInterface.Log.Log_Target.Trade_BillBuy 
                                                    , ""
                                                                              , true, "");
                    return GetBillBuy_INFO_BYID(billbuyid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_BillBuy 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateBillBuy(uint billid,DateTime billoutdate,string description, Contact contact,Currency currency,double ExchangeRate,double discount,string notes)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + BillBuyTable.TableName
                    + " set "
                    + BillBuyTable.BillDate  + "='" + billoutdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + BillBuyTable.BillDescription + "='" + description   + "'"
                    + ","
                    + BillBuyTable.ContactID + "=" + contact .ContactID 
                    + ","
                    + BillBuyTable.CurrencyID  + "=" + currency .CurrencyID
                    + ","
                     + BillBuyTable.ExchangeRate  + "=" + ExchangeRate 
                    + ","
                    + BillBuyTable.Discount  + "=" + discount  
                    + ","
                    + BillBuyTable.Notes  + "='" + notes  + "'"

                    + " where "
                    + BillBuyTable.BillBuyID  + "=" + billid  
                    );
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.UPDATE 
                        , DatabaseInterface.Log.Log_Target.Trade_BillBuy
                         , ""
                         , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_BillBuy
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteBillBuy(uint billid)
            {
                try
                {
                    if(new ItemINSQL (DB).Does_Operation_Has_ItemsIN (Operation.BILL_BUY,billid ))
                    {
                        throw new Exception("توجد عناصر داخلة تابعة لهذه الفاتورة يجب حذفها اولا");
                    }
                    DB.ExecuteSQLCommand("delete from   "
                   + PayOUTSQL .PayOUTTable.TableName
                   + " where "
                   + PayOUTSQL.PayOUTTable.OperationID + "=" + billid
                    + " and "
                   + PayOUTSQL.PayOUTTable.OperationType  + "=" + Operation.BILL_BUY 
                   );
                   
                    DB.ExecuteSQLCommand("delete from   "
                    + BillBuyTable.TableName
                    + " where "
                    + BillBuyTable.BillBuyID  + "=" + billid  
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE 
                       , DatabaseInterface.Log.Log_Target.Trade_BillBuy
                        , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_BillBuy
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            //public List <BillOUT > GetBillOUTList()
            //{
            //    try
            //    {
            //        List<TradeIN > tradeinlist = new List<TradeIN >();
            //        DataTable t = new DataTable();
            //        t = DB.GetData("select * from   "
            //        + BillOUTTable.TableName
            //          );

            //        for (int i = 0; i < t.Rows.Count; i++)
            //        {

            //            int sourceid = Convert.ToInt32 (t.Rows[i][0].ToString());
            //            TradeSource TradeSource_ = new TradeSourceSQL(DB).GetTradeSourceInforBYID(Convert.ToInt32(t.Rows[i][2].ToString()));
            //            DateTime tradeindate = Convert.ToDateTime(t.Rows[i][3].ToString());
            //            string notes = t.Rows[i][4].ToString();

            //            tradeinlist.Add(new TradeIN  (sourceid   , TradeSource_ , tradeindate ,notes  ));
            //        }
            //        return tradeinlist;
            //    }
            //    catch (Exception ee)
            //    {
            //        System.Windows.Forms.MessageBox.Show("", "" + ee.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //        return null ;
            //    }
            //}
            internal double GetBillBuyValue(uint billid)
            {
                try
                {

                    return new OperationSQL(DB).Get_OperationValue(Operation.BILL_BUY,billid);
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillBuyValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
            internal double GetBillBuy_PaysValue(uint billid)
            {
                try
                {

                    return new OperationSQL(DB).Get_OperationPaysValue_UPON_OperationCurrency(Operation.BILL_BUY, billid);
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillBuyValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
        }
        public class ContactSQL
        {

            DatabaseInterface DB;
            
            private static class ContactBillsTable
            {
                public const string TableName = " [dbo].[Bills_Get_Contact_BillsReport_Details]";
                public const string Bill_Date = "Bill_Date";
                public const string Bill_ID = "Bill_ID";
                public const string BillType = "BillType";
                public const string Bill_Description = "Bill_Description";
                public const string Bill_Operations = "Bill_Operations";
                public const string Currency_Name = "Currency_Name";
                public const string Bill_Value = "Bill_Value";
                public const string Pays_Value = "Pays_Value";
                public const string Remain = "Remain";

            }

            private static class ContactBillsReportTable
            {
                public const string TableName = "[dbo].[Bills_Get_Contact_BillsReport]";
                public const string CurrencyID = "CurrencyID";
                public const string Currency = "Currency";
                public const string BillsIN_Count = "BillsIN_Count";
                public const string BillsIN_Value = "BillsIN_Value";
                public const string BillsIN_Pays_Value = "BillsIN_Pays_Value";
                public const string BillsM_Count = "BillsM_Count";
                public const string BillsM_Value = "BillsM_Value";
                public const string BillsM_Pays_Value = "BillsM_Pays_Value";
                public const string BillsOUT_Count = "BillsOUT_Count";
                public const string BillsOUT_Value = "BillsOUT_Value";
                public const string BillsOUT_Pays_Value = "BillsOUT_Pays_Value";


            }
            private static class ContactTable
            {
                public const string TableName = "Trade_Contact";
                public const string ContactID = "ContactID";
                public const string ContactType = "ContactType";
                public const string Name = "Name";
                public const string Phone = "Phone";
                public const string Mobile = "Mobile";
                public const string Address = "Address";

            }
            public ContactSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public Contact GetContactInforBYID(uint contactid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                    +ContactTable.ContactType + ","
                    + ContactTable.Name +","
                    + ContactTable.Phone + ","
                    + ContactTable.Mobile + ","
                    + ContactTable.Address 
                    + " from   "
                    + ContactTable.TableName
                    + " where "
                    + ContactTable.ContactID  + "=" + contactid
                  );
                if (t.Rows.Count == 1)
                {
                    bool contacttype = Convert.ToBoolean(t.Rows[0][0].ToString());
                    string name = t.Rows[0][1].ToString();
                    string phone = t.Rows[0][2].ToString();
                    string mobile = t.Rows[0][3].ToString();
                    string address = t.Rows[0][4].ToString();

                    return new Contact(contactid,contacttype  ,name , phone, mobile, address);

                }
                else
                    return null;
            }
            public bool AddContact(bool type, string name,string phone,string mobile,string address)
            {
                try
                {
                    DB.ExecuteSQLCommand(" insert into "
                    + ContactTable.TableName
                    + "("
                    + ContactTable.ContactType + ","
                    + ContactTable.Name+","
                    + ContactTable.Phone  + ","
                    + ContactTable.Mobile  + ","
                    + ContactTable.Address  
                    + ")"
                    + "values"
                    + "("
                    +(type?1:0).ToString ()
                    +","
                    + "'" + name   + "'"
                    +","
                    + "'" + phone  + "'"
                    + ","
                    + "'" + mobile  + "'"
                    + ","
                    + "'" + address  + "'"
                    + ")"
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.INSERT 
                       , DatabaseInterface.Log.Log_Target.Trade_Contact
                        , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Trade_Contact
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateContact(uint contactid,bool type, string name, string phone, string mobile, string address)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                       + ContactTable.TableName
                       + " set "
                       + ContactTable.ContactType + "=" + (type ? 0 : 1).ToString()
                       +","
                       + ContactTable.Name   + "='" + name   + "'"
                       +","
                       + ContactTable.Phone + "='" + phone  + "'"
                       + ","
                       + ContactTable.Mobile  + "='" + mobile  + "'"
                       + ","
                       + ContactTable.Address + "='" + address  + "'"
                    + " where "
                    + ContactTable.ContactID + "=" + contactid
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                      , DatabaseInterface.Log.Log_Target.Trade_Contact
                       , ""
                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_Contact
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteContact(uint contactid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + ContactTable.TableName
                    + " where "
                    + ContactTable.ContactID   + "=" + contactid
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE 
                       , DatabaseInterface.Log.Log_Target.Trade_Contact
                        , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_Contact
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<Contact > GetContactList()
            {
                List<Contact> contactlist = new List<Contact>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ContactTable.ContactID + ","
                    + ContactTable.ContactType + ","
                    + ContactTable.Name + ","
                    + ContactTable.Phone + ","
                    + ContactTable.Mobile + ","
                    + ContactTable.Address
                    + " from   "
                    + ContactTable.TableName
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint contactid =  Convert.ToUInt32(t.Rows[i][0].ToString());
                        bool contacttype = Convert.ToBoolean(t.Rows[i][1].ToString());
                        string name = t.Rows[i][2].ToString();
                        string phone = t.Rows[i][3].ToString();
                        string mobile = t.Rows[i][4].ToString();
                        string address = t.Rows[i][5].ToString();

                        contactlist.Add ( new Contact(contactid, contacttype, name, phone, mobile, address));
                    }
                    return contactlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return contactlist;
                }
            }
            internal List<Contact> SearchContact(string text)
            {
                List<Contact> list = new List<Contact>();
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ContactTable.ContactID + ","
                    + ContactTable.ContactType + ","
                    + ContactTable.Name + ","
                    + ContactTable.Phone + ","
                    + ContactTable.Mobile + ","
                    + ContactTable.Address
                    + " from   "
                    + ContactTable.TableName
                       + " where " + ContactTable.Name  + " like  '%" +text  + "%'");
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint contactid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        bool contacttype = Convert.ToBoolean(t.Rows[i][1].ToString());
                        string name = t.Rows[i][2].ToString();
                        string phone = t.Rows[i][3].ToString();
                        string mobile = t.Rows[i][4].ToString();
                        string address = t.Rows[i][5].ToString();

                        list.Add(new Contact(contactid, contacttype, name, phone, mobile, address));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("SearchFolder:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }
            }
            public List<BillReportDetail> GetContactBillsList(Contact Contact_)
            {
                List<BillReportDetail> BillReportDetailList = new List<BillReportDetail>();
                try
                {

                    DataTable table = new DataTable();
                    table = DB.GetData("select "
                        + ContactBillsTable.Bill_Date +","
                        + ContactBillsTable.Bill_ID + ","
                        + ContactBillsTable.BillType + ","
                        + ContactBillsTable.Bill_Description + ","
                        + ContactBillsTable.Bill_Operations + ","
                        + ContactBillsTable.Currency_Name + ","
                        + ContactBillsTable.Bill_Value + ","
                        + ContactBillsTable.Pays_Value  + ","
                        + ContactBillsTable.Remain  
                        + " from  "
                        +ContactBillsTable .TableName 
                        +"("
                        + Contact_.ContactID 
                        +")"
                        + " order by Bill_Date"

                  );

                    for (int i = 0; i <table .Rows.Count; i++)
                    {
                        DateTime billdate = Convert.ToDateTime(table.Rows[i][0].ToString());
                        int billid = Convert.ToInt32(table.Rows[i][1].ToString());
                        string billtype = table.Rows[i][2].ToString();
                        string desc = table.Rows[i][3].ToString();
                        string operations = table.Rows[i][4].ToString();
                        string currency = table.Rows[i][5].ToString();
                        double value = Convert.ToDouble(table.Rows[i][6].ToString());
                        double paid = Convert.ToDouble(table.Rows[i][7].ToString());
                        double remain = Convert.ToDouble(table.Rows[i][8].ToString());
                        BillReportDetail billreportDetail = new BillReportDetail(billdate, billid, billtype, desc, Contact_.ContactName, operations, currency, value, paid, remain);
                        BillReportDetailList.Add(billreportDetail);
                    }
                    return BillReportDetailList;

                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return BillReportDetailList;
                }
            }
            public List<BillCurrencyReport> GetContactBillsReportList(Contact Contact_)
            {
                List<BillCurrencyReport> BillCurrencyReportList = new List<BillCurrencyReport>();

                if (Contact_ == null) return BillCurrencyReportList;
                try
                {

                    DataTable table = new DataTable();
                    table = DB.GetData("select "
                        + ContactBillsReportTable.CurrencyID + ","
                        + ContactBillsReportTable.Currency + ","
                        + ContactBillsReportTable.BillsIN_Count + ","
                        + ContactBillsReportTable.BillsIN_Value + ","
                        + ContactBillsReportTable.BillsIN_Pays_Value + ","
                       + ContactBillsReportTable.BillsM_Count + ","
                        + ContactBillsReportTable.BillsM_Value + ","
                        + ContactBillsReportTable.BillsM_Pays_Value + ","
                        + ContactBillsReportTable.BillsOUT_Count  + ","
                        + ContactBillsReportTable.BillsOUT_Value  + ","
                        + ContactBillsReportTable.BillsOUT_Pays_Value  
                        + " from  "
                        + ContactBillsReportTable.TableName
                        + "("
                        + Contact_.ContactID
                        + ")"

                        
                  );
                   
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
                catch(Exception ee)
                {
                    MessageBox.Show(" GetContactBillsReportList:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BillCurrencyReportList;
                }
            }

            #region ReportDetails
            public static class Contact_Pays_GetReport_Detail_Table
            {
                public const string TableName = " [dbo].[Contact_Pays_GetReport_Details]";
                public const string OperationType = "OperationType";
                public const string OperationID = "OperationID";
                public const string Direction = "Direction";
                public const string PayDate = "PayDate";
                public const string PayID = "PayID";
                public const string Value_ = "Value_";
                public const string Currency = "Currency";
                public const string ExchangeRate = "ExchangeRate";
                public const string RealValue = "RealValue";
            }
            public static class Contact_Buy_GetReport_Details_Table
            {

                public const string TableName = "[dbo].[Contact_Buy_GetReport_Details]";
                public const string Bill_Date = "Bill_Date";
                public const string Bill_ID = "Bill_ID";
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
            public static class Contact_Sell_GetReport_Details_Table
            {

                public const string TableName = "[dbo].[Contact_Sell_GetReport_Details]";
                public const string Bill_Date = "Bill_Date";
                public const string Bill_ID = "Bill_ID";
                public const string SellType = "SellType";
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
            public static class Contact_Maintenance_GetReport_Details_Table
            {

                public const string TableName = "[dbo].[Contact_Maintenance_GetReport_Details]";
                public const string MaintenanceOPR_Date = "MaintenanceOPR_Date";
                public const string MaintenanceOPR_ID = "MaintenanceOPR_ID";
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
            public List<Contact_Pays_ReportDetail> Get_Contact_Pays_ReportDetail(uint ContactID)
            {
                List<Contact_Pays_ReportDetail> Contact_Pays_ReportDetailList = new List<Contact_Pays_ReportDetail>();
                try
                {

                    DataTable table = new DataTable();
                    table = DB.GetData("select "
                     + Contact_Pays_GetReport_Detail_Table.OperationType  + ","
                     + Contact_Pays_GetReport_Detail_Table.OperationID + ","
                     + Contact_Pays_GetReport_Detail_Table.Direction + ","
                     + Contact_Pays_GetReport_Detail_Table.PayDate  + ","
                     + Contact_Pays_GetReport_Detail_Table.PayID  + ","
                     + Contact_Pays_GetReport_Detail_Table.Value_ + ","
                     + Contact_Pays_GetReport_Detail_Table.Currency + ","
                     + Contact_Pays_GetReport_Detail_Table.ExchangeRate + ","
                     + Contact_Pays_GetReport_Detail_Table.RealValue
                    + " from   "
                    + Contact_Pays_GetReport_Detail_Table.TableName
                    + " ( "
                    + ContactID
                    + ") order by "
                    + Contact_Pays_GetReport_Detail_Table.PayDate 
                      );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        uint operationtype= Convert.ToUInt32(table.Rows[i][0].ToString());
                        uint operationID = Convert.ToUInt32(table.Rows[i][1].ToString());
                        bool Direction_ = Convert.ToBoolean(table.Rows[i][2].ToString());
                        DateTime paydate = Convert.ToDateTime(table.Rows[i][3].ToString());
                        int payid = Convert.ToInt32(table.Rows[i][4].ToString());
                        double value = Convert.ToDouble(table.Rows[i][5].ToString());
                        string currency = table.Rows[i][6].ToString();
                        double exchangerate = Convert.ToDouble(table.Rows[i][7].ToString());
                        double realvalue = Convert.ToDouble(table.Rows[i][8].ToString());

                        Contact_Pays_ReportDetail Contact_Pays_ReportDetail_
                            = new Contact_Pays_ReportDetail(operationtype,operationID,
                            Direction_ , paydate , payid 
                            , value, currency, exchangerate, realvalue);
                        Contact_Pays_ReportDetailList.Add(Contact_Pays_ReportDetail_);
                    }
                    return Contact_Pays_ReportDetailList;
                }
                catch
                {
                    MessageBox.Show("فشل جلب تقرير دفعات اليوم التفصيلي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return Contact_Pays_ReportDetailList;
                }
            }
            public List<Contact_MaintenanceOPRs_ReportDetail> Get_Contact_MaintenanceOPRs_ReportDetail(uint ContactID)
            {
                List<Contact_MaintenanceOPRs_ReportDetail> List = new List<Contact_MaintenanceOPRs_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Contact_Maintenance_GetReport_Details_Table.MaintenanceOPR_Date + ","
                   + Contact_Maintenance_GetReport_Details_Table.MaintenanceOPR_ID + ","
                   + Contact_Maintenance_GetReport_Details_Table.ItemID + ","
                   + Contact_Maintenance_GetReport_Details_Table.FalutDesc + ","
                   + Contact_Maintenance_GetReport_Details_Table.MaintenanceOPR_Endworkdate + ","
                   + Contact_Maintenance_GetReport_Details_Table.MaintenanceOPR_Rpaired + ","
                   + Contact_Maintenance_GetReport_Details_Table.MaintenanceOPR_DeliverDate + ","
                   + Contact_Maintenance_GetReport_Details_Table.MaintenanceOPR_EndWarrantyDate + ","
                   + Contact_Maintenance_GetReport_Details_Table.BillMaintenanceID + ","
                   + Contact_Maintenance_GetReport_Details_Table.BillValue + ","
                   + Contact_Maintenance_GetReport_Details_Table.CurrencyID + ","
                   + Contact_Maintenance_GetReport_Details_Table.ExchangeRate + ","
                   + Contact_Maintenance_GetReport_Details_Table.PaysAmount + ","
                   + Contact_Maintenance_GetReport_Details_Table.PaysRemain + ","
                   + Contact_Maintenance_GetReport_Details_Table.Bill_ItemsOut_Value + ","
                   + Contact_Maintenance_GetReport_Details_Table.Bill_ItemsOut_RealValue + ","
                   + Contact_Maintenance_GetReport_Details_Table.Bill_RealValue + ","
                   + Contact_Maintenance_GetReport_Details_Table.Bill_Pays_RealValue

                   + " from "
                   + Contact_Maintenance_GetReport_Details_Table.TableName
                    + "("
                    +ContactID
                   + ")"
                   + " order by  "
                   + Contact_Maintenance_GetReport_Details_Table.MaintenanceOPR_Date 
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime maintenanceopr_date = Convert.ToDateTime(table.Rows[i][0].ToString());
                        int maintenance_opr_id = Convert.ToInt32(table.Rows[i][1].ToString());
                        Item Item_ = new ItemObj.ItemObjSQL.ItemSQL(DB)
                            .GetItemInfoByID(Convert.ToUInt32(table.Rows[i][2]));
                        string faultdesc = table.Rows[i][3].ToString();



                        DateTime? endworkdate, deliverdate, endwarrantry;
                        bool? Repaired;
                        uint? BillID;
                        Currency currency;
                        double? billvalue, exchangerate, paysremain, itemsoutRealvalue, billrealvalue, bill_Pays_realvalue;
                        string PaysAmount, itemsoutvalue;


                        try
                        {
                            endworkdate = Convert.ToDateTime(table.Rows[i][4]);
                            Repaired = Convert.ToBoolean(table.Rows[i][5]);
                        }
                        catch
                        {
                            endworkdate = null;
                            Repaired = null;
                        }

                        try
                        {
                            deliverdate = Convert.ToDateTime(table.Rows[i][6]);
                        }
                        catch
                        {
                            deliverdate = null;
                        }
                        try
                        {
                            endwarrantry = Convert.ToDateTime(table.Rows[i][7]);
                        }
                        catch
                        {
                            endwarrantry = null;
                        }
                        try
                        {

                            BillID = Convert.ToUInt32(table.Rows[i][8]);
                            billvalue = Convert.ToDouble(table.Rows[i][9].ToString());
                            currency = new CurrencySQL(DB)
                               .GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][10].ToString()));
                            exchangerate = Convert.ToDouble(table.Rows[i][11].ToString());
                            PaysAmount = table.Rows[i][12].ToString();
                            paysremain = Convert.ToDouble(table.Rows[i][13].ToString()); ;

                            itemsoutvalue = table.Rows[i][14].ToString();
                            itemsoutRealvalue = Convert.ToDouble(table.Rows[i][15].ToString());
                            billrealvalue = Convert.ToDouble(table.Rows[i][16].ToString()); ;
                            bill_Pays_realvalue = Convert.ToDouble(table.Rows[i][17].ToString()); ;

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




                        Contact_MaintenanceOPRs_ReportDetail Contact_MaintenanceOPRs_ReportDetail_
                            = new Contact_MaintenanceOPRs_ReportDetail(maintenanceopr_date
                            , maintenance_opr_id, Item_, faultdesc, endworkdate, Repaired, deliverdate
                            , endwarrantry, BillID, billvalue, currency, exchangerate, PaysAmount, paysremain,
                            itemsoutvalue, itemsoutRealvalue, billrealvalue, bill_Pays_realvalue);
                        List.Add(Contact_MaintenanceOPRs_ReportDetail_);
                    }
                    return List;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Contact_MaintenanceOPRs_ReportDetail: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public List<Contact_Sells_ReportDetail> Get_Contact_Sells_ReportDetail(uint ContactID)
            {
                List<Contact_Sells_ReportDetail> List = new List<Contact_Sells_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Contact_Sell_GetReport_Details_Table.Bill_Date + ","
                   + Contact_Sell_GetReport_Details_Table.Bill_ID + ","
                   + Contact_Sell_GetReport_Details_Table.SellType + ","
                   + Contact_Sell_GetReport_Details_Table.ClauseS_Count + ","
                   + Contact_Sell_GetReport_Details_Table.ItemsOutValue + ","
                   + Contact_Sell_GetReport_Details_Table.CurrencyID + ","
                   + Contact_Sell_GetReport_Details_Table.ExchangeRate + ","
                   + Contact_Sell_GetReport_Details_Table.PaysCount + ","
                   + Contact_Sell_GetReport_Details_Table.PaysAmount + ","
                   + Contact_Sell_GetReport_Details_Table.PaysRemain + ","
                   + Contact_Sell_GetReport_Details_Table.Source_ItemsIN_Cost_Details + ","
                   + Contact_Sell_GetReport_Details_Table.Source_ItemsIN_RealCost + ","
                   + Contact_Sell_GetReport_Details_Table.ItemOUT_RealValue + ","
                   + Contact_Sell_GetReport_Details_Table.RealPaysValue
                   + " from "
                   + Contact_Sell_GetReport_Details_Table.TableName
                    + "("
                   +ContactID
                   + ")"
                   + " order by  "
                   + Contact_Sell_GetReport_Details_Table.Bill_Date
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime billdate = Convert.ToDateTime(table.Rows[i][0].ToString());
                        int billid = Convert.ToInt32(table.Rows[i][1].ToString());
                        string selltype = table.Rows[i][2].ToString();
                        int clause_Count = Convert.ToInt32(table.Rows[i][3].ToString());
                        double items_outvalue = Convert.ToDouble(table.Rows[i][4].ToString());
                        Currency currency = new CurrencySQL(DB)
                            .GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][5].ToString()));
                        double exchangerate = Convert.ToDouble(table.Rows[i][6].ToString());
                        int PaysCount = Convert.ToInt32(table.Rows[i][7].ToString());
                        string PaysAmount = table.Rows[i][8].ToString();
                        double remain = Convert.ToDouble(table.Rows[i][9].ToString()); ;
                        string bill_source_itemsin_cost = table.Rows[i][10].ToString();
                        double bill_source_itemsin_realcost = Convert.ToDouble(table.Rows[i][11].ToString());
                        double itemsout_realvalue = Convert.ToDouble(table.Rows[i][12].ToString());
                        double real_pays = Convert.ToDouble(table.Rows[i][13].ToString());

                        Contact_Sells_ReportDetail Contact_Sells_ReportDetail_
                            = new Contact_Sells_ReportDetail(billdate, billid, selltype, 
                            clause_Count, items_outvalue, currency, exchangerate, PaysCount, PaysAmount, remain
                            , bill_source_itemsin_cost, bill_source_itemsin_realcost, itemsout_realvalue, real_pays);
                        List.Add(Contact_Sells_ReportDetail_);
                    }
                    return List;
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Get_Contact_Sells_ReportDetail:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            public List<Contact_Buys_ReportDetail> Get_Contact_Buys_ReportDetail(uint ContactID)
            {
                List<Contact_Buys_ReportDetail> List = new List<Contact_Buys_ReportDetail>();
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Contact_Buy_GetReport_Details_Table.Bill_Date + ","
                   + Contact_Buy_GetReport_Details_Table.Bill_ID + ","
                   + Contact_Buy_GetReport_Details_Table.ClauseS_Count + ","
                   + Contact_Buy_GetReport_Details_Table.Amount_IN + ","
                    + Contact_Buy_GetReport_Details_Table.Amount_Remain + ","
                    + Contact_Buy_GetReport_Details_Table.BillValue + ","
                   + Contact_Buy_GetReport_Details_Table.CurrencyID + ","
                   + Contact_Buy_GetReport_Details_Table.ExchangeRate + ","
                   + Contact_Buy_GetReport_Details_Table.PaysAmount + ","
                   + Contact_Buy_GetReport_Details_Table.PaysRemain + ","
                   + Contact_Buy_GetReport_Details_Table.Bill_RealValue + ","
                   + Contact_Buy_GetReport_Details_Table.Bill_Pays_RealValue + ","
                   + Contact_Buy_GetReport_Details_Table.Bill_ItemsOut_Value + ","
                  + Contact_Buy_GetReport_Details_Table.Bill_ItemsOut_RealValue + ","
                  + Contact_Buy_GetReport_Details_Table.Bill_Pays_Return_Value + ","
                   + Contact_Buy_GetReport_Details_Table.Bill_Pays_Return_RealValue
                   + " from "
                   + Contact_Buy_GetReport_Details_Table.TableName
                    + "("
                   +ContactID 
                   + ")"
                   + " order by  "
                   + Contact_Buy_GetReport_Details_Table.Bill_Date
                   );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        DateTime billdate = Convert.ToDateTime(table.Rows[i][0].ToString());
                        int billid = Convert.ToInt32(table.Rows[i][1].ToString());
                        int clause_Count = Convert.ToInt32(table.Rows[i][2].ToString());
                        double amontin = Convert.ToInt32(table.Rows[i][3].ToString());
                        double amountremain = Convert.ToInt32(table.Rows[i][4].ToString());


                        double billvalue = Convert.ToDouble(table.Rows[i][5].ToString());
                        Currency currency = new CurrencySQL(DB)
                            .GetCurrencyINFO_ByID(Convert.ToUInt32(table.Rows[i][6].ToString()));
                        double exchangerate = Convert.ToDouble(table.Rows[i][7].ToString());
                        string PaysAmount = table.Rows[i][8].ToString();
                        double paysremain = Convert.ToDouble(table.Rows[i][9].ToString()); ;
                        double billrealvalue = Convert.ToDouble(table.Rows[i][10].ToString()); ;
                        double bill_Pays_realvalue = Convert.ToDouble(table.Rows[i][11].ToString()); ;

                        string itemsoutvalue = table.Rows[i][12].ToString();
                        double itemsoutRealvalue = Convert.ToDouble(table.Rows[i][13].ToString());

                        string bill_pays_returns_value = table.Rows[i][14].ToString();
                        double bill_pays_returns_realvalue = Convert.ToDouble(table.Rows[i][15].ToString());

                        Contact_Buys_ReportDetail Contact_Buys_ReportDetail_
                            = new Contact_Buys_ReportDetail(billdate, billid, 
                            clause_Count, amontin, amountremain, billvalue, currency, exchangerate
                            , PaysAmount, paysremain, billrealvalue, bill_Pays_realvalue, itemsoutvalue, itemsoutRealvalue
                            , bill_pays_returns_value, bill_pays_returns_realvalue);
                        List.Add(Contact_Buys_ReportDetail_);
                    }
                    return List;
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Get_Contact_Buys_ReportDetail:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return List;
                }
            }
            #endregion
            #region Report
            public static class Contact_Pays_Report_Table
            {

                public const string TableName = "[dbo].[Contact_Pays_GetReport]";
                public const string CurrencyID = "CurrencyID";
                public const string Currency = "Currency";
                public const string PayIN_Sell = "PayIN_Sell";
                public const string PayIN_Maintenance = "PayIN_Maintenance";
                public const string PayOUT_Buy = "PayOUT_Buy";
            }
            public static class Contact_Sells_Report_Table
            {
                
                public const string TableName = "[dbo].[Contact_Sell_GetReport]";
                public const string Bills_Count = "Bills_Count";
                public const string Bills_Value = "Bills_Value";
                public const string Bills_Pays_Value = "Bills_Pays_Value";
                public const string Bills_Pays_Remain = "Bills_Pays_Remain";
                public const string Bills_Pays_Remain_UPON_BillsCurrency = "Bills_Pays_Remain_UPON_BillsCurrency";
                public const string Bills_ItemsIN_Value = "Bills_ItemsIN_Value";
                public const string Bills_ItemsIN_RealValue = "Bills_ItemsIN_RealValue";
                public const string Bills_RealValue = "Bills_RealValue";
                public const string Bills_Pays_RealValue = "Bills_Pays_RealValue";

            }
            public static class Contact_Buys_Report_Table
            {

                public const string TableName = "[dbo].[Contact_Buy_GetReport]";
                public const string Bills_Count = "Bills_Count";
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
            public static class Contact_MaintenanceOPRs_Report_Table
            {

                public const string TableName = "[dbo].[Contact_MaintenanceOPR_GetReport]";

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
            public Contact_MaintenanceOPRs_Report Get_Contact_MaintenanceOPRs_Report(uint ContactID)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
                   + Contact_MaintenanceOPRs_Report_Table.MaintenanceOPRs_Count + ","
                   + Contact_MaintenanceOPRs_Report_Table.MaintenanceOPRs_EndWork_Count + ","
                    + Contact_MaintenanceOPRs_Report_Table.MaintenanceOPRs_Repaired_Count + ","
                    + Contact_MaintenanceOPRs_Report_Table.MaintenanceOPRs_Warranty_Count + ","
                   + Contact_MaintenanceOPRs_Report_Table.MaintenanceOPRs_EndWork_Count + ","
                   + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_Value + ","
                   + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_Pays_Value + ","
                   + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_Pays_Remain + ","
                   + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_Pays_Remain_UPON_MaintenanceOPRsCurrency + ","
                   + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_ItemsOut_Value + ","
                    + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_ItemsOut_RealValue + ","
                    + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_RealValue + ","
                    + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_Pays_RealValue + ","
                     + Contact_MaintenanceOPRs_Report_Table.BillMaintenances_Count
                   + " from "
                   + Contact_MaintenanceOPRs_Report_Table.TableName
                    + "("
                   + ContactID 
                   + ")"
                   );
                    if (table.Rows.Count == 1)
                    {
                        int maintenanceopr_count = Convert.ToInt32(table.Rows[0][0].ToString());
                        int maintenanceopr_endwork_count = Convert.ToInt32(table.Rows[0][1].ToString());
                        int maintenanceopr_repaired_count = Convert.ToInt32(table.Rows[0][2].ToString());
                        int maintenanceopr_warranty_count = Convert.ToInt32(table.Rows[0][3].ToString());
                        int maintenanceopr_ENDwarranty_count = Convert.ToInt32(table.Rows[0][4].ToString());
                        string bills_value = table.Rows[0][5].ToString();
                        string bills_pays_value = table.Rows[0][6].ToString();
                        string bills_pays_remain = table.Rows[0][7].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][8].ToString());
                        string bills_itemsout_value = table.Rows[0][9].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());

                        double bills_realvalue = Convert.ToDouble(table.Rows[0][11].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][12].ToString());
                        int billscount = Convert.ToInt32(table.Rows[0][13].ToString());


                        Contact_MaintenanceOPRs_Report Contact_MaintenanceOPRs_Report_
                            = new Contact_MaintenanceOPRs_Report( maintenanceopr_count, maintenanceopr_endwork_count
                            , maintenanceopr_repaired_count, maintenanceopr_warranty_count, maintenanceopr_ENDwarranty_count, billscount, bills_value, bills_pays_value
                            , bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsout_value, bills_itemsout_realvalue
                            , bills_realvalue, bills_pays_realvalue);
                        return Contact_MaintenanceOPRs_Report_;
                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_Report_MaintenanceOPRs_Report: " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public Contact_Sells_Report Get_Contact_Sells_Report(uint ContactID)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "

                   + Contact_Sells_Report_Table.Bills_Count + ","
                   + Contact_Sells_Report_Table.Bills_Value + ","
                   + Contact_Sells_Report_Table.Bills_Pays_Value + ","
                   + Contact_Sells_Report_Table.Bills_Pays_Remain + ","
                   + Contact_Sells_Report_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Contact_Sells_Report_Table.Bills_ItemsIN_Value + ","
                   + Contact_Sells_Report_Table.Bills_ItemsIN_RealValue + ","
                   + Contact_Sells_Report_Table.Bills_RealValue + ","
                   + Contact_Sells_Report_Table.Bills_Pays_RealValue

                   + " from "
                   + Contact_Sells_Report_Table.TableName
                    + "("
                   +ContactID 
                   + ")"

                   );
                    if (table.Rows.Count == 1)
                    {

                        int bills_count = Convert.ToInt32(table.Rows[0][0].ToString());

                        string bills_value = table.Rows[0][1].ToString();
                        string bills_pays_value = table.Rows[0][2].ToString();
                        string bills_pays_remain = table.Rows[0][3].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][4].ToString());
                        string bills_itemsin_value = table.Rows[0][5].ToString();
                        double bills_itemsin_realvalue = Convert.ToDouble(table.Rows[0][6].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[0][7].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][8].ToString());



                        Contact_Sells_Report Contact_Sells_Report_
                            = new Contact_Sells_Report( bills_count,  bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_itemsin_value, bills_itemsin_realvalue, bills_realvalue, bills_pays_realvalue);
                        return Contact_Sells_Report_;
                    }
                    else return null;
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Get_Contact_Sells_Report:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public Contact_Buys_Report Get_Contact_Buys_Report(uint ContactID)
            {
                try
                {

                    DataTable table = DB.GetData(
                   " select "
 
                   + Contact_Buys_Report_Table.Bills_Count + ","
                    + Contact_Buys_Report_Table.Bills_Amounts_IN + ","
                    + Contact_Buys_Report_Table.Bills_Amounts_Remain + ","
                   + Contact_Buys_Report_Table.Bills_Value + ","
                   + Contact_Buys_Report_Table.Bills_Pays_Value + ","
                   + Contact_Buys_Report_Table.Bills_Pays_Remain + ","
                   + Contact_Buys_Report_Table.Bills_Pays_Remain_UPON_BillsCurrency + ","
                   + Contact_Buys_Report_Table.Bills_RealValue + ","
                   + Contact_Buys_Report_Table.Bills_Pays_RealValue + ","
                    + Contact_Buys_Report_Table.Bills_ItemsOut_Value + ","
                    + Contact_Buys_Report_Table.Bills_ItemsOut_RealValue + ","
                    + Contact_Buys_Report_Table.Bills_Pays_Return_Value + ","
                    + Contact_Buys_Report_Table.Bills_Pays_Return_RealValue

                   + " from "
                   + Contact_Buys_Report_Table.TableName
                    + "("
                   + ContactID 
                   + ")"

                   );
                    if (table.Rows.Count == 1)
                    {

                        int bills_count = Convert.ToInt32(table.Rows[0][0].ToString());
                        int amountin = Convert.ToInt32(table.Rows[0][1].ToString());
                        int amountremain = Convert.ToInt32(table.Rows[0][2].ToString());
                        string bills_value = table.Rows[0][3].ToString();
                        string bills_pays_value = table.Rows[0][4].ToString();
                        string bills_pays_remain = table.Rows[0][5].ToString();
                        double bills_pays_remain_upon_billcurrency = Convert.ToDouble(table.Rows[0][6].ToString());
                        double bills_realvalue = Convert.ToDouble(table.Rows[0][7].ToString());
                        double bills_pays_realvalue = Convert.ToDouble(table.Rows[0][8].ToString());
                        string bills_itemsout_value = table.Rows[0][9].ToString();
                        double bills_itemsout_realvalue = Convert.ToDouble(table.Rows[0][10].ToString());
                        string bills_pays_returns_value = table.Rows[0][11].ToString();
                        double bills_pays_returns_realvalue = Convert.ToDouble(table.Rows[0][12].ToString());



                        Contact_Buys_Report Contact_Buys_Report_
                            = new Contact_Buys_Report( bills_count, amountin, amountremain, bills_value
                            , bills_pays_value, bills_pays_remain, bills_pays_remain_upon_billcurrency, bills_realvalue, bills_pays_realvalue, bills_itemsout_value
                            , bills_itemsout_realvalue, bills_pays_returns_value, bills_pays_returns_realvalue);
                        return Contact_Buys_Report_;
                    }
                    else return null;
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Get_Contact_Buys_Report:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            
            public List<Contact_PayCurrencyReport> Get_Contact_PayCurrencyReport(uint ContactID)
            {
                List<Contact_PayCurrencyReport> Contact_PayCurrencyReportList = new List<Contact_PayCurrencyReport>();
                try
                {

                    DataTable table = DB.GetData(
                  " select "
                  + Contact_Pays_Report_Table.CurrencyID + ","
                  + Contact_Pays_Report_Table.Currency + ","
                  + Contact_Pays_Report_Table.PayIN_Sell + ","
                  + Contact_Pays_Report_Table.PayIN_Maintenance + ","
                  + Contact_Pays_Report_Table.PayOUT_Buy
                  + " from "
                  + Contact_Pays_Report_Table.TableName
                   + "("
                  + ContactID
                  + ")"
                  );
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        uint currencyid = Convert.ToUInt32(table.Rows[i][0].ToString());
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(currencyid);
                        double payin_sell = Convert.ToDouble(table.Rows[i][2].ToString());
                        double payin_mainenance = Convert.ToDouble(table.Rows[i][3].ToString());

                        double payout_buy = Convert.ToDouble(table.Rows[i][4].ToString());

                        Contact_PayCurrencyReport Contact_PayCurrencyReport =
                            new Contact_PayCurrencyReport(Currency_, payin_sell, payin_mainenance
                            , payout_buy);
                        Contact_PayCurrencyReportList.Add(Contact_PayCurrencyReport);
                    }
                    return Contact_PayCurrencyReportList;
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Get_Contact_PayCurrencyReport:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return Contact_PayCurrencyReportList;
                }
            }
            #endregion

        }
        public class SellTypeSql
        {
            public static class SellTypeTable
            {
                public const string TableName = "Trade_SellTypes";
                public const string SellTypeID = "SellTypeID";
                public const string SellTypeName = "SellTypeName";
                public const string Disable = "Disable";
            }
            DatabaseInterface DB;
            public SellTypeSql(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public SellType GetSellTypeinfo(uint selltypeid)
            {

                DataTable t = DB.GetData("select "
                + SellTypeTable.SellTypeID + ","
                + SellTypeTable.SellTypeName
                + " from  "
                + SellTypeTable.TableName
                + " where "
                + SellTypeTable.SellTypeID + "=" + selltypeid);
               
                if (t.Rows.Count == 0) return null;
                else return new SellType(Convert.ToUInt32(t.Rows[0][0].ToString()), t.Rows[0][1].ToString());
            }
            public SellType GetSellTypeinfo(string SellTypeName_)
            {
                DataTable t = DB.GetData("select "
                 + SellTypeTable.SellTypeID + ","
                + SellTypeTable.SellTypeName
                + " from  "
                + SellTypeTable.TableName
                + " where "
            + SellTypeTable.SellTypeName + "='" + SellTypeName_ + "'");
                if (t.Rows.Count == 0) return null;
                else return new SellType(Convert.ToUInt32(t.Rows[0][0].ToString()), t.Rows[0][1].ToString());
            }
            public bool IsSellTypeExists(string SellType_name)
            {
                try
                {
                    DataTable t = DB.GetData("select * from  "
                  + SellTypeTable.TableName
                  + " where "
                  + SellTypeTable.SellTypeName + "='" + SellType_name + "'");
                    if (t.Rows.Count > 0) return true;
                    else return false;
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception("حدث خطأ اثناء الاتصال بقاعدة البيانات", sqlEx);
                }
            }

            public bool AddSellType(string SellTypename)
            {
                try
                {
                    if (IsSellTypeExists(SellTypename))
                    {
                        MessageBox.Show("البيانات موجودة مسبقا", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    DB.ExecuteSQLCommand("insert into  "
                        + SellTypeTable.TableName
                        + " ( "
                        + SellTypeTable.SellTypeName
                        + ")values( "
                        + "'" + SellTypename + "'"
                        + ")"
                        );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.INSERT
                       , DatabaseInterface.Log.Log_Target.Trade_SellTypes
                        , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_SellTypes 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateSellType(uint SellTypeid, string newSellTypename)
            {
                try
                {
                    DB.ExecuteSQLCommand("update   "
                        + SellTypeTable.TableName
                        + " set "
                        + SellTypeTable.SellTypeName + "='" + newSellTypename + "'"
                        + " where "
                        + SellTypeTable.SellTypeID + "=" + SellTypeid
                        );
                    DB.AddLog(
             DatabaseInterface.Log.LogType.UPDATE 
             , DatabaseInterface.Log.Log_Target.Trade_SellTypes
              , ""
              , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_SellTypes
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteSellType(uint SellTypeid)
            {
                try
                {

                    DB.ExecuteSQLCommand("delete from    "
                        + SellTypeTable.TableName
                        + " where "
                        + SellTypeTable.SellTypeID + "=" + SellTypeid
                        );
                    DB.AddLog(
             DatabaseInterface.Log.LogType.DELETE 
             , DatabaseInterface.Log.Log_Target.Trade_SellTypes
              , ""
              , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_SellTypes
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<SellType> GetSellTypeList()
            {
                try
                {
                    List<SellType> list = new List<SellType>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select * from "
                        + SellTypeTable.TableName
                       );


                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        SellType m = new SellType(Convert.ToUInt32(t.Rows[i][0]), t.Rows[i][1].ToString());
                        list.Add(m);
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب قائمة علاقات العناصر:", ee.Message);
                    return null;
                }

            }
        }
        public class TradeStateSQL
        {
            DatabaseInterface DB;
            private static class TradeStateTable
            {
                public const string TableName = "Trade_TradeState";
                public const string TradeStateID = "TradeStateID";
                public const string TradeStateName = "TradeStateName";


            }
            public TradeStateSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public TradeState GetTradeStateBYID(uint tradestateid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select * from   "
                + TradeStateTable.TableName
                + " where "
                + TradeStateTable.TradeStateID  + "=" + tradestateid
                  );
                if (t.Rows.Count == 1)
                {
                    string tradestatename = t.Rows[0][1].ToString();


                    return new TradeState (tradestateid , tradestatename);

                }
                else
                    return null;
            }
            public bool AddTradeState(string tradestatename)
            {
                try
                {
                    DB.ExecuteSQLCommand(" insert into "
                    + TradeStateTable .TableName
                    + "("
                    + TradeStateTable.TradeStateName
                    + ")"
                    + "values"
                    + "("
                    + "'" + tradestatename   + "'"

                    + ")"
                    );
                    DB.AddLog(
              DatabaseInterface.Log.LogType.INSERT
              , DatabaseInterface.Log.Log_Target.Item_TradeState
               , ""
               , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Item_TradeState
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateTradeState(uint tradestateidid, string newname)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + TradeStateTable .TableName
                    + " set "
                       + TradeStateTable.TradeStateName  + "='" + newname  + "'"
                    + " where "
                    + TradeStateTable.TradeStateID + "=" + tradestateidid
                    );
                    
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.UPDATE 
                          , DatabaseInterface.Log.Log_Target.Item_TradeState
                           , ""
                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Item_TradeState
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteTradestate(uint tradestateidid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + TradeStateTable.TableName
                    + " where "
                    + TradeStateTable.TradeStateID + "=" + tradestateidid
                     );
                    DB.AddLog(
              DatabaseInterface.Log.LogType.DELETE 
              , DatabaseInterface.Log.Log_Target.Item_TradeState
               , ""
               , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Item_TradeState
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<TradeState > GetTradeStateList()
            {
                try
                {
                    List<TradeState> tradestatelist = new List<TradeState>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select * from   "
                    + TradeStateTable.TableName
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint ownerrid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string ownername = t.Rows[i][1].ToString();


                        tradestatelist.Add(new TradeState(ownerrid, ownername));
                    }
                    return tradestatelist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("", "" + ee.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        public class ItemINSQL
        {
            DatabaseInterface DB;
            internal  static class ItemINTable
            {
                public const string TableName = "Trade_ItemIN";
                public const string ItemINID = "ItemINID";
                public const string OperationType = "OperationType";
                public const string OperationID = "OperationID";
                public const string ItemID = "ItemID";
                public const string TradeStateID = "TradeStateID";
                public const string Amount = "Amount";
                public const string ConsumeUnitID = "ConsumeUnitID";
                public const string Cost = "Cost";
                public const string Notes = "Notes";

            }
            private static class INCost_Table
            {
                public const string TableName = "[dbo].[Trade_ItemIN_GetCost]";
                public const string Value = "Value_";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
            }
            public ItemINSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public ItemIN  GetItemININFO_BYID(uint iteminid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + ItemINTable.OperationType + ","
                     + ItemINTable.OperationID  + ","
                     + ItemINTable.ItemID + ","
                     + ItemINTable.TradeStateID + ","
                     + ItemINTable.Amount + ","
                     + ItemINTable.ConsumeUnitID+","
                     //+ ItemINTable.Cost  + ","
                     + ItemINTable.Notes 

                     + " from   "
                    + ItemINTable.TableName
                    + " where "
                    + ItemINTable.ItemINID + "=" + iteminid
                      );
                    if (t.Rows.Count == 1)
                    {
                        uint operationtype = Convert.ToUInt32(t.Rows [0][0].ToString ());
                        uint operationid = Convert.ToUInt32(t.Rows[0][1].ToString());
                        Item item = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[0][2].ToString()));
                        TradeState tradestate = new TradeStateSQL(DB).GetTradeStateBYID(Convert.ToUInt32(t.Rows[0][3].ToString()));
                        double amount = Convert.ToDouble(t.Rows[0][4].ToString());
                        ConsumeUnit consumeunit;
                        try
                        {
                            uint consumeunitid = Convert.ToUInt32(t.Rows[0][5].ToString());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunitid);

                        }
                        catch
                        {
                            consumeunit = new ConsumeUnit(0, item.DefaultConsumeUnit, item, 1);
                        }
                        //double buyprice = Convert.ToDouble(t.Rows[0][6].ToString());
                        string notes = t.Rows[0][6].ToString();

                        INCost INCost_ = GetItemINCost(iteminid);
                        return new ItemIN (iteminid ,new Operation ( operationtype ,operationid ), item, tradestate, amount, consumeunit, INCost_, notes);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetItemININFO_BYID" + ee.Message ,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return null;
                }
                
            }
            public ItemIN AddItemIN(Operation Operation_, Item item,TradeState tradestate,double  amount,ConsumeUnit consumeunit,double  cost,string notes)
            {
                try
                {
                   
                    DataTable t= DB.GetData (" insert into "
                    + ItemINTable.TableName
                    + "("
                    + ItemINTable.OperationType   +","
                     + ItemINTable.OperationID  + ","
                    + ItemINTable.ItemID + ","
                    + ItemINTable.TradeStateID + ","
                    + ItemINTable.Amount  + ","
                    + ItemINTable.ConsumeUnitID +","
                    + ItemINTable.Cost  + ","
                    + ItemINTable.Notes 
                    + ")"
                    + "values"
                    + "("
                    + Operation_.OperationType 
                    + ","
                     + Operation_.OperationID
                    + ","
                    + item.ItemID 
                     + ","
                    + tradestate .TradeStateID
                     + ","
                      + amount 
                     + ","
                     +(consumeunit == null ? "null" : consumeunit.ConsumeUnitID.ToString())
                     + ","
                      + cost   
                     + ","
                     + "'"+notes +"'"
                    + ")"
                     + " SELECT SCOPE_IDENTITY() "
                    );

                    uint itemin = Convert.ToUInt32(t.Rows[0][0].ToString());

                   

                    DB.AddLog(
              DatabaseInterface.Log.LogType.INSERT
              , DatabaseInterface.Log.Log_Target.Trade_ItemIN 
               , ""
               , true, "");
                    return GetItemININFO_BYID(itemin);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_ItemIN 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateItemIN(uint iteminid,Item item,TradeState tradestate, double  amount,ConsumeUnit consumeunit, double cost,  string notes)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + ItemINTable.TableName
                    + " set "
                     + ItemINTable.ItemID + "=" + item.ItemID 
                    + ","
                     + ItemINTable.TradeStateID  + "=" + tradestate .TradeStateID 
                    + ","
                    + ItemINTable.Amount  + "=" +amount 
                    + ","
                    + ItemINTable.ConsumeUnitID  + "=" + (consumeunit == null ? "null" : consumeunit.ConsumeUnitID.ToString())
                    + ","
                    + ItemINTable.Cost   + "=" +cost  
                    + ","
                    + ItemINTable.Notes   + "='" + notes  +"'"
                    + " where "
                    + ItemINTable.ItemINID +"="+ iteminid 
                    );
                    DB.AddLog(
            DatabaseInterface.Log.LogType.UPDATE 
            , DatabaseInterface.Log.Log_Target.Trade_ItemIN
             , ""
             , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemIN
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteItemIN(uint iteminid)
            {
                try
                {
                    ItemIN itemin = GetItemININFO_BYID(iteminid);
                    List<ItemOUT> itemoutlist =new ItemOUTSQL (DB). GetItemIN_ItemOUTList(itemin);
                    if(itemoutlist .Count >0)
                    {
                        throw new Exception("لا يمكن حذف عملية الادخال , يجب اولا الغاء عمليات الاخراج اللتي مصدرها عملية الادخال"+iteminid );
                    }
                    List <TradeItemStore > placeslist= new TradeItemStoreSQL(DB).GetItemStoredPlaces(iteminid);
                    if (placeslist.Count > 0)
                    {
                        throw new Exception("يجب اولا الغاء  عمليات التخزين التابعة لعملية الادخال" + iteminid);
                    }
                    DB.ExecuteSQLCommand("delete from   "
                    + ItemINTable.TableName
                    + " where "
                    + ItemINTable.ItemINID + "=" + iteminid
                    );
                    DB.AddLog(
           DatabaseInterface.Log.LogType.DELETE 
           , DatabaseInterface.Log.Log_Target.Trade_ItemIN
            , ""
            , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemIN
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Does_Operation_Has_ItemsIN(uint oprtype,uint oprid)
            {
                try
                {
                    DataTable t= DB.GetData ("select * from   "
                    + ItemINTable.TableName
                    + " where "
                    + ItemINTable.OperationType + "=" + oprtype
                    + " and "
                    + ItemINTable.OperationID + "=" + oprid 
                    );
                    if (t.Rows.Count > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("DeleteItemINListForOperation" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<ItemIN_ItemOUTReport> GetItemIN_ItemOUTReport_List(Operation operation)
            {
                List<ItemIN_ItemOUTReport> ItemIN_ItemOUTReportList = new List<ItemIN_ItemOUTReport>();
                try
                {
                    List<ItemIN> ItemINList = GetItemINList(operation);
                    ItemOUTSQL ItemOUTSQL_ = new ItemOUTSQL(DB);
                    for (int i = 0; i < ItemINList.Count; i++)
                    {
                        List<ItemOUT> ItemOUTList = ItemOUTSQL_.GetItemIN_ItemOUTList(ItemINList[i]);
                        ItemIN_ItemOUTReportList.Add(new ItemIN_ItemOUTReport(ItemINList[i], ItemOUTList));
                    }
                }
                catch
                {

                }
                return ItemIN_ItemOUTReportList;
            }
            private INCost GetItemINCost(uint iteminid)
            {
                try
                {
                    DataTable t = DB.GetData("select "
                   +INCost_Table.Value +","
                   + INCost_Table.CurrencyID  + ","
                   + INCost_Table.ExchangeRate  
                    + " from   "
                    + INCost_Table.TableName
                    +"("
                    +iteminid
                    +")"
                   
                    );
                    if (t.Rows.Count == 1)
                    {
                        double value = Convert.ToDouble(t.Rows [0][0].ToString ());
                        Currency _Currency =new CurrencySQL (DB).GetCurrencyINFO_ByID ( Convert.ToUInt32 (t.Rows[0][1].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[0][2].ToString());
                        return new INCost(value, _Currency, exchangerate);
                    }
                    else
                        throw new Exception("فشل جلب نكلفة المادة");
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItemINCost" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                     return new INCost(-1, new CurrencySQL (DB).GetReferenceCurrency (), 1); ;
                }
            }
            public List<ItemIN  > GetItemINList(Operation operation)
            {
                try
                {
                    List<ItemIN> ItemINlist = new List<ItemIN>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + ItemINTable.ItemINID   + ","
                    + ItemINTable.ItemID + ","
                    + ItemINTable.TradeStateID + ","
                    + ItemINTable.Amount + ","
                    + ItemINTable.ConsumeUnitID + ","
                    //+ ItemINTable.Cost  + ","
                    + ItemINTable.Notes 

                    + " from   "
                    + ItemINTable.TableName
                    +" where "
                    + ItemINTable.OperationType + "=" + operation.OperationType 
                    + " and "
                    + ItemINTable.OperationID  + "=" + operation.OperationID 

                );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint iteminid  = Convert.ToUInt32(t.Rows[i][0].ToString());
                        Item item = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][1].ToString()));
                        TradeState tradestate = new TradeStateSQL(DB).GetTradeStateBYID(Convert.ToUInt32(t.Rows[i][2].ToString()));
                        double  amount = Convert.ToDouble(t.Rows[i][3].ToString());
                        ConsumeUnit consumeunit;
                        try
                        {
                            uint consumeunitid = Convert.ToUInt32(t.Rows[i][4].ToString());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunitid);

                        }
                        catch
                        {
                            consumeunit = new ConsumeUnit(0, item.DefaultConsumeUnit, item, 1);
                        }
                        //double cost = Convert.ToDouble(t.Rows[i][5].ToString());
                         string notes = t.Rows[i][5].ToString();
                        INCost INCost_ = GetItemINCost(iteminid);
                        ItemINlist.Add(new ItemIN(iteminid ,operation , item, tradestate, amount, consumeunit, INCost_, notes));

                    }
                    return ItemINlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItemINList" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }

            internal List<ItemIN_StoreReport> GetItemIN_StoreReportList(ItemIN itemIN_)
            {
                List<ItemIN_StoreReport> List = new List<ItemIN_StoreReport>();
                try
                {
                    AvailableItemSQL AvailableItemSQL_ = new AvailableItemSQL(DB);

                    double nonsotred = new TradeItemStoreSQL(DB).getNON_StoredAmount(itemIN_);

                    if (nonsotred > 0)
                    {
                        List.Add(new ItemIN_StoreReport(itemIN_, null, itemIN_._ConsumeUnit, nonsotred, AvailableItemSQL_.GetSpentAmount_by_Place(itemIN_, null)));
                    }
            
                    List<TradeItemStore> TradeItemStoreList  = new TradeItemStoreSQL(DB).GetItemStoredPlaces(itemIN_.ItemINID);
                
                    for (int i = 0; i < TradeItemStoreList.Count; i++)
                    {
                        List.Add(new ItemIN_StoreReport(itemIN_, TradeItemStoreList[i]._TradeStorePlace, TradeItemStoreList[i]._ConsumeUnit, TradeItemStoreList[i].Amount, AvailableItemSQL_.GetSpentAmount_by_Place(itemIN_, TradeItemStoreList[i]._TradeStorePlace)));
                    }
                }
                catch(Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("ITEMINSQL-GetItemIN_StoreReportList:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }
                return List;
            }
        }
        public class ItemOUTSQL
        {
            DatabaseInterface DB;
            internal static class ItemOUTTable
            {
                public const string TableName = "Trade_ItemOUT";
                public const string ItemOUTID = "ItemOUTID";
                public const string OperationType = "OperationType";
                public const string OperationID = "OperationID";
                public const string ItemINID = "ItemINID";
                public const string PlaceID = "PlaceID";
                public const string Amount = "Amount";
                public const string ConsumeUnitID = "ConsumeUnitID";
                public const string Cost = "Cost";
                public const string Notes = "Notes";

            }
            private static class OUTValue_Table
            {
                public const string TableName = "[dbo].[Trade_ItemOUT_GetOutValue]";
                public const string Value = "Value_";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
            }
            public ItemOUTSQL(DatabaseInterface db)
            {
                DB = db;

            }
            private OUTValue  GetOUTValue(uint itemoutid)
            {
                try
                {
                    DataTable t = DB.GetData("select "
                   + OUTValue_Table.Value + ","
                   + OUTValue_Table.CurrencyID + ","
                   + OUTValue_Table.ExchangeRate
                    + " from   "
                    + OUTValue_Table.TableName
                    + "("
                    + itemoutid
                    + ")"

                    );
                    if (t.Rows.Count == 1)
                    {
                        double value = Convert.ToDouble(t.Rows[0][0].ToString());
                        Currency _Currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[0][2].ToString());
                        return new OUTValue (value, _Currency, exchangerate);
                    }
                    else
                        throw new Exception("فشل جلب نكلفة المادة");
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItemINCost" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return new OUTValue (-1, new CurrencySQL(DB).GetReferenceCurrency(), 1); ;
                }
            }
            public ItemOUT GetItemOUTINFO_BYID(uint itemoutid)
            {
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + ItemOUTTable.OperationType + ","
                     + ItemOUTTable.OperationID + ","
                     + ItemOUTTable.ItemINID + ","
                     + ItemOUTTable.PlaceID + ","
                     + ItemOUTTable.Amount + ","
                     + ItemOUTTable.ConsumeUnitID + ","
                     //+ ItemOUTTable.Cost + ","
                     + ItemOUTTable.Notes
                     + " from   "
                    + ItemOUTTable.TableName
                    + " where "
                    + ItemOUTTable.ItemOUTID + "=" + itemoutid
                      );
                    if (t.Rows.Count == 1)
                    {

                        uint operationtype = Convert.ToUInt32(t.Rows[0][0].ToString());
                        uint operationid = Convert.ToUInt32(t.Rows[0][1].ToString());
                        ItemIN ItemIN_ = new ItemINSQL(DB).GetItemININFO_BYID(Convert.ToUInt32(t.Rows[0][2].ToString()));
                        TradeStorePlace Place;
                        try
                        {
                            Place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(t.Rows[0][3].ToString()));
                        }
                        catch
                        {
                            Place = null;
                        }
                        double amount = Convert.ToInt32(t.Rows[0][4].ToString());
                        ConsumeUnit consumeunit;
                        try
                        {
                            uint consumeunitid = Convert.ToUInt32(t.Rows[0][5].ToString());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunitid);

                        }
                        catch
                        {
                            consumeunit = new ConsumeUnit(0, ItemIN_._Item.DefaultConsumeUnit, ItemIN_._Item, 1);

                        }

                        //double Cost = Convert.ToDouble(t.Rows[0][6].ToString());
                        string notes = t.Rows[0][6].ToString();
                        OUTValue OUTValue_ = GetOUTValue(itemoutid);
                        return new ItemOUT(itemoutid, new Operation ( operationtype, operationid), ItemIN_, Place, amount, consumeunit, OUTValue_, notes);


                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItemOUTINFO_BYID" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }
            public ItemOUT AddItemOUT(Operation Operation_, uint iteminid, TradeStorePlace Place, double amount, ConsumeUnit consumeunit, double? cost, string notes)
            {
                try
                {
                    
                    DataTable t = DB.GetData(" insert into "
                    + ItemOUTTable.TableName
                    + "("
                    + ItemOUTTable.OperationType + ","
                    + ItemOUTTable.OperationID + ","
                    + ItemOUTTable.ItemINID + ","
                     + ItemOUTTable.PlaceID + ","
                    + ItemOUTTable.Amount + ","
                    + ItemOUTTable.ConsumeUnitID + ","
                    + ItemOUTTable.Cost + ","
                    + ItemOUTTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + Operation_.OperationType 
                    + ","
                    + Operation_.OperationID 
                    + ","
                    + iteminid
                    + ","
                    + (Place == null ? "null" : Place.PlaceID.ToString())
                    + ","
                    + amount
                    + ","
                    + (consumeunit == null ? "null" : consumeunit.ConsumeUnitID.ToString())
                    + ","
                    + (cost  == null ? "null" : cost.ToString())
                    + ","
                    + "'" + notes + "'"
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );

                    uint itemoutid = Convert.ToUInt32(t.Rows[0][0].ToString());

                    
                    DB.AddLog(
                           DatabaseInterface.Log.LogType.INSERT 
                           , DatabaseInterface.Log.Log_Target.Trade_ItemOut 
                            , ""
                            , true, "");
                    return GetItemOUTINFO_BYID(itemoutid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemOut 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateItemOUT(uint itemoutidid, uint iteminid, TradeStorePlace Place, double amount, ConsumeUnit ConsumeUnit_, double? cost, string notes)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + ItemOUTTable.TableName
                    + " set "
                    + ItemOUTTable.ItemINID + "=" + iteminid
                    + ","
                    + ItemOUTTable.PlaceID + "=" + (Place == null ? "null" : Place.PlaceID.ToString())
                    + ","
                    + ItemOUTTable.Amount + "=" + amount
                    + ","
                    + ItemOUTTable.ConsumeUnitID + "=" + (ConsumeUnit_ == null ? "null" : ConsumeUnit_.ConsumeUnitID.ToString())
                    + ","
                    + ItemOUTTable.Cost + "=" + (cost == null ? "null" : cost.ToString())
                    + ","
                    + ItemOUTTable.Notes + "='" + notes + "'"
                    + " where "
                    + ItemOUTTable.ItemOUTID + "=" + itemoutidid
                    );
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.UPDATE 
                          , DatabaseInterface.Log.Log_Target.Trade_ItemOut
                           , ""
                           , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemOut
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteItemOUT(uint itemoutid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + ItemOUTTable.TableName
                    + " where "
                    + ItemOUTTable.ItemOUTID + "=" + itemoutid
                    );
                    DB.AddLog(
      DatabaseInterface.Log.LogType.DELETE 
      , DatabaseInterface.Log.Log_Target.Trade_ItemOut
       , ""
       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemOut
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Does_Operation_Has_ItemsOUT(uint oprtype, uint oprid)
            {
                try
                {
                    DataTable t = DB.GetData("select * from   "
                    + ItemOUTTable.TableName
                    + " where "
                    + ItemOUTTable.OperationType + "=" + oprtype
                    + " and "
                    + ItemOUTTable.OperationID + "=" + oprid
                    );
                    if (t.Rows.Count > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("DeleteItemINListForOperation" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<ItemOUT> GetItemOUTList(Operation operation)
            {
                try
                {
                    List<ItemOUT> ItemOUTList = new List<ItemOUT>();

                    DataTable t = new DataTable();
             
                    t = DB.GetData("select "
                     + ItemOUTTable.ItemOUTID + ","
                     + ItemOUTTable.ItemINID + ","
                     + ItemOUTTable.PlaceID + ","
                     + ItemOUTTable.Amount + ","
                     + ItemOUTTable.ConsumeUnitID + ","
                     //+ ItemOUTTable.Cost + ","
                     + ItemOUTTable.Notes
                     + " from   "
                    + ItemOUTTable.TableName
                    + " where "
                    + ItemOUTTable.OperationType + "=" + operation.OperationType 
                    + " and "
                    + ItemOUTTable.OperationID + "=" + operation.OperationID 
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint itemoutid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        ItemIN itemin = new ItemINSQL(DB).GetItemININFO_BYID(Convert.ToUInt32(t.Rows[i][1].ToString()));
                        TradeStorePlace Place;
                        try
                        {
                            Place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(t.Rows[i][2].ToString()));
                        }
                        catch
                        {
                            Place = null;
                        }

                        double amount = Convert.ToDouble(t.Rows[i][3].ToString());

                        ConsumeUnit consumeunit;
                        try
                        {
                            uint consumeunitid = Convert.ToUInt32(t.Rows[i][4].ToString());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunitid);

                        }
                        catch
                        {
                            consumeunit = new ConsumeUnit(0, itemin._Item.DefaultConsumeUnit, itemin._Item, 1);

                        }

                        //double cost = Convert.ToDouble(t.Rows[i][5].ToString());
                        string notes = t.Rows[i][5].ToString();
                        OUTValue OUTValue_ = GetOUTValue(itemoutid);
                        ItemOUTList.Add(new ItemOUT(itemoutid, operation, itemin, Place, amount, consumeunit, OUTValue_ , notes));

                    }
                    return ItemOUTList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItemOUTList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public int GetItemsOUT_Count(Operation operation)
            {
                try
                {
                    DataTable t = new DataTable();

                    t = DB.GetData("select  count(*) from "
                    + ItemOUTTable.TableName
                    + " where "
                    + ItemOUTTable.OperationType + "=" + operation.OperationType
                    + " and "
                    + ItemOUTTable.OperationID + "=" + operation.OperationID
                      );
                    return Convert.ToInt32(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItemsOUT_Count" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }

            }
            public List<ItemOUT> GetItemIN_ItemOUTList(ItemIN itemin)
            {
                try
                {
                    List<ItemOUT> ItemOUTList = new List<ItemOUT>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + ItemOUTTable.ItemOUTID + ","
                     + ItemOUTTable.OperationType + ","
                      + ItemOUTTable.OperationID + ","
                     + ItemOUTTable.PlaceID + ","
                     + ItemOUTTable.Amount + ","
                     + ItemOUTTable.ConsumeUnitID + ","
                     + ItemOUTTable.Cost + ","
                     + ItemOUTTable.Notes
                     + " from   "
                    + ItemOUTTable.TableName
                    + " where "
                    + ItemOUTTable.ItemINID + "=" + itemin.ItemINID
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint itemoutid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        uint operationtype = Convert.ToUInt32(t.Rows[i][1].ToString());
                        uint operationid = Convert.ToUInt32(t.Rows[i][2].ToString());

                        TradeStorePlace Place;
                        try
                        {
                            Place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(t.Rows[i][3].ToString()));
                        }
                        catch
                        {
                            Place = null;
                        }

                        double amount = Convert.ToDouble(t.Rows[i][4].ToString());

                        ConsumeUnit consumeunit;
                        try
                        {
                            uint consumeunitid = Convert.ToUInt32(t.Rows[i][5].ToString());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunitid);

                        }
                        catch
                        {
                            consumeunit = new ConsumeUnit(0, itemin._Item.DefaultConsumeUnit, itemin._Item, 1);

                        }

                        //double cost = Convert.ToDouble(t.Rows[i][6].ToString());
                        string notes = t.Rows[i][6].ToString();
                        OUTValue OUTValue_ = GetOUTValue(itemoutid);
                        ItemOUTList.Add(new ItemOUT(itemoutid,new Operation ( operationtype, operationid), itemin, Place, amount, consumeunit, OUTValue_ , notes));

                    }
                    return ItemOUTList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItemOUTList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        public class ItemINSellPriceSql
        {
            public static class BuyOPRSellPriceTable
            {
                public const string TableName = "Trade_ItemIN_SellPrices";
                public const string SellPriceID = "SellPriceID";
                public const string ItemINID = "ItemINID";
                public const string ConsumeUnitID = "ConsumeUnitID";
                public const string SellTypeID = "SellTypeID";
                public const string Price = "Price";
            }
            DatabaseInterface DB;
            public ItemINSellPriceSql(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public bool IsPriceSet(ItemIN ItemIN_,  ConsumeUnit ConsumeUnit_, SellType SellType_)
            {
                try
                {
                    string cid_string = " is null";
                    if (ConsumeUnit_ != null)
                        if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = "=" + ConsumeUnit_.ConsumeUnitID.ToString();

                    DataTable t = DB.GetData("select * from   "
                        + BuyOPRSellPriceTable.TableName
                        + " where "
                       + BuyOPRSellPriceTable.ItemINID + "=" + ItemIN_.ItemINID   + " and "
                        + BuyOPRSellPriceTable.ConsumeUnitID + cid_string + " and "
                        + BuyOPRSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID

                        );
                    if (t.Rows.Count > 0)
                        return true;
                    else return false;

                }
                catch (Exception ee)
                {
                    throw new Exception("IsPriceSet :" + ee.Message);

                }

            }
            public bool SetItemINPrice(ItemIN ItemIN_,  ConsumeUnit ConsumeUnit_, SellType SellType_, double price)
            {
                bool is_price_set = false;
                try
                {
                    is_price_set = IsPriceSet(ItemIN_, ConsumeUnit_, SellType_);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل ضبط التسعير:" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (!is_price_set)
                {
                    string cid_string = "  null";
                    if (ConsumeUnit_ != null)
                        if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = ConsumeUnit_.ConsumeUnitID.ToString();
                    try
                    {


                        DB.ExecuteSQLCommand("insert into  "
                            + BuyOPRSellPriceTable.TableName
                            + " ( "
                            + BuyOPRSellPriceTable.ItemINID + ","
                             + BuyOPRSellPriceTable.ConsumeUnitID + ","
                              + BuyOPRSellPriceTable.SellTypeID + ","
                            + BuyOPRSellPriceTable.Price
                            + ")values( "
                            + ItemIN_.ItemINID + ","
                            + cid_string + ","
                            + SellType_.SellTypeID + ","
                            + price
                            + ")"
                            );
                        DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Trade_ItemIN_SellPrice
                       , ""
                       , true, "");
                        return true;
                    }
                    catch (Exception ee)
                    {
                        DB.AddLog(
                                DatabaseInterface.Log.LogType.INSERT 
                                , DatabaseInterface.Log.Log_Target.Trade_ItemIN_SellPrice
                                , ""
                              , false, ee.Message);
                        System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {

                    string cid_string = " is null";
                    if (ConsumeUnit_ != null)
                        if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = "=" + ConsumeUnit_.ConsumeUnitID.ToString();
                    try
                    {
                        DB.ExecuteSQLCommand("update  "
                            + BuyOPRSellPriceTable.TableName
                            + " set "
                            + BuyOPRSellPriceTable.Price + "=" + price
                            + " where "
                            + BuyOPRSellPriceTable.ItemINID + "=" + ItemIN_.ItemINID
                            + " and "
                             + BuyOPRSellPriceTable.ConsumeUnitID + cid_string 
                             + " and "
                              + BuyOPRSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID
                            );
                        DB.AddLog(
                    DatabaseInterface.Log.LogType.UPDATE 
                    , DatabaseInterface.Log.Log_Target.Trade_ItemIN_SellPrice
                     , ""
                     , true, "");
                        return true;
                    }
                    catch (Exception ee)
                    {
                        DB.AddLog(
                                DatabaseInterface.Log.LogType.UPDATE 
                                , DatabaseInterface.Log.Log_Target.Trade_ItemIN_SellPrice
                                , ""
                              , false, ee.Message);
                        System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return false;
                    }
                }

            }

            public bool UNSetBuyOPRPrice(ItemIN ItemIN_, ConsumeUnit ConsumeUnit_, SellType SellType_)
            {
                string cid_string = " is null";
                if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = "=" + ConsumeUnit_.ConsumeUnitID.ToString();
                try
                {

                    DB.ExecuteSQLCommand("delete from   "
                        + BuyOPRSellPriceTable.TableName
                        + " where "
                       + BuyOPRSellPriceTable.ItemINID + "=" + ItemIN_.ItemINID + " and "
                        + BuyOPRSellPriceTable.ConsumeUnitID + cid_string + " and "
                        + BuyOPRSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID

                        );
                    DB.AddLog(
                   DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Trade_ItemIN_SellPrice
                    , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemIN_SellPrice
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public double? GetPrice(ItemIN ItemIN_, SellType SellType_, ConsumeUnit ConsumeUnit_)
            {
                try
                {

                    string cid_string = " is null";
                    if (ConsumeUnit_ != null)
                        if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = "=" + ConsumeUnit_.ConsumeUnitID.ToString();
                    double? price;

                    DataTable t = DB.GetData("select "
                          + BuyOPRSellPriceTable.Price
                        + " from "
                        + BuyOPRSellPriceTable.TableName
                         + " where "
                        + BuyOPRSellPriceTable.ItemINID + "=" + ItemIN_.ItemINID
                        + " and "
                         + BuyOPRSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID
                        + " and "
                         + BuyOPRSellPriceTable.ConsumeUnitID + cid_string);

                    if (t.Rows.Count == 1)
                    {
                        price = Convert.ToDouble(t.Rows[0][0]);
                    }
                    else price = null;
                    return price;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message); return null;
                }
            }
            public List<ItemINSellPrice> GetItemINPrices(ItemIN ItemIN_)
            {
                try
                {
                    List<ItemINSellPrice> list = new List<ItemINSellPrice>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + BuyOPRSellPriceTable.ConsumeUnitID + ","
                           + BuyOPRSellPriceTable.SellTypeID + ","
                          + BuyOPRSellPriceTable.Price
                        + " from "
                        + BuyOPRSellPriceTable.TableName
                         + " where "
                        + BuyOPRSellPriceTable.ItemINID + "=" + ItemIN_.ItemINID

                       );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        ConsumeUnit CU;
                        try
                        {
                            CU = new ConsumeUnitSql(DB).GetConsumeAmountinfo(Convert.ToUInt32(t.Rows[i][0].ToString()));
                        }
                        catch
                        {
                            CU = new ConsumeUnit(0, ItemIN_._Item.DefaultConsumeUnit, ItemIN_._Item, 1);
                        }
                        ItemINSellPrice m = new ItemINSellPrice(ItemIN_, CU, new SellTypeSql(DB).GetSellTypeinfo(Convert.ToUInt32(t.Rows[i][1].ToString())), Convert.ToDouble(t.Rows[i][2].ToString()));
                        list.Add(m);
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب اسعار العنصر:", ee.Message);
                    return null;
                }

            }
          
        }
        public class AvailableItemSQL
        {
            public const string GET_AVAILABLE_AMOUNT_BYPLACE_FUNCTION = "[dbo].[Trade_GetAvailableAmount_ByPlace]";
            public const string GET_SPENT_AMOUNT_BYPLACE_FUNCTION = "[dbo].[Trade_GetSpentAmount_ByPlace]";

            public const string GET_AVAILABLE_AMOUNT_BYITEMIN_FUNCTION = "[dbo].[Trade_GetAvailableAmount_ByItemIN]";
            public const string GET_SPENT_AMOUNT_BYITEMIN_FUNCTION = "[dbo].[Trade_GetSpentAmount_ByItemIN]";


            public static class PlaceAvailabeItem_ItemDetailsTable
            {
                public const string TableName = "[dbo].[Trade_GetAvailableItemsInPlace_ItemDetails]";
                public const string ItemID = "ItemID";
                public const string Itemname = "Itemname";
                public const string ItemCompany = "ItemCompany";
                public const string ItemFolder = "ItemFolder";
                public const string AvailableItemStates = "AvailableItemStates";
            }
            public static class PlaceAvailabeItem_ItemINDetailsTable
            {
                public const string TableName = "[dbo].[Trade_GetAvailableItemsInPlace_ItemINDetails]";
                public const string ItemSourceType = "ItemSourceType";
                public const string StoreType = "StoreType";
                public const string OprID = "OprID";
                public const string ParentOperationDesc = "ParentOperationDesc";
                public const string ParentOperationID = "ParentOperationID";
                public const string consumeunitname = "consumeunitname";
                public const string StoredAmount = "StoredAmount";
                public const string SpentAmount = "SpentAmount";
                public const string AvailableAmount = "AvailableAmount";
            }
            public static class AvailabeItem_ItemINDetailsTable
            {
                public const string TableName = "[dbo].[Trade_GetItemINList_ForAvailableItem]";
                public const string TradeStateName = "TradeStateName";
                public const string ItemINID = "ItemINID";
                public const string OperationType = "OperationType";
                public const string OperationID = "OperationID";
                public const string consumeunitname = "consumeunitname";
                public const string Amount = "Amount";
                public const string SpentAmount = "SpentAmount";
                public const string AvailableAmount = "AvailableAmount";
            }
            DatabaseInterface DB;
            public AvailableItemSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public double GetAvailabeAmount_by_Place(ItemIN ItemIN_, TradeStorePlace place)
            {
                try
                {

                    DataTable t = DB.GetData(
                        "select  "
                        + GET_AVAILABLE_AMOUNT_BYPLACE_FUNCTION
                        + "("
                        + ItemIN_.ItemINID .ToString()+","
                        + (place == null ? "null" : place.PlaceID.ToString())
                        + ")"
                        );
                    return Convert.ToDouble(t.Rows[0][0].ToString());
                }
                catch(Exception ee)
                {
                    MessageBox.Show(":فشل جلب الكمية المتوفرة" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;

                }


            }
            public double GetSpentAmount_by_Place(ItemIN ItemIN_, TradeStorePlace place)
            {
                try
                {

                    //DataTable t = DB.GetData(
                    //    "select  "
                    //    + GET_SPENT_AMOUNT_BYPLACE_FUNCTION
                    //    + "("
                    //    + ItemIN_.ItemINID.ToString() + ","
                    //    + (place == null ? "null" : place.PlaceID.ToString())
                    //    + ")"
                    //    );

                    //return Convert.ToDouble(t.Rows[0][0].ToString());
                    return 0;

                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetSpentAmount_by_Place:" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;

                }


            }
            public double GetAvailabeAmount_by_ItemIN(ItemIN ItemIN_)
            {
                try
                {

                    DataTable t = DB.GetData(
                        "select  "
                        + GET_AVAILABLE_AMOUNT_BYITEMIN_FUNCTION
                        + "("
                        + ItemIN_.ItemINID.ToString() 
                        + ")"
                        );
                    return Convert.ToDouble(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":فشل جلب الكمية المتوفرة" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;

                }


            }
            public List<PlaceAvailbeItems_ItemDetails> GetStoredItems(uint placeid)
            {
                List<PlaceAvailbeItems_ItemDetails> itemslist = new List<PlaceAvailbeItems_ItemDetails>();
                try
                {
                
                   
                    DataTable t = DB.GetData(
                        "select  "
                        + PlaceAvailabeItem_ItemDetailsTable.ItemID + ","
                        + PlaceAvailabeItem_ItemDetailsTable.Itemname + ","
                        + PlaceAvailabeItem_ItemDetailsTable.ItemCompany + ","
                        + PlaceAvailabeItem_ItemDetailsTable.ItemFolder + ","
                        + PlaceAvailabeItem_ItemDetailsTable.AvailableItemStates 

                        + " from  "
                        + PlaceAvailabeItem_ItemDetailsTable.TableName + "(" + placeid.ToString() + ")"
                        );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint itemid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string itemname = t.Rows[i][1].ToString();
                        string itemcompany = t.Rows[i][2].ToString();
                        string itemfolder = t.Rows[i][3].ToString();
                        string available_states_amount = t.Rows[i][4].ToString();
                        itemslist.Add(new PlaceAvailbeItems_ItemDetails(itemid, itemname, itemcompany, itemfolder, available_states_amount));
                    }
                    return itemslist;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":فشل جلب العناصر المخزنة" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return itemslist;
                }
            }
            public List<PlaceAvailbeItems_ItemINDetails> GetStoredItems_BuyOPRDetails(TradeStorePlace place, Item item)
            {
                List<PlaceAvailbeItems_ItemINDetails> iteminlist = new List<PlaceAvailbeItems_ItemINDetails>();
                try
                {

                    DataTable t = DB.GetData(
                        "select  "
                        + PlaceAvailabeItem_ItemINDetailsTable.ItemSourceType  + ","
                        + PlaceAvailabeItem_ItemINDetailsTable.StoreType  + ","
                        + PlaceAvailabeItem_ItemINDetailsTable.OprID   + ","
                       + PlaceAvailabeItem_ItemINDetailsTable.ParentOperationDesc   + ","
                        + PlaceAvailabeItem_ItemINDetailsTable.ParentOperationID + ","
                         + PlaceAvailabeItem_ItemINDetailsTable.consumeunitname + ","
                        + PlaceAvailabeItem_ItemINDetailsTable.StoredAmount + ","
                         + PlaceAvailabeItem_ItemINDetailsTable.SpentAmount + ","
                          + PlaceAvailabeItem_ItemINDetailsTable.AvailableAmount
                        + " from  "
                        + PlaceAvailabeItem_ItemINDetailsTable.TableName
                        + "(" + place.PlaceID.ToString() + "," + item.ItemID.ToString() + ")"
                        );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        string itemsourcetype = t.Rows[i][0].ToString();
                        uint storetype = Convert.ToUInt32(t.Rows[i][1].ToString());
                        uint oprid = Convert.ToUInt32(t.Rows[i][2].ToString());
                        string parentopertaiondesc = t.Rows[i][3].ToString();
                        uint parentoperationid = Convert.ToUInt32(t.Rows[i][4].ToString());
                        string consumunitname = t.Rows[i][5].ToString();
                        double storedamount = Convert.ToDouble(t.Rows[i][6].ToString());
                        double SpentAMount = Convert.ToDouble(t.Rows[i][7].ToString());
                        double Available = Convert.ToDouble(t.Rows[i][8].ToString());
                        iteminlist.Add(new PlaceAvailbeItems_ItemINDetails(place, item, itemsourcetype ,storetype ,oprid ,parentopertaiondesc ,parentoperationid , consumunitname, storedamount, SpentAMount, Available));
                    }
                    return iteminlist;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":GetStoredItems_BuyOPRDetails" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return iteminlist;
                }
            }
            internal List<AvailbeItems_ItemINDetails> GetItemINList_ForAvailableItem(uint itemid)
            {
                List<AvailbeItems_ItemINDetails> iteminlist = new List<AvailbeItems_ItemINDetails>();
                try
                {
                    
                    DataTable t = DB.GetData(
                        "select  "
                        //+ AvailabeItem_ItemINDetailsTable.TradeStateName + ","
                        + AvailabeItem_ItemINDetailsTable.ItemINID  + ","
                       // + AvailabeItem_ItemINDetailsTable.OperationType  + ","
                       //+ AvailabeItem_ItemINDetailsTable.OperationID  + ","
                       //  + AvailabeItem_ItemINDetailsTable.consumeunitname + ","
                       // + AvailabeItem_ItemINDetailsTable.Amount   + ","
                         + AvailabeItem_ItemINDetailsTable.SpentAmount + ","
                          + AvailabeItem_ItemINDetailsTable.AvailableAmount
                        + " from  "
                        + AvailabeItem_ItemINDetailsTable.TableName
                        + "(" + itemid.ToString() + ")"
                        );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        ItemIN ItemIN_ = new ItemINSQL(DB).GetItemININFO_BYID(Convert.ToUInt32(t.Rows[i][0].ToString()));
                        //string tradestatename = t.Rows[i][0].ToString();
                        //uint iteminid = Convert.ToUInt32(t.Rows[i][1].ToString());
                        //uint operationtype = Convert.ToUInt32(t.Rows[i][2].ToString());
                        //uint operationid = Convert.ToUInt32(t.Rows[i][3].ToString());
                        //string consumunitname = t.Rows[i][4].ToString();
                        //double buyamount = Convert.ToDouble(t.Rows[i][5].ToString());
                        double SpentAMount = Convert.ToDouble(t.Rows[i][1].ToString());
                        double Available = Convert.ToDouble(t.Rows[i][2].ToString());
                        iteminlist.Add(new AvailbeItems_ItemINDetails( ItemIN_ , SpentAMount, Available));
                    }
                    return iteminlist;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":مصادر العنصر" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return iteminlist;
                }
            }
            internal List<AvailableItem> GetAvailableItemsInFolder(Folder folder)
            {
                List<AvailableItem> list = new List<AvailableItem>();
                try
                {

                    if (folder == null) return list;

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ItemSQL.ItemTable.ItemID + ","
                        + "[dbo].[Trade_GetAMountInfo_ForITEM]("
                        + ItemSQL.ItemTable.ItemID + ")"
                        + " from " + ItemSQL.ItemTable.TableName
                       + " where " + ItemSQL.ItemTable.FolderID + "=" + folder.FolderID
                       );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        Item Item_ =new ItemSQL(DB). GetItemInfoByID(Convert.ToUInt32(t.Rows[i][0].ToString()));
                        string availableamount = t.Rows[i][1].ToString();
                        string folderpath = new FolderSQL(DB).GetFolderPath(Item_.folder);
                        list.Add(new AvailableItem(Item_, availableamount, folderpath));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب العناصر المتوفرة: " + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

            }
            public List<AvailableItem> FilterAvailableItemsBySpec(List<ItemSpec_Restrict_Options> ItemSpec_Restrict_Options_List, List<ItemSpec_Value> ItemSpec_Value_List)
            {
                string Cmd_Statemanet = "  ";
                for (int i = 0; i < ItemSpec_Restrict_Options_List.Count; i++)
                {
                    Cmd_Statemanet += "select "
                        + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.ItemID
                        + " from "
                        + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.TableName
                        + " where concat("
                        + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.SpecID
                        + ","
                        + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.OptionID
                        + ")="
                        + ItemSpec_Restrict_Options_List[i].ItemSpecRestrict_.SpecID.ToString()
                        + ItemSpec_Restrict_Options_List[i].OptionID.ToString();
                    if (i != ItemSpec_Restrict_Options_List.Count - 1)
                        Cmd_Statemanet += "  INTERSECT ";
                }
                if (ItemSpec_Value_List.Count > 0 && ItemSpec_Restrict_Options_List.Count > 0) Cmd_Statemanet += " INTERSECT ";
                for (int i = 0; i < ItemSpec_Value_List.Count; i++)
                {
                    Cmd_Statemanet += "select "
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.ItemID
                                      + " from "
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.TableName
                                      + " where concat("
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.SpecID
                                      + ","
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.Value
                                      + ")='"
                                      + ItemSpec_Value_List[i].ItemSpec_.SpecID.ToString()
                                      + ItemSpec_Value_List[i].Value + "'";
                    if (i != ItemSpec_Value_List.Count - 1)
                        Cmd_Statemanet += "  INTERSECT ";
                }
                List<AvailableItem> Item_list = new List<AvailableItem>();

                DataTable t = new DataTable();
                t = DB.GetData(Cmd_Statemanet
                   );
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    Item Item_ = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][0].ToString()));
                    string availableamount = GetAvailabeAmount (Item_.ItemID);
                    string folderpath = new FolderSQL(DB).GetFolderPath(Item_.folder);
                    Item_list.Add(new AvailableItem(Item_, availableamount, folderpath));
                }
                return Item_list;
            }
            public string GetAvailabeAmount(uint itemid)
            {
                string amount = "";
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select [dbo].[Trade_GetAMountInfo_ForITEM]("
                        + itemid + ")");
                    if (t.Rows.Count == 1)
                    {
                        amount = t.Rows[0][0].ToString();

                    }

                    return amount;

                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب الكميات المتوفرة من العنصر:" + ee.Message);
                    return "";
                }
            }
            internal List<AvailableItemSimple> GetAvailableItemsSimple()
            {
                List<AvailableItemSimple> list = new List<AvailableItemSimple>();
                try
                {
 
                   
                    DataTable t = new DataTable();
                    t = DB.GetData("select ItemID "
                                + ","
                                + "(select FolderName from Item_Folder where Item_Folder.FolderID = Item_Item.FolderID)"
                                + ",ItemName,ItemCompany,dbo.Trade_GetAMountInfo_ForITEM(ItemID),"
                                +FolderSQL.FolderPathFunction +"(FolderID) "
                                + "from Item_Item"
                       );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint itemid=Convert.ToUInt32(t.Rows[i][0].ToString());
                        string foldername = t.Rows[i][1].ToString();
                        string itemname = t.Rows[i][2].ToString();
                        string itemcompany = t.Rows[i][3].ToString();
                        string availableamount = t.Rows[i][4].ToString();
                        string path = t.Rows[i][5].ToString();
                        //string folderpath = new FolderSQL(DB).GetFolderPath(Item_.folder);
                        list.Add(new AvailableItemSimple(itemid,foldername ,itemname ,itemcompany
                            , availableamount, path ));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب العناصر المتوفرة: " + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

            }
        }
        public class TradeStoreContainerSQL
        {
            DatabaseInterface DB;
            private static class TradeStoreContainerTable
            {
                public const string TableName = "Trade_Store_Container";
                public const string ContainerID = "ContainerID";
                public const string ContainerName = "ContainerName";
                public const string ParentContainerID = "ParentContainerID";
                public const string Desc = "Description";
            }
            public TradeStoreContainerSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public string GetContainerPath(TradeStoreContainer  container)
            {
                List<string> container_path = new List<string>();
                TradeStoreContainer f = container ;
                string s = "ROOT / ";
                while (f.ParentContainerID != null)
                {
                    f = GetContainerBYID(Convert.ToUInt32(f.ParentContainerID  ));
                    container_path.Add(f.ContainerName );
                }
                for (int i = container_path.Count - 1; i >= 0; i--)
                    s += container_path[i] + " /";
                return s;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="container"></param>
            /// <returns></returns>
            public TradeStoreContainer  GetParentContainer(TradeStoreContainer container)
            {
                try
                {
                    if (container.ParentContainerID == null) return null;
                    DataTable t = new DataTable();
                    try
                    {
                        t = DB.GetData("select  "
                                    + TradeStoreContainerTable.ContainerName + ","
                                    + TradeStoreContainerTable.ParentContainerID + ","
                                    + TradeStoreContainerTable.Desc
                                    + " from "
                                    + TradeStoreContainerTable.TableName
                                    + " where "
                                    + TradeStoreContainerTable.ContainerID + "=" + container.ParentContainerID
                                    );
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }


                    uint fid = Convert.ToUInt32(container.ParentContainerID);
                    string fname = t.Rows[0][0].ToString();
                    uint? p;
                    try
                    {
                        p = Convert.ToUInt32(t.Rows[0][1]);
                    }
                    catch
                    {
                        p = null;
                    }
                    string desc = t.Rows[0][2].ToString();
                    return new TradeStoreContainer(fid, fname, p, desc);

                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("AddContainer" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }


               
            }
            public TradeStoreContainer GetContainerBYID(uint containerid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + TradeStoreContainerTable.ContainerName + ","
                    + TradeStoreContainerTable.ParentContainerID + ","
                    + TradeStoreContainerTable.Desc
                    + " from   "
                    + TradeStoreContainerTable.TableName
                    + " where "
                    + TradeStoreContainerTable.ContainerID + "=" + containerid
                      );
                    if (t.Rows.Count == 1)
                    {
                        string containername = t.Rows[0][0].ToString();

                        string desc = t.Rows[0][2].ToString();
                        uint? parentcontainerID;
                        try
                        {
                            parentcontainerID = Convert.ToUInt32(t.Rows[0][1].ToString());
                        }
                        catch
                        {
                            parentcontainerID = null;
                        }


                        return new TradeStoreContainer(containerid, containername, parentcontainerID, desc);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("AddContainer" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
               
            }
            public bool AddContainer(uint? parentcontainerID,string Containername, string desc)
            {
                try
                {
                    string parentid_string = "";
                    if (parentcontainerID == null)
                        parentid_string = "null";
                    else
                        parentid_string = parentcontainerID.ToString();
                    DB.ExecuteSQLCommand(" insert into "
                    + TradeStoreContainerTable.TableName
                    + "("
                    + TradeStoreContainerTable.ContainerName+","
                    +TradeStoreContainerTable.ParentContainerID + ","
                    + TradeStoreContainerTable.Desc 
                    + ")"
                    + "values"
                    + "("
                    + "'" + Containername  + "'"
                    +","
                    + parentid_string
                    + ","
                    +"'"+desc+"'" 
                    + ")"
                    );

                    DB.AddLog(
                                       DatabaseInterface.Log.LogType.INSERT
                                       , DatabaseInterface.Log.Log_Target.Trade_Store_Container
                                        , ""
                                        , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_Store_Container
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
      
            }
            public bool UpdateContainer(uint containerid, string containername,string desc)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + TradeStoreContainerTable.TableName
                    + " set "
                    + TradeStoreContainerTable.ContainerName  + "='" + containername + "'"
                    + ","
                    + TradeStoreContainerTable.Desc + "=" + desc 
                    + " where "
                    + TradeStoreContainerTable.ContainerID + "=" + containerid
                    );
                    DB.AddLog(
                                        DatabaseInterface.Log.LogType.UPDATE 
                                        , DatabaseInterface.Log.Log_Target.Trade_Store_Container
                                         , ""
                                         , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_Store_Container
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteContainer(uint containerid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + TradeStoreContainerTable.TableName
                    + " where "
                    + TradeStoreContainerTable.ContainerID + "=" + containerid
                    );
                    DB.AddLog(
                                      DatabaseInterface.Log.LogType.DELETE 
                                      , DatabaseInterface.Log.Log_Target.Trade_Store_Container
                                       , ""
                                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_Store_Container
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<TradeStoreContainer> GetContainerChildsList(TradeStoreContainer container)
            {
                try
                {
                    string parentid_string = "";
                    if (container == null)
                        parentid_string = " is null ";
                    else
                        parentid_string =" = "+ container.ContainerID  .ToString();
                    List<TradeStoreContainer> conainerchilds_list = new List<TradeStoreContainer>();
                    DataTable t = new DataTable();
                    //Forms.Form1 d = new Forms.Form1("select    "
                    //+ TradeStoreContainerTable.ContainerID + ","
                    //+ TradeStoreContainerTable.ContainerName + ","
                    //+ TradeStoreContainerTable.Desc
                    //+ " from   "
                    //+ TradeStoreContainerTable.TableName
                    //   + " where "
                    //+ TradeStoreContainerTable.ParentContainerID + parentid_string
                    // );
                    //d.ShowDialog();
                    t = DB.GetData("select    "
                    +TradeStoreContainerTable.ContainerID  + ","
                    + TradeStoreContainerTable.ContainerName + ","
                    + TradeStoreContainerTable.Desc
                    + " from   "
                    + TradeStoreContainerTable.TableName
                       + " where "
                    + TradeStoreContainerTable.ParentContainerID  + parentid_string
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint containerid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string containername = t.Rows[i][1].ToString();
                        string desc = t.Rows[i][2].ToString();
                        uint? p;
                        if (container  == null) p = null;
                        else p = container.ContainerID ;


                        conainerchilds_list.Add(new TradeStoreContainer(containerid, containername, p , desc));
                    }
                    return conainerchilds_list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("فشل جلب الحاويات الابناء" + ee.Message, "خطأ" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }

            internal List<TradeStoreContainer> SearchContainer(string text)
            {
                List<TradeStoreContainer> list = new List<TradeStoreContainer>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + TradeStoreContainerTable.ContainerID + ","
                        + TradeStoreContainerTable.ContainerName + ","
                        + TradeStoreContainerTable.ParentContainerID + ","
                        + TradeStoreContainerTable.Desc
                        + " from " + TradeStoreContainerTable.TableName
                       + " where " + TradeStoreContainerTable.ContainerName + " like  '%" + text + "%'");
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint conainerid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string conainername = t.Rows[i][1].ToString();
                        uint? p;
                        try
                        {
                            p = Convert.ToUInt32(t.Rows[i][2].ToString());
                        }
                        catch
                        {
                            p = null;
                        }

                        string d = t.Rows[i][3].ToString();
                        list.Add(new TradeStoreContainer(conainerid, conainername, p, d));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show(ee.Message + ":فشل جلب بيانات مكان التخزين", "خطأ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

                
            }
        }
        public class TradeStorePlaceSQL
        {

            DatabaseInterface DB;
            private static class TradeStorePlaceTable
            {
                public const string TableName = "Trade_Store_Place";
                public const string PlaceID = "PlaceID";
                public const string PlaceName = "PlaceName";
                public const string ContainerID = "ContainerID";
                public const string Desc = "Description";

            }
           
            public TradeStorePlaceSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public string GetPlacePath(TradeStorePlace  place)
            {
                if (place == null) return "";
                TradeStoreContainerSQL  containerSQL_ = new TradeStoreContainerSQL(DB);
                List<string> container_path = new List<string>();
                TradeStoreContainer f = place._TradeStoreContainer;
                string s = "ROOT /";

                while (f.ParentContainerID  != null)
                {
                    container_path.Add(f.ContainerName );
                    f = containerSQL_ .GetContainerBYID (Convert.ToUInt32(f.ParentContainerID ));

                }
                container_path.Add(f.ContainerName );
                for (int i = container_path.Count - 1; i >= 0; i--)
                    s += container_path[i] + "/";
                return s;
            }
            public TradeStorePlace GetTradeStorePlaceBYID(uint placeid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + TradeStorePlaceTable.PlaceName + ","
                    + TradeStorePlaceTable.ContainerID + ","
                    + TradeStorePlaceTable.Desc
                    + " from   "
                    + TradeStorePlaceTable.TableName
                    + " where "
                    + TradeStorePlaceTable.PlaceID  + "=" + placeid
                      );
                    if (t.Rows.Count == 1)
                    {
                        string placename = t.Rows[0][0].ToString();
                        string desc = t.Rows[0][2].ToString();
                        TradeStoreContainer container = new TradeStoreContainerSQL(DB).GetContainerBYID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                        return new TradeStorePlace(placeid, placename, container, desc);
                    }
                    else
                        return null;
                }
                catch(Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show(ee.Message + ":فشل جلب بيانات مكان التخزين", "خطأ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }
            public bool AddPlace(TradeStoreContainer container, string placename, string desc)
            {
                try
                {
                    DB.ExecuteSQLCommand(" insert into "
                    + TradeStorePlaceTable.TableName
                    + "("
                    + TradeStorePlaceTable.PlaceName  + ","
                    + TradeStorePlaceTable.ContainerID  + ","
                    + TradeStorePlaceTable.Desc
                    + ")"
                    + "values"
                    + "("
                    + "'" + placename  + "'"
                    + ","
                    + container .ContainerID 
                    + ","
                    + "'" + desc + "'" 
                    + ")"
                    );
                    DB.AddLog(
                                      DatabaseInterface.Log.LogType.INSERT
                                      , DatabaseInterface.Log.Log_Target.Trade_Store_Place 
                                       , ""
                                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_Store_Place 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdatePlace(TradeStoreContainer container, uint placeid, string placename,  string desc)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + TradeStorePlaceTable.TableName
                    + " set "
                    + TradeStorePlaceTable.PlaceName  + "='" + placename  + "'"
                    + ","
                    + TradeStorePlaceTable.ContainerID  + "=" + container .ContainerID 
                    + ","
                    + TradeStorePlaceTable.Desc + "=" + desc
                    + " where "
                    + TradeStorePlaceTable.ContainerID + "=" + placeid
                    );
                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.UPDATE 
                                     , DatabaseInterface.Log.Log_Target.Trade_Store_Place
                                      , ""
                                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_Store_Place
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeletePlace(uint placeid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + TradeStorePlaceTable.TableName
                    + " where "
                    + TradeStorePlaceTable.PlaceID  + "=" + placeid
                    );
                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.DELETE 
                                     , DatabaseInterface.Log.Log_Target.Trade_Store_Place
                                      , ""
                                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_Store_Place
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<TradeStorePlace > GetPlacesINContainer(TradeStoreContainer container)
            {
                try
                {
                 
                    List<TradeStorePlace> conainerPlaces_list = new List<TradeStorePlace>();
                    if (container == null) return conainerPlaces_list;
                    DataTable t = new DataTable();
                    t = DB.GetData("select    "
                    + TradeStorePlaceTable.PlaceID + ","
                    + TradeStorePlaceTable.PlaceName + ","
                    + TradeStorePlaceTable.Desc
                    + " from   "
                    + TradeStorePlaceTable.TableName
                       + " where "
                    + TradeStorePlaceTable.ContainerID + "=" + container .ContainerID 
                      );
                  
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint placeid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string placename = t.Rows[i][1].ToString();
                        string desc = t.Rows[0][2].ToString();



                        conainerPlaces_list.Add(new TradeStorePlace (placeid , placename , container, desc));
                    }
                    return conainerPlaces_list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show(ee.Message+ ":فشل جلب اماكن التخزين في الحاوية", "خطأ" + ee.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }

            internal List<TradeStorePlace> SearchPlace(string text)
            {

                List<TradeStorePlace> list = new List<TradeStorePlace>();
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + TradeStorePlaceTable.PlaceID + ","
                        + TradeStorePlaceTable.PlaceName + ","
                        + TradeStorePlaceTable.ContainerID + ","
                        + TradeStorePlaceTable.Desc
                        + " from " + TradeStorePlaceTable.TableName
                       + " where " + TradeStorePlaceTable.PlaceName + " like  '%" + text + "%'");
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint placeid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string placename = t.Rows[i][1].ToString();
                        TradeStoreContainer container = new TradeStoreContainerSQL(DB).GetContainerBYID(Convert.ToUInt32(t.Rows[i][2].ToString()));
                        string d = t.Rows[i][3].ToString();
                        list.Add(new TradeStorePlace(placeid, placename, container, d));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("SearchPlace" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }
               
            }
           
        }
        public class BillSellSQL
        {
            DatabaseInterface DB;
            private static class BillSELLTable
            {
                public const string TableName = "Trade_BillSell";
                public const string BillSellID = "BillSellID";
                public const string BillDate = "BillDate";
                public const string BillDescription = "BillDescription";
                public const string SellTypeID = "SellTypeID";
                public const string ContactID = "ContactID";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string Discount = "Discount";
                public const string Notes = "Notes";

            }
            public BillSellSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public BillSell  GetBillSell_INFO_BYID(uint billid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + BillSELLTable.BillDate  + ","
                    + BillSELLTable.BillDescription + ","
                    + BillSELLTable.SellTypeID  + ","
                    + BillSELLTable.ContactID + ","
                    + BillSELLTable.CurrencyID + ","
                     + BillSELLTable.ExchangeRate + ","
                    + BillSELLTable.Discount + ","
                    + BillSELLTable.Notes
                    + " from   "
                    + BillSELLTable.TableName
                    + " where "
                    + BillSELLTable.BillSellID + "=" + billid 
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime billindate = Convert.ToDateTime(t.Rows[0][0]);
                        string desc = t.Rows[0][1].ToString();
                        SellType  SellType_ =new SellTypeSql (DB).GetSellTypeinfo (Convert .ToUInt32 ( t.Rows[0][2].ToString()));
                        Contact Contact_ = new ContactSQL(DB).GetContactInforBYID(Convert.ToUInt32(t.Rows[0][3].ToString()));
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][4].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[0][5].ToString());
                        double discount = Convert.ToDouble(t.Rows[0][6].ToString());
                        string notes = t.Rows[0][7].ToString();
                        return new BillSell (billid , billindate, desc, SellType_, Contact_, Currency_, exchangerate, discount, notes);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillIN_INFO_BYID" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }
            public BillSell  AddBillSell(DateTime billindate, string description,SellType  SellType_, Contact contact, Currency currency, double exchangerate, double discount, string notes)
            {
                try
                {

                    DataTable t = DB.GetData(" insert into "
                    + BillSELLTable.TableName
                    + "("
                    + BillSELLTable.BillDate 
                    + ","
                    + BillSELLTable.BillDescription
                    + ","
                    + BillSELLTable.SellTypeID
                    + ","
                    + BillSELLTable.ContactID
                    + ","
                    + BillSELLTable.CurrencyID
                    + ","
                     + BillSELLTable.ExchangeRate
                    + ","
                    + BillSELLTable.Discount
                    + ","
                    + BillSELLTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + "'" + billindate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'" + description + "'"
                     + ","
                    +  SellType_ .SellTypeID
                    + ","
                    + contact.ContactID
                    + ","
                    + currency.CurrencyID
                    + ","
                    + exchangerate
                    + ","
                    + discount
                    + ","
                    + "'" + notes + "'"
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint billid = Convert.ToUInt32(t.Rows[0][0].ToString());

                    
                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.INSERT
                                     , DatabaseInterface.Log.Log_Target.Trade_BillSell
                                      , ""
                                      , true, "");
                    return GetBillSell_INFO_BYID(billid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_BillSell 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateBillIN(uint billid, DateTime billindate, string description,SellType   SellType_, Contact contact, Currency currency, double exchangerate, double discount, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand("update  "
                    + BillSELLTable.TableName
                    + " set "
                    + BillSELLTable.BillDate  + "='" + billindate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + BillSELLTable.BillDescription + "='" + description + "'"
                     + ","
                    + BillSELLTable.SellTypeID + "=" + SellType_.SellTypeID
                    + ","
                    + BillSELLTable.ContactID + "=" + contact.ContactID
                    + ","
                    + BillSELLTable.CurrencyID + "=" + currency.CurrencyID
                    + ","
                   + BillSELLTable.ExchangeRate + "=" + exchangerate
                    + ","
                    + BillSELLTable.Discount + "=" + discount
                    + ","
                    + BillSELLTable.Notes + "='" + notes + "'"
                    + " where "
                    + BillSELLTable.BillSellID  + "=" + billid 
                    );

                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.UPDATE 
                                     , DatabaseInterface.Log.Log_Target.Trade_BillSell
                                      , ""
                                      , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_BillSell
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteBillSell(uint billid)
            {
                try
                {
                    DB.ExecuteSQLCommand( "delete from   "
                   + ItemOUTSQL .ItemOUTTable .TableName
                   + " where "
                   + ItemOUTSQL .ItemOUTTable .OperationType + "=" + Operation .BILL_SELL
                   + " and "
                   + ItemOUTSQL.ItemOUTTable.OperationID + "=" +billid
                   );
                    DB.ExecuteSQLCommand("delete from   "
                    + BillSELLTable.TableName
                    + " where "
                    + BillSELLTable.BillSellID  + "=" + billid 
                    );
                    DB.AddLog(
                                      DatabaseInterface.Log.LogType.DELETE 
                                      , DatabaseInterface.Log.Log_Target.Trade_BillSell
                                       , ""
                                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_BillSell
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

            internal double GetBillSellValue(uint billsellid)
            {
                try
                {

                    return new OperationSQL(DB).Get_OperationValue(Operation.BILL_SELL, billsellid );
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillSellValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
            internal double GetBillSell_PaysValue(uint billsellid)
            {
                try
                {
                    return new OperationSQL(DB).Get_OperationPaysValue_UPON_OperationCurrency (Operation.BILL_SELL , billsellid );
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillSellValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
        }
       
        
       

        
        public class TradeItemStoreSQL
        {
            DatabaseInterface DB;
            private static class TradeItemStoreTable
            {
                public const string TableName = "Trade_Items_Store";
                public const string PlaceID = "PlaceID";
                public const string ItemSourceOPRID = "ItemSourceOPRID";
                public const string StoreType = "StoreType";

                public const string Amount = "Amount";
                public const string ConsumeUnitID = "ConsumeUnitID";


            }
            public TradeItemStoreSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public TradeItemStore GetTradeItemStoreINFO(TradeStorePlace place, uint ItemSourceOPRID_,uint StoreType_)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + TradeItemStoreTable.Amount+","
                        +TradeItemStoreTable .ConsumeUnitID 
                        + " from   "
                        + TradeItemStoreTable.TableName
                        + " where "
                        + TradeItemStoreTable.PlaceID + "=" + place.PlaceID
                        +" and "
                        + TradeItemStoreTable.ItemSourceOPRID  + "=" + ItemSourceOPRID_
                        + " and "
                        + TradeItemStoreTable.StoreType  + "=" + StoreType_ 
                      );

                    if (t.Rows.Count == 1)
                    {
                        int amount = Convert.ToInt32(t.Rows[0][0].ToString());
                        ConsumeUnit consumeunit ;
                        try
                        {
                            uint consumeunit_id = Convert.ToUInt32(t.Rows [0][1].ToString ());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunit_id);
                        }catch
                        {
                            consumeunit = null;
                        }
                        return new TradeItemStore( place,ItemSourceOPRID_ ,StoreType_ , amount,consumeunit );
                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetTradeItemStoreINFO" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public bool IS_ItemStoredInPlace( uint PlaceID, uint ItemSourceOPRID_, uint StoreType_)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select  * from   "
                        + TradeItemStoreTable.TableName
                        +" where "
                        + TradeItemStoreTable.PlaceID +"="+PlaceID
                       + " and "
                        + TradeItemStoreTable.ItemSourceOPRID + "=" + ItemSourceOPRID_
                        + " and "
                        + TradeItemStoreTable.StoreType + "=" + StoreType_
                      );
                    if (t.Rows.Count > 0) return true;
                    else return false;
                }
                catch(Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("IS_ItemStoredInPlace" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    return false; 
                }
            }
            public bool UpdateItemAmountStored( uint PlaceID, uint ItemSourceOPRID_, uint StoreType_, double  amount)
            {
                try
                {
     
                    DB.ExecuteSQLCommand("update  "
                        + TradeItemStoreTable.TableName
                        +" set "
                        + TradeItemStoreTable.Amount+"=" +amount
                        + " where "
                        + TradeItemStoreTable.PlaceID + "=" + PlaceID
                         + " and "
                        + TradeItemStoreTable.ItemSourceOPRID + "=" + ItemSourceOPRID_
                        + " and "
                        + TradeItemStoreTable.StoreType + "=" + StoreType_
                      );
                    DB.AddLog(
                                    DatabaseInterface.Log.LogType.UPDATE
                                    , DatabaseInterface.Log.Log_Target.Trade_ItemsStore 
                                     , ""
                                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE
                            , DatabaseInterface.Log.Log_Target.Trade_ItemsStore
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Store_Item_INPlace( uint PlaceID, uint ItemSourceOPRID_, uint StoreType_, double  amount,ConsumeUnit ConsumeUnit_)
            {
                try
                {
                    if (IS_ItemStoredInPlace(PlaceID, ItemSourceOPRID_ ,StoreType_ ))
                        UpdateItemAmountStored(PlaceID, ItemSourceOPRID_, StoreType_, amount);
                    else 
                    DB.ExecuteSQLCommand(" insert into "
                    + TradeItemStoreTable.TableName
                    + "("

                    + TradeItemStoreTable.PlaceID
                    + ","
                     + TradeItemStoreTable.ItemSourceOPRID
                    + ","
                     + TradeItemStoreTable.StoreType 
                    + ","
                    + TradeItemStoreTable.Amount
                    + ","
                    + TradeItemStoreTable.ConsumeUnitID 
                    + ")"
                    + "values"
                    + "("
 
                    + PlaceID
                    + ","
                    + ItemSourceOPRID_ 
                    + ","
                    + StoreType_ 
                    + ","
                    + amount
                    + ","
                    + (ConsumeUnit_ ==null ?"null": ConsumeUnit_.ConsumeUnitID .ToString ())
                    + ")"
                    );
                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.INSERT 
                                     , DatabaseInterface.Log.Log_Target.Trade_ItemsStore
                                      , ""
                                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemsStore
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UNStore_Item_INPlace( uint PlaceID, uint ItemSourceOPRID_, uint StoreType_)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                        + TradeItemStoreTable.TableName
                        + " where "
                        + TradeItemStoreTable.PlaceID + "=" + PlaceID
                         + " and "
                        + TradeItemStoreTable.ItemSourceOPRID + "=" + ItemSourceOPRID_
                        + " and "
                        + TradeItemStoreTable.StoreType + "=" + StoreType_
                      );
                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.DELETE 
                                     , DatabaseInterface.Log.Log_Target.Trade_ItemsStore
                                      , ""
                                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_ItemsStore
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
           
            public int GetCountTypes_OF_AvailableItems(uint placeid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select count(distinct(ItemID)) from "
                        +" [dbo].[Trade_GetAvailableItemsInPlace](   "
                        +placeid 
                        +" )"
                      );


                    return Convert .ToInt32 (t.Rows [0][0].ToString ());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetCountTypes_OF_AvailableItems" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return 0;
                }
            }
            public List<TradeItemStore > GetItemsStoredINPlace(TradeStorePlace place)
            {
                try
                {
                    List<TradeItemStore> storeditems = new List<TradeItemStore>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + TradeItemStoreTable.ItemSourceOPRID   + ","
                          + TradeItemStoreTable.StoreType  + ","
                         + TradeItemStoreTable.Amount + ","
                        + TradeItemStoreTable.ConsumeUnitID
                        + " from   "
                        + TradeItemStoreTable.TableName
                        +" where "
                         + TradeItemStoreTable.PlaceID+"=" +place .PlaceID 
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint  itemsourceopr_id = Convert.ToUInt32(t.Rows[i][0].ToString());
                        uint storetype = Convert.ToUInt32(t.Rows[i][1].ToString());
                        double  amount = Convert.ToInt32(t.Rows[i][2].ToString());
                        ConsumeUnit consumeunit;
                        try
                        {
                            uint consumeunit_id = Convert.ToUInt32(t.Rows[i][2].ToString());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunit_id);
                        }
                        catch
                        {
                            consumeunit = null;
                        }
                        storeditems.Add(new TradeItemStore( place , itemsourceopr_id,storetype , amount,consumeunit  ));
                    }
                    return storeditems;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("TradeItemStoreSQL-GetItemsStoredINPlace" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<TradeItemStore> GetItemStoredPlaces(uint iteminid)
            {
                
                List<TradeItemStore> storeditems = new List<TradeItemStore>();
                try
                {
    
                    DataTable t= DB.GetData("select "
                        + TradeItemStoreTable.PlaceID  + ","
                        + TradeItemStoreTable.Amount + ","
                        + TradeItemStoreTable.ConsumeUnitID
                        + " from   "
                        + TradeItemStoreTable.TableName
                        + " where "
                        + TradeItemStoreTable.ItemSourceOPRID + "=" + iteminid 
                        + " and "
                        + TradeItemStoreTable.StoreType + "=" + TradeItemStore.ITEMIN_STORE_TYPE
                      );
                  
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                       
                        TradeStorePlace place = new TradeStorePlaceSQL (DB).GetTradeStorePlaceBYID(Convert.ToUInt32(t.Rows[i][0].ToString()));
                        int amount = Convert.ToInt32(t.Rows[i][1].ToString());
                        ConsumeUnit consumeunit;
                        try
                        {
                            uint consumeunit_id = Convert.ToUInt32(t.Rows[i][2].ToString());
                            consumeunit = new ConsumeUnitSql(DB).GetConsumeAmountinfo(consumeunit_id);
                        }
                        catch
                        {
                            ItemIN ItemIN_ = new ItemINSQL(DB).GetItemININFO_BYID(iteminid);
                            consumeunit = new ConsumeUnit (0, ItemIN_._Item.DefaultConsumeUnit, ItemIN_._Item,1);
                        }
                       
                        storeditems.Add(new TradeItemStore( place,iteminid ,TradeItemStore .ITEMIN_STORE_TYPE, amount,consumeunit ));
                        
                    }

                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("TradeItemStoreSQL-GetItemStoredPlaces" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }
                
                return storeditems;
            }
            internal double getNON_StoredAmount(ItemIN ItemIN_)
            {
          
                double storedamount = 0;
                try
                {
                    
                   DataTable t=  DB.GetData ("Select sum("
                        + TradeItemStoreTable .Amount 
                        + ") from   "
                    + TradeItemStoreTable.TableName
                    + " where "
                   + TradeItemStoreTable.ItemSourceOPRID + "=" + ItemIN_.ItemINID 
                   + " and "
                   + TradeItemStoreTable.StoreType + "=" + TradeItemStore.ITEMIN_STORE_TYPE
                    );
                        storedamount = Convert.ToDouble(t.Rows[0][0].ToString());
                        return (ItemIN_.Amount - storedamount);           
                }
                catch 
                {
                    return ItemIN_.Amount;
                }
            }
            public TradeStorePlace  GetMaintenanceStorePlace(UInt32 maintenenaceoprid)
            {
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + TradeItemStoreTable.PlaceID 
                        + " from   "
                        + TradeItemStoreTable.TableName
                        + " where "
                        + TradeItemStoreTable.ItemSourceOPRID + "=" + maintenenaceoprid
                        + " and "
                        + TradeItemStoreTable.StoreType + "=" + TradeItemStore.MAINTENANCE_ITEM_STORE_TYPE
                      );

                   if(t.Rows .Count ==1)
                    {
                        TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
                        return place;
                    }
                    return null ;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetMaintenanceStorePlace" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public TradeStorePlace GetAccessoryStorePlace(UInt32 AccessoryID)
            {
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + TradeItemStoreTable.PlaceID
                        + " from   "
                        + TradeItemStoreTable.TableName
                        + " where "
                        + TradeItemStoreTable.ItemSourceOPRID + "=" + AccessoryID
                        + " and "
                        + TradeItemStoreTable.StoreType + "=" + TradeItemStore.MAINTENANCE_ACCESSORIES_ITEM_STORE_TYPE
                      );

                    if (t.Rows.Count == 1)
                    {
                        TradeStorePlace place = new TradeStorePlaceSQL(DB).GetTradeStorePlaceBYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
                        return place;
                    }
                    return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetAccessoryStorePlace" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
           
        }
 
        
    }
}
