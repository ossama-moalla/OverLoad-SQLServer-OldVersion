using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ItemProject.Company.Objects;

namespace ItemProject
{
    public class DatabaseInterface
    {
   

        private const string START_DATE_FUNCTION = "[dbo].[OverLoad_GetDateBaseData]";
        private const int DataBaseID = 1;
        private const  string  App_Start_Date= "2020-1-13";
        private DateTime Database_Start_Date;



        public   SqlConnection   DATABASE_CONNECTION;
        public SqlCommand   DATABASE_SQL_COMMAND;
        public Company.Objects.Part COMPANY ;

        private User Administartor;
        private User _User;
        private bool DataBaseLocked;
        public DatabaseInterface(string db)
        {
            string s="Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+db+";Integrated Security=True;Connect Timeout=30";
            DATABASE_CONNECTION = new SqlConnection(s);
            DATABASE_SQL_COMMAND = new SqlCommand ("", DATABASE_CONNECTION);
            COMPANY = new Company.Objects.Part(0, "الشركة", DateTime.Now, null);
            try
            {
      
                DataTable t = GetData("select * from "
             + START_DATE_FUNCTION + "()");
                int databaseid = Convert.ToInt32(t.Rows[0][0]);
                Database_Start_Date = Convert.ToDateTime(t.Rows[0][1]);

                if (Database_Start_Date != Convert.ToDateTime(DatabaseInterface.App_Start_Date) ||
                    databaseid!=DatabaseInterface.DataBaseID) throw new Exception();
            }catch
            {
                throw new Exception("قاعدة بيانات غير صحيحة");
            }

            Administartor = new User(0, "Administrator", Database_Start_Date, false, null); ;
            DataBaseLocked = false;


        }
        public   void LogIN(string UserName,string PassWord)
        {
            try
            {
                DataTable t1 = GetData(" select  * from "
                     + UserTable.TableName
                      + " where "
                     + UserTable.OV_UserName + "='" + UserName+"'"
                     );
                if (t1.Rows.Count == 0)
                    throw new Exception("اسم المستخدم غير صحيح");
                //ShowSqlQuery dd = new ShowSqlQuery();
                DataTable t2= GetData(" select "
                    +UserTable .OV_UserID
                    +","
                     + UserTable.AddDate 
                    + ","
                     + UserTable.Disabled_ 
                     + ","
                     + UserTable.EmployeeID   
                    + " from "
                     + UserTable.TableName
                    + " where "
                     + UserTable.OV_UserName +"='"+UserName+"'"
                     +" and "
                     + UserTable.OV_Password  + "='" + PassWord+ "'"
                    );
                 if(t2.Rows .Count ==1)
                {
                    uint userid = Convert.ToUInt32(t2.Rows [0][0].ToString ());
                    DateTime adddate = Convert.ToDateTime(t2.Rows[0][1].ToString());
                    Employee Employee_;
                    bool disabled = Convert.ToBoolean(t2.Rows[0][2].ToString());
                    try
                    {
                         Employee_ = new Company.CompanySQL.EmployeeSQL(this).GetEmployeeInforBYID(Convert.ToUInt32(t2.Rows[0][3].ToString()));
                        this._User = new User(userid, UserName, adddate, disabled, Employee_);
                    }
                    catch
                    {

                        if (UserName != Administartor.UserName)
                            throw new Exception("على مايبدو تم تعديل قاعدة البيانات بشكل مخالف للتصميم :)");
                        this._User = new User(userid, UserName, adddate, disabled, null);
                    }
                   
                    AddLog(Log.LogType.LOGIN 
                    , Log.Log_Target.User
                    ,  UserName, true , "");
                }
                else throw new Exception("كلمة المرور غير صحيحة");


            }
            catch(Exception ee)
            {
                AddLog(Log.LogType.LOGIN
                    , Log.Log_Target.User
                    , UserName, false,ee.Message  );
                throw new Exception(ee.Message);
            }
        }
        public string GetUser_EmployeeName()
        {
            if (this._User._Employee == null) return " مدير النظام";
            else return this._User._Employee.EmployeeName;
        }
        public void ExecuteSQLCommand(string SQLCommand)
        {
            if (DataBaseLocked) throw new Exception("قاعدة البيانات مقفولة");

                DATABASE_SQL_COMMAND.CommandText = SQLCommand;
                if (DATABASE_CONNECTION.State != ConnectionState.Open) /*DATABASE_CONNECTION.Close();*/
                    DATABASE_CONNECTION.Open();
                DATABASE_SQL_COMMAND.ExecuteNonQuery();
                //DATABASE_CONNECTION.Close();
         

        }

