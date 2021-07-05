using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Threading;
using ItemProject.ItemObj.Objects;
using ItemProject.ItemObj.ItemObjSQL;
using ItemProject.Trade.Objects;
using ItemProject.Trade.TradeSQL;

namespace ItemProject.Trade.Forms.Container
{
    public partial class ShowLocations : Form
    {
        System.Windows.Forms.MenuItem CreateContainerMenuItem;
        System.Windows.Forms.MenuItem CreatePlaceMenuItem;

        System.Windows.Forms.MenuItem OpenContainerMenuItem;
        System.Windows.Forms.MenuItem EditContainerMenuItem;
        System.Windows.Forms.MenuItem DeleteContainerMenuItem;


        System.Windows.Forms.MenuItem OpenPlaceMenuItem;
        System.Windows.Forms.MenuItem EditPlaceMenuItem;
        System.Windows.Forms.MenuItem DeletePlaceMenuItem;

        System.Windows.Forms.MenuItem CutMenuItem;
        System.Windows.Forms.MenuItem PasteMenuItem;

        DatabaseInterface DB;
        TradeStoreContainer container;
        List<TradeStoreContainer> ContainersListView = new List<TradeStoreContainer>();
        List<TradeStorePlace> PlacesListView = new List<TradeStorePlace>();
        TradeStoreContainerSQL Containersql;
        TradeStorePlaceSQL placesql;
        Button front, end;
        int Path_startIndex = 0;
        Button[] b;
        List<TradeStoreContainer> Moved_ContainerList;
        List<TradeStorePlace> Moved_PlaceList ;

        //Folder Move_SourceFolder;
        bool GetPlace;
        private TradeStorePlace _ReturnPlace;
        public TradeStorePlace ReturnPlace
        {
            get { return _ReturnPlace; }
        }
        delegate void TreeviewVoidDelegate();
        public ShowLocations(DatabaseInterface db,TradeStoreContainer f,bool GetPlace_)
        {
            InitializeComponent();
            GetPlace = GetPlace_;
            if (GetPlace) Select.Visible = true;
            else Select.Visible = false;
            CreateContainerMenuItem   = new System.Windows.Forms.MenuItem("انشاء حاية جديدة", CreateContainer_MenuItem_Click);
            CreatePlaceMenuItem  = new System.Windows.Forms.MenuItem("انشاء مكان تخزين", CreatePlace_MenuItem_Click);
            OpenContainerMenuItem   = new System.Windows.Forms.MenuItem("استعراض", OpenContainer_MenuItem_Click); ;
            EditContainerMenuItem   = new System.Windows.Forms.MenuItem("تعديل", EditContainer_MenuItem_Click); ;
            DeleteContainerMenuItem   = new System.Windows.Forms.MenuItem("حذف", DeleteContainer_MenuItem_Click); ;
            OpenPlaceMenuItem  = new MenuItem("استعراض", OpenPlace_MenuItem_Click);
            EditPlaceMenuItem  = new MenuItem("تعديل", EditPlace_MenuItem_Click);
            DeletePlaceMenuItem  = new MenuItem("حذف", DeletePlace_MenuItem_Click);
            CutMenuItem = new MenuItem("قص", Cut_MenuItem_Click);
            PasteMenuItem = new MenuItem("لصق", Paste_MenuItem_Click);
            Moved_ContainerList = new List<TradeStoreContainer>();
            Moved_PlaceList = new List<TradeStorePlace>();
            DB = db;
            Containersql  = new TradeStoreContainerSQL (DB);
            placesql  = new TradeStorePlaceSQL (DB);
       
            container  = f;
            comboBoxFilterLocationPlace.SelectedIndex = 0;
            comboBoxFilterLocationPlace.SelectedIndexChanged += new EventHandler(comboBoxFilterItemFolder_SelectedIndexChanged);
            FillTreeViewContainer();
            OpenContainer();
           
            front = new Button();
             end = new Button();
            front.Font = new Font(front.Font.FontFamily, 6 );
            
            front.Size = new Size(25, PanelPath.Height);
            front.TextAlign = ContentAlignment.MiddleCenter;
            front.Text = ">>";
            front.BackColor = Color.SkyBlue;
            front.Location = new Point(0, 0);
            end.Size = new Size(25, PanelPath.Height);

            end.Text = "<<";
            end.Font = new Font(front.Font.FontFamily, 6);
            end.BackColor = Color.SkyBlue;
            front.Click += new EventHandler(Front_Click);
            end.Click += new EventHandler(End_Click);
            OptimizeDatagridVeiwSpec_Columns_Width();
            listView1.Focus();
        }
        public void OpenContainer()
        {
            comboBoxFilterLocationPlace.SelectedIndex = 0;
           
            textBoxSearch.Text = "";
            //Thread thread1, thread2;
            //thread1 = new Thread(new ThreadStart(RefreshTreeView));
            //thread1.Start();


            //thread2 = new Thread(new ThreadStart(FolderIDPath));
            //thread2.Start();

            ContainerIDPath();
            RefreshTreeView();
            ContainersListView = Containersql  .GetContainerChildsList (container );
            PlacesListView  = placesql .GetPlacesINContainer (container );
            RefreshList(ContainersListView,PlacesListView  );
          }       

