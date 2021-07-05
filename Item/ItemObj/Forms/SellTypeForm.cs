using ItemProject.ItemObj.ItemObjSQL;
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
namespace ItemProject.ItemObj.Forms
{
    public partial class SellTypeForm : Form
    {
        List<SellType> SellTypeList = new List<SellType>();
        List<TradeState> TradeStateList = new List<TradeState>();
        SellTypeSql SellTypeSQL_;
        TradeStateSQL TradeStateSQL_;
        DatabaseInterface DB;
        MenuItem AddSellType;
        MenuItem UpdateSellType;
        MenuItem deleteSellType;
        MenuItem AddItemBysSellState;
        MenuItem UpdateItemBysSellState;
        MenuItem deleteItemBysSellState;
        public SellTypeForm(DatabaseInterface db)
        {
            DB = db;
            SellTypeSQL_ = new SellTypeSql(DB);
            TradeStateSQL_ = new TradeStateSQL(DB);
            InitializeComponent();
            AddSellType = new MenuItem("اضافة", AddSellType_MenuItem_Click);
            UpdateSellType = new MenuItem("تعديل", UpdateSellType_MenuItem_Click);
            deleteSellType = new MenuItem("حذف", DeleteSellType_MenuItem_Click);
            AddItemBysSellState = new MenuItem("اضافة", AddItemBuySellState_MenuItem_Click);
            UpdateItemBysSellState = new MenuItem("تعديل", UpdateItemBuySellState_MenuItem_Click);
            deleteItemBysSellState = new MenuItem("حذف", DeletItemBuySellState_MenuItem_Click);

           
            Refresh_ListViewSellTypes();
            Refresh_ListViewItemBuySellState();
        }
        public  void Refresh_ListViewSellTypes()
        {

            SellTypeList = SellTypeSQL_.GetSellTypeList();
            listViewMeasureType.Items.Clear();
            if(SellTypeList != null)
            {
                for(int i=0;i< SellTypeList.Count;i++)
                {
                    
                    ListViewItem item = new ListViewItem(SellTypeList[i].SellTypeName);
                    item.Name = SellTypeList[i].SellTypeID .ToString();
                    listViewMeasureType.Items.Add(item);
                }
            }
        }
        public void Refresh_ListViewItemBuySellState()
        {

            TradeStateList = TradeStateSQL_.GetTradeStateList ();
            listViewItemBuySellState.Items.Clear();
            if (TradeStateList != null)
            {
                for (int i = 0; i < TradeStateList .Count; i++)
                {

                    ListViewItem item = new ListViewItem(TradeStateList [i].TradeStateName );
                    item.Name = TradeStateList [i].TradeStateID .ToString();
                    listViewItemBuySellState.Items.Add(item);
                }
            }
        }
        private void listViewMeasureType_MouseDown(object sender, MouseEventArgs e)
        {

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    listViewMeasureType.ContextMenu = null;
                    bool match = false;
                    ListViewItem listitem = new ListViewItem();
                    foreach (ListViewItem item1 in listViewMeasureType.Items)
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

                            MenuItem[] mi1 = new MenuItem[] { AddSellType, UpdateSellType, deleteSellType  };
                    listViewMeasureType.ContextMenu = new ContextMenu(mi1);
                    }
                    else
                    {

                        MenuItem[] mi = new MenuItem[] { AddSellType  };
                    listViewMeasureType.ContextMenu = new ContextMenu(mi);
                     }

                }
            
        }
        private void UpdateSellType_MenuItem_Click(object sender, EventArgs e)
        {
            ItemProject.Forms.InputBox inp = new ItemProject.Forms.InputBox("تعديل", " أدخل نمط التسعير الجديد", listViewMeasureType.SelectedItems[0].Text );
            inp.button1.Text = "تعديل";
            DialogResult d = inp.ShowDialog();
            if (d == DialogResult.OK)
            {
                if (SellTypeSQL_.UpdateSellType  (Convert .ToUInt32 ( listViewMeasureType.SelectedItems [0].Name ) ,
                    inp.textBox1.Text))
                {
                    Refresh_ListViewSellTypes();
                    MessageBox.Show("تم التعديل بنجاح", "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("فشل التعديل ", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void AddSellType_MenuItem_Click(object sender, EventArgs e)
        {
            ItemProject.Forms.InputBox inp = new ItemProject.Forms.InputBox("نمط تسعير جديد", "أدخل اسم نمط التسعير");
            DialogResult d= inp.ShowDialog();
            if(d==DialogResult.OK)
            {
                if (SellTypeSQL_.AddSellType (inp.textBox1.Text))
                {
                    Refresh_ListViewSellTypes();
                    MessageBox.Show("تم الادخال بنجاح", "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("فشل الادخال ", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
 

        }
        private void DeleteSellType_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("هل انت متاكد من الحذف", "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (d != DialogResult.OK) return;
            SellTypeSQL_  .DeleteSellType   (Convert.ToUInt32(listViewMeasureType.SelectedItems[0].Name));
            Refresh_ListViewSellTypes();

        }

        private void listViewItemBuySellState_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listViewItemBuySellState.ContextMenu = null;
                bool match = false;
                ListViewItem listitem = new ListViewItem();
                foreach (ListViewItem item1 in listViewItemBuySellState.Items)
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

                    MenuItem[] mi1 = new MenuItem[] { AddItemBysSellState, UpdateItemBysSellState, deleteItemBysSellState  };
                    listViewItemBuySellState.ContextMenu = new ContextMenu(mi1);
                }
                else
                {

                    MenuItem[] mi = new MenuItem[] { AddItemBysSellState };
                    listViewItemBuySellState.ContextMenu = new ContextMenu(mi);
                }

            }
        }
        private void UpdateItemBuySellState_MenuItem_Click(object sender, EventArgs e)
        {
            ItemProject.Forms.InputBox inp = new ItemProject.Forms.InputBox("تعديل", " أدخل معلومات  حالة بيع- شراء العنصر", listViewMeasureType.SelectedItems[0].Text);
            inp.button1.Text = "تعديل";
            DialogResult d = inp.ShowDialog();
            if (d == DialogResult.OK)
            {
                if (TradeStateSQL_.UpdateTradeState (Convert.ToUInt32(listViewItemBuySellState.SelectedItems[0].Name),
                    inp.textBox1.Text))
                {
                    Refresh_ListViewItemBuySellState();
                    MessageBox.Show("تم التعديل بنجاح", "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("فشل التعديل ", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void AddItemBuySellState_MenuItem_Click(object sender, EventArgs e)
        {
            ItemProject.Forms.InputBox inp = new ItemProject.Forms.InputBox("", "أدخل اسم حالة بيع-شراء العنصر");
            DialogResult d = inp.ShowDialog();
            if (d == DialogResult.OK)
            {
                if (TradeStateSQL_.AddTradeState  (inp.textBox1.Text))
                {
                    Refresh_ListViewItemBuySellState();
                    MessageBox.Show("تم الادخال بنجاح", "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("فشل الادخال ", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void DeletItemBuySellState_MenuItem_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("هل انت متاكد من الحذف", "تحذير", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (d != DialogResult.OK) return;
            TradeStateSQL_  .DeleteTradestate  (Convert.ToUInt32(listViewItemBuySellState .SelectedItems[0].Name));
            Refresh_ListViewItemBuySellState();

        }
    }
}
