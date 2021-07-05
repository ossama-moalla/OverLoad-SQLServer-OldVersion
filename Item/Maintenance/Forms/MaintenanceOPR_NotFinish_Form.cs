using ItemProject.Maintenance.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Maintenance.Forms
{
    public partial class MaintenanceOPR_NotFinish_Form : Form
    {
        System.Windows.Forms.MenuItem CreateMaintenanceOPR_MenuItem;
        System.Windows.Forms.MenuItem OpenMaintenanceOPR_MenuItem;
        System.Windows.Forms.MenuItem EditMaintenanceOPR_MenuItem;
        System.Windows.Forms.MenuItem DeleteMaintenanceOPR_MenuItem;

        DatabaseInterface DB;
        
        MaintenanceSQL.MaintenanceOPRSQL MaintenanceOPRSQL_;
        public MaintenanceOPR_NotFinish_Form(DatabaseInterface db)
        {
            DB = db;
            InitializeComponent();
            labelDateNow.Text = DateTime.Now.ToShortDateString();
            MaintenanceOPRSQL_ = new MaintenanceSQL.MaintenanceOPRSQL(DB);
            CreateMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("انشاء عملية صيانة", CreateMaintenanceOPR_MenuItem_Click);
            OpenMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("فتح", OpenMaintenanceOPR_MenuItem_Click);
            EditMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("تعديل", EditMaintenanceOPR_MenuItem_Click);
            DeleteMaintenanceOPR_MenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteMaintenanceOPR_MenuItem_Click);

            RefreshMaintenanceOPRs();
        }
        public async void RefreshMaintenanceOPRs()
        {
            List<MaintenanceOPR> MaintenanceOPRList = MaintenanceOPRSQL_.GetNOT_Finsh_MaintenanceOPRList();

            for (int i = 0; i < MaintenanceOPRList.Count; i++)
            {

                ListViewItem ListViewItem_ = new ListViewItem(MaintenanceOPRList[i]._Operation.OperationID.ToString());
                ListViewItem_.Name = MaintenanceOPRList[i]._Operation.OperationID.ToString();
  
                ListViewItem_.SubItems.Add(MaintenanceOPRList[i].EntryDate.ToShortDateString ());
                ListViewItem_.SubItems.Add(MaintenanceOPRList[i]._Contact.Get_Complete_ContactName_WithHeader());
                ListViewItem_.SubItems.Add(MaintenanceOPRList[i]._Item.folder.FolderName);
                ListViewItem_.SubItems.Add(MaintenanceOPRList[i]._Item.ItemName);
                ListViewItem_.SubItems.Add(MaintenanceOPRList[i]._Item.ItemCompany);

                ListViewItem_.SubItems.Add(MaintenanceOPRList[i].FaultDesc);
                ListViewItem_.BackColor = Color.LightYellow;
                listViewMaintenanceOPRs.Items.Add(ListViewItem_);
            }
        }
        private void ListViewMaintenanceOPRs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewMaintenanceOPRs.SelectedItems.Count > 0)
                OpenMaintenanceOPR_MenuItem.PerformClick();
        }
        private void ListViewMaintenanceOPRs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                OpenMaintenanceOPR_MenuItem.PerformClick();
        }
        private void ListViewMaintenanceOPRs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listViewMaintenanceOPRs.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    foreach (ListViewItem item1 in listViewMaintenanceOPRs.Items)
                    {
                        if (item1.Bounds.Contains(new Point(e.X, e.Y)))
                        {
                            match = true;
                            listitem = item1;
                            break;
                        }
                    }

                    if (match)
                    {

                            List<MenuItem> MenuItemList = new List<MenuItem>();
                            MenuItemList.AddRange(new MenuItem[] {OpenMaintenanceOPR_MenuItem , EditMaintenanceOPR_MenuItem , DeleteMaintenanceOPR_MenuItem
                            , new MenuItem("-"),CreateMaintenanceOPR_MenuItem });
                            listViewMaintenanceOPRs.ContextMenu = new ContextMenu(MenuItemList.ToArray());


                       

                    }
                    else
                    {
 
                            MenuItem[] mi1 = new MenuItem[] { CreateMaintenanceOPR_MenuItem };
                            listViewMaintenanceOPRs.ContextMenu = new ContextMenu(mi1);

                    }

                }
            }
        }
        private void CreateMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB, null);
            MaintenanceOPRForm_.ShowDialog();
            if (MaintenanceOPRForm_.Changed)
            {
                RefreshMaintenanceOPRs();
            }
        }
        private void OpenMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            uint MaintenanceOPRid = Convert.ToUInt32(listViewMaintenanceOPRs.SelectedItems[0].Name);
            MaintenanceOPR MaintenanceOPR_ = MaintenanceOPRSQL_.GetMaintenancePRINFO_BYID(MaintenanceOPRid);
            Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB, MaintenanceOPR_, false);
            MaintenanceOPRForm_.ShowDialog();
            if (MaintenanceOPRForm_.Changed)
            {
                
            }
            MaintenanceOPRForm_.Dispose();
        }
        private void EditMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            uint MaintenanceOPRid = Convert.ToUInt32(listViewMaintenanceOPRs.SelectedItems[0].Name);
            MaintenanceOPR MaintenanceOPR_ = MaintenanceOPRSQL_.GetMaintenancePRINFO_BYID(MaintenanceOPRid);
            Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB, MaintenanceOPR_, true);
            MaintenanceOPRForm_.ShowDialog();
            if (MaintenanceOPRForm_.Changed)
            {
                RefreshMaintenanceOPRs();
            }
            MaintenanceOPRForm_.Dispose();
        }
        private void DeleteMaintenanceOPR_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dd = MessageBox.Show("هل أنت متأكد من الحذف؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dd != DialogResult.OK) return;
            uint MaintenanceOPRid = Convert.ToUInt32(listViewMaintenanceOPRs.SelectedItems[0].Name);
            bool success = MaintenanceOPRSQL_.DeleteMaintenanceOPR(MaintenanceOPRid);
            if (success)
            {
                RefreshMaintenanceOPRs();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