        public void RefreshList(List <TradeStoreContainer  > containers,List <TradeStorePlace> places)
        {
            listView1.Items.Clear();
            if(comboBoxFilterLocationPlace .SelectedIndex !=2)
            {
    
                for (int i = 0; i < containers.Count; i++)
                {
                    ListViewItem item = new ListViewItem(containers[i].ContainerName );
                    item.Name = containers[i].ContainerID .ToString();
                    item.SubItems.Add("حاوية");
                    item.SubItems.Add(containers[i].Desc);
                    if (textBoxSearch.Text.Length > 0)
                        item.SubItems.Add(Containersql .GetContainerPath(containers[i]));
                    else
                        item.SubItems.Add(placesql  .GetPlacesINContainer(containers[i]).Count .ToString ()+" مكان تخزين , "+Containersql .GetContainerChildsList (containers[i]).Count .ToString ()+"  حاوية ");
                    item.ImageIndex = 0;
                    listView1.Items.Add(item);

                }
            }
            
            if(comboBoxFilterLocationPlace.SelectedIndex != 1)
            {

                for (int i = 0; i < places.Count; i++)
                {
                    ListViewItem item = new ListViewItem(places[i].PlaceName);
                    item.Name = places[i].PlaceID.ToString();
                    item.SubItems.Add("مكان تخزين");
                    item.SubItems.Add(places[i].Desc);

                    if (textBoxSearch.Text.Length > 0)
                        item.SubItems.Add(placesql  .GetPlacePath (places [i]));
                    else
                    {
                        item.SubItems.Add("عدد أنواع العناصر المخزنة :"+new TradeItemStoreSQL(DB).GetCountTypes_OF_AvailableItems(places[i].PlaceID ) .ToString ());
                    }
                    item.ImageIndex = 1;
                    listView1.Items.Add(item);

                }
               
            }





        }
        public void RefreshTreeView()
        {
            if (this.treeViewContainers .InvokeRequired)
            {
                TreeviewVoidDelegate d = new TreeviewVoidDelegate(RefreshTreeView);
                this.Invoke(d, new object[] {  });
            }
            else
            {
                string fid;
                if (container   == null) fid = "null";
                else fid = container .ContainerID  .ToString();
                TreeNode[] node = treeViewContainers.Nodes.Find(fid, true);
                if (node.Length == 0) return;
                node[0].Expand();
                treeViewContainers.SelectedNode = node[0];
            }
          
        }
        public void ContainerIDPath()
        {

            if (this.PanelPath .InvokeRequired)
            {
                TreeviewVoidDelegate d = new TreeviewVoidDelegate(ContainerIDPath);
                this.Invoke(d, new object[] { });
            }
            else
            {

                TradeStoreContainer  TempContainer = container;
                List<TradeStoreContainer> list = new List<TradeStoreContainer>();
                while (true)
                {

                    if (TempContainer != null) { list.Add(TempContainer); TempContainer = Containersql  .GetParentContainer (TempContainer); }
                    else { list.Add(TempContainer); break; }


                }
                Button[] b = new Button[list .Count ];
                for (int j = 0; j < list.Count; j++)
                {
                    int i = list.Count - j - 1;
                    b[i] = new Button();
                    b[i].Image = ImageListButton.Images[0];

                    b[i].AutoSize = true;
                    b[i].AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    b[i].AutoEllipsis = false;
                    b[i].Font = new Font(b[i].Font.FontFamily, 10);
                    b[i].FlatStyle = FlatStyle.Flat;
                    b[i].FlatAppearance.BorderSize = 0;
                    if (list[j] == null)
                    {
                        b[i].Name = "null";
                        b[i].Text = "ROOT";
                    }
                    else
                    {
                        b[i].Name = list[j].ContainerID  .ToString();
                        b[i].Text = list[j].ContainerName .ToString(); ;
                    }
                    b[i].TextImageRelation = TextImageRelation.ImageBeforeText;
                    b[i].Click += new EventHandler(Button_Path_Click);

                }
                PanelPath.Controls.Clear();
                int ButtonWidth = 0;
                for (int i = 0; i < b.Length; i++)
                {
                    ButtonWidth = ButtonWidth + b[i].Width;
                }
                if (ButtonWidth > PanelPath.Width)
                {
                    int availablewidth = 0;
                    PanelPath.Controls.Add(front);
                    availablewidth = PanelPath.Width - front.Width - end.Width;
                    int buton_x = front.Width;
                    Path_startIndex = b.Length - 1;
                    int wid = 0;
                    for (int j = b.Length - 1; j > 0; j--)
                    {
                        wid = wid + b[j].Width;
                        if (wid > availablewidth) { Path_startIndex = j + 1; break; }
                    }
                    int i = Path_startIndex;
                    while (i < b.Length)
                    {
                        b[i].Location = new Point(buton_x, PanelPath.Location.Y - 2);
                        PanelPath.Controls.Add(b[i]);
                        availablewidth = availablewidth - b[i].Width;
                        buton_x = buton_x + b[i].Width;
                        i++;
                    }
                }
                else
                {
                    int x1 = 0;
                    for (int i = 0; i < b.Length; i++)
                    {
                        b[i].Location = new Point(x1, PanelPath.Location.Y - 2);
                        PanelPath.Controls.Add(b[i]);
                        b[i].Show();
                        x1 = x1 + b[i].Width;
                    }

                }
            }
           

        }
        public void FillTreeViewContainer()
        {
            List<TradeStoreContainer > containersParents = new List<TradeStoreContainer>();
            containersParents = Containersql .GetContainerChildsList(null);
            treeViewContainers.Nodes.Clear();
            TreeNode r = new TreeNode("الجذر");
            r.Name = "null";
            r.ImageIndex = 0;
            treeViewContainers.Nodes.Add(r);
            while (containersParents.Count != 0)
            {
                List<TradeStoreContainer> containersChilds = new List<TradeStoreContainer>();
                for (int i = 0; i < containersParents.Count; i++)
                {
                    TreeNode n = new TreeNode(containersParents[i].ContainerName );
                    n.Name = containersParents[i].ContainerID .ToString();
                    n.ImageIndex = 0;
                    string parentid = "";
                    if (containersParents[i].ParentContainerID  == null)
                        parentid = "null";
                    else parentid = containersParents[i].ParentContainerID .ToString();
                    TreeNode[] nodes = treeViewContainers.Nodes.Find(parentid , true);
                    nodes[0].Nodes.Add(n);
                    containersChilds.AddRange(Containersql .GetContainerChildsList (containersParents[i]));

                }

                containersParents.Clear();
                containersParents = containersChilds;
            }

        }
        private void OptimizePath()
        {
            PanelPath.Controls.Clear();
            int buton_x = 0;
            int front_width = 0;
            if (Path_startIndex > 0)
            {
                PanelPath.Controls.Add(front);
                 buton_x = front.Width;
                front_width = front.Width;
            }
            int i = Path_startIndex;
            int availablewidth = PanelPath.Width - front_width - end.Width;
            
            while (i < b.Length)
            {

                b[i].Location = new Point(buton_x, PanelPath.Location.Y - 2);
                if (b[i].Width > availablewidth)
                {
                    end.Location = new Point(buton_x, 0);
                    PanelPath.Controls.Add(end);
                    break;
                }
                PanelPath.Controls.Add(b[i]);
                availablewidth = availablewidth - b[i].Width;
                buton_x = buton_x + b[i].Width;
                i++;
            }

        }
        private void Button_Path_Click(object sender, EventArgs e)
        {
           
            Button bb = (Button)sender;
            try
            {
                container =Containersql  .GetContainerBYID ( Convert.ToUInt32(bb.Name ));
            }
            catch
            {
                container  = null;
            }
            OpenContainer ();
        }
        private void End_Click(object sender, EventArgs e)
        {
            Path_startIndex = Path_startIndex + 1;
            OptimizePath();
        }
        private void Front_Click(object sender, EventArgs e)
        {
            if (Path_startIndex == 0) return;
            Path_startIndex = Path_startIndex - 1;
            OptimizePath();
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
                    if (item1.Bounds  .Contains(new Point(e.X, e.Y)))
                    {
                        match = true;
                        listitem = item1;
                        break;
                    }
                }
                if (match)
                {

                    if (listitem.SubItems [1].Text == "حاوية")
                    {
                        MenuItem[] mi1 = new MenuItem[] {OpenContainerMenuItem ,EditContainerMenuItem  ,DeleteContainerMenuItem  
                            ,new MenuItem ("-")  };
                        MenuItemList.AddRange(mi1);
                    }
                    else
                    {
                        MenuItem[] mi1 = new MenuItem[] {OpenPlaceMenuItem ,EditPlaceMenuItem  ,DeletePlaceMenuItem 
                            ,new MenuItem ("-") };
                        MenuItemList.AddRange(mi1);

                    }
                    MenuItemList.Add(CutMenuItem);

                }
                //////////////
                
