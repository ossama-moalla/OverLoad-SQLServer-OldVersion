using ItemProject.AccountingObj.Objects;
using ItemProject.Trade.Objects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemProject
{
    public  class ComboboxItem
    {
        public string Text { get; set; }
        public uint Value { get; set; }
        public ComboboxItem(string Text_, uint Value_)
        {
            Text = Text_;
            Value = Value_;
        }
        public override string ToString()
        {
            return Text;
        }
    }
    public static class ProgramGeneralMethods
    {
        public static Currency GetDefaultCurrency(DatabaseInterface DB)
        {
            
            try
            {

                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);

                if (reg != null)
                {
                    string currencyid_Str = (string)reg.GetValue("DefaultCurrency");
                    uint currencyid = Convert.ToUInt32(currencyid_Str);
                    Currency defaultCurrency = new AccountingObj.AccountingSQL.CurrencySQL(DB).GetCurrencyINFO_ByID(currencyid);
                    if (defaultCurrency == null) throw new Exception();
                    return defaultCurrency;
                }
                else
                {
                    return new AccountingObj.AccountingSQL.CurrencySQL(DB).GetReferenceCurrency();
                }
            }
            catch
            {
                return  new AccountingObj.AccountingSQL.CurrencySQL(DB).GetReferenceCurrency();
            }


       }
        public static string GetDataBasePath()
        {

            try
            {

                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);

                if (reg != null)
                {
                     return (string)reg.GetValue("DataBase Location");
                }
                else return "";
            }
            catch
            {
                return "";
            }


        }
        public static async  void  SetDataBasePath(string path)
        {

            try
            {

                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);
                if (reg == null)
                {
                    reg = localMachine.CreateSubKey("Software\\OverLoadServer_1.0.0");
                }
                reg.SetValue("DataBase Location", path );

            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("فشل تخزين مسار قاعدة البيانات" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }


        }
        public static bool SetDefaultCurrency(uint CurrencyID)
        {
           try
            {
                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);
                if (reg == null)
                {
                    reg = localMachine.CreateSubKey("Software\\OverLoadServer_1.0.0");
                }

                reg.SetValue("DefaultCurrency", CurrencyID.ToString ());
                return true;
            }
            catch(Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(""+ee.Message ,"",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error );
                return false;
            }

        }
        public  static  void FillComboBoxCurrency(ref System.Windows.Forms.ComboBox comboBoxCurrency,DatabaseInterface DB, Currency currency)
        {
            if (currency == null) currency=GetDefaultCurrency (DB);
            comboBoxCurrency.Items.Clear();
            int selected_index = 0;
            try
            {
                List<Currency> CurrencyList = new AccountingObj.AccountingSQL.CurrencySQL(DB).GetCurrencyList();
                for (int i = 0; i < CurrencyList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(CurrencyList[i].CurrencyName + "(" + CurrencyList[i].CurrencySymbol + ")", CurrencyList[i].CurrencyID);
                    comboBoxCurrency.Items.Add(item);
                    if (currency != null && currency.CurrencyID == CurrencyList[i].CurrencyID)
                        selected_index = i;
                }
                comboBoxCurrency.SelectedIndex = selected_index;

            }
            catch
            {

            }

        }
        public static void FillComboBoxTradeState(ref System.Windows.Forms.ComboBox comboBoxTradeState, DatabaseInterface DB, Trade.Objects.TradeState TradeState_)
        {
            if (TradeState_ == null) TradeState_ = GetDefaultTradeState(DB);
            comboBoxTradeState.Items.Clear();
            int selected_index = 0;
            try
            {
                List<TradeState> TradeStateList = new Trade.TradeSQL.TradeStateSQL(DB).GetTradeStateList();
                for (int i = 0; i < TradeStateList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(TradeStateList[i].TradeStateName , TradeStateList[i].TradeStateID);
                    comboBoxTradeState .Items.Add(item);
                    if (TradeState_ != null && TradeState_ .TradeStateID == TradeStateList[i].TradeStateID)
                        selected_index = i;
                }
                comboBoxTradeState.SelectedIndex = selected_index;

            }
            catch
            {

            }

        }
        public static bool SetDefaultTradeState(uint TradeStateID)
        {
            try
            {
                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);
                if (reg == null)
                {
                    reg = localMachine.CreateSubKey("Software\\OverLoadServer_1.0.0");
                }

                reg.SetValue("DefaultTradeState", TradeStateID.ToString());
                return true;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }

        }
        public static Trade.Objects.TradeState GetDefaultTradeState(DatabaseInterface DB)
        {

            try
            {

                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);

                if (reg != null)
                {
                    string tradestateid_Str = (string)reg.GetValue("DefaultTradeState");
                    uint tradestateid = Convert.ToUInt32(tradestateid_Str);
                    Trade.Objects.TradeState defaultTradeState = new Trade.TradeSQL.TradeStateSQL(DB).GetTradeStateBYID(tradestateid);
                    return defaultTradeState;
                }
                else return null;
            }
            catch
            {
                return null;
            }


        }
        public static void FillComboBoxSellType(ref System.Windows.Forms.ComboBox comboBoxSellType, DatabaseInterface DB, Trade.Objects.SellType SellType_)
        {
            if (SellType_ == null) SellType_ = GetDefaultSellType(DB);
            comboBoxSellType.Items.Clear();
            int selected_index = 0;
            try
            {
                List<SellType> SellTypeList = new Trade.TradeSQL.SellTypeSql(DB).GetSellTypeList();
                for (int i = 0; i < SellTypeList.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem(SellTypeList[i].SellTypeName, SellTypeList[i].SellTypeID);
                    comboBoxSellType.Items.Add(item);
                    if (SellType_ != null && SellType_.SellTypeID == SellTypeList[i].SellTypeID)
                        selected_index = i;
                }
                comboBoxSellType.SelectedIndex = selected_index;

            }
            catch
            {

            }

        }
        public static bool SetDefaultSellType(uint SellTypeID)
        {
            try
            {
                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);
                if (reg == null)
                {
                    reg = localMachine.CreateSubKey("Software\\OverLoadServer_1.0.0");
                }

                reg.SetValue("DefaultSellType", SellTypeID.ToString());
                return true;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }

        }
        public static Trade.Objects.SellType GetDefaultSellType(DatabaseInterface DB)
        {

            try
            {

                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);

                if (reg != null)
                {
                    string SellTypeid_Str = (string)reg.GetValue("DefaultSellType");
                    uint SellTypeid = Convert.ToUInt32(SellTypeid_Str);
                    Trade.Objects.SellType defaultSellType = new Trade.TradeSQL.SellTypeSql(DB).GetSellTypeinfo(SellTypeid);
                    return defaultSellType;
                }
                else return null;
            }
            catch
            {
                return null;
            }


        }
        public static string GetUserName()
        {

            try
            {

                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);

                if (reg != null)
                {
                    return (string)reg.GetValue("UserName");
                }
                else return "";
            }
            catch
            {
                return "";
            }


        }
        public static async void SetUserName(string username)
        {

            try
            {

                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Default); //here you specify where exactly you want your entry

                var reg = localMachine.OpenSubKey("Software\\OverLoadServer_1.0.0", true);
                if (reg == null)
                {
                    reg = localMachine.CreateSubKey("Software\\OverLoadServer_1.0.0");
                }
                reg.SetValue("UserName", username );

            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("فشل تسجيل اسم المستخدم" + ee.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }


        }
    }
}
