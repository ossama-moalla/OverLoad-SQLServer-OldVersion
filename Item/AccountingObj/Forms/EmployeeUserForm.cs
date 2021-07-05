using ItemProject.Company.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.AccountingObj.Forms
{

    public partial class EmployeeUserForm : Form
    {
        public const uint ResetPassWord_Function=1;
        public const uint EditUserData_Function = 2;
        private Employee _Employee;
        private DatabaseInterface.User _User;
        private DatabaseInterface DB;
        private uint Function;
        public EmployeeUserForm(DatabaseInterface db,Employee Employee_)
        {
            InitializeComponent();
            DB = db;
            _Employee = Employee_;
            textBoxEmployeeID.Text = _Employee .EmployeeID.ToString();
            textBoxEmployeeName.Text = _Employee.EmployeeName ;
            buttonSave.Name = "buttonAdd";
            buttonSave.Text = "اضافة";
            comboBoxDisable.SelectedIndex = 0;
        }

        public EmployeeUserForm(DatabaseInterface db, DatabaseInterface.User User_, uint Function_)
        {
            try
            {
                Function = Function_;
                if (Function != ResetPassWord_Function && Function != EditUserData_Function)
                    throw new Exception("تابع غير معروف");
                Function = Function_;
                DB = db;
                InitializeComponent();
                _User  = User_;
                _Employee  = _User._Employee ;
                textBoxEmployeeID.Text = _Employee.EmployeeID.ToString();
                textBoxEmployeeName.Text = _Employee.EmployeeName;
                textBoxUserName.Text = _User.UserName;
                if (_User.Disabled) comboBoxDisable.SelectedIndex = 1;
                else comboBoxDisable.SelectedIndex = 0;
                
                if (Function == ResetPassWord_Function)
                {

                    textBoxUserName.ReadOnly = true;
                    comboBoxDisable .Enabled = false;
                    textBoxPassword.ReadOnly = false ;
                    textBoxPasswordConfirm.ReadOnly = false ;
                    textBoxUserName.BackColor = Color.Gray;
     
                }
                else
                {

                    textBoxPassword.Enabled  = false  ;
                    textBoxPasswordConfirm.Enabled = false;
                    textBoxUserName.ReadOnly = false ;
                    comboBoxDisable.Enabled = true ;
                    textBoxPassword.BackColor  = Color.Gray   ;
                    textBoxPasswordConfirm.BackColor  = Color.Gray ;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(":فشل تحميل الصفحة" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
       

        private void buttonSave_Click(object sender, EventArgs e)
        {

            if (_Employee  == null) return;

            if(!textBoxPassword.Text .SequenceEqual(textBoxPasswordConfirm.Text ))
            {
                MessageBox.Show("كلمة المرور غير متطابقة في الحقلين", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxPassword.Text.Contains (" "))
            {
                MessageBox.Show("كلمة المرور يجب ان لا تحوي فراغات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool disabled;
            if (comboBoxDisable.SelectedIndex == 0) disabled  = false ;
            else
                disabled = true ;
            if (buttonSave.Name == "buttonAdd")
            {
                try
                {
                    bool success =DB.AddUser 
                        (_Employee .EmployeeID, textBoxUserName.Text, textBoxPassword.Text ,disabled );
                    if (success)
                    {

                        MessageBox.Show("تم الاضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":تعذر اضافة المستخدم " + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                try
                {
                    if (Function == ResetPassWord_Function)
                    {
                        bool success = DB.ResetUserPassword
                          (_Employee.EmployeeID, textBoxPassword .Text );
                        if (success)
                        {

                            MessageBox.Show("تم اعادة تعيين كلمة المرور بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();

                        }
                    }
                    else
                    {
                        bool success = DB.EditUserData 
                          (_Employee.EmployeeID, textBoxUserName.Text,disabled );
                        if (success)
                        {

                            MessageBox.Show("تم الحفظ بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();

                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(":حدث خطأ" + ee.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