                if (Moved_PlaceList.Count > 0 || Moved_ContainerList.Count > 0)
                {
                    MenuItemList.Add(PasteMenuItem);
                }
                MenuItem[] m_i = new MenuItem[] { new MenuItem("-") ,CreateContainerMenuItem,CreatePlaceMenuItem , new MenuItem("-") };
                MenuItemList.AddRange(m_i);
                if (container  == null)
                {
                    CreatePlaceMenuItem .Enabled = false;
                }
                else
                {
                    CreatePlaceMenuItem.Enabled = true;
                }
                listView1.ContextMenu = new ContextMenu(MenuItemList.ToArray ());

            }

        }
        #region ContextMenuItemEWvents
        private void CreateContainer_MenuItem_Click(object sender, EventArgs e)
        {

            List <TradeStoreContainer > containersInCurrentContainer = Containersql .GetContainerChildsList(container );
            string name=null;
            int j = 1;
            bool Exists = true;
            name = "مجلد جديد" + j;
            if (containersInCurrentContainer.Count > 0)
            {
                while (Exists)
                {
                    bool found = false;
                    for (int i = 0; i < containersInCurrentContainer.Count; i++)
                    {
                        if (containersInCurrentContainer[i].ContainerName  == name)
                            found = true;
                    }
                    if (found == true)
                    { j++; name = "مجلد جديد" + j; }
                    else Exists = false;
                }
            }
            uint? p_id;
            if (container  == null) p_id = null;
            else p_id =container .ContainerID  ;
            ContainerAddForm inp = new ContainerAddForm(this.DB, p_id,name,"");
            DialogResult dd= inp.ShowDialog();
            if(dd==DialogResult.OK )
            {
                
                FillTreeViewContainer ();
                OpenContainer ();

            }
            
        }
       
        private void OpenContainer_MenuItem_Click(object sender, EventArgs e)
        {

            string id_s = listView1.SelectedItems[0].Name;
            
            try
            {
                container  =Containersql .GetContainerBYID( Convert.ToUInt32(id_s));
                OpenContainer ();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        private void EditContainer_MenuItem_Click(object sender, EventArgs e)
        {
            TradeStoreContainer  trmpcontainer = Containersql.GetContainerBYID (Convert .ToUInt32  (listView1.SelectedItems[0].Name));
            ContainerAddForm inp = new ContainerAddForm(this.DB, trmpcontainer);
            DialogResult dd = inp.ShowDialog();
            if (dd == DialogResult.OK)
            {

                FillTreeViewContainer ();
                OpenContainer ();

            }

        }
        private void DeleteContainer_MenuItem_Click(object sender, EventArgs e)
        {
            TradeStoreContainer trmpcontainer = Containersql .GetContainerBYID (Convert.ToUInt32(listView1.SelectedItems[0].Name));
            DialogResult d = MessageBox.Show("لا يمكن حذف المجلد في حال لم يكن فارغ,هل انت متأكد من حذف المجلد؟", "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (d==DialogResult.OK)
                if(Containersql.DeleteContainer (trmpcontainer.ContainerID))
                {
                    FillTreeViewContainer();
                    OpenContainer();
                }

        }
        private void Paste_MenuItem_Click(object sender, EventArgs e)
        {
            //if(folder==null && Moved_ItemList.Count >0)
            //{
            //    MessageBox.Show("لا يمكن نقل العناصر للمجلد الجذر","خطأ",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //    return;
            //}
            // foldersql.MoveFolders(folder, Moved_FolderList).ToString ();
            //itemsql.MoveItems(folder, Moved_ItemList);
            //FillTreeViewFolder();
            //OpenFolder();
            
        }
        private void Cut_MenuItem_Click(object sender, EventArgs e)
        {

            //Moved_FolderList.Clear();
            //Moved_ItemList.Clear();
            //Move_SourceFolder = folder;
            //for(int i=0;i<listView1 .SelectedItems .Count;i++)
            //{
            //    if (listView1.SelectedItems[i].SubItems[2].Text == "مجلد")
            //    {
            //        Moved_FolderList.Add(foldersql.GetFolderInfoByID(Convert .ToUInt32 (listView1.SelectedItems[i].Name )));
            //    }
            //    else
            //    {
            //        Moved_ItemList.Add(itemsql .GetItemInfoByID(Convert.ToUInt32(listView1.SelectedItems[i].Name)));
            //    }
            //}
            //if(Moved_ItemList.Count >0)
            //{
            //    DialogResult d = MessageBox.Show("نقل العناصر سيؤدي الى حذف جميع قيم الخصائص المضبوطة للعناصر المنقولة, هل انت متاكد من الاستمرار بلعملية؟", "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //    if(d!=DialogResult.OK)
            //    {
            //        Moved_FolderList.Clear();
            //        Moved_ItemList.Clear();
            //        return;
            //    }
            //}

        }
        private void CreatePlace_MenuItem_Click(object sender, EventArgs e)
        {
            AddPlaceForm addplaceform = new AddPlaceForm(DB, container);
            DialogResult d = addplaceform.ShowDialog();
            if (d == DialogResult.OK)
            {
                //ItemForm itemform = new ItemForm(DB, itemsql.GetItemInfoByName(itemadd.textBoxItemName.Text, itemadd.textBoxCompanyName.Text));
                //itemform.ShowDialog();
                PlacesListView = placesql.GetPlacesINContainer(container);
                RefreshList(ContainersListView, PlacesListView);
            }
        }
        private void OpenPlace_MenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string id_s1 = listView1.SelectedItems[0].Name;
                TradeStorePlace place = placesql.GetTradeStorePlaceBYID(Convert.ToUInt32(id_s1));
                PlaceItemsForm placeform = new PlaceItemsForm(DB, place,false );
                placeform.ShowDialog();
            }
        }
        private void EditPlace_MenuItem_Click(object sender, EventArgs e)
        {
            string id_s1 = listView1.SelectedItems[0].Name;
            TradeStorePlace  place = placesql .GetTradeStorePlaceBYID(Convert.ToUInt32(id_s1));
            AddPlaceForm  editplaceform = new AddPlaceForm(DB, container ,place );
            DialogResult d = editplaceform.ShowDialog();
            if (d == DialogResult.OK)
            {
                //ItemForm itemform = new ItemForm(DB, itemsql.GetItemInfoByName(itemadd.textBoxItemName.Text, itemadd.textBoxCompanyName.Text));
                //itemform.ShowDialog();
                PlacesListView  = placesql .GetPlacesINContainer(container );
                RefreshList (ContainersListView , PlacesListView );
            }
        }
        private void DeletePlace_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show(" هل أنت متاكد من اتمام عملية الحذف؟  ", "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (d != DialogResult.OK) return;
            string id_s1 = listView1.SelectedItems[0].Name;
            TradeStorePlace  place = placesql .GetTradeStorePlaceBYID(Convert.ToUInt32(id_s1));
            bool success = placesql.DeletePlace (place.PlaceID );
            if (success)
            {
                PlacesListView = placesql.GetPlacesINContainer(container);
                RefreshList(ContainersListView, PlacesListView);
            }
        }

        #endregion
   

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            if(e.Button==MouseButtons.Left && listView1.SelectedItems.Count >0 )
            {
                
                switch (listView1.SelectedItems[0].SubItems[1].Text)
                {
                    case "حاوية":
                        string id_s = listView1.SelectedItems[0].Name;
                        try
                        {
                            container  = Containersql .GetContainerBYID (Convert.ToUInt32(id_s));
                            OpenContainer ();
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }
                        break;
                    case "مكان تخزين":
                        try
                        {
                            string id_s1 = listView1.SelectedItems[0].Name;
                            TradeStorePlace  place = placesql .GetTradeStorePlaceBYID(Convert.ToUInt32(id_s1));
                            if(!GetPlace )
                            {
                                PlaceItemsForm placeform = new PlaceItemsForm(DB, place,false );
                                placeform.ShowDialog();
                             }
                            else
                            {
                                _ReturnPlace = place;
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }

                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }

                        break;
                }

            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if (container   == null) return;
            container  = Containersql .GetParentContainer (container );
            OpenContainer ();
        }



        private void comboBoxFilterItemFolder_SelectedIndexChanged(object sender, EventArgs e)
        {

            RefreshList (ContainersListView ,PlacesListView );
        }



        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Length > 0)
            {
                splitContainer1.Panel2Collapsed = true;

                ContainersListView = Containersql .SearchContainer(textBoxSearch.Text);
                PlacesListView = placesql .SearchPlace(textBoxSearch.Text);
                RefreshList(ContainersListView, PlacesListView);
            }
            else OpenContainer ();
        }


  

        //private void dataGridViewSpec_Resize(object sender, EventArgs e)
        //{
        //    OptimizeDatagridVeiwSpec_Columns_Width();
        //}
        public void OptimizeDatagridVeiwSpec_Columns_Width()
        {
            //dataGridViewSpec.RowHeadersWidth = dataGridViewSpec.Width / 2;
            //dataGridViewSpec.Columns[0].Width = dataGridViewSpec.Width / 2;
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData ==Keys.Back )
            {
                if (container  == null) return;
                container = Containersql.GetParentContainer(container);
                OpenContainer ();
                return;
            }
            if(listView1 .SelectedItems .Count >0 )
            {
                
                switch (e.KeyData )
                {
                    case Keys.Enter:
                        switch (listView1.SelectedItems[0].SubItems[1].Text)
                        {
                            case "حاوية":
                                string id_s = listView1.SelectedItems[0].Name;
                                try
                                {
                                    container = Containersql .GetContainerBYID(Convert.ToUInt32(id_s));
                                    OpenContainer ();
                                }
                                catch (Exception ee)
                                {
                                    MessageBox.Show(ee.Message);
                                }
                                break;
                            case "مكان تخزين":
                                if (!GetPlace)
                                {
                                    string id_s1 = listView1.SelectedItems[0].Name;
                                    TradeStorePlace place = placesql.GetTradeStorePlaceBYID(Convert.ToUInt32(id_s1));
                                    PlaceItemsForm placeform = new PlaceItemsForm(DB, place, false);
                                    placeform.ShowDialog();
                                }
                                else
                                    ReturPlace();
                                break;
                        }
                        break;
                    case Keys.Delete:

                        switch (listView1.SelectedItems[0].SubItems[1].Text)
                        {
                            case "حاوية":
                                DeleteContainerMenuItem.PerformClick();
                                break;
                            case "مكان تخزين":
                                DeletePlaceMenuItem.PerformClick();
                                break;
                        }
                        break;
   


                }

            }
        }

        private void Select_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].SubItems[1].Text == "مكان تخزين")
            {
                ReturPlace();
            }
        }
        public void ReturPlace()
        {
            _ReturnPlace  = new TradeStorePlaceSQL(DB ).GetTradeStorePlaceBYID((Convert.ToUInt32(listView1.SelectedItems[0].SubItems[0].Name)));
            if (_ReturnPlace != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else MessageBox.Show("حصل خطأ","",MessageBoxButtons.OK ,MessageBoxIcon.Error);
          
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeViewFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            if (e.Button == MouseButtons.Left )
            {
                if (treeViewContainers.SelectedNode != null)
                {
                    try
                    {
                        container = Containersql.GetContainerBYID(Convert.ToUInt32(treeViewContainers.SelectedNode.Name));
                    }
                    catch
                    {
                        container = null;
                    }

                    OpenContainer ();
                }
          
            }
        }
    }
  
}
