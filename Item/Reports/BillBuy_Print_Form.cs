using ItemProject.Reports.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Reports
{
    public partial class BillBuy_Print_Form : Form
    {
        List<ItemIN_Report> ItemIN_ReportList;
        public BillBuy_Print_Form(List<ItemIN_Report> ItemIN_ReportList_)
        {
            InitializeComponent();
            ItemIN_ReportList = ItemIN_ReportList_;
        }


        private void BillBuy_Print_Form_Load(object sender, EventArgs e)
        {

            ItemIN_ReportBindingSource.DataSource = ItemIN_ReportList;
            this.reportViewer1.Refresh();
            this.reportViewer1.RefreshReport();
        }
    }
}
