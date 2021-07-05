using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemProject.Reports
{
    namespace   Objects
    {
        public class ItemOUT_Report
        {
            public uint ItemOUTID { get; set; }
            public string  ItemType { get; set; }
            public string  ItemName { get; set; }
            public string  ItemCompany { get; set; }
            public double SingleValue { get; set; }
            public double Amount { get; set; }
            public string  ConsumeUnit { get; set; }
            public double TotlalValue { get; set; }
            public ItemOUT_Report( 
                uint ItemOUTID_,
             string ItemType_,
             string ItemName_,
             string ItemCompany_,
             double SingleValue_,
             double Amount_,
             string  ConsumeUnit_,
             double TotlalValue_)
            {
                ItemOUTID = ItemOUTID_;
             ItemType = ItemType_;
                ItemName = ItemName_;
                ItemCompany = ItemCompany_;
                SingleValue = SingleValue_;
                Amount = Amount_;
                ConsumeUnit = ConsumeUnit_;
                TotlalValue = TotlalValue_;

            }
            public static List <ItemOUT_Report > GetItemOUT_ReportList(List <Trade.Objects.ItemOUT> ItemOUTList)
            {
                List < ItemOUT_Report > list= new List<ItemOUT_Report>();
                try
                {
                    for(int i=0;i<ItemOUTList.Count;i++)
                    {
                        list.Add(new ItemOUT_Report(ItemOUTList[i].ItemOUTID , ItemOUTList[i]._ItemIN._Item.folder .FolderName
                            , ItemOUTList[i]._ItemIN ._Item .ItemName, ItemOUTList[i]._ItemIN._Item .ItemCompany, ItemOUTList[i]._OUTValue .Value , ItemOUTList[i].Amount
                            , ItemOUTList[i]._ConsumeUnit.ConsumeUnitName,(ItemOUTList[i].Amount * ItemOUTList[i]._OUTValue .Value )));
                    }

                }catch
                {

                }
                return list;
            }
    }
        public class ItemIN_Report
        {
            public uint ItemINID { get; set; }
            public string ItemType { get; set; }
            public string ItemName { get; set; }
            public string ItemCompany { get; set; }
            public double SingleValue { get; set; }
            public double Amount { get; set; }
            public string ConsumeUnit { get; set; }
            public double TotlalValue { get; set; }
            public ItemIN_Report(
                uint ItemINID_,
             string ItemType_,
             string ItemName_,
             string ItemCompany_,
             double SingleValue_,
             double Amount_,
             string ConsumeUnit_,
             double TotlalValue_)
            {
                ItemINID = ItemINID_;
                ItemType = ItemType_;
                ItemName = ItemName_;
                ItemCompany = ItemCompany_;
                SingleValue = SingleValue_;
                Amount = Amount_;
                ConsumeUnit = ConsumeUnit_;
                TotlalValue = TotlalValue_;

            }
            public static List<ItemIN_Report> GetItemIN_ReportList(List<Trade.Objects.ItemIN> ItemINList)
            {
                List<ItemIN_Report> list = new List<ItemIN_Report>();
                try
                {
                    for (int i = 0; i < ItemINList.Count; i++)
                    {
                        list.Add(new ItemIN_Report(ItemINList[i].ItemINID, ItemINList[i]._Item.folder.FolderName
                            , ItemINList[i]._Item.ItemName, ItemINList[i]._Item.ItemCompany, ItemINList[i]._INCost.Value, ItemINList[i].Amount
                            , ItemINList[i]._ConsumeUnit.ConsumeUnitName, (ItemINList[i].Amount * ItemINList[i]._INCost .Value)));
                    }

                }
                catch
                {

                }
                return list;
            }
        }
        public class Bill_PayOUT_Report
        {
            public uint PayOprID { get; set; }
            public DateTime PayOprDate { get; set; }
            public double Value { get; set; }
            public string  Currency { get; set; }
            public double ExchangeRate { get; set; }

            public Bill_PayOUT_Report(uint PayOprID_, DateTime PayOprDate_
                , double Value_, double ExchangeRate_, string  Currency_
               )
            {
                PayOprID = PayOprID_;
                PayOprDate = PayOprDate_;
                Value = Value_;
                Currency = Currency_;
                ExchangeRate = ExchangeRate_;

            }

        }
    }
}
