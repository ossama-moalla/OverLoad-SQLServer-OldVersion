using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject
{
    public partial class SelectDataBaseForm : Form
    {
        private DatabaseInterface _DatabaseInterface;
        public DatabaseInterface DatabaseInterface_
        {
            get
            {
                return _DatabaseInterface;
            }
        }
        public SelectDataBaseForm()
        {
            InitializeComponent();
            textBoxDatabasePath.Text = ProgramGeneralMethods.GetDataBasePath();
            textboxUserName .Text = ProgramGeneralMethods.GetUserName ();
        }

        public static bool ConnectDataBase(string path)
        {
           
            try
            {
                DatabaseInterface DB = new DatabaseInterface(path);
                if (DB.DATABASE_CONNECTION.State != ConnectionState.Open)
                    DB.DATABASE_CONNECTION.Open();
                if (DB.DATABASE_CONNECTION.State == System.Data.ConnectionState.Open) DB.DATABASE_CONNECTION.Close();
                return true;
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }
        }
       
        private void buttonNext_Click(object sender, EventArgs e)
        {
           
            if(textBoxDatabasePath .Text .Length ==0)
            {
                MessageBox.Show("يرجى تحديد قاعدة البيانات","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (!ConnectDataBase (textBoxDatabasePath.Text))
            {
                
                MessageBox.Show("فشل الاتصال بقاعدة البيانات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
           
            ProgramGeneralMethods.SetDataBasePath(textBoxDatabasePath.Text);
            _DatabaseInterface = new DatabaseInterface(textBoxDatabasePath.Text);
            try
            {
                try
                {
                    _DatabaseInterface.LogIN(textboxUserName.Text, textBoxPassWord.Text);
                    ProgramGeneralMethods.SetUserName(textboxUserName.Text);
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message , "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message , "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
 
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog .Filter = "Sql DataBase(*.mdf)|*.mdf";
            DialogResult d= filedialog.ShowDialog();
            if (d == DialogResult.OK)
                textBoxDatabasePath.Text = filedialog.FileName;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) buttonNext.PerformClick();
        }
    }
}
