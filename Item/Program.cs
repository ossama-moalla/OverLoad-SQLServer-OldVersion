using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                SelectDataBaseForm SelectDataBaseForm_ = new SelectDataBaseForm();
                DialogResult dd = SelectDataBaseForm_.ShowDialog();
                if (dd == DialogResult.OK)
                {
                    DatabaseInterface DB = SelectDataBaseForm_.DatabaseInterface_;

                    SelectDataBaseForm_.Dispose();
                    //DatabaseInterface DB = new DatabaseInterface(path);
                    try
                    {
                        string temppath = Application.StartupPath + "\\" + "OverLoadTemp";
                        if (!System.IO.Directory.Exists(temppath)) System.IO.Directory.CreateDirectory(temppath);

                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(temppath);

                        foreach (System.IO.FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                    }
                    catch
                    {

                    }


                    ////Application.Run(new ItemObj.Forms.ShowAvailableItemSimpleForm (DB,false ));
                    Application.Run(new AccountingObj.Forms.MainWindowForm(DB));


                }
                else break;

            }



        }
        public static bool ConnectDataBase(string path)
        {
            DatabaseInterface DB = new DatabaseInterface(path);
            try
            {
                DB.DATABASE_CONNECTION.Open();
                if (DB.DATABASE_CONNECTION.State == System.Data.ConnectionState.Open) DB.DATABASE_CONNECTION.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


}
