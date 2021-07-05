using ItemProject.ItemObj.Objects;
using ItemProject.Trade.Objects;
using ItemProject.Trade.TradeSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Trade.Forms.Container
{
    public partial class PlaceItem_ItemINForm : Form
    {
        TradeStorePlace place;
        System.Windows.Forms.MenuItem OpenItemIN;
        Item _Item;
        DatabaseInterface DB;
        List <PlaceAvailbeItems_ItemINDetails > StoredItems;
        private ItemIN _ReturnItemIN;
        bool GetItemIN;
        public ItemIN ReturnItemIN
        {
            get { return _ReturnItemIN; }
        }
        public PlaceItem_ItemINForm(DatabaseInterface db,TradeStorePlace Place_,Item Item_,bool GetItemIN_)
        {
            InitializeComponent();
            DB = db;
            OpenItemIN = new System.Windows.Forms.MenuItem("عرض تفاصيل عملية الادخال", OpenItemIN_MenuItem_Click);

            GetItemIN = GetItemIN_;
            if (GetItemIN) Select.Visible = true;
            place = Place_;
            _Item = Item_;
            if (GetItemIN) Select.Visible = true;
            else Select.Visible = false;
            ConfigureListviewColumns();
        }

        private void OpenItemIN_MenuItem_Click(object sender, EventArgs e)
        {
            ItemIN ItemIN_ = new ItemINSQL (DB).GetItemININFO_BYID(Convert.ToUInt32(listView1.SelectedItems[0].Name));
            TradeForms.ItemINForm ItemINInfoForm_ = new TradeForms.ItemINForm(DB, ItemIN_, false);
            ItemINInfoForm_.ShowDialog();
        }

        private void PlaceForm_Load(object sender, EventArgs e)
        {
            try
            {
                textBoxPlaceID.Text  = place.PlaceID.ToString ();
                textBoxPlaceName.Text  = place.PlaceName;
                textBoxPlaceLocation.Text  = place._TradeStoreContainer.ContainerName;
                textBoxDesc.Text = place.Desc;
                textBoxItemName.Text = _Item.ItemName;
                textBoxItemCompany.Text = _Item.ItemCompany ;
                textBoxItemType.Text = _Item.folder .FolderName ;
                StoredItems = new AvailableItemSQL(DB).GetStoredItems_BuyOPRDetails(place,_Item );
                FillComboBox_TradeState();
                RefreshList();
            }
            catch
            {
                MessageBox.Show("فشل تحميل صفحة مكان التخزين","",MessageBoxButtons.OK ,MessageBoxIcon.Error );
                this.Close();
            }
        }
   
        private void FillComboBox_TradeState()
        {
 
            IEnumerable<string> distincTradeStates = StoredItems.Select(x => x.ItemStoreType ).Distinct();
            comboBoxStateFilter .Items.Clear();
            comboBoxStateFilter.Items.Add("الكل");
            foreach (var s in distincTradeStates)
                comboBoxStateFilter.Items.Add(s);
            comboBoxStateFilter.SelectedIndex = 0;
        }

        private void RefreshList()
        {
           try
            {
                listView1.Items.Clear();
                for(int i=0;i<StoredItems .Count;i++)
                {
                    if (comboBoxStateFilter.SelectedIndex > 0 && StoredItems[i].ItemStoreType  != comboBoxStateFilter.SelectedItem.ToString()) continue;
                    if (comboBoxContactFilter.SelectedIndex > 0 && StoredItems[i].ParentOperationDesc  != comboBoxContactFilter.SelectedItem.ToString()) continue;


                    ListViewItem ListViewItem_ = new ListViewItem(StoredItems[i].ItemStoreType  .ToString ());
                    if(StoredItems[i].StoreType==TradeItemStore .ITEMIN_STORE_TYPE)
                        ListViewItem_.Name = "I"+StoredItems[i].OprID   .ToString();
                    else if (StoredItems[i].StoreType == TradeItemStore.MAINTENANCE_ITEM_STORE_TYPE)
                        ListViewItem_.Name = "M" + StoredItems[i].OprID.ToString();
                    else if (StoredItems[i].StoreType == TradeItemStore.MAINTENANCE_ACCESSORIES_ITEM_STORE_TYPE)
                        ListViewItem_.Name = "A" + StoredItems[i].OprID.ToString();
                    ListViewItem_.SubItems.Add(StoredItems[i].OprID .ToString());
                    ListViewItem_.SubItems.Add( StoredItems[i].ParentOperationDesc);
                    ListViewItem_.SubItems.Add(StoredItems[i].ParentOperationID   .ToString());
                    ListViewItem_.SubItems.Add(StoredItems[i].consumeunitname  .ToString());
                    ListViewItem_.SubItems.Add(StoredItems[i].StoredAmount .ToString());
                    ListViewItem_.SubItems.Add(StoredItems[i].SpentAmount .ToString());
                    ListViewItem_.SubItems.Add(StoredItems[i].AvailableAmount .ToString());
                    ListViewItem_.BackColor = Color.LightSteelBlue;
                    listView1.Items.Add(ListViewItem_);
                }
            }

              catch
            {
                MessageBox.Show("حصل خطأ اثناء تحديث القائمة","",MessageBoxButtons.OK,MessageBoxIcon.Error );
            }
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void comboBoxStateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<PlaceAvailbeItems_ItemINDetails> filteredlist_byState = new List<PlaceAvailbeItems_ItemINDetails>();
            if (comboBoxStateFilter .SelectedIndex == 0)
                filteredlist_byState = StoredItems;
            else
                filteredlist_byState = StoredItems.Where(item => item.ItemStoreType  == comboBoxStateFilter.SelectedItem.ToString()).ToList();
            IEnumerable<string  > distinctSources= filteredlist_byState.Select(x => x.ParentOperationDesc   ).Distinct();
            comboBoxContactFilter.Items.Clear();
            comboBoxContactFilter.Items.Add("الكل");
            foreach (var s in distinctSources)
                comboBoxContactFilter.Items.Add( s);
            comboBoxContactFilter.SelectedIndex = 0;
        }

        private void comboBoxContactFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshList();
        }
        private void Select_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].SubItems[1].Text == "مكان تخزين")
            {
                ReturItemIN();
            }
        }
        public void ReturItemIN()
        {
            _ReturnItemIN = new ItemINSQL(DB).GetItemININFO_BYID((Convert.ToUInt32(listView1.SelectedItems[0].SubItems[0].Name)));
            if (_ReturnItemIN != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else MessageBox.Show("حصل خطأ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left && listView1.SelectedItems.Count > 0)
            {

                if (!GetItemIN)
                {
                    string s = listView1 .SelectedItems[0].Name;
                    uint id= Convert.ToUInt32(s.Substring(1));
                 
                    if (s.Substring(0, 1) == "A")
                    {
                        Maintenance.Objects.MaintenanceOPR_Accessory Accessory_ = new Maintenance.MaintenanceSQL.MaintenanceAccessorySQL(DB).Get_Accessory_INFO_BYID(id);
                        Maintenance.Forms.MaintenanceAccessoryForm MaintenanceAccessoryForm_ = new Maintenance.Forms.MaintenanceAccessoryForm(DB, Accessory_, false);
                        MaintenanceAccessoryForm_.ShowDialog();
                    }
                    else if (s.Substring(0, 1) =="M")
                    {
                        Maintenance.Objects.MaintenanceOPR MaintenanceOPR_ = new Maintenance.MaintenanceSQL.MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(id);
                        Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB, MaintenanceOPR_, false);
                        MaintenanceOPRForm_.ShowDialog();

                    }
                    else
                    {
                        ItemIN ItemIN_ = new ItemINSQL(DB).GetItemININFO_BYID(id);
                        TradeForms.ItemINForm BuyOprInfoForm_ = new TradeForms.ItemINForm(DB, ItemIN_, false);
                        BuyOprInfoForm_.ShowDialog();

                    }
                  
                }
                else ReturItemIN();

            }
        }
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {

                if (e.KeyData == Keys.Enter)
                {
                    if (!GetItemIN)
                    {

                        string s = listView1.SelectedItems[0].Name;
                        uint id = Convert.ToUInt32(s.Substring(1));

                        if (s.Substring(0, 1) == "A")
                        {
                            Maintenance.Objects.MaintenanceOPR_Accessory Accessory_ = new Maintenance.MaintenanceSQL.MaintenanceAccessorySQL(DB).Get_Accessory_INFO_BYID(id);
                            Maintenance.Forms.MaintenanceAccessoryForm MaintenanceAccessoryForm_ = new Maintenance.Forms.MaintenanceAccessoryForm(DB, Accessory_, false);
                            MaintenanceAccessoryForm_.ShowDialog();
                        }
                        else if (s.Substring(0, 1) == "M")
                        {
                            Maintenance.Objects.MaintenanceOPR MaintenanceOPR_ = new Maintenance.MaintenanceSQL.MaintenanceOPRSQL(DB).GetMaintenancePRINFO_BYID(id);
                            Maintenance.Forms.MaintenanceOPRForm MaintenanceOPRForm_ = new Maintenance.Forms.MaintenanceOPRForm(DB, MaintenanceOPR_, false);
                            MaintenanceOPRForm_.ShowDialog();

                        }
                        else
                        {
                            ItemIN ItemIN_ = new ItemINSQL(DB).GetItemININFO_BYID(id);
                            TradeForms.ItemINForm BuyOprInfoForm_ = new TradeForms.ItemINForm(DB, ItemIN_, false);
                            BuyOprInfoForm_.ShowDialog();

                        }
                    }
                    else ReturItemIN();

                }

            }
        }
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                List<MenuItem> MenuItemList = new List<MenuItem>();
                listView1.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                foreach (ListViewItem item1 in listView1.Items)
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

                    MenuItem[] mi1 = new MenuItem[] { OpenItemIN };
                    MenuItemList.AddRange(mi1);

                }
                //////////////

                listView1.ContextMenu = new ContextMenu(MenuItemList.ToArray());

            }

        }
        private void ConfigureListviewColumns()
        {
            int columnswidth = (listView1.Width / listView1.Columns.Count) - 1; ;
            for (int i = 0; i < listView1.Columns.Count; i++)
                listView1.Columns[i].Width = columnswidth;
        }
        private void listView1_Resize(object sender, EventArgs e)
        {
            ConfigureListviewColumns();
        }
    }
}
