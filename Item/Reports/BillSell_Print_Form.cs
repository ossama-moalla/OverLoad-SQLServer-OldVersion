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
    public partial class BillSell_Print_Form : Form
    {
        List<ItemOUT_Report> ItemOUT_ReportList;
        public BillSell_Print_Form(List<ItemOUT_Report> ItemOUT_ReportList_)
        {
            InitializeComponent();
            ItemOUT_ReportList = ItemOUT_ReportList_;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            this.reportViewerBillSell.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            ItemOUT_ReportBindingSource.DataSource = ItemOUT_ReportList;
        }

        private void BillSell_Print_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
