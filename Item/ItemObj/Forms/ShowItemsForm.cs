﻿using System;
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

namespace ItemProject.ItemObj.Forms
{
    public partial class ShowItemsForm : Form
    {
        System.Windows.Forms.MenuItem CreateFolderMenuItem;
        System.Windows.Forms.MenuItem CreateItemMenuItem;

        System.Windows.Forms.MenuItem OpenFolderMenuItem;
        System.Windows.Forms.MenuItem OpenFolderPageMenuItem;
        System.Windows.Forms.MenuItem EditFolderMenuItem;
        System.Windows.Forms.MenuItem DeleteFolderMenuItem;
        //System.Windows.Forms.MenuItem AddFolderSpec;

        System.Windows.Forms.MenuItem OpenItemMenuItem;
        System.Windows.Forms.MenuItem EditItemMenuItem;
        System.Windows.Forms.MenuItem DeleteItemMenuItem;

        System.Windows.Forms.MenuItem CutMenuItem;
        System.Windows.Forms.MenuItem PasteMenuItem;


        System.Windows.Forms.MenuItem SpecFilter;


        DatabaseInterface DB;
        Folder folder;
        List<Folder> FoldersListView = new List<Folder>();
        List<Item> ItemsListView = new List<Item>();
        FolderSQL foldersql;
        ItemSQL itemsql;
        Button front, end;
        int Path_startIndex = 0;
        Button[] b;
        ComboBox[] ComboboxSpec_Value;
        TextBox[] TextBoxSpec_Value;
        List<Folder> Moved_FolderList;
        List<Item> Moved_ItemList;

        Folder Move_SourceFolder;