        public DataTable GetData(string SQLCommand)
        {
            try
            {
                if (DATABASE_CONNECTION.State != ConnectionState.Open) 
                DATABASE_CONNECTION.Open();
                DATABASE_SQL_COMMAND.CommandText = SQLCommand;
                SqlDataAdapter   DATABASE_ADAPTER = new SqlDataAdapter();
                DATABASE_ADAPTER.SelectCommand = DATABASE_SQL_COMMAND;
                DataTable table = new DataTable(); 
                DATABASE_ADAPTER.Fill(table);
                //DATABASE_CONNECTION.Close();
                return table;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("GetData"+ex.Message);
                return null;
            }

        }


        #region Log
        public void FillComboBoxLogTypes(ref ComboBox ComboBox_)
        {
            ComboBox_.Items.Clear();
            ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
            ComboBox_.Items.Add(new ComboboxItem("تسجيل الدخول", Log.LogType.LOGIN));
            ComboBox_.Items.Add(new ComboboxItem("ادخال بيانات", Log.LogType.INSERT));
            ComboBox_.Items.Add(new ComboboxItem("تعديل بيانات", Log.LogType.UPDATE));
            ComboBox_.Items.Add(new ComboboxItem("حذف بيانات", Log.LogType.DELETE));

        }

        public void FillComboBox_MainTarget(ref ComboBox ComboBox_)
        {
            ComboBox_.Items.Clear();
            ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
            ComboBox_.Items.Add(new ComboboxItem("المستخدمين و تسجيل الدخول", 1));
            ComboBox_.Items.Add(new ComboboxItem("العملة وحركة النقود", 2));
            ComboBox_.Items.Add(new ComboboxItem("الوظائف و الاقسام و الموظفين", 3));
            ComboBox_.Items.Add(new ComboboxItem("الاصناف و العناصر", 4));
            ComboBox_.Items.Add(new ComboboxItem("التفكيك و التجميع", 5));
            ComboBox_.Items.Add(new ComboboxItem("الصيانة و فواتير الصيانة", 6));
            ComboBox_.Items.Add(new ComboboxItem("المبيع و الشراء وجهات الاتصال ", 7));
            ComboBox_.Items.Add(new ComboboxItem("المستودع و عمليات التخزين", 8));
            ComboBox_.Items.Add(new ComboboxItem("حركة المواد", 9));
        }
        public void FillComboBox_SlaveTareget(ref ComboBox ComboBox_, uint MaintType)
        {
            ComboBox_.Items.Clear();
            switch (MaintType)
            {
                case 0:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));

