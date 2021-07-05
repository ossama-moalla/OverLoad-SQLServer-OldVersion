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
    public partial class PlaceItemsForm : Form
    {
        System.Windows.Forms.MenuItem ShowSources;
        System.Windows.Forms.MenuItem OpenItem;
        TradeStorePlace place;
        DatabaseInterface DB;
        private Item  _ReturnItem;
        bool GetItem;
        public Item ReturnItem
        {
            get { return _ReturnItem; }
        }
        List <PlaceAvailbeItems_ItemDetails > StoredItems;
        public PlaceItemsForm(DatabaseInterface db,TradeStorePlace Place_,bool GetItem_)
        {
            InitializeComponent();
            DB = db;
            GetItem = GetItem_;
            place = Place_;
            if (GetItem) Select.Visible = true;
            else Select.Visible = false;
            ShowSources = new System.Windows.Forms.MenuItem("استعراض المصادر", ShowSources_MenuItem_Click);
            OpenItem  = new System.Windows.Forms.MenuItem("فتح صفحة العنصر", OpenItem_MenuItem_Click);

        }

        private void OpenItem_MenuItem_Click(object sender, EventArgs e)
        {

            string id_s = listView1.SelectedItems[0].Name;

            try
            {
                Item item = new ItemObj.ItemObjSQL.ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(id_s));
                ItemProject.ItemObj.Forms.ItemForm form = new ItemProject.ItemObj .Forms.ItemForm(DB, item);
                form.ShowDialog();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void ShowSources_MenuItem_Click(object sender, EventArgs e)
        {
            string id_s = listView1.SelectedItems[0].Name;

            try
            {
                Item item = new ItemObj.ItemObjSQL.ItemSQL(DB).GetItemInfoByID(Convert.ToUInt32(id_s));
                PlaceItem_ItemINForm form = new PlaceItem_ItemINForm(DB,place , item,false );
                form.ShowDialog();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void PlaceForm_Load(object sender, EventArgs e)
        {
            try
            {
                textBoxPlaceID.Text  = place.PlaceID.ToString ();
                textBoxPlaceName.Text  = place.PlaceName;
                textBoxPlaceLocation.Text  = place._TradeStoreContainer.ContainerName;
                textBoxDesc.Text = place.Desc;
                StoredItems = new AvailableItemSQL (DB).GetStoredItems(place.PlaceID);
                FillComboBox_Company_Folder();
                RefreshStoredItems();
                ConfigureListViewColumns();
                this.listView1.Resize += new System.EventHandler(this.listView1_Resize);
                
            }
            catch
            {
                MessageBox.Show("فشل تحميل صفحة مكان التخزين","",MessageBoxButtons.OK ,MessageBoxIcon.Error );
                this.Close();
            }
        }
   
        private void FillComboBox_Company_Folder()
        {
 
            IEnumerable<string> distincfolders = StoredItems.Select(x => x.ItemFolder).Distinct();
            comboBoxFolderFilter .Items.Clear();
            comboBoxFolderFilter.Items.Add("الكل");
            foreach (var s in distincfolders)
                comboBoxFolderFilter.Items.Add(s);
            comboBoxFolderFilter.SelectedIndex = 0;
        }

        private void RefreshStoredItems()
        {
           try
            {
                listView1.Items.Clear();
                for(int i=0;i<StoredItems .Count;i++)
                {
                    if (comboBoxCompanyFilter.SelectedIndex > 0 && StoredItems[i].ItemCompany != comboBoxCompanyFilter.SelectedItem.ToString()) continue;
                    if (comboBoxFolderFilter.SelectedIndex > 0 && StoredItems[i].ItemFolder != comboBoxFolderFilter.SelectedItem.ToString()) continue;

                    ListViewItem ListViewItem_ = new ListViewItem(StoredItems[i].ItemID .ToString ());
                    ListViewItem_.Name = StoredItems[i].ItemID.ToString();
                    ListViewItem_.SubItems.Add(StoredItems[i].ItemName .ToString());
                    ListViewItem_.SubItems.Add(StoredItems[i].ItemCompany .ToString());
                    ListViewItem_.SubItems.Add(StoredItems[i].ItemFolder .ToString());
                    ListViewItem_.SubItems.Add(StoredItems[i].AvailableItemStates.ToString());
                    ListViewItem_.BackColor = Color.LightSteelBlue  ;
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
            RefreshStoredItems();
        }

        private void comboBoxFolderFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<PlaceAvailbeItems_ItemDetails> filteredlist_byFolder = new List<PlaceAvailbeItems_ItemDetails>();
            if (comboBoxFolderFilter.SelectedIndex == 0)
                filteredlist_byFolder = StoredItems;
            else
                filteredlist_byFolder = StoredItems.Where(item => item.ItemFolder == comboBoxFolderFilter.SelectedItem.ToString ()).ToList ();
            IEnumerable<string> distinctcompanies = filteredlist_byFolder.Select(x => x.ItemCompany).Distinct();
            comboBoxCompanyFilter.Items.Clear();
            comboBoxCompanyFilter.Items.Add("الكل");
            foreach (var s in distinctcompanies)
                comboBoxCompanyFilter.Items.Add(s);
            comboBoxCompanyFilter.SelectedIndex = 0;
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left && listView1.SelectedItems.Count > 0)
            {
                if (!GetItem)
                    ShowSources.PerformClick();
                else ReturnItemDialog();
                
            }
        }
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
    
            if (listView1.SelectedItems.Count > 0)
            {

                if (e.KeyData==Keys .Enter)
                {
                    if (!GetItem)
                        ShowSources.PerformClick();
                    else ReturnItemDialog();

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

                        MenuItem[] mi1 = new MenuItem[] {ShowSources  ,OpenItem  };
                        MenuItemList.AddRange(mi1);

                }
                //////////////

                listView1.ContextMenu = new ContextMenu(MenuItemList.ToArray());

            }

        }
        private void Select_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].SubItems[1].Text == "مكان تخزين")
            {
                ReturnItemDialog();
            }
        }
        public void ReturnItemDialog()
        {
            _ReturnItem  = new ItemObj.ItemObjSQL.ItemSQL(DB).GetItemInfoByID((Convert.ToUInt32(listView1.SelectedItems[0].SubItems[0].Name)));
            if (_ReturnItem != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else MessageBox.Show("حصل خطأ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            ConfigureListViewColumns();
        }
        public async  void ConfigureListViewColumns()
        {
            listView1.Columns[0].Width = 125;
            listView1.Columns[1].Width = 150;
            listView1.Columns[2].Width = 150;
            listView1.Columns[3].Width = 150;
            listView1.Columns[4].Width = listView1.Width - 580;

        }
    }
}