        delegate void TreeviewVoidDelegate();
        public ShowItemsForm(DatabaseInterface db, Folder f)
        {
            InitializeComponent();

            CreateFolderMenuItem = new System.Windows.Forms.MenuItem("انشاء صنف", CreateFolder_MenuItem_Click);
            CreateItemMenuItem = new System.Windows.Forms.MenuItem("انشاء عنصر", CreateItem_MenuItem_Click);
            OpenFolderMenuItem = new System.Windows.Forms.MenuItem("فتح", OpenFolder_MenuItem_Click); ;
            OpenFolderPageMenuItem = new System.Windows.Forms.MenuItem("استعراض بيانات الصنف", OpenFolder_Page_MenuItem_Click); ;

            EditFolderMenuItem = new System.Windows.Forms.MenuItem("تعديل", EditFolder_MenuItem_Click); ;
            DeleteFolderMenuItem = new System.Windows.Forms.MenuItem("حذف", DeleteFolder_MenuItem_Click); ;
            //AddFolderSpec = new System.Windows.Forms.MenuItem("الخصائص المشتركة لعناصر الصنف", AddFolderSpec_MenuItem_Click);
            OpenItemMenuItem = new MenuItem("فتح صفحة العنصر", OpenItem_MenuItem_Click);
            EditItemMenuItem = new MenuItem("تعديل العنصر", EditItem_MenuItem_Click);
            DeleteItemMenuItem = new MenuItem("حذف العنصر", DeleteItem_MenuItem_Click);
            SpecFilter = new MenuItem("فلترة حسب الخصائص", SpecFilter_MenuItem_Click);
            CutMenuItem = new MenuItem("قص", Cut_MenuItem_Click);
            PasteMenuItem = new MenuItem("لصق", Paste_MenuItem_Click);
            Moved_FolderList = new List<Folder>();
            Moved_ItemList = new List<Item>();
            DB = db;
            foldersql = new FolderSQL(DB);
            itemsql = new ItemSQL(DB);

            folder = f;

            comboBox1.SelectedIndex = 0;
            comboBoxFilterItemFolder.SelectedIndex = 0;
            comboBoxFilterItemFolder.SelectedIndexChanged += new EventHandler(comboBoxFilterItemFolder_SelectedIndexChanged);
            FillTreeViewFolder();
            OpenFolder();

            front = new Button();
            end = new Button();
            front.Font = new Font(front.Font.FontFamily, 6);

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

        }
        public async  void OpenFolder()
        {
            comboBoxFilterItemFolder.SelectedIndex = 0;

            textBoxSearch.Text = "";
            //Thread thread1, thread2;
            //thread1 = new Thread(new ThreadStart(RefreshTreeView));
            //thread1.Start();


            //thread2 = new Thread(new ThreadStart(FolderIDPath));
            //thread2.Start();

            FolderIDPath();
            RefreshTreeView();
            FoldersListView = foldersql.GetFolderChilds(folder);
            ItemsListView = itemsql.GetItemsInFolder(folder);
            FillComboBoxComapines(ItemsListView);
            RefreshItems(FoldersListView, ItemsListView);
            if (splitContainer1.Panel2Collapsed == false)
                if (folder == null)
                    splitContainer1.Panel2Collapsed = true;
                else FillFolderSpec();

        }
        public async  void FillFolderSpec()
        {

            splitContainer1.Panel2.Controls.Clear();
            if (folder == null) splitContainer1.Panel2Collapsed = true;
            List<ItemSpecDisplay> ItemSpecDisplayList = new List<ItemSpecDisplay>();
            List<ItemSpec> ItemSpecList_ = new ItemSpecSQL(DB).GetItemSpecList(folder);
            List<ItemSpec_Restrict> ItemSpec_Restrict_List_ = new ItemSpec_Restrict_SQL(DB).GetItemSpecRestrictList(folder);
            ItemSpec_Restrict_Options_SQL ItemSpec_Restrict_Options_SQL = new ItemSpec_Restrict_Options_SQL(DB);
            for (int i = 0; i < ItemSpec_Restrict_List_.Count; i++)
            {
                ItemSpecDisplayList.Add(new ItemSpecDisplay(folder, ItemSpec_Restrict_List_[i].SpecID, ItemSpec_Restrict_List_[i].SpecName, ItemSpec_Restrict_List_[i].SpecIndex, false));
            }
            for (int i = 0; i < ItemSpecList_.Count; i++)
            {
                ItemSpecDisplayList.Add(new ItemSpecDisplay(folder, ItemSpecList_[i].SpecID, ItemSpecList_[i].SpecName, ItemSpecList_[i].SpecIndex, true));
            }
            ComboboxSpec_Value = new ComboBox[ItemSpec_Restrict_List_.Count];
            TextBoxSpec_Value = new TextBox[ItemSpecList_.Count];
            ItemSpecDisplayList = ItemSpecDisplayList.OrderBy(m => m.SpecIndex).ToList();


            if (ItemSpecDisplayList.Count == 0)
            {
                MessageBox.Show("لا توجد خصائص مدخلة لهذا المجلد", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                splitContainer1.Panel2Collapsed = true;
                return;
            }
            Label LabelPanelTitle = new Label();
            LabelPanelTitle.BackColor = Color.LightGreen;
            LabelPanelTitle.BorderStyle = BorderStyle.FixedSingle;
            LabelPanelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
            LabelPanelTitle.AutoSize = false;
            LabelPanelTitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LabelPanelTitle.Location = new System.Drawing.Point(0, 0);
            //this.label2.Name = "label2";
            LabelPanelTitle.Size = new System.Drawing.Size(splitContainer1.Panel2.Width, 20);
            LabelPanelTitle.TextAlign = ContentAlignment.MiddleLeft;
            LabelPanelTitle.Text = "فلترة العناصر حسب الخصائص";


            Button ButonSpecFilterClose = new Button();
            ButonSpecFilterClose.Location = new System.Drawing.Point(splitContainer1.Panel2.Width / 2 - 40, splitContainer1.Panel2.Height - 40);
            ButonSpecFilterClose.Size = new System.Drawing.Size(57, 29);
            ButonSpecFilterClose.Click += new EventHandler(ButonSpecFilterClose_Click);
            ButonSpecFilterClose.Text = "اغلاق";
            ButonSpecFilterClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom)));
            Panel panelFolderSpecs = new Panel();
            panelFolderSpecs.BorderStyle = BorderStyle.FixedSingle;
            panelFolderSpecs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom)));
            panelFolderSpecs.AutoScroll = true;
            panelFolderSpecs.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            panelFolderSpecs.Location = new System.Drawing.Point(5, 30);
            //this.label2.Name = "label2";
            panelFolderSpecs.Size = new System.Drawing.Size(splitContainer1.Panel2.Width - 10, splitContainer1.Panel2.Height - 75);
            panelFolderSpecs.Controls.Clear();



            Label[] label = new Label[ItemSpecDisplayList.Count];

            int Start_Paint_Position = 10;
            int h = 10;
            int w = panelFolderSpecs.Width - 40;
            int TextBoxSpec_Value_Index = 0;
            int ComboboxSpec_Value_Index = 0;
            for (int i = 0; i < ItemSpecDisplayList.Count; i++)
            {
                label[i] = new Label();
                label[i].BackColor = Color.Aquamarine;
                label[i].BorderStyle = BorderStyle.FixedSingle;
                label[i].Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
                label[i].AutoSize = false;
                label[i].Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label[i].Location = new System.Drawing.Point(Start_Paint_Position, h);
                //this.label2.Name = "label2";
                label[i].Size = new System.Drawing.Size(w, 20);
                label[i].TextAlign = ContentAlignment.MiddleLeft;
                label[i].TabIndex = 15;
                label[i].Text = ItemSpecDisplayList[i].SpecName;
                panelFolderSpecs.Controls.Add(label[i]);
                if (ItemSpecDisplayList[i].Spectype == false)
                {
                    ComboboxSpec_Value[ComboboxSpec_Value_Index] = new ComboBox();
                    ComboboxSpec_Value[ComboboxSpec_Value_Index].Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
                    ComboboxSpec_Value[ComboboxSpec_Value_Index].AutoSize = false;
                    ComboboxSpec_Value[ComboboxSpec_Value_Index].Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (h + 25 > panelFolderSpecs.Height)
                        w = panelFolderSpecs.Width - 40;// - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;

                    ComboboxSpec_Value[ComboboxSpec_Value_Index].Location = new System.Drawing.Point(Start_Paint_Position, h + 25);
                    ComboboxSpec_Value[ComboboxSpec_Value_Index].Name = ItemSpecDisplayList[i].SpecID.ToString();
                    ComboboxSpec_Value[ComboboxSpec_Value_Index].BackColor = Color.White;
                    ComboboxSpec_Value[ComboboxSpec_Value_Index].Size = new System.Drawing.Size(w, 20);
                    ComboboxSpec_Value[ComboboxSpec_Value_Index].DropDownStyle = ComboBoxStyle.DropDownList;
                    List<ItemSpec_Restrict_Options> ItemSpec_Restrict_Options_ = new List<ItemSpec_Restrict_Options>();
                    ItemSpec_Restrict ItemSpec_Restrict_ = new ItemSpec_Restrict(folder, ItemSpecDisplayList[i].SpecID, ItemSpecDisplayList[i].SpecName, ItemSpecDisplayList[i].SpecIndex);
                    ItemSpec_Restrict_Options_ = new ItemSpec_Restrict_Options_SQL(DB).GetItemSpec_Restrict_Options_List(ItemSpec_Restrict_);
                    Dictionary<uint, string> combosource = new Dictionary<uint, string>();
                    if (ItemSpec_Restrict_Options_.Count > 0)
                    {
                        combosource.Add(0, "غير محدد");
                        for (int k = 0; k < ItemSpec_Restrict_Options_.Count; k++)
                        {
                            combosource.Add(ItemSpec_Restrict_Options_[k].OptionID, ItemSpec_Restrict_Options_[k].OptionName);
                        }

                        ComboboxSpec_Value[ComboboxSpec_Value_Index].DataSource = new BindingSource(combosource, null);
                        ComboboxSpec_Value[ComboboxSpec_Value_Index].DisplayMember = "Value";
                        ComboboxSpec_Value[ComboboxSpec_Value_Index].ValueMember = "Key";
                        ComboboxSpec_Value[ComboboxSpec_Value_Index].SelectedIndexChanged += new System.EventHandler(this.comboBox_ItemSpec_Value_SelectedIndexChanged);

                    }
                    else
                    {
                        combosource.Add(0, " لم يتم ادخال قيم");
                        ComboboxSpec_Value[ComboboxSpec_Value_Index].DataSource = new BindingSource(combosource, null);
                        ComboboxSpec_Value[ComboboxSpec_Value_Index].DisplayMember = "Value";
                        ComboboxSpec_Value[ComboboxSpec_Value_Index].ValueMember = "Key";
                    }

                    panelFolderSpecs.Controls.Add(ComboboxSpec_Value[ComboboxSpec_Value_Index]);
                    h = h + 60;
                    if (h > panelFolderSpecs.Height)
                        w = panelFolderSpecs.Width - 40; //- System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
                    ComboboxSpec_Value_Index += 1;
                }
                else
                {
                    TextBoxSpec_Value[TextBoxSpec_Value_Index] = new TextBox();
                    TextBoxSpec_Value[TextBoxSpec_Value_Index].Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
                    TextBoxSpec_Value[TextBoxSpec_Value_Index].AutoSize = false;
                    TextBoxSpec_Value[TextBoxSpec_Value_Index].Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (h + 25 > panelFolderSpecs.Height)
                        w = panelFolderSpecs.Width - 40;
                    TextBoxSpec_Value[TextBoxSpec_Value_Index].Location = new System.Drawing.Point(Start_Paint_Position, h + 25);
                    TextBoxSpec_Value[TextBoxSpec_Value_Index].Name = ItemSpecDisplayList[i].SpecID.ToString();
                    TextBoxSpec_Value[TextBoxSpec_Value_Index].Size = new System.Drawing.Size(w, 20);
                    TextBoxSpec_Value[TextBoxSpec_Value_Index].TextChanged += new EventHandler(textBox_ItemSpec_Value_TextChanged);
                    panelFolderSpecs.Controls.Add(TextBoxSpec_Value[TextBoxSpec_Value_Index]);
                    h = h + 60;
                    if (h > panelFolderSpecs.Height)
                        w = panelFolderSpecs.Width - 40;
                    TextBoxSpec_Value_Index += 1;
                }

            }


            //Label[] label_ = new Label[ItemSpec_List.Count];
            //TextBoxSpec_Value = new TextBox[ItemSpec_List.Count];
            //for (int i = 0; i < ItemSpec_List.Count; i++)
            //{
            //    label_[i] = new Label();
            //    label_[i].BackColor = Color.Aquamarine;
            //    label_[i].BorderStyle = BorderStyle.FixedSingle;
            //    label_[i].Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left)));
            //    label_[i].AutoSize = false;
            //    label_[i].Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //    label_[i].Location = new System.Drawing.Point(Start_Paint_Position, h);
            //    //this.label2.Name = "label2";
            //    label_[i].Size = new System.Drawing.Size(w, 20);
            //    label_[i].TextAlign = ContentAlignment.MiddleLeft;
            //    label_[i].Text = ItemSpec_List[i].SpecName;
            //    panelFolderSpecs.Controls.Add(label_[i]);

            //}
            Label padding = new Label();
            padding.Location = new System.Drawing.Point(Start_Paint_Position, h);
            //this.label2.Name = "label2";
            padding.Size = new System.Drawing.Size(w, 20);
            panelFolderSpecs.Controls.Add(padding);
            splitContainer1.Panel2.Controls.Add(LabelPanelTitle);
            splitContainer1.Panel2.Controls.Add(panelFolderSpecs);
            splitContainer1.Panel2.Controls.Add(ButonSpecFilterClose);

        }
        private void comboBox_ItemSpec_Value_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterItemsBySpec();
        }
        private void textBox_ItemSpec_Value_TextChanged(object sender, EventArgs e)
        {
            FilterItemsBySpec();
        }
        public async void FilterItemsBySpec()
        {
            ItemSpec_Restrict_Options_SQL ItemSpec_Restrict_Options_SQL_ = new ItemSpec_Restrict_Options_SQL(DB);
            List<ItemSpec_Restrict_Options> ItemSpec_Restrict_Options_List = new List<ItemSpec_Restrict_Options>();
            List<ItemSpec_Value> ItemSpec_Value_List = new List<ItemSpec_Value>();

            for (int i = 0; i < ComboboxSpec_Value.Length; i++)
            {
                if (ComboboxSpec_Value[i].SelectedIndex >= 1)
                {
                    ItemSpec_Restrict ItemSpec_Restrict_ = new ItemSpec_Restrict_SQL(DB).GetItemSpecRestrictInfoByID(Convert.ToUInt32(ComboboxSpec_Value[i].Name));
                    ItemSpec_Restrict_Options_List.Add(new ItemSpec_Restrict_Options(ItemSpec_Restrict_, ((KeyValuePair<uint, string>)ComboboxSpec_Value[i].SelectedItem).Key, ((KeyValuePair<uint, string>)ComboboxSpec_Value[i].SelectedItem).Value));

                }
            }
            for (int i = 0; i < TextBoxSpec_Value.Length; i++)
            {
                if (TextBoxSpec_Value[i].Text.Length > 0)
                {
                    ItemSpec ItemSpec_ = new ItemSpecSQL(DB).GetItemSpecInfoByID(Convert.ToUInt32(TextBoxSpec_Value[i].Name));
                    ItemSpec_Value_List.Add(new ItemSpec_Value(null, ItemSpec_, TextBoxSpec_Value[i].Text));

                }
            }
            if (ItemSpec_Restrict_Options_List.Count == 0 && ItemSpec_Value_List.Count == 0)
            { OpenFolder();
                return;
            }
            List<Item> FilteredItems = new ItemSQL(DB).FilterItemsBySpec(ItemSpec_Restrict_Options_List, ItemSpec_Value_List);
            RefreshItems(new List<Folder>(), FilteredItems);

        }
        public async void FillComboBoxComapines(List<Item> items)
        {

            comboBoxCompanies.Items.Clear();
            List<string> companies = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                int ind = companies.IndexOf(items[i].ItemCompany.ToString());
                if (ind < 0) companies.Add(items[i].ItemCompany.ToString());

            }
            if (companies.Count > 0)
            {

                comboBoxCompanies.SelectedIndexChanged -= new EventHandler(comboBoxCompanies_SelectedIndexChanged);
                comboBoxCompanies.Enabled = true;
                comboBoxCompanies.Items.Add("الكل");
                comboBoxCompanies.Items.AddRange(companies.ToArray());
                comboBoxCompanies.SelectedIndex = 0;
                comboBoxCompanies.SelectedIndexChanged += new EventHandler(comboBoxCompanies_SelectedIndexChanged);

            }
            else comboBoxCompanies.Enabled = false;

        }
        public async void RefreshItems(List<Folder> folders, List<Item> items)
        {
            listView1.Items.Clear();
            if (comboBoxFilterItemFolder.SelectedIndex != 2)
            {

                for (int i = 0; i < folders.Count; i++)
                {
                    ListViewItem item = new ListViewItem(folders[i].FolderName);
                    item.Name = folders[i].FolderID.ToString();
                    item.SubItems.Add("");
                    item.SubItems.Add("مجلد");
                    item.SubItems.Add(folders[i].CreateDate.ToString());
                    if (textBoxSearch.Text.Length > 0)
                        item.SubItems.Add(foldersql.GetFolderPath(folders[i]));
                    else
                       item.SubItems.Add(itemsql.GetItemsInFolder(folders[i]).Count.ToString() + " عنصر , " + foldersql.GetFolderChilds(folders[i]).Count.ToString() + "  مجلد ");
                    item.SubItems.Add(folders[i].DefaultConsumeUnit );
                    item.ImageIndex = 1;
                    listView1.Items.Add(item);

                }
            }

            if (comboBoxFilterItemFolder.SelectedIndex != 1)
            {

                List<string> companies = new List<string>();
                for (int i = 0; i < items.Count; i++)
                {
                    ListViewItem item = new ListViewItem(items[i].ItemName);
                    item.Name = items[i].ItemID.ToString();
                    item.SubItems.Add(items[i].ItemCompany.ToString());
                    item.SubItems.Add("عنصر");
                    item.SubItems.Add(items[i].CreateDate.ToString());
                    if (textBoxSearch.Text.Length > 0)
                        item.SubItems.Add(itemsql.GetItemPath(items[i]));
                    else
                    {

                        item.SubItems.Add(items[i].MarketCode);
                        item.SubItems.Add(items[i].DefaultConsumeUnit);
                    }
                    item.ImageIndex = 0;
                    if (comboBoxCompanies.SelectedIndex > 0 && items[i].ItemCompany.ToString() != comboBoxCompanies.SelectedItem.ToString()) continue;
                    listView1.Items.Add(item);

                }

            }
            if (comboBoxFilterItemFolder.SelectedIndex == 1) comboBoxCompanies.Enabled = false;
            else
            {
                if (comboBoxCompanies.Items.Count > 0)
                { comboBoxCompanies.Enabled = true;
                    //comboBoxCompanies.SelectedIndex = 0;
                }
                else comboBoxCompanies.Enabled = false;
            }





        }
        public async void RefreshTreeView()
        {
            if (this.treeViewFolders.InvokeRequired)
            {
                TreeviewVoidDelegate d = new TreeviewVoidDelegate(RefreshTreeView);
                this.Invoke(d, new object[] { });
            }
            else
            {
                string fid;
                if (folder == null) fid = "null";
                else fid = folder.FolderID.ToString();
                TreeNode[] node = treeViewFolders.Nodes.Find(fid, true);
                if (node.Length == 0) return;
                node[0].Expand();
                treeViewFolders.SelectedNode = node[0];
            }

        }
        public async void FolderIDPath()
        {

            if (this.PanelPath.InvokeRequired)
            {
                TreeviewVoidDelegate d = new TreeviewVoidDelegate(FolderIDPath);
                this.Invoke(d, new object[] { });
            }
            else
            {

                Folder TempFolder = folder;
                List<Folder> list = new List<Folder>();
                while (true)
                {

                    if (TempFolder != null) { list.Add(TempFolder); TempFolder = foldersql.GetParentFolder(TempFolder); }
                    else { list.Add(TempFolder); break; }


                }
                Button[] b = new Button[list.Count];
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
                        b[i].Name = list[j].FolderID.ToString();
                        b[i].Text = list[j].FolderName.ToString(); ;
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
        public async void FillTreeViewFolder()
        {
            List<Folder> FoldersParents = new List<Folder>();

            FoldersParents = foldersql.GetFolderChilds(null);
            treeViewFolders.Nodes.Clear();
            TreeNode r = new TreeNode("الجذر");
            r.Name = "null";
            r.ImageIndex = 0;
            treeViewFolders.Nodes.Add(r);
            while (FoldersParents.Count != 0)
            {
                List<Folder> FoldersChilds = new List<Folder>();
                for (int i = 0; i < FoldersParents.Count; i++)
                {
                    TreeNode n = new TreeNode(FoldersParents[i].FolderName);
                    n.Name = FoldersParents[i].FolderID.ToString();
                    n.ImageIndex = 0;
                    string parentid = "";
                    if (FoldersParents[i].ParentFolderID == null)
                        parentid = "null";
                    else parentid = FoldersParents[i].ParentFolderID.ToString();
                    TreeNode[] nodes = treeViewFolders.Nodes.Find(parentid, true);
                    nodes[0].Nodes.Add(n);
                    FoldersChilds.AddRange(foldersql.GetFolderChilds(FoldersParents[i]));

                }

                FoldersParents.Clear();
                FoldersParents = FoldersChilds;
            }

        }
        private async void OptimizePath()
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
                folder = foldersql.GetFolderInfoByID(Convert.ToUInt32(bb.Name));
            }
            catch
            {
                folder = null;
            }
            OpenFolder();
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
                    if (item1.Bounds.Contains(new Point(e.X, e.Y)))
                    {
                        match = true;
                        listitem = item1;
                        break;
                    }
                }
                if (match)
                {

                    if (listitem.SubItems[2].Text == "مجلد")
                    {
                        MenuItem[] mi1 = new MenuItem[] {OpenFolderMenuItem,OpenFolderPageMenuItem ,EditFolderMenuItem ,DeleteFolderMenuItem /*,AddFolderSpec*/
                            ,new MenuItem ("-")  };
                        MenuItemList.AddRange(mi1);
                    }
                    else
                    {
                        MenuItem[] mi1 = new MenuItem[] {OpenItemMenuItem,EditItemMenuItem ,DeleteItemMenuItem
                            ,new MenuItem ("-") };
                        MenuItemList.AddRange(mi1);

                    }
                    MenuItemList.Add(CutMenuItem);

                }
                //////////////

                if (Moved_ItemList.Count > 0 || Moved_FolderList.Count > 0)
                {
                    MenuItemList.Add(PasteMenuItem);
                }
                MenuItem[] m_i = new MenuItem[] { new MenuItem("-"), CreateFolderMenuItem, CreateItemMenuItem, new MenuItem("-"), SpecFilter };
                MenuItemList.AddRange(m_i);
                if (folder == null)
                {
                    CreateItemMenuItem.Enabled = false;
                    SpecFilter.Enabled = false;
                }
                else
                {
                    CreateItemMenuItem.Enabled = true;
                    SpecFilter.Enabled = true;
                }
                //if(textBoxSearch .Text .Length >0)
                //{
                //    CreateFolderMenuItem.Enabled = false;
                //    CreateItemMenuItem.Enabled = false;
                //    SpecFilter.Enabled = false;
                //}
                listView1.ContextMenu = new ContextMenu(MenuItemList.ToArray());

            }

        }
        #region ContextMenuItemEWvents
        private void CreateFolder_MenuItem_Click(object sender, EventArgs e)
        {

            List<Folder> FoldersInCurrentFolder = foldersql.GetFolderChilds(folder);
            string name = null;
            int j = 1;
            bool Exists = true;
            name = "مجلد جديد" + j;
            if (FoldersInCurrentFolder.Count > 0)
            {
                while (Exists)
                {
                    bool found = false;
                    for (int i = 0; i < FoldersInCurrentFolder.Count; i++)
                    {
                        if (FoldersInCurrentFolder[i].FolderName == name)
                            found = true;
                    }
                    if (found == true)
                    { j++; name = "مجلد جديد" + j; }
                    else Exists = false;
                }
            }
            uint? p_id;
            if (folder == null) p_id = null;
            else p_id = folder.FolderID;
            FolderForm inp = new FolderForm(this.DB, p_id, name);
            DialogResult dd = inp.ShowDialog();
            if (dd == DialogResult.OK)
            {

                FillTreeViewFolder();
                OpenFolder();

            }

        }
        private void CreateItem_MenuItem_Click(object sender, EventArgs e)
        {
            ItemAdd itemadd = new ItemAdd(DB, folder);
            DialogResult d = itemadd.ShowDialog();
            if (d == DialogResult.OK)
            {
                ItemForm itemform = new ItemForm(DB, itemsql.GetItemInfoByName(itemadd.textBoxItemName.Text, itemadd.textBoxCompanyName.Text));
                itemform.ShowDialog();
                ItemsListView = itemsql.GetItemsInFolder(folder);
                FillComboBoxComapines(ItemsListView);
                RefreshItems(FoldersListView, ItemsListView);
            }
        }
        private void OpenFolder_MenuItem_Click(object sender, EventArgs e)
        {
            string id_s = listView1.SelectedItems[0].Name;
            try
            {
                folder = foldersql.GetFolderInfoByID(Convert.ToUInt32(id_s));
                OpenFolder();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        private void OpenFolder_Page_MenuItem_Click(object sender, EventArgs e)
        {
            Folder trmpfolder = foldersql.GetFolderInfoByID(Convert.ToUInt32(listView1.SelectedItems[0].Name));
            FolderForm inp = new FolderForm(this.DB, trmpfolder, false );
            DialogResult dd = inp.ShowDialog();
            //if (dd == DialogResult.OK)
            //{

            //    FillTreeViewFolder();
            //    OpenFolder();

            //}

        }
        private void EditFolder_MenuItem_Click(object sender, EventArgs e)
        {
            Folder trmpfolder = foldersql.GetFolderInfoByID(Convert.ToUInt32(listView1.SelectedItems[0].Name));
            FolderForm inp = new FolderForm(this.DB, trmpfolder,true );
            DialogResult dd = inp.ShowDialog();
            if (dd == DialogResult.OK)
            {

                FillTreeViewFolder();
                OpenFolder();

            }

        }
        private void DeleteFolder_MenuItem_Click(object sender, EventArgs e)
        {
            Folder tempfolder = foldersql.GetFolderInfoByID(Convert.ToUInt32(listView1.SelectedItems[0].Name));
            DialogResult d = MessageBox.Show("لا يمكن حذف المجلد في حال لم يكن فارغ,كما سيتم حذف جميع الخصائص المرتبطة به,هل انت متأكد من حذف المجلد:" + tempfolder.FolderName, "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (d == DialogResult.OK)
                if (foldersql.DeleteFolder(tempfolder))
                {
                    FillTreeViewFolder();
                    OpenFolder();
                }

        }
        private void Paste_MenuItem_Click(object sender, EventArgs e)
        {
            if (folder == null && Moved_ItemList.Count > 0)
            {
                MessageBox.Show("لا يمكن نقل العناصر للمجلد الجذر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool success1 = foldersql.MoveFolders(folder, Moved_FolderList);
            bool success2 = itemsql.MoveItems(folder, Moved_ItemList);
            if (success1 || success2 )
            {
                FillTreeViewFolder();
                OpenFolder();
                Moved_FolderList = new List<Folder>();
                Moved_ItemList = new List<Item>();
            }
           
            
        }
        private void Cut_MenuItem_Click(object sender, EventArgs e)
        {

            Moved_FolderList.Clear();
            Moved_ItemList.Clear();
            Move_SourceFolder = folder;
            for(int i=0;i<listView1 .SelectedItems .Count;i++)
            {
                if (listView1.SelectedItems[i].SubItems[2].Text == "مجلد")
                {
                    Moved_FolderList.Add(foldersql.GetFolderInfoByID(Convert .ToUInt32 (listView1.SelectedItems[i].Name )));
                }
                else
                {
                    Moved_ItemList.Add(itemsql .GetItemInfoByID(Convert.ToUInt32(listView1.SelectedItems[i].Name)));
                }
            }
            if(Moved_ItemList.Count >0)
            {
                DialogResult d = MessageBox.Show("نقل العناصر سيؤدي الى حذف جميع قيم الخصائص المضبوطة للعناصر المنقولة, هل انت متاكد من الاستمرار بلعملية؟", "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if(d!=DialogResult.OK)
                {
                    Moved_FolderList.Clear();
                    Moved_ItemList.Clear();
                    return;
                }
            }

        }
        private void AddFolderSpec_MenuItem_Click(object sender, EventArgs e)
        {
            Folder f = foldersql.GetFolderInfoByID(Convert.ToUInt32(listView1.SelectedItems[0].Name));
            FolderSpec folderspec = new FolderSpec(DB, f);
            folderspec.ShowDialog();
        }
        private void OpenItem_MenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count >0)
            {
                string id_s1 = listView1.SelectedItems[0].Name;
                Item item = itemsql.GetItemInfoByID(Convert.ToUInt32(id_s1));
                ItemForm itemform = new ItemForm(this.DB, item);
                itemform.Show();
            }
        }
        private void EditItem_MenuItem_Click(object sender, EventArgs e)
        {
            string id_s1 = listView1.SelectedItems[0].Name;
            Item item = itemsql.GetItemInfoByID(Convert.ToUInt32(id_s1));
            ItemAdd itemadd = new ItemAdd(DB, item );
            DialogResult d = itemadd.ShowDialog();
            if (d == DialogResult.OK)
            {
                ItemForm itemform = new ItemForm(DB, itemsql.GetItemInfoByName(itemadd.textBoxItemName.Text, itemadd.textBoxCompanyName.Text));
                itemform.ShowDialog();
                ItemsListView = itemsql.GetItemsInFolder(folder);
                FillComboBoxComapines(ItemsListView);
                RefreshItems(FoldersListView, ItemsListView);
            }
        }
        private void DeleteItem_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("في حال كان العنصر مستخدم في عمليات البيع و الشراء و عمليات الصيانة فسيتعذر حذفه , هل أنت متاكد من اتمام عملية الحذف؟  ", "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (d != DialogResult.OK) return;
            string id_s1 = listView1.SelectedItems[0].Name;
            Item item = itemsql.GetItemInfoByID(Convert.ToUInt32(id_s1));
            bool success = itemsql.DeleteItem(item);
            if (success)
            {
                ItemsListView = itemsql.GetItemsInFolder(folder);
                FillComboBoxComapines(ItemsListView);
                RefreshItems(FoldersListView, ItemsListView);
            }
        }
        private void SpecFilter_MenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = false;
            FillFolderSpec();
        }
        #endregion
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (comboBox1.SelectedIndex)
            {
                case 2: listView1.View = View.List; break;
                case 1: listView1.View = View.SmallIcon ; break;
                case 0: listView1.View = View.Details ; break;
            }
           
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            if(e.Button==MouseButtons.Left && listView1.SelectedItems.Count >0 )
            {
                
                switch (listView1.SelectedItems[0].SubItems[2].Text)
                {
                    case "مجلد":
                        string id_s = listView1.SelectedItems[0].Name;
                        try
                        {
                            folder = foldersql.GetFolderInfoByID(Convert.ToUInt32(id_s));
                            OpenFolder();
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }
                        break;
                    case "عنصر":
                        try
                        {
                            string id_s1 = listView1.SelectedItems[0].Name;
                            Item item = itemsql.GetItemInfoByID(Convert.ToUInt32(id_s1));
                            ItemForm itemform = new ItemForm(this.DB, item);
                            itemform.Show();
                        }
                        catch
                        {

                        }
                  
                        break;
                }

            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if (folder  == null) return;
            folder = foldersql.GetParentFolder(folder);
            OpenFolder();
        }
        private void comboBoxFilterItemFolder_SelectedIndexChanged(object sender, EventArgs e)
        {

            RefreshItems(FoldersListView ,ItemsListView );
        }

        private void comboBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshItems(FoldersListView, ItemsListView);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Length > 0)
            {

                splitContainer1.Panel2Collapsed = true;
                if(checkBoxSearchType .Checked ==false )
                {
                    FoldersListView = foldersql.SearchFolder(textBoxSearch.Text);
                    ItemsListView = itemsql.SearchItem(textBoxSearch.Text);
                    FillComboBoxComapines(ItemsListView);
                    RefreshItems(FoldersListView, ItemsListView);
                }
                else
                {
                    ItemsListView = itemsql.SearchItemInFolder(folder, textBoxSearch.Text);
                    FillComboBoxComapines(ItemsListView);
                    RefreshItems(FoldersListView, ItemsListView);
                }
            }
            else OpenFolder();
        }
        private void checkBoxSearchType_CheckedChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Length > 0)
            {

                splitContainer1.Panel2Collapsed = true;
                if (checkBoxSearchType.Checked == false)
                {
                    FoldersListView = foldersql.SearchFolder(textBoxSearch.Text);
                    ItemsListView = itemsql.SearchItem(textBoxSearch.Text);
                    FillComboBoxComapines(ItemsListView);
                    RefreshItems(FoldersListView, ItemsListView);
                }
                else
                {
                    ItemsListView = itemsql.SearchItemInFolder(folder , textBoxSearch.Text);
                    FillComboBoxComapines(ItemsListView);
                    RefreshItems(FoldersListView, ItemsListView);
                }
            }
            else OpenFolder();
        }



        private void ButonSpecFilterClose_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed  = true;
            OpenFolder();
        }

    
   

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData ==Keys.Back )
            {
                if (folder == null) return;
                folder = foldersql.GetParentFolder(folder);
                OpenFolder();
                return;
            }
            if(listView1 .SelectedItems .Count >0 )
            {
                
                switch (e.KeyData )
                {
                    case Keys.Enter:
                        switch (listView1.SelectedItems[0].SubItems[2].Text)
                        {
                            case "مجلد":
                                string id_s = listView1.SelectedItems[0].Name;
                                try
                                {
                                    folder = foldersql.GetFolderInfoByID(Convert.ToUInt32(id_s));
                                    OpenFolder();
                                }
                                catch (Exception ee)
                                {
                                    MessageBox.Show(ee.Message);
                                }
                                break;
                            case "عنصر":
                                string id_s1 = listView1.SelectedItems[0].Name;
                                Item item = itemsql.GetItemInfoByID(Convert.ToUInt32(id_s1));
                                ItemForm itemform = new ItemForm(this.DB, item);
                                itemform.Show();
                                break;
                        }
                        break;
                    case Keys.Delete:

                        switch (listView1.SelectedItems[0].SubItems[2].Text)
                        {
                            case "مجلد":
                                DeleteFolderMenuItem.PerformClick();
                                break;
                            case "عنصر":
                                DeleteItemMenuItem.PerformClick();
                                break;
                        }
                        break;
   


                }

            }
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            Adjust_ListViewItemsColumns();
        }
        public async void Adjust_ListViewItemsColumns()
        {
            listView1.Columns[0].Width = 250;//itemname
            listView1.Columns[1].Width = 150;//company
            listView1.Columns[2].Width = 100;//Type
            listView1.Columns[3].Width = 200;//createdate
            listView1.Columns[4].Width = 150;//marketcode
            listView1.Columns[5].Width = 200;//defaultconsumeunbit

        }

        private void ShowItemsForm_Load(object sender, EventArgs e)
        {
            Adjust_ListViewItemsColumns(); ;
            textBoxSearch.Focus();
            //this.listView1.Resize += new System.EventHandler(this.listView1_Resize);

        }

   

        private void treeViewFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            if (e.Button == MouseButtons.Left )
            {
                if (treeViewFolders.SelectedNode != null)
                {
                    try
                    {
                        folder = foldersql.GetFolderInfoByID(Convert.ToUInt32(treeViewFolders.SelectedNode.Name));
                    }
                    catch
                    {
                        folder = null;
                    }

                    OpenFolder();
                }
          
            }
        }
    }
  
}