                    break;
                case 1:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));

                    break;
                case 2:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    break;
                case 3:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("الموظفين", 1));
                    ComboBox_.Items.Add(new ComboboxItem("الرواتب و أوامر الصرف", 2));
                    ComboBox_.Items.Add(new ComboboxItem("الوظائف و الأقسام", 3));
                    break;
                case 4:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("الاصناف", 1));
                    ComboBox_.Items.Add(new ComboboxItem("العناصر", 2));
                    ComboBox_.Items.Add(new ComboboxItem("خصائص العناصر و ضبط القيم", 3));
                    break;
                case 5:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    break;
                case 6:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("فواتير الصيانة", 1));
                    ComboBox_.Items.Add(new ComboboxItem("عمليات الصيانة", 2));
                    ComboBox_.Items.Add(new ComboboxItem("عمليات الفحص و الاعطال", 3));
                    break;
                case 7:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    break;
                case 8:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    break;
                case 9:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    break;

            }

        }
        public void FillComboBox_Targets(ref ComboBox ComboBox_, uint MaintType, uint SlaveType)
        {
            ComboBox_.Items.Clear();
            switch (MaintType)
            {
                case 0:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    break;
                case 1:
                    ComboBox_.Items.Add(new ComboboxItem("المستخدمين", Log.Log_Target.User ));

                    break;
                case 2:

                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("العملة", Log.Log_Target.Currency ));
                    ComboBox_.Items.Add(new ComboboxItem("عمليات الصرف", Log.Log_Target.ExchangeOPR ));
                    ComboBox_.Items.Add(new ComboboxItem("الدفعات الواردة", Log.Log_Target.PayIN ));
                    ComboBox_.Items.Add(new ComboboxItem("الدفعات الخارجة", Log.Log_Target.PayOUT));
                    break;

                case 3:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    switch (SlaveType)
                    {
                        case 1:
                            ComboBox_.Items.Add(new ComboboxItem("الوثائق", Log.Log_Target.Document ));
                            ComboBox_.Items.Add(new ComboboxItem("الموظفين", Log.Log_Target.Employee ));
                            ComboBox_.Items.Add(new ComboboxItem("الشهادات", Log.Log_Target.Employee_Certificate ));
                            ComboBox_.Items.Add(new ComboboxItem("المؤهلات", Log.Log_Target.Employee_Qualification ));
                            ComboBox_.Items.Add(new ComboboxItem("الصور", Log.Log_Target.Employee_Image ));
                            break;
                        case 2:
                            ComboBox_.Items.Add(new ComboboxItem("بنود الراتب", Log.Log_Target.Employee_SalaryClause));
                            ComboBox_.Items.Add(new ComboboxItem("اوامر الصرف", Log.Log_Target.Employee_PayOrder));
                            ComboBox_.Items.Add(new ComboboxItem("صرف رواتب", Log.Log_Target.Salary_PayOrder ));

                            break;
                        case 3:
                            ComboBox_.Items.Add(new ComboboxItem("الوظائف", Log.Log_Target.Employeement));
                            ComboBox_.Items.Add(new ComboboxItem("المستويات الوظيفية", Log.Log_Target.Employeement_Level));
                            ComboBox_.Items.Add(new ComboboxItem("الاقسام", Log.Log_Target.CompanyPart));
                            break;
                    }

                    break;

                case 4:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    switch (SlaveType)
                    {
                        case 1:
                            ComboBox_.Items.Add(new ComboboxItem("الاصناف", Log.Log_Target.Item_Folder));
                            break;
                        case 2:
                            ComboBox_.Items.Add(new ComboboxItem("العناصر", Log.Log_Target.Item_Item));
                            ComboBox_.Items.Add(new ComboboxItem("وحدات التوزيع العناصر", Log.Log_Target.Item_ConsumeUint));
                            ComboBox_.Items.Add(new ComboboxItem("الاسعار الدارجة", Log.Log_Target.Item_SellPrice));
                            ComboBox_.Items.Add(new ComboboxItem("الصور", Log.Log_Target.Item_Image));
                            ComboBox_.Items.Add(new ComboboxItem("الملفات", Log.Log_Target.Item_File));
                            ComboBox_.Items.Add(new ComboboxItem("العلاقات", Log.Log_Target.Item_RealtionShip));
                            break;
                        case 3:
                            ComboBox_.Items.Add(new ComboboxItem("الخصائص غير المقيدة", Log.Log_Target.Item_Spec));
                            ComboBox_.Items.Add(new ComboboxItem("الخصائص المقيدة", Log.Log_Target.Item_Spec_Restrict));
                            ComboBox_.Items.Add(new ComboboxItem("خيارات الخصائص المقيدة", Log.Log_Target.Item_Spec_Restrict_Option));
                            ComboBox_.Items.Add(new ComboboxItem("قيم الخصائص غير المقيدة", Log.Log_Target.Item_Spec_Value));
                            ComboBox_.Items.Add(new ComboboxItem("قيم الخصائص  المقيدة", Log.Log_Target.Item_Spec_Restrict_Value));

                            break;

                    }
                    break;
                    ;
                case 5:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("التجميع", Log.Log_Target.Trade_Assemblage));
                    ComboBox_.Items.Add(new ComboboxItem("التفكيك", Log.Log_Target.Trade_DisAssemblage));
                    break;


                case 6:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    switch (SlaveType)
                    {
                        case 1:
                            ComboBox_.Items.Add(new ComboboxItem("فواتير الصيانة", Log.Log_Target.Item_Folder));
                            ComboBox_.Items.Add(new ComboboxItem("فواتير الصيانة-بند عملية فحص", Log.Log_Target.Item_Folder));
                            ComboBox_.Items.Add(new ComboboxItem("فواتير الصيانة-بند عملية اصلاح", Log.Log_Target.Item_Folder));

                            break;
                        case 2:
                            ComboBox_.Items.Add(new ComboboxItem("عمليات الصيانة", Log.Log_Target.Item_Folder));
                            ComboBox_.Items.Add(new ComboboxItem("ملحقات الصيانة", Log.Log_Target.Item_Folder));

                            break;
                        case 3:
                            ComboBox_.Items.Add(new ComboboxItem("عمليات الفحص", Log.Log_Target.Item_Spec));
                            ComboBox_.Items.Add(new ComboboxItem("ملف عملية فحص", Log.Log_Target.Item_Spec_Restrict));
                            ComboBox_.Items.Add(new ComboboxItem("عمليات القياس", Log.Log_Target.Item_Spec_Restrict_Option));
                            ComboBox_.Items.Add(new ComboboxItem("العناصر المفقودة و التالفة", Log.Log_Target.Item_Spec_Value));
                            ComboBox_.Items.Add(new ComboboxItem("الاعطال", Log.Log_Target.Item_Spec_Restrict_Value));
                            ComboBox_.Items.Add(new ComboboxItem("انتهاء العمل في عملية الصيانة", Log.Log_Target.Item_Spec_Value));
                            ComboBox_.Items.Add(new ComboboxItem(" عمليات الاصلاح", Log.Log_Target.Item_Spec_Restrict_Value));
                            ComboBox_.Items.Add(new ComboboxItem(" عمليات الربط", Log.Log_Target.Item_Spec_Restrict_Value));

                            break;

                    }
                    break;

                case 7:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("الجهات", Log.Log_Target.Trade_Contact));
                    ComboBox_.Items.Add(new ComboboxItem("فواتير الشراء", Log.Log_Target.Trade_BillBuy ));
                    ComboBox_.Items.Add(new ComboboxItem("فواتير المبيع", Log.Log_Target.Trade_BillSell));
                    ComboBox_.Items.Add(new ComboboxItem("اتلاف", Log.Log_Target.Trade_Ravage ));
                    ComboBox_.Items.Add(new ComboboxItem("حالة البيع و الشراء", Log.Log_Target.Item_TradeState));
                    ComboBox_.Items.Add(new ComboboxItem("انماط البيع", Log.Log_Target.Trade_SellTypes));
                    break;
                //            //in out item

                case 8:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("ادخال عناصر", Log.Log_Target.Trade_ItemIN));
                    ComboBox_.Items.Add(new ComboboxItem("اخراج عناصر", Log.Log_Target.Trade_ItemOut));
                    ComboBox_.Items.Add(new ComboboxItem("ضبط اسعار العناصر الداخلة", Log.Log_Target.Trade_ItemIN_SellPrice));
                    break;
                //             public const uint Trade_ItemsStore = 801;
                //public const uint Trade_Store_Container = 802;
                //public const uint Trade_Store_Place = 803;
                case 9:
                    ComboBox_.Items.Add(new ComboboxItem("اظهار الكل", 0));
                    ComboBox_.Items.Add(new ComboboxItem("حاويات اماكن التخزين", Log.Log_Target.Trade_Store_Container));
                    ComboBox_.Items.Add(new ComboboxItem("اماكن التخزين", Log.Log_Target.Trade_Store_Place ));
                    ComboBox_.Items.Add(new ComboboxItem("تخزين المواد", Log.Log_Target.Trade_ItemsStore));
                    break;

            }

        }

        public class Log
        {
            public static class LogType
            {
                public const uint LOGIN = 1;
                public const uint INSERT = 2;
                public const uint UPDATE = 3;
                public const uint DELETE = 4;
            }
            public static string GetLogType_Name(uint logtype)
            {
                switch (logtype)
                { case LogType.LOGIN :
                        return "تسجيل دخول";
                    case LogType.INSERT:
                        return "ادخال";
                    case LogType.UPDATE:
                        return "تعديل";
                    case LogType.DELETE:
                        return "حذف";
                    default:
                        return "----";
                }
            }
            public static string GetLogTarget_Name(uint logtarget)
            {
                switch (logtarget)
                {
                    case Log_Target.User:return "المستخدمين";

                    //Money
                    case  Log_Target.Currency: return "العملات";
                    case Log_Target.ExchangeOPR: return "عمليات الصرف";
                    case Log_Target.PayIN: return "الدفعات الواردة";
                    case Log_Target.PayOUT: return "الدفعات الصادرة";

                    //Company

                    //Employee
                    case Log_Target.Document: return "وثائق موظفين";
                    case Log_Target.Employee: return "الموظفين";
                    case Log_Target.Employee_Certificate: return "شهادات الموظفين";
                    case Log_Target.Employee_Image: return "صور الموظفين";
                    case Log_Target.Employee_Qualification: return "مؤهلات الموظفين";
                    case Log_Target.Employee_SalaryClause: return "بنود راتب موظف";
                    //Salary And Payorder
                    case Log_Target.Employee_PayOrder: return "اوامر الصرف";
                    case Log_Target.Salary_PayOrder: return "اوامر صرف راتب";
                    //Part and EmployeeMent
                    case Log_Target.Employeement: return "الوظائف";
                    case Log_Target.Employeement_Level: return "المستويات الوظيفية";
                    case Log_Target.CompanyPart: return "الاقسام";


                    //Item
                    case Log_Target.Item_Folder: return "اصناف العناصر";
                    //
                    case Log_Target.Item_ConsumeUint: return "وحدات توزيع العناصر";
                    case Log_Target.Item_Item: return "العناصر";
                    case Log_Target.Item_File: return "ملفات العناصر";
                    case Log_Target.Item_Image: return "صورة عنصر";
                    case Log_Target.Item_RealtionShip: return "العلاقات بين العناصر";
                    case Log_Target.Item_SellPrice: return "اسعار العناصر الدارجة";
                    //
                    case Log_Target.Item_Spec: return "خصائص العناصر الغير مقيدة";
                    case Log_Target.Item_Spec_Value: return "قيمة خاصية غير مقيدة";
                    case Log_Target.Item_Spec_Restrict: return "خصائص العناصر المقيدة";
                    case Log_Target.Item_Spec_Restrict_Option: return "خيارات العناصر المقيدة";
                    case Log_Target.Item_Spec_Restrict_Value: return "قيمة خاصية مقيدة";


                    //INdustry
                    case Log_Target.Trade_Assemblage: return "تجميع";
                    case Log_Target.Trade_DisAssemblage: return "تفكيك";

                    //Mainteance
                    case Log_Target.Trade_BillMaintenenace: return "فاتورة صيانة";
                    case Log_Target.Trade_BillMaintenenace_Clause_DiagnosticOPR: return "بند فحص-فاتورة صيانة";
                    case Log_Target.Trade_BillMaintenenace_Clause_RepairOPR: return "بند اصلاح- فاتورة صيانة";
                    //
                    case Log_Target.Maintenenace_MaintenenaceOPR: return "عملية صيانة";
                    case Log_Target.Maintenenace_Accessory: return "ملحق صيانة";

                    case Log_Target.Maintenenace_DiagnosticOPR: return "عملية فحص";
                    case Log_Target.Maintenenace_DiagnosticOPR_File: return "ملف عملية فحص";
                    case Log_Target.Maintenenace_DiagnosticOPR_MeasureOPR: return "عملية قياس";
                    case Log_Target.Maintenenace_DiagnosticOPR_MissedFaultItem: return "عنصر تالف او مفقود";
                    case Log_Target.Maintenenace_EndWork: return "عملية صيانة-انتهاء العمل";
                    case Log_Target.Maintenenace_Fault: return "عملية صيانة-عطل";
                    case Log_Target.Maintenenace_Fault_RepairOPR: return "عملية اصلاح";
                    case Log_Target.Maintenenace_Tag: return "عملية صيانة -رابط";

                    //Trade
                    case Log_Target.Trade_Contact: return "جهات التعامل";
                    case Log_Target.Trade_BillBuy: return "فاتورة شراء";
                    case Log_Target.Trade_BillSell: return "فاتورة مبيعات";
                    case Log_Target.Trade_Ravage: return "عملية اتلاف";


                    case Log_Target.Item_TradeState: return "حالة البيع و الشراء لعنصر";
                    case Log_Target.Trade_SellTypes: return "انماط البيع";

                    //in out item
                    case Log_Target.Trade_ItemOut: return "اخراج عناصر";
                    case Log_Target.Trade_ItemIN: return "ادخال عناصر";
                    case Log_Target.Trade_ItemIN_SellPrice: return "اسعار العناصر الداخلة";
                    case Log_Target.Trade_BillAdditionalClause: return "بند فاتورة اضافي";
                    //Store
                    case Log_Target.Trade_ItemsStore: return "تخزين عناصر";
                    case Log_Target.Trade_Store_Container: return "حاويات اماكن التخزين";
                    case Log_Target.Trade_Store_Place: return "اماكن التخزين";
                    default: return "----";
                }
    }
            public static class Log_Target
            {
                //User
                public const uint User = 101;

                //Money
                public const uint Currency = 201;
                public const uint ExchangeOPR = 202;
                public const uint PayIN = 203;
                public const uint PayOUT = 204;

                //Company

                    //Employee
                    public const uint Document = 311;
                    public const uint Employee = 312;
                    public const uint Employee_Certificate = 313;
                    public const uint Employee_Image = 314;
                    public const uint Employee_Qualification = 315;
                    public const uint Employee_SalaryClause = 316;
                    //Salary And Payorder
                    public const uint Employee_PayOrder = 321;
                    public const uint Salary_PayOrder = 322;
                    //Part and EmployeeMent
                    public const uint Employeement = 331;
                    public const uint Employeement_Level = 332;
                    public const uint CompanyPart = 333;


                //Item
                    public const uint Item_Folder = 411;
                    //
                    public const uint Item_ConsumeUint =421;  
                    public const uint Item_Item = 422;
                    public const uint Item_File = 423;
                    public const uint Item_Image = 424;
                    public const uint Item_RealtionShip = 425;
                    public const uint Item_SellPrice = 426;
                    //
                    public const uint Item_Spec = 431;
                    public const uint Item_Spec_Value = 432;
                    public const uint Item_Spec_Restrict = 433;
                    public const uint Item_Spec_Restrict_Option = 434;
                    public const uint Item_Spec_Restrict_Value = 435;
                

                //INdustry
                public const uint Trade_Assemblage = 501;
                public const uint Trade_DisAssemblage = 502;

                //Mainteance
                    public const uint Trade_BillMaintenenace = 611;
                    public const uint Trade_BillMaintenenace_Clause_DiagnosticOPR = 612;
                    public const uint Trade_BillMaintenenace_Clause_RepairOPR = 613;
                    //
                    public const uint Maintenenace_MaintenenaceOPR = 621;
                    public const uint Maintenenace_Accessory = 622;

                    public const uint Maintenenace_DiagnosticOPR = 631;
                    public const uint Maintenenace_DiagnosticOPR_File = 632;
                    public const uint Maintenenace_DiagnosticOPR_MeasureOPR = 633;
                    public const uint Maintenenace_DiagnosticOPR_MissedFaultItem = 634;
                    public const uint Maintenenace_EndWork = 635;
                    public const uint Maintenenace_Fault = 636;
                    public const uint Maintenenace_Fault_RepairOPR = 637;
                    public const uint Maintenenace_Tag = 638;

                //Trade
                public const uint Trade_Contact = 701;
                public const uint Trade_BillBuy = 702;
                public const uint Trade_BillSell = 703;
                public const uint Trade_Ravage = 704;
               
     
                public const uint Item_TradeState = 708;
                public const uint Trade_SellTypes = 709;

                //in out item

                public const uint Trade_ItemOut = 901;
                public const uint Trade_ItemIN = 902;
                public const uint Trade_ItemIN_SellPrice = 903;
                public const uint Trade_BillAdditionalClause = 904;
                //Store
                public const uint Trade_ItemsStore = 801;
                public const uint Trade_Store_Container = 802;
                public const uint Trade_Store_Place = 803;
               
            }
       

            public uint LogID;
            public DateTime LogDateTime;
            public uint _LogType;
            public uint _LogTarget;
            public string LogDesc;
            public string EmployeeName;
            public bool Success;
            internal string  ErrorMessage;

            public Log( uint LogID_,
                  DateTime LogDateTime_,
                    uint LogType_,
             uint LogTarget_,
            string LogDesc_,
             string EmployeeName_,
             bool Success_,
             string ErrorMessage_)
            { 
                 LogID= LogID_;
                _LogType = LogType_;
                _LogTarget = LogTarget_;
            LogDesc = LogDesc_;
             LogDateTime= LogDateTime_;
             EmployeeName= EmployeeName_;
             Success= Success_;
            ErrorMessage= ErrorMessage_;
        }
        }
        private static class LogTable
        {
            public const string TableName = "[dbo].[OverLoad_Log]";
            public const string LogID = "LogID";
            public const string LogDateTime = "LogDateTime";
            public const string LogType = "LogType";
            public const string LogTarget = "LogTarget";
            public const string LogDesc = "LogDesc";
            public const string EmployeeName = "EmployeeName";

            public const string Success = "Success";
            public const string ErrorMessage = "ErrorMessage";
        }
        internal  async void AddLog(uint LogType,uint LogTarget,string LogDesc,bool Success,string ErrorMessage)
        {
            ErrorMessage=ErrorMessage.Replace("'", string.Empty);
            try
            {
               
                string SQLCommand = " insert into "
                + LogTable.TableName
                + "("
                + LogTable.LogType 
                + ","
                 + LogTable.LogTarget
                + ","
                + LogTable.LogDesc
                + ","
                + LogTable.EmployeeName
                + ","
                + LogTable.Success
                 + ","
                + LogTable.ErrorMessage
                + ")"
                + "values"
                + "("
                + LogType 
                + ","
                + LogTarget
                + ","
                + "'" + LogDesc + "'"
                + ","
                 + (this._User == null ? "''" : (this._User._Employee == null ? "'مدير النظام'" :
                 "'"+ this._User._Employee.EmployeeName+"'"))
                + ","
                  + (Success == true ? "1" : "0")
                + ","
                + "'" + (ErrorMessage.Length <200?ErrorMessage:ErrorMessage .Substring (0,200)) + "'"
                + ")";
                
                //ShowSqlQuery dd = new ShowSqlQuery(SQLCommand); dd.ShowDialog();
                DATABASE_SQL_COMMAND.CommandText = SQLCommand;
                if (DATABASE_CONNECTION.State != ConnectionState.Open) /*DATABASE_CONNECTION.Close();*/
                    DATABASE_CONNECTION.Open();
                DATABASE_SQL_COMMAND.ExecuteNonQuery();


            }
            catch (Exception ee)
            {
                DataBaseLocked = true;
                throw new Exception(ee.Message );
                
            }
        }
        public List <Log > GetLogList()
        {
            List<Log> list = new List<Log>();
            try
            {
                DataTable t = this.GetData("select "
                    +LogTable .LogID+","
                    + LogTable.LogDateTime + ","
                    + LogTable.LogType + ","
                    + LogTable.LogTarget + ","
                    + LogTable.LogDesc + ","
                    + LogTable.EmployeeName  + ","
                    + LogTable.Success  + ","
                    + LogTable.ErrorMessage  
                    +" from "
                    +LogTable .TableName 
                    );
                for(int i=0;i<t.Rows .Count;i++)
                {
                    uint logid = Convert.ToUInt32(t.Rows [i][0]);
                    DateTime  logdaetime = Convert.ToDateTime (t.Rows[i][1]);
                    uint logtype = Convert.ToUInt32(t.Rows[i][2]);
                    uint logtarget = Convert.ToUInt32(t.Rows[i][3]);
                    string  desc = t.Rows[i][4].ToString ();
                    string employee_name = t.Rows[i][5].ToString();
                    bool success  = Convert.ToBoolean (t.Rows[i][6]);
                    string  errormesage = t.Rows[i][7].ToString ();
                    list.Add(new Log(logid ,logdaetime,logtype,logtarget ,desc,employee_name  ,success ,errormesage ));
                }
          
            }catch(Exception ee)
            {
                MessageBox.Show("GetLogList:" + ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return list;
        }
        #endregion
        #region User
        public  class User
        {
            public uint UserID;
            public string UserName;
            public DateTime adddate;
            public bool Disabled;
            internal Employee _Employee;
           
            public User(uint UserID_, string UserName_,DateTime adddate_,bool Disabled_, Employee Employee_)
            {
                UserID = UserID_;
                _Employee = Employee_;
                UserName = UserName_;
                adddate = adddate_;
                Disabled = Disabled_;
            }
        }

        private static class UserTable
        {
            public const string TableName = "[dbo].[OverLoad_User]";
            public const string OV_UserID = "OV_UserID";
            public const string OV_UserName = "OV_UserName";
            public const string OV_Password = "OV_Password";
            public const string AddDate = "AddDate";
            public const string Disabled_ = "Disabled_";
            public const string EmployeeID = "EmployeeID";
        }
        public User GetEmployeeUser(Employee Employee_)
        {
            try
            {
                DataTable t = new DataTable();
                t = this .GetData("select "
                 + UserTable.OV_UserID  + ","
                 + UserTable.OV_UserName + ","
                 + UserTable.AddDate  + ","
                 + UserTable.Disabled_ 
                + " from   "
                + UserTable.TableName
                + " where "
                + UserTable.EmployeeID  + "=" + Employee_.EmployeeID
                  );
                if (t.Rows.Count == 1)
                {
                    uint userid = Convert.ToUInt32(t.Rows[0][0].ToString());
                    string username = t.Rows[0][1].ToString();
                    DateTime adddate = Convert.ToDateTime(t.Rows[0][2].ToString());
                    bool disabled = Convert.ToBoolean(t.Rows[0][3].ToString());
                    return new User(userid, username, adddate, disabled, Employee_);
                }
                else
                    return null;
            }
            catch (Exception ee)
            {
                MessageBox.Show("GetEmployeeUser:" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public bool AddUser(uint EmployeeID, string UserName, string Password,bool Disabled)
        {
            try
            {
                this .ExecuteSQLCommand(
                    " insert into "
                + UserTable  .TableName
                + "("
                + UserTable.OV_UserName
                + ","
                + UserTable.OV_Password
                  + ","
                + UserTable.Disabled_ 
                + ","
                + UserTable.EmployeeID 

                + ")"
                + "values"
                + "("
                + "'" + UserName  + "'"
                + ","
                  + "'" + Password  + "'"
               + ","
                +( Disabled ==true ?"1":"0")
                  + ","
                + EmployeeID 
                + ")"
                );
                AddLog(Log.LogType.INSERT
                    , Log.Log_Target.User
                    , UserName
                    , true
                    , "");
                
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show("AddUser" + ee.Message , "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog(Log.LogType.INSERT
                   , Log.Log_Target.User
                   , UserName
                   , false 
                   ,ee.Message );
                return false;
            }

        }
        public bool EditUserData(uint EmployeeID, string UserName,bool Disable)
        {
            try
            {
                this .ExecuteSQLCommand("update  "
                + UserTable .TableName
                + " set "
                + UserTable.OV_UserName + "='" + UserName + "',"
                  + UserTable.Disabled_  + "=" + (Disable ?"1":"0")
                + " where "
                + UserTable.EmployeeID + "=" + EmployeeID
                );
                AddLog(Log.LogType.UPDATE 
                   , Log.Log_Target.User
                   , "تعديل اسم المستخدم للموظف رقم:"+EmployeeID.ToString ()
                   , true
                   , "");
                return true;
            }
            catch (Exception ee)
            {
                AddLog(Log.LogType.UPDATE
                  , Log.Log_Target.User
                  , "تعديل اسم المستخدم للموظف رقم:" + EmployeeID.ToString()
                  , false 
                  , ee.Message );
                MessageBox.Show("UpdateUserName" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public bool DeleteUser(uint EmployeeID)
        {
            try
            {
                this .ExecuteSQLCommand( "delete from   "
                + UserTable.TableName
                + " where "
                + UserTable .EmployeeID  + "=" + EmployeeID 
                );
                AddLog(Log.LogType.DELETE 
                  , Log.Log_Target.User
                  , "حذف  المستخدم للموظف رقم:" + EmployeeID.ToString()
                  , true
                  , "");
                return true;
            }
            catch (Exception ee)
            {
                AddLog(Log.LogType.DELETE 
                  , Log.Log_Target.User
                  , "حذف اسم المستخدم للموظف رقم:" + EmployeeID.ToString()
                  , true
                  , ee.Message );
                MessageBox.Show("DeleteUser", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public bool DisableUser(uint EmployeeID)
        {
            try
            {
                this.ExecuteSQLCommand( "update  "
                + UserTable.TableName
                + " set "
                + UserTable.Disabled_ + "=1"
                + " where "
                + UserTable.EmployeeID + "=" + EmployeeID
                );
                AddLog(Log.LogType.UPDATE 
                    , Log.Log_Target.User
                    , "تعطيل حساب الموظف رقم::" + EmployeeID.ToString()
                    , true, "");
                return true;
            }
            catch (Exception ee)
            {
                AddLog(Log.LogType.UPDATE
                   , Log.Log_Target.User
                   , "تعطيل حساب الموظف رقم::" + EmployeeID.ToString()
                   , false , ee.Message );
                MessageBox.Show("DisableUser" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public bool ResetUserPassword(uint EmployeeID, string NewPassword)
        {
            try
            {
                this.ExecuteSQLCommand("update  "
                + UserTable.TableName
                + " set "
                + UserTable.OV_Password  + "='"+NewPassword +"'"
                + " where "
                + UserTable.EmployeeID + "=" + EmployeeID
                );
                AddLog(Log.LogType.UPDATE
                    , Log.Log_Target.User
                    , "اعادة تعيين كلمة المرور للموظف رقم:" + EmployeeID.ToString()
                    , true, "");
                return true;
            }
            catch (Exception ee)
            {
                AddLog(Log.LogType.UPDATE
                    , Log.Log_Target.User
                    , "اعادة تعيين كلمة المرور للموظف رقم:" + EmployeeID.ToString()
                    , false , ee.Message );
                MessageBox.Show("ResetUserPassword" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public bool UpdateMYPassword(string oldPassword, string NewPassword)
        {
            try
            {
                DataTable t2 = GetData(" select *  from "
                      + UserTable.TableName
                     + " where "
                      + UserTable.EmployeeID + "=" + this._User._Employee.EmployeeID
                      + " and "
                      + UserTable.OV_Password + "='" + oldPassword + "'"
                     );
                if (t2.Rows.Count == 1)
                {
                    this.ExecuteSQLCommand( "update  "
                   + UserTable.TableName
                   + " set "
                   + UserTable.OV_Password + "='" + NewPassword + "'"
                   + " where "
                   + UserTable.EmployeeID + "=" + this._User._Employee.EmployeeID
                   );
                    AddLog(Log.LogType.UPDATE
                    , Log.Log_Target.User
                    , "تعديل كلمة المرور:" + this._User.UserName
                       , true, "");
                    return true;
                }
                else throw new Exception("كلمة المرور غير صحيحة");
            }
            catch(Exception ee)
            {
                AddLog(Log.LogType.UPDATE
                   , Log.Log_Target.User
                   , "تعديل كلمة المرور:" + this._User.UserName
                      , false , ee.Message );
                throw new Exception(ee.Message);
            }
            
        }

        internal void LogOut()
        {
            this._User = null;
        }
        #endregion


        #region Generalmethod

        #endregion
    }

}
