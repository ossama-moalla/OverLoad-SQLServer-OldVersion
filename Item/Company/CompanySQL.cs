using ItemProject.AccountingObj.AccountingSQL;
using ItemProject.AccountingObj.Objects;
using ItemProject.Company.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Company
{
    namespace CompanySQL
    {
        public class CompanyReportSQL
        {
            DatabaseInterface DB;
            private static class EmployeesReportTable
            {
                public const string TableName = "[dbo].[Company_Get_Employees_Report]";
                public const string EmployeeID = "EmployeeID";
                public const string EmployeeName = "EmployeeName";
                public const string Gender = "Gender";
                public const string BirthDate = "BirthDate";
                public const string NationalID = "NationalID";
                public const string MaritalStatus = "MaritalStatus";
                public const string Mobile = "Mobile";
                public const string Phone = "Phone";
                public const string Address = "Address_";
                public const string EmailAddress = "EmailAddress";
                public const string JobState = "JobState";
                public const string EmployeeMentState = "EmployeeMentState";
                public const string EmployeeStateCode = "EmployeeStateCode";
            }
            public static class EmployeeMent_Employee_ReportTable
            {
                public const string TableName = "[dbo].[Company_Get_EmployeeMent_Report]";
                public const string LevelID = "LevelID";
                public const string LevelName = "LevelName";
                public const string EmployeeMentID = "EmployeeMentID";
                public const string EmployeeMentName = "EmployeeMentName";
                public const string PartName = "PartName";
                public const string EmployeeID = "EmployeeID";
                public const string EmployeeName = "EmployeeName";
                public const string JobstartID = "JobstartID";
                public const string JobStartDate = "JobStartDate";
                public const string AssignID = "AssignID";
                public const string AssignDate = "AssignDate";

            }
            public static class EmployeesOPRReportTable
            {
                public const string TableName = "[dbo].[Company_GetEmployeesOPR_LOG]";
                public const string OPR_Type = "OPR_Type";
                public const string OPR_Desc = "OPR_Desc";
                public const string OPR_ID = "OPR_ID";
                public const string OPR_Date = "OPR_Date";
                public const string OPR_Employee = "OPR_Employee";
                public const string OPR_EmployeeMent = "OPR_EmployeeMent";

            }
            public CompanyReportSQL(DatabaseInterface db)
            {
                DB = db;

            }
            //public List<EmployeesOPRReport > Get_EmployeesOPRReport_List()
            //{
            //    List<EmployeesOPRReport> list = new List<EmployeesOPRReport>();
            //    try
            //    {

            //        DataTable t = new DataTable();
            //        t = DB.GetData("select "
            //            + EmployeesOPRReportTable.OPR_Type + ","
            //         + EmployeesOPRReportTable.OPR_Desc + ","
            //          + EmployeesOPRReportTable.OPR_ID + ","
            //           + EmployeesOPRReportTable.OPR_Date + ","
            //            + EmployeesOPRReportTable.OPR_Employee + ","
            //        + EmployeesOPRReportTable.OPR_EmployeeMent
            //        + " from   "
            //        + EmployeesOPRReportTable.TableName+"()"
            //        +" order by "
            //        + EmployeesOPRReportTable.OPR_Date

            //      );

            //        for (int i = 0; i < t.Rows.Count; i++)
            //        {
            //            uint oprtype = Convert.ToUInt32(t.Rows [i][0].ToString ());
            //            string oprdesc = t.Rows[i][1].ToString();
            //            uint oprid = Convert.ToUInt32(t.Rows[i][2].ToString());
            //            DateTime oprdate = Convert.ToDateTime(t.Rows[i][3].ToString());

            //            string employee_name = t.Rows[i][4].ToString();
            //            string employeement_name = t.Rows[i][5].ToString();
            //            list.Add(new EmployeesOPRReport(oprtype , oprdesc
            //                , oprid, oprdate , employee_name ,employeement_name));
            //        }
            //        return list;
            //    }
            //    catch (Exception ee)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Get_EmployeesOPRReport_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //        return null;
            //    }
            //}
            public List<EmployeeMent_Employee_Report> Get_EmployeeMent_Employee_Report_List()
            {
                List<EmployeeMent_Employee_Report> list = new List<EmployeeMent_Employee_Report>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + EmployeeMent_Employee_ReportTable.LevelID  + ","
                    + EmployeeMent_Employee_ReportTable.LevelName + ","
                    + EmployeeMent_Employee_ReportTable.EmployeeMentID + ","
                    + EmployeeMent_Employee_ReportTable.EmployeeMentName + ","
                    + EmployeeMent_Employee_ReportTable.PartName  + ","
                    + EmployeeMent_Employee_ReportTable.EmployeeID + ","
                    + EmployeeMent_Employee_ReportTable.EmployeeName + ","
                    + EmployeeMent_Employee_ReportTable.JobstartID + ","
                    + EmployeeMent_Employee_ReportTable.JobStartDate + ","
                    + EmployeeMent_Employee_ReportTable.AssignID + ","
                    + EmployeeMent_Employee_ReportTable.AssignDate
                    + " from   "
                    + EmployeeMent_Employee_ReportTable.TableName + "()"
                    + " order by "
                    + EmployeeMent_Employee_ReportTable.LevelID

                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint levelid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string levelname = t.Rows[i][1].ToString();
                        uint employeement_id = Convert.ToUInt32(t.Rows[i][2].ToString());
                        string employeement_name = t.Rows[i][3].ToString();
                        string part_name = t.Rows[i][4].ToString();
                        uint? employee_id;
                        try
                        {
                             employee_id = Convert.ToUInt32(t.Rows[i][5].ToString());

                        }
                        catch
                        {
                            employee_id = null;
                        }
                        string employee_name = t.Rows[i][6].ToString();
                        uint? jobstartid;
                        try
                        {
                            jobstartid = Convert.ToUInt32(t.Rows[i][7].ToString());

                        }
                        catch
                        {
                            jobstartid = null;
                        }
                        DateTime ? jobstart_date;
                        try
                        {
                            jobstart_date = Convert.ToDateTime (t.Rows[i][8].ToString());

                        }
                        catch
                        {
                            jobstart_date = null;
                        }
                        uint? assign_id;
                        try
                        {
                            assign_id = Convert.ToUInt32(t.Rows[i][9].ToString());

                        }
                        catch
                        {
                            assign_id = null;
                        }
                        DateTime? assign_date;
                        try
                        {
                            assign_date = Convert.ToDateTime(t.Rows[i][10].ToString());

                        }
                        catch
                        {
                            assign_date = null;
                        }


                        list.Add(new EmployeeMent_Employee_Report(levelid,levelname,employeement_id,employeement_name,part_name
                            ,employee_id ,employee_name,jobstartid,jobstart_date ,assign_id,assign_date));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_EmployeesOPRReport_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<EmployeesReport> GetEmployeesReportList()
            {
                List<EmployeesReport> list = new List<EmployeesReport>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeesReportTable.EmployeeID + ","
                        + EmployeesReportTable.EmployeeName + ","
                        + EmployeesReportTable.Gender + ","
                        + EmployeesReportTable.BirthDate + ","
                        + EmployeesReportTable.NationalID + ","
                        + EmployeesReportTable.MaritalStatus + ","
                        + EmployeesReportTable.Mobile + ","
                        + EmployeesReportTable.Phone + ","
                        + EmployeesReportTable.EmailAddress + ","
                        + EmployeesReportTable.Address + ","
                        + EmployeesReportTable.JobState  + ","
                        + EmployeesReportTable.EmployeeMentState + ","
                        + EmployeesReportTable.EmployeeStateCode 
                    + " from   "
                    + EmployeesReportTable.TableName+"()"
                     + " order by   "
                    + EmployeesReportTable.EmployeeID
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint employeeid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string name = t.Rows[i][1].ToString();
                        bool gender = Convert.ToBoolean(t.Rows[i][2].ToString());
                        DateTime birthdate = Convert.ToDateTime(t.Rows[i][3].ToString());
                        string nationalid = t.Rows[i][4].ToString();
                        MaritalStatus MaritalStatus_ = MaritalStatus.Get_MaritalStatus_BY_ID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        string mobile = t.Rows[i][6].ToString();
                        string phone = t.Rows[i][7].ToString();
                        string emailaddress = t.Rows[i][8].ToString();
                        string address = t.Rows[i][9].ToString();
                        string jobstate = t.Rows[i][10].ToString();
                        string employeementstate = t.Rows[i][11].ToString();
                        uint employeestatecode= Convert.ToUInt32(t.Rows[i][12].ToString());
                        list.Add(new EmployeesReport (employeeid, name, gender, birthdate, nationalid, MaritalStatus_, mobile, phone, emailaddress, address, jobstate , employeementstate, employeestatecode));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetEmployeesReportList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
  
        public class PartSQL
        {

            DatabaseInterface DB;
            public static class CompanyPartTable
            {
                public const string TableName = "Company_Part";
                public const string PartID = "PartID";
                public const string PartName = "PartName";
                public const string ParentPartID = "ParentPartID";
                public const string CreateDate = "CreateDate";

            }
            public PartSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public string GetPartPath(Part  Part_)
            {
                List<string> f_path = new List<string>();
                Part f = Part_ ;
                string s =DB .COMPANY.PartName + " / ";
                while (f.ParentPartID  != null)
                {
                    f = GetPartInfoByID(Convert.ToUInt32(f.ParentPartID ));
                    f_path.Add(f.PartName );
                }
                for (int i = f_path.Count - 1; i >= 0; i--)
                    s += f_path[i] + " /";
                return s;
            }
            public bool CreatePart(string name,DateTime CreateDate, uint? parentid)
            {
                string parentid_string;
                if (parentid == null)
                    parentid_string = "null";
                else
                    parentid_string = parentid.ToString();
               try
                {
                    DB.ExecuteSQLCommand(" insert into "
                    + CompanyPartTable.TableName
                    + "("
                    + CompanyPartTable.PartName
                    + ","
                    + CompanyPartTable.ParentPartID 
                    + ","
                    + CompanyPartTable.CreateDate
                    + ")"
                    + "values"
                    + "("
                    + "'" + name + "'"
                    + ","
                    + parentid_string
                     + ","
                    + "'" + CreateDate .ToString("yyyy-MM-dd") + "'"
                    + ")"
                    );
                    DB.AddLog(
                  DatabaseInterface.Log.LogType.INSERT 
              , DatabaseInterface.Log.Log_Target.CompanyPart 
              , ""
                , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.INSERT
                            , DatabaseInterface.Log.Log_Target.CompanyPart
                            , ""
                          , false , ee.Message );
                    MessageBox.Show("CreatePart" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool UpdatePart(uint PartID, string newname, DateTime CreateDate)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                    + CompanyPartTable.TableName
                    + " set "
                    + CompanyPartTable.PartName  + "='" + newname + "'"
                     + ","
                    + CompanyPartTable.CreateDate  + "=" + "'" + CreateDate.ToString("yyyy-MM-dd") + "'"
                    + " where "
                    + CompanyPartTable.PartID + "=" +PartID 
                    );
                    DB.AddLog(
                           DatabaseInterface.Log.LogType.UPDATE 
                           , DatabaseInterface.Log.Log_Target.CompanyPart
                           , ""
                         , true , "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.UPDATE
                          , DatabaseInterface.Log.Log_Target.CompanyPart
                          , ""
                        , false, ee.Message);
                    MessageBox.Show("UpdatePart" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeletePart(uint  partid)
            {
                //List<Item> Items = new ItemSQL(DB).GetItemsInPart(Part__);
                //List<Part> Parts = new PartSQL(DB).GetPartChilds(Part__);
                //if (Parts.Count > 0 || Items.Count > 0)
                //{
                //    MessageBox.Show("المجلد" + Part__.PartName + " غير فارغ لا يمكن حذفه!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return false;
                //}
                try
                {
                    
                    DB.ExecuteSQLCommand(
                        "delete from   "
                    + CompanyPartTable.TableName
                    + " where "
                    + CompanyPartTable.PartID + "=" + partid
                    );
                    DB.AddLog(
                          DatabaseInterface.Log.LogType.DELETE 
                          , DatabaseInterface.Log.Log_Target.CompanyPart
                          , ""
                        , true, "");
                    return true;
                }
                catch(Exception ee)
                {
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.DELETE
                         , DatabaseInterface.Log.Log_Target.CompanyPart
                         , ""
                       , false , ee.Message );
                    MessageBox.Show("DeletePart"+ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public Part GetParentPart(Part f)
            {
                try
                {
                    if (f.ParentPartID == null) return null;
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + CompanyPartTable.PartName  + ","
                        + CompanyPartTable.ParentPartID + ","
                        + CompanyPartTable.CreateDate  
                        + " from   "
                        + CompanyPartTable.TableName
                        + " where "
                        + CompanyPartTable.PartID  + "=" + f.ParentPartID
                      );
                    if (t.Rows.Count == 1)
                    {

                        uint fid = Convert.ToUInt32(f.ParentPartID);
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
                        DateTime d = Convert.ToDateTime(t.Rows[0][2]);
                        return new Part(fid, fname, d, p );
                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetParentPart" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null ;
                }
               

            }
            public List<Part> GetPartChilds(Part Part)
            {
                List<Part> list = new List<Part>();
                try
                {
                    string parentid_str;
                    if (Part == null) parentid_str = " is null";
                    else parentid_str = "=" + Part.PartID.ToString();

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + CompanyPartTable.PartID + ","
                        + CompanyPartTable.PartName + ","
                        + CompanyPartTable.CreateDate
                        + " from   "
                        + CompanyPartTable.TableName
                        + " where "
                        + CompanyPartTable.ParentPartID + parentid_str
                        );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint fid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string fname = t.Rows[i][1].ToString();
                        DateTime d = Convert.ToDateTime(t.Rows[i][2]);  
                        uint? p;
                        if (Part == null) p = null;
                        else p = Part.PartID;

                        list.Add(new Part(fid, fname, d, p));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetPartChilds" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }
                
            }
            public List<Part> SearchPart(string n_)
            {
                List<Part> list = new List<Part>();
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + CompanyPartTable.PartID + ","
                        + CompanyPartTable.PartName + ","
                        + CompanyPartTable.ParentPartID + ","
                        + CompanyPartTable.CreateDate 
                        + " from " + CompanyPartTable.TableName
                       + " where " + CompanyPartTable.PartName + " like  '%" + n_ + "%'");
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
                        list.Add(new Part(fid, fname, d, p));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("SearchPart" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return list;
                }

               
            }
            //public int GetPartIDByName(string name,int? id)
            //{
            //    string parentid;
            //    if (id == null) parentid = " is null";
            //    else parentid = "=" + id.ToString();

            //    System.Data.DataTable table = DB.GetData("select " + CompanyPartTable.PartID
            //        + " from " + CompanyPartTable.Part
            //        + " where " + CompanyPartTable.PartName + "='" + name + "'"
            //        );
            //    return Convert.ToInt32(table.Rows[0][0]);
            //}
            public Part GetPartInfoByID(uint id)
            {
                DataTable t = new DataTable();
                t = DB.GetData("select "
                    + CompanyPartTable.PartName + ","
                    + CompanyPartTable.ParentPartID + ","
                    + CompanyPartTable.CreateDate
                    + " from " + CompanyPartTable.TableName
                    + " where " + CompanyPartTable.PartID  +"="+id
                    );

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
                DateTime d = Convert.ToDateTime(t.Rows[0][2]);
                return new Part(id, fname, d, p );
            }
            public List<Part> GetPartsList()
            {
                List<Part> list = new List<Part>();
                DataTable t = DB.GetData("select * from " + CompanyPartTable.TableName);
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
                    list.Add(new Part(fid, fname, d,p ));
                }
                return list;
            }
            public bool IS_Move_Able(Part DestinationPart, Part Part)
            {

                if (DestinationPart == Part) return false;
                if (DestinationPart == null) return true;
                Part Parent_temp, Child_Temp;
                Child_Temp = DestinationPart;

                while (true)
                {

                    Parent_temp = GetParentPart(Child_Temp);
                    if (Parent_temp == Part) return false;
                    if (Parent_temp == null) return true;

                    Child_Temp = Parent_temp;
                }

            }
            public bool MoveParts(Part DestinationPart, List<Part> PartsList)
            {
                if (PartsList.Count == 0) return false;
                for (int i = 0; i < PartsList.Count; i++)
                {
                    if (!IS_Move_Able(DestinationPart, PartsList[i]))
                    {
                        MessageBox.Show("لا يمكن نقل قسم الى قسم ابن له", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                try
                {
                    for (int i = 0; i < PartsList.Count; i++)
                    {
                        string desteniationPart_id_str;
                        if (DestinationPart == null)
                            desteniationPart_id_str = "null";
                        else
                            desteniationPart_id_str = DestinationPart.PartID.ToString();
                        DB.ExecuteSQLCommand( "update "
                            + CompanyPartTable.TableName
                            + " set "
                            + CompanyPartTable.ParentPartID + "=" + desteniationPart_id_str
                            + " where "
                             + CompanyPartTable.PartID + "=" + PartsList[i].PartID
                            );
                        DB.AddLog(
                         DatabaseInterface.Log.LogType.UPDATE 
                         , DatabaseInterface.Log.Log_Target.CompanyPart
                         , ""
                       , true, "");
                    }
                    return true;
                }
                catch (Exception   ee)
                {
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.UPDATE
                        , DatabaseInterface.Log.Log_Target.CompanyPart
                        , ""
                      , false , ee.Message );
                    MessageBox.Show("MoveParts"+ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public class EmployeeMentLevelSQL
        {
            DatabaseInterface DB;
            public static class EmployeeMentLevelTable
            {
                public const string TableName = "Company_EmployeeMent_Level";
                public const string LevelID = "LevelID";
                public const string LevelName = "LevelName";
            }
            public EmployeeMentLevelSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public EmployeeMentLevel Get_EmployeeMentLevel_Info_BY_ID(uint  levelid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeMentLevelTable.LevelName 
                        + " from   "
                        + EmployeeMentLevelTable.TableName
                        + " where "
                        + EmployeeMentLevelTable.LevelID + "=" + levelid
                      );
                    if (t.Rows.Count == 1)
                    {

                        string  levelname = t.Rows[0][0].ToString();
                        return new EmployeeMentLevel(levelid , levelname);

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_EmployeeMentLevel_Info" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            public bool Add_EmployeeMentLevel( string levelname)
            {
                try
                {

                    DB.ExecuteSQLCommand( " insert into "
                    + EmployeeMentLevelTable.TableName
                    + "("
                    + EmployeeMentLevelTable.LevelName 
                    + ")"
                    + "values"
                    + "("
                     + "'" + levelname + "'"
                    + ")"
                    );
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.INSERT 
                        , DatabaseInterface.Log.Log_Target.Employeement_Level
                        , ""
                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.INSERT
                       , DatabaseInterface.Log.Log_Target.Employeement_Level
                       , ""
                     , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add_EmployeeMentLevel" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_EmployeeMentLevel(uint levelid  ,string Newlevelname)
            {
                try
                {

                    DB.ExecuteSQLCommand("update  "
                       + EmployeeMentLevelTable.TableName
                       + " set "
                       + EmployeeMentLevelTable.LevelName  + "='" + Newlevelname + "'"
                       + " where "
                       + EmployeeMentLevelTable.LevelID  + "=" + levelid 
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                      , DatabaseInterface.Log.Log_Target.Employeement_Level
                      , ""
                    , true ,"");
                    return true;
                }
                catch (Exception ee)
                {

                    DB.AddLog(
                       DatabaseInterface.Log.LogType.UPDATE 
                       , DatabaseInterface.Log.Log_Target.Employeement_Level
                       , ""
                     , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("Update_EmployeeMentLevel" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

            public bool Delete_EmployeeMentLevel(uint  levelid)
            {
                try
                {
                    DB.ExecuteSQLCommand( "update  "
                    + EmployeeMentLevelTable.TableName
                    + " where "
                    + EmployeeMentLevelTable.LevelID  + "="+levelid 
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE 
                      , DatabaseInterface.Log.Log_Target.Employeement_Level
                      , ""
                    , true , "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.DELETE
                     , DatabaseInterface.Log.Log_Target.Employeement_Level
                     , ""
                   , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("Delete_EmployeeMentLevel" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<EmployeeMentLevel> Get_EmployeeMentLevel_List()
            {
                List<EmployeeMentLevel> list = new List<EmployeeMentLevel>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeMentLevelTable.LevelID  + ","
                    + EmployeeMentLevelTable.LevelName
                    + " from   "
                    + EmployeeMentLevelTable.TableName
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint levelid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string levelname = t.Rows[i][1].ToString();
                        
                        list.Add(new EmployeeMentLevel(levelid, levelname ));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_EmployeeMentLevel_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        public class EmployeeMentSQL
        {

            DatabaseInterface DB;
            private static class EmployeeMentTable
            {
                public const string TableName = "Company_EmployeeMent";
                public const string EmployeeMentID = "EmployeeMentID";
                public const string EmployeeMentName = "EmployeeMentName";
                public const string LevelID = "LevelID";
                public const string CreateDate = "CreateDate";
                public const string PartID = "PartID";
      
            }
            public EmployeeMentSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public EmployeeMent Get_EmployeeMent_InfoBYID(uint ID)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeMentTable.EmployeeMentName + ","
                        + EmployeeMentTable.CreateDate + ","
                        + EmployeeMentTable.LevelID + ","
                        + EmployeeMentTable.PartID
                        + " from   "
                        + EmployeeMentTable.TableName
                        + " where "
                        + EmployeeMentTable.EmployeeMentID + "=" + ID
                      );
                    if (t.Rows.Count == 1)
                    {

                        string name = t.Rows[0][0].ToString();
                        DateTime createdate = Convert.ToDateTime(t.Rows[0][1].ToString());
                        EmployeeMentLevel EmployeeMentLevel_  = new EmployeeMentLevelSQL(DB).Get_EmployeeMentLevel_Info_BY_ID(Convert.ToUInt32(t.Rows[0][2]));
                        Part Part_;
                        try
                        {
                             Part_ = new PartSQL(DB).GetPartInfoByID(Convert.ToUInt32(t.Rows[0][3]));

                        }
                        catch
                        {
                            Part_ = null;
                        }
                        return new EmployeeMent(ID, name, createdate,EmployeeMentLevel_, Part_);

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_EmployeeMent_InfoBYID" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
                
            public bool Add_EmployeeMent(string name,  DateTime CreateDate,uint levelid, Part  Part_)
            {
                try
                {

                    DB.ExecuteSQLCommand(" insert into "
                    + EmployeeMentTable.TableName
                    + "("
                    + EmployeeMentTable.EmployeeMentName  + ","
                    + EmployeeMentTable.CreateDate  + ","
                    + EmployeeMentTable.LevelID + ","
                    + EmployeeMentTable.PartID
                    + ")"
                    + "values"
                    + "("
                     + "'" + name + "'"
                    + ","
                    + "'" + CreateDate .ToString("yyyy-MM-dd") + "'"
                    + ","
                    +levelid
                    + ","
                    +(Part_ ==null ?"null": Part_ .PartID.ToString ())
                    + ")"
                    );
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.INSERT
                    , DatabaseInterface.Log.Log_Target.Employeement
                    , name
                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Employeement
                   , name
                     , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("AddEmployee" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_EmployeeMent(uint ID, string name,  DateTime CreatDate,uint levelid)
            {
                try
                {
                   
                    DB.ExecuteSQLCommand(
                         "update  "
                       + EmployeeMentTable.TableName
                       + " set "
                       + EmployeeMentTable.EmployeeMentName  + "='" + name + "'"
                       + ","
                       + EmployeeMentTable.CreateDate + "='" + CreatDate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                        + ","
                       + EmployeeMentTable.LevelID  + "=" + levelid 

                       + " where "
                       + EmployeeMentTable.EmployeeMentID  + "=" + ID
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Employeement
                   , "تعديل الوظيفة ذات الرقم :"+ID .ToString ()
                     , true ,"");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.UPDATE
                  , DatabaseInterface.Log.Log_Target.Employeement
                  , "تعديل الوظيفة ذات الرقم :" + ID.ToString()
                    , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_EmployeeMent" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool MoveEmployeeMents(Part DestinationPart, List<EmployeeMent> EmployeeMentList)
            {
                if (EmployeeMentList.Count == 0) return false;
                try
                {
                    for (int i = 0; i < EmployeeMentList.Count; i++)
                    {
                        DB.ExecuteSQLCommand( "update "
                            + EmployeeMentTable.TableName
                            + " set "
                            + EmployeeMentTable.PartID  + "=" +(DestinationPart ==null ?"null": DestinationPart.PartID.ToString ())
                            + " where "
                             + EmployeeMentTable.EmployeeMentID  + "=" + EmployeeMentList[i].EmployeeMentID
                            );
                    }
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.UPDATE
                 , DatabaseInterface.Log.Log_Target.Employeement
                 ,""
                   , true ,"");
                    return true;
                }
                catch(Exception ee)
                {
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.UPDATE
                , DatabaseInterface.Log.Log_Target.Employeement
                , ""
                  , false , ee.Message );
                    MessageBox.Show("فشل النقل ", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_EmployeeMent(uint ID)
            {
                try
                {
                    DB.ExecuteSQLCommand(
                        "update  "
                    + EmployeeMentTable.TableName
                    + " where "
                    + EmployeeMentTable.EmployeeMentID  + "=" + ID
                    );
                    DB.AddLog(
                   DatabaseInterface.Log.LogType.DELETE 
               , DatabaseInterface.Log.Log_Target.Employeement
               , ""
                 , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                  DatabaseInterface.Log.LogType.DELETE
              , DatabaseInterface.Log.Log_Target.Employeement
              , ""
                , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Delete_EmployeeMent" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<EmployeeMent> Get_EmployeeMent_List()
            {
                List<EmployeeMent> list = new List<EmployeeMent>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeMentTable.EmployeeMentID   + ","
                    + EmployeeMentTable.EmployeeMentName  + ","
                    + EmployeeMentTable.CreateDate + ","
                    + EmployeeMentTable.LevelID  + ","
                    + EmployeeMentTable.PartID  
                    + " from   "
                    + EmployeeMentTable.TableName
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint ID = Convert.ToUInt32(t.Rows[i][0]);
                        string name = t.Rows[i][1].ToString();
                        DateTime createdate = Convert.ToDateTime(t.Rows[i][2].ToString());
                        EmployeeMentLevel EmployeeMentLevel_= new EmployeeMentLevelSQL(DB).Get_EmployeeMentLevel_Info_BY_ID (Convert.ToUInt32(t.Rows[0][3]));
                        Part Part_;
                        try
                        {
                            Part_ = new PartSQL(DB).GetPartInfoByID(Convert.ToUInt32(t.Rows[0][4]));

                        }
                        catch
                        {
                            Part_ = DB.COMPANY ;
                        }
                        list.Add(new EmployeeMent(ID, name, createdate,EmployeeMentLevel_, Part_));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetEmployeeList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<EmployeeMent> Get_EmployeeMent_List_IN_Part(Part Part_)
            {
                
                List<EmployeeMent> list = new List<EmployeeMent>();
                    try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeMentTable.EmployeeMentID + ","
                    + EmployeeMentTable.EmployeeMentName + ","
                    + EmployeeMentTable.CreateDate + ","
                    + EmployeeMentTable.LevelID 
                    + " from   "
                    + EmployeeMentTable.TableName
                    + " where   "
                    + EmployeeMentTable.PartID +(Part_ ==null ?" is null":"="+ Part_.PartID.ToString () )

                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint ID = Convert.ToUInt32(t.Rows[i][0]);
                        string name = t.Rows[i][1].ToString();
                        DateTime createdate = Convert.ToDateTime(t.Rows[i][2].ToString());
                        EmployeeMentLevel EmployeeMentLevel_ = new EmployeeMentLevelSQL(DB).Get_EmployeeMentLevel_Info_BY_ID(Convert.ToUInt32(t.Rows[0][3]));

                        list.Add(new EmployeeMent(ID, name, createdate,EmployeeMentLevel_, Part_));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_EmployeeMent_List_IN_Part" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }

            internal int GetEmployeeMentsCountInPart(Part Part_)
            {
                try
                {
                    DataTable t= DB.GetData ("select count (*) from   "
                    + EmployeeMentTable.TableName
                    + " where "
                    + EmployeeMentTable.PartID + (Part_ == null ? " is null" : "=" + Part_.PartID.ToString())
                    );
                    return Convert.ToInt32(t.Rows [0][0].ToString ()); ;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetEmployeeMentsCountInPart" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }

            internal  string  GetEmployeeMentPath(EmployeeMent employeeMent)
            {
                PartSQL PartSQL_ = new PartSQL(DB);
                List<string> f_path = new List<string>();
                Part f = employeeMent._Part;
                string s =DB.COMPANY.PartName+ " \\";

                while (f.ParentPartID != null)
                {
                    f_path.Add(f.PartName);
                    f = PartSQL_.GetPartInfoByID(Convert.ToUInt32(f.ParentPartID));

                }
                f_path.Add(f.PartName);
                for (int i = f_path.Count - 1; i >= 0; i--)
                    s += f_path[i] + "/";
                return s;
            }

            internal List<EmployeeMent> SearchEmployeeMent(string text)
            {
                List<EmployeeMent> list = new List<EmployeeMent>();
                if (text .Length == 0) return list;
                DataTable t = new DataTable();
                t = DB.GetData("select "
                    + EmployeeMentTable.EmployeeMentID  + ","
                    + EmployeeMentTable.EmployeeMentName + ","
                    + EmployeeMentTable.CreateDate  + ","
                    + EmployeeMentTable.LevelID  + ","
                    + EmployeeMentTable.PartID  
                    + " from " + EmployeeMentTable.TableName
                   + " where " + EmployeeMentTable.EmployeeMentName  + " like '%" + text + "%'");
                for (int i = 0; i < t.Rows.Count; i++)
                {

                    uint employeement_id = Convert.ToUInt32(t.Rows[i][0].ToString());
                    string employeement_name = t.Rows[i][1].ToString();
                    DateTime createdate = Convert.ToDateTime(t.Rows[i][2]);
                    EmployeeMentLevel EmployeeMentLevel_ = new EmployeeMentLevelSQL(DB).Get_EmployeeMentLevel_Info_BY_ID(Convert.ToUInt32(t.Rows[0][3]));

                    Part Part_;
                    try
                    {
                        Part_ = new PartSQL(DB).GetPartInfoByID(Convert.ToUInt32(t.Rows[0][4]));

                    }
                    catch
                    {
                        Part_ = null;
                    }
                    list.Add(new EmployeeMent(employeement_id, employeement_name, createdate,EmployeeMentLevel_, Part_));
                }
                return list;
            }
        }
        public class EmployeeSQL
        {
            DatabaseInterface DB;
            private static class EmployeeTable
            {
                public const string TableName = "Company_Employee";
                public const string EmployeeID = "EmployeeID";
                public const string EmployeeName = "EmployeeName";
                public const string Gender = "Gender";
                public const string BirthDate = "BirthDate";
                public const string NationalID = "NationalID";
                public const string MaritalStatus = "MaritalStatus";
                public const string Mobile = "Mobile";
                public const string Phone = "Phone";
                public const string Address = "Address_";
                public const string EmailAddress = "EmailAddress";
                public const string Report = "Report";
                public const string CurrencyID = "CurrencyID";
            }
            public static class EmployeeImageTable
            {
                public const string TableName = "Company_Employee_Image";
                public const string EmployeeID = "EmployeeID";
                public const string Employee_Image = "Employee_Image";
            }
            public EmployeeSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public Employee  GetEmployeeInforBYID(uint EmployeeID)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeTable.EmployeeName + ","
                        + EmployeeTable.Gender + ","
                        + EmployeeTable.BirthDate + ","
                        + EmployeeTable.NationalID  + ","
                        + EmployeeTable.MaritalStatus  + ","
                        + EmployeeTable.Mobile + ","
                        + EmployeeTable.Phone + ","
                        + EmployeeTable.EmailAddress  + ","
                        + EmployeeTable.Address + ","
                        + EmployeeTable.Report + ","
                         + EmployeeTable.CurrencyID 
                        + " from   "
                        + EmployeeTable.TableName
                        + " where "
                        + EmployeeTable.EmployeeID + "=" + EmployeeID
                      );
                    if (t.Rows.Count == 1)
                    {

                        string name = t.Rows[0][0].ToString();
                        bool gender = Convert.ToBoolean(t.Rows[0][1].ToString());
                        DateTime birthdate = Convert.ToDateTime(t.Rows[0][2].ToString());
                        string nationalid = t.Rows[0][3].ToString();
                        MaritalStatus MaritalStatus_ = MaritalStatus .Get_MaritalStatus_BY_ID (Convert.ToUInt32 ( t.Rows[0][4].ToString()));
                        string mobile = t.Rows[0][5].ToString();
                        string phone = t.Rows[0][6].ToString();
                        string emailaddress = t.Rows[0][7].ToString();
                        string address = t.Rows[0][8].ToString();
                        string notes = t.Rows[0][9].ToString();
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert .ToUInt32(t.Rows[0][10].ToString()));
                        return new Employee(EmployeeID, name, gender, birthdate,nationalid,MaritalStatus_
                            , mobile, phone,emailaddress, address, notes,currency );

                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetEmployeeInforBYID" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }
            public Employee  AddEmployee( string name, bool gender,DateTime birthdate
               , string nationalid,MaritalStatus MaritalStatus_,  string mobile, string phone,string emailaddress, string address,string report,uint CurrencyID)
            {
                try
                {
                   
                    DataTable t= DB.GetData (" insert into "
                    + EmployeeTable.TableName
                    + "("
                    + EmployeeTable.EmployeeName  + ","
                    + EmployeeTable.Gender + ","
                    + EmployeeTable.BirthDate  + ","
                    + EmployeeTable.NationalID  + ","
                    + EmployeeTable.MaritalStatus  + ","
                    + EmployeeTable.Mobile + ","
                     + EmployeeTable.Phone  + ","
                      + EmployeeTable.EmailAddress  + ","
                    + EmployeeTable.Address + ","
                    + EmployeeTable.Report + ","
                    + EmployeeTable.CurrencyID 
                    + ")"
                    + "values"
                    + "("
                     + "'" + name + "'"
                    + ","
                    + (gender  ? 1 : 0).ToString()
                    + ","
                    + "'" + birthdate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                     + ","
                    + "'" + nationalid  + "'"
                     + ","
                    +MaritalStatus_.MaritalStatusID
                    + ","
                    + "'" + mobile + "'"
                    + ","
                    + "'" + phone  + "'"
                    + ","
                    + "'" + emailaddress  + "'"
                    + ","
                    + "'" + address + "'"
                    + ","
                    + "'" + report + "'"
                      + ","
                    + CurrencyID
                    + ")"
                     + " SELECT SCOPE_IDENTITY() "
                    );
                    uint employeeid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.INSERT 
                     , DatabaseInterface.Log.Log_Target.Employee
                     , ""
                   , true ,"");
                    return GetEmployeeInforBYID(employeeid);
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.INSERT
                    , DatabaseInterface.Log.Log_Target.Employee
                    , ""
                  , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("AddEmployee" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
            }
            public bool UpdateEmpolyee(uint employeeidid, string name, bool gender, DateTime birthdate
               , string nationalid, MaritalStatus MaritalStatus_, string mobile, string phone, string emailaddress, string address, string report,uint CurrecnyID)
            {
                try
                {
                    DB.ExecuteSQLCommand("update  "
                       + EmployeeTable.TableName
                       + " set "
                       + EmployeeTable.EmployeeName  + "='" + name + "'"
                       + ","
                       + EmployeeTable.Gender  + "=" + (gender ? 1 : 0).ToString()
                       + ","
                        + EmployeeTable.BirthDate  +"='" + birthdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                      + ","
                       + EmployeeTable.NationalID  + "='" + nationalid  + "'"
                       + ","
                       + EmployeeTable.MaritalStatus  + "=" + MaritalStatus_.MaritalStatusID 
                        + ","
                       + EmployeeTable.Mobile + "='" + mobile + "'"
                       + ","
                       + EmployeeTable.Phone + "='" + phone + "'"
                         + ","
                       + EmployeeTable.EmailAddress  + "='" + emailaddress  + "'"
                       + ","
                       + EmployeeTable.Address + "='" + address + "'"
                        + ","
                       + EmployeeTable.Report   + "='" + report + "'"
                        + ","
                       + EmployeeTable.CurrencyID  + "=" + CurrecnyID
                    + " where "
                    + EmployeeTable.EmployeeID  + "=" + employeeidid 
                    );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.UPDATE 
                     , DatabaseInterface.Log.Log_Target.Employee
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.UPDATE
                    , DatabaseInterface.Log.Log_Target.Employee
                    , ""
                  , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("UpdateEmpolyee" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool DeleteEmployee(uint employeeid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + EmployeeTable.TableName
                    + " where "
                    + EmployeeTable.EmployeeID  + "=" + employeeid
                    );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.DELETE 
                     , DatabaseInterface.Log.Log_Target.Employee
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.DELETE
                     , DatabaseInterface.Log.Log_Target.Employee
                     , ""
                   , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("DeleteEmployee" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public byte[] GetEmployeeImage(uint  EmployeeID_ )
            {
                byte[] image_array;
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeImageTable.Employee_Image
                        + " from "
                        + EmployeeImageTable.TableName
                        + " where "
                        + EmployeeImageTable.EmployeeID + " = " + EmployeeID_);
                    if (t.Rows.Count > 0)
                    {
                        image_array = (byte[])t.Rows[0][0];
                        return image_array;
                    }
                    else return null;

                }
                catch (Exception ee)
                {
                    MessageBox.Show("GetEmployeeImage:" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public bool SetEmployeeImage(uint EmployeeID_, byte[] Image_)
            {

                try
                {

                    if (!UnSetEmployeeImage(EmployeeID_)) throw new Exception();

                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                    command.Connection = DB.DATABASE_CONNECTION;
                    command.CommandText = @"INSERT INTO "
                        +EmployeeImageTable.TableName
                        +"("+ EmployeeImageTable.EmployeeID + ","
                        + EmployeeImageTable.Employee_Image
                        + ")values(@Employeeid,@imagee)";

                    command.Parameters.AddWithValue("@Employeeid", EmployeeID_);
                    command.Parameters.AddWithValue("@imagee", Image_);
                    command.Parameters[0].SqlDbType = SqlDbType.Int;
                    command.Parameters[1].SqlDbType = SqlDbType.Binary;
                    if (DB.DATABASE_CONNECTION.State == ConnectionState.Closed)
                        DB.DATABASE_CONNECTION.Open();
                    command.ExecuteNonQuery();
                    DB.DATABASE_CONNECTION.Close();

                    DB.AddLog(
                     DatabaseInterface.Log.LogType.INSERT 
                     , DatabaseInterface.Log.Log_Target.Employee_Image
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.INSERT
                        , DatabaseInterface.Log.Log_Target.Employee_Image
                        , ""
                      , false , ee.Message );
                    MessageBox.Show("SetEmployeeImage:" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false ;
                }
            }
            public bool UnSetEmployeeImage(uint EmployeeID_)
            {
                try
                {
                    DB.ExecuteSQLCommand( "delete from "
                        + EmployeeImageTable.TableName
                        + " where "
                        + EmployeeImageTable.EmployeeID + "=" + EmployeeID_
                        );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.DELETE 
                     , DatabaseInterface.Log.Log_Target.Employee_Image
                     , ""
                   , true, "");
                    return true;
                }
                catch(Exception ee)
                {
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.DELETE
                     , DatabaseInterface.Log.Log_Target.Employee_Image
                     , ""
                   , false ,ee.Message );
                    return false;
                }
            }


            public List<Employee_User > GetEmployeeUserAccountList()
            {
                List<Employee_User> list = new List<Employee_User>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeTable.EmployeeID + ","
                        + EmployeeTable.EmployeeName + ","
                        + EmployeeTable.Gender + ","
                        + EmployeeTable.BirthDate + ","
                        + EmployeeTable.NationalID + ","
                        + EmployeeTable.MaritalStatus + ","
                        + EmployeeTable.Mobile + ","
                        + EmployeeTable.Phone + ","
                        + EmployeeTable.EmailAddress + ","
                        + EmployeeTable.Address + ","
                        + EmployeeTable.Report + ","
                         + EmployeeTable.CurrencyID
                        + " from   "
                        + EmployeeTable.TableName

                      );
                    for(int i=0;i<t.Rows .Count;i++)
                    {
                        uint EmployeeID = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string name = t.Rows[i][1].ToString();
                        bool gender = Convert.ToBoolean(t.Rows[i][2].ToString());
                        DateTime birthdate = Convert.ToDateTime(t.Rows[i][3].ToString());
                        string nationalid = t.Rows[i][4].ToString();
                        MaritalStatus MaritalStatus_ = MaritalStatus.Get_MaritalStatus_BY_ID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        string mobile = t.Rows[i][6].ToString();
                        string phone = t.Rows[i][7].ToString();
                        string emailaddress = t.Rows[i][8].ToString();
                        string address = t.Rows[i][9].ToString();
                        string notes = t.Rows[i][10].ToString();
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][11].ToString()));
                        Employee Employee_= new Employee(EmployeeID, name, gender, birthdate, nationalid, MaritalStatus_
                            , mobile, phone, emailaddress, address, notes, currency);
                        DatabaseInterface.User User_ = DB.GetEmployeeUser(Employee_);
                        list .Add ( new Employee_User (Employee_, User_));

                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetEmployeeUserAccountList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }
            }
        }
        public class EmployeeQualificationSQL
        {
            DatabaseInterface DB;
            private static class EmployeeQualificationTable
            {
                public const string TableName = "Company_Employee_Qualification";
                public const string EmployeeID = "EmployeeID";
                public const string QualificationDesc = "QualificationDesc";
                public const string StartDate = "StartDate";
                public const string EndDate = "EndDate";
                public const string Notes = "Notes";
            }
            public EmployeeQualificationSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public EmployeeQualification Get_Qualification_Info(Employee Employee_, string QualificationDesc_)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeQualificationTable.StartDate + ","
                        + EmployeeQualificationTable.EndDate + ","
                        + EmployeeQualificationTable.Notes
                        + " from   "
                        + EmployeeQualificationTable.TableName
                        + " where "
                        + EmployeeQualificationTable.QualificationDesc + "='" + QualificationDesc_ + "'"
                        + " and "
                        + EmployeeQualificationTable.EmployeeID + "=" + Employee_.EmployeeID
                      );
                    if (t.Rows.Count == 1)
                    {
   
                        DateTime startdate = Convert.ToDateTime(t.Rows[0][0].ToString());
                        DateTime enddate = Convert.ToDateTime(t.Rows[0][1].ToString());
                        string notes = t.Rows[0][1].ToString();

                        return new EmployeeQualification(Employee_, QualificationDesc_, startdate, enddate, notes);

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_Qualification_Info" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            public bool Add_Qualification(uint EmployeeID, string QualificationDesc,
                DateTime startdate, DateTime enddate, string notes)
            {
                try
                {
                    if(startdate >=enddate )
                    {
                        MessageBox.Show("تاريخ النعاية يجب ان يكون اكبر من تاريخ البداية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false ;
                    }
                    DB.ExecuteSQLCommand( " insert into "
                    + EmployeeQualificationTable.TableName
                    + "("
                    + EmployeeQualificationTable.EmployeeID + ","
                    + EmployeeQualificationTable.QualificationDesc + ","
                    + EmployeeQualificationTable.StartDate + ","
                    + EmployeeQualificationTable.EndDate + ","
                    + EmployeeQualificationTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + EmployeeID
                    + ","
                     + "'" + QualificationDesc + "'"
                    + ","
                    + "'" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                     + "'" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'" + notes + "'"
                    + ")"
                    );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.INSERT 
                     , DatabaseInterface.Log.Log_Target.Employee_Qualification 
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.INSERT
                     , DatabaseInterface.Log.Log_Target.Employee_Qualification
                     , ""
                   , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add_Qualification" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_Qualification(uint EmployeeID, string QualificationDesc, string NewQualificationDesc,  DateTime startdate
                , DateTime enddate, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand("update  "
                       + EmployeeQualificationTable.TableName
                       + " set "
                       + EmployeeQualificationTable.QualificationDesc + "='" + NewQualificationDesc + "'"
                       + ","
                       + EmployeeQualificationTable.StartDate + "='" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                       + ","
                       + EmployeeQualificationTable.EndDate + "='" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                       + ","
                       + EmployeeQualificationTable.Notes + "='" + notes + "'"
                       + " where "
                       + EmployeeQualificationTable.QualificationDesc + "='" + QualificationDesc + "'"
                         + " and "
                        + EmployeeQualificationTable.EmployeeID + "=" + EmployeeID
                       );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.UPDATE 
                     , DatabaseInterface.Log.Log_Target.Employee_Qualification
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.UPDATE
                    , DatabaseInterface.Log.Log_Target.Employee_Qualification
                    , ""
                  , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_Qualification" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

            public bool Delete_Qualification(uint EmployeeID, string QualificationDesc)
            {
                try
                {
                    DB.ExecuteSQLCommand( "update  "
                    + EmployeeQualificationTable.TableName
                    + " where "
                    + EmployeeQualificationTable.QualificationDesc + "='" + QualificationDesc + "'"
                     + " and "
                        + EmployeeQualificationTable.EmployeeID + "=" + EmployeeID
                    );
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.DELETE 
                    , DatabaseInterface.Log.Log_Target.Employee_Qualification
                    , ""
                  , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.DELETE
                    , DatabaseInterface.Log.Log_Target.Employee_Qualification
                    , ""
                  , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Delete_Qualification" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<EmployeeQualification> Get_Qualification_List(Employee Employee_)
            {
                List<EmployeeQualification> list = new List<EmployeeQualification>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeQualificationTable.QualificationDesc + ","
                     + EmployeeQualificationTable.StartDate + ","
                      + EmployeeQualificationTable.EndDate + ","
                    + EmployeeQualificationTable.Notes
                    + " from   "
                    + EmployeeQualificationTable.TableName
                     + " where   "
                    + EmployeeQualificationTable.EmployeeID + "=" + Employee_.EmployeeID 
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        string QualificationDesc_ = t.Rows[i][0].ToString();
                        DateTime startdate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        DateTime enddate = Convert.ToDateTime(t.Rows[i][2].ToString());
                        string notes = t.Rows[i][3].ToString();
                        list.Add(new EmployeeQualification(Employee_, QualificationDesc_, startdate, enddate, notes));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_Qualification_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        public class EmployeeCertificateSQL
        {
            DatabaseInterface DB;
            private static class EmployeeCertificateTable
            {
                public const string TableName = "Company_Employee_Certificate";
                public const string EmployeeID = "EmployeeID";
                public const string CertificateDesc = "CertificateDesc";
                public const string University = "University";
                public const string StartDate = "StartDate";
                public const string EndDate = "EndDate";
                public const string Notes = "Notes";
            }
            public EmployeeCertificateSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public EmployeeCertificate Get_Certificate_Info(Employee Employee_, string CertificateDesc_)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeCertificateTable.University  + ","
                        + EmployeeCertificateTable.StartDate   + ","
                        + EmployeeCertificateTable.EndDate  + ","
                        + EmployeeCertificateTable.Notes
                        + " from   "
                        + EmployeeCertificateTable.TableName
                        + " where "
                        + EmployeeCertificateTable.CertificateDesc + "='" + CertificateDesc_+"'"
                        + " and "
                        + EmployeeCertificateTable.EmployeeID  + "=" + Employee_.EmployeeID 
                      );
                    if (t.Rows.Count == 1)
                    {
                        string university = t.Rows[0][0].ToString();
                        DateTime  startdate = Convert.ToDateTime (t.Rows[0][1].ToString());
                        DateTime enddate = Convert.ToDateTime(t.Rows[0][2].ToString());
                        string notes = t.Rows[0][3].ToString();

                        return new EmployeeCertificate(Employee_, CertificateDesc_, university ,startdate ,enddate  , notes);

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_Certificate_Info" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            public bool Add_Certificate(uint EmployeeID, string CertificateDesc, string university,
                DateTime  startdate,DateTime enddate, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand( " insert into "
                    + EmployeeCertificateTable.TableName
                    + "("
                    + EmployeeCertificateTable.EmployeeID + ","
                    + EmployeeCertificateTable.CertificateDesc + ","
                    + EmployeeCertificateTable.University  + ","
                    + EmployeeCertificateTable.StartDate  + ","
                    + EmployeeCertificateTable.EndDate  + ","
                    + EmployeeCertificateTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + EmployeeID 
                    + ","
                     + "'" + CertificateDesc + "'"
                    + ","
                     + "'" + university  + "'"
                    + ","
                    + "'" + startdate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                     + "'" + enddate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'" + notes + "'"
                    + ")"
                    );
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.INSERT 
                    , DatabaseInterface.Log.Log_Target.Employee_Certificate 
                    , ""
                  , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                   DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Employee_Certificate
                   , ""
                 , false ,ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add_Certificate" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_Certificate(uint EmployeeID, string CertificateDesc, string NewCertificateDesc,string university, DateTime startdate
                ,DateTime enddate, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand( "update  "
                       + EmployeeCertificateTable.TableName
                       + " set "
                       + EmployeeCertificateTable.CertificateDesc + "='" + NewCertificateDesc + "'"
                       + ","
                       + EmployeeCertificateTable.University + "='" + startdate  + "'"
                       + ","
                       + EmployeeCertificateTable.StartDate  + "='" + startdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                       + ","
                       + EmployeeCertificateTable.EndDate  + "='" +enddate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                       + ","
                       + EmployeeCertificateTable.Notes + "='" + notes + "'"
                       + " where "
                       + EmployeeCertificateTable.CertificateDesc + "='" + CertificateDesc + "'"
                         + " and "
                        + EmployeeCertificateTable.EmployeeID + "=" + EmployeeID
                       );
                    DB.AddLog(
                   DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Employee_Certificate
                   , ""
                 , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                  DatabaseInterface.Log.LogType.UPDATE
                  , DatabaseInterface.Log.Log_Target.Employee_Certificate
                  , ""
                , false ,ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_Certificate" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

            public bool Delete_Certificate(uint EmployeeID, string CertificateDesc)
            {
                try
                {
                    DB.ExecuteSQLCommand( "update  "
                    + EmployeeCertificateTable.TableName
                    + " where "
                    + EmployeeCertificateTable.CertificateDesc + "='" + CertificateDesc + "'"
                     + " and "
                        + EmployeeCertificateTable.EmployeeID + "=" + EmployeeID
                    );
                    DB.AddLog(
                  DatabaseInterface.Log.LogType.DELETE 
                  , DatabaseInterface.Log.Log_Target.Employee_Certificate
                  , ""
                , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                 DatabaseInterface.Log.LogType.DELETE
                 , DatabaseInterface.Log.Log_Target.Employee_Certificate
                 , ""
               , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Delete_Certificate" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<EmployeeCertificate> Get_Certificate_List(Employee Employee_)
            {
                List<EmployeeCertificate> list = new List<EmployeeCertificate>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + EmployeeCertificateTable.CertificateDesc + ","
                    + EmployeeCertificateTable.University  + ","
                     + EmployeeCertificateTable.StartDate  + ","
                      + EmployeeCertificateTable.EndDate  + ","
                    + EmployeeCertificateTable.Notes
                    + " from   "
                    + EmployeeCertificateTable.TableName
                     + " where   "
                    + EmployeeCertificateTable.EmployeeID + "=" + Employee_.EmployeeID
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        string CertificateDesc_ = t.Rows[i][0].ToString();
                        string university = t.Rows[i][1].ToString();
                        DateTime startdate = Convert.ToDateTime(t.Rows[i][2].ToString());
                        DateTime enddate = Convert.ToDateTime(t.Rows[i][3].ToString());
                        string notes = t.Rows[i][4].ToString();
                        list.Add(new EmployeeCertificate(Employee_, CertificateDesc_, university ,startdate ,enddate , notes));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_Certificate_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        public class DocumentSQL
        {
            DatabaseInterface DB;
            private static class DocumentTable
            {
                public const string TableName = "Company_Document";
                public const string DocumentID = "DocumentID";
                public const string DocumentDate = "DocumentDate";
                public const string EmployeeID = "EmployeeID";
                public const string DocumentType = "DocumentType";
                public const string ExcuteDate = "ExcuteDate";
                public const string TargetDocumentID = "TargetDocumentID";
                public const string EmployeementID = "EmployeementID";
                public const string Notes = "Notes";

            }
            public DocumentSQL(DatabaseInterface db)
            {
                DB = db;
            }
            public Document  Get_Document_Info_BYID(uint DocumentID_)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + DocumentTable.DocumentDate + ","
                        + DocumentTable.EmployeeID + ","
                        + DocumentTable.DocumentType  + ","
                        + DocumentTable.ExcuteDate  + ","
                        + DocumentTable.TargetDocumentID  + ","
                        + DocumentTable.EmployeementID + ","
                        + DocumentTable.Notes
                        + " from   "
                        + DocumentTable.TableName
                        + " where "
                        + DocumentTable.DocumentID + "=" + DocumentID_
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime documentdate = Convert.ToDateTime(t.Rows[0][0].ToString());
                        Employee Employee_ = new EmployeeSQL(DB).GetEmployeeInforBYID(Convert.ToUInt32(t.Rows[0][1].ToString()));
                        uint DocumnetType = Convert.ToUInt32(t.Rows[0][2].ToString());
                        DateTime executeDdte = Convert.ToDateTime(t.Rows[0][3].ToString());
                        Document target_document;
                        try
                        {
                            target_document = Get_Document_Info_BYID(Convert.ToUInt32(t.Rows[0][4].ToString()));
                        }
                        catch
                        {
                            target_document = null;
                        }
                        EmployeeMent EmployeeMent_;
                        try
                        {
                            EmployeeMent_ = new EmployeeMentSQL (DB).Get_EmployeeMent_InfoBYID(Convert.ToUInt32(t.Rows[0][5].ToString()));
                        }
                        catch
                        {
                            EmployeeMent_ = null;
                        }
                        string notes = t.Rows[0][6].ToString();
                        return new Document(DocumentID_, documentdate, Employee_,DocumnetType,executeDdte ,target_document ,EmployeeMent_, notes);

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_Document_Info_BYID" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            public bool Create_Document(uint EmployeeID,uint  type, DateTime executedate,Document targetdocument,EmployeeMent EmployeeMent_, string notes)
            {
                try
                {
                    if (type < 1 || type > 4)
                    {
                        MessageBox.Show("نوع مستند غير صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false ;
                    }
                    DB.ExecuteSQLCommand( " insert into "
                    + DocumentTable.TableName
                    + "("
                     + DocumentTable.EmployeeID + ","
                        + DocumentTable.DocumentType + ","
                        + DocumentTable.ExcuteDate + ","
                        + DocumentTable.TargetDocumentID + ","
                        + DocumentTable.EmployeementID + ","
                        + DocumentTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + EmployeeID
                    + ","
                    + type 
                    + ","
                    + "'" + executedate .ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                     + (targetdocument ==null ?"null":targetdocument .DocumentID.ToString ())
                    + ","
                     + (EmployeeMent_ == null ? "null" : EmployeeMent_.EmployeeMentID .ToString())
                    + ","
                    + "'" + notes + "'"
                    + ")"
                    );
                    DB.AddLog(
                         DatabaseInterface.Log.LogType.INSERT 
                         , DatabaseInterface.Log.Log_Target.Document 
                         , ""
                       , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.INSERT
                        , DatabaseInterface.Log.Log_Target.Document
                        , ""
                      , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Create_Document" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_Document(uint DocumentID, DateTime executedate, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand("update  "
                       + DocumentTable.TableName
                       + " set "
                       + DocumentTable.ExcuteDate  + "='" + executedate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                       + ","
                       + DocumentTable.Notes + "='" + notes + "'"
                       + " where "
                       + DocumentTable.DocumentID + "=" + DocumentID
                    );
                    DB.AddLog(
                        DatabaseInterface.Log.LogType.UPDATE 
                        , DatabaseInterface.Log.Log_Target.Document
                        , ""
                      , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.UPDATE
                       , DatabaseInterface.Log.Log_Target.Document
                       , ""
                     , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_Document" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

            public bool Delete_Document(uint DocumentID_)
            {
                try
                {
                    DB.ExecuteSQLCommand( "delete from  "
                    + DocumentTable.TableName
                    + " where "
                    + DocumentTable.DocumentID + "=" + DocumentID_
                    );
                    DB.AddLog(
                       DatabaseInterface.Log.LogType.DELETE 
                       , DatabaseInterface.Log.Log_Target.Document
                       , ""
                     , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.DELETE
                      , DatabaseInterface.Log.Log_Target.Document
                      , ""
                    , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("فشل حذف المستند" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<Document > Get_DocumentReport_List()
            {
                List<Document> list = new List<Document>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + DocumentTable.DocumentID + ","
                        + DocumentTable.DocumentDate + ","
                        + DocumentTable.EmployeeID + ","
                        + DocumentTable.DocumentType + ","
                        + DocumentTable.ExcuteDate + ","
                        + DocumentTable.TargetDocumentID + ","
                        + DocumentTable.EmployeementID + ","
                        + DocumentTable.Notes
                    + " from   "
                    + DocumentTable.TableName
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint DocumentID_ = Convert.ToUInt32(t.Rows[i][0].ToString());

                        DateTime documentdate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        Employee Employee_ = new EmployeeSQL(DB).GetEmployeeInforBYID(Convert.ToUInt32(t.Rows[i][2].ToString()));
                        uint DocumnetType = Convert.ToUInt32(t.Rows[i][3].ToString());
                        DateTime executeDdte = Convert.ToDateTime(t.Rows[i][4].ToString());
                        Document target_document;
                        try
                        {
                            target_document = Get_Document_Info_BYID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        }
                        catch
                        {
                            target_document = null;
                        }
                        EmployeeMent EmployeeMent_;
                        try
                        {
                            EmployeeMent_ = new EmployeeMentSQL(DB).Get_EmployeeMent_InfoBYID(Convert.ToUInt32(t.Rows[i][6].ToString()));
                        }
                        catch
                        {
                            EmployeeMent_ = null;
                        }
                        string notes = t.Rows[i][7].ToString();
                        list .Add ( new Document(DocumentID_, documentdate, Employee_, DocumnetType, executeDdte, target_document, EmployeeMent_, notes));

                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_Document_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }
            }
            public List<Document > Get_Employee_Document_List(Employee Employee_)
            {
                List<Document> list = new List<Document>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + DocumentTable.DocumentID + ","
                        + DocumentTable.DocumentDate + ","
                        + DocumentTable.DocumentType + ","
                        + DocumentTable.ExcuteDate + ","
                        + DocumentTable.TargetDocumentID + ","
                        + DocumentTable.EmployeementID + ","
                        + DocumentTable.Notes
                    + " from   "
                    + DocumentTable.TableName
                      + " where   "
                    + DocumentTable.EmployeeID +"="+ Employee_.EmployeeID 
                  );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint DocumentID_ = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime documentdate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        uint DocumnetType = Convert.ToUInt32(t.Rows[i][2].ToString());
                        DateTime executeDdte = Convert.ToDateTime(t.Rows[i][3].ToString());
                        Document target_document;
                        try
                        {
                            target_document = Get_Document_Info_BYID(Convert.ToUInt32(t.Rows[i][4].ToString()));
                        }
                        catch
                        {
                            target_document = null;
                        }
                        EmployeeMent EmployeeMent_;
                        try
                        {
                            EmployeeMent_ = new EmployeeMentSQL(DB).Get_EmployeeMent_InfoBYID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        }
                        catch
                        {
                            EmployeeMent_ = null;
                        }
                        string notes = t.Rows[i][6].ToString();
                        list.Add(new Document(DocumentID_, documentdate, Employee_, DocumnetType, executeDdte, target_document, EmployeeMent_, notes));

                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_Employee_Document_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return list;
                }
            }

            //public Document Get_Employee_Active_Document(Employee Employee_)
            //{
            //    try
            //    {

            //        DataTable t = new DataTable();
            //        t = DB.GetData("select "
            //            + DocumentTable.DocumentID  + ","
            //            + DocumentTable.OprDate + ","
            //            + DocumentTable.Notes
            //         + " from   "
            //         + DocumentTable.TableName
            //         + " where "
            //        + DocumentTable.EmployeeID  + "="+ Employee_.EmployeeID
            //         + " and  "
            //        + DocumentTable.EndDate + " is  null"
            //      );

            //        if (t.Rows.Count == 1)
            //        {
            //            uint Documentid = Convert.ToUInt32(t.Rows[0][0]);
            //            DateTime OprDate = Convert.ToDateTime(t.Rows[0][1].ToString());

            //            string notes = t.Rows[0][2].ToString();
            //            List<EmployeeMentAssign> EmployeeMentAssignList = new EmployeeMentAssignSQL(DB).Get_Document_EmployeeMentAssign_List(Documentid);
            //            return new Document(Documentid, OprDate, Employee_, null , notes, EmployeeMentAssignList);

            //        }
            //        else return null;
            //    }
            //    catch (Exception ee)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Get_Employee_Active_Document" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //        return null;
            //    }
            //}

        }
        public class SalaryClauseSQL
        {

            DatabaseInterface DB;
            private static class SalaryClauseTable
            {
                public const string TableName = "Company_Employee_SalaryClause";
                public const string EmployeeID = "EmployeeID";
                public const string SalaryClauseID = "SalaryClauseID";
                public const string CreateDate = "CreateDate";
                public const string SalaryClauseDesc = "SalaryClauseDesc";
                public const string ClauseType = "ClauseType";
                public const string ExecuteDate = "ExecuteDate";
                public const string MonthCount = "MonthCount";
                public const string Value = "ClauseValue";
                public const string Notes = "Notes";
            }
            public SalaryClauseSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public SalaryClause Get_SalaryClause_Info_BYID(uint  clauseid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + SalaryClauseTable.EmployeeID + ","
                        + SalaryClauseTable.CreateDate  + ","
                        + SalaryClauseTable.SalaryClauseDesc + ","
                        + SalaryClauseTable.ClauseType + ","
                        + SalaryClauseTable.ExecuteDate + ","
                        + SalaryClauseTable.MonthCount  + ","
                        + SalaryClauseTable.Value  + ","
                        + SalaryClauseTable.Notes  
                        + " from   "
                        + SalaryClauseTable.TableName
                        + " where "
                        + SalaryClauseTable.SalaryClauseID  + "=" + clauseid 
                      );
                    if (t.Rows.Count == 1)
                    {

                        Employee employee =new EmployeeSQL(DB ).GetEmployeeInforBYID ( Convert.ToUInt32(t.Rows[0][0].ToString()));
                        DateTime createdate = Convert.ToDateTime(t.Rows[0][1].ToString());
                        string desc = t.Rows[0][2].ToString();
                        bool type = Convert.ToBoolean(t.Rows[0][3].ToString());
                        DateTime executedate = Convert.ToDateTime(t.Rows[0][4].ToString());
                        uint? monthcount;
                        try
                        {
                            monthcount = Convert.ToUInt32 (t.Rows[0][5].ToString());
                        }
                        catch
                        {
                            monthcount = null;
                        }
                        double value = Convert.ToDouble(t.Rows[0][6].ToString());
                        string notes = t.Rows[0][7].ToString();
                        return new SalaryClause(employee,clauseid, createdate,desc, type , executedate ,monthcount,value ,notes  );

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_SalaryClause_Info" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            public bool Add_SalaryClause(uint EmployeeID,string desc,bool type,DateTime  executedate,uint? monthcount,double value,string notes)
            {
                try
                {
                    DB.ExecuteSQLCommand( " insert into "
                    + SalaryClauseTable.TableName
                    + "("
                    + SalaryClauseTable.EmployeeID + ","
                        + SalaryClauseTable.SalaryClauseDesc + ","
                        + SalaryClauseTable.ClauseType + ","
                        + SalaryClauseTable.ExecuteDate  + ","
                        + SalaryClauseTable.MonthCount + ","
                        + SalaryClauseTable.Value + ","
                        + SalaryClauseTable.Notes
                    + ")"
                    + "values"
                    + "("
                    + EmployeeID 
                    + ","
                     + "'" + desc  + "'"
                    + ","
                     + (type ==true ?"1":"0")
                    + ","
                     + "'" + executedate.ToString("yyyy-MM-dd") + "'"
                    + ","
                     + (monthcount ==null ?"null":monthcount .ToString ())
                    + ","
                     + value 
                    + ","
                    + "'" + notes + "'"
                    + ")"
                    );
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT 
                      , DatabaseInterface.Log.Log_Target.Employee_SalaryClause 
                      , ""
                    , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                      DatabaseInterface.Log.LogType.INSERT
                      , DatabaseInterface.Log.Log_Target.Employee_SalaryClause
                      , ""
                    , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add_SalaryClause" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_SalaryClause(uint clauseid, string Desc_,DateTime  executedate, uint?  monthcount,double value, string  notes)
            {
                try
                {
                        DialogResult dd = MessageBox.Show(" تعديل بند الراتب سيؤدي الى خلل في ارشيف الرواتب في حال تم استخدامه سابقا في صرف راتب", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dd != DialogResult.OK) return false;
                    DB.ExecuteSQLCommand("update  "
                       + SalaryClauseTable.TableName
                       + " set "
                       + SalaryClauseTable.SalaryClauseDesc   + "='" + Desc_  + "'"
                       + ","
                        + SalaryClauseTable.ExecuteDate   + "='" + executedate.ToString("yyyy-MM-dd") + "'"
                        + ","
                       + SalaryClauseTable.MonthCount   + "=" + (monthcount == null ? "null" : monthcount.ToString())
                        + ","
                        + SalaryClauseTable.Value  + "=" + value 
                        + ","
                       + SalaryClauseTable.Notes  + "='" + notes  + "'"
                       + " where "
                       + SalaryClauseTable.SalaryClauseID  + "=" + clauseid
                    );
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.UPDATE 
                     , DatabaseInterface.Log.Log_Target.Employee_SalaryClause
                     , ""
                   , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                     DatabaseInterface.Log.LogType.UPDATE
                     , DatabaseInterface.Log.Log_Target.Employee_SalaryClause
                     , ""
                   , false ,ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_SalaryClause" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }

            public bool Delete_SalaryClause(uint clauseid)
            {
                try
                {
                    DialogResult dd = MessageBox.Show(" حذف بند الراتب سيؤدي الى خلل في ارشيف الرواتب في حال تم استخدامه سابقا في صرف راتب","",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning );
                    if (dd != DialogResult.OK) return false ;
                    DB.ExecuteSQLCommand("delete from   "
                    + SalaryClauseTable.TableName
                    + " where "
                    + SalaryClauseTable.SalaryClauseID  + "=" + clauseid
                    );
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.DELETE 
                    , DatabaseInterface.Log.Log_Target.Employee_SalaryClause
                    , ""
                  , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.DELETE
                    , DatabaseInterface.Log.Log_Target.Employee_SalaryClause
                    , ""
                  , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Delete_SalaryClause" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<SalaryClause> Get_SalaryClause_List(Employee  employee)
            {
                List<SalaryClause> list = new List<SalaryClause>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + SalaryClauseTable.SalaryClauseID + ","
                        + SalaryClauseTable.CreateDate + ","
                        + SalaryClauseTable.SalaryClauseDesc + ","
                        + SalaryClauseTable.ClauseType + ","
                        + SalaryClauseTable.ExecuteDate  + ","
                        + SalaryClauseTable.MonthCount + ","
                        + SalaryClauseTable.Value + ","
                        + SalaryClauseTable.Notes
                        + " from   "
                        + SalaryClauseTable.TableName
                        + " where "
                        + SalaryClauseTable.EmployeeID  + "=" + employee.EmployeeID
                      );
                    for(int i=0;i<t.Rows .Count;i++)
                    {

                        uint clauseid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime createdate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string desc = t.Rows[i][2].ToString();
                        bool type = Convert.ToBoolean(t.Rows[i][3].ToString());
                        DateTime executedate = Convert.ToDateTime(t.Rows[i][4].ToString());
                        uint? monthcount;
                        try
                        {
                            monthcount = Convert.ToUInt32(t.Rows[i][5].ToString());
                        }
                        catch
                        {
                            monthcount = null;
                        }
                        double value = Convert.ToDouble(t.Rows[i][6].ToString());
                        string notes = t.Rows[i][7].ToString();
                        list.Add ( new SalaryClause(employee, clauseid, createdate, desc, type, executedate , monthcount, value, notes));

                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_SalaryClause_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }

        }
        public class SalarysPayOrderSQL
        {
            DatabaseInterface DB;
            private static class SalarysPayOrderTable
            {
                public const string TableName = "Company_SalarysPayOrder";
                public const string SalarysPayOrderID = "SalarysPayOrderID";
                public const string OrderDate = "OrderDate";
                public const string ExecuteYear = "ExecuteYear";
                public const string ExecuteMonth = "ExecuteMonth";
                public const string Notes = "Notes";


            }
            private static class SalarysPayOrderYearReportTable
            {
                public const string TableName = "[dbo].[Company_GetSalarys_Year_Report]";
                public const string Year_Month = "Year_Month";
                public const string Year_Month_Name = "Year_Month_Name";
                public const string SalarysPayOrderID = "SalarysPayOrderID";
                public const string OrderDate = "OrderDate";
                public const string EmployeesCount = "EmployeesCount";
                public const string MoneyAmount = "MoneyAmount";


            }
            private static class SalarysPayOrderEmployeeReportTable
            {
                public const string TableName = "[dbo].[Company_GetSalarysPayOrder_Employees_Report]";
                public const string EmployeeID = "EmployeeID";
                public const string EmployeeName = "EmployeeName";
                public const string JobState = "JobState";
                public const string EmployeeMentState = "EmployeeMentState";
                public const string EmployeeStateCode = "EmployeeStateCode";
                public const string ExcpectedSalary = "ExcpectedSalary";
                public const string PayOrderID = "PayOrderID";
                public const string PayOrderValue = "PayOrderValue";
                public const string PayOrderCurrecnyID = "PayOrderCurrecnyID";
                public const string PayOrderExchangeRate = "PayOrderExchangeRate";

            }
            public SalarysPayOrderSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public SalarysPayOrder Get_SalarysPayOrder_Info_ByID(uint  id)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + SalarysPayOrderTable.OrderDate  + ","
                        + SalarysPayOrderTable.ExecuteYear + ","
                        + SalarysPayOrderTable.ExecuteMonth + ","
                        + SalarysPayOrderTable.Notes 
                        + " from   "
                        + SalarysPayOrderTable.TableName
                        + " where "
                        + SalarysPayOrderTable.SalarysPayOrderID   + "=" + id
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime orderdate = Convert.ToDateTime(t.Rows [0][0].ToString ());
                        int executeyear = Convert.ToInt32(t.Rows[0][1].ToString());
                        int executemonth = Convert.ToInt32(t.Rows[0][2].ToString());
                        string  notes = t.Rows[0][3].ToString();
                        return new SalarysPayOrder(id ,orderdate ,executeyear,executemonth,notes );

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_SalarysPayOrder_Info" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public SalarysPayOrder Get_SalarysPayOrder_Info_ByMonth_Year(int year,int month)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + SalarysPayOrderTable.SalarysPayOrderID  + ","
                        + SalarysPayOrderTable.OrderDate + ","
  
                        + SalarysPayOrderTable.Notes
                        + " from   "
                        + SalarysPayOrderTable.TableName
                        + " where "
                        + SalarysPayOrderTable.ExecuteYear+"="+year 
                        + " and "
                        + SalarysPayOrderTable.ExecuteMonth + "=" + month 
                      );
                    if (t.Rows.Count == 1)
                    {
                        uint id = Convert.ToUInt32(t.Rows[0][0].ToString());
                        DateTime orderdate = Convert.ToDateTime(t.Rows[0][1].ToString());
                        string notes = t.Rows[0][2].ToString();
                        return new SalarysPayOrder(id, orderdate, year , month , notes);

                    }
                    else
                        return null;
                }

                catch (Exception ee)
                {
                    MessageBox.Show("Get_SalarysPayOrder_Info" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            public bool Add_SalarysPayOrder(int exe_year, int exe_month, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand(" insert into "
                    + SalarysPayOrderTable.TableName
                    + "("
                    + SalarysPayOrderTable.ExecuteYear + ","
                    + SalarysPayOrderTable.ExecuteMonth  + ","
                    + SalarysPayOrderTable.Notes 
                    + ")"
                    + "values"
                    + "("
                    + exe_year 
                    + ","
                    + exe_month
                    + ","
                     + "'" + notes  + "'"
                    + ")"
                    );
                    DB.AddLog(
                    DatabaseInterface.Log.LogType.INSERT 
                    , DatabaseInterface.Log.Log_Target.Salary_PayOrder 
                    , ""
                  , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                   DatabaseInterface.Log.LogType.INSERT
                   , DatabaseInterface.Log.Log_Target.Salary_PayOrder
                   , ""
                 , false ,ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add_SalarysPayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_SalarysPayOrder(uint id,int exe_year, int exe_month, string notes)
            {
                try
                {

                    DB.ExecuteSQLCommand("update  "
                       + SalarysPayOrderTable.TableName
                       + " set "

                       + SalarysPayOrderTable.ExecuteMonth + "=" + exe_month 
                       + ","
                       + SalarysPayOrderTable.ExecuteMonth + "=" + exe_year
                        + ","
                       + SalarysPayOrderTable.Notes + "='" + notes  + "'"
                       
                       + " where "
                       + SalarysPayOrderTable.SalarysPayOrderID  + "=" + id
                    );
                    DB.AddLog(
                   DatabaseInterface.Log.LogType.UPDATE 
                   , DatabaseInterface.Log.Log_Target.Salary_PayOrder
                   , ""
                 , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                  DatabaseInterface.Log.LogType.UPDATE
                  , DatabaseInterface.Log.Log_Target.Salary_PayOrder
                  , ""
                , false ,ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_SalarysPayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_SalarysPayOrder(uint id)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + SalarysPayOrderTable.TableName
                    + " where "
                    + SalarysPayOrderTable.SalarysPayOrderID    + "=" + id
                    );
                    DB.AddLog(
                  DatabaseInterface.Log.LogType.DELETE 
                  , DatabaseInterface.Log.Log_Target.Salary_PayOrder
                  , ""
                , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                 DatabaseInterface.Log.LogType.DELETE
                 , DatabaseInterface.Log.Log_Target.Salary_PayOrder
                 , ""
               , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Delete_SalarysPayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<SalarysPayOrder> Get_SalarysPayOrder_List()
            {
                List<SalarysPayOrder> list = new List<SalarysPayOrder>();
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + SalarysPayOrderTable.SalarysPayOrderID  + ","
                        + SalarysPayOrderTable.OrderDate + ","
                        + SalarysPayOrderTable.ExecuteYear + ","
                        + SalarysPayOrderTable.ExecuteMonth + ","
                        + SalarysPayOrderTable.Notes
                        + " from   "
                        + SalarysPayOrderTable.TableName
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint id = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime orderdate = Convert.ToDateTime(t.Rows[i][1].ToString());
                       int executeyear = Convert.ToInt32(t.Rows[i][2].ToString());
                        int executemonth = Convert.ToInt32(t.Rows[i][3].ToString());
                        string notes = t.Rows[i][4].ToString();
                        list.Add(new SalarysPayOrder(id, orderdate, executeyear, executemonth, notes));
                    }
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_SalarysPayOrder_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<SalarysPayOrderMonthReport> Get_GetSalarysPayOrderMonthReport_List_In_Year(int year)
            {
                List<SalarysPayOrderMonthReport> list = new List<SalarysPayOrderMonthReport>();
                try
                {
 
                    DataTable t =  DB.GetData("select "
                        + SalarysPayOrderYearReportTable.Year_Month + ","
                        + SalarysPayOrderYearReportTable.Year_Month_Name + ","
                        + SalarysPayOrderYearReportTable.SalarysPayOrderID  + ","
                        + SalarysPayOrderYearReportTable.OrderDate  + ","
                        + SalarysPayOrderYearReportTable.EmployeesCount  + ","
                        + SalarysPayOrderYearReportTable.MoneyAmount 
                        + " from   "
                        + SalarysPayOrderYearReportTable.TableName
                        +"("+year +")"
                      );
                    
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        //MessageBox.Show("1");
                        int monthno = Convert.ToInt32(t.Rows[i][0].ToString());
                        string monthname = t.Rows[i][1].ToString();
                        string payorderid;
                        string  orderdate;
                        string  employeescount;
                        string moneyamount;

                            payorderid = t.Rows[i][2].ToString();
                            orderdate = t.Rows[i][3].ToString();
                            employeescount =t.Rows[i][4].ToString();
                            moneyamount = t.Rows[i][5].ToString();


                        list.Add(new SalarysPayOrderMonthReport (year,monthno,monthname, payorderid, orderdate, employeescount , moneyamount) );
                    }
                   
                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_GetSalarysPayOrderMonthReport_List_In_Year" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<SalarysPayOrderEmployeeReport> Get_GetSalarysPayOrderEmployees_Report_List(uint salaryspayorderid)
            {
                List<SalarysPayOrderEmployeeReport> list = new List<SalarysPayOrderEmployeeReport>();
                try
                {

                    DataTable t = DB.GetData("select "
                        + SalarysPayOrderEmployeeReportTable.EmployeeID  + ","
                        + SalarysPayOrderEmployeeReportTable.EmployeeName + ","
                        + SalarysPayOrderEmployeeReportTable.JobState  + ","
                        + SalarysPayOrderEmployeeReportTable.EmployeeMentState + ","
                        + SalarysPayOrderEmployeeReportTable.EmployeeStateCode  + ","
                        + SalarysPayOrderEmployeeReportTable.ExcpectedSalary  + ","
                         + SalarysPayOrderEmployeeReportTable.PayOrderID + ","
                         + SalarysPayOrderEmployeeReportTable.PayOrderValue  + ","
                          + SalarysPayOrderEmployeeReportTable.PayOrderCurrecnyID  + ","
                        + SalarysPayOrderEmployeeReportTable.PayOrderExchangeRate 
                        + " from   "
                        + SalarysPayOrderEmployeeReportTable.TableName
                        + "(" + salaryspayorderid  + ")"
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
             
                        uint employeeid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string employeename = t.Rows[i][1].ToString();
                        string jobsate = t.Rows[i][2].ToString();
                        string employeementstate = t.Rows[i][3].ToString();
                        uint employeestatecode= Convert.ToUInt32(t.Rows[i][4].ToString());
                        string expectedsalary = t.Rows[i][5].ToString();
                        uint? payorderid;
                        double? salarypayordervalue;
                        Currency salarypayorderCurency;
                        double? salarypayorderExchangerate;
                        try
                        {
                            payorderid = Convert.ToUInt32(t.Rows[i][6].ToString());
                            salarypayordervalue = Convert.ToDouble(t.Rows[i][7].ToString());
                            salarypayorderCurency = new CurrencySQL(DB).GetCurrencyINFO_ByID(
                                Convert.ToUInt32 (t.Rows[i][8].ToString()));
                            salarypayorderExchangerate = Convert.ToDouble(t.Rows[i][9].ToString());
                        }
                        catch
                        {
                            payorderid = null;
                            salarypayordervalue = null ;
                            salarypayorderCurency = null ;
                            salarypayorderExchangerate = null ;
                        }


                        list.Add(new SalarysPayOrderEmployeeReport
                            (employeeid , employeename, jobsate , employeementstate,employeestatecode
                            , expectedsalary,payorderid , salarypayordervalue, salarypayorderCurency,salarypayorderExchangerate));
                    }

                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_GetSalarysPayOrderEmployees_Report_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }

            #region SalarysPayOrder_Employee_Report_Currency
            private static class SalarysPayOrderEmployeeReport_Currency_Table
            {
                public const string TableName = "[dbo].[Company_GetSalarysPayOrder_Employees_Report_Currency]";
                public const string CurrencyID = "CurrencyID";
                public const string Currency = "Currency";
                public const string SalarysValue = "SalarysValue";
                public const string PaysValue = "PaysValue";
                public const string RealSalarysValue = "RealSalarysValue";
                public const string RealPaysValue = "RealPaysValue";

            }
            public List<SalarysPayOrderReport_Currency> Get_GetSalarysPayOrderEmployees_Currency_Report_List(uint salaryspayorderid)
            {
                List<SalarysPayOrderReport_Currency> list = new List<SalarysPayOrderReport_Currency>();
                try
                {

                    DataTable t = DB.GetData("select "
                        + SalarysPayOrderEmployeeReport_Currency_Table.CurrencyID + ","
                        + SalarysPayOrderEmployeeReport_Currency_Table.Currency + ","
                        + SalarysPayOrderEmployeeReport_Currency_Table.SalarysValue + ","
                        + SalarysPayOrderEmployeeReport_Currency_Table.PaysValue  + ","
                        + SalarysPayOrderEmployeeReport_Currency_Table.RealSalarysValue + ","
                        + SalarysPayOrderEmployeeReport_Currency_Table.RealPaysValue 
                        + " from   "
                        + SalarysPayOrderEmployeeReport_Currency_Table.TableName
                        + "(" + salaryspayorderid + ")"
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint currencyid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string currencyname = t.Rows[i][1].ToString();
                        double salarysvalue = Convert.ToDouble(t.Rows[i][2].ToString());
                        double paysvalues =Convert .ToDouble ( t.Rows[i][3].ToString());
                        double realsalarysvalue = Convert.ToDouble(t.Rows[i][4].ToString());
                        double realpaysvalues = Convert.ToDouble(t.Rows[i][5].ToString());
                        

                        list.Add(new SalarysPayOrderReport_Currency
                            (currencyid , currencyname, salarysvalue , paysvalues , realsalarysvalue
                            , realpaysvalues));
                    }

                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_GetSalarysPayOrderEmployees_Currency_Report_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            #endregion
            #region SalarysPayOrder_Year_Report_Currency
            private static class SalarysPayOrder_Year_Report_Currency_Table
            {
                public const string TableName = "[dbo].[Company_GetSalarys_Year_Report_Currency]";
                public const string CurrencyID = "CurrencyID";
                public const string Currency = "Currency";
                public const string SalarysValue = "SalarysValue";
                public const string PaysValue = "PaysValue";
                public const string RealSalarysValue = "RealSalarysValue";
                public const string RealPaysValue = "RealPaysValue";

            }
            public List<SalarysPayOrderReport_Currency> Get_GetSalarysPayOrder_Year_Report_Currency_List(int  year)
            {
                List<SalarysPayOrderReport_Currency> list = new List<SalarysPayOrderReport_Currency>();
                try
                {

                    DataTable t = DB.GetData("select "
                        + SalarysPayOrder_Year_Report_Currency_Table.CurrencyID + ","
                        + SalarysPayOrder_Year_Report_Currency_Table.Currency + ","
                        + SalarysPayOrder_Year_Report_Currency_Table.SalarysValue + ","
                        + SalarysPayOrder_Year_Report_Currency_Table.PaysValue + ","
                        + SalarysPayOrder_Year_Report_Currency_Table.RealSalarysValue + ","
                        + SalarysPayOrder_Year_Report_Currency_Table.RealPaysValue
                        + " from   "
                        + SalarysPayOrder_Year_Report_Currency_Table.TableName
                        + "(" + year + ")"
                      );

                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        uint currencyid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        string currencyname = t.Rows[i][1].ToString();
                        double salarysvalue = Convert.ToDouble(t.Rows[i][2].ToString());
                        double paysvalues = Convert.ToDouble(t.Rows[i][3].ToString());
                        double realsalarysvalue = Convert.ToDouble(t.Rows[i][4].ToString());
                        double realpaysvalues = Convert.ToDouble(t.Rows[i][5].ToString());


                        list.Add(new SalarysPayOrderReport_Currency
                            (currencyid, currencyname, salarysvalue, paysvalues, realsalarysvalue
                            , realpaysvalues));
                    }

                    return list;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_GetSalarysPayOrder_Year_Report_Currency_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            #endregion
        }
        public class EmployeePayOrderSQL
        {

            DatabaseInterface DB;
            internal static class PayOrderTable
            {
                public const string TableName = "Company_Employee_PayOrder";
                public const string PayOrderID = "PayOrderID";
                public const string PayOrderDate = "PayOrderDate";
                public const string SalarysPayOrderID = "SalarysPayOrderID";
                public const string PayOrderDesc = "PayOrderDesc";
                public const string EmployeeID = "EmployeeID";
                public const string CurrencyID = "CurrencyID";
                public const string ExchangeRate = "ExchangeRate";
                public const string PayOrderValue = "PayOrderValue";
            }
            public EmployeePayOrderSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public EmployeePayOrder GetPayOrder_INFO_BYID(uint PayOrderid)
            {
                try
                {
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                        + PayOrderTable.PayOrderDate
                        + ","
                        + PayOrderTable.PayOrderDesc
                        + ","
                        + PayOrderTable.EmployeeID 
                        + ","
                        + PayOrderTable.ExchangeRate
                        + ","
                        + PayOrderTable.CurrencyID
                        + ","
                        + PayOrderTable.PayOrderValue
                        + ","
                        + PayOrderTable.SalarysPayOrderID 
                        + " from   "
                        + PayOrderTable.TableName
                        + " where "
                        + PayOrderTable.PayOrderID + "=" + PayOrderid
                      );
                    if (t.Rows.Count == 1)
                    {
                        DateTime payorderDate = Convert.ToDateTime(t.Rows[0][0].ToString());
                        string description = t.Rows[0][1].ToString();
                        Employee Employee_ = new EmployeeSQL(DB).GetEmployeeInforBYID(Convert.ToUInt32(t.Rows[0][2].ToString()));

                        double exchangerate = Convert.ToDouble(t.Rows[0][3].ToString());
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][4].ToString()));
                        double  Value =Convert .ToDouble ( t.Rows[0][5].ToString());
  
                        SalarysPayOrder SalarysPayOrder_;
                        try
                        { SalarysPayOrder_ =
                            new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByID(Convert.ToUInt32(t.Rows[0][6].ToString()));

                        }catch
                        {
                            SalarysPayOrder_ = null;
                        }
                        return new EmployeePayOrder(PayOrderid, payorderDate,SalarysPayOrder_, description,Employee_ , currency, exchangerate, Value);
                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetPayOrder_INFO_BYID:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null ;
                }
               
            }
            public EmployeePayOrder GetPayOrder_INFO_BY_SalarysPayOrderID(SalarysPayOrder SalarysPayOrder_, Employee Employee_)
            {
                try
                {
                    if (SalarysPayOrder_ == null || Employee_ == null) return null;
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                         + PayOrderTable.PayOrderID
                         + ","
                        + PayOrderTable.PayOrderDate
                        + ","
                        + PayOrderTable.PayOrderDesc
                        + ","
                        + PayOrderTable.ExchangeRate
                        + ","
                        + PayOrderTable.CurrencyID
                        + ","
                        + PayOrderTable.PayOrderValue

                        + " from   "
                        + PayOrderTable.TableName
                        + " where "
                         + PayOrderTable.EmployeeID  +"="+Employee_.EmployeeID
                           + " and "
                         + PayOrderTable.SalarysPayOrderID  + "=" + SalarysPayOrder_.SalarysPayOrderID 
                      );
                    if (t.Rows.Count == 1)
                    {
                        uint PayOrderid= Convert.ToUInt32 (t.Rows[0][0].ToString());
                        DateTime payorderDate = Convert.ToDateTime(t.Rows[0][1].ToString());
                        string description = t.Rows[0][2].ToString();
                        double exchangerate = Convert.ToDouble(t.Rows[0][3].ToString());
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][4].ToString()));
                        double Value = Convert.ToDouble(t.Rows[0][5].ToString());
                       return new EmployeePayOrder(PayOrderid, payorderDate, SalarysPayOrder_, description, Employee_, currency, exchangerate, Value);
                    }
                    else
                        return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetPayOrder_INFO_BYID" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

            }
            public EmployeePayOrder GetEmployeeSalaryPayOrder_By_Month(Employee Employee_, int year, int month)
            {
                try
                {
                    SalarysPayOrder SalarysPayOrder_ = new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByMonth_Year(year, month);
                    if (SalarysPayOrder_ != null)
                    {
                        EmployeePayOrder EmployeeSalaryPayOrder_ = GetPayOrder_INFO_BY_SalarysPayOrderID(SalarysPayOrder_, Employee_);
                        return EmployeeSalaryPayOrder_;
                    }
                    else return null;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetEmployeeSalaryPayOrder_By_Month" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public bool Add_PayOrder(DateTime PayOrderdate, uint EmployeeID,  string description, Currency currency, double exchangerate, double  value)
            {
                try
                {

                    DB.ExecuteSQLCommand(" insert into "
                    + PayOrderTable.TableName
                    + "("
                    + PayOrderTable.PayOrderDate 
                    + ","
                    + PayOrderTable.PayOrderDesc
                    + ","
                    + PayOrderTable.EmployeeID 
                    + ","
                    + PayOrderTable.ExchangeRate
                    + ","
                    + PayOrderTable.CurrencyID
                    + ","
                    + PayOrderTable.PayOrderValue  
                    + ")"
                    + "values"
                    + "("
                    + "'" + PayOrderdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + "'" + description + "'"
                    + ","
                    + EmployeeID  
                    + ","
                    + exchangerate
                    + ","
                    + currency.CurrencyID
                    + ","
                    + value 
                    + ")"
                    );
                    DB.AddLog(
                 DatabaseInterface.Log.LogType.INSERT 
                 , DatabaseInterface.Log.Log_Target.Employee_PayOrder
                 , ""
               , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
               DatabaseInterface.Log.LogType.INSERT
               , DatabaseInterface.Log.Log_Target.Employee_PayOrder
               , ""
             , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add_PayOrder" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Add__Salary_PayOrder(DateTime PayOrderdate, uint EmployeeID, uint SalaryPayOrderID, Currency currency, double exchangerate, double value)
            {
                try
                {

                    DB.ExecuteSQLCommand( " insert into "
                    + PayOrderTable.TableName
                    + "("
                    + PayOrderTable.PayOrderDate
                    + ","
                    + PayOrderTable.SalarysPayOrderID
                    + ","
                    + PayOrderTable.EmployeeID
                    + ","
                    + PayOrderTable.ExchangeRate
                    + ","
                    + PayOrderTable.CurrencyID
                    + ","
                    + PayOrderTable.PayOrderValue
                    + ")"
                    + "values"
                    + "("
                    + "'" + PayOrderdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + SalaryPayOrderID
                    + ","
                    + EmployeeID
                    + ","
                    + exchangerate
                    + ","
                    + currency.CurrencyID
                    + ","
                    + value
                    + ")"
                    );
                    DB.AddLog(
                           DatabaseInterface.Log.LogType.INSERT
                           , DatabaseInterface.Log.Log_Target.Employee_PayOrder
                           , ""
                         , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                                    DB.AddLog(
                               DatabaseInterface.Log.LogType.INSERT
                               , DatabaseInterface.Log.Log_Target.Employee_PayOrder
                               , ""
                             , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Add__Salary_PayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_PayOrder(uint PayOrderid, DateTime PayOrderdate , uint  EmployeeID,  string description, Currency currency, double exchangerate,  double  value)
            {
                try
                {

                    DB.ExecuteSQLCommand("update  "
                    + PayOrderTable.TableName
                    + " set "
                    + PayOrderTable.PayOrderDate + "=" + "'" + PayOrderdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + PayOrderTable.PayOrderDesc + "='" + description + "'"
                    + ","
                    + PayOrderTable.EmployeeID    + "=" + EmployeeID    
                    + ","
                    + PayOrderTable.ExchangeRate + "=" + exchangerate
                    + ","
                    + PayOrderTable.PayOrderValue   + "="+value
                    + " where "
                    + PayOrderTable.PayOrderID + "=" + PayOrderid
                    );
                    DB.AddLog(
                              DatabaseInterface.Log.LogType.UPDATE 
                              , DatabaseInterface.Log.Log_Target.Employee_PayOrder 
                              , ""
                            , true ,"");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                              DatabaseInterface.Log.LogType.UPDATE 
                              , DatabaseInterface.Log.Log_Target.Employee_PayOrder
                              , ""
                            , false, ee.Message);
                    System.Windows.Forms.MessageBox.Show("Update_PayOrder" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Update_Salary_PayOrder(uint PayOrderid, DateTime PayOrderdate, uint EmployeeID , Currency currency, double exchangerate, double value)
            {
                try
                {

                    DB.ExecuteSQLCommand( "update  "
                    + PayOrderTable.TableName
                    + " set "
                    + PayOrderTable.PayOrderDate + "=" + "'" + PayOrderdate.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                    + ","
                    + PayOrderTable.EmployeeID + "=" + EmployeeID
                    + ","
                    + PayOrderTable.ExchangeRate + "=" + exchangerate
                    + ","
                    + PayOrderTable.PayOrderValue + "=" + value
                    + " where "
                    + PayOrderTable.PayOrderID + "=" + PayOrderid
                    );
                    DB.AddLog(
                             DatabaseInterface.Log.LogType.UPDATE
                             , DatabaseInterface.Log.Log_Target.Employee_PayOrder 
                             , ""
                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                             DatabaseInterface.Log.LogType.UPDATE
                             , DatabaseInterface.Log.Log_Target.Employee_PayOrder 
                             , ""
                           , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Update_Salary_PayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public bool Delete_PayOrder(uint PayOrderid)
            {
                try
                {
                    DB.ExecuteSQLCommand("delete from   "
                    + PayOrderTable.TableName
                    + " where "
                    + PayOrderTable.PayOrderID + "=" + PayOrderid
                    );
                    DB.AddLog(
                             DatabaseInterface.Log.LogType.DELETE 
                             , DatabaseInterface.Log.Log_Target.Employee_PayOrder
                             , ""
                           , true, "");
                    return true;
                }
                catch (Exception ee)
                {
                    DB.AddLog(
                            DatabaseInterface.Log.LogType.DELETE
                            , DatabaseInterface.Log.Log_Target.Employee_PayOrder
                            , ""
                          , false , ee.Message );
                    System.Windows.Forms.MessageBox.Show("Delete_PayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            public List<EmployeePayOrder> GetPayPayOrders_List()
            {
                try
                {
                    List<EmployeePayOrder> PayOrderList = new List<EmployeePayOrder>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + PayOrderTable.PayOrderID
                    + ","
                    + PayOrderTable.PayOrderDate
                    + ","
                    + PayOrderTable.PayOrderDesc
                    + ","
                    + PayOrderTable.EmployeeID   
                    + ","
                    + PayOrderTable.ExchangeRate
                    + ","
                    + PayOrderTable.CurrencyID
                    + ","
                    + PayOrderTable.PayOrderValue
                     + ","
                    + PayOrderTable.SalarysPayOrderID 
                    + " from   "
                    + PayOrderTable.TableName

                  );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint payinid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime payindate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string description = t.Rows[i][2].ToString();
                        Employee Employee_ = new EmployeeSQL(DB).GetEmployeeInforBYID (Convert.ToUInt32(t.Rows[0][2].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[i][4].ToString());
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][5].ToString()));
                        double Value = Convert.ToDouble(t.Rows[0][6].ToString());
                        SalarysPayOrder SalarysPayOrder_ =
    new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByID(Convert.ToUInt32(t.Rows[0][7].ToString()));
                        PayOrderList.Add(new EmployeePayOrder(payinid, payindate,SalarysPayOrder_, description, Employee_  , currency, exchangerate, Value ));
                    }
                    return PayOrderList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetPayPayOrders_List" + ee.Message, "" , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<EmployeePayOrder> GetPayOrders_List_For_Employee(Employee  Employee_)
            {
                try
                {
                    List<EmployeePayOrder> PayOrderList = new List<EmployeePayOrder>();
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + PayOrderTable.PayOrderID
                    + ","
                    + PayOrderTable.PayOrderDate
                    + ","
                    + PayOrderTable.PayOrderDesc
                    + ","
                    + PayOrderTable.ExchangeRate
                    + ","
                    + PayOrderTable.CurrencyID
                    + ","
                    + PayOrderTable.PayOrderValue
                     + ","
                    + PayOrderTable.SalarysPayOrderID 
                    + " from   "
                    + PayOrderTable.TableName
                      + " from   "
                    + PayOrderTable.EmployeeID   +"="+Employee_  .EmployeeID    

                  );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint payinid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime payindate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string description = t.Rows[i][2].ToString();
                        double exchangerate = Convert.ToDouble(t.Rows[i][3].ToString());
                        Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][4].ToString()));
                        double Value = Convert.ToDouble(t.Rows[0][5].ToString());
                        SalarysPayOrder SalarysPayOrder_ =
   new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByID(Convert.ToUInt32(t.Rows[0][6].ToString()));
                        PayOrderList.Add(new EmployeePayOrder(payinid, payindate,SalarysPayOrder_, description, Employee_  , currency, exchangerate, Value ));
                    }
                    return PayOrderList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetPayOrders_List_For_Employee" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            internal double GetPayOrderValue(uint payorderid)
            {
                try
                {

                    return new Trade.TradeSQL.OperationSQL(DB).Get_OperationValue(Trade.Objects.Operation.Employee_PayOrder , payorderid );
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetPayOrderValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
            internal double GetPayOrder_PaysValue(uint payorderid)
            {
                try
                {
                    return new Trade.TradeSQL.OperationSQL(DB).Get_OperationPaysValue_UPON_OperationCurrency (Trade.Objects.Operation.Employee_PayOrder, payorderid);
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("GetPayOrder_PaysValue:" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return -1;
                }
            }
        }
        //public class EmployeeSalaryPayOrderSQL
        //{

        //    DatabaseInterface DB;
        //    internal static class EmployeeSalaryPayOrderTable
        //    {
        //        public const string TableName = "Company_Employee_SalaryPayOrder";
        //        public const string EmployeeSalarysPayOrderID = "EmployeeSalarysPayOrderID";
        //        public const string SalarysPayOrderID = "SalarysPayOrderID";
        //        public const string EmployeeID = "EmployeeID";
        //        public const string CurrencyID = "CurrencyID";
        //        public const string ExchangeRate = "ExchangeRate";
        //        public const string Value = "SalaryValue";
        //    }
        //    public EmployeeSalaryPayOrderSQL(DatabaseInterface db)
        //    {
        //        DB = db;

        //    }
        //    public EmployeeSalaryPayOrder GetEmployeeSalaryPayOrder_INFO_BYID(SalarysPayOrder SalarysPayOrder_,Employee Employee_)
        //    {
        //        try
        //        {
        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //                + EmployeeSalaryPayOrderTable.CurrencyID 
        //                + ","
        //                + EmployeeSalaryPayOrderTable.ExchangeRate 
        //                + ","
        //                + EmployeeSalaryPayOrderTable.Value
        //                + ","
        //                + EmployeeSalaryPayOrderTable.EmployeeSalarysPayOrderID
        //                + " from   "
        //                + EmployeeSalaryPayOrderTable.TableName
        //                + " where "
        //                + EmployeeSalaryPayOrderTable.EmployeeID  + "=" + Employee_.EmployeeID
        //                + " and "
        //                + EmployeeSalaryPayOrderTable.SalarysPayOrderID  + "=" + SalarysPayOrder_.SalarysPayOrderID 
        //              );
        //            if (t.Rows.Count == 1)
        //            {
        //                Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[0][0].ToString()));

        //                double exchangerate = Convert.ToDouble(t.Rows[0][1].ToString());
        //                double value =Convert .ToDouble ( t.Rows[0][2].ToString());
        //                uint employeesalarypayorderid = Convert.ToUInt32(t.Rows[0][3].ToString());
        //                return new EmployeeSalaryPayOrder(employeesalarypayorderid , SalarysPayOrder_ , Employee_,  currency, exchangerate, value );
        //            }
        //            else
        //                return null;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("GetEmployeeSalaryPayOrder_INFO_BYID" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return null;
        //        }

        //    }
        //    public bool Add_EmployeeSalaryPayOrder(uint  SalarysPayOrderID, uint  EmployeeID, Currency currency, double exchangerate, double  value)
        //    {
        //        try
        //        {

        //            DB.ExecuteSQLCommand(""," insert into "
        //            + EmployeeSalaryPayOrderTable.TableName
        //            + "("
        //            + EmployeeSalaryPayOrderTable.SalarysPayOrderID  
        //            + ","
        //            + EmployeeSalaryPayOrderTable.EmployeeID  
        //            + ","
        //            + EmployeeSalaryPayOrderTable.CurrencyID 
        //            + ","
        //            + EmployeeSalaryPayOrderTable.ExchangeRate
        //            + ","
        //            + EmployeeSalaryPayOrderTable.Value 
        //            + ")"
        //            + "values"
        //            + "("
        //            + SalarysPayOrderID 
        //            + ","
        //            +  EmployeeID 
        //            + ","
        //            + currency .CurrencyID 
        //            + ","
        //            + exchangerate
        //            + ","
        //            + value 
        //            + ")"
        //            );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("Add_EmployeeSalaryPayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Update_EmployeeSalaryPayOrder(uint SalarysPayOrderID, uint EmployeeID, Currency currency, double exchangerate, double value)
        //    {
        //        try
        //        {

        //            DB.ExecuteSQLCommand("","update  "
        //            + EmployeeSalaryPayOrderTable.TableName
        //            + " set "
        //            + EmployeeSalaryPayOrderTable.CurrencyID  + "=" + currency.CurrencyID 
        //            + ","
        //            + EmployeeSalaryPayOrderTable.ExchangeRate  + "="+exchangerate 
        //            + ","
        //            + EmployeeSalaryPayOrderTable.Value  + "="+value
        //            + " where "
        //            + EmployeeSalaryPayOrderTable.EmployeeID + "=" + EmployeeID
        //              + " and "
        //            + EmployeeSalaryPayOrderTable.SalarysPayOrderID  + "=" + SalarysPayOrderID 

        //            );
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("Update_EmployeeSalaryPayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public bool Delete_EmployeeSalaryPayOrder(uint SalarysPayOrderID, uint EmployeeID)
        //    {
        //        try
        //        {
        //            DB.ExecuteSQLCommand("","delete from   "
        //            + EmployeeSalaryPayOrderTable.TableName
        //                             + " where "
        //            + EmployeeSalaryPayOrderTable.EmployeeID + "=" + EmployeeID
        //              + " and "
        //            + EmployeeSalaryPayOrderTable.SalarysPayOrderID + "=" + SalarysPayOrderID);
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("Delete_EmployeeSalaryPayOrder" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    public double GetExpectedSalaryForMonth(uint employeeid, int year,int month)
        //    {
        //        try
        //        {
 
        //            DataTable t = new DataTable();
        //            t = DB.GetData("select  "
        //                + "[dbo].[Company_GetEmployeeSalaryClauses_IN_Month] "
        //                +"("
        //                +employeeid 
        //                +","+
        //                +year
        //                + "," 
        //                +month 
        //                +")"
        //              );
    
        //            if (t.Rows.Count == 1)
        //            {
        //                return Convert.ToDouble(t.Rows [0][0].ToString ());
        //            }
        //            else
        //                return 0;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("GetExpectedSalaryForMonth" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return -1;
        //        }
        //    }
        //    public EmployeeSalaryPayOrder GetEmployeeSalaryPayOrder_By_Month(Employee Employee_, int year, int month)
        //    {
        //        try
        //        {
        //            SalarysPayOrder SalarysPayOrder_ = new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByMonth_Year(year, month);
        //            if (SalarysPayOrder_ != null)
        //            {
        //                EmployeeSalaryPayOrder EmployeeSalaryPayOrder_ = GetEmployeeSalaryPayOrder_INFO_BYID(SalarysPayOrder_, Employee_);
        //                return EmployeeSalaryPayOrder_;
        //            }
        //            else return null;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("GetEmployeeSalaryPayOrder_By_Month" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return null ;
        //        }
        //    }
        //    public List<EmployeeSalaryPayOrder> GetEmployeeSalaryPayOrders_List_For_SalarysPayOrder(SalarysPayOrder SalarysPayOrder_)
        //    {
        //        List<EmployeeSalaryPayOrder> EmployeeSalaryPayOrderList = new List<EmployeeSalaryPayOrder>();

        //        try
        //        {
        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //            + EmployeeSalaryPayOrderTable.EmployeeID 
        //            + ","
        //            + EmployeeSalaryPayOrderTable.CurrencyID 
        //            + ","
        //            + EmployeeSalaryPayOrderTable.ExchangeRate 
        //            + ","
        //            + EmployeeSalaryPayOrderTable.Value
        //            + ","
        //            + EmployeeSalaryPayOrderTable.EmployeeSalarysPayOrderID
        //            + " from   "
        //            + EmployeeSalaryPayOrderTable.TableName

        //          );
        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                Employee Employee_ = new EmployeeSQL(DB).GetEmployeeInforBYID(Convert.ToUInt32 (t.Rows [i][0].ToString ()));
        //                Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][1].ToString()));

        //                double exchangerate = Convert.ToDouble(t.Rows[i][2].ToString());
        //                double value = Convert.ToDouble(t.Rows[i][3].ToString());
        //                uint employeesalarypayorderid = Convert.ToUInt32(t.Rows[0][4].ToString());
        //                EmployeeSalaryPayOrderList.Add ( new EmployeeSalaryPayOrder(employeesalarypayorderid, SalarysPayOrder_, Employee_, currency, exchangerate, value));
        //            }
        //            return EmployeeSalaryPayOrderList;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("GetPayEmployeeSalaryPayOrders_List" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return EmployeeSalaryPayOrderList;
        //        }
        //    }
        //    public List<EmployeeSalaryPayOrder> GetEmployeeSalaryPayOrders_List_For_Employee(Employee Employee_)
        //    {

        //            List<EmployeeSalaryPayOrder> list = new List<EmployeeSalaryPayOrder>();
        //        try
        //        {
        //            DataTable t = new DataTable();
        //            t = DB.GetData("select "
        //            + EmployeeSalaryPayOrderTable.SalarysPayOrderID 
        //            + ","
        //            + EmployeeSalaryPayOrderTable.CurrencyID
        //            + ","
        //            + EmployeeSalaryPayOrderTable.ExchangeRate
        //            + ","
        //            + EmployeeSalaryPayOrderTable.Value
        //                + ","
        //                  + EmployeeSalaryPayOrderTable.EmployeeSalarysPayOrderID
        //            + " from   "
        //            + EmployeeSalaryPayOrderTable.TableName

        //          );
        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {
        //                SalarysPayOrder SalarysPayOrder_ = new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByID(Convert.ToUInt32(t.Rows[i][0].ToString()));
        //                Currency currency = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][1].ToString()));

        //                double exchangerate = Convert.ToDouble(t.Rows[i][2].ToString());
        //                double value = Convert.ToDouble(t.Rows[i][3].ToString());
        //                uint employeesalarypayorderid = Convert.ToUInt32(t.Rows[0][4].ToString());
        //                list.Add(new EmployeeSalaryPayOrder(employeesalarypayorderid, SalarysPayOrder_, Employee_, currency, exchangerate, value));
        //            }
        //            return list;
        //        }
        //        catch (Exception ee)
        //        {
        //            System.Windows.Forms.MessageBox.Show("GetEmployeeSalaryPayOrders_List_For_Employee" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return list;
        //        }
        //    }

        //}
        public class PayOrderReportSQL
        {
            DatabaseInterface DB;
     
                internal static class PayOrderReportTable
            {
                    public const string TableName = "[dbo].[Company_GetAllPayOrders]";
                public const string PayOrderType = "PayOrderType";
                public const string PayOrderID = "PayOrderID";
                    public const string PayOrderDate = "PayOrderDate";
                    public const string PayOrderDesc = "PayOrderDesc";
                    public const string EmployeeID = "EmployeeID";
                    public const string EmployeeName = "EmployeeName";
                    public const string CurrencyID = "CurrencyID";
                    public const string ExchangeRate = "ExchangeRate";
                    public const string Value = "Value";
                public const string PaysAmount = "PaysAmount";
            }
            public const string GetPayordersPaysReport = "[dbo].[Company_GetAllPayOrders_PaysReport]";
      
            public PayOrderReportSQL(DatabaseInterface db)
            {
                DB = db;

            }
            public List <PayOrderReport > Get_Company_PayOrdersReportList()
            {
                List<PayOrderReport> PayOrderList = new List<PayOrderReport>();
                try
                {
                    
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + PayOrderReportTable.PayOrderID
                    + ","
                    + PayOrderReportTable.PayOrderDate
                    + ","
                    + PayOrderReportTable.PayOrderDesc
                    + ","
                    + PayOrderReportTable.EmployeeID
                    + ","
                     + PayOrderReportTable.EmployeeName
                    + ","
                    + PayOrderReportTable.Value
                    + ","
                    + PayOrderReportTable.CurrencyID
                    + ","
                    + PayOrderReportTable.ExchangeRate
                    + ","
                    + PayOrderReportTable.PayOrderType
                     + ","
                    + PayOrderReportTable.PaysAmount
                    + " from   "
                    + PayOrderReportTable.TableName + "()"
                    + " order by "
                     + PayOrderReportTable.PayOrderDate
                  );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint payinid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime payindate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string description = t.Rows[i][2].ToString();
                        uint EmployeeID_ = Convert.ToUInt32(t.Rows[i][3].ToString());
                        string employeename = t.Rows[i][4].ToString();
                        double Value = Convert.ToDouble(t.Rows[i][5].ToString());

                        Currency Currency_ = new CurrencySQL (DB).GetCurrencyINFO_ByID ( Convert.ToUInt32(t.Rows[i][6].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[i][7].ToString());
                        bool type = Convert.ToBoolean(t.Rows[i][8].ToString());
                        string pays_amount = t.Rows[i][9].ToString();
                        PayOrderList.Add(new PayOrderReport(type, payinid, payindate, description, EmployeeID_, employeename,
                            Currency_, exchangerate, Value, pays_amount));

                    }
                    return PayOrderList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_Company_PayOrdersReportList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
            public List<PayOrderReport> Get_Employee_PayOrdersReportList(uint EmployeeID)
            {
                List<PayOrderReport> PayOrderList = new List<PayOrderReport>();
                try
                {
 
                    DataTable t = new DataTable();
                    t = DB.GetData("select "
                    + PayOrderReportTable.PayOrderID
                    + ","
                    + PayOrderReportTable.PayOrderDate
                    + ","
                    + PayOrderReportTable.PayOrderDesc
                    + ","
                    + PayOrderReportTable.EmployeeID
                    + ","
                     + PayOrderReportTable.EmployeeName
                    + ","
                    + PayOrderReportTable.Value
                    + ","
                    + PayOrderReportTable.CurrencyID
                    + ","
                    + PayOrderReportTable.ExchangeRate
                    + ","
                    + PayOrderReportTable.PayOrderType
                     + ","
                    + PayOrderReportTable.PaysAmount
                    + " from   "
                    + PayOrderReportTable.TableName + "()"
                    
                     + " where   "
                    + PayOrderReportTable.EmployeeID +"=" + EmployeeID
                    + " order by "
                    + PayOrderReportTable.PayOrderDate


                  );
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        uint payinid = Convert.ToUInt32(t.Rows[i][0].ToString());
                        DateTime payindate = Convert.ToDateTime(t.Rows[i][1].ToString());
                        string description = t.Rows[i][2].ToString();
                        uint EmployeeID_ = Convert.ToUInt32(t.Rows[i][3].ToString());
                        string employeename = t.Rows[i][4].ToString();
                        double Value = Convert.ToDouble(t.Rows[i][5].ToString());


                        Currency Currency_ = new CurrencySQL(DB).GetCurrencyINFO_ByID(Convert.ToUInt32(t.Rows[i][6].ToString()));
                        double exchangerate = Convert.ToDouble(t.Rows[i][7].ToString());
                        bool type = Convert.ToBoolean(t.Rows[i][8].ToString());
                        string pays_amount = t.Rows[i][9].ToString();
                        PayOrderList.Add(new PayOrderReport(type, payinid, payindate, description, EmployeeID_, employeename,
                            Currency_, exchangerate, Value, pays_amount));
                    }
                    return PayOrderList;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_Employee_PayOrdersReportList" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
           public AllPayOrdersReport Get_AllPayOrdersReport()
            {
                try
                {

                    DataTable t = new DataTable();
                    t = DB.GetData("select * from   "
                    + GetPayordersPaysReport + "()"
                  );
                           string Payorders_Value=t.Rows[0][0].ToString();
                    string Payorders_PaysAmount = t.Rows[0][1].ToString();
                    string Payorders_PaysRemain = t.Rows[0][2].ToString();
                    double Payorders_RealValue = Convert.ToDouble(t.Rows[0][3].ToString());
                    double Payorders_Pays_RealValue = Convert.ToDouble(t.Rows[0][4].ToString());

                    return new AllPayOrdersReport(Payorders_Value, Payorders_PaysAmount, Payorders_PaysRemain
                        , Payorders_RealValue, Payorders_Pays_RealValue) ;
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Get_AllPayOrdersReport" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }
            }
        }
    }
}
