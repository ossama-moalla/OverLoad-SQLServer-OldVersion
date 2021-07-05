using ItemProject.Company.CompanySQL;
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

namespace ItemProject.Company.Forms
{
    public partial class CompanySalarysForm : Form
    {
        MenuItem OpenSalarysPayOrder_MenuItem;
        MenuItem CreateSalarysPayOrder_MenuItem;
        MenuItem UpdateSalarysPayOrder_MenuItem;
        MenuItem DeleteSalarysPayOrder_MenuItem;
        DatabaseInterface DB;
        AccountingObj.Objects.Currency ReferenceCurrency;
        public CompanySalarysForm(DatabaseInterface db)
        {
            DB = db;
            InitializeComponent();
            ReferenceCurrency = new AccountingObj.AccountingSQL.CurrencySQL(DB).GetReferenceCurrency();

            OpenSalarysPayOrder_MenuItem = new System.Windows.Forms.MenuItem("فتح تفاصيل أمر الصرف ", OpenSalarysPayOrder_MenuItem_Click);
            CreateSalarysPayOrder_MenuItem = new System.Windows.Forms.MenuItem("صرف راوتب هذا الشهر ", CreateSalarysPayOrder_MenuItem_Click);
            UpdateSalarysPayOrder_MenuItem = new System.Windows.Forms.MenuItem("تعديل أمر الصرف", UpdateSalarysPayOrder_MenuItem_Click);
            DeleteSalarysPayOrder_MenuItem = new System.Windows.Forms.MenuItem("حذف أمر الصرف ", DeleteSalarysPayOrder_MenuItem_Click);
            textBoxYear.Text = DateTime.Now.Year.ToString();
            GetYearSalaris(DateTime.Now.Year);
            this.textBoxYear.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            AdjustlistViewYearSalaries_ColumnsWidth();
            this.listViewYearSalaries.Resize += new System.EventHandler(this.listViewYearSalaries_Resize);

        }
        public async  void GetYearSalaris(int year)
        {
            List<SalarysPayOrderMonthReport> SalarysPayOrderMonthReportList = new SalarysPayOrderSQL(DB).Get_GetSalarysPayOrderMonthReport_List_In_Year(year);

            listViewYearSalaries.Items.Clear();

            for (int i = 0; i < SalarysPayOrderMonthReportList.Count; i++)
            {
                ListViewItem ListViewItem__ = new ListViewItem(SalarysPayOrderMonthReportList[i].Year .ToString ());
                ListViewItem__.Name = SalarysPayOrderMonthReportList[i].Year .ToString("D4") +
                    SalarysPayOrderMonthReportList[i].MonthNO.ToString("D2") +
                    SalarysPayOrderMonthReportList[i].SalarysPayOrderID;
                ListViewItem__.SubItems.Add(SalarysPayOrderMonthReportList[i].MonthNO.ToString ());
                ListViewItem__.SubItems.Add(SalarysPayOrderMonthReportList[i].MonthName);
                    ListViewItem__.SubItems.Add(SalarysPayOrderMonthReportList[i].SalarysPayOrderID);
                    ListViewItem__.SubItems.Add(SalarysPayOrderMonthReportList[i].SalarysPayOrderDate);
                    ListViewItem__.SubItems.Add(SalarysPayOrderMonthReportList[i].EmployeesCount);
                    ListViewItem__.SubItems.Add(SalarysPayOrderMonthReportList[i].MoneyAmount);


                listViewYearSalaries.Items.Add(ListViewItem__);

            }
            FillReport( year );
        }
        private async void FillReport(int year)
        {
            List<SalarysPayOrderReport_Currency> SalarysPayOrderYearReport_Currency_List =
                new SalarysPayOrderSQL(DB).Get_GetSalarysPayOrder_Year_Report_Currency_List(year) ;

            double real_value_all = 0;
            string value_all = "";

            //List<SalarysPayOrderEmployeeReport> SalarysPayOrderEmployeeReportList_notnull = SalarysPayOrderEmployeeReportList_.Where(x => x.PayedSalaryValue != null).ToList();
            //List<Currency> ByCurrency = SalarysPayOrderEmployeeReportList_notnull.Select(x => x.PayedSalaryCurrecny).Distinct().ToList();

            for (int j = 0; j < SalarysPayOrderYearReport_Currency_List.Count; j++)
            {
                value_all += " "+SalarysPayOrderYearReport_Currency_List[j].SalarysValue + SalarysPayOrderYearReport_Currency_List[j].CurrencyName;
                if (j != SalarysPayOrderYearReport_Currency_List.Count - 1)
                    value_all += " , ";
                real_value_all += SalarysPayOrderYearReport_Currency_List[j].RealSalarysValue;
            }
            textBoxValueAll.Text = value_all;
            textBoxRealValueAll.Text = System.Math.Round(real_value_all, 2).ToString() + " " + ReferenceCurrency.CurrencyName;
        }
        private async  void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBoxYear.Text .Length ==4)
            {
                try
                {
                    int year = Convert.ToInt32(textBoxYear.Text );
                    GetYearSalaris(year);
                }
                catch
                {

                }
            }
        }
        private void ButtonLeftRight_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            bool left;
            if (b.Name == "ButtonLeft") left = true;
            else left = false;
            try
            {
                if (left)
                {
                    int year = Convert.ToInt32(textBoxYear.Text);
                    textBoxYear.Text = (year - 1).ToString();
                }
                else
                {
                    int year = Convert.ToInt32(textBoxYear.Text);
                    textBoxYear.Text = (year + 1).ToString();
                }

            }
            catch
            {

            }
         
        }
        private void DeleteSalarysPayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewYearSalaries.SelectedItems.Count > 0)
            {
                try
                {
                    int year = Convert.ToInt32(listViewYearSalaries.SelectedItems[0].Name.Substring(0, 4));
                    int month = Convert.ToInt32(listViewYearSalaries.SelectedItems[0].Name.Substring(4, 2));

                    DialogResult dd = MessageBox.Show("هل انت متاكد من حذف أمر صرف الرواتب لسنة؟  :"+year.ToString ()+" شهر "+month.ToString (), "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dd != DialogResult.OK) return;
                    uint salaryspayorderid = Convert.ToUInt32(listViewYearSalaries.SelectedItems[0].Name.Substring(6));
                    bool success = new SalarysPayOrderSQL(DB).Delete_SalarysPayOrder (salaryspayorderid);
     
                    if (success )
                    {
                        try
                        {
                            
                            int year1 = Convert.ToInt32(textBoxYear.Text);
                            GetYearSalaris(year1);
                        }
                        catch
                        {

                        }
                       
                    }

                }
                catch
                {

                }

            }
        }
        private void UpdateSalarysPayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewYearSalaries.SelectedItems.Count > 0)
            {
                try
                {
                    uint salaryspayorderid = Convert.ToUInt32(listViewYearSalaries.SelectedItems[0].Name.Substring(6));
                    SalarysPayOrder SalarysPayOrder_ = new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByID(salaryspayorderid);
                    SalarysPayOrderForm SalarysPayOrderForm_ = new SalarysPayOrderForm(DB, SalarysPayOrder_, true );
                    SalarysPayOrderForm_.ShowDialog();
                    if (SalarysPayOrderForm_.Changed)
                    {
                        int year = Convert.ToInt32(textBoxYear.Text);
                        GetYearSalaris(year);
                    }

                }
                catch
                {

                }

            }
        }
        private void OpenSalarysPayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewYearSalaries.SelectedItems.Count > 0)
            {
                try
                {
                    uint salaryspayorderid = Convert.ToUInt32(listViewYearSalaries.SelectedItems[0].Name.Substring(6));
                    SalarysPayOrder SalarysPayOrder_ = new SalarysPayOrderSQL(DB).Get_SalarysPayOrder_Info_ByID(salaryspayorderid);
                    SalarysPayOrderForm SalarysPayOrderForm_ = new SalarysPayOrderForm(DB, SalarysPayOrder_, false );
                    SalarysPayOrderForm_.ShowDialog();
                    if (SalarysPayOrderForm_.Changed)
                    {
                        int year = Convert.ToInt32(textBoxYear.Text);
                        GetYearSalaris(year);
                    }

                }
                catch
                {

                }

            }
        }
        private void CreateSalarysPayOrder_MenuItem_Click(object sender, EventArgs e)
        {
            if (listViewYearSalaries .SelectedItems.Count > 0)
            {
                try
                {
                    int year = Convert.ToInt32(listViewYearSalaries.SelectedItems [0].Name.Substring(0, 4));
                    int month = Convert.ToInt32(listViewYearSalaries.SelectedItems[0].Name.Substring(4, 2));
                    SalarysPayOrderForm SalarysPayOrderForm_ = new SalarysPayOrderForm(DB,year,month );
                    SalarysPayOrderForm_.ShowDialog();
                    if(SalarysPayOrderForm_.Changed )
                    {
                        int year1 = Convert.ToInt32(textBoxYear.Text);
                        GetYearSalaris(year1);
                    }

                }
                catch
                {

                }

            }
        }
        private void listViewYearSalaries_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left && listViewYearSalaries.SelectedItems.Count > 0)
            {
                try
                {

                    uint salaryspayorderid = Convert.ToUInt32(listViewYearSalaries.SelectedItems [0].Name.Substring(5));
                    OpenSalarysPayOrder_MenuItem.PerformClick();
                }
                catch
                {
                    CreateSalarysPayOrder_MenuItem.PerformClick();
                }
                
            }
        }
 
        private void listViewYearSalaries_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                List<MenuItem> MenuItemList = new List<MenuItem>();
                listViewYearSalaries.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                foreach (ListViewItem EmployeeMent1 in listViewYearSalaries.Items)
                {
                    if (EmployeeMent1.Bounds.Contains(new Point(e.X, e.Y)))
                    {
                        match = true;
                        listitem = EmployeeMent1;
                        break;
                    }
                }
                if (match)
                {
                    MenuItem[] mi1;
                    try
                    {

                        uint salaryspayorderid = Convert.ToUInt32(listitem.Name.Substring(5));
                        mi1 = new MenuItem[] {OpenSalarysPayOrder_MenuItem   ,UpdateSalarysPayOrder_MenuItem ,DeleteSalarysPayOrder_MenuItem
                              };
                    }
                    catch
                    {
                        mi1 = new MenuItem[] {CreateSalarysPayOrder_MenuItem    };
                    }

                    MenuItemList.AddRange(mi1);
                    listViewYearSalaries.ContextMenu = new ContextMenu(MenuItemList.ToArray());


                }
                else listViewYearSalaries.ContextMenu = null;
            }

        }
        public async void AdjustlistViewYearSalaries_ColumnsWidth()
        {
            try
            {
                listViewYearSalaries.Columns[0].Width = 100;//year no
                listViewYearSalaries.Columns[1].Width = 100;//month no
                listViewYearSalaries.Columns[2].Width = 150;//month name
                listViewYearSalaries.Columns[3].Width = 150;//orderid
                listViewYearSalaries.Columns[4].Width = 150;//order date
                listViewYearSalaries.Columns[5].Width = 200;//employees count
                listViewYearSalaries.Columns[6].Width = (listViewYearSalaries.Width - 855) ;//moneyamount


            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("AdjustlistViewDocuments_ColumnsWidth" + Environment.NewLine + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void listViewYearSalaries_Resize(object sender, EventArgs e)
        {
            AdjustlistViewYearSalaries_ColumnsWidth();
        }
    }
}

