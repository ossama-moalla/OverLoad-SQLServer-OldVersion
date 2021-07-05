using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.Objects;
using ItemProject.Maintenance.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemProject.Trade
{
    namespace  Objects
    {
        public class IndustryOPR
        {
            public const bool ASSEMBLAGE_OPR = true;
            public const bool DISASSEMBLAGE_OPR = false ;
            /// <summary>
            /// ///
            /// </summary>
            public bool OPR_Type;
            public uint OPR_ID;
            public DateTime OPR_Date;
            public string FolderName;
            public string ItemName;
            public string CompanyName;
            public string ItemSerial;
            public double Cost;
            public string  CurrencyName;
            public double ExchangeRate;
            public string OPRStatus;
            public string OPRItemsInfo;
            public IndustryOPR(bool OPR_Type_,uint OPR_ID_
                ,DateTime OPR_Date_, string FolderName_, string ItemName_
                , string CompanyName_,string ItemSerial_
                , double Cost_,string CurrencyName_, 
                double ExchangeRate_,string OPRStatus_, string OPRItemsInfo_)
            {
                OPR_Type = OPR_Type_;
                OPR_ID = OPR_ID_;
                OPR_Date = OPR_Date_;
                FolderName = FolderName_;
                ItemName = ItemName_;
                CompanyName = CompanyName_;
                ItemSerial = ItemSerial_;
                Cost = Cost_;
                CurrencyName  = CurrencyName_;
                ExchangeRate = ExchangeRate_;
                OPRStatus = OPRStatus_;
                OPRItemsInfo = OPRItemsInfo_;
            }
        }
        public class DisAssemblabgeOPR
        {
            public Operation _Operation;
            public DateTime OprDate;
            public ItemOUT _ItemOUT;
            public string ItemSerial;
            public Currency _Currency;
            public double ExchangeRate;
            public bool Ravage;
            public DisAssemblabgeOPR(uint OperationID, DateTime OprDate_
                ,  ItemOUT ItemOUT_,
                string ItemSerial_,Currency Currency_,double ExchangeRate_,
                bool Ravage_)
            {
                _Operation = new Operation(Operation.DISASSEMBLAGE, OperationID);
                OprDate = OprDate_;
                _ItemOUT = ItemOUT_;
                ItemSerial = ItemSerial_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                Ravage = Ravage_;
                
            }
        }
        public class AssemblabgeOPR
        {
            public Operation _Operation;
            public DateTime OprDate;
            public ItemIN _ItemIN;
            public string ItemSerial;
            public Currency _Currency;
            public double ExchangeRate;
            public AssemblabgeOPR(uint OperationID, DateTime OprDate_,
                ItemIN ItemIN_,
                string ItemSerial_, Currency Currency_, double ExchangeRate_
                )
            {
                _Operation = new Operation(Operation.ASSEMBLAGE , OperationID);
                OprDate = OprDate_;
                _ItemIN = ItemIN_;
                ItemSerial = ItemSerial_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;


            }
        }

    }
}
