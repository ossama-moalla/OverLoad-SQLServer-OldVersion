using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            PrintDialog printdlg = new PrintDialog();
            PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();

            // preview the assigned document or you can create a different previewButton for it
            printPrvDlg.Document = pd;
            printPrvDlg.ShowDialog(); // this shows the preview and then show the Printer Dlg below

            printdlg.Document = pd;

            if (printdlg.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Single yPos = 0;
            Single leftMargin = e.MarginBounds.Left;
            Single topMargin = e.MarginBounds.Top;
            //Image img = Image.FromFile("logo.bmp");
            Rectangle logo = new Rectangle(40, 40, 50, 50);
            using (Font printFont = new Font("Arial", 20.0f))
            {
                //e.Graphics.DrawImage(img, logo);
                e.Graphics.DrawString("Header", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            }
            using (SolidBrush blueBrush = new SolidBrush(Color.Black))
            {
                Rectangle rect = new Rectangle(100, 100, 500, 120);
                e.Graphics.FillRectangle(blueBrush, rect);
            }
        }

    }
}
