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
        public class IndustrySQL
        {
            DatabaseInterface DB;
            private static class IndustryTable
            {
                public const string TableName = "[dbo].[Trade_GetIndustrialOperations]()";
                public const string OprType = "OprType";
                public const string OprID = "OprID";
                public const string OprDate = "OprDate";
                public const string FolderName = "FolderName";
                public const string ItemName = "ItemName";
                public const string CompanyName = "CompanyName";
                public const string ItemSerial = "ItemSerial";
                public const string Cost = "Cost";
                public const string CurrencyName = "CurrencyName";
                public const string ExchangeRate = "ExchangeRate";
                public const string OPR_Status = "OPR_Status";
                public const string OPR_INFO = "OPR_INFO";
            }
            public IndustrySQL(DatabaseInterface db)
            {
                DB = db;

            }
            public List<IndustryOPR > GetIndustryOperations()
            {
                List<IndustryOPR> IndustryOPRList = new List<IndustryOPR>();
                try
                {
                    
                    DataTable t =DB.GetData(" select "
                        +IndustryTable .OprType +","
                        + IndustryTable.OprID  + ","
                        + IndustryTable.OprDate  + ","
                        + IndustryTable.FolderName + ","
                        + IndustryTable.ItemName  + ","
                        + IndustryTable.CompanyName  + ","
                        + IndustryTable.ItemSerial  + ","
                         + IndustryTable.Cost  + ","
                        + IndustryTable.CurrencyName  + ","
                        + IndustryTable.ExchangeRate  + ","
                        + IndustryTable.OPR_Status +","
                        + IndustryTable.OPR_INFO
                        + " from "
                        +IndustryTable.TableName 

                        );
                    for(int i=0;i<t.Rows .Count;i++)
                    {
                        bool oprtype = Convert.ToBoolean(t.Rows[i][0].ToString());
                        uint oprid = Convert.ToUInt32 (t.Rows[i][1].ToString());
                        DateTime  oprdate = Convert.ToDateTime(t.Rows[i][2].ToString());
                        string foldername = t.Rows[i][3].ToString();
                        string itemname =t.Rows[i][4].ToString();
                        string companyname = t.Rows[i][5].ToString();
                        string itemserial = t.Rows[i][6].ToString();
                        double cost = Convert.ToDouble(t.Rows[i][7].ToString());

                        string currencyname = t.Rows[i][8].ToString();
                        double exchangerate = Convert.ToDouble(t.Rows [i][9].ToString ());
                        string oprstatus = t.Rows[i][10].ToString();
                        string oprinfo = t.Rows[i][11].ToString();
                        IndustryOPRList.Add(new IndustryOPR(oprtype ,oprid ,oprdate ,foldername,itemname,companyname,
                            itemserial,cost,currencyname,exchangerate,oprstatus , oprinfo));
                    }
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetIndustryOperations" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
                return IndustryOPRList;
            }
        }
         public class AssemblageSQL
        {
            DatabaseInterface DB;
            private static class AssemblageTable
            {
                public const string TableName = "Trade_Assemblage";
                public const string AssemblageID = "AssemblageID";
                public const string OprDate = "OprDate";
                public const string ItemSerial = "ItemSerial";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";

            }
            public AssemblageSQL(DatabaseInterface db)
            {
                DB = db;

            }

            public AssemblabgeOPR GetAssemblageOPR_INFO_BYID(uint oprid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + AssemblageTable.OprDate + ","
                    + AssemblageTable.ItemSerial + ","
                    + AssemblageTable.CurrencyID + ","
                     + AssemblageTable.ExchangeRate
                    + " from   "
                    + AssemblageTable.TableName
                    + " where "
                    + AssemblageTable.AssemblageID  + "=" + oprid
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime oprdate = Convert.ToDateTime(t.Rows[0][0]);
                        string itemserial = t.Rows[0][1].ToString();
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][2].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[0][3].ToString());
                        Operation operation = new Operation(Operation.ASSEMBLAGE , oprid);
                        List<ItemIN > ItemINList = new ItemINSQL (DB).GetItemINList(operation);
                        ItemIN  itemin;
                        if (ItemINList.Count == 1)
                            itemin = ItemINList[0];
                        else
                            itemin = null;
                        return new AssemblabgeOPR (oprid, oprdate  , itemin 
                            , itemserial, Currency_, exchangerate);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetAssemblageOPR_INFO_BYID" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }

            public AssemblabgeOPR CreateAssemblageOPR
                (DateTime oprdate, Item item, TradeState tradestate, 
                double cost, string notes
                , string itemserial
                , Currency currency, double exchangerate)
            {
                try
                {

                    DataTable t = DB.GetData(" insert into "
                    + AssemblageTable.TableName
                    + "("
                    + AssemblageTable.OprDate + ","
                    + AssemblageTable.ItemSerial + ","
                    + AssemblageTable.CurrencyID + ","
                     + AssemblageTable.ExchangeRate
                    + ")"
                    + "values"
                    + "("
                    + "'" + oprdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'" + itemserial + "'"
                     + ","
                    + currency.CurrencyID
                    + ","
                    + exchangerate
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint oprid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    Operation assemblage_opr = new Operation(Operation.ASSEMBLAGE,oprid);
                    ItemIN  itemin = new ItemINSQL (DB).AddItemIN 
                        (assemblage_opr , item , tradestate , 1, null, cost, notes);
                    if (itemin == null)
                    {
                        DeleteDisAssemblageOPR(oprid);
                        throw new Exception("فشل انشاء عملية تجميع");
                    }
                    
                    DB.AddLog(
                                    DatabaseInterface.Log.LogType.INSERT 
                                    , DatabaseInterface.Log.Log_Target.Trade_Assemblage
                                     , ""
                                     , true, "");
                    return GetAssemblageOPR_INFO_BYID(oprid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT 
                            , DatabaseInterface.Log.Log_Target.Trade_Assemblage
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateAssemblageOPR
                 (uint oprid, DateTime oprdate, uint iteminid,
                Item  item, TradeState  tradestate 
                , double cost, string notes
                , string itemserial
                , Currency currency, double exchangerate)
            {
                try
                {
                    new ItemINSQL (DB).UpdateItemIN (iteminid , item, tradestate , 1, null, cost, notes);
                    DB.ExecuteSQLCommand("update  "
                    + AssemblageTable.TableName
                    + " set "
                    + AssemblageTable.OprDate + "='" + oprdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + AssemblageTable.ItemSerial + "='" + itemserial + "'"
                    + ","
                    + AssemblageTable.CurrencyID + "=" + currency.CurrencyID
                    + ","
                   + AssemblageTable.ExchangeRate + "=" + exchangerate

                    + " where "
                    + AssemblageTable.AssemblageID  + "=" + oprid
                    );
                    DB.AddLog(
                                    DatabaseInterface.Log.LogType.UPDATE
                                    , DatabaseInterface.Log.Log_Target.Trade_Assemblage 
                                     , ""
                                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE
                            , DatabaseInterface.Log.Log_Target.Trade_Assemblage 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteDisAssemblageOPR(uint oprid)
            {
                try
                {
                    if (new ItemOUTSQL(DB).Does_Operation_Has_ItemsOUT(Operation.ASSEMBLAGE , oprid))
                    {
                        throw new Exception("يجب اولا حذف العناصر المساهمة في عملية التجميع");
                    }
                    //   DB.ExecuteSQLCommand("","delete from   "
                    //  + ItemOUTSQL.ItemOUTTable.TableName
                    //  + " where "
                    //  + ItemOUTSQL.ItemOUTTable.OperationType + "=" + Operation.DISASSEMBLAGE
                    //  + " and "
                    //  + ItemOUTSQL.ItemOUTTable.OperationID + "=" + oprid

                    //  );
                   AssemblabgeOPR AssemblabgeOPR_= GetAssemblageOPR_INFO_BYID(oprid);
                  List <ItemOUT > itemoutlist=  new ItemOUTSQL(DB).GetItemIN_ItemOUTList(AssemblabgeOPR_._ItemIN);
                    if(itemoutlist.Count >0)
                    {
                        throw new Exception("العنصر الناتج عن عملية التجميع تم اخراجه يجب اولا الغاء عملية اخراج العنصر");
                    }
                    DB.ExecuteSQLCommand("delete from   "
                 + ItemINSQL.ItemINTable.TableName
                 + " where "
                 + ItemINSQL.ItemINTable.OperationType + "=" + Operation.DISASSEMBLAGE
                 + " and "
                 + ItemINSQL.ItemINTable.OperationID + "=" + oprid
                 );

                    DB.ExecuteSQLCommand("delete from   "
                    + AssemblageTable.TableName
                    + " where "
                    + AssemblageTable.AssemblageID  + "=" + oprid
                    );
                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.DELETE 
                                     , DatabaseInterface.Log.Log_Target.Trade_Assemblage
                                      , ""
                                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_Assemblage
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public class DisAssemblageSQL
        {
            DatabaseInterface DB;
            private static class DisAssemblageTable
            {
                public const string TableName = "Trade_DisAssemblage";
                public const string DisAssemblageID = "DisAssemblageID";
                public const string OprDate = "OprDate";
                public const string ItemSerial = "ItemSerial";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string Ravage = "Ravage";

            }
            public DisAssemblageSQL(DatabaseInterface db)
            {
                DB = db;

            }

            public DisAssemblabgeOPR GetDisAssemblageOPR_INFO_BYID(uint oprid)
            {
                try
                {
 
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + DisAssemblageTable.OprDate + ","
                    + DisAssemblageTable.ItemSerial + ","
                    + DisAssemblageTable.CurrencyID + ","
                      + DisAssemblageTable.ExchangeRate + ","
                      + DisAssemblageTable.Ravage
                    + " from   "
                    + DisAssemblageTable.TableName
                    + " where "
                    + DisAssemblageTable.DisAssemblageID  + "=" + oprid
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime oprdate = Convert.ToDateTime(t.Rows[0][0]);
                        string itemserial = t.Rows[0][1].ToString();
                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][2].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[0][3].ToString());
                        bool ravage = Convert.ToBoolean(t.Rows[0][4].ToString());
                        Operation operation = new Operation(Operation.DISASSEMBLAGE, oprid);
                        List<ItemOUT> ItemOUTList = new ItemOUTSQL(DB).GetItemOUTList(operation);
                        ItemOUT itemout;
                        if (ItemOUTList.Count == 1)
                            itemout = ItemOUTList[0];
                        else
                            itemout = null;

                        return new DisAssemblabgeOPR(oprid,  oprdate, itemout
                            , itemserial, Currency_, exchangerate, ravage);

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetAssemblageOPR_INFO_BYID" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }

            public DisAssemblabgeOPR CreateAssemblageOPR
                (DateTime oprdate, uint iteminid, TradeStorePlace Place,
                  string notes
                , string itemserial
                , Currency currency, double exchangerate,bool ravage)
            {
                try
                {
                    
                    DataTable t = DB.GetData(" insert into "
                    + DisAssemblageTable.TableName
                    + "("
                    + DisAssemblageTable.OprDate + ","
                    + DisAssemblageTable.ItemSerial + ","
                    + DisAssemblageTable.CurrencyID + ","
                     + DisAssemblageTable.ExchangeRate+","
                      + DisAssemblageTable.Ravage  
                    + ")"
                    + "values"
                    + "("
                    + "'" + oprdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'" + itemserial + "'"
                     + ","
                    + currency.CurrencyID
                    + ","
                    + exchangerate
                    + ","
                    +( ravage ?1:0)
                    + ")"
                    + " SELECT SCOPE_IDENTITY() "
                    );
                    uint oprid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    Operation disassemblage_opr = new Operation( Operation.DISASSEMBLAGE, oprid);
                    ItemOUT itemout = new ItemOUTSQL(DB).AddItemOUT(disassemblage_opr, iteminid, Place, 1, null , null, notes);
                    if (itemout == null)
                    {
                        DeleteDisAssemblageOPR(oprid);
                        throw new Exception("فشل انشاء عملية تفكيك");
                    }
                    
                    DB.AddLog(
                                       DatabaseInterface.Log.LogType.INSERT
                                       , DatabaseInterface.Log.Log_Target.Trade_DisAssemblage 
                                        , ""
                                        , true, "");
                    return GetDisAssemblageOPR_INFO_BYID(oprid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.Trade_DisAssemblage 
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public bool UpdateDisAssemblageOPR
                 (uint oprid, DateTime oprdate, uint iteminid, TradeStorePlace Place
                ,  string notes
                , string itemserial
                , Currency currency, double exchangerate, bool ravage)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                                        + ItemOUTSQL .ItemOUTTable .TableName
                                        + " where  "
                                        + ItemOUTSQL.ItemOUTTable.OperationID  + "=" + oprid
                                        + " and "
                                        + ItemOUTSQL.ItemOUTTable.OperationType + "=" + Operation.DISASSEMBLAGE  
                                        );
                    ItemOUT itemout = new ItemOUTSQL(DB).AddItemOUT(new Operation(Operation.DISASSEMBLAGE, oprid), iteminid, Place, 1, null, null , notes);
                    if (itemout == null)
                    {
                        DeleteDisAssemblageOPR(oprid);
                        throw new Exception("فشل تعديل بيانات العنصر المفكك");
                    }
                    DB.ExecuteSQLCommand("update  "
                    + DisAssemblageTable.TableName
                    + " set "
                    + DisAssemblageTable.OprDate   + "='" + oprdate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + DisAssemblageTable.ItemSerial  + "='" + itemserial  + "'"
                    + ","
                    + DisAssemblageTable.CurrencyID + "=" + currency.CurrencyID
                    + ","
                   + DisAssemblageTable.ExchangeRate + "=" + exchangerate
                     + ","
                    + DisAssemblageTable.Ravage  + "="+ (ravage ? 1 : 0)
                    + " where "
                    + DisAssemblageTable.DisAssemblageID   + "=" + oprid  
                    );

                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.UPDATE 
                                     , DatabaseInterface.Log.Log_Target.Trade_DisAssemblage
                                      , ""
                                      , true, "");
                    return true ;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.UPDATE 
                            , DatabaseInterface.Log.Log_Target.Trade_DisAssemblage
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool DeleteDisAssemblageOPR(uint oprid)
            {
                try
                {
                    if (new ItemINSQL(DB).Does_Operation_Has_ItemsIN(Operation.DISASSEMBLAGE , oprid))
                    {
                        throw new Exception("يجب اولا حذف العناصر الناتجة عن   عملية التفكيك");
                    }
                    //   DB.ExecuteSQLCommand("","delete from   "
                    //  + ItemOUTSQL.ItemOUTTable.TableName
                    //  + " where "
                    //  + ItemOUTSQL.ItemOUTTable.OperationType + "=" + Operation.DISASSEMBLAGE
                    //  + " and "
                    //  + ItemOUTSQL.ItemOUTTable.OperationID + "=" + oprid

                    //  );
                    DisAssemblabgeOPR DisAssemblabgeOPR_ = GetDisAssemblageOPR_INFO_BYID(oprid);
                    List<ItemIN > ItemINlist = new ItemINSQL(DB).GetItemINList(DisAssemblabgeOPR_._Operation );
                    if (ItemINlist.Count > 0)
                    {
                        throw new Exception("يجب اولا حذف العناصر الداخلة عن طريق عملية التفكيك ");
                    }
                 //   DB.ExecuteSQLCommand("","delete from   "
                 //+ ItemINSQL.ItemINTable.TableName
                 //+ " where "
                 //+ ItemINSQL.ItemINTable.OperationType + "=" + Operation.DISASSEMBLAGE
                 //+ " and "
                 //+ ItemINSQL.ItemINTable.OperationID + "=" + oprid
                 //);
                    DB.ExecuteSQLCommand( "delete from   "
                   + ItemOUTSQL .ItemOUTTable .TableName
                   + " where "
                   + ItemOUTSQL .ItemOUTTable .OperationType + "=" + Operation .DISASSEMBLAGE 
                   + " and "
                   + ItemOUTSQL.ItemOUTTable.OperationID + "=" +oprid
                    
                   );
                 //   DB.ExecuteSQLCommand("","delete from   "
                 //+ ItemINSQL.ItemINTable.TableName
                 //+ " where "
                 //+ ItemINSQL.ItemINTable.OperationType + "=" + Operation.DISASSEMBLAGE
                 //+ " and "
                 //+ ItemINSQL.ItemINTable.OperationID + "=" + oprid
                 //);
                    DB.ExecuteSQLCommand("delete from   "
                    + DisAssemblageTable.TableName
                    + " where "
                    + DisAssemblageTable.DisAssemblageID   + "=" + oprid  
                    );

                    DB.AddLog(
                                     DatabaseInterface.Log.LogType.DELETE 
                                     , DatabaseInterface.Log.Log_Target.Trade_DisAssemblage
                                      , ""
                                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE 
                            , DatabaseInterface.Log.Log_Target.Trade_DisAssemblage
                            , ""
                          , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
       
        }    
    }
}
