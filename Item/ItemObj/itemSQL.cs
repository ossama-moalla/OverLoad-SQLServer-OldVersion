using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using ItemProject.ItemObj.Objects;
using ItemProject.Trade.Objects;
using ItemProject.Trade.TradeSQL;

namespace ItemProject.ItemObj
{
    namespace ItemObjSQL
    {
        public class FolderSQL
        {
            DatabaseInterface DB;
            public static class FolderTable
            {
                public const string TableName = "Item_Folder";
                public const string FolderID = "FolderID";
                public const string FolderName = "FolderName";
                public const string ParentFolderID = "ParentFolderID";
                public const string CreateDate = "CreateDate";
                public const string DefaultConsumeUnit = "DefaultConsumeUnit";

            }
            public const string FolderPathFunction = "[dbo].[Item_GetFolderPath]";
            public FolderSQL(DatabaseInterface db)
            {
                DB = db;
                
            }
            public string GetFolderPath(Folder Folder_)
            {
                if (Folder_ == null) return "Root\\";
                DataTable t = DB.GetData("select "
                    +FolderSQL.FolderPathFunction
                    +"("
                    +Folder_.FolderID
                    +")");
                return t.Rows[0][0].ToString();
                //List<string> f_path = new List<string>();
                //Folder f = Folder_;
                //string s="ROOT / ";
                //while(f.ParentFolderID !=null)
                //{
                //    f = GetFolderInfoByID(Convert .ToUInt32 ( f.ParentFolderID));
                //    f_path.Add(f.FolderName);
                //}
                //for (int i = f_path.Count - 1; i >= 0; i--)
                //    s += f_path[i] + " /" ;
                //return s;
            }
            public bool  CreateFolder(string name, uint? parentid,string default_consumeunit)
            {
                try
                {
                    DateTime time = DateTime.Now;              // Use current time
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string datetime = "'" + time.ToString(format) + "'";
                    string parentid_string;
                    if (parentid == null)
                        parentid_string = "null";
                    else
                        parentid_string = parentid.ToString();


     
                    DB.ExecuteSQLCommand( " insert into "
                    + FolderTable.TableName 
                    + "("
                    + FolderTable.FolderName
                    + ","
                    + FolderTable.ParentFolderID
                    + ","
                    + FolderTable.CreateDate
                     + ","
                    + FolderTable.DefaultConsumeUnit
                    + ")"
                    + "values"
                    + "("
                    + "'" + name + "'"
                    + ","
                    + parentid_string
                     + ","
                    + datetime
                      + ","
                      + "'" + default_consumeunit + "'"
                    + ")"
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Item_Folder 
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                      , DatabaseInterface.Log.Log_Target.Item_Folder
                      , ""
                    , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateFolder(Folder folder,string newname, string default_consumeunit)
            {
                   try
                {
                    DB.ExecuteSQLCommand( "update  "
                    + FolderTable.TableName
                    + " set "
                    + FolderTable.FolderName+"='"+newname +"'"
                    +","
                    + FolderTable.DefaultConsumeUnit  + "='" + default_consumeunit  + "'"
                    + " where "
                    +FolderTable.FolderID  +"="+folder .FolderID 
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                      , DatabaseInterface.Log.Log_Target.Item_Folder
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                      , DatabaseInterface.Log.Log_Target.Item_Folder
                      , ""
                    , false , ee.Message  );
                    MessageBox.Show("","خطأ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteFolder(Folder folder__)
            {
                List<Item> Items = new ItemSQL(DB).GetItemsInFolder(folder__);
                List<Folder> Folders = new FolderSQL(DB).GetFolderChilds(folder__);
                if (Folders.Count >0 || Items .Count >0)
                {
                    MessageBox.Show("المجلد"+folder__ .FolderName +" غير فارغ لا يمكن حذفه!","خطأ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return false;
                }
                try
                {
                    try
                    {
                        ItemSpec_Restrict_SQL ItemSpec_Restrict_SQL_= new ItemSpec_Restrict_SQL(DB);
                        List <ItemSpec_Restrict> ItemSpec_Restrict_List = ItemSpec_Restrict_SQL_.GetItemSpecRestrictList(folder__);
                        for(int i=0; i< ItemSpec_Restrict_List.Count;i++)
                        {
                            ItemSpec_Restrict_SQL_.DeleteItemSpecRestrict(ItemSpec_Restrict_List[i].SpecID);
                        }
                        DB.ExecuteSQLCommand("delete from   "
                          + ItemSpecSQL.ItemSpecTable.TableName
                          + " where "
                          + ItemSpecSQL.ItemSpecTable.FolderID + "=" + folder__.FolderID
                          );

                    }
                    catch(SqlException  ee)
                    {
                        MessageBox.Show(ee.Message);
                        MessageBox.Show("فشل حذف الخصائص المرتبطة بهذا المجلد , فشل حذف المجلد!", "حدث خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    DB.ExecuteSQLCommand( "delete from   "
                    + FolderTable.TableName
                    + " where "
                    + FolderTable.FolderID + "=" + folder__.FolderID
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_Folder
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_Folder
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public Folder GetParentFolder(Folder f)
            {
                if (f.ParentFolderID == null) return null;
                DataTable t = new DataTable();
                try
                {
                    t = DB.GetData("select * from " + FolderTable.TableName
                   + " where " + FolderTable.FolderID + "=" + f.ParentFolderID);
                }
                catch(Exception ee)
                {
                    MessageBox.Show("فشل الاتصال بقاعدة البيانات","خطأ",MessageBoxButtons.OK,MessageBoxIcon.Error );
                    return null;
                }
                
                
                uint fid =Convert .ToUInt32  ( f.ParentFolderID);
                string fname = t.Rows[0][1].ToString ();
                uint? p;
                try
                {
                    p = Convert.ToUInt32(t.Rows[0][2]);
                }catch
                {
                    p = null;
                }
                DateTime d = Convert.ToDateTime(t.Rows[0][3]);
                string default_consumeUnit = t.Rows[0][4].ToString();
                return new Folder(fid, fname, p, d,default_consumeUnit);
              
            }
            public List<Folder > GetFolderChilds(Folder folder)
            {
                List<Folder> list = new List<Folder>();
                string parentid;
                if (folder  == null) parentid = " is null";
                else parentid ="="+ folder.FolderID .ToString();

                DataTable  t = new  DataTable();
                t = DB.GetData("select * from " + FolderTable.TableName 
                   + " where " + FolderTable.ParentFolderID   + parentid
                   + " order by " + FolderTable.FolderName
                   );
                for(int i=0;i<t.Rows.Count;i++)
                {

                    uint fid = Convert.ToUInt32(t.Rows[i][0].ToString());
                    string fname = t.Rows[i][1].ToString();
                    uint? p;
                    if (folder == null) p = null;
                    else p = folder.FolderID;
                    DateTime d = Convert.ToDateTime(t.Rows[i][3]);
                    string default_consumeUnit = t.Rows[i][4].ToString();
                    list .Add (  new Folder(fid, fname, p, d,default_consumeUnit ));
                }
                return list ;
            }
            public List<Folder> SearchFolder(string  n_)
            {
                List<Folder> list = new List<Folder>();
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + FolderTable.FolderID + ","
                        + FolderTable.FolderName + ","
                        + FolderTable.ParentFolderID + ","
                        + FolderTable.CreateDate + ","
                        + FolderTable.DefaultConsumeUnit 
                        + " from " + FolderTable.TableName
                       + " where " + FolderTable.FolderName + " like  '%" + n_ + "%'");
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint fid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string fname = t.Rows[i][1].ToString();
                        uint? p;
                        try
                        {
                            p = Convert.ToUInt32(t.Rows[i][2].ToString());
                        }
                        catch
                        {
                            p = null;
                        }

                        DateTime d = Convert.ToDateTime(t.Rows[i][3]);
                        string default_consumeUnit = t.Rows[i][4].ToString();
                        list.Add(new Folder(fid, fname, p, d, default_consumeUnit));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("SearchFolder:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list ;
                }

            }
            //public int GetFolderIDByName(string name,int? id)
            //{
            //    string parentid;
            //    if (id == null) parentid = " is null";
            //    else parentid = "=" + id.ToString();

            //    System.Data.DataTable table = DB.GetData("select " + FolderTable.FolderID
            //        + " from " + FolderTable.Folder
            //        + " where " + FolderTable.FolderName + "='" + name + "'"
            //        );
            //    return Convert.ToInt32(table.Rows[0][0]);
            //}
            public Folder  GetFolderInfoByID(uint id)
            {
                DataTable t = DB.GetData("select * from " + FolderTable.TableName 
                   + " where " + FolderTable.FolderID + "=" + id);

                uint fid = Convert.ToUInt32(t.Rows[0][0].ToString());
                string fname = t.Rows[0][1].ToString();

                uint? p;
                try
                {
                    p = Convert.ToUInt32(t.Rows[0][2]);
                }
                catch
                {
                    p = null;
                }
                DateTime d = Convert.ToDateTime(t.Rows[0][3]);
                string default_consumeUnit = t.Rows[0][4].ToString();
                return new Folder(fid, fname, p, d,default_consumeUnit );
            }
            public List <Folder >  GetFoldersList()
            {
                List<Folder> list = new List<Folder>();
                DataTable t = DB.GetData("select * from " + FolderTable.TableName);
                for (int i = 0; i < t.Rows.Count; i++)
                {

                    uint fid = Convert.ToUInt32(t.Rows[i][0].ToString());
                    string fname = t.Rows[i][1].ToString();
                    uint? p;
                    try
                    {
                        p = Convert.ToUInt32(t.Rows[i][2]);
                    }
                    catch
                    {
                        p = null;
                    }
                    DateTime d = Convert.ToDateTime(t.Rows[i][3]);
                    string default_consumeUnit = t.Rows[0][4].ToString();
                    list.Add(new Folder(fid, fname, p, d,default_consumeUnit ));
                }
                return list;
            }
            public bool IS_Move_Able(Folder DestinationFolder,Folder folder)
            {
                
                if (DestinationFolder == folder) return false;
                if (DestinationFolder == null) return true;
                Folder Parent_temp,Child_Temp;
                Child_Temp = DestinationFolder;

                while (true )
                {

                    Parent_temp = GetParentFolder(Child_Temp);
                    if (Parent_temp == folder ) return false ;
                    if (Parent_temp == null) return true ;

                    Child_Temp = Parent_temp;
                }

            }
            public bool MoveFolders(Folder DestinationFolder,List <Folder> FoldersList)
            {
                if (FoldersList.Count == 0) return false;
                for (int i = 0; i < FoldersList.Count; i++)
                {
                    if(!IS_Move_Able(DestinationFolder,FoldersList[i] ))
                    {
                        MessageBox.Show("لا يمكن نقل مجلد الى مجلد ابن له","خطا",MessageBoxButtons.OK,MessageBoxIcon.Error );
                        return false;
                    }
                }
                    try
                {
                    for(int i=0;i<FoldersList.Count;i++)
                    {
                        string desteniationFolder_id_str;
                        if (DestinationFolder == null)
                            desteniationFolder_id_str = "null";
                        else
                            desteniationFolder_id_str = DestinationFolder.FolderID.ToString();
                        DB.ExecuteSQLCommand("update "
                            +FolderSQL .FolderTable .TableName 
                            +" set "
                            + FolderSQL.FolderTable.ParentFolderID +"="+ desteniationFolder_id_str
                            + " where "
                             + FolderSQL.FolderTable.FolderID  + "=" + FoldersList[i].FolderID
                            );
                    }
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_Folder
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_Folder
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
         }
     
        public class ItemSQL
        {
            DatabaseInterface DB;
            public static class ItemTable
            {

                public const string TableName = "Item_Item";
                public const string ItemID = "ItemID";
                public const string ItemName = "ItemName";
                public const string ItemCompany = "ItemCompany";
                public const string FolderID = "FolderID";

                public const string MarketCode = "MarketCode";
                public const string CreateDate = "CreateDate";
                public const string DefaultConsumeUnit = "DefaultConsumeUnit";
     

            }
            public static class ItemImageTable
            {
                public const string TableName = "Item_ItemImage";
                public const string ItemID = "ItemID";
                public const string Item_Image = "Item_Image";
            }
            public static class ItemFileTable
            {
                public const string TableName = "Item_ItemFiles";
                public const string ItemID = "ItemID";
                public const string ItemImage = "ItemImage";
            }
            public ItemSQL(DatabaseInterface db)
            {
                DB = db;
            }

            public string GetItemPath(Item Item_)
            {
                FolderSQL FolderSQL_ = new FolderSQL(DB);
                List<string> f_path = new List<string>();
                Folder f = Item_.folder;
                string s = "ROOT /";

                while (f.ParentFolderID != null)
                {
                    f_path.Add(f.FolderName);
                    f = FolderSQL_. GetFolderInfoByID(Convert.ToUInt32(f.ParentFolderID));
                    
                }
                f_path.Add(f.FolderName);
                for (int i = f_path.Count - 1; i >= 0; i--)
                    s += f_path[i] + "/";
                return s;
            }
            public string GetItemPath(AvailableItem AvailableItem_)
            {
                FolderSQL FolderSQL_ = new FolderSQL(DB);
                List<string> f_path = new List<string>();
                Folder f = AvailableItem_._Item.folder ;
                string s = "ROOT /";

                while (f.ParentFolderID != null)
                {
                    f_path.Add(f.FolderName);
                    f = FolderSQL_.GetFolderInfoByID(Convert.ToUInt32(f.ParentFolderID));

                }
                f_path.Add(f.FolderName);
                for (int i = f_path.Count - 1; i >= 0; i--)
                    s += f_path[i] + "/";
                return s;
            }
            public bool CreateItem(Folder folder,string itemname,string itemcompany,string marketcode,string DefaultConsumeUnit_)
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";
                string datetime = "'" + time.ToString(format) + "'";
                if (folder == null)
                {
                    MessageBox.Show("لا يمكن اضافة عناصر للمجلد الجذر","خطأ ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return false;
                }
                   
                try
                {
                    DB.ExecuteSQLCommand(" insert into "
                    + ItemTable .TableName
                    + "("
                     + ItemTable.FolderID
                      + ","
                    + ItemTable.ItemName 
                    + ","
                    + ItemTable.ItemCompany 
                    + ","
                    + ItemTable.MarketCode
                     + ","
                    + ItemTable.CreateDate
                       + ","
                    + ItemTable.DefaultConsumeUnit 
                    + ")"
                    + "values"
                    + "("
                    +folder .FolderID
                     + ","
                    + "'" + itemname  + "'"
                    + ","
                     +"'" + itemcompany  + "'"
                     + ","
                    + "'" + marketcode  + "'"
                     + ","
                     +datetime
                      + ","
                     + "'" + DefaultConsumeUnit_ + "'"
                    + ")"
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.INSERT 
                       , DatabaseInterface.Log.Log_Target.Item_Item 
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Item_Item 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool UpdateItem(Item Item_ , string new_itemname, string new_item_company, string new_marketcode,  string new_DefaultConsumeUnit_)
            {


                try
                {

                    DB.ExecuteSQLCommand(" update  "
                    + ItemTable.TableName
                    + " set "
                     + ItemTable.ItemName + "='" + new_itemname+"'"
                      + ","
                    + ItemTable.ItemCompany + "='" + new_item_company + "'"
                    + ","
                    + ItemTable.MarketCode + "='" + new_marketcode + "'"
                     + ","
                    + ItemTable.DefaultConsumeUnit + "='" + new_DefaultConsumeUnit_ + "'"
                    + " where "
                    + ItemTable.ItemID +"="+Item_.ItemID 
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_Item
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_Item
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool DeleteItem(Item item_)
            {
                try
                {
                    DB.ExecuteSQLCommand(" delete from   "
               + ItemSellPriceSql.ItemSellPriceTable.TableName 
               + " where  "
               + ItemSellPriceSql.ItemSellPriceTable.ItemID + "=" + item_.ItemID);


                    DB.ExecuteSQLCommand(" delete from   "
               + ConsumeUnitSql.ConsumeUnitTable.TableName
               + " where  "
               + ConsumeUnitSql.ConsumeUnitTable.ItemID + "=" + item_.ItemID);

                    DB.ExecuteSQLCommand(" delete from   "
                   + ItemRelationShipsSQL .ItemRelationShipsTable.TableName
                   + " where  "
                   + ItemRelationShipsSQL.ItemRelationShipsTable.ItemID + "=" + item_.ItemID
                   +"or "
                   + ItemRelationShipsSQL.ItemRelationShipsTable.AnotherItemID + "=" + item_.ItemID
                   );

                    DB.ExecuteSQLCommand(" delete from   "
                    + ItemSpec_Value_SQL.ItemSpec_Value_Table.TableName
                    + " where  "
                    + ItemSpec_Value_SQL.ItemSpec_Value_Table.ItemID + "=" + item_.ItemID);

                    DB.ExecuteSQLCommand(" delete from   "
                    + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble .TableName 
                    + " where  "
                    + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.ItemID  + "=" + item_.ItemID);

                    DB.ExecuteSQLCommand(" delete from   "
              + ItemFileSQL .ItemFileTable.TableName
              + " where  "
              + ItemFileSQL.ItemFileTable.ItemID + "=" + item_.ItemID);

                    DB.ExecuteSQLCommand(" delete from   "
              + ItemImageTable.TableName
              + " where  "
              + ItemImageTable.ItemID + "=" + item_.ItemID);

                    DB.ExecuteSQLCommand(" delete from   "
                     + ItemTable.TableName
                     + " where  "
                     + ItemTable.ItemID + "=" + item_.ItemID);
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE 
                       , DatabaseInterface.Log.Log_Target.Item_Item
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_Item
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<Item> FilterItemsBySpec(List<ItemSpec_Restrict_Options> ItemSpec_Restrict_Options_List,List <ItemSpec_Value> ItemSpec_Value_List)
            {
                string Cmd_Statemanet= "  ";
                for (int i = 0; i < ItemSpec_Restrict_Options_List.Count; i++)
                {
                    Cmd_Statemanet += "select "
                        + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.ItemID 
                        +" from "
                        + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.TableName
                        +" where concat("
                        +ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.SpecID 
                        +","
                        +  ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.OptionID 
                        + ")="
                        + ItemSpec_Restrict_Options_List[i].ItemSpecRestrict_.SpecID.ToString ()
                        + ItemSpec_Restrict_Options_List[i].OptionID.ToString();
                    if (i != ItemSpec_Restrict_Options_List.Count - 1)
                        Cmd_Statemanet += "  INTERSECT ";
                }
                if (ItemSpec_Value_List.Count > 0 && ItemSpec_Restrict_Options_List.Count > 0) Cmd_Statemanet += " INTERSECT ";
                for (int i = 0; i < ItemSpec_Value_List.Count; i++)
                {
                    Cmd_Statemanet += "select "
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.ItemID 
                                      +" from "
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.TableName
                                      +" where concat("
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.SpecID
                                      + ","
                                      + ItemSpec_Value_SQL.ItemSpec_Value_Table.Value
                                      + ")='"
                                      + ItemSpec_Value_List[i].ItemSpec_.SpecID.ToString()
                                      + ItemSpec_Value_List[i].Value + "'";
                    if (i != ItemSpec_Value_List.Count - 1)
                        Cmd_Statemanet += "  INTERSECT ";
                }
                List<Item> Item_list = new List<Item>();

                DataTable t = new DataTable();
                t = DB.GetData(Cmd_Statemanet
                   );
                for(int i=0;i<t.Rows.Count;i++)
                {
                    Item_list.Add(new ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(t.Rows[i][0])));
                }
                return Item_list;
            }
            public List<Item> GetItemsInFolder(Folder folder)
            {
                List<Item> list = new List<Item>();
                if (folder == null) return list ;

                DataTable t = new DataTable();
                t = DB.GetData("select "
                    +ItemTable .ItemID+","
                    + ItemTable.ItemName  + ","
                    + ItemTable.ItemCompany  + ","
                    + ItemTable.MarketCode + ","
                    + ItemTable.CreateDate + ","
                    +ItemTable .DefaultConsumeUnit 
                    + " from " + ItemTable .TableName
                   + " where " + ItemTable.FolderID  +"="+ folder.FolderID
                    + " order by " + ItemTable.ItemName 
                   );
                for (int i = 0; i < t.Rows.Count; i++)
                {

                    uint itemid = Convert.ToUInt32(t.Rows[i][0].ToString());
                    string itemname = t.Rows[i][1].ToString();
                    string itemcompany = t.Rows[i][2].ToString();
                    string marketcode = t.Rows[i][3].ToString();
                    DateTime d = Convert.ToDateTime(t.Rows[i][4]);
                    string DefaultConsumeUnit_= t.Rows[i][5].ToString();

                    list.Add(new Item(folder,itemid, itemname , itemcompany,marketcode  , d, DefaultConsumeUnit_));
                }
                return list;
            }
            public List<Item> SearchItem(string n)
            {
                List<Item> list = new List<Item>();
                try
                {
                    if (n.Length == 0) return list;
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ItemTable.ItemID + ","
                        + ItemTable.ItemName + ","
                        + ItemTable.ItemCompany + ","
                        + ItemTable.MarketCode + ","
                        + ItemTable.CreateDate + ","
                         + ItemTable.DefaultConsumeUnit + ","
                        + ItemTable.FolderID
                        + " from " + ItemTable.TableName
                       + " where " + ItemTable.ItemName + " like '%" + n + "%'"
                         + " order by " + ItemTable.ItemName
                       );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint itemid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string itemname = t.Rows[i][1].ToString();
                        string itemcompany = t.Rows[i][2].ToString();
                        string marketcode = t.Rows[i][3].ToString();
                        DateTime d = Convert.ToDateTime(t.Rows[i][4]);
                        string DefaultConsumeUnit_ = t.Rows[i][5].ToString();
                        uint folderid = Convert.ToUInt32(t.Rows[i][6]);
                        list.Add(new Item(new FolderSQL(DB).GetFolderInfoByID(folderid), itemid, itemname, itemcompany, marketcode, d, DefaultConsumeUnit_));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("SearchItem:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }
                
            }
            public List<Item> SearchItemInFolder(Folder Folder_, string n)
            {
                List<Item> list = new List<Item>();
                if (Folder_ == null) return list;
                try
                {
                    if (n.Length == 0) return list;
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ItemTable.ItemID + ","
                        + ItemTable.ItemName + ","
                        + ItemTable.ItemCompany + ","
                        + ItemTable.MarketCode + ","
                        + ItemTable.CreateDate + ","
                         + ItemTable.DefaultConsumeUnit 
                       + " from " + ItemTable.TableName
                       + " where " + ItemTable.ItemName + " like '%" + n + "%'"
                       +" and " + ItemTable.FolderID+"="+ Folder_.FolderID 

                       + " order by " + ItemTable.ItemName
                       );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint itemid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string itemname = t.Rows[i][1].ToString();
                        string itemcompany = t.Rows[i][2].ToString();
                        string marketcode = t.Rows[i][3].ToString();
                        DateTime d = Convert.ToDateTime(t.Rows[i][4]);
                        string DefaultConsumeUnit_ = t.Rows[i][5].ToString();

                        list.Add(new Item(Folder_, itemid, itemname, itemcompany, marketcode, d, DefaultConsumeUnit_));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("SearchItemINFolder:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }

            }
            public List<AvailableItem > SearchAvailableItem(string n)
            {
                List<AvailableItem > list = new List<AvailableItem>();
                if (n.Length == 0) return list;
                DataTable t = new DataTable();
                t = DB.GetData("select "
                    + ItemTable.ItemID +","
                    + "[dbo].[Trade_GetAMountInfo_ForITEM]("
                        + ItemTable.ItemID + ")"
                    + " from " + ItemTable.TableName
                   + " where " + ItemTable.ItemName + " like '%" + n + "%'"
                   +" and "
                   +"len([dbo].[Trade_GetAMountInfo_ForITEM]("
                        + ItemTable.ItemID + "))>0"
                   );
                for (int i = 0; i < t.Rows.Count; i++)
                {

                    Item Item_ =GetItemInfoByID ( Convert.ToUInt32(t.Rows[i][0].ToString()));
                    string availableamount = t.Rows[i][1].ToString();
                    string folderpath = new FolderSQL(DB).GetFolderPath(Item_.folder);
                    list.Add(new AvailableItem (Item_ ,availableamount ,folderpath ));
                }
                return list;
            }
            public List<AvailableItem> SearchAvailableItemInFolder(Folder Folder_, string n)
            {
                List<AvailableItem> list = new List<AvailableItem>();
                try
                { 
                
                if (n.Length == 0) return list;
                DataTable t = new DataTable();
                t = DB.GetData("select "
                    + ItemTable.ItemID + ","
                    + "[dbo].[Trade_GetAMountInfo_ForITEM]("
                        + ItemTable.ItemID + ")"
                    + " from " + ItemTable.TableName
                   + " where " + ItemTable.ItemName + " like '%" + n + "%'"
                   + " and "
                   + "len([dbo].[Trade_GetAMountInfo_ForITEM]("
                    + ItemTable.ItemID + "))>0"
                    + " and " + ItemTable.FolderID + "=" + Folder_.FolderID

                   + " order by " + ItemTable.ItemName
                   );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        Item Item_ = GetItemInfoByID(Convert.ToUInt32(t.Rows[i][0].ToString()));
                        string availableamount = t.Rows[i][1].ToString();
                        string folderpath = new FolderSQL(DB).GetFolderPath(Item_.folder);
                        list.Add(new AvailableItem(Item_, availableamount, folderpath));
                    }
                return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("SearchItemINFolder:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }

            }
            public Item GetItemInfoByID(uint id)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ItemTable.ItemID + ","
                        + ItemTable.ItemName + ","
                        + ItemTable.ItemCompany + ","
                        + ItemTable.FolderID + ","
                        + ItemTable.MarketCode + ","
                        + ItemTable.CreateDate + ","
                         + ItemTable.DefaultConsumeUnit  
                        + " from " + ItemTable.TableName
                       + " where " + ItemTable.ItemID + "=" + id);
                    if (t.Rows.Count == 1)
                    {
                        uint itemid = Convert.ToUInt32(t.Rows[0][0].ToString());
                        string itemname = t.Rows[0][1].ToString();
                        string itemcompany = t.Rows[0][2].ToString();
                        uint folderid = Convert.ToUInt32(t.Rows[0][3].ToString());
                        FolderSQL f = new FolderSQL(this.DB);

                        string marketcode = t.Rows[0][4].ToString();
                        DateTime d = Convert.ToDateTime(t.Rows[0][5]);
                        string DefaultConsumeUnit_ = t.Rows[0][6].ToString();
                        return (new Item(f.GetFolderInfoByID(folderid), itemid, itemname, itemcompany, marketcode, d, DefaultConsumeUnit_));

                    }

                    else return null;

                }
                catch (Exception ee)
                {
                    MessageBox.Show("جلب بيانات العنصر:"+ee.Message);
                    return null;
                }
               

            }
            public Item GetItemInfoByName(string name, string company)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ItemTable.ItemID + ","
                        + ItemTable.ItemName + ","
                        + ItemTable.ItemCompany + ","
                        + ItemTable.FolderID + ","
                        + ItemTable.MarketCode + ","
                        + ItemTable.CreateDate + ","
                        + ItemTable.DefaultConsumeUnit  
                        + " from " + ItemTable.TableName
                       + " where " + ItemTable.ItemName  + "='" + name +"'"
                       + " and "  +ItemTable .ItemCompany +"='"+company +"'"
                       );

                    uint itemid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    string itemname = t.Rows[0][1].ToString();
                    string itemcompany = t.Rows[0][2].ToString();
                    uint folderid = Convert.ToUInt32(t.Rows[0][3].ToString());
                    FolderSQL f = new FolderSQL(this.DB);

                    string marketcode = t.Rows[0][4].ToString();
                    DateTime d = Convert.ToDateTime(t.Rows[0][5]);
                    string DefaultConsumeUnit_ = t.Rows[0][6].ToString();
                  

                    return (new Item(f.GetFolderInfoByID(folderid), itemid, itemname, itemcompany, marketcode, d, DefaultConsumeUnit_));
                }
                catch (Exception ee)
                {
                    MessageBox.Show("جلب بيانات العنصر:" + ee.Message);
                    return null;
                }


            }
            public bool MoveItems(Folder DestinationFolder, List<Item > ItemsList)
            {
                if (ItemsList.Count == 0) return false;
                try
                {
                    for (int i = 0; i < ItemsList.Count; i++)
                    {
                        DB.ExecuteSQLCommand("update "
                            + ItemSQL .ItemTable.TableName
                            + " set "
                            + ItemSQL.ItemTable.FolderID  + "=" + DestinationFolder.FolderID
                            + " where "
                             + ItemSQL.ItemTable.ItemID + "=" + ItemsList[i].ItemID 
                            );
                    }
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.UPDATE
                       , DatabaseInterface.Log.Log_Target.Item_Item
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_Item
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public byte [] GetItemImage(Item Item_)
            {
                byte[] image_array  ;
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        +ItemImageTable.Item_Image
                        + " from "
                        +ItemImageTable.TableName
                        +" where "
                        +ItemImageTable.ItemID+" = "+ Item_.ItemID );
                    if (t.Rows.Count > 0)
                    {
                        image_array = (byte[])t.Rows[0][0];
                        return image_array;
                    }
                    else return null;
                    
                }
                catch(Exception ee)
                {
                    MessageBox.Show("GetItemImage:"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool SetItemImage(Item Item_,byte [] Image_)
            {

                try
                {

                    if (!UnSetItemImage(Item_)) throw new Exception();
   
                        SqlCommand command = new SqlCommand();
                        command.Connection = DB.DATABASE_CONNECTION;
                        command.CommandText = @"INSERT INTO Item_ItemImage (ItemID,Item_Image)values(@itemid,@imagee)";
        
                        command.Parameters.AddWithValue("@itemid", Item_.ItemID);
                        command.Parameters.AddWithValue("@imagee", Image_);
                        command.Parameters[0].SqlDbType = SqlDbType.Int;
                        command.Parameters[1].SqlDbType  = SqlDbType.Binary;
                        if (DB.DATABASE_CONNECTION.State == ConnectionState.Closed)
                            DB.DATABASE_CONNECTION.Open();
                       command.ExecuteNonQuery();
                        DB.DATABASE_CONNECTION.Close ();


                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Item_Image
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Item_Image 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UnSetItemImage(Item Item_)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from "
                        +ItemImageTable.TableName
                        +" where "
                        + ItemImageTable.ItemID +"="+Item_.ItemID
                        );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE 
                       , DatabaseInterface.Log.Log_Target.Item_Image 
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_Image 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public List <string > GetAllItemsNameList()
            {
                List<string > list = new List<string>();
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select distinct "
                        + ItemTable.ItemName
                        + " from " + ItemTable.TableName);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        string itemname = t.Rows[i][0].ToString();
                        list.Add(itemname);
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetAllItemsNameList:" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list ;
                }
            }
            public List<string> GetAllCompaniesNameList()
            {
                List<string> list = new List<string>();
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select distinct "
                        + ItemTable.ItemCompany
                        + " from " + ItemTable.TableName);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        string companyname = t.Rows[i][0].ToString();
                        list.Add(companyname);
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetAllCompaniesNameList:" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }
            }


        }
        public class ItemFileSQL
        {
            DatabaseInterface DB;
            public static class ItemFileTable
            {
                public const string TableName = "Item_ItemFiles";
                public const string ItemID = "ItemID";
                public const string FileID = "FileID";
                public const string Item_FileName = "Item_FileName";
                public const string FileDescription = "FileDescription";
                public const string AddDate = "AddDate";
                public const string FileData = "FileData";
            }
        
            public ItemFileSQL(DatabaseInterface db)
            {
                DB = db;
            }
            public bool AddItemFile(Item Item_, string File_Name,string File_Description,byte [] File_Data)
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = DB.DATABASE_CONNECTION;
                    command.CommandText = @"INSERT INTO Item_ItemFiles (ItemID,Item_FileName,FileDescription,FileData)values(@itemid,@FileName,@FileDescription,@FileData)";

                    command.Parameters.AddWithValue("@itemid", Item_.ItemID);
                    command.Parameters.AddWithValue("@FileName", File_Name);
                    command.Parameters.AddWithValue("@FileDescription", File_Description);
                    command.Parameters.AddWithValue("@FileData", File_Data);
                    command.Parameters[0].SqlDbType = SqlDbType.Int;
                    command.Parameters[1].SqlDbType = SqlDbType.Text ;
                    command.Parameters[2].SqlDbType = SqlDbType.Text;
                    command.Parameters[3].SqlDbType = SqlDbType.Binary;
                    if (DB.DATABASE_CONNECTION.State == ConnectionState.Closed)
                        DB.DATABASE_CONNECTION.Open();
                    command.ExecuteNonQuery();
                    DB.DATABASE_CONNECTION.Close();
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Item_File 
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Item_File 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteItemFile(uint file_id)
            {

                try
                {
                    DB.ExecuteSQLCommand(" delete from  "
                        + ItemFileTable.TableName
                        + " where "
                        + ItemFileTable.FileID + "=" + file_id
                        );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_File
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_File 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateFileInfo(uint File_ID,string file_name,string file_description)
            {

                try
                {
                    DB.ExecuteSQLCommand(" update "
                        +ItemFileTable .TableName 
                        +" set "
                        + ItemFileTable.Item_FileName +"='"+file_name +"',"
                        + ItemFileTable.FileDescription + "='" + file_description + "'"
                        +" where "
                        + ItemFileTable.FileID + "=" + File_ID
                        );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_File 
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Item_File 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<ItemFile> GetItemFileList(Item Item_)
            {
                List<ItemFile> ItemFilesList = new List<ItemFile>();
                try
                {
               
                    DataTable t =DB.GetData("select "
                    + ItemFileTable.FileID +","
                    + ItemFileTable.Item_FileName + ","
                    + ItemFileTable.FileDescription + ","
                    + ItemFileTable.AddDate 
                    + " from "
                    + ItemFileTable.TableName
                    + " where "
                    + ItemFileTable.ItemID  + "=" + Item_.ItemID
                    );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint fileid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string filename = t.Rows[i][1].ToString();
                        string filedescription = t.Rows[i][2].ToString();
                        DateTime datetime = Convert.ToDateTime(t.Rows[i][3].ToString());
                        long filesize = GetFileSize (fileid );
                        ItemFilesList.Add(new ItemFile(Item_, fileid, filename,filedescription , filesize, datetime));
                    }
                    return ItemFilesList;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return null;
                }
            }    
            public  long  GetFileSize(uint fileid)
            {
                try
                {

                    DataTable t = DB.GetData("select datalength("
                    + ItemFileTable.FileData
                    + ") from "
                    + ItemFileTable.TableName
                    + " where "
                    + ItemFileTable.FileID + "=" + fileid
                    );
                    if (t.Rows.Count > 0)
                    {
                        return (long)t.Rows[0][0];

                    }
                    else return -1;
                }
                catch (Exception ee)
                {
                    return -1;
                }
            }   
            public byte [] GetFileData(uint fileid)
            {
                try
                {

                    DataTable t = DB.GetData("select "
                    + ItemFileTable.FileData
                    + " from "
                    + ItemFileTable.TableName
                    + " where "
                    + ItemFileTable.FileID + "=" + fileid
                    );
                    if (t.Rows.Count > 0)
                    {
                        return (byte[])t.Rows[0][0];

                    }
                    else return null ;
                }
                catch (Exception ee)
                {

                    return null;
                }
            }

        }

        public class ItemSpec_Restrict_SQL
        {
            DatabaseInterface DB;
            public static class ItemSpec_Restrict_Table
            {
                public const string TableName = "Item_ItemSpec_Restrict";
                public const string SpecID = "SpecID";
                public const string SpecName = "SpecName";
                public const string FolderID = "FolderID"; 
                public const string SpecIndex = "SpecIndex";
            }

            public ItemSpec_Restrict_SQL(DatabaseInterface db)
            {
                DB = db;
            }
   

            public bool AddItemSpecRestrict(Folder Folder_,string ItemSpecName_, uint specindex)
            {
                    try
                    {
                    DB.ExecuteSQLCommand("insert into " + ItemSpec_Restrict_Table.TableName
                            + " ("
                            + ItemSpec_Restrict_Table.FolderID +","
                            + ItemSpec_Restrict_Table.SpecName + ","
                             + ItemSpec_Restrict_Table.SpecIndex  
                            + ")values("
                            +Folder_ .FolderID +","
                            +"'"+ ItemSpecName_ + "'" + ","
                            +specindex 
                            + ")"
                         );
                    DB.AddLog(
                   DatabaseInterface.Log.LogType.INSERT 
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict
                   , ""
                 , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool UpdatetemSpecRestrict(uint id,string newname, uint specindex)
            {

                try
                {

                    DB.ExecuteSQLCommand("update " + ItemSpec_Restrict_Table.TableName
                            + " set "
                            + ItemSpec_Restrict_Table.SpecName + "='"+newname +"',"
                            + ItemSpec_Restrict_Table.SpecIndex  + "="+specindex 
                            + " where "
                            + ItemSpec_Restrict_Table.SpecID +"="+id
                         );
                    DB.AddLog(
                 DatabaseInterface.Log.LogType.UPDATE 
                 , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict
                 , ""
               , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool DeleteItemSpecRestrict(uint id)
            {

                try
                {
                    DB.ExecuteSQLCommand("delete from "
                           + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.TableName
                           + " where "
                           + ItemSpec_Restrict_Value_SQL.ItemSpec_Restrict_Value_TAble.SpecID + "=" + id
                       );
                    DB.ExecuteSQLCommand("delete from " 
                            + ItemSpec_Restrict_Options_SQL.ItemSpec_Restrict_Option_Table.TableName
                            + " where "
                            + ItemSpec_Restrict_Options_SQL.ItemSpec_Restrict_Option_Table.SpecID + "=" + id
                        );
                    DB.ExecuteSQLCommand("delete from " 
                            + ItemSpec_Restrict_Table.TableName
                            + " where "
                            + ItemSpec_Restrict_Table.SpecID + "=" + id
                         );
                    DB.AddLog(
                 DatabaseInterface.Log.LogType.DELETE 
                 , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict
                 , ""
               , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public ItemSpec_Restrict  GetItemSpecRestrictInfoByID(uint specid)
            {
                try
                {
                    FolderSQL foldersql = new FolderSQL(DB);
                    ItemSpec_Restrict m;
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + ItemSpec_Restrict_Table.FolderID + ","
                        + ItemSpec_Restrict_Table.SpecName +","
                         + ItemSpec_Restrict_Table.SpecIndex 
                       + " from " + ItemSpec_Restrict_Table.TableName
                       + " where " + ItemSpec_Restrict_Table.SpecID + "=" + specid);
                    if (t.Rows.Count > 0)
                    {
                        m = new ItemSpec_Restrict(foldersql.GetFolderInfoByID(Convert.ToUInt32(t.Rows[0][0])), specid,  t.Rows[0][1].ToString(), Convert.ToUInt32(t.Rows[0][2]));

                    }
                    else m = null;
                    return m;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب بيانات الخاصية"+ee.Message );
                    return null;
                }
               
            }

            public List<ItemSpec_Restrict  > GetItemSpecRestrictList(Folder  Folder_)
            {
                try
                {
                    List<ItemSpec_Restrict> list = new List<ItemSpec_Restrict>();
                    DataTable t = new DataTable();
                   
                    t = DB.GetData("select "
                        + ItemSpec_Restrict_Table.SpecID + ","
                        + ItemSpec_Restrict_Table.SpecName + ","
                        + ItemSpec_Restrict_Table.SpecIndex 
                       + " from " + ItemSpec_Restrict_Table.TableName
                       + " where " + ItemSpec_Restrict_Table.FolderID + "=" + Folder_.FolderID
                       );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint specid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string specname = t.Rows[i][1].ToString();
                        uint specindex= Convert.ToUInt32(t.Rows[i][2].ToString());

                        list.Add(new ItemSpec_Restrict(Folder_, specid, specname, specindex));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب خاصيات المجلد" + ee.Message);
                    return null;
                }

            }
        }
        public class ItemSpec_Restrict_Options_SQL
        {
            public static class ItemSpec_Restrict_Option_Table
            {
                public const string TableName = "Item_ItemSpec_Restrict_Options";
                public const string SpecID = "SpecID";
                public const string OptionID = "OptionID";
                public const string OptionName = "OptionName";
   

            }
            DatabaseInterface DB;
            public ItemSpec_Restrict_Options_SQL(DatabaseInterface db)
            {
                DB = db;
            }
            public ItemSpec_Restrict_Options Get_ItemSpec_Restrict_Options_Info_ByName(ItemSpec_Restrict ItemSpec_Restrict_, string  option_name)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                   + ItemSpec_Restrict_Option_Table.OptionID
                   + " from  "
                  + ItemSpec_Restrict_Option_Table.TableName
                  + " where "
                  + ItemSpec_Restrict_Option_Table.SpecID  + "=" + ItemSpec_Restrict_.SpecID 
                  + " and "
                  + ItemSpec_Restrict_Option_Table.OptionName + "='" + option_name+"'"

                  );
                if (t.Rows.Count == 1) return new ItemSpec_Restrict_Options(ItemSpec_Restrict_, Convert.ToUInt32(t.Rows[0][0]), option_name);
                else return null;
            }
            public string Get_ItemSpec_Restrict_Options_Name_ByID(uint optionid)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                   +ItemSpec_Restrict_Option_Table.OptionName
                   + " from  "
                  + ItemSpec_Restrict_Option_Table.TableName
                  + " where "
                  + ItemSpec_Restrict_Option_Table.OptionID  + "=" + optionid

                  );
                if (t.Rows.Count > 0) return t.Rows [0][0].ToString ();
                else return null;
            }
            public bool Add_ItemSpec_Restrict_Option(ItemSpec_Restrict  ItemSpecRestrict_,string optionname)
            {

                    if(optionname.Length  == 0)
                    {
                        MessageBox.Show("القيمة يجب ان لا تكون فارغة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false ;
                    }
                 if (IsExists(ItemSpecRestrict_,optionname ))
                {
                    MessageBox.Show("القيمة مودخلة بلفعل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                try
                    {
                        DB.ExecuteSQLCommand("insert into  "
                         + ItemSpec_Restrict_Option_Table.TableName
                         + " ( "
                         + ItemSpec_Restrict_Option_Table.SpecID + ","
                         + ItemSpec_Restrict_Option_Table.OptionName 
                         + ")values( "
                         + ItemSpecRestrict_.SpecID + ","
                         + "'"+ optionname + "'"  
                         + ")"
                         );
                    DB.AddLog(
              DatabaseInterface.Log.LogType.INSERT
              , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Option
              , ""
            , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Option
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool Update_ItemSpec_Restrict_Option(uint OptionID, string new_optioname)
            {

                    if (new_optioname.Length == 0)
                    {
                        MessageBox.Show("القيمة يجب ان لا تكون فارغة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                 
                    try
                    {
                        DB.ExecuteSQLCommand("update  "
                         + ItemSpec_Restrict_Option_Table.TableName
                         + " set "
                         + ItemSpec_Restrict_Option_Table.OptionName + "=" + "'" + new_optioname + "'" 
                         + " where  "
                         + ItemSpec_Restrict_Option_Table.OptionID + "=" + OptionID
                         );
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.UPDATE 
                          , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Option
                          , ""
                        , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Option
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_ItemSpec_Restrict_Option(uint OptionID)
            {

                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                     + ItemSpec_Restrict_Option_Table.TableName
                     + " where  "
                     + ItemSpec_Restrict_Option_Table.OptionID + "=" + OptionID
                     );
                    DB.AddLog(
              DatabaseInterface.Log.LogType.DELETE 
              , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Option
              , ""
            , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Option
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool IsExists(ItemSpec_Restrict  ItemSpecRestrict_,string optionname)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select * from  "
                  + ItemSpec_Restrict_Option_Table.TableName
                  + " where "
                  + ItemSpec_Restrict_Option_Table.SpecID  + "=" + ItemSpecRestrict_.SpecID 
                  +" and "
                  + ItemSpec_Restrict_Option_Table.OptionName+"='"+ optionname + "'"
                  );
                if (t.Rows.Count > 0) return true;
                else return false;
            }
            public List<ItemSpec_Restrict_Options > GetItemSpec_Restrict_Options_List(ItemSpec_Restrict ItemSpec_Restrict_)
            {
                try
                {
                    List<ItemSpec_Restrict_Options> ItemSpec_Restrict_Options_List = new List<ItemSpec_Restrict_Options>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                      + ItemSpec_Restrict_Option_Table.OptionID + ","
                      + ItemSpec_Restrict_Option_Table.OptionName
                       + " from  "
                     + ItemSpec_Restrict_Option_Table.TableName
                     + " where "
                     + ItemSpec_Restrict_Option_Table.SpecID + "=" + ItemSpec_Restrict_.SpecID
                     );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        ItemSpec_Restrict_Options ItemSpec_Restrict_Options_ = new ItemSpec_Restrict_Options(ItemSpec_Restrict_, Convert.ToUInt32(t.Rows[i][0].ToString()), t.Rows[i][1].ToString());
                        ItemSpec_Restrict_Options_List.Add(ItemSpec_Restrict_Options_);
                    }
                    return ItemSpec_Restrict_Options_List;
                }
                catch
                {
                    return null;
                }
               
            }
            
  
        }
        public  class ItemSpec_Restrict_Value_SQL
        {
            public static class ItemSpec_Restrict_Value_TAble
            {
                public const string TableName = "Item_ItemSpec_Restrict_Value";
                public const string ItemID = "ItemID";
                public const string OptionID = "OptionID";
                public const string SpecID = "SpecID";
            }
            DatabaseInterface DB;
            public ItemSpec_Restrict_Value_SQL(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public List <ItemSpec_Restrict_Value> Get_ItemValuesList_For_SpecRestrict(Item Item_, ItemSpec_Restrict ItemSpec_Restrict_)
            {
               
                try
                {
                    List<ItemSpec_Restrict_Value> ItemSpec_Restrict_ValueList = new List<ItemSpec_Restrict_Value>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ItemSpec_Restrict_Value_TAble.OptionID  
                       + " from " + ItemSpec_Restrict_Value_TAble.TableName
                       + " where "
                       + ItemSpec_Restrict_Value_TAble.ItemID  + "=" + Item_.ItemID
                       + " and  "
                       + ItemSpec_Restrict_Value_TAble.SpecID + "=" + ItemSpec_Restrict_.SpecID 
                       );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint optionid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string optionname = new ItemSpec_Restrict_Options_SQL(DB).Get_ItemSpec_Restrict_Options_Name_ByID(optionid);

                        ItemSpec_Restrict_ValueList.Add(new ItemSpec_Restrict_Value(Item_, ItemSpec_Restrict_, new ItemSpec_Restrict_Options(ItemSpec_Restrict_, optionid, optionname)));
                    }
                    return ItemSpec_Restrict_ValueList;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("   فشل جلب قيم العنصر للخاصية" + ee.Message);
                    return null;
                }

            }
            public bool Add_ItemSpec_Restrict_Value(Item Item_,ItemSpec_Restrict_Options ItemSpec_Restrict_Options_)
            {
                try
                {
                    if(IS_Exists_ItemSpec_Restrict_Value(Item_, ItemSpec_Restrict_Options_) )
                    {
                        MessageBox.Show("القيمة مدخلة بلفعل","خطا",MessageBoxButtons.OK ,MessageBoxIcon.Error );
                        return false;
                    }
   
                    DB.ExecuteSQLCommand("insert into "
                        + ItemSpec_Restrict_Value_TAble.TableName 
                        +"("
                        + ItemSpec_Restrict_Value_TAble.ItemID+","
                        + ItemSpec_Restrict_Value_TAble.SpecID  + ","
                        + ItemSpec_Restrict_Value_TAble.OptionID
                        + ")values("
                        +Item_ .ItemID +","
                        + ItemSpec_Restrict_Options_.ItemSpecRestrict_.SpecID +","
                        + ItemSpec_Restrict_Options_.OptionID 
                        +")"

                        );
                    DB.AddLog(
             DatabaseInterface.Log.LogType.INSERT
             , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Value 
             , ""
           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Value
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool IS_Exists_ItemSpec_Restrict_Value(Item Item_, ItemSpec_Restrict_Options ItemSpec_Restrict_Options_)
            {
                try
                {
                    DataTable t =   DB.GetData ("select * from  "
                        + ItemSpec_Restrict_Value_TAble.TableName
                        + " where "
                        + ItemSpec_Restrict_Value_TAble.ItemID + "=" + Item_.ItemID
                        + ItemSpec_Restrict_Value_TAble.SpecID + "="+ ItemSpec_Restrict_Options_.ItemSpecRestrict_.SpecID
                        + ItemSpec_Restrict_Value_TAble.OptionID + "=" + ItemSpec_Restrict_Options_.OptionID
                        );
                    if (t.Rows.Count > 0)
                        return true;
                    else return false;
                }
                catch
                {
                    return false;
                }
            }
            public bool Delete_ItemValueRestrict(Item Item_, ItemSpec_Restrict_Options ItemSpec_Restrict_Options_)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from  "
                        + ItemSpec_Restrict_Value_TAble.TableName
                        + " where "
                        + ItemSpec_Restrict_Value_TAble.ItemID + "=" + Item_.ItemID +" and "
                        + ItemSpec_Restrict_Value_TAble.SpecID + "="+ ItemSpec_Restrict_Options_.ItemSpecRestrict_.SpecID + " and "
                        + ItemSpec_Restrict_Value_TAble.OptionID  + "="+ ItemSpec_Restrict_Options_.OptionID 
                       
                      

                        );
                    DB.AddLog(
            DatabaseInterface.Log.LogType.DELETE 
            , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Value
            , ""
          , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Restrict_Value
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public class ItemSpecSQL
        {
            DatabaseInterface DB;
            public static class ItemSpecTable
            {
                public const string TableName = "Item_ItemSpec";
                public const string SpecID = "SpecID";
                public const string SpecName = "SpecName";
                public const string FolderID = "FolderID";
                public const string SpecIndex = "SpecIndex";
            }

            public ItemSpecSQL(DatabaseInterface db)
            {
                DB = db;
            }


            public bool AddItemSpec(Folder Folder_, string ItemSpecName_,uint specindex)
            {
                try
                {
                    DB.ExecuteSQLCommand("insert into " + ItemSpecTable.TableName
                            + " ("
                            + ItemSpecTable.FolderID + ","
                            + ItemSpecTable.SpecName + ","
                            + ItemSpecTable.SpecIndex 
                            + ")values("
                            + Folder_.FolderID + ","
                            + "'" + ItemSpecName_ + "'," 
                            +specindex 
                            + ")"
                         );
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.INSERT
                        , DatabaseInterface.Log.Log_Target.Item_Spec
                        , ""
                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_Spec
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool UpdatetemSpec(uint id, string newname,uint specindex)
            {

                try
                {

                    DB.ExecuteSQLCommand("update " + ItemSpecTable.TableName
                            + " set "
                            + ItemSpecTable.SpecName + "='" + newname + "',"
                            + ItemSpecTable.SpecIndex  + "=" + specindex  
                            + " where "
                            + ItemSpecTable.SpecID + "=" + id
                         );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                      , DatabaseInterface.Log.Log_Target.Item_Spec
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public bool DeleteItemSpec(uint id)
            {

                try
                {
                    DB.ExecuteSQLCommand("delete from " + ItemSpec_Value_SQL .ItemSpec_Value_Table.TableName
                            + " where "
                            + ItemSpec_Value_SQL.ItemSpec_Value_Table.SpecID + "=" + id
                         );
                    DB.ExecuteSQLCommand("delete from " + ItemSpecTable.TableName
                            + " where "
                            + ItemSpecTable.SpecID + "=" + id
                         );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_Spec
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            public ItemSpec GetItemSpecInfoByID(uint itemspecid)
            {
                try
                {
                    FolderSQL foldersql = new FolderSQL(DB);;
                    ItemSpec m;
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + ItemSpecTable.FolderID + ","
                        + ItemSpecTable.SpecName+","
                        + ItemSpecTable.SpecIndex 
                       + " from " + ItemSpecTable.TableName
                       + " where " + ItemSpecTable.SpecID + "=" + itemspecid);
                    if (t.Rows.Count > 0)
                    {
 
                        m = new ItemSpec(foldersql.GetFolderInfoByID(Convert.ToUInt32(t.Rows[0][0])), itemspecid, t.Rows[0][1].ToString(), Convert.ToUInt32(t.Rows[0][2]));

                    }
                    else m = null;
                    return m;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب بيانات الخاصية" + ee.Message);
                    return null;
                }

            }

            public List<ItemSpec> GetItemSpecList(Folder Folder_)
            {
                try
                {
                    List<ItemSpec> list = new List<ItemSpec>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + ItemSpecTable.SpecID + ","
                        + ItemSpecTable.SpecName + ","
                         + ItemSpecTable.SpecIndex 
                       + " from " + ItemSpecTable.TableName
                       + " where " + ItemSpecTable.FolderID + "=" + Folder_.FolderID);

                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint specid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string specname = t.Rows[i][1].ToString();
                        uint specindex= Convert.ToUInt32(t.Rows[i][2].ToString());
                        list.Add(new ItemSpec(Folder_, specid, specname, specindex));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("   فشل جلب خاصيات المجلد النصية" + ee.Message);
                    return null;
                }

            }
        }
        public class ItemSpec_Value_SQL
        {
            public static class ItemSpec_Value_Table
            {
                public const string TableName = "Item_ItemSpec_Value";
                public const string ItemID = "ItemID";
                public const string SpecID = "SpecID";
                public const string Value = "Value";

            }
            DatabaseInterface DB;
            public ItemSpec_Value_SQL(DatabaseInterface db)
            {
                DB = db;
            }
            public bool SetItemValue(Item Item_, ItemSpec ItemSpec_,string value)
            {
                try
                {
                    UNSetItemValueRestrict(Item_, ItemSpec_);

                    DB.ExecuteSQLCommand("insert into "
                        + ItemSpec_Value_Table.TableName
                        + "("
                        + ItemSpec_Value_Table.ItemID + ","
                        + ItemSpec_Value_Table.SpecID + ","
                        + ItemSpec_Value_Table.Value 
                        + ")values("
                        + Item_.ItemID + ","
                        + ItemSpec_.SpecID + ","
                        + "'"+value +"'"
                        + ")"

                        );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                      , DatabaseInterface.Log.Log_Target.Item_Spec_Value
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Value 
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UNSetItemValueRestrict(Item Item_, ItemSpec ItemSpec_)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from  "
                        + ItemSpec_Value_Table.TableName
                        + " where "
                        + ItemSpec_Value_Table.ItemID + "=" + Item_.ItemID+" and "
                        + ItemSpec_Value_Table.SpecID + "=" + ItemSpec_.SpecID


                        );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_Spec_Value
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_Spec_Value
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            public ItemSpec_Value  GetItemSpec_Value(Item item, ItemSpec ItemSpec_)
            {
                DataTable t = DB.GetData("select "
                     + ItemSpec_Value_Table.Value 
                   + " from " + ItemSpec_Value_Table.TableName
                   + " where " + ItemSpec_Value_Table.SpecID + "=" + ItemSpec_.SpecID
                   + " and " + ItemSpec_Value_Table.ItemID + "=" + item.ItemID);
                if (t.Rows.Count > 0)
                {

                    return new ItemSpec_Value(item, ItemSpec_, t.Rows[0][0].ToString ());
                }
                else
                {
                    return null;
                }
            }
 
        }
        public class ItemRelationShipsSQL
        {
            public static class ItemRelationShipsTable
            {
                public const string TableName = "Item_ItemRelationShips";
                public const string ItemID = "ItemID";
                public const string AnotherItemID = "AnotherItemID";
                public const string RelationShip = "RelationShip";
                public const string Notes = "Notes";
            }

            DatabaseInterface DB;
            public ItemRelationShipsSQL(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public bool ISDataExists(Item item, Item anotheritem)
            {
                try
                {
                    DataTable t = DB.GetData("select * from  "
                  + ItemRelationShipsTable.TableName
                  + " where ("
                  + ItemRelationShipsTable.ItemID + "=" + item.ItemID 
                  + " and "
                  + ItemRelationShipsTable.AnotherItemID     + "=" + anotheritem  .ItemID
                  + ") or ("
                  + ItemRelationShipsTable.ItemID   + "=" + anotheritem .ItemID
                  + " and "
                  + ItemRelationShipsTable.AnotherItemID + "=" + item  .ItemID +")"

                        );
                    if (t.Rows.Count > 0) return true;
                    else return false;
                }
                catch(SqlException sqlEx)
                {
                    throw new Exception("حدث خطأ اثناء الاتصال بقاعدة البيانات", sqlEx);
                }
            }
            public bool AddItemRelation(Item item, Item anotheritem, uint relation,string notes)
            {
                try
                {
                    if (ISDataExists(item, anotheritem))
                    {
                        MessageBox.Show("توجد علاقة بين العنصرين معرفة مسبقا","خطا",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return false;
                    }
                    DB.ExecuteSQLCommand("insert into  "
                        + ItemRelationShipsTable.TableName
                        + " ( "
                        + ItemRelationShipsTable.ItemID + ","
                        + ItemRelationShipsTable.AnotherItemID  + ","
                        + ItemRelationShipsTable.RelationShip   + ","
                        + ItemRelationShipsTable.Notes 
                        + ")values( "
                        + item .ItemID  + ","
                        + anotheritem .ItemID + ","
                        +relation  + ","
                        +"'"+notes +"'"
                        + ")"
                        );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                      , DatabaseInterface.Log.Log_Target.Item_RealtionShip
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_RealtionShip
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateItemRelation(Item item, Item anotheritem, uint relation, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand("update   "
                        + ItemRelationShipsTable.TableName   
                        +" set "         
                        + ItemRelationShipsTable.RelationShip + "="+relation +","
                         + ItemRelationShipsTable.AnotherItemID + "=" + anotheritem.ItemID + ","
                        + ItemRelationShipsTable.Notes+"='"+notes +"'"
                        + " where  "
                        + ItemRelationShipsTable.ItemID + "="+item .ItemID 
                        +" and "
                        + ItemRelationShipsTable.AnotherItemID + "="+anotheritem .ItemID 
                        );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                      , DatabaseInterface.Log.Log_Target.Item_RealtionShip
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Item_RealtionShip
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteItemRelation(uint  first_itemid,uint second_itemid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                                            + ItemRelationShipsTable.TableName
                                            + " where "
                                            + ItemRelationShipsTable.ItemID + "=" + first_itemid
                                            + "  and  "
                                            + ItemRelationShipsTable.AnotherItemID  + "=" + second_itemid
                                            );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Item_RealtionShip
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_RealtionShip
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            internal ItemRelation GetItemRealtion(Item sourceItem, Item anotherItem)
            {
                try
                {

                    DataTable t = DB.GetData("select "
                   + ItemRelationShipsTable.RelationShip + ","
                   + ItemRelationShipsTable.Notes
                   + " from  "
                   + ItemRelationShipsTable.TableName
                   + " where "
                   + ItemRelationShipsTable.ItemID + "=" + sourceItem.ItemID
                   + " and "
                   + ItemRelationShipsTable.AnotherItemID + "=" + anotherItem.ItemID
                   );
                    if (t.Rows.Count == 1)
                    {
       
                        uint relation = Convert.ToUInt32(t.Rows[0][0].ToString());
                        string notes = t.Rows[0][1].ToString();
                        return new ItemRelation(sourceItem, anotherItem, relation, false, notes);
                    }
                    else 
                    return null;

                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return null;
                }
               
            }
            public List <ItemRelation > GetItemRelationsList(Item item)
            {
                
                List<ItemRelation> ItemRelationList = new List<ItemRelation>();
                DataTable t = DB.GetData("select "
                  + ItemRelationShipsTable.ItemID +","
                  + ItemRelationShipsTable.AnotherItemID  + ","
                  + ItemRelationShipsTable.RelationShip + ","
                  + ItemRelationShipsTable.Notes  
                  + " from  "
                  + ItemRelationShipsTable.TableName
                  + " where "
                  + ItemRelationShipsTable.ItemID + "=" + item.ItemID
                  + " or "
                  + ItemRelationShipsTable.AnotherItemID  + "=" + item.ItemID
                  );
                
                ItemSQL ItemSQL_ = new ItemSQL(DB);
                for (int i=0;i<t.Rows .Count;i++)
                {
                    uint table_itemid1 = Convert.ToUInt32(t.Rows[i][0].ToString());
                    uint table_itemid2 = Convert.ToUInt32(t.Rows[i][1].ToString());
                    uint relation = Convert.ToUInt32 (t.Rows [i][2].ToString ());
                    string notes = t.Rows[i][3].ToString();
                    ItemRelation ItemRelation_;
                    if (table_itemid1 == item .ItemID )
                     ItemRelation_ = new ItemRelation(item ,ItemSQL_.GetItemInfoByID (table_itemid2),relation,false , notes );
                    else
                    {
                        switch (relation)
                        {
                            case 0:break;
                            case 1:relation = 2;break;
                            case 2:relation = 1;break;
                        }
                        ItemRelation_ = new ItemRelation(item, ItemSQL_.GetItemInfoByID(table_itemid1), relation, true , notes);

                    }

                    ItemRelationList.Add(ItemRelation_);
                }
                return ItemRelationList;
            }

         
        }
        public class ConsumeUnitSql
        {
            public static class ConsumeUnitTable
            {
                public const string TableName = "Item_ComsumeUnits";
                public const string ConsumeUnitID = "ConsumeUnitID";
                public const string ConsumeUnitName = "ConsumeUnitName";
                public const string ItemID = "ItemID";
                public const string Factor = "Factor";
            }
            DatabaseInterface DB;
            public ConsumeUnitSql(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public ConsumeUnit GetConsumeAmountinfo(uint ConsumeUnitid)
            {

                DataTable t = DB.GetData("select "
                + ConsumeUnitTable.ConsumeUnitID + ","
                + ConsumeUnitTable.ConsumeUnitName + ","
                 + ConsumeUnitTable.ItemID  + ","
                  + ConsumeUnitTable.Factor  
                + " from  "
                + ConsumeUnitTable.TableName
                + " where "
                + ConsumeUnitTable.ConsumeUnitID + "=" + ConsumeUnitid);
                if (t.Rows.Count == 0) return null;
                else return new ConsumeUnit(Convert.ToUInt32(t.Rows[0][0].ToString()), t.Rows[0][1].ToString(),new ItemSQL (DB).GetItemInfoByID(Convert .ToUInt32 (t.Rows[0][2].ToString())),Convert .ToDouble (t.Rows[0][3].ToString()));
            }
       

            public bool AddConsumeUnit(Item item, string ConsumeUnitname,double factor)
            {
                try
                {
                    if(ConsumeUnitname ==item .DefaultConsumeUnit )
                    {
                        MessageBox.Show("وسم وحدة التوزيع يجب ان يكون مختلف عن اسم وحدة التوزيع الافتراضية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
  
                    DB.ExecuteSQLCommand("insert into  "
                        + ConsumeUnitTable.TableName
                        + " ( "
                        + ConsumeUnitTable.ConsumeUnitName+","
                         + ConsumeUnitTable.ItemID  + ","
                          + ConsumeUnitTable.Factor  
                        + ")values( "
                        + "'" + ConsumeUnitname + "'"+","
                        + item .ItemID  + ","
                        +factor 
                        + ")"
                        );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.INSERT
                       , DatabaseInterface.Log.Log_Target.Item_ConsumeUint
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Item_ConsumeUint
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool UpdateConsumeUnit(Item item,uint ConsumeUnitid, string newConsumeUnitname,double Factor)
            {
                try
                {
                    if(item.DefaultConsumeUnit == newConsumeUnitname)
                    {
                        MessageBox.Show("وسم وحدة التوزيع يجب ان يكون مختلف عن اسم وحدة التوزيع الافتراضية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    DB.ExecuteSQLCommand("update   "
                        + ConsumeUnitTable.TableName
                        + " set "
                        + ConsumeUnitTable.ConsumeUnitName + "='" + newConsumeUnitname + "',"
                         + ConsumeUnitTable.Factor  + "=" + Factor  
                        + " where "
                        + ConsumeUnitTable.ConsumeUnitID + "=" + ConsumeUnitid
                        );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                      , DatabaseInterface.Log.Log_Target.Item_ConsumeUint
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Item_ConsumeUint
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteConsumeUnit(uint ConsumeUnitid)
            {
                try
                {

                    DB.ExecuteSQLCommand("Delete from    "
                        + ItemSellPriceSql.ItemSellPriceTable .TableName
                        + " where "
                        + ItemSellPriceSql.ItemSellPriceTable.ConsumeUnitID + "=" + ConsumeUnitid
                        );
                    DB.ExecuteSQLCommand("Delete from    "
                        + ConsumeUnitTable.TableName
                        + " where "
                        + ConsumeUnitTable.ConsumeUnitID + "=" + ConsumeUnitid
                        );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE 
                       , DatabaseInterface.Log.Log_Target.Item_ConsumeUint
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_ConsumeUint
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<ConsumeUnit > GetConsumeUnitList(Item item_)
            {
                try
                {
                    List<ConsumeUnit> list = new List<ConsumeUnit>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + ConsumeUnitTable.ConsumeUnitID + ","
                           + ConsumeUnitTable.ConsumeUnitName + ","
                          + ConsumeUnitTable.Factor
                        + " from "
                        + ConsumeUnitTable.TableName
                         + " where "
                        + ConsumeUnitTable.ItemID  + "=" + item_.ItemID 
                       );

                    list.Add(new ConsumeUnit(0, item_.DefaultConsumeUnit, item_, 1));
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        ConsumeUnit m = new ConsumeUnit(Convert.ToUInt32(t.Rows[i][0]), t.Rows[i][1].ToString(), item_, Convert.ToDouble(t.Rows[i][2].ToString()));
                        list.Add(m);
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("فشل جلب قائمة وحدات التوزيع:", ee.Message);
                    return null;
                }

            }
        }
        public class ItemSellPriceSql
        {
            public static class ItemSellPriceTable
            {
                public const string TableName = "Item_SellPrices";
                public const string SellPriceID = "SellPriceID";
                public const string ItemID = "ItemID";
                public const string TradeStateID = "TradeStateID";
                public const string ConsumeUnitID = "ConsumeUnitID";
                public const string SellTypeID = "SellTypeID";
                public const string Price = "Price";
            }
            DatabaseInterface DB;
            public ItemSellPriceSql(DatabaseInterface DB_)
            {
                DB = DB_;
            }
            public bool IsPriceSet(Item item_, TradeState TradeState_, ConsumeUnit ConsumeUnit_, SellType SellType_)
            {
                try
                {
                    string cid_string = " is null";
                    if(ConsumeUnit_ !=null )
                    if (ConsumeUnit_.ConsumeUnitID != 0 ) cid_string = "=" + ConsumeUnit_.ConsumeUnitID.ToString();

                    DataTable t = DB.GetData("select * from   "
                        + ItemSellPriceTable.TableName
                        + " where "
                       + ItemSellPriceTable.ItemID + "=" + item_.ItemID + " and "
                        + ItemSellPriceTable.TradeStateID + "=" + TradeState_.TradeStateID + " and "
                        + ItemSellPriceTable.ConsumeUnitID + cid_string + " and "
                        + ItemSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID

                        );
                    if (t.Rows.Count > 0)
                        return true;
                    else return false;

                }
                catch (Exception ee)
                {
                    throw new Exception ("IsPriceSet :" + ee.Message);

                }
                
            }
             public bool SetItemPrice(Item item_, TradeState TradeState_, ConsumeUnit ConsumeUnit_, SellType SellType_,double price)
            {
                bool is_price_set = false;
                try
                {
                    is_price_set = IsPriceSet(item_, TradeState_, ConsumeUnit_, SellType_);
                }
                catch(Exception ee)
                {
                    MessageBox.Show("فشل ضبط التسعير:" +ee.Message ,"",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return false;
                }
                if(!is_price_set )
                {
                    string cid_string = "  null";
                    if (ConsumeUnit_ != null)
                        if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = ConsumeUnit_.ConsumeUnitID.ToString();
                    try
                    {
              

                        DB.ExecuteSQLCommand("insert into  "
                            + ItemSellPriceTable.TableName
                            + " ( "
                            + ItemSellPriceTable.ItemID + ","
                            + ItemSellPriceTable.TradeStateID + ","
                             + ItemSellPriceTable.ConsumeUnitID + ","
                              + ItemSellPriceTable.SellTypeID + ","
                            + ItemSellPriceTable.Price
                            + ")values( "
                            + item_.ItemID + ","
                            + TradeState_.TradeStateID  + ","
                            + cid_string + ","
                            + SellType_.SellTypeID + ","
                            + price
                            + ")"
                            );
                        DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                      , DatabaseInterface.Log.Log_Target.Item_SellPrice 
                      , ""
                    , true, "");
                        return true;
                    }
                    catch (Exception ee)
                    {
                        DB.AddLog(
                          DatabaseInterface.Log.LogType.INSERT
                       , DatabaseInterface.Log.Log_Target.Item_SellPrice
                          , ""
                        , false, ee.Message);
                        MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {

                    string cid_string = " is null";
                    if (ConsumeUnit_ != null)
                        if (ConsumeUnit_.ConsumeUnitID != 0) cid_string ="="+ ConsumeUnit_.ConsumeUnitID.ToString();
                    try
                    {
                        DB.ExecuteSQLCommand( "update  "
                            + ItemSellPriceTable.TableName
                            + " set "
                            + ItemSellPriceTable.Price+"="+price 
                            +" where "
                            + ItemSellPriceTable.ItemID +"="+ item_.ItemID + " and "
                            + ItemSellPriceTable.TradeStateID + "=" + TradeState_.TradeStateID  + " and "
                             + ItemSellPriceTable.ConsumeUnitID + cid_string + " and "
                              + ItemSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID 
                            );
                        DB.AddLog(
                     DatabaseInterface.Log.LogType.UPDATE 
                     , DatabaseInterface.Log.Log_Target.Item_SellPrice
                     , ""
                   , true, "");
                        return true;
                    }
                    catch (Exception ee)
                    {
                        DB.AddLog(
                          DatabaseInterface.Log.LogType.UPDATE 
                       , DatabaseInterface.Log.Log_Target.Item_SellPrice
                          , ""
                        , false, ee.Message);
                        MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                
            }

            public bool UNSetItemPrice(Item item_, TradeState TradeState_, ConsumeUnit ConsumeUnit_, SellType SellType_)
            {
                string cid_string = " is null";
                if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = "=" + ConsumeUnit_.ConsumeUnitID.ToString();
                try
                {

                    DB.ExecuteSQLCommand("delete from   "
                        + ItemSellPriceTable.TableName
                        + " where "
                       + ItemSellPriceTable.ItemID + "=" + item_.ItemID + " and "
                       + ItemSellPriceTable.TradeStateID + "=" + TradeState_.TradeStateID  + " and "
                        + ItemSellPriceTable.ConsumeUnitID + cid_string + " and "
                        + ItemSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID
                        
                        );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.DELETE 
                     , DatabaseInterface.Log.Log_Target.Item_SellPrice
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                   , DatabaseInterface.Log.Log_Target.Item_SellPrice
                      , ""
                    , false, ee.Message);
                    MessageBox.Show("", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public double? GetPrice(Item item, TradeState TradeState_, SellType SellType_, ConsumeUnit ConsumeUnit_)
            {
                try
                {

                    string cid_string = " is null";
                    if(ConsumeUnit_ !=null)
                    if (ConsumeUnit_.ConsumeUnitID != 0) cid_string = "="+ConsumeUnit_.ConsumeUnitID.ToString();
                    double? price;

                    DataTable t = DB.GetData("select "
                          + ItemSellPriceTable.Price
                        + " from "
                        + ItemSellPriceTable.TableName
                         + " where "
                        + ItemSellPriceTable.ItemID + "=" + item.ItemID
                        + " and "
                        + ItemSellPriceTable.TradeStateID + "=" + TradeState_.TradeStateID
                        + " and "
                         + ItemSellPriceTable.SellTypeID + "=" + SellType_.SellTypeID
                        + " and "
                         + ItemSellPriceTable.ConsumeUnitID + cid_string);

                    if (t.Rows.Count == 1)
                    {
                        price = Convert.ToDouble(t.Rows[0][0]);
                    }
                    else price = null;
                    return price;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);return null;
                }
            }
            public List<ItemSellPrice > GetItemPrices(Item item_,TradeState TradeState_)
            {
                try
                {
                    List<ItemSellPrice> list = new List<ItemSellPrice>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + ItemSellPriceTable.ConsumeUnitID + ","
                           + ItemSellPriceTable.SellTypeID  + ","
                          + ItemSellPriceTable.Price 
                        + " from "
                        + ItemSellPriceTable.TableName
                         + " where "
                        + ItemSellPriceTable.ItemID + "=" + item_.ItemID
                        + " and "
                        + ItemSellPriceTable.TradeStateID + "=" + TradeState_.TradeStateID

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
                            CU = new ConsumeUnit (0,item_ .DefaultConsumeUnit ,item_ ,1);
                        }
                        ItemSellPrice m = new ItemSellPrice( item_, TradeState_, CU, new SellTypeSql (DB).GetSellTypeinfo(Convert .ToUInt32 (t.Rows[i][1].ToString())),Convert .ToDouble (t.Rows[i][2].ToString()));
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
            public List<ItemSellPrice> GetItemPrices(Item item_)
            {
                try
                {
                    List<ItemSellPrice> list = new List<ItemSellPrice>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        +ItemSellPriceTable .TradeStateID +","
                         + ItemSellPriceTable.ConsumeUnitID + ","
                           + ItemSellPriceTable.SellTypeID + ","
                          + ItemSellPriceTable.Price
                        + " from "
                        + ItemSellPriceTable.TableName
                         + " where "
                        + ItemSellPriceTable.ItemID + "=" + item_.ItemID


                       );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        TradeState TradeState_ = new TradeStateSQL(DB).GetTradeStateBYID(Convert.ToUInt32(t.Rows[i][0].ToString()));
                        ConsumeUnit CU;
                        try
                        {
                            CU = new ConsumeUnitSql(DB).GetConsumeAmountinfo(Convert.ToUInt32(t.Rows[i][1].ToString()));
                        }
                        catch
                        {
                            CU = new ConsumeUnit(0, item_.DefaultConsumeUnit, item_, 1);
                        }
                        ItemSellPrice m = new ItemSellPrice(item_, TradeState_, CU, new SellTypeSql(DB).GetSellTypeinfo(Convert.ToUInt32(t.Rows[i][2].ToString())), Convert.ToDouble(t.Rows[i][3].ToString()));
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
       
        //public class ItemBuySellStateSql
        //{
        //    public static class ItemBuySellStateTable
        //    {
        //        public const string TableName = "Item_ItemBuySellState";
        //        public const string ItemBuySellStateID = "ItemBuySellStateID";
        //        public const string ItemBuySellStateName = "ItemBuySellStateName";
        //    }
        //    DatabaseInterface DB;
        //    public ItemBuySellStateSql(DatabaseInterface DB_)
        //    {
        //        DB = DB_;
        //    }
        //    public ItemBuySellState GetItemBuySellStateinfo(uint itembuysellstateid)
        //    {

        //        DataTable t = DB.GetData("select "
        //        + ItemBuySellStateTable.ItemBuySellStateID + ","
        //        + ItemBuySellStateTable.ItemBuySellStateName
        //        + " from  "
        //        + ItemBuySellStateTable.TableName
        //        + " where "
        //        + ItemBuySellStateTable.ItemBuySellStateID + "=" + itembuysellstateid );
        //        if (t.Rows.Count == 0) return null;
        //        else return new ItemBuySellState (Convert.ToUInt32(t.Rows[0][0].ToString()), t.Rows[0][1].ToString());
        //    }
        //    public ItemBuySellState GetItemBuySellStateinfo(string itembuysellstate_Name)
        //    {
        //        DataTable t = DB.GetData("select "
        //         + ItemBuySellStateTable.ItemBuySellStateID + ","
        //        + ItemBuySellStateTable.ItemBuySellStateName
        //        + " from  "
        //        + ItemBuySellStateTable.TableName
        //        + " where "
        //    + ItemBuySellStateTable.ItemBuySellStateName + "='" + itembuysellstate_Name  + "'");
        //        if (t.Rows.Count == 0) return null;
        //        else return new ItemBuySellState(Convert.ToUInt32(t.Rows[0][0].ToString()), t.Rows[0][1].ToString());
        //    }
        //    public bool IsItemBuySellStateExists(string ItemBuySellState_name)
        //    {
        //        try
        //        {
        //            DataTable t = DB.GetData("select * from  "
        //          + ItemBuySellStateTable.TableName
        //          + " where "
        //          + ItemBuySellStateTable.ItemBuySellStateName + "='" + ItemBuySellState_name + "'");
        //            if (t.Rows.Count > 0) return true;
        //            else return false;
        //        }
        //        catch (SqlException sqlEx)
        //        {
        //            throw new Exception("حدث خطأ اثناء الاتصال بقاعدة البيانات", sqlEx);
        //        }
        //    }

        //    public bool AddItemBuySellState(string itembuysellstatename)
        //    {
        //        try
        //        {
        //            if (IsItemBuySellStateExists (itembuysellstatename ))
        //            {
        //                MessageBox.Show("البيانات موجودة مسبقا", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return false;
        //            }
        //            DB.ExecuteSQLCommand("","insert into  "
        //                + ItemBuySellStateTable.TableName
        //                + " ( "
        //                + ItemBuySellStateTable.ItemBuySellStateName
        //                + ")values( "
        //                + "'" + itembuysellstatename  + "'"
        //                + ")"
        //                );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            return false;
        //        }
        //    }
        //    public bool UpdatItemBuySellState(uint itembuysellstateid, string new_itembuysellstate_name)
        //    {
        //        try
        //        {
        //            DB.ExecuteSQLCommand("","update   "
        //                + ItemBuySellStateTable.TableName
        //                + " set "
        //                + ItemBuySellStateTable.ItemBuySellStateName + "='" + new_itembuysellstate_name + "'"
        //                + " where "
        //                + ItemBuySellStateTable.ItemBuySellStateID + "=" + itembuysellstateid
        //                );
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    public bool DeleteItemBuySellState(uint itembuysellstateid)
        //    {
        //        try
        //        {

        //            DB.ExecuteSQLCommand("","delete from    "
        //                + ItemBuySellStateTable.TableName
        //                + " where "
        //                + ItemBuySellStateTable.ItemBuySellStateID + "=" + itembuysellstateid 
        //                );
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    public List<ItemBuySellState> GetItemBuySellStateList()
        //    {
        //        try
        //        {
        //            List<ItemBuySellState> list = new List<ItemBuySellState>();
        //            DataTable t = new DataTable();
        //            t = DB.GetData("select * from "
        //                + ItemBuySellStateTable.TableName
        //               );


        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                ItemBuySellState m = new ItemBuySellState(Convert.ToUInt32(t.Rows[i][0]), t.Rows[i][1].ToString());
        //                list.Add(m);
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            MessageBox.Show("فشل جلب قائمة قائمة حالا شراء بيع العنصر:", ee.Message);
        //            return null;
        //        }

        //    }
        //}

    }
}
