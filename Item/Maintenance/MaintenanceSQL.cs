﻿using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.ItemObjSQL;
using ItemProject.ItemObj.Objects;
using ItemProject.Maintenance.Objects;
using ItemProject.Trade.Objects;
using ItemProject.Trade.TradeSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Maintenance
{
    namespace MaintenanceSQL
    {
        public class BillMaintenanceSQL
        {
            DatabaseInterface DB;
            private static class BillMaintenanceTable
            {
                public const string TableName = "Trade_BillMaintenance";
                public const string MaintenanceOPRID = "MaintenanceOPRID";
                public const string BillMaintenanceID = "BillMaintenanceID";
                public const string BillDate = "BillDate";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string Discount = "Discount";
                public const string Notes = "Notes";

            }
        
            public BillMaintenanceSQL(DatabaseInterface db)
            {
                DB = db;

            }
            internal BillMaintenance GetBillMaintenance_By_MaintenaceOPR(MaintenanceOPR MaintenanceOPR_)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + BillMaintenanceTable.BillMaintenanceID + ","
                    + BillMaintenanceTable.BillDate + ","
                    + BillMaintenanceTable.CurrencyID + ","
                    + BillMaintenanceTable.ExchangeRate + ","
                    + BillMaintenanceTable.Discount + ","
                    + BillMaintenanceTable.Notes
                    + " from   "
                    + BillMaintenanceTable.TableName
                    + " where "
                    + BillMaintenanceTable.MaintenanceOPRID + "=" + MaintenanceOPR_._Operation.OperationID
                      );
                    if (t.Rows.Count == 1)
                    {
                        uint  billid =Convert.ToUInt32(t.Rows[0][0]);
                        DateTime billdate = Convert.ToDateTime(t.Rows[0][1]);
                        //string desc = t.Rows[0][1].ToString ();
                        //Contact  Contact_ = new ContactSQL(DB).GetContactInforBYID (Convert.ToUInt32(t.Rows[0][2].ToString()));
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][2].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[0][3].ToString());
                        double discount = Convert.ToDouble(t.Rows[0][4].ToString());
                        string notes = t.Rows[0][5].ToString();
                        return new BillMaintenance(MaintenanceOPR_, billid, billdate, Currency_, exchangerate, discount, notes);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillMaintenance_By_MaintenaceOPR" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public BillMaintenance GetBillMaintenance_INFO_BYID(uint billid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                 + BillMaintenanceTable.MaintenanceOPRID  + ","
                + BillMaintenanceTable.BillDate+","
                + BillMaintenanceTable.CurrencyID + ","
                + BillMaintenanceTable.ExchangeRate  + ","
                + BillMaintenanceTable.Discount + ","
                + BillMaintenanceTable.Notes 
                + " from   "
                + BillMaintenanceTable.TableName
                + " where "
                + BillMaintenanceTable.BillMaintenanceID  + "=" + billid  
                  );
                if (t.Rows.Count == 1)
                {
                    MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(Convert .ToUInt32 (t.Rows[0][0]));
                    DateTime billdate = Convert.ToDateTime(t.Rows [0][1]);
                    //string desc = t.Rows[0][1].ToString ();
                    //Contact  Contact_ = new ContactSQL(DB).GetContactInforBYID (Convert.ToUInt32(t.Rows[0][2].ToString()));
                    Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][2].ToString()));
                    double exchangerate = Convert.ToDouble(t.Rows[0][3].ToString());
                    double discount =Convert .ToDouble ( t.Rows[0][4].ToString());
                    string notes = t.Rows[0][5].ToString();
                    return new BillMaintenance  (MaintenanceOPR_,billid , billdate  ,Currency_,exchangerate ,discount ,notes );

                }
                else
                    return null;
            }
            public BillMaintenance AddBillMaintenance(uint MaintenanceOPRID ,DateTime billdate
                ,Currency currency,double ExchangeRate,double discount,string notes)
            {
                try
                {
                   

                    DataTable t= DB.GetData (" insert into "
                    + BillMaintenanceTable.TableName
                    + "("
                     + BillMaintenanceTable.MaintenanceOPRID 
                    + ","
                    + BillMaintenanceTable.BillDate  
                    + ","
                    + BillMaintenanceTable.CurrencyID 
                    + ","
                    + BillMaintenanceTable.ExchangeRate 
                    + ","
                    + BillMaintenanceTable.Discount 
                    + ","
                    + BillMaintenanceTable.Notes 
                    + ")"
                    + "values"
                    + "("
                     + MaintenanceOPRID
                    + ","
                    + "'" + billdate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
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
                    uint billid = Convert.ToUInt32(t.Rows [0][0].ToString ());

                    
                    DB.AddLog( DatabaseInterface.Log.LogType.INSERT
                                         , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace
                                         , ""
                                       , true, "");
                    return GetBillMaintenance_INFO_BYID(billid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_SellPrice
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateBillMaintenance(uint billid,DateTime billdate,Currency currency,double ExchangeRate,double discount,string notes)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + BillMaintenanceTable.TableName
                    + " set "
                    + BillMaintenanceTable.BillDate  + "='" + billdate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    //+ BillMaintenanceTable.BillDescription + "='" + description   + "'"
                    //+ ","
                    //+ BillMaintenanceTable.ContactID + "=" + contact .ContactID 
                    //+ ","
                    + BillMaintenanceTable.CurrencyID  + "=" + currency .CurrencyID
                    + ","
                     + BillMaintenanceTable.ExchangeRate  + "=" + ExchangeRate 
                    + ","
                    + BillMaintenanceTable.Discount  + "=" + discount  
                    + ","
                    + BillMaintenanceTable.Notes  + "='" + notes  + "'"

                    + " where "
                    + BillMaintenanceTable.BillMaintenanceID  + "=" + billid  
                    );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.UPDATE 
                     , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace 
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show(""+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteBillMaintenance(uint billid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                   + PayINSQL.PayINTable .TableName
                   + " where "
                   + PayINSQL .PayINTable.OperationID   + "=" + billid
                   +" and "
                   + PayINSQL.PayINTable.OperationType  + "=" + Operation .BILL_MAINTENANCE
                   );
                   
                    DB.ExecuteSQLCommand("delete from   "
                    + BillMaintenanceTable.TableName
                    + " where "
                    + BillMaintenanceTable.BillMaintenanceID   + "=" + billid  
                    );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.DELETE 
                     , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
           
            public List<ItemOUT> Get_BillMaintenance_ItemsOUT(BillMaintenance BillMaintenance_)
            {
                List<ItemOUT> List = new List<Trade.Objects.ItemOUT>();
                try
                {
                    List.AddRange(new ItemOUTSQL(DB).GetItemOUTList(BillMaintenance_._Operation));

                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_BillMaintenance_ItemsOUT:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }
                return List;
            }
            #region Bill Clauses
            private static class BillMaintenance_RepairOPR_Clause_Table
            {
                public const string TableName = "Trade_BillMaintenance_Clause_RepairOPR";
                public const string BillMaintenanceID = "BillMaintenanceID";
                public const string RepairOPRID = "RepairOPRID";
                public const string Value_ = "Value_";


            }
            private static class BillMaintenance_DiagnosticOPR_Clause_Table
            {
                public const string TableName = "Trade_BillMaintenance_Clause_DiagnosticOPR";
                public const string BillMaintenanceID = "BillMaintenanceID";
                public const string DiagnosticOPRID = "DiagnosticOPRID";
                public const string Value_ = "Value_";


            }
            public bool BillMClause_IsRepairOPRValueSet(uint billid, uint RepairOPRID)
            {
                try
                {
                    DataTable t = DB.GetData(
                        "select * from "
                        +BillMaintenance_RepairOPR_Clause_Table.TableName
                    
                        );
                    if (t.Rows.Count   > 0) return true;
                    else return false;

                }
                catch (Exception ee)
                {
                    throw new Exception("BillMClause_IsRepairOPRValueSet:" + ee.Message);
                }
            }
            public bool BillMaintenance_Set_RepairOPR_Clause_Value(uint BillID,uint RepairOPRID,double Value_)
            {
                uint logtype;
                if (BillMClause_IsRepairOPRValueSet(BillID, RepairOPRID))
                    logtype = DatabaseInterface.Log.LogType.UPDATE;
                else
                    logtype = DatabaseInterface.Log.LogType.INSERT;

               try
                {
                    
                    if (BillMClause_IsRepairOPRValueSet(BillID, RepairOPRID))
                    {
                            DB.ExecuteSQLCommand("update  "
                        + BillMaintenance_RepairOPR_Clause_Table.TableName
                        + " set "
                        + BillMaintenance_RepairOPR_Clause_Table.Value_  + "=" + Value_


                        + " where "
                        + BillMaintenance_RepairOPR_Clause_Table.BillMaintenanceID + "=" + BillID
                        +" and "
                        + BillMaintenance_RepairOPR_Clause_Table.RepairOPRID + "=" + RepairOPRID 
                        );
     
                            DB.AddLog(
                   DatabaseInterface.Log.LogType.UPDATE
                         , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_RepairOPR
                         , ""
                       , true, "");
                    }
                    else
                    {
                            DataTable t = DB.GetData(" insert into "
                        + BillMaintenance_RepairOPR_Clause_Table.TableName
                        + "("
                         + BillMaintenance_RepairOPR_Clause_Table.BillMaintenanceID 
                        + ","
                        + BillMaintenance_RepairOPR_Clause_Table.RepairOPRID 
                        + ","
                        + BillMaintenance_RepairOPR_Clause_Table.Value_ 
                    
                        + ")"
                        + "values"
                        + "("
                         + BillID 
                        + ","
                        + RepairOPRID 
                        + ","
                        + Value_ 
                        + ")"
                        );
                        }

                        DB.AddLog(
                          DatabaseInterface.Log.LogType.INSERT 
                          , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_RepairOPR
                          , ""
                        , true, "");
                        return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      logtype 
                   , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool BillMaintenance_UNSet_RepairOPR_Clause_Value(uint BillID, uint RepairOPRID)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                   + BillMaintenance_RepairOPR_Clause_Table.TableName
                   + " where "
                   + BillMaintenance_RepairOPR_Clause_Table.BillMaintenanceID  + "=" + BillID 
                   + " and "
                   + BillMaintenance_RepairOPR_Clause_Table.RepairOPRID  + "=" +RepairOPRID 
                   );

                    DB.AddLog(
                              DatabaseInterface.Log.LogType.DELETE
                              , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_RepairOPR 
                              , ""
                            , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE
                            , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_RepairOPR 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public double? GetRepairOPRClauseValue(uint BillID, uint RepairOPRID)
            {
                try
                {
                   DataTable t= DB.GetData ("select     "
                   + BillMaintenance_RepairOPR_Clause_Table.Value_ 
                   + " from "
                   + BillMaintenance_RepairOPR_Clause_Table.TableName
                   + " where "
                   + BillMaintenance_RepairOPR_Clause_Table.BillMaintenanceID + "=" + BillID
                   + " and "
                   + BillMaintenance_RepairOPR_Clause_Table.RepairOPRID + "=" + RepairOPRID
                   );

                    if (t.Rows.Count == 1)
                    {
                        return Convert.ToDouble(t.Rows[0][0]);
                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("BillMaintenance_UNSet_RepairOPR_Clause_Value:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
      
            public bool BillMClause_IsDiagnosticOPRValueSet(uint billid, uint DiagnosticOPRID)
            {
                try
                {
                    DataTable t = DB.GetData(
                        "select * from "
                        + BillMaintenance_DiagnosticOPR_Clause_Table.TableName

                        );
                    if (t.Rows.Count > 0) return true;
                    else return false;

                }
                catch (Exception ee)
                {
                    throw new Exception("BillMClause_IsDiagnosticOPRValueSet:" + ee.Message);
                }
            }
            public bool BillMaintenance_Set_DiagnosticOPR_Clause_Value(uint BillID, uint DiagnosticOPRID, double Value_)
            {
                uint logtype;
                if (BillMClause_IsDiagnosticOPRValueSet(BillID, DiagnosticOPRID))
                    logtype = DatabaseInterface.Log.LogType.UPDATE;
                else
                    logtype = DatabaseInterface.Log.LogType.INSERT ;
                try
                {
                    if (BillMClause_IsDiagnosticOPRValueSet(BillID, DiagnosticOPRID))
                    {
                        DB.ExecuteSQLCommand("update  "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.TableName
                    + " set "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.Value_ + "=" + Value_


                    + " where "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.BillMaintenanceID + "=" + BillID
                    + " and "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.DiagnosticOPRID + "=" + DiagnosticOPRID
                    );

                        DB.AddLog(
                                  DatabaseInterface.Log.LogType.UPDATE 
                                  , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_DiagnosticOPR
                                  , ""
                                , true, "");
                    }
                    else
                    {
                        DataTable t = DB.GetData(" insert into "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.TableName
                    + "("
                     + BillMaintenance_DiagnosticOPR_Clause_Table.BillMaintenanceID
                    + ","
                    + BillMaintenance_DiagnosticOPR_Clause_Table.DiagnosticOPRID
                    + ","
                    + BillMaintenance_DiagnosticOPR_Clause_Table.Value_

                    + ")"
                    + "values"
                    + "("
                     + BillID
                    + ","
                    + DiagnosticOPRID
                    + ","
                    + Value_
                    + ")"
                    );
                        DB.AddLog(
                                 DatabaseInterface.Log.LogType.INSERT 
                                 , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_DiagnosticOPR
                                 , ""
                               , true, "");
                    }

                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                                 logtype 
                                 , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_DiagnosticOPR
                                 , ""
                               , true, "");
                    System.Windows.Forms.MessageBox.Show("BillMaintenance_Set_DiagnosticOPR_Clause_Value:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool BillMaintenance_UNSet_DiagnosticOPR_Clause_Value(uint BillID, uint DiagnosticOPRID)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                   + BillMaintenance_DiagnosticOPR_Clause_Table.TableName
                   + " where "
                   + BillMaintenance_DiagnosticOPR_Clause_Table.BillMaintenanceID + "=" + BillID
                   + " and "
                   + BillMaintenance_DiagnosticOPR_Clause_Table.DiagnosticOPRID + "=" + DiagnosticOPRID
                   );

                    DB.AddLog(
                              DatabaseInterface.Log.LogType.DELETE
                              , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_DiagnosticOPR 
                              , ""
                            , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE
                            , DatabaseInterface.Log.Log_Target.Trade_BillMaintenenace_Clause_DiagnosticOPR 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("Delete_PayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public double? GetDiagnosticOPRClauseValue(uint BillID, uint DiagnosticOPRID)
            {
                try
                {
                    DataTable t = DB.GetData("select     "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.Value_
                    + " from "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.TableName
                    + " where "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.BillMaintenanceID + "=" + BillID
                    + " and "
                    + BillMaintenance_DiagnosticOPR_Clause_Table.DiagnosticOPRID + "=" + DiagnosticOPRID
                    );

                    if (t.Rows.Count == 1)
                    {
                        return Convert.ToDouble(t.Rows[0][0]);
                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("BillMaintenance_UNSet_DiagnosticOPR_Clause_Value:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<BillMaintenance_Clause> BillMaintenance_GetClauses(BillMaintenance BillMaintenance_)
            {
                List<BillMaintenance_Clause> List = new List<Objects.BillMaintenance_Clause>();

                    try
                    {
                        List<ItemOUT> ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(BillMaintenance_._Operation);
                        for (int i = 0; i < ItemOUTList.Count; i++)
                            List.Add
                                (new Objects.BillMaintenance_Clause
                                (BillMaintenance_._Operation.OperationID, ItemOUTList[i]));
                    }
                    catch (Exception ee)
                    {
                        System.Windows.Forms.MessageBox.Show("BillMaintenance_ItemOutClauses:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    }
                    try
                    {
                        List<RepairOPR> RepairOPRList = new MaintenanceOPRSQL(DB).Get_MaintenanceOPR_RepairOPRList(BillMaintenance_._MaintenanceOPR);
                        for (int i = 0; i < RepairOPRList.Count; i++)
                            List.Add
                                (new Objects.BillMaintenance_Clause
                                (BillMaintenance_._Operation.OperationID, RepairOPRList[i],
                                GetRepairOPRClauseValue(BillMaintenance_._Operation.OperationID, RepairOPRList[i]._Operation.OperationID)));
                    }
                    catch (Exception ee)
                    {
                        System.Windows.Forms.MessageBox.Show("BillMaintenance_RepairOPRClauses:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                    try
                    {
                        List<DiagnosticOPRReport> DiagnosticOPRList = new DiagnosticOPRSQL(DB).GetSubDiagnosticOPRReportList(BillMaintenance_._MaintenanceOPR, null);
                        for (int i = 0; i < DiagnosticOPRList.Count; i++)
                            List.Add
                                (new Objects.BillMaintenance_Clause
                                (BillMaintenance_._Operation.OperationID, DiagnosticOPRList[i]._DiagnosticOPR,
                                GetDiagnosticOPRClauseValue(BillMaintenance_._Operation.OperationID, DiagnosticOPRList[i]._DiagnosticOPR.DiagnosticOPRID)));
                    }
                    catch (Exception ee)
                    {
                        System.Windows.Forms.MessageBox.Show("BillMaintenance_DiagnosticOPRClauses:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                try
                {
                    List<BillAdditionalClause > BillAdditionalClauseList = new BillAdditionalClauseSQL(DB).GetBill_AdditionalClauses (BillMaintenance_._Operation );
                    for (int i = 0; i < BillAdditionalClauseList.Count; i++)
                        List.Add
                            (new Objects.BillMaintenance_Clause
                            (BillMaintenance_._Operation.OperationID, BillAdditionalClauseList[i]));
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("BillMaintenance_DiagnosticOPRClauses:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }


                return List;
            }

            //public List<BillMaintenance_RepairOPR_Clause> Get_RepairOPRClauses(BillMaintenance BillMaintenance_)
            //{
            //    List<BillMaintenance_RepairOPR_Clause> List = new List<Objects.BillMaintenance_RepairOPR_Clause>();
            //    try
            //    {
            //        List<RepairOPR> RepairOPRList = new MaintenanceOPRSQL(DB).Get_MaintenanceOPR_RepairOPRList(BillMaintenance_._MaintenanceOPR);
            //        for (int i = 0; i < RepairOPRList.Count; i++)
            //            List.Add
            //                (new Objects.BillMaintenance_RepairOPR_Clause
            //                (BillMaintenance_._Operation.OperationID, RepairOPRList[i],
            //                GetRepairOPRClauseValue(BillMaintenance_._Operation.OperationID, RepairOPRList[i]._Operation.OperationID)));

            //    }
            //    catch (Exception ee)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Get_RepairOPRClauses:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            //    }
            //    return List;
            //}
            //public List<BillMaintenance_DiagnosticOPR_Clause> Get_DiagnosticOPRClauses(BillMaintenance BillMaintenance_)
            //{
            //    List<BillMaintenance_DiagnosticOPR_Clause> List = new List<Objects.BillMaintenance_DiagnosticOPR_Clause>();
            //    try
            //    {
            //        List<DiagnosticOPRReport> DiagnosticOPRList = new DiagnosticOPRSQL(DB).GetSubDiagnosticOPRReportList(BillMaintenance_._MaintenanceOPR,null );
            //        for (int i = 0; i < DiagnosticOPRList.Count; i++)
            //            List.Add
            //                (new Objects.BillMaintenance_DiagnosticOPR_Clause
            //                (BillMaintenance_._Operation.OperationID, DiagnosticOPRList[i]._DiagnosticOPR,
            //                GetDiagnosticOPRClauseValue(BillMaintenance_._Operation.OperationID, DiagnosticOPRList[i]._DiagnosticOPR.DiagnosticOPRID )));

            //    }
            //    catch (Exception ee)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Get_DiagnosticOPRClauses:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            //    }
            //    return List;
            //}
            #endregion

            internal double GetBillMaintenanceValue(uint billmid)
            {
                try
                {

                    return new OperationSQL(DB).Get_OperationValue(Operation.BILL_MAINTENANCE, billmid );
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillMaintenanceValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
            internal double GetBillMaintenance_PaysValue(uint billmid)
            {
                try
                {
                    return new OperationSQL(DB).Get_OperationPaysValue_UPON_OperationCurrency(Operation.BILL_MAINTENANCE, billmid );
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetBillMaintenance_PaysValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
        }
        public class MaintenanceOPRSQL
        {
            DatabaseInterface DB;
            internal  static class MaintenanceOPRTable
            {
               
                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR";
                public const string MaintenanceOPRID = "MaintenanceOPRID";
                public const string EntryDate = "EntryDate";
                public const string ContactID = "ContactID";
                public const string ItemID = "ItemID";
                public const string ItemSerial = "ItemSerial";
                public const string FaultDesc = "FaultDesc";
                public const string Notes = "Notes";

            }
            
            public MaintenanceOPRSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public MaintenanceOPR GetMaintenancePRINFO_BYID(uint MaintenanceOPRid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + MaintenanceOPRTable.EntryDate  + ","
                     + MaintenanceOPRTable.ContactID  + ","
                     + MaintenanceOPRTable.ItemID + ","
                     + MaintenanceOPRTable.ItemSerial + ","
                     + MaintenanceOPRTable.FaultDesc + ","
                     + MaintenanceOPRTable.Notes 
                     + " from   "
                    + MaintenanceOPRTable.TableName
                    + " where "
                    + MaintenanceOPRTable.MaintenanceOPRID  + "=" + MaintenanceOPRid
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime entrydate = Convert.ToDateTime(t.Rows[0][0].ToString());
                        Contact Contact_ = new ContactSQL(DB).GetContactInforBYID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                        Item item = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[0][2].ToString()));
                        string itemserial = t.Rows[0][3].ToString();
                        string  faultdesc = t.Rows[0][4].ToString();
                        string notes = t.Rows[0][5].ToString();
                        TradeStorePlace place = new TradeItemStoreSQL(DB).GetMaintenanceStorePlace(MaintenanceOPRid);
                        MaintenanceOPR_EndWork MaintenanceOPR_EndWork_ = Get_MaintenanceOPR_EndWork_ForMaintenanceOPR(MaintenanceOPRid);
                        return new MaintenanceOPR (MaintenanceOPRid,entrydate ,Contact_
                            , item, itemserial , faultdesc, place, MaintenanceOPR_EndWork_, notes);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetMaintenanceOPRINFO_BYID" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }

      

            public MaintenanceOPR AddMaintenanceOPR(DateTime EntryDate, uint ContactID, uint itemid
                , string  itemserial, string  faultdesc
                , string notes,TradeStorePlace Place)
            {
                try
                {
                    
                    DataTable t = DB.GetData(" insert into "
                    + MaintenanceOPRTable.TableName
                    + "("
                    + MaintenanceOPRTable.EntryDate  + ","
                    + MaintenanceOPRTable.ContactID  + ","
                     + MaintenanceOPRTable.ItemID + ","
                     + MaintenanceOPRTable.ItemSerial + ","
                     + MaintenanceOPRTable.FaultDesc + ","
                     + MaintenanceOPRTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + "'" + EntryDate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + ContactID 
                    + ","
                    + itemid
                     + ","
                   + "'" + itemserial + "'"
                    + ","
                    + "'" + faultdesc + "'"
                      + ","
                     + "'" + notes + "'"
                    + ")"
                     + " SELECT SCOPE_IDENTITY() "
                    );

                    uint maintenenaceoprid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    if (Place  != null) StoreMaintenanceItem (maintenenaceoprid, Place .PlaceID);
                   
                    DB.AddLog(
                             DatabaseInterface.Log.LogType.INSERT 
                             , DatabaseInterface.Log.Log_Target.Maintenenace_MaintenenaceOPR
                             , ""
                           , true, "");
                    return GetMaintenancePRINFO_BYID(maintenenaceoprid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_MaintenenaceOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateMaintenanceOPR(uint MaintenanceOPRID_, DateTime EntryDate, uint ContactID, uint itemid
                , string itemserial, string faultdesc
                ,  string notes,TradeStorePlace Place)
            {

                MaintenanceOPR MaintenanceOPR_ = GetMaintenancePRINFO_BYID(MaintenanceOPRID_);
                if (MaintenanceOPR_ == null) throw new Exception("عملية الصيانة غير موجودة");
                try
                {

                    DB.ExecuteSQLCommand("update  "
                    + MaintenanceOPRTable.TableName
                      + " set "
                       + MaintenanceOPRTable.EntryDate  + "=" + "'" + EntryDate .ToString("yyyy-MM-dd HH:mm:ss") + "'" + ","
                       + MaintenanceOPRTable.ContactID  + "=" + ContactID  + ","
                      + MaintenanceOPRTable.ItemID + "=" + itemid + ","
                      + MaintenanceOPRTable.ItemSerial + "='" + itemserial + "',"
                      + MaintenanceOPRTable.FaultDesc + "='" + faultdesc + "',"
                      + MaintenanceOPRTable.Notes + "='" + notes + "'"
                     + " where "
                    + MaintenanceOPRTable.MaintenanceOPRID  + "=" + MaintenanceOPRID_
                    );
                    if (Place == null)
                    {
                        if (MaintenanceOPR_.Place != null) UNStoreMaintenanceItem(MaintenanceOPRID_, MaintenanceOPR_.Place.PlaceID);
                    }
                    else
                    {
                        StoreMaintenanceItem (MaintenanceOPRID_, Place.PlaceID);
                    }
                    DB.AddLog(
                              DatabaseInterface.Log.LogType.UPDATE 
                              , DatabaseInterface.Log.Log_Target.Maintenenace_MaintenenaceOPR 
                              , ""
                            , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_MaintenenaceOPR 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteMaintenanceOPR(uint Maintenanceoprid)
            {
                try
                {
                    MaintenanceOPR MaintenanceOPR_ = GetMaintenancePRINFO_BYID(Maintenanceoprid);
                    if (MaintenanceOPR_ == null) throw new Exception("عملية الصيانة غير موجودة");

                    if (MaintenanceOPR_.Place != null) UNStoreMaintenanceItem(Maintenanceoprid, MaintenanceOPR_.Place.PlaceID);

                    DB.ExecuteSQLCommand("delete from   "
                    + MaintenanceOPRTable.TableName
                    + " where "
                    + MaintenanceOPRTable.MaintenanceOPRID  + "=" + Maintenanceoprid
                    );
                    DB.AddLog(
                              DatabaseInterface.Log.LogType.DELETE 
                              , DatabaseInterface.Log.Log_Target.Maintenenace_MaintenenaceOPR
                              , ""
                            , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE
                            , DatabaseInterface.Log.Log_Target.Maintenenace_MaintenenaceOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool StoreMaintenanceItem(uint maintenenaceoprid, uint placeid)
            {
                try
                {
                    new TradeItemStoreSQL(DB).Store_Item_INPlace(placeid, maintenenaceoprid, TradeItemStore.MAINTENANCE_ITEM_STORE_TYPE, 1, null);

                    return true;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UNStoreMaintenanceItem(uint maintenenaceoprid, uint placeid)
            {
                try
                {
                    new TradeItemStoreSQL(DB).UNStore_Item_INPlace(placeid, maintenenaceoprid, TradeItemStore.MAINTENANCE_ITEM_STORE_TYPE );
                    return true;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
      
            public List <RepairOPR> Get_MaintenanceOPR_RepairOPRList(MaintenanceOPR MaintenanceOPR_)
            {
                List<RepairOPR> List = new List<RepairOPR >();
                try
                {
                    List<MaintenanceFaultReport> faultlist = new MaintenanceFaultSQL(DB).GetMaintenanceOPR_Report_Fault_List(MaintenanceOPR_);
                    RepairOPRSQL RepairOPRSQL_ = new RepairOPRSQL(DB);
                    for (int i=0;i< faultlist.Count;i++)
                    {
                        List.AddRange(RepairOPRSQL_.GetFault_RepairOPR_List(faultlist[i].Fault));
                    }

                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_MaintenanceOPR_RepairOPRList:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }
                return List;
            }

            public List<MaintenanceOPR> GetNOT_Finsh_MaintenanceOPRList()
            {
                try
                {

            List<MaintenanceOPR> MaintenanceOPRlist = new List<MaintenanceOPR>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + MaintenanceOPRTable.MaintenanceOPRID + ","
                     + MaintenanceOPRTable.EntryDate  + ","
                     + MaintenanceOPRTable.ContactID  + ","
                     + MaintenanceOPRTable.ItemID  + ","
                     + MaintenanceOPRTable.ItemSerial  + ","
                     + MaintenanceOPRTable.FaultDesc  + ","
                     + MaintenanceOPRTable.Notes
                     + " from   "
                    + MaintenanceOPRTable.TableName
                    + " where not exists(select * from "
                    +MaintenanceOPR_EndWorkTable.TableName 
                    +" where "
                     + MaintenanceOPR_EndWorkTable.TableName
                     +"."
                     + MaintenanceOPR_EndWorkTable.MaintenanceOPRID 
                     +"="
                     + MaintenanceOPRTable.TableName
                     + "."
                    + MaintenanceOPRTable.MaintenanceOPRID  + ")  order by "
                    + MaintenanceOPRTable.EntryDate
                      );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint maintenanceoprid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime entrydate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        Contact contact = new ContactSQL(DB).GetContactInforBYID(Convert.ToUInt32(t.Rows[i][2].ToString()));
                        Item item = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][3].ToString()));
                        string itemserial = t.Rows[i][4].ToString();
                        string faultdesc = t.Rows[i][5].ToString();
                        string notes = t.Rows[i][6].ToString();
                        TradeStorePlace place = new TradeItemStoreSQL(DB).GetMaintenanceStorePlace(maintenanceoprid);



                        MaintenanceOPRlist.Add(new MaintenanceOPR(maintenanceoprid,entrydate ,contact ,item ,itemserial ,faultdesc
                            ,place,null ,notes ));

                    }
                    return MaintenanceOPRlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_MaintenanceOPR_RepairOPRList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            #region Maintenance_EndWork
            internal static class MaintenanceOPR_EndWorkTable
            {

                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_EndWork";
                public const string MaintenanceOPRID = "MaintenanceOPRID";
                public const string EndWorkDate = "EndWorkDate";
                public const string Repaired = "Repaired";
                public const string Report = "Report";
                public const string DeliveredDate = "DeliveredDate";
                public const string EndwarrantyDate = "EndwarrantyDate";


            }
            public  MaintenanceOPR_EndWork Get_MaintenanceOPR_EndWork_ForMaintenanceOPR(uint maintenanceOPRid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + MaintenanceOPR_EndWorkTable.EndWorkDate + ","
                     + MaintenanceOPR_EndWorkTable.Repaired + ","
                     + MaintenanceOPR_EndWorkTable.Report + ","
                     + MaintenanceOPR_EndWorkTable.DeliveredDate + ","
                     + MaintenanceOPR_EndWorkTable.EndwarrantyDate
                     + " from   "
                    + MaintenanceOPR_EndWorkTable.TableName
                    + " where "
                    + MaintenanceOPR_EndWorkTable.MaintenanceOPRID + "=" + maintenanceOPRid
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime endworkdate = Convert.ToDateTime(t.Rows[0][0]);
                        bool repaired = Convert.ToBoolean(t.Rows[0][1]);
                        string report = t.Rows[0][2].ToString();
                        DateTime? deliverdate, endwarrantydate;
                        try
                        {
                            deliverdate = Convert.ToDateTime(t.Rows[0][3]);
                        }
                        catch
                        {
                            deliverdate = null;
                        }
                        try
                        {
                            endwarrantydate = Convert.ToDateTime(t.Rows[0][4]);
                        }
                        catch
                        {
                            endwarrantydate = null;
                        }
                        return new MaintenanceOPR_EndWork(maintenanceOPRid, endworkdate
                            , repaired, report, deliverdate, endwarrantydate);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_MaintenanceOPR_EndWork_ForMaintenanceOPR" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public bool Create_MaintenanceOPREndWork(uint MaintenanceOPRID,DateTime EndWorkDate,bool Repaired,DateTime?DeliverDate,DateTime?EndWarrantyDate,string Report)
            {
                try
                {

                    DataTable t = DB.GetData(" insert into "
                    + MaintenanceOPR_EndWorkTable.TableName
                    + "("
                    + MaintenanceOPR_EndWorkTable.MaintenanceOPRID + ","
                    + MaintenanceOPR_EndWorkTable.EndWorkDate + ","
                     + MaintenanceOPR_EndWorkTable.Repaired + ","
                     + MaintenanceOPR_EndWorkTable.DeliveredDate + ","
                     + MaintenanceOPR_EndWorkTable.EndwarrantyDate + ","
                     + MaintenanceOPR_EndWorkTable.Report
                    + ")"
                    + "values"
                    + "("
                     + MaintenanceOPRID
                    + ","
                    + "'" + EndWorkDate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    +( Repaired == true ? "1" : "0")
                     + ","
                   + (DeliverDate  == null  ? "null" : "'" +Convert .ToDateTime ( DeliverDate).ToString("yyyy-MM-dd HH:mm:ss") + "'")
                    + ","
                   + (EndWarrantyDate == null ? "null" : "'" + Convert.ToDateTime(EndWarrantyDate).ToString("yyyy-MM-dd HH:mm:ss") + "'")
                      + ","
                     + "'" + Report + "'"
                    + ")"
                     + " SELECT SCOPE_IDENTITY() "
                    );

                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_EndWork 
                            , ""
                          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_EndWork 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_MaintenanceOPREndWork(uint MaintenanceOPRID_, DateTime EndWorkDate, bool Repaired, DateTime? DeliverDate, DateTime? EndWarrantyDate, string Report)
            {

                 try
                {

                    DB.ExecuteSQLCommand("update  "
                    + MaintenanceOPR_EndWorkTable.TableName
                      + " set "
                       + MaintenanceOPR_EndWorkTable.EndWorkDate + "=" + "'" + EndWorkDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + ","
                       + MaintenanceOPR_EndWorkTable.Repaired + "=" + (Repaired == true ? "1" : "0") + ","
                      + MaintenanceOPR_EndWorkTable.DeliveredDate + "=" + (DeliverDate == null ? "null" : "'" + Convert.ToDateTime(DeliverDate).ToString("yyyy-MM-dd HH:mm:ss") + "'") + ","
                      + MaintenanceOPR_EndWorkTable.EndwarrantyDate + "=" + (EndWarrantyDate == null ? "null" : "'" + Convert.ToDateTime(EndWarrantyDate).ToString("yyyy-MM-dd HH:mm:ss") + "'") + ","
                      + MaintenanceOPR_EndWorkTable.Report + "='" + Report + "'"

                     + " where "
                    + MaintenanceOPR_EndWorkTable.MaintenanceOPRID + "=" + MaintenanceOPRID_
                    );

                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_EndWork
                            , ""
                          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_EndWork
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_MaintenanceOPREndWork(uint Maintenanceoprid)
            {
                try
                {
  
                    DB.ExecuteSQLCommand("delete from   "
                    + MaintenanceOPR_EndWorkTable.TableName
                    + " where "
                    + MaintenanceOPR_EndWorkTable.MaintenanceOPRID + "=" + Maintenanceoprid
                    );
                    DB.AddLog(
                                DatabaseInterface.Log.LogType.DELETE 
                                , DatabaseInterface.Log.Log_Target.Maintenenace_EndWork
                                 , ""
                                  , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_EndWork
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            #endregion

        }
        public class MaintenanceAccessorySQL
        {
            DatabaseInterface DB;
            private static class MaintenanceAccessoryTable
            {
                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_Accessory";
                public const string AccessoryID = "AccessoryID";
                public const string MaintenanceOPRID = "MaintenanceOPRID";
                public const string ItemID = "ItemID";
                public const string ItemSerial = "ItemSerial";
                public const string Notes = "Notes";


            }
            public MaintenanceAccessorySQL(DatabaseInterface db)
            {
                DB = db;

            }
            public MaintenanceOPR_Accessory Get_Accessory_INFO_BYID(uint accessoryid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                + MaintenanceAccessoryTable.MaintenanceOPRID  + ","
                + MaintenanceAccessoryTable.ItemID  + ","
                + MaintenanceAccessoryTable.ItemSerial  + ","
                + MaintenanceAccessoryTable.Notes
                + " from   "
                + MaintenanceAccessoryTable.TableName
                + " where "
                + MaintenanceAccessoryTable.AccessoryID  + "=" + accessoryid 
                  );
                if (t.Rows.Count == 1)
                {
                    MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(Convert .ToUInt32 (t.Rows [0][0].ToString ()));
                    Item  Item_ = new ItemSQL (DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                    string itemserial = t.Rows[0][2].ToString();
                    string notes = t.Rows[0][3].ToString();
                    TradeStorePlace place = new TradeItemStoreSQL(DB).GetAccessoryStorePlace(accessoryid);
                    return new MaintenanceOPR_Accessory (accessoryid , MaintenanceOPR_ ,Item_ ,itemserial ,notes ,place );

                }
                else
                    return null;
            }
            public MaintenanceOPR_Accessory  AddAccessory(uint MaintenanceOPRID,uint ItemID,string itemserial,string notes,TradeStorePlace place)
            {
                try
                {


                    DataTable t = DB.GetData(" insert into "
                    + MaintenanceAccessoryTable.TableName
                    + "("
                    + MaintenanceAccessoryTable.MaintenanceOPRID 
                    + ","
                    + MaintenanceAccessoryTable.ItemID 
                    + ","
                    + MaintenanceAccessoryTable.ItemSerial 
                    + ","
                    + MaintenanceAccessoryTable.Notes 
                    + ")"
                    + "values"
                    + "("
                    + MaintenanceOPRID 
                    + ","
                    + ItemID 
                    + ","
                     + "'" + itemserial  + "'"
                    + ","
                     + "'" + notes + "'"
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint accessoryid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    if (place != null) StoreAccessory(accessoryid, place.PlaceID);
   
                    DB.AddLog(
                                DatabaseInterface.Log.LogType.INSERT 
                                , DatabaseInterface.Log.Log_Target.Maintenenace_Accessory 
                                 , ""
                                  , true, "");
                    return Get_Accessory_INFO_BYID(accessoryid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Accessory 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateAccessory(uint accessoryid, uint ItemID, string itemserial, string notes, TradeStorePlace place)
            {
                try
                {
                    MaintenanceOPR_Accessory accessory = Get_Accessory_INFO_BYID(accessoryid);
                    if (accessory == null) throw new Exception("الملحق غير موجود");
                    DB.ExecuteSQLCommand("update  "
                    + MaintenanceAccessoryTable.TableName
                    + " set "
                    + MaintenanceAccessoryTable.ItemID  + "=" + ItemID 
                    + ","
                    + MaintenanceAccessoryTable.ItemSerial  + "='" +itemserial +"'"
                    + ","
                    + MaintenanceAccessoryTable.Notes  + "='" + notes  + "'"

                    + " where "
                    + MaintenanceAccessoryTable.AccessoryID  + "=" + accessoryid 
                    );
                    if (place == null)
                    {
                        if (accessory.Place != null) UNStoreAccessory(accessoryid , accessory.Place.PlaceID);
                    }
                    else
                    {
                        StoreAccessory(accessoryid, place.PlaceID);
                    }
                    DB.AddLog(
                             DatabaseInterface.Log.LogType.UPDATE 
                             , DatabaseInterface.Log.Log_Target.Maintenenace_Accessory 
                             , ""
                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Accessory 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteAccessory(uint accessoryid)
            {
                try
                {
                    MaintenanceOPR_Accessory accessory = Get_Accessory_INFO_BYID(accessoryid);
                    if (accessory == null) throw new Exception("الملحق غير موجود");
                    if (accessory.Place != null) UNStoreAccessory(accessoryid ,accessory .Place .PlaceID );

                    DB.ExecuteSQLCommand("delete from   "
                    + MaintenanceAccessoryTable.TableName
                    + " where "
                    + MaintenanceAccessoryTable.AccessoryID  + "=" + accessoryid
                    );
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Accessory
                            , ""
                          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Accessory
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool StoreAccessory(uint accessoryid,uint placeid)
            {
                try
                {
                    new TradeItemStoreSQL(DB).Store_Item_INPlace (placeid, accessoryid, TradeItemStore.MAINTENANCE_ACCESSORIES_ITEM_STORE_TYPE,1,null );

                    return true;
                }catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UNStoreAccessory(uint accessoryid, uint placeid)
            {
                try
                {
                    new TradeItemStoreSQL(DB).UNStore_Item_INPlace(placeid, accessoryid, TradeItemStore.MAINTENANCE_ACCESSORIES_ITEM_STORE_TYPE);
                    return true;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<MaintenanceOPR_Accessory > GetMaintenanceOPR_Accessories_List(MaintenanceOPR MaintenanceOPR_)
            {
                List<MaintenanceOPR_Accessory> Accessorylist = new List<MaintenanceOPR_Accessory>();

                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + MaintenanceAccessoryTable.AccessoryID  + ","
                    + MaintenanceAccessoryTable.ItemID + ","
                    + MaintenanceAccessoryTable.ItemSerial + ","
                    + MaintenanceAccessoryTable.Notes
                    + " from   "
                    + MaintenanceAccessoryTable.TableName
                    + " where "
                    + MaintenanceAccessoryTable.MaintenanceOPRID  + "=" + MaintenanceOPR_._Operation.OperationID
                      );
                    for(int i=0;i<t.Rows .Count;i++)
                    {
                        uint accessoryid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        Item Item_ = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][1].ToString()));
                        string itemserial = t.Rows[i][2].ToString();
                        string notes = t.Rows[i][3].ToString();
                        TradeStorePlace place = new TradeItemStoreSQL(DB).GetAccessoryStorePlace(accessoryid);
                        Accessorylist.Add ( new MaintenanceOPR_Accessory(accessoryid, MaintenanceOPR_, Item_, itemserial, notes, place));

                    }
                    return Accessorylist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("", "" + ee.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return Accessorylist;
                }
            }

        }
        public class DiagnosticOPRSQL
        {
            DatabaseInterface DB;
            internal static class DiagnosticOPRTable
            {

                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_DiagnosticOPR";
                public const string MaintenanceOPRID = "MaintenanceOPRID";
                public const string ParentDiagnosticOPRID = "ParentDiagnosticOPRID";
                public const string DiagnosticOPRID = "DiagnosticOPRID";
                public const string DiagnosticOPRDate = "DiagnosticOPRDate";
                public const string ItemID = "ItemID";
                public const string Desc = "Description_";
                public const string Location = "Location";
                public const string Normal = "Normal";
                public const string Report = "Report";

            }
            public DiagnosticOPRSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public DiagnosticOPR GetDiagnosticOPRINFO_BYID(uint DiagnosticOPRid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + DiagnosticOPRTable.MaintenanceOPRID + ","
                     + DiagnosticOPRTable.ParentDiagnosticOPRID  + ","
                     + DiagnosticOPRTable.DiagnosticOPRDate + ","
                     + DiagnosticOPRTable.ItemID  + ","
                     + DiagnosticOPRTable.Desc  + ","
                     + DiagnosticOPRTable.Location + ","
                     + DiagnosticOPRTable.Normal  + ","
                     + DiagnosticOPRTable.Report 
                     + " from   "
                    + DiagnosticOPRTable.TableName
                    + " where "
                    + DiagnosticOPRTable.DiagnosticOPRID  + "=" + DiagnosticOPRid
                      );
                    if (t.Rows.Count == 1)
                    {
                        MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
                        uint? ParentDiagnosticID;
                        try
                        {
                            ParentDiagnosticID = Convert.ToUInt32(t.Rows[0][1].ToString());
                        }
                        catch
                        {
                            ParentDiagnosticID = null;
                        }

                        DateTime OprDate = Convert.ToDateTime(t.Rows [0][2].ToString ());
                        Item item;
                        try
                        {
                             item = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[0][3].ToString()));

                        }
                        catch
                        {
                            item = null;
                        }
                        string desc = t.Rows[0][4].ToString();
                        string location = t.Rows[0][5].ToString();

                        bool? normal;
                        try
                        {
                            normal = Convert.ToBoolean(t.Rows[0][6].ToString());
                        }
                        catch
                        {
                            normal = null;
                        }

                        string report = t.Rows[0][7].ToString();


                        return new DiagnosticOPR(MaintenanceOPR_, ParentDiagnosticID, DiagnosticOPRid, OprDate
                            , item, desc, location, normal, report);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetDiagnosticOPRINFO_BYID" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
            public DiagnosticOPR AddDiagnosticOPR(uint MaintenanceOPRID,uint? P_DiagnosticID, DateTime OPRDate
                , Item  item,string desc,string location,bool? normal,string report)
            {
                try
                {
                    string P_DiagnosticID_str = (P_DiagnosticID ==null ?"null":P_DiagnosticID .ToString ());

                    DataTable t = DB.GetData(" insert into "
                    + DiagnosticOPRTable.TableName
                    + "("
                    + DiagnosticOPRTable.MaintenanceOPRID  + ","
                    + DiagnosticOPRTable.ParentDiagnosticOPRID  + ","
                    + DiagnosticOPRTable.DiagnosticOPRDate  + ","
                    + DiagnosticOPRTable.ItemID  + ","
                    + DiagnosticOPRTable.Desc  + ","
                     + DiagnosticOPRTable.Location   + ","
                     + DiagnosticOPRTable.Normal  + ","
                     + DiagnosticOPRTable.Report 
                    + ")"
                    + "values"
                    + "("
                    + MaintenanceOPRID 
                    + ","
                    + P_DiagnosticID_str
                    + ","
                    + "'" + Convert.ToDateTime(OPRDate ).ToString("yyyy-MM-dd HH:mm:ss") + "'"
                      + ","
                     + (item  == null ? "null" : item.ItemID .ToString())
                     + ","
                     + "'"+desc +"'"
                     + ","
                     + "'" + location  + "'"
                     + ","
                     + (normal == null ? "  null " : (normal == true ? "1" : "0"))
                    + ","
                     + "'" + report  + "'"
                    + ")"
                     + " SELECT SCOPE_IDENTITY() "
                    );
                    uint Diagnosticoprid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR
                            , ""
                          , true, "");
                    return GetDiagnosticOPRINFO_BYID(Diagnosticoprid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateDiagnosticOPR(uint DiagnosticOPRID_
               , DateTime OPRDate
                , Item item, string desc, string location, bool? normal, string report)
            {

                try
                {

                    DB.ExecuteSQLCommand("update  "
                    + DiagnosticOPRTable.TableName
                      + " set "
                      + DiagnosticOPRTable.DiagnosticOPRDate + "='" + Convert.ToDateTime(OPRDate).ToString("yyyy-MM-dd HH:mm:ss") + "',"
                      + DiagnosticOPRTable.ItemID + "=" + (item == null ? "null" : item.ItemID.ToString())+","
                      + DiagnosticOPRTable.Desc  + "='" + desc  + "',"
                      + DiagnosticOPRTable.Location  + "='" + location  + "',"
                      + DiagnosticOPRTable.Normal  + "=" + (normal == null ? "  null " : (normal == true ? "1" : "0")) + ","
                      + DiagnosticOPRTable.Report  + "='" + report  + "'"
                     + " where "
                    + DiagnosticOPRTable.DiagnosticOPRID + "=" + DiagnosticOPRID_
                    );
                    DB.AddLog(
                           DatabaseInterface.Log.LogType.UPDATE 
                           , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR
                           , ""
                         , true, "");
                    return true; 
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteDiagnosticOPR(uint DiagnosticOPRid)
            {
                try
                {
                    new MaintenanceTagSQL(DB).Delete_DiagnosticOPR_Tags(DiagnosticOPRid);
                    DB.ExecuteSQLCommand("delete from   "
                    + DiagnosticOPRTable.TableName
                    + " where "
                    + DiagnosticOPRTable.DiagnosticOPRID  + "=" + DiagnosticOPRid
                    );
                    DB.AddLog(
                           DatabaseInterface.Log.LogType.DELETE 
                           , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR
                           , ""
                         , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public List<DiagnosticOPRReport> GetSubDiagnosticOPRReportList(MaintenanceOPR MaintenanceOPR_,DiagnosticOPR  ParentDiagnosticOPR)
            {
                List<DiagnosticOPRReport> list = new List<DiagnosticOPRReport>();
                try
                {
                    string parentid = (ParentDiagnosticOPR == null ? " is null" :"="+ ParentDiagnosticOPR.DiagnosticOPRID.ToString());

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + DiagnosticOPRTable.DiagnosticOPRID + ","
                     + DiagnosticOPRTable.DiagnosticOPRDate + ","
                     + DiagnosticOPRTable.ItemID + ","
                     + DiagnosticOPRTable.Desc + ","
                     + DiagnosticOPRTable.Location + ","
                     + DiagnosticOPRTable.Normal + ","
                     + DiagnosticOPRTable.Report
                     + " from   "
                    + DiagnosticOPRTable.TableName
                    + " where "
                    + DiagnosticOPRTable.MaintenanceOPRID + "=" + MaintenanceOPR_._Operation.OperationID
                     + " and "
                    + DiagnosticOPRTable.ParentDiagnosticOPRID + parentid
                      );
                    uint? p_id;
                    try
                    {
                        p_id = ParentDiagnosticOPR.DiagnosticOPRID;
                        }
                    catch
                    {
                        p_id = null;
                    }
                    MeasureOPRSQL MeasureOPRSQL_ = new MeasureOPRSQL(DB);
                    DiagnosticOPRFileSQL DiagnosticOPRFileSQL_ = new DiagnosticOPRFileSQL(DB);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint diagnosticoprid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime OprDate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        Item item;
                        try
                        {
                            item = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][2].ToString()));

                        }
                        catch
                        {
                            item = null;
                        }
                        string desc = t.Rows[0][3].ToString();
                        string location = t.Rows[0][4].ToString();

                        bool? normal;
                        try
                        {
                            normal = Convert.ToBoolean(t.Rows[0][5].ToString());
                        }
                        catch
                        {
                            normal = null;
                        }

                        string report = t.Rows[0][6].ToString();
                        int tagscount = new MaintenanceTagSQL(DB).GetDiagnosticOPR_TagsCount(diagnosticoprid);
                        list.Add(new DiagnosticOPRReport(new DiagnosticOPR(MaintenanceOPR_, p_id, diagnosticoprid , OprDate
                            , item, desc, location, normal, report), MeasureOPRSQL_.GetDiagnostic_MeasureOPRCount(diagnosticoprid)
                            , DiagnosticOPRFileSQL_.GetDiagnostic_FileCount (diagnosticoprid),GetDiagnosticOPR_SubDiagnosticOPRCount(diagnosticoprid),tagscount));

                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetSubDiagnosticOPRReportList:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list ;
                }

            }
            public List<DiagnosticOPRReport> Get_All_DiagnosticOPRReportList(MaintenanceOPR MaintenanceOPR_)
            {
                List<DiagnosticOPRReport> list = new List<DiagnosticOPRReport>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                     + DiagnosticOPRTable.DiagnosticOPRID + ","
                     + DiagnosticOPRTable.DiagnosticOPRDate + ","
                     + DiagnosticOPRTable.ItemID + ","
                     + DiagnosticOPRTable.Desc + ","
                     + DiagnosticOPRTable.Location + ","
                     + DiagnosticOPRTable.Normal + ","
                     + DiagnosticOPRTable.Report + ","
                      + DiagnosticOPRTable.ParentDiagnosticOPRID 
                     + " from   "
                    + DiagnosticOPRTable.TableName
                    + " where "
                    + DiagnosticOPRTable.MaintenanceOPRID + "=" + MaintenanceOPR_._Operation.OperationID

                      );
               
                    MeasureOPRSQL MeasureOPRSQL_ = new MeasureOPRSQL(DB);
                    DiagnosticOPRFileSQL DiagnosticOPRFileSQL_ = new DiagnosticOPRFileSQL(DB);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint diagnosticoprid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime OprDate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        Item item;
                        try
                        {
                            item = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][2].ToString()));

                        }
                        catch
                        {
                            item = null;
                        }
                        string desc = t.Rows[0][3].ToString();
                        string location = t.Rows[0][4].ToString();

                        bool? normal;
                        try
                        {
                            normal = Convert.ToBoolean(t.Rows[0][5].ToString());
                        }
                        catch
                        {
                            normal = null;
                        }

                        string report = t.Rows[0][6].ToString();
                        uint? p_id;
                        try
                        {
                            p_id = Convert.ToUInt32(t.Rows[i][7]);
                        }
                        catch
                        {
                            p_id = null;
                        }
                        int tagscount = new MaintenanceTagSQL(DB).GetDiagnosticOPR_TagsCount(diagnosticoprid);
                        list.Add(new DiagnosticOPRReport(new DiagnosticOPR(MaintenanceOPR_, p_id, diagnosticoprid, OprDate
                            , item, desc, location, normal, report), MeasureOPRSQL_.GetDiagnostic_MeasureOPRCount(diagnosticoprid)
                            , DiagnosticOPRFileSQL_.GetDiagnostic_FileCount(diagnosticoprid), GetDiagnosticOPR_SubDiagnosticOPRCount(diagnosticoprid), tagscount));

                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetAllDiagnosticOPRReportList:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }

            }
            public int GetDiagnosticOPR_SubDiagnosticOPRCount(uint DiagnosticOPRID_)
            {

                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select count(*) from "
                        + DiagnosticOPRTable.TableName
                         + " where "
                        + DiagnosticOPRTable.ParentDiagnosticOPRID  + "=" + DiagnosticOPRID_
                       );
                    if (t.Rows.Count > 0)
                    {
                        return Convert.ToInt32(t.Rows[0][0].ToString());
                    }
                    else return 0;

                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetDiagnosticOPR_SubDiagnosticOPRCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

            }
        }
        public class MeasureOPRSQL
        {
            public static class MeasureOPRTable
            {
                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_DiagnosticOPR_MeasureOPR";
                public const string DiagnosticOPRID = "DiagnosticOPRID";
                public const string MeasureOPRID = "MeasureOPRID";
                public const string Desc = "Description_";
                public const string Value = "Value";
                public const string MeasureUnit = "MeasureUnit";
                public const string Normal = "Normal";
            }
            DatabaseInterface DB;
            public MeasureOPRSQL(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public MeasureOPR GetMeasureOPRinfo_ByID(uint MeasureOPRid)
            {
                try
                {
                    DataTable t = DB.GetData("select "
                    + MeasureOPRTable.DiagnosticOPRID + ","
                       + MeasureOPRTable.Desc + ","
                        + MeasureOPRTable.Value + ","
                         + MeasureOPRTable.MeasureUnit + ","
                         + MeasureOPRTable.Normal 
                       + " from  "
                       + MeasureOPRTable.TableName
                       + " where "
                       + MeasureOPRTable.MeasureOPRID + "=" + MeasureOPRid);
                    if (t.Rows.Count == 1)
                    {
                        DiagnosticOPR DiagnosticOPR_ = new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
                        string desc = t.Rows[0][1].ToString();
                        double value = Convert.ToDouble(t.Rows [0][2].ToString ());
                        string measureunit = t.Rows[0][3].ToString();
                        bool? normal;
                        try
                        {
                            normal = Convert.ToBoolean(t.Rows[0][4].ToString());
                        }
                        catch
                        {
                            normal = null;
                        }
                        return new MeasureOPR (DiagnosticOPR_,MeasureOPRid,desc,value ,measureunit ,normal );
                    } 
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetMeasureOPRinfo_ByID" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }


            public bool AddMeasureOPR(uint DiagnosticOPRID,string Desc,double Value,string MeasureUnit,bool? normal)
            {
                try
                {

                    DB.ExecuteSQLCommand("insert into  "
                        + MeasureOPRTable.TableName
                        + " ( "
                        + MeasureOPRTable.DiagnosticOPRID + ","
                        + MeasureOPRTable.Desc  + ","
                         + MeasureOPRTable.Value  + ","
                        + MeasureOPRTable.MeasureUnit + ","
                         + MeasureOPRTable.Normal
                        + ")values( "
                        +DiagnosticOPRID + ","
                        + "'" + Desc  + "'" + ","
                        + Value  + ","
                        + "'" + MeasureUnit  + "'" 
                        +(normal ==null ?"null":(normal ==true ?"1":"0"))
                        + ")"
                        );
                    DB.AddLog(
                             DatabaseInterface.Log.LogType.INSERT 
                             , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MeasureOPR 
                             , ""
                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MeasureOPR 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateMeasureOPR(uint MeasureOPRID, string Desc, double Value, string MeasureUnit, bool? normal)
            {
                try
                {
                    DB.ExecuteSQLCommand("update   "
                        + MeasureOPRTable.TableName
                        + " set "
                        + MeasureOPRTable.Desc  + "='" + Desc  + "',"
                         + MeasureOPRTable.Value  + "=" + Value  + ","
                         + MeasureOPRTable.MeasureUnit  + "='" + MeasureUnit +"',"
                         +MeasureOPRTable.Normal +"="+(normal == null ? "null" : (normal == true ? "1" : "0"))
                        + " where "
                        + MeasureOPRTable.MeasureOPRID  + "=" + MeasureOPRID 
                        );
                    DB.AddLog(
                 DatabaseInterface.Log.LogType.UPDATE 
                 , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MeasureOPR
                 , ""
               , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MeasureOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteMeasureOPR(uint MeasureOPRID)
            {
                try
                {


                    DB.ExecuteSQLCommand("Delete from    "
                        + MeasureOPRTable.TableName
                        + " where "
                        + MeasureOPRTable.MeasureOPRID  + "=" + MeasureOPRID 
                        );
                    DB.AddLog(
                DatabaseInterface.Log.LogType.DELETE 
                , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MeasureOPR
                , ""
              , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MeasureOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<MeasureOPR> GetMeasureOPRList(DiagnosticOPR DiagnosticOPR_)
            {
                List<MeasureOPR> list = new List<MeasureOPR>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + MeasureOPRTable.MeasureOPRID  + ","
                           + MeasureOPRTable.Desc  + ","
                             + MeasureOPRTable.Value  + ","
                          + MeasureOPRTable.MeasureUnit + ","
                          + MeasureOPRTable.Normal 
                        + " from "
                        + MeasureOPRTable.TableName
                         + " where "
                        + MeasureOPRTable.DiagnosticOPRID  + "=" + DiagnosticOPR_.DiagnosticOPRID
                       );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint MeasureOPRID = Convert.ToUInt32(t.Rows [i][0].ToString ());
                        string desc = t.Rows[i][1].ToString();
                        double value = Convert.ToDouble(t.Rows[i][2].ToString());
                        string MeasureUnit = t.Rows[i][3].ToString();
                        bool? normal;
                        try
                        {
                            normal = Convert.ToBoolean(t.Rows[i][4].ToString());
                        }
                        catch
                        {
                            normal = null;
                        }
                        MeasureOPR m = new MeasureOPR(DiagnosticOPR_, MeasureOPRID, desc, value, MeasureUnit,normal );
                        list.Add(m);
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetMeasureOPRList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

            }
            public int GetDiagnostic_MeasureOPRCount(uint  DiagnosticOPRID_)
            {

                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select count(*) from "
                        + MeasureOPRTable.TableName
                         + " where "
                        + MeasureOPRTable.DiagnosticOPRID + "=" + DiagnosticOPRID_
                       );
                    if (t.Rows.Count > 0)
                    {
                        return Convert.ToInt32(t.Rows[0][0].ToString());
                    }
                    else return 0;
                    
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetDiagnostic_MeasureOPRCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

            }
            public List<string > GetMeasureUnitList()
            {
                List<string > list = new List<string >();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select Distinct "
                          + MeasureOPRTable.MeasureUnit
                        + " from "
                        + MeasureOPRTable.TableName);

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        string MeasureUnit = t.Rows[i][0].ToString();
                        list.Add(MeasureUnit);
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetMeasureUnitList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

            }
        }
        public class DiagnosticOPRFileSQL
        {
            DatabaseInterface DB;
            public static class DiagnosticOPRFileTable
            {
                public const string TableName = "Item_ItemFiles";
                public const string DiagnosticOPRID = "DiagnosticOPRID";
                public const string FileID = "FileID";
                public const string Item_FileName = "Item_FileName";
                public const string FileDescription = "FileDescription";
                public const string AddDate = "AddDate";
                public const string FileData = "FileData";
            }

            public DiagnosticOPRFileSQL(DatabaseInterface db)
            {
                DB = db;
            }
            public bool AddDiagnosticOPRFile(DiagnosticOPR DiagnosticOPR_, string File_Name, string File_Description, byte[] File_Data)
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = DB.DATABASE_CONNECTION;
                    command.CommandText = @"INSERT INTO Item_ItemFiles (ItemID,Item_FileName,FileDescription,FileData)values(@itemid,@FileName,@FileDescription,@FileData)";

                    command.Parameters.AddWithValue("@DiagnosticOPRID", DiagnosticOPR_.DiagnosticOPRID);
                    command.Parameters.AddWithValue("@FileName", File_Name);
                    command.Parameters.AddWithValue("@FileDescription", File_Description);
                    command.Parameters.AddWithValue("@FileData", File_Data);
                    command.Parameters[0].SqlDbType = SqlDbType.Int;
                    command.Parameters[1].SqlDbType = SqlDbType.Text;
                    command.Parameters[2].SqlDbType = SqlDbType.Text;
                    command.Parameters[3].SqlDbType = SqlDbType.Binary;
                    if (DB.DATABASE_CONNECTION.State == ConnectionState.Closed)
                        DB.DATABASE_CONNECTION.Open();
                    command.ExecuteNonQuery();
                    DB.DATABASE_CONNECTION.Close();
                    DB.AddLog(
                DatabaseInterface.Log.LogType.INSERT
                , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_File
                , ""
              , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_File 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteDiagnosticOPRFile(uint file_id)
            {

                try
                {
                    DB.ExecuteSQLCommand(" delete from  "
                        + DiagnosticOPRFileTable.TableName
                        + " where "
                        + DiagnosticOPRFileTable.FileID + "=" + file_id
                        );
                    DB.AddLog(
               DatabaseInterface.Log.LogType.DELETE 
               , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_File
               , ""
             , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_File
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateDiagnosticOPRFileInfo(uint File_ID, string file_name, string file_description)
            {

                try
                {
                    DB.ExecuteSQLCommand(" update "
                        + DiagnosticOPRFileTable.TableName
                        + " set "
                        + DiagnosticOPRFileTable.Item_FileName + "='" + file_name + "',"
                        + DiagnosticOPRFileTable.FileDescription + "='" + file_description + "'"
                        + " where "
                        + DiagnosticOPRFileTable.FileID + "=" + File_ID
                        );
                    DB.AddLog(
               DatabaseInterface.Log.LogType.UPDATE 
               , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_File
               , ""
             , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_File
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<DiagnosticFile> GetDiagnosticOPRFileList(DiagnosticOPR DiagnosticOPR_)
            {
                List<DiagnosticFile> DiagnosticFileList = new List<DiagnosticFile>();
                try
                {

                    DataTable t = DB.GetData("select "
                    + DiagnosticOPRFileTable.FileID + ","
                    + DiagnosticOPRFileTable.Item_FileName + ","
                    + DiagnosticOPRFileTable.FileDescription + ","
                    + DiagnosticOPRFileTable.AddDate
                    + " from "
                    + DiagnosticOPRFileTable.TableName
                    + " where "
                    + DiagnosticOPRFileTable.DiagnosticOPRID + "=" + DiagnosticOPR_.DiagnosticOPRID
                    );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint fileid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string filename = t.Rows[i][1].ToString();
                        string filedescription = t.Rows[i][2].ToString();
                        DateTime datetime = Convert.ToDateTime(t.Rows[i][3].ToString());
                        long filesize = GetFileSize(fileid);
                        DiagnosticFileList.Add(new DiagnosticFile(DiagnosticOPR_, fileid, filename, filedescription, filesize, datetime));
                    }
                    return DiagnosticFileList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetDiagnosticOPRFileList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return DiagnosticFileList;
                }
            }
            public long GetFileSize(uint fileid)
            {
                try
                {

                    DataTable t = DB.GetData("select datalength("
                    + DiagnosticOPRFileTable.FileData
                    + ") from "
                    + DiagnosticOPRFileTable.TableName
                    + " where "
                    + DiagnosticOPRFileTable.FileID + "=" + fileid
                    );
                    if (t.Rows.Count > 0)
                    {
                        return (long)t.Rows[0][0];

                    }
                    else return -1;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetFileSize" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
            public byte[] GetFileData(uint fileid)
            {
                try
                {

                    DataTable t = DB.GetData("select "
                    + DiagnosticOPRFileTable.FileData
                    + " from "
                    + DiagnosticOPRFileTable.TableName
                    + " where "
                    + DiagnosticOPRFileTable.FileID + "=" + fileid
                    );
                    if (t.Rows.Count > 0)
                    {
                        return (byte[])t.Rows[0][0];

                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetFileData" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public int GetDiagnostic_FileCount(uint DiagnosticOPRID_)
            {

                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select count(*) from "
                        + DiagnosticOPRFileTable.TableName
                         + " where "
                        + DiagnosticOPRFileTable.DiagnosticOPRID + "=" + DiagnosticOPRID_
                       );
                    if (t.Rows.Count > 0)
                    {
                        return Convert.ToInt32(t.Rows[0][0].ToString());
                    }
                    else return 0;

                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetDiagnostic_FileCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

            }
        }
        public class MaintenanceFaultSQL
        {
            DatabaseInterface DB;
            private static class MaintenanceFaultTable
            {
                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_Fault";
                public const string MaintenanceOPRID = "MaintenanceOPRID";
                public const string ItemID = "ItemID";
                public const string FaultID = "FaultID";
                public const string FaultDate = "FaultDate";
                public const string FaultDesc = "FaultDesc";
                public const string FaultReport = "FaultReport";





            }
            public MaintenanceFaultSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public MaintenanceFault  Get_Fault_INFO_BYID(uint faultid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                + MaintenanceFaultTable.MaintenanceOPRID + ","
                + MaintenanceFaultTable.ItemID + ","
                + MaintenanceFaultTable.FaultDesc + ","
                + MaintenanceFaultTable.FaultDate + ","
                + MaintenanceFaultTable.FaultReport 
                + " from   "
                + MaintenanceFaultTable.TableName
                + " where "
                + MaintenanceFaultTable.FaultID + "=" + faultid
                  );
                if (t.Rows.Count == 1)
                {
                    MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
                    Item Item_ = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                    string faultdesc = t.Rows[0][2].ToString();
                    DateTime faultdate =Convert.ToDateTime ( t.Rows[0][3].ToString());
                    string faultreport = t.Rows[0][4].ToString();
                    return new MaintenanceFault ( MaintenanceOPR_, Item_, faultid,  faultdate, faultdesc,faultreport);

                }
                else
                    return null;
            }
            public MaintenanceFault  AddFault(uint MaintenanceOPRID, uint ItemID,  DateTime  faultdate, string faultdesc,string faultreport)
            {
                try
                {
                    DataTable t = DB.GetData(" insert into "
                    + MaintenanceFaultTable.TableName
                    + "("
                    + MaintenanceFaultTable.MaintenanceOPRID
                    + ","
                    + MaintenanceFaultTable.ItemID
                    + ","
                    + MaintenanceFaultTable.FaultDesc 
                    + ","
                    + MaintenanceFaultTable.FaultDate
                         + ","
                    + MaintenanceFaultTable.FaultReport 
                    + ")"
                    + "values"
                    + "("
                    + MaintenanceOPRID
                    + ","
                    + ItemID
                    + ","
                     + "'" + faultdesc + "'"
                    + ","
                     + "'" + faultdate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                      + ","
                     + "'" + faultreport  + "'"
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint faultid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    
                    DB.AddLog(
                  DatabaseInterface.Log.LogType.INSERT
                  , DatabaseInterface.Log.Log_Target.Maintenenace_Fault
                  , ""
                , true, "");
                    return Get_Fault_INFO_BYID(faultid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Fault 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateFault(uint faultid, uint ItemID, DateTime faultdate, string faultdesc,string faultreport)
            {
                try
                {
                    //MaintenanceOPR_Accessory accessory = Get_Accessory_INFO_BYID(accessoryid);
                    //if (accessory == null) throw new Exception("الملحق غير موجود");
                    DB.ExecuteSQLCommand("update  "
                    + MaintenanceFaultTable.TableName
                    + " set "
                    + MaintenanceFaultTable.ItemID + "=" + ItemID
                    + ","
                    + MaintenanceFaultTable.FaultDesc + "='" + faultdesc + "'"
                    +","
                    + MaintenanceFaultTable.FaultReport  + "='" + faultreport  + "'"
                    + ","
                    + MaintenanceFaultTable.FaultDate + "='" + faultdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"

                    + " where "
                    + MaintenanceFaultTable.FaultID + "=" + faultid
                    );
                    DB.AddLog(
                                         DatabaseInterface.Log.LogType.UPDATE 
                                         , DatabaseInterface.Log.Log_Target.Maintenenace_Fault 
                                         , ""
                                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Fault 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteFault(uint faultid)
            {
                try
                {
                    //MaintenanceOPR_Accessory accessory = Get_Accessory_INFO_BYID(accessoryid);
                    //if (accessory == null) throw new Exception("الملحق غير موجود");
                    //if (accessory.Place != null) UNStoreAccessory(accessoryid, accessory.Place.PlaceID);

                    new MaintenanceTagSQL(DB).Delete_Fault_Tags(faultid);

                    DB.ExecuteSQLCommand("delete from   "
                    + MaintenanceFaultTable.TableName
                    + " where "
                    + MaintenanceFaultTable.FaultID + "=" + faultid 
                    );
                    DB.AddLog(
                                         DatabaseInterface.Log.LogType.DELETE 
                                         , DatabaseInterface.Log.Log_Target.Maintenenace_Fault
                                         , ""
                                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Fault
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

           public uint GetFault_RepairOPR_Count(uint faultid)
            {
                //try
                //{
                //    DataTable t = new DataTable();
                //    t = DB.GetData("select count (*)"
                //    + MaintenanceFaultTable.MaintenanceOPRID + ","
                //    + MaintenanceFaultTable.ItemID + ","
                //    + MaintenanceFaultTable.FaultDesc + ","
                //    + MaintenanceFaultTable.FaultDate
                //    + " from   "
                //    + MaintenanceFaultTable.TableName
                //    + " where "
                //    + MaintenanceFaultTable.FaultID + "=" + faultid
                //      );
                //    if (t.Rows.Count == 1)
                //    {
                //        MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
                //        Item Item_ = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                //        string faultdesc = t.Rows[0][2].ToString();
                //        DateTime faultdate = Convert.ToDateTime(t.Rows[0][3].ToString());
                //        return new MaintenanceFault(MaintenanceOPR_, Item_, faultid, faultdesc, faultdate);

                //    }
                //    else
                //        return null;
                //}
                //catch
                //{

                //}
                return 0;
            }
            public List<MaintenanceFaultReport> GetMaintenanceOPR_Report_Fault_List(MaintenanceOPR MaintenanceOPR_)
            {
                List<MaintenanceFaultReport> FaultReportlist = new List<MaintenanceFaultReport>();

                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + MaintenanceFaultTable.FaultID + ","
                    + MaintenanceFaultTable.ItemID + ","
                    + MaintenanceFaultTable.FaultDesc + ","
                    + MaintenanceFaultTable.FaultDate + ","
                    + MaintenanceFaultTable.FaultReport 
                    + " from   "
                    + MaintenanceFaultTable.TableName
                    + " where "
                    + MaintenanceFaultTable.MaintenanceOPRID + "=" + MaintenanceOPR_._Operation.OperationID
                      );
                    RepairOPRSQL RepairOPRSQL_ = new RepairOPRSQL(DB);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint faultid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        Item Item_ = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][1].ToString()));
                        string faultdesc = t.Rows[i][2].ToString();
                        DateTime  faultdate =Convert .ToDateTime ( t.Rows[i][3].ToString());
                        string faultreport = t.Rows[i][4].ToString();
                        int repairopr_count = RepairOPRSQL_.GetFault_RepairOPR_Count(faultid);
                        int Affictive_repairopr_count = RepairOPRSQL_.GetFault_Affictive_RepairOPR_Count(faultid);
                        int tagscount = new MaintenanceTagSQL (DB).GetFault_MaintenanceTagCount(faultid);
                        FaultReportlist.Add(new MaintenanceFaultReport ( new MaintenanceFault (MaintenanceOPR_, Item_, faultid, faultdate, faultdesc,faultreport  ),repairopr_count,Affictive_repairopr_count , tagscount));
                        
                    }
                    return FaultReportlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetMaintenanceOPR_Report_Fault_List" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return FaultReportlist;
                }
            }
            public List<MaintenanceFaultReport> GetItem_Fault_List(Item  Item_)
            {
                List<MaintenanceFaultReport> FaultReportlist = new List<MaintenanceFaultReport>();

                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + MaintenanceFaultTable.FaultID + ","
                    + MaintenanceFaultTable.MaintenanceOPRID + ","
                    + MaintenanceFaultTable.FaultDesc + ","
                    + MaintenanceFaultTable.FaultDate + ","
                    + MaintenanceFaultTable.FaultReport 
                    + " from   "
                    + MaintenanceFaultTable.TableName
                    + " where "
                    + MaintenanceFaultTable.ItemID + "=" + Item_.ItemID
                      );
                    RepairOPRSQL RepairOPRSQL_ = new RepairOPRSQL(DB);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint faultid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        MaintenanceOPR MaintenanceOPR_ = new MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(Convert.ToUInt32(t.Rows[i][1].ToString()));
                        string faultdesc = t.Rows[i][2].ToString();
                        DateTime faultdate = Convert.ToDateTime(t.Rows[i][3].ToString());
                        string faultreport = t.Rows[i][4].ToString();

                        int repairopr_count = RepairOPRSQL_.GetFault_RepairOPR_Count(faultid);
                        int Affictive_repairopr_count = RepairOPRSQL_.GetFault_Affictive_RepairOPR_Count(faultid);
                        FaultReportlist.Add(new MaintenanceFaultReport(new MaintenanceFault(MaintenanceOPR_, Item_, faultid, faultdate, faultdesc,faultreport  ), repairopr_count, Affictive_repairopr_count, 0));

                    }
                    return FaultReportlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItem_Fault_List" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return FaultReportlist;
                }
            }
            public List<string > GetItem_FaultDescList(Item Item_)
            {
                List<string> FaultDesclist = new List<string>();

                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select distinct "
                    + MaintenanceFaultTable.FaultDesc 
                    + " from   "
                    + MaintenanceFaultTable.TableName
                    + " where "
                    + MaintenanceFaultTable.ItemID + "=" + Item_.ItemID
                      );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        string faultdesc = t.Rows[i][0].ToString();
                        FaultDesclist.Add( faultdesc);

                    }
                    return FaultDesclist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetItem_FaultDescList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return FaultDesclist;
                }
            }
        }
        public class RepairOPRSQL
        {
            DatabaseInterface DB;
            private static class RepairOPRTable
            {
                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_Fault_RepairOPR";
                public const string FaultID = "FaultID";
                public const string RepairOPRID = "RepairOPRID";
                public const string RepairOPRDate = "RepairOPRDate";
   
                public const string RepairOPRDesc = "RepairOPRDesc";
                public const string RepairOPRReport = "RepairOPRReport";
                public const string FaultRepair = "FaultRepair";


            }
            public RepairOPRSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public RepairOPR  Get_RepairOPR_INFO_BYID(uint repairopr_id)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + RepairOPRTable.FaultID + ","
                    + RepairOPRTable.RepairOPRDate + ","
                    + RepairOPRTable.RepairOPRDesc + ","
                    + RepairOPRTable.RepairOPRReport + ","
                    + RepairOPRTable.FaultRepair
                    + " from   "
                    + RepairOPRTable.TableName
                    + " where "
                    + RepairOPRTable.RepairOPRID + "=" + repairopr_id
                      );
                    if (t.Rows.Count == 1)
                    {
                        MaintenanceFault MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
                        DateTime repairopr_date = Convert.ToDateTime(t.Rows[0][1].ToString());
                        string repairdesc = t.Rows[0][2].ToString();
                        string repairreport = t.Rows[0][3].ToString();
                        bool faultrepair = Convert.ToBoolean(t.Rows[0][4].ToString());
                        int installeditems_count = new ItemOUTSQL(DB).GetItemsOUT_Count(new Operation(Operation.REPAIROPR, repairopr_id));
                        return new RepairOPR(repairopr_id, MaintenanceFault_, repairopr_date,
                            repairdesc, repairreport, faultrepair, installeditems_count,0);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_RepairOPR_INFO_BYID " + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }             
            }
            public RepairOPR AddRepairOPR(uint faultid, DateTime repairopr_date, string  repairdesc, string repairreport,bool faultrepair)
            {
                try
                {


                    DataTable t = DB.GetData(" insert into "
                    + RepairOPRTable.TableName
                    + "("
                    + RepairOPRTable.FaultID
                    + ","
                    + RepairOPRTable.RepairOPRDate
                    + ","
                    + RepairOPRTable.RepairOPRDesc
                    + ","
                    + RepairOPRTable.RepairOPRReport
                     + ","
                    + RepairOPRTable.FaultRepair
                    + ")"
                    + "values"
                    + "("
                    + faultid
                     + ","
                     + "'" + repairopr_date.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'" + repairdesc  + "'"
                    + ","
                    + "'" + repairreport + "'"
                    + ","
                    + (faultrepair ==true ?"1":"0")
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint repairoprid = Convert.ToUInt32(t.Rows[0][0].ToString());
                   
                    DB.AddLog(
                                 DatabaseInterface.Log.LogType.INSERT 
                                 , DatabaseInterface.Log.Log_Target.Maintenenace_Fault_RepairOPR
                                 , ""
                                                           , true, "");
                    return Get_RepairOPR_INFO_BYID(repairoprid); 
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Fault_RepairOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateRepairOPR(uint repairopr_id, DateTime repairoprdate, string  repairdesc, string repairreport,bool faultrepair)
            {
                try
                {

                    DB.ExecuteSQLCommand("update  "
                    + RepairOPRTable.TableName
                    + " set "
                    + RepairOPRTable.RepairOPRDate + "='" + repairoprdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + RepairOPRTable.RepairOPRDesc + "='" + repairdesc + "'"
                    + ","
                   + RepairOPRTable.RepairOPRReport + "='" + repairreport + "'"
                    + ","
                    + RepairOPRTable.FaultRepair+"="+(faultrepair ==true ?"1":"0") 
                    + " where "
                    + RepairOPRTable.RepairOPRID + "=" + repairopr_id
                    );

                    DB.AddLog(
                                DatabaseInterface.Log.LogType.UPDATE 
                                , DatabaseInterface.Log.Log_Target.Maintenenace_Fault_RepairOPR
                                , ""
                                                          , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Fault_RepairOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteRepairOPR(uint RepairOPRid)
            {
                try
                {
                    //MaintenanceOPR_Accessory accessory = Get_Accessory_INFO_BYID(accessoryid);
                    //if (accessory == null) throw new Exception("الملحق غير موجود");
                    //if (accessory.Place != null) UNStoreAccessory(accessoryid, accessory.Place.PlaceID);

                    DB.ExecuteSQLCommand("delete from   "
                    + RepairOPRTable.TableName
                    + " where "
                    + RepairOPRTable.FaultID + "=" + RepairOPRid
                    );
                    DB.AddLog(
                                DatabaseInterface.Log.LogType.DELETE 
                                , DatabaseInterface.Log.Log_Target.Maintenenace_Fault_RepairOPR
                                , ""
                                , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Fault_RepairOPR
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }

            public int GetFault_RepairOPR_Count(uint faultid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select count (*) from   "
                    + RepairOPRTable.TableName
                    + " where "
                    + RepairOPRTable.FaultID + "=" + faultid
                      );
                    return Convert.ToInt32(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetFault_RepairOPR_Count " + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }

            }
            public int GetFault_Affictive_RepairOPR_Count(uint faultid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select count (*) from   "
                    + RepairOPRTable.TableName
                    + " where "
                    + RepairOPRTable.FaultID + "=" + faultid
                    + " and "
                    + RepairOPRTable.FaultRepair  + "=" + "1"
                      );
                    return Convert.ToInt32(t.Rows[0][0].ToString());
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetFault_Affictive_RepairOPR_Count " + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
            public List<RepairOPR> GetFault_RepairOPR_List(MaintenanceFault Fault_)
            {
                List<RepairOPR> RepairOPRlist = new List<RepairOPR>();

                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + RepairOPRTable.RepairOPRID + ","
                    + RepairOPRTable.RepairOPRDate + ","
                    + RepairOPRTable.RepairOPRDesc + ","
                    + RepairOPRTable.RepairOPRReport + ","
                    + RepairOPRTable.FaultRepair
                    + " from   "
                    + RepairOPRTable.TableName
                    + " where "
                    + RepairOPRTable.FaultID + "=" + Fault_.FaultID
                      );
                    for(int i=0;i<t.Rows .Count;i++)
                    {
                        uint  repairoprid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime repairopr_date = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string repairdesc = t.Rows[i][2].ToString();
                        string repairreport = t.Rows[i][3].ToString();
                        bool faultrepair = Convert.ToBoolean(t.Rows[i][4].ToString());
                        int installeditems_count = new ItemOUTSQL(DB).GetItemsOUT_Count(new Operation(Operation.REPAIROPR, repairoprid));
                        RepairOPRlist.Add ( new RepairOPR(repairoprid, Fault_, repairopr_date,
                            repairdesc, repairreport, faultrepair, installeditems_count,0));

                    }
                    return RepairOPRlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return RepairOPRlist;
                }
            }
           
        }
        public class MissedFaultItemSQL
        {
            DatabaseInterface DB;
            private static class Missed_Fault_Item_Table
            {
                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_DiagnosticOPR_MissedFaultItem";
                public const string ID = "MissedFaultID";
                public const string Type = "Type";
                public const string DiagnosticOPRID = "DiagnosticOPRID";
                public const string ItemID = "ItemID";
                public const string Location = "Location";
                public const string Notes = "Notes";



            }
  
            public MissedFaultItemSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public Missed_Fault_Item  GetMissedFaultItem_INFO_BYID(uint id)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                + Missed_Fault_Item_Table.DiagnosticOPRID + ","
                + Missed_Fault_Item_Table.ItemID + ","
                + Missed_Fault_Item_Table.Type   + ","
                + Missed_Fault_Item_Table.Location + ","
                 + Missed_Fault_Item_Table.Notes 
                + " from   "
                + Missed_Fault_Item_Table.TableName
                + " where "
                + Missed_Fault_Item_Table.ID  + "=" + id
                  );
                if (t.Rows.Count == 1)
                {
                    DiagnosticOPR DiagnosticOPR_ = new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID ( Convert.ToUInt32(t.Rows[0][0].ToString()));
                    Item Item_ = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                    bool type = Convert.ToBoolean(t.Rows[0][2].ToString());
                    string location = t.Rows[0][3].ToString();
                    string notes = t.Rows[0][4].ToString();
                    int tags_count = new MaintenanceTagSQL(DB).GetMissedFaultItem_MaintenanceTagCount(id);
                    return new Missed_Fault_Item (id,type , DiagnosticOPR_, Item_, location , notes , tags_count);

                }
                else
                    return null;
            }
            public Missed_Fault_Item AddMissedFaultItem(uint DiagnosticOPRID, uint ItemID,bool type, string location, string  notes)
            {
                try
                {
                    DataTable t = DB.GetData(" insert into "
                    + Missed_Fault_Item_Table.TableName
                    + "("
                    + Missed_Fault_Item_Table.DiagnosticOPRID 
                    + ","
                    + Missed_Fault_Item_Table.ItemID
                    + ","
                    + Missed_Fault_Item_Table.Type
                    + ","
                    + Missed_Fault_Item_Table.Location
                     + ","
                    + Missed_Fault_Item_Table.Notes
                    + ")"
                    + "values"
                    + "("
                    + DiagnosticOPRID
                    + ","
                    + ItemID
                    + ","
                    + (type == true ? "1" : "0")
                    + ","
                     + "'" + location  + "'"
                    + ","
                     + "'" + notes  + "'"
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint id = Convert.ToUInt32(t.Rows[0][0].ToString());
                    
                    DB.AddLog(
                                DatabaseInterface.Log.LogType.INSERT
                                , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MissedFaultItem 
                                , ""
                                                          , true, "");
                    return GetMissedFaultItem_INFO_BYID(id);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MissedFaultItem 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }
            public bool UpdateMissed_Fault_Item(uint id, uint ItemID, bool type,string location, string  notes)
            {
                try
                {
                    //MaintenanceOPR_Accessory accessory = Get_Accessory_INFO_BYID(accessoryid);
                    //if (accessory == null) throw new Exception("الملحق غير موجود");
                    DB.ExecuteSQLCommand("update  "
                    + Missed_Fault_Item_Table.TableName
                    + " set "
                    + Missed_Fault_Item_Table.ItemID + "=" + ItemID
                     + ","
                    + Missed_Fault_Item_Table.Type  + "="+(type ==true ?"1":"0")
                    + ","
                    + Missed_Fault_Item_Table.Location  + "='" + location  + "'"
                    + ","
                    + Missed_Fault_Item_Table.Notes  + "='" + notes  + "'"

                    + " where "
                    + Missed_Fault_Item_Table.ID  + "=" + id 
                    );

                    DB.AddLog(
                                DatabaseInterface.Log.LogType.UPDATE 
                                , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MissedFaultItem
                                , ""
                                                          , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MissedFaultItem
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteMissed_Fault_Item(uint id)
            {
                try
                {
                    //MaintenanceOPR_Accessory accessory = Get_Accessory_INFO_BYID(accessoryid);
                    //if (accessory == null) throw new Exception("الملحق غير موجود");
                    //if (accessory.Place != null) UNStoreAccessory(accessoryid, accessory.Place.PlaceID);
                    new MaintenanceTagSQL(DB).Delete_MissedFault_Tags(id);

                    DB.ExecuteSQLCommand("delete from   "
                    + Missed_Fault_Item_Table.TableName
                    + " where "
                    + Missed_Fault_Item_Table.ID  + "=" + id
                    );
                    DB.AddLog(
                                 DatabaseInterface.Log.LogType.DELETE 
                                 , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MissedFaultItem
                                 , ""
                                                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_DiagnosticOPR_MissedFaultItem
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

            
            public List<Missed_Fault_Item> DiagnosticOPR_GetMissed_Fault_Item_List(DiagnosticOPR DiagnosticOPR_)
            {
                List<Missed_Fault_Item> Missed_Fault_Itemlist = new List<Missed_Fault_Item>();

                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + Missed_Fault_Item_Table.ID  + ","
                    + Missed_Fault_Item_Table.Type  + ","
                    + Missed_Fault_Item_Table.ItemID + ","
                    + Missed_Fault_Item_Table.Location  + ","
                    + Missed_Fault_Item_Table.Notes 
                    + " from   "
                    + Missed_Fault_Item_Table.TableName
                    + " where "
                    + Missed_Fault_Item_Table.DiagnosticOPRID   + "=" + DiagnosticOPR_.DiagnosticOPRID
                      );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint id = Convert.ToUInt32(t.Rows[i][0].ToString());
                        bool type = Convert.ToBoolean(t.Rows[i][1].ToString());
                        Item Item_ = new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][2].ToString()));
                        string location = t.Rows[i][3].ToString();
                        string  notes = t.Rows[i][4].ToString();
                        int tags_count = new MaintenanceTagSQL (DB).GetMissedFaultItem_MaintenanceTagCount(id);
                        Missed_Fault_Itemlist.Add(new Missed_Fault_Item (id,type, DiagnosticOPR_, Item_, location , notes , tags_count ));

                    }
                    return Missed_Fault_Itemlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("DiagnosticOPR_GetMissed_Fault_Item_List:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return Missed_Fault_Itemlist;
                }
            }
            public List<Missed_Fault_Item> MaintenanceOPR_GetMissed_Fault_Item_List(MaintenanceOPR MaintenanceOPR_)
            {
                List<Missed_Fault_Item> Missed_Fault_Itemlist = new List<Missed_Fault_Item>();

                try
                {
                    List<DiagnosticOPRReport> DiagnosticOPRList = new DiagnosticOPRSQL(DB).Get_All_DiagnosticOPRReportList(MaintenanceOPR_);
                    for (int i = 0; i < DiagnosticOPRList.Count; i++)
                        Missed_Fault_Itemlist.AddRange(DiagnosticOPR_GetMissed_Fault_Item_List(DiagnosticOPRList[i]._DiagnosticOPR));
                    return Missed_Fault_Itemlist;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("MaintenanceOPR_GetMissed_Fault_Item_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return Missed_Fault_Itemlist;
                }
            }

        }
        public class MaintenanceTagSQL
        {
            public static class MaintenanceTagTable
            {
                public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_Tag";
                public const string TagID = "TagID";
                public const string FaultID = "FaultID";
                public const string DiagnosticOPRID = "DiagnosticOPRID";
                public const string MissedFaultItemID = "MissedFaultItemID";
                public const string TagINFO = "TagINFO";
            }
            //public static class DiagnosticOPR_Fault_TagTable
            //{
            //    public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_DiagnosticOPR_Fault_Link";
            //    public const string TagID = "TagID";
            //    public const string DiagnosticOPRID = "DiagnosticOPRID";
            //    public const string FaultID = "FaultID";
            //    public const string TagINFO = "TagINFO";
            //}
            //public static class DiagnosticOPR_MissedFaultItem_TagTable
            //{
            //    public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_DiagnosticOPR_MissedFaultItem_Link";
            //    public const string TagID = "TagID";
            //    public const string DiagnosticOPRID = "DiagnosticOPRID";
            //    public const string MissedFaultID = "MissedFaultID";
            //    public const string TagINFO = "TagINFO";
            //}
            //public static class Fault_MissedFaultItem_TagTable
            //{
            //    public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_Fault_MissedFaultItem_Link";
            //    public const string TagID = "TagID";
            //    public const string MissedFaultID = "MissedFaultID";
            //    public const string FaultID = "FaultID";
            //    public const string TagINFO = "TagINFO";
            //}
            DatabaseInterface DB;
            public MaintenanceTagSQL(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public MaintenanceTag GetMaintenanceTaginfo_ByID(uint Tagid)
            {
                try
                {
                    DataTable t = DB.GetData("select "
                    + MaintenanceTagTable.FaultID  + ","
                       + MaintenanceTagTable.DiagnosticOPRID   + ","
                        + MaintenanceTagTable.MissedFaultItemID  + ","
                         + MaintenanceTagTable.TagINFO
                       + " from  "
                       + MaintenanceTagTable.TableName
                       + " where "
                       + MaintenanceTagTable.TagID + "=" + Tagid);
                    if (t.Rows.Count == 1)
                    {
                        MaintenanceFault MaintenanceFault_;
                        try
                        {
                             MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(Convert.ToUInt32(t.Rows[0][0]));
                        }
                        catch
                        {
                            MaintenanceFault_ = null;
                        }
                        DiagnosticOPR DiagnosticOPR_;
                        try
                        {
                            DiagnosticOPR_ = new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID (Convert.ToUInt32(t.Rows[0][1]));
                        }
                        catch
                        {
                            DiagnosticOPR_ = null;
                        }
                        Missed_Fault_Item Missed_Fault_Item_;
                        try
                        {
                            Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID(Convert.ToUInt32(t.Rows[0][2]));
                        }
                        catch
                        {
                            Missed_Fault_Item_ = null;
                        }
                        string taginfo = t.Rows[0][3].ToString();
                        return new MaintenanceTag(Tagid, taginfo, MaintenanceFault_, DiagnosticOPR_, Missed_Fault_Item_);
                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetMaintenanceTaginfo_ByID" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
            public bool CreateMaintenanceTag(string taginfo,MaintenanceFault MaintenanceFault_,DiagnosticOPR DiagnosticOPR_, Missed_Fault_Item Missed_Fault_Item_)
            {
                try
                {


                    DB.ExecuteSQLCommand("insert into  "
                        + MaintenanceTagTable.TableName
                        + " ( "
                        + MaintenanceTagTable.FaultID  + ","
                        + MaintenanceTagTable.DiagnosticOPRID   + ","
                         + MaintenanceTagTable.MissedFaultItemID   + ","
                        + MaintenanceTagTable.TagINFO
                        + ")values( "
                        + (MaintenanceFault_ ==null?"null": MaintenanceFault_.FaultID.ToString ()) + ","
                          + (DiagnosticOPR_ == null ? "null": DiagnosticOPR_.DiagnosticOPRID .ToString()) +","
                         +(Missed_Fault_Item_ == null ? "null": Missed_Fault_Item_.ID .ToString()) +","
                        + "'" + taginfo + "'"
                        + ")"
                        );
                    DB.AddLog(
                                DatabaseInterface.Log.LogType.INSERT 
                                , DatabaseInterface.Log.Log_Target.Maintenenace_Tag 
                                , ""
                                                          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Tag 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateMaintenanceTag(uint TagID, string INFO)
            {
                try
                {
                    DB.ExecuteSQLCommand("update   "
                        + MaintenanceTagTable.TableName
                        + " set "
                        + MaintenanceTagTable.TagINFO + "='" + INFO + "',"
                        + " where "
                        + MaintenanceTagTable.TagID + "=" + TagID
                        );
                    DB.AddLog(
                                 DatabaseInterface.Log.LogType.UPDATE 
                                 , DatabaseInterface.Log.Log_Target.Maintenenace_Tag
                                 , ""
                                                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Tag
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteMaintenanceTag(uint MaintenanceTagID)
            {
                try
                {


                    DB.ExecuteSQLCommand("Delete from    "
                        + MaintenanceTagTable.TableName
                        + " where "
                        + MaintenanceTagTable.TagID + "=" + MaintenanceTagID
                        );
                    DB.AddLog(
                                DatabaseInterface.Log.LogType.DELETE 
                                , DatabaseInterface.Log.Log_Target.Maintenenace_Tag
                                , ""
                                                          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Maintenenace_Tag
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<MaintenanceTag> Get_DiagnosticOPR_Tag_List(DiagnosticOPR DiagnosticOPR_)
            {
                List<MaintenanceTag> list = new List<MaintenanceTag>();
                try
                {
                    DataTable t = DB.GetData("select "
                            + MaintenanceTagTable.TagID  + ","
                     + MaintenanceTagTable.FaultID + ","
                         + MaintenanceTagTable.MissedFaultItemID + ","
                          + MaintenanceTagTable.TagINFO
                        + " from  "
                        + MaintenanceTagTable.TableName
                        + " where "
                       + MaintenanceTagTable.DiagnosticOPRID  + "=" + DiagnosticOPR_ .DiagnosticOPRID);
                    for (int i=0;i<t.Rows .Count;i++)
                    {
                        uint tagid = Convert.ToUInt32(Convert.ToUInt32(t.Rows[i][0]));
                        MaintenanceFault MaintenanceFault_;
                        try
                        {
                            MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(Convert.ToUInt32(t.Rows[i][1]));
                        }
                        catch
                        {
                            MaintenanceFault_ = null;
                        }
                        
                        Missed_Fault_Item Missed_Fault_Item_;
                        try
                        {
                            Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID(Convert.ToUInt32(t.Rows[i][2]));
                        }
                        catch
                        {
                            Missed_Fault_Item_ = null;
                        }
                        string taginfo = t.Rows[0][3].ToString();
                        list.Add(new MaintenanceTag (tagid ,taginfo ,MaintenanceFault_,DiagnosticOPR_,Missed_Fault_Item_));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_DiagnosticOPR_Tag_List" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

            }
            public List<MaintenanceTag> Get_Fault_Tag_List(MaintenanceFault MaintenanceFault_)
            {
                List<MaintenanceTag> list = new List<MaintenanceTag>();
                try
                {
                    DataTable t = DB.GetData("select "
                            + MaintenanceTagTable.TagID + ","
                     + MaintenanceTagTable.DiagnosticOPRID  + ","
                         + MaintenanceTagTable.MissedFaultItemID + ","
                          + MaintenanceTagTable.TagINFO
                        + " from  "
                        + MaintenanceTagTable.TableName
                        + " where "
                       + MaintenanceTagTable.FaultID  + "=" + MaintenanceFault_.FaultID );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint tagid = Convert.ToUInt32(Convert.ToUInt32(t.Rows[i][0]));
                        DiagnosticOPR DiagnosticOPR_;
                        try
                        {
                            DiagnosticOPR_ = new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID (Convert.ToUInt32(t.Rows[i][1]));
                        }
                        catch
                        {
                            DiagnosticOPR_ = null;
                        }

                        Missed_Fault_Item Missed_Fault_Item_;
                        try
                        {
                            Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID(Convert.ToUInt32(t.Rows[i][2]));
                        }
                        catch
                        {
                            Missed_Fault_Item_ = null;
                        }
                        string taginfo = t.Rows[0][3].ToString();
                        list.Add(new MaintenanceTag(tagid, taginfo, MaintenanceFault_, DiagnosticOPR_, Missed_Fault_Item_));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_DiagnosticOPR_Tag_List" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

            }
            public List<MaintenanceTag> GetMissedFaultItem_TagList(Missed_Fault_Item Missed_Fault_Item_)
            {
                List<MaintenanceTag> list = new List<MaintenanceTag>();
                try
                {
                    DataTable t = DB.GetData("select "
                      + MaintenanceTagTable.TagID + ","
                     + MaintenanceTagTable.FaultID  + ","
                     + MaintenanceTagTable.DiagnosticOPRID + ","

                          + MaintenanceTagTable.TagINFO
                        + " from  "
                        + MaintenanceTagTable.TableName
                        + " where "
                       + MaintenanceTagTable.MissedFaultItemID  + "=" + Missed_Fault_Item_.ID);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint tagid = Convert.ToUInt32(Convert.ToUInt32(t.Rows[i][0]));
                        MaintenanceFault MaintenanceFault_;
                        try
                        {
                            MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(Convert.ToUInt32(t.Rows[i][1]));
                        }
                        catch
                        {
                            MaintenanceFault_ = null;
                        }
                        DiagnosticOPR DiagnosticOPR_;
                        try
                        {
                            DiagnosticOPR_ = new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID(Convert.ToUInt32(t.Rows[i][2]));
                        }
                        catch
                        {
                            DiagnosticOPR_ = null;
                        }

                        
                        string taginfo = t.Rows[0][3].ToString();
                        list.Add(new MaintenanceTag(tagid, taginfo, MaintenanceFault_, DiagnosticOPR_, Missed_Fault_Item_));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Get_DiagnosticOPR_Tag_List" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

            }
            public int GetDiagnosticOPR_TagsCount(uint DiagnosticOPRID_)
            {

                try
                {
                    DataTable t = DB.GetData("select count (*)   from  "
                    + MaintenanceTagTable.TableName
                    + " where "
                   + MaintenanceTagTable.DiagnosticOPRID  + "=" +  DiagnosticOPRID_);
                    return Convert.ToInt32(t.Rows [0][0]);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetDiagnosticOPR_TagsCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

            }
       
         
            public int GetMissedFaultItem_MaintenanceTagCount(uint ID)
            {
                try
                {
                    DataTable t = DB.GetData("select count (*)   from  "
                    + MaintenanceTagTable.TableName
                    + " where "
                   + MaintenanceTagTable.MissedFaultItemID  + "=" + ID );
                    return Convert.ToInt32(t.Rows[0][0]);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetMissedFaultItem_MaintenanceTagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            public int GetFault_MaintenanceTagCount(uint FaultID)
            {
                try
                {
                    DataTable t = DB.GetData("select count (*)   from  "
                    + MaintenanceTagTable.TableName
                    + " where "
                   + MaintenanceTagTable.FaultID  + "=" + FaultID);
                    return Convert.ToInt32(t.Rows[0][0]);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetFault_MaintenanceTagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            public bool  Delete_Fault_Tags(uint FaultID)
            {
                try
                {
                    DataTable t = DB.GetData("delete   from  "
                    + MaintenanceTagTable.TableName
                    + " where "
                   + MaintenanceTagTable.FaultID + "=" + FaultID);
                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Delete_Fault_Tags" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool Delete_DiagnosticOPR_Tags(uint diagnosticoprID)
            {
                try
                {
                    DataTable t = DB.GetData("delete   from  "
                    + MaintenanceTagTable.TableName
                    + " where "
                   + MaintenanceTagTable.DiagnosticOPRID  + "=" + diagnosticoprID);
                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Delete_DiagnosticOPR_Tags" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_MissedFault_Tags(uint ID)
            {
                try
                {
                    DataTable t = DB.GetData("delete   from  "
                    + MaintenanceTagTable.TableName
                    + " where "
                   + MaintenanceTagTable.MissedFaultItemID  + "=" + ID);
                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Delete_MissedFault_Tags" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


        }
        //public class DiagnosticOPR_Fault_TagSQL
        //{
          
        //    public static class DiagnosticOPR_Fault_TagTable
        //    {
        //        public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_DiagnosticOPR_Fault_Link";
        //        public const string TagID = "TagID";
        //        public const string DiagnosticOPRID = "DiagnosticOPRID";
        //        public const string FaultID = "FaultID";
        //        public const string TagINFO = "TagINFO";
        //    }
           
        //    DatabaseInterface DB;
        //    public DiagnosticOPR_Fault_TagSQL(DatabaseInterface DB_)
        //    {
        //        DB = DB_;
        //    }
        //    public DiagnosticOPR_Fault_Tag Get_DiagnosticOPR_Fault_Tag_info_ByID(uint Tagid)
        //    {
        //        try
        //        {
        //            DataTable t = DB.GetData("select "
        //            + DiagnosticOPR_Fault_TagTable.DiagnosticOPRID + ","
        //                + DiagnosticOPR_Fault_TagTable.FaultID + ","
        //                 + DiagnosticOPR_Fault_TagTable.TagINFO
        //               + " from  "
        //               + DiagnosticOPR_Fault_TagTable.TableName
        //               + " where "
        //               + DiagnosticOPR_Fault_TagTable.TagID + "=" + Tagid);
        //            if (t.Rows.Count == 1)
        //            {
        //                DiagnosticOPR DiagnosticOPR_= new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
        //                 MaintenanceFault MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(Convert.ToUInt32(t.Rows[0][1].ToString()));
        //                string taginfo = t.Rows[0][2].ToString();
        //                return new DiagnosticOPR_Fault_Tag(Tagid, MaintenanceFault_, DiagnosticOPR_, taginfo);
        //            }
        //            else
        //                return null;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_DiagnosticOPR_Faul_Tag_info_ByID" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return null;
        //        }

        //    }
        //    public bool Add_DiagnosticOPR_Fault_Tag(DiagnosticOPR DiagnosticOPR_, 
        //        MaintenanceFault MaintenanceFault_, string taginfo)
        //    {
        //        try
        //        {

        //            DB.ExecuteSQLCommand("","insert into  "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                + " ( "
        //                + DiagnosticOPR_Fault_TagTable.DiagnosticOPRID + ","
        //                 + DiagnosticOPR_Fault_TagTable.FaultID + ","
        //                + DiagnosticOPR_Fault_TagTable.TagINFO
        //                + ")values( "
        //                +  DiagnosticOPR_.DiagnosticOPRID.ToString() + ","
        //                +  MaintenanceFault_.FaultID.ToString() + ","
        //                + "'" + taginfo + "'"
        //                + ")"
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Add_DiagnosticOPR_Fault_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Update_DiagnosticOPR_Fault_Tag(uint TagID, string INFO)
        //    {
        //        try
        //        {
        //            DB.ExecuteSQLCommand("","update   "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                + " set "
        //                + DiagnosticOPR_Fault_TagTable.TagINFO + "='" + INFO + "',"
        //                + " where "
        //                + DiagnosticOPR_Fault_TagTable.TagID + "=" + TagID
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("_DiagnosticOPR_Fault_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Delete_DiagnosticOPR_Fault_Tag(uint TagID)
        //    {
        //        try
        //        {


        //            DB.ExecuteSQLCommand("","Delete from    "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                + " where "
        //                + DiagnosticOPR_Fault_TagTable.TagID + "=" + TagID 
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("DeleteMeasureOPR" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        
        //    public List<DiagnosticOPR_Fault_Tag> Get_DiagnosticOPR_Fault_TagList(DiagnosticOPR DiagnosticOPR_)
        //    {
        //        List<MaintenanceTagSummary> list = new List<MaintenanceTagSummary>();
        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //                + DiagnosticOPR_Fault_TagTable.TagID + ","
        //                 + DiagnosticOPR_Fault_TagTable.FaultID + ","
        //                 + DiagnosticOPR_Fault_TagTable.TagINFO
        //                + " from "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_Fault_TagTable.DiagnosticOPRID + "=" + DiagnosticOPR_.DiagnosticOPRID
        //               );

        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                uint Tagid = Convert.ToUInt32(t.Rows[i][0].ToString());
        //                uint id;
        //                string desc;

        //                MaintenanceFault MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(Convert.ToUInt32(t.Rows[0][1].ToString()));
        //                id = MaintenanceFault_.FaultID;
        //                desc = MaintenanceFault_.FaultDesc;
        //                string taginfo = t.Rows[0][2].ToString();
        //                list.Add(new MaintenanceTagSummary(Tagid, MaintenanceTagSummary.DiagnosicOPR_Fault_Tag_Type, id, desc, taginfo));
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_DiagnosticOPR_Fault_TagList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return list;
        //        }

        //    }
        //    public int Get_DiagnosticOPR_Fault_TagCount_ForDiagnosticOPR(uint DiagnosticOPRID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select count(*) from "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_Fault_TagTable.DiagnosticOPRID + "=" + DiagnosticOPRID
        //               );
        //            if (t.Rows.Count > 0)
        //            {
        //                return Convert.ToInt32(t.Rows[0][0].ToString());
        //            }
        //            else return 0;

        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("GetDiagnostic_MaintenanceTagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return -1;
        //        }

        //    }
        //    public bool Delete_DiagnosticOPR_Fault_Tags_ByDiagnosticOPR(uint DiagnosticOPRID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("delete  from "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_Fault_TagTable.DiagnosticOPRID + "=" + DiagnosticOPRID 
        //               );
        //            return true;

        //        }
        //        catch (Exception ee)
        //        {
        //            throw new Exception("DeleteDiagnosticOPR_Tags" + ee.Message);
        //        }

        //    }
        //    public List<DiagnosticOPR_Fault_Tag> Get_DiagnosticOPR_Fault_TagList(MaintenanceFault MaintenanceFault_)
        //    {
        //        List<MaintenanceTagSummary> list = new List<MaintenanceTagSummary>();
        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //                + DiagnosticOPR_Fault_TagTable.TagID + ","
        //                 + DiagnosticOPR_Fault_TagTable.DiagnosticOPRID + ","
        //                 + DiagnosticOPR_Fault_TagTable.TagINFO
        //                + " from "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_Fault_TagTable.FaultID + "=" + MaintenanceFault_.FaultID
        //               );

        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                uint Tagid = Convert.ToUInt32(t.Rows[i][0].ToString());
        //                uint id;
        //                string desc;
        //                 DiagnosticOPR DiagnosticOPR_ = new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID(Convert.ToUInt32(t.Rows[0][1].ToString()));
        //                 id = DiagnosticOPR_.DiagnosticOPRID;
        //                 desc = DiagnosticOPR_.Desc;


        //                string taginfo = t.Rows[0][2].ToString();
        //                list.Add(new MaintenanceTagSummary(Tagid, MaintenanceTagSummary.DiagnosicOPR_Fault_Tag_Type, id, desc, taginfo));
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("GetFault_TagSummarryList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return list;
        //        }

        //    }
        //    public int Get_DiagnosticOPR_Fault_TagCount_ForFault(uint FaultID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select count(*) from "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_Fault_TagTable.FaultID + "=" + FaultID
        //               );
        //            if (t.Rows.Count > 0)
        //            {
        //                return Convert.ToInt32(t.Rows[0][0].ToString());
        //            }
        //            else return 0;

        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("GetDiagnosticOPR_Fault_TagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return -1;
        //        }

        //    }
        //    public bool Delete_DiagnosticOPR_Fault_Tags_ByFault(uint FaultID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("delete  from "
        //                + DiagnosticOPR_Fault_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_Fault_TagTable.FaultID + "=" + FaultID
        //               );
        //            return true;

        //        }
        //        catch (Exception ee)
        //        {
        //            throw new Exception("Delete_DiagnosticOPR_Fault_Tags" + ee.Message);
        //        }

        //    }
        //}
        //public class DiagnosticOPR_MissedFaultItem_TagSQL
        //{
  
        //    public static class DiagnosticOPR_MissedFaultItem_TagTable
        //    {
        //        public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_DiagnosticOPR_MissedFaultItem_Link";
        //        public const string TagID = "TagID";
        //        public const string DiagnosticOPRID = "DiagnosticOPRID";
        //        public const string MissedFaultID = "MissedFaultID";
        //        public const string TagINFO = "TagINFO";
        //    }
    
        //    DatabaseInterface DB;
        //    public DiagnosticOPR_MissedFaultItem_TagSQL(DatabaseInterface DB_)
        //    {
        //        DB = DB_;
        //    }
        //    public DiagnosticOPR_MissedFaultItem_Tag Get_DiagnosticOPR_MissedFaultItem_Tag_info_ByID(uint Tagid)
        //    {
        //        try
        //        {
        //            DataTable t = DB.GetData("select "
        //            + DiagnosticOPR_MissedFaultItem_TagTable.DiagnosticOPRID + ","
        //               + DiagnosticOPR_MissedFaultItem_TagTable.MissedFaultID + ","
        //                 + DiagnosticOPR_MissedFaultItem_TagTable.TagINFO
        //               + " from  "
        //               + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //               + " where "
        //               + DiagnosticOPR_MissedFaultItem_TagTable.TagID + "=" + Tagid);
        //            if (t.Rows.Count == 1)
        //            {
        //                DiagnosticOPR DiagnosticOPR_= new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
        //                Missed_Fault_Item Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID(Convert.ToUInt32(t.Rows[0][1].ToString()));
        //                string taginfo = t.Rows[0][2].ToString();
        //                return new DiagnosticOPR_MissedFaultItem_Tag(Tagid, DiagnosticOPR_, Missed_Fault_Item_, taginfo);
        //            }
        //            else
        //                return null;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_DiagnosticOPR_MissedFaultItem_Tag_info_ByID" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return null;
        //        }

        //    }
        //    public bool Add_DiagnosticOPR_MissedFaultItem_Tag(DiagnosticOPR DiagnosticOPR_, Missed_Fault_Item Missed_Fault_Item_,
        //         string taginfo)
        //    {

        //        try
        //        {

        //            DB.ExecuteSQLCommand("","insert into  "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                + " ( "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.DiagnosticOPRID + ","
        //                + DiagnosticOPR_MissedFaultItem_TagTable.MissedFaultID + ","
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TagINFO
        //                + ")values( "
        //                + DiagnosticOPR_.DiagnosticOPRID.ToString() + ","
        //                +  Missed_Fault_Item_.ID.ToString() + ","
        //                + "'" + taginfo + "'"
        //                + ")"
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Add_DiagnosticOPR_MissedFaultItem_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Update_DiagnosticOPR_MissedFaultItem_Tag(uint TagID, string INFO)
        //    {
        //        try
        //        {
        //            DB.ExecuteSQLCommand("","update   "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                + " set "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TagINFO + "='" + INFO + "',"
        //                + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TagID + "=" + TagID
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Update_DiagnosticOPR_MissedFaultItem_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Delete_DiagnosticOPR_MissedFaultItem_Tag(uint TagID)
        //    {
        //        try
        //        {


        //            DB.ExecuteSQLCommand("","Delete from    "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TagID + "=" + TagID
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Delete_DiagnosticOPR_MissedFaultItem_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public List<MaintenanceTagSummary> Get_DiagnosticOPR_MissedFaultItem_TagList(DiagnosticOPR DiagnosticOPR_)
        //    {
        //        List<MaintenanceTagSummary> list = new List<MaintenanceTagSummary>();
        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TagID + ","
        //                 + DiagnosticOPR_MissedFaultItem_TagTable.MissedFaultID + ","
        //                 + DiagnosticOPR_MissedFaultItem_TagTable.TagINFO
        //                + " from "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.DiagnosticOPRID + "=" + DiagnosticOPR_.DiagnosticOPRID
        //               );

        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                uint Tagid = Convert.ToUInt32(t.Rows[i][0].ToString());
        //                uint id;
        //                string desc;
        //                    Missed_Fault_Item Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID (Convert.ToUInt32(t.Rows[i][1].ToString()));

        //                    id = Missed_Fault_Item_.ID;
        //                    desc = "الموقع :" + Missed_Fault_Item_.Location;
   

        //                string taginfo = t.Rows[i][2].ToString();
        //                list.Add(new MaintenanceTagSummary(Tagid, MaintenanceTagSummary.DiagnosicOPR_MissedFaultItem_Tag_Type, id, desc, taginfo));
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_DiagnosticOPR_MissedFaultItem_TagList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return list;
        //        }

        //    }
        //    public int Get_DiagnosticOPR_MissedFaultItem_TagCount_ForDiagnosticOPR(uint DiagnosticOPRID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select count(*) from "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.DiagnosticOPRID + "=" + DiagnosticOPRID
        //               );
        //            if (t.Rows.Count > 0)
        //            {
        //                return Convert.ToInt32(t.Rows[0][0].ToString());
        //            }
        //            else return 0;

        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_DiagnosticOPR_MissedFaultItem_TagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return -1;
        //        }

        //    }
        //    public bool Delete_DiagnosticOPR_MissedFaultItem_Tags_ByDiagnosticOPR(uint DiagnosticOPRID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("delete  from "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.DiagnosticOPRID + "=" + DiagnosticOPRID
        //               );
        //            return true;

        //        }
        //        catch (Exception ee)
        //        {
        //            throw new Exception("Delete_DiagnosticOPR_MissedFaultItem_Tags" + ee.Message);
        //        }

        //    }
        //    public List<MaintenanceTagSummary> Get_DiagnosticOPR_MissedFaultItem_TagList(Missed_Fault_Item Missed_Fault_Item_)
        //    {
        //        List<MaintenanceTagSummary> list = new List<MaintenanceTagSummary>();
        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TagID + ","
        //                 + DiagnosticOPR_MissedFaultItem_TagTable.DiagnosticOPRID + ","
        //                 + DiagnosticOPR_MissedFaultItem_TagTable.TagINFO
        //                + " from "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.MissedFaultID + "=" + Missed_Fault_Item_.ID
        //               );

        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                uint Tagid = Convert.ToUInt32(t.Rows[i][0].ToString());
        //                uint id;
        //                string desc;
        //                DiagnosticOPR DiagnosticOPR_ = new DiagnosticOPRSQL(DB).GetDiagnosticOPRINFO_BYID(Convert.ToUInt32(t.Rows[i][1].ToString()));
        //                id = DiagnosticOPR_.DiagnosticOPRID;
        //                desc = DiagnosticOPR_.Desc;
        //                string taginfo = t.Rows[i][2].ToString();
        //                list.Add(new MaintenanceTagSummary(Tagid, MaintenanceTagSummary.DiagnosicOPR_MissedFaultItem_Tag_Type, id, desc, taginfo));
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_DiagnosticOPR_MissedFaultItem_TagList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return list;
        //        }

        //    }
        //    public int Get_DiagnosticOPR_MissedFaultItem_TagCount_ForMissedFaultItem(uint ID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select count(*) from "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.MissedFaultID + "=" + ID
        //               );
        //            if (t.Rows.Count > 0)
        //            {
        //                return Convert.ToInt32(t.Rows[0][0].ToString());
        //            }
        //            else return 0;

        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Ge_DiagnosticOPR_MissedFaultItem_TagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return -1;
        //        }

        //    }
        //    public bool Delete_DiagnosticOPR_MissedFaultItem_Tags_ByMissedFaultItem(uint ID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("delete  from "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + DiagnosticOPR_MissedFaultItem_TagTable.MissedFaultID + "=" + ID 
        //               );
        //            return true;

        //        }
        //        catch (Exception ee)
        //        {
        //            throw new Exception("Delete_DiagnosticOPR_MissedFaultItem_Tags" + ee.Message);
        //        }

        //    }
       
        //}
        //public class Fault_MissedFaultItem_TagSQL
        //{
  
        //    public static class Fault_MissedFaultItem_TagTable
        //    {
        //        public const string TableName = "Trade_BillMaintenance_MaintenanceOPR_Fault_MissedFaultItem_Link";
        //        public const string TagID = "TagID";
        //        public const string MissedFaultID = "MissedFaultID";
        //        public const string FaultID = "FaultID";
        //        public const string TagINFO = "TagINFO";
        //    }
        //    DatabaseInterface DB;
        //    public Fault_MissedFaultItem_TagSQL(DatabaseInterface DB_)
        //    {
        //        DB = DB_;
        //    }
        //    public Fault_MissedFaultItem_Tag Get_Fault_MissedFaultItem_Tag_info_ByID(uint Tagid)
        //    {
        //        try
        //        {
        //            DataTable t = DB.GetData("select "
        //            + Fault_MissedFaultItem_TagTable.FaultID  + ","
        //               + Fault_MissedFaultItem_TagTable.MissedFaultID + ","
        //                 + Fault_MissedFaultItem_TagTable.TagINFO
        //               + " from  "
        //               + Fault_MissedFaultItem_TagTable.TableName
        //               + " where "
        //               + Fault_MissedFaultItem_TagTable.TagID + "=" + Tagid);
        //            if (t.Rows.Count == 1)
        //            {
        //                MaintenanceFault MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID(Convert.ToUInt32(t.Rows[0][0].ToString()));
        //                Missed_Fault_Item Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID (Convert.ToUInt32(t.Rows[0][1].ToString()));
        //                string taginfo = t.Rows[0][2].ToString();
        //                return new Fault_MissedFaultItem_Tag(Tagid, MaintenanceFault_, Missed_Fault_Item_, taginfo);
        //            }
        //            else
        //                return null;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_Fault_MissedFaultItem_Tag_info_ByID" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return null;
        //        }

        //    }
        //    public bool Add_Fault_MissedFaultItem_Tag(MaintenanceFault MaintenanceFault_, Missed_Fault_Item Missed_Fault_Item_,
        //         string taginfo)
        //    {
        //        try
        //        {

        //            DB.ExecuteSQLCommand("","insert into  "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                + " ( "
        //                + Fault_MissedFaultItem_TagTable.FaultID  + ","
        //                + Fault_MissedFaultItem_TagTable.MissedFaultID + ","
        //                + Fault_MissedFaultItem_TagTable.TagINFO
        //                + ")values( "
        //                + MaintenanceFault_.FaultID .ToString() + ","
        //                + Missed_Fault_Item_.ID.ToString() + ","
        //                + "'" + taginfo + "'"
        //                + ")"
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Add_Fault_MissedFaultItem_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Update_Fault_MissedFaultItem_Tag(uint TagID, string INFO)
        //    {
        //        try
        //        {
        //            DB.ExecuteSQLCommand("","update   "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                + " set "
        //                + Fault_MissedFaultItem_TagTable.TagINFO + "='" + INFO + "',"
        //                + " where "
        //                + Fault_MissedFaultItem_TagTable.TagID + "=" + TagID
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Update_Fault_MissedFaultItem_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Delete__Fault_MissedFaultItem_Tag(uint TagID)
        //    {
        //        try
        //        {


        //            DB.ExecuteSQLCommand("","Delete from    "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                + " where "
        //                + Fault_MissedFaultItem_TagTable.TagID + "=" + TagID
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Delete__Fault_MissedFaultItem_Tag" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public List<MaintenanceTagSummary> Get_Fault_MissedFaultItem_TagList(MaintenanceFault MaintenanceFault_)
        //    {
        //        List<MaintenanceTagSummary> list = new List<MaintenanceTagSummary>();
        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //                + Fault_MissedFaultItem_TagTable.TagID + ","
        //                 + Fault_MissedFaultItem_TagTable.MissedFaultID + ","
        //                 + Fault_MissedFaultItem_TagTable.TagINFO
        //                + " from "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + Fault_MissedFaultItem_TagTable.FaultID  + "=" + MaintenanceFault_.FaultID 
        //               );

        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                uint Tagid = Convert.ToUInt32(t.Rows[i][0].ToString());
        //                uint id;
        //                string desc;
        //                Missed_Fault_Item Missed_Fault_Item_ = new MissedFaultItemSQL(DB).GetMissedFaultItem_INFO_BYID (Convert.ToUInt32(t.Rows[i][1].ToString()));
        //                id = Missed_Fault_Item_.ID;
        //                desc = "الموقع :" + Missed_Fault_Item_.Location;
        //                string taginfo = t.Rows[i][2].ToString();
        //                list.Add(new MaintenanceTagSummary(Tagid, MaintenanceTagSummary.Fault_MissedFaultItem_Tag_Type , id, desc, taginfo));
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_Fault_MissedFaultItem_TagList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return list;
        //        }

        //    }
        //    public int Get_Fault_MissedFaultItem_TagCount_ForFault(uint FaultID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select count(*) from "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + Fault_MissedFaultItem_TagTable.FaultID  + "=" + FaultID 
        //               );
        //            if (t.Rows.Count > 0)
        //            {
        //                return Convert.ToInt32(t.Rows[0][0].ToString());
        //            }
        //            else return 0;

        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_Fault_MissedFaultItem_TagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return -1;
        //        }

        //    }
        //    public bool Delete_Fault_MissedFaultItem_Tags_ByFault(uint FaultID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("delete  from "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + Fault_MissedFaultItem_TagTable.FaultID  + "=" + FaultID 
        //               );
        //            return true;

        //        }
        //        catch (Exception ee)
        //        {
        //            throw new Exception("Delete_Fault_MissedFaultItem_TagTags" + ee.Message);
        //        }

        //    }
        //    public List<MaintenanceTagSummary> Get_Fault_MissedFaultItem_TagList(Missed_Fault_Item Missed_Fault_Item_)
        //    {
        //        List<MaintenanceTagSummary> list = new List<MaintenanceTagSummary>();
        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //                + Fault_MissedFaultItem_TagTable.TagID + ","
        //                 + Fault_MissedFaultItem_TagTable.FaultID  + ","
        //                 + Fault_MissedFaultItem_TagTable.TagINFO
        //                + " from "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + Fault_MissedFaultItem_TagTable.MissedFaultID + "=" + Missed_Fault_Item_.ID
        //               );

        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                uint Tagid = Convert.ToUInt32(t.Rows[i][0].ToString());
        //                uint id;
        //                string desc;
        //                MaintenanceFault MaintenanceFault_ = new MaintenanceFaultSQL(DB).Get_Fault_INFO_BYID (Convert.ToUInt32(t.Rows[i][1].ToString()));
        //                id = MaintenanceFault_.FaultID ;
        //                desc = MaintenanceFault_.FaultDesc ;
        //                string taginfo = t.Rows[i][2].ToString();
        //                list.Add(new MaintenanceTagSummary(Tagid, MaintenanceTagSummary.Fault_MissedFaultItem_Tag_Type , id, desc, taginfo));
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_Fault_MissedFaultItem_TagList" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return list;
        //        }

        //    }
        //    public int Get_Fault_MissedFaultItem_TagCount_ForMissedFaultItem(uint ID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("select count(*) from "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + Fault_MissedFaultItem_TagTable.MissedFaultID + "=" + ID
        //               );
        //            if (t.Rows.Count > 0)
        //            {
        //                return Convert.ToInt32(t.Rows[0][0].ToString());
        //            }
        //            else return 0;

        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("Get_Fault_MissedFaultItem_TagCount" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return -1;
        //        }

        //    }
        //    public bool Delete_Fault_MissedFaultItem_Tags_ByMissedFaultItem(uint ID)
        //    {

        //        try
        //        {

        //            DataTable t = new DataTable();
        //            t = DB.GetData("delete  from "
        //                + Fault_MissedFaultItem_TagTable.TableName
        //                 + " where "
        //                + Fault_MissedFaultItem_TagTable.MissedFaultID + "=" + ID
        //               );
        //            return true;

        //        }
        //        catch (Exception ee)
        //        {
        //            throw new Exception("Delete_Fault_MissedFaultItem_Tags" + ee.Message);
        //        }

        //    }
        //}
    }
}
