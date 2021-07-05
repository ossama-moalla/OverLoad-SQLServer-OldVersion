using ItemProject.Company.CompanySQL;
using ItemProject.Company.Forms;
using ItemProject.Company.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.AccountingObj.Forms
{
    public partial class ShowLogForm : Form
    {
        //System.Windows.Forms.MenuItem AddUserMenuItem;
        //System.Windows.Forms.MenuItem EditUserMenuItem;
        //System.Windows.Forms.MenuItem DeleteUserMenuItem;
        //System.Windows.Forms.MenuItem ResetPassword;

        //System.Windows.Forms.MenuItem OpenEmployeeMentMenuItem;
        //System.Windows.Forms.MenuItem EditEmployeeMentMenuItem;


        DatabaseInterface DB;
        List<DatabaseInterface.Log> LogsList;
        public ShowLogForm(DatabaseInterface db)
        {
            InitializeComponent();
            DB = db;
            LogsList = DB.GetLogList();
            fillComboBoxEmployee(LogsList);
            AdjustlistViewLogsColumnsWidth();
            //RefreshLogsList();

        }

        private void fillComboBoxEmployee(List<DatabaseInterface.Log> LogsList_)
        {
            List<string> employees = LogsList_.Select(x => x.EmployeeName).Distinct().ToList();
            comboBoxEmployee.Items.Add("الكل");
            for (int i = 0; i < employees.Count; i++)
                comboBoxEmployee.Items.Add(employees[i]);
            comboBoxEmployee.SelectedIndex = 0;
        }

        private async void RefreshLogsList()
        {
            ComboboxItem maintarget = (ComboboxItem)comboBoxMainTarget.SelectedItem;
            ComboboxItem slavetarget = (ComboboxItem)comboBoxSlaveTarget.SelectedItem;
            ComboboxItem target = (ComboboxItem)comboBoxTarget.SelectedItem;
            ComboboxItem type = (ComboboxItem)comboBoxLogType .SelectedItem;
            listViewLogs.Items.Clear();
            for (int i = 0; i < LogsList.Count; i++)
            {
                
                if (maintarget.Value  > 0)
                {

                    uint main, slave;

                    if (LogsList[i]._LogTarget > 100)
                        main = (LogsList[i]._LogTarget - (LogsList[i]._LogTarget % 100)) / 100;
                    else
                        main = 0;
    
                    if (maintarget.Value != main) continue;
                        if(slavetarget.Value >0)
                        {
                            uint slavetemp = LogsList[i]._LogTarget - main * 100;
                            slave = (slavetemp - slavetemp % 10) / 10;
                            if (slavetarget.Value !=slave) continue;
                           if(target.Value >0)
                                    if (LogsList[i]._LogTarget != target.Value) continue;

                        }
                   
                }
               if(type .Value >0)
                {
                    if (type.Value != LogsList[i]._LogType) continue;
                }
                if (comboBoxEmployee.SelectedIndex > 0 && comboBoxEmployee.SelectedItem.ToString() != LogsList[i].EmployeeName) continue;
                if (comboBoxSuccess.SelectedIndex > 0)
                {
                    if (comboBoxSuccess.SelectedIndex == 1 && LogsList[i].Success == false) continue;
                    if (comboBoxSuccess.SelectedIndex == 2 && LogsList[i].Success == true ) continue;
                }
                ListViewItem ListViewItem_ = new ListViewItem(LogsList[i].LogID.ToString());
                ListViewItem_.Name = LogsList[i].LogID .ToString();
                ListViewItem_.SubItems.Add(LogsList[i].LogDateTime .ToShortDateString () );
                ListViewItem_.SubItems.Add(LogsList[i].LogDateTime.ToShortTimeString());
                ListViewItem_.SubItems.Add(LogsList[i].EmployeeName);
                ListViewItem_.SubItems.Add(DatabaseInterface .Log.GetLogType_Name( LogsList[i]._LogType));
                ListViewItem_.SubItems.Add(DatabaseInterface.Log.GetLogTarget_Name(LogsList[i]._LogTarget ));
                ListViewItem_.SubItems.Add(LogsList[i].LogDesc);
           
                if (LogsList[i].Success  == false  )
                {
                    ListViewItem_.SubItems.Add("فشل");
                    ListViewItem_.SubItems.Add(LogsList[i].ErrorMessage );
                    ListViewItem_.BackColor = Color.Orange ;
                    
                }
                else
                {
                    ListViewItem_.SubItems.Add("نجاح");
                    ListViewItem_.SubItems.Add("  -  ");
                    ListViewItem_.BackColor = Color.LimeGreen;
                }
                listViewLogs.Items.Add(ListViewItem_);

            }

        }


        private void listViewLogs_MouseDown(object sender, MouseEventArgs e)
        {
            //    listViewLogs.ContextMenu = null;
            //    bool match = false;
            //    ListViewItem listitem = new ListViewItem();
            //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //    {
            //        foreach (ListViewItem item1 in listViewLogs.Items)
            //        {
            //            if (item1.Bounds.Contains(new Point(e.X, e.Y)))
            //            {
            //                match = true;
            //                listitem = item1;
            //                break;
            //            }
            //        }
            //        if (match)
            //        {
            //            MenuItem[] mi1;
            //            uint sid = Convert.ToUInt32(listitem.Name);
            //            List<Employee_User> list = LogsList.Where(x => x._Employee.EmployeeID == sid).ToList();
            //            if (list.Count > 0)
            //            {
            //                DatabaseInterface.User user = list[0]._user;
            //                if (user != null)
            //                {
            //                    mi1 = new MenuItem[] { EditUserMenuItem ,ResetPassword,DeleteUserMenuItem
            //                          , new MenuItem("-"), OpenEmployeeMentMenuItem ,EditEmployeeMentMenuItem };

            //                }
            //                else
            //                {
            //                    mi1 = new MenuItem[] { AddUserMenuItem
            //                          , new MenuItem("-"), OpenEmployeeMentMenuItem ,EditEmployeeMentMenuItem };
            //                }
            //                listViewLogs.ContextMenu = new ContextMenu(mi1);
            //            }
            //            else
            //                listViewLogs.ContextMenu = null;



            //        }


            //    }

        }
        private void listViewLogs_Resize(object sender, EventArgs e)
        {
            //MaintenanceEmployeesReport .AdjustlistViewEmployeesReportOPRColumnsWidth (ref listViewSubDiagnosticOPR);
            AdjustlistViewLogsColumnsWidth();
        }
        public async void AdjustlistViewLogsColumnsWidth()
        {
            try
            {
                listViewLogs.Columns[0].Width = 100;//id
                listViewLogs.Columns[1].Width = 100;//date
                listViewLogs.Columns[2].Width = 100;//time
                listViewLogs.Columns[3].Width = 150;//name
                listViewLogs.Columns[4].Width = 150;//type
                listViewLogs.Columns[5].Width =150;//target
                listViewLogs.Columns[6].Width = 325;//desc
     
                listViewLogs.Columns[7].Width = 100;//success
                listViewLogs.Columns[8].Width = 250;//errormessage
    
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("AdjustlistViewEmployeesColumnsWidth" + Environment.NewLine + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void comboBoxMainType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem item = (ComboboxItem)comboBoxMainTarget.SelectedItem;
  
                DB.FillComboBox_SlaveTareget(ref comboBoxSlaveTarget, item.Value);
                comboBoxSlaveTarget.Enabled = true;
                comboBoxSlaveTarget.SelectedIndex = 0;

            if (comboBoxSlaveTarget.Items.Count == 1) comboBoxSlaveTarget.Enabled = false;
           

        }

        private void ShowLogForm_Load(object sender, EventArgs e)
        {
            DB.FillComboBoxLogTypes(ref comboBoxLogType);
            comboBoxLogType.SelectedIndex = 0;
            this.comboBoxLogType.SelectedIndexChanged += new System.EventHandler(this.comboBoxLogType_SelectedIndexChanged);

            DB.FillComboBox_MainTarget(ref comboBoxMainTarget);
            this.comboBoxMainTarget.SelectedIndexChanged += new System.EventHandler(this.comboBoxMainType_SelectedIndexChanged);
            this.comboBoxSlaveTarget.SelectedIndexChanged += new System.EventHandler(this.comboBoxSlaveType_SelectedIndexChanged);

            comboBoxMainTarget.SelectedIndex = 0;
            comboBoxSuccess .SelectedIndex = 0;
            this.comboBoxEmployee.SelectedIndexChanged += new System.EventHandler(this.comboBoxEmployee_SelectedIndexChanged);
            this.comboBoxSuccess.SelectedIndexChanged += new System.EventHandler(this.comboBoxSuccess_SelectedIndexChanged);

        }

        private void comboBoxSlaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem itemmain = (ComboboxItem)comboBoxMainTarget.SelectedItem;
            ComboboxItem itemslave = (ComboboxItem)comboBoxSlaveTarget.SelectedItem;


                DB.FillComboBox_Targets(ref comboBoxTarget, itemmain.Value, itemslave.Value);
                comboBoxTarget.Enabled = true;
                comboBoxTarget.SelectedIndex = 0;

            if (comboBoxTarget.Items.Count == 1) comboBoxTarget.Enabled = false;

            
        }

        private void comboBoxTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboboxItem maintype = (ComboboxItem)comboBoxMainTarget.SelectedItem;
            //ComboboxItem slavetype = (ComboboxItem)comboBoxSlaveTarget.SelectedItem;
            //ComboboxItem targettype = (ComboboxItem)comboBoxTarget.SelectedItem;
            //MessageBox.Show(maintype.Value + ":" + slavetype.Value + ":" + targettype.Value);
            RefreshLogsList();
        }

        private void comboBoxLogType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshLogsList();
        }

        private void comboBoxEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshLogsList(); 
        }

        private void comboBoxSuccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshLogsList(); 
        }
    }
}