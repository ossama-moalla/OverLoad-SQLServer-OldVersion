using ItemProject.AccountingObj.Objects;
using ItemProject.ItemObj.Objects;
using ItemProject.Maintenance.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemProject.Trade
{
    namespace  Objects
    {
        public  class Operation
        {
            public const uint BILL_BUY = 1;
            public const uint BILL_SELL= 2;
            public const uint BILL_MAINTENANCE = 3;
            public const uint Employee_PayOrder = 4;
            public const uint MAINTENANCE_OPR = 5;
            public const uint ASSEMBLAGE = 6;
            public const uint DISASSEMBLAGE = 7;
            public const uint RAVAGE = 8;
            public const uint REPAIROPR = 9;
            /// <summary>
            /// ////////////
            /// </summary>
            public uint OperationType;
            public uint OperationID;
            public Operation (uint OperationType_,uint OperationID_)
            {
                OperationType = OperationType_;
                OperationID = OperationID_;
            }
            public static  string GetOperationName(uint operationtype)
            {
                string operationname = "";
                switch (operationtype)
                {
                    case BILL_BUY:
                        operationname= "فاتور شراء";
                        break;
                    case BILL_SELL :
                        operationname = "فاتور مبيع";
                        break;
                    case BILL_MAINTENANCE:
                        operationname = "فاتور صيانة";
                        break;
                    case Employee_PayOrder:
                        operationname = "امر صرف";
                        break;
                    case MAINTENANCE_OPR:
                        operationname = "عملية صيانة";
                        break;
                    case ASSEMBLAGE:
                        operationname = "عملية تجميع";
                        break;
                    case DISASSEMBLAGE:
                        operationname = "عملية تفكيك";
                        break;
                    case RAVAGE:
                        operationname = "عملية اتلاف";
                        break;
                    case REPAIROPR:
                        operationname = "عملية اصلاح";
                        break;
                }
                return operationname;
            }
            public static string GetOperationItemOutDesc(uint operationtype)
            {
                string operationname = "";
                switch (operationtype)
                {

                    case BILL_SELL:
                        operationname = "اخراج مادة عن طريق فاتورة مبيع";
                        break;

                    case BILL_MAINTENANCE:
                        operationname = "اخراج مادة عن طريق فاتورة صيانة";
                        break;
                    case ASSEMBLAGE:
                        operationname = "ادراج عنصر في عملية تجميع";
                        break;
                    case DISASSEMBLAGE:
                        operationname = "تفكيك عنصر";
                        break;
                    case RAVAGE:
                        operationname = "اتلاف عنصر";
                        break;
                    case REPAIROPR:
                        operationname = "اخراج مادة عن طريق عملية اضلاح";
                        break;
                    default:
                        throw new Exception("عملية غير صحيحة");
                }
                return operationname;
            }
            public static  void FillComboBoxBillType_PayIN(ref System.Windows.Forms.ComboBox ComboBox_,uint OperationType)
            {
                ComboBox_.Items.Clear();
                int selected_index = 0;
                try
                {
                    List<ComboboxItem> PayIN_OperationTypeList = new List<ComboboxItem>();
                    PayIN_OperationTypeList.Add(new ComboboxItem("مبيع", Operation.BILL_SELL));
                    PayIN_OperationTypeList.Add(new ComboboxItem("صيانة", Operation.BILL_MAINTENANCE));
                    for (int i = 0; i < PayIN_OperationTypeList.Count; i++)
                    {
                        ComboBox_.Items.Add(PayIN_OperationTypeList[i]);
                        if (OperationType == PayIN_OperationTypeList[i].Value )
                            selected_index = i;
                    }
                    ComboBox_.SelectedIndex = selected_index;

                }
                catch
                {

                }
            }
            public static void FillComboBoxBillType_PayOUT(ref System.Windows.Forms.ComboBox ComboBox_, uint OperationType)
            {
                ComboBox_.Items.Clear();
                int selected_index = 0;
                try
                {
                    List<ComboboxItem> PayOut_OperationTypeList = new List<ComboboxItem>();
                    PayOut_OperationTypeList.Add(new ComboboxItem("شراء", Operation.BILL_BUY));
                    for (int i = 0; i < PayOut_OperationTypeList.Count; i++)
                    {
                        ComboBox_.Items.Add(PayOut_OperationTypeList[i]);
                        if (OperationType == PayOut_OperationTypeList[i].Value)
                            selected_index = i;
                    }
                    ComboBox_.SelectedIndex = selected_index;

                }
                catch
                {

                }
            }
        }
        public class Bill
        {
            public Operation _Operation;
            public DateTime BillDate;
            public string BillDescription;
            public Contact _Contact;
            public Currency _Currency;
            public double ExchangeRate;
            public double Discount;
            public string Notes;
            //public Bill(uint BillID_, DateTime BillDate_, string BillDescription_,  Contact Contact_, Currency Currency_, double ExchangeRate_, double Discount_, string Notes_)
            //{
            //    BillID = BillID_;
            //    BillDate = BillDate_;
            //    BillDescription = BillDescription_;
            //    _Contact = Contact_;
            //    _Currency = Currency_;
            //    ExchangeRate = ExchangeRate_;
            //    Discount = Discount_;
            //    Notes = Notes_;
            //}
        }
        public class BillAdditionalClause
        {
            public Operation _Operation;
            public uint ClauseID;
            public string Description;
            public double Value;
            public BillAdditionalClause (Operation Operation_,uint ClauseID_
                ,string Description_,double Value_)
            {
                _Operation = Operation_;
                ClauseID = ClauseID_;
                Description = Description_;
                Value = Value_;

            }

        }
        public class BillSell:Bill
        {

            public SellType  _SellType;
            public BillSell(uint BillID_, DateTime BillDate_,string BillDescription_, SellType   SellType_, Contact Contact_, Currency Currency_, double ExchangeRate_, double Discount_,string Notes_)
                //:base (BillID_,BillDate_ ,BillDescription_ ,Contact_,Currency_ ,ExchangeRate_ ,Discount_ ,Notes_ )
            {
                _Operation = new Operation (Operation.BILL_SELL,BillID_ );
                BillDate = BillDate_;
                BillDescription = BillDescription_;
                _SellType = SellType_;
                _Contact = Contact_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                Discount = Discount_;
                Notes = Notes_;
            }
        }

        public class BillBuy : Bill
        {
            public BillBuy(uint BillID_, DateTime BillDate_, string BillDescription_, Contact Contact_, Currency Currency_, double ExchangeRate_, double Discount_, string Notes_)

            {
                _Operation = new Operation(Operation.BILL_BUY, BillID_);
                BillDate = BillDate_;
                BillDescription = BillDescription_;
                _Contact = Contact_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
                Discount = Discount_;
                Notes = Notes_;
            }

        }
        public class ItemIN
        {
            public uint ItemINID;
            public Operation _Operation;
            public Item _Item;
            public TradeState _TradeState;
            public double Amount;
            public ConsumeUnit _ConsumeUnit;
            public INCost _INCost;
            public string Notes;
            public ItemIN(uint ItemINID_, Operation Operation_, 
                Item Item_, TradeState TradeState_, double Amount_,
                ConsumeUnit ConsumeUnit_, INCost INCost_, string Notes_)
            {
                
                ItemINID = ItemINID_;
                _Operation = Operation_;
                _Item = Item_;
                _TradeState = TradeState_;
                Amount = Amount_;
                _ConsumeUnit = ConsumeUnit_;
                _INCost = INCost_;
                Notes = Notes_;

            }
        }
        public class ItemIN_StoreReport
        {
            public ItemIN _ItemIN;
            public TradeStorePlace Place;
            public ConsumeUnit _ConsumeUnit;
            public double StoreAmount;
            public double SpentAmount;
            public ItemIN_StoreReport(   ItemIN ItemIN_,
             TradeStorePlace Place_,
             ConsumeUnit ConsumeUnit_,
             double StoreAmount_,
             double SpentAmount_)
            {
                   _ItemIN= ItemIN_;
             Place= Place_;
             _ConsumeUnit= ConsumeUnit_;
             StoreAmount= StoreAmount_;
            SpentAmount= SpentAmount_;
        }
        }
        public class INCost
        {
            public double Value;
            public Currency _Currency;
            public double ExchangeRate;
            public INCost ( double Value_,
             Currency Currency_,
             double ExchangeRate_)
            {
                Value = Value_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
            }
        }
        public class OUTValue
        {
            public double Value;
            public Currency _Currency;
            public double ExchangeRate;
            public OUTValue(double Value_,
             Currency Currency_,
             double ExchangeRate_)
            {
                Value = Value_;
                _Currency = Currency_;
                ExchangeRate = ExchangeRate_;
            }
        }
        public class ItemOUT
        {
            public uint ItemOUTID {get; }
            public Operation _Operation { get; set; }
            public ItemIN _ItemIN { get; set; }
            public TradeStorePlace Place { get; set; }
            public double Amount { get; set; }
            public ConsumeUnit _ConsumeUnit { get; set; }
            public OUTValue _OUTValue { get; set; }
            public string Notes { get; set; }
            public ItemOUT(uint ItemOUTID_, Operation Operation_, ItemIN ItemIN_, TradeStorePlace Place_,  double Amount_, ConsumeUnit ConsumeUnit_, OUTValue OUTValue_, string Notes_)
            {
                ItemOUTID = ItemOUTID_;
                _Operation = Operation_;
                _ItemIN = ItemIN_;
                Place = Place_;
                Amount = Amount_;
                _ConsumeUnit = ConsumeUnit_;
                _OUTValue = OUTValue_;
                Notes = Notes_;

            }
           

        }
        internal class ItemOUTWithCurrencyInfo
        {
            public ItemOUT _ItemOUT;
            public Currency _Currency;
            public ItemOUTWithCurrencyInfo(DatabaseInterface DB, ItemOUT ItemOUT_)
            {
                _ItemOUT = ItemOUT_;
                _Currency = new TradeSQL.OperationSQL(DB).GetOperationItemOUTCurrency(ItemOUT_._Operation);
            }
            public static List<ItemOUTWithCurrencyInfo> ConvertTo_ItemOUTWithCurrencyInfoList(DatabaseInterface DB, List<ItemOUT> ItemOUTList)
            {
                List<ItemOUTWithCurrencyInfo> list = new List<ItemOUTWithCurrencyInfo>();
                try
                {
                    for (int i = 0; i < ItemOUTList.Count; i++)
                    {
                        list.Add(new ItemOUTWithCurrencyInfo(DB, ItemOUTList[i]));
                    }
                    return list;
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("فشل جلب التفاصيل العملة", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    return list;
                }
            }
            public static string GetTotalItemsOUTValue(List<ItemOUTWithCurrencyInfo> ItemOUTWithCurrencyInfoList)
            {
                string value_str = "";
                try
                {

                    List<uint> currencyIDlist = ItemOUTWithCurrencyInfoList.Select(x => x._Currency.CurrencyID).Distinct().ToList();
                    for (int i = 0; i < currencyIDlist.Count; i++)
                    {

                        double valuecurrency = 0;
                        List<ItemOUTWithCurrencyInfo> templist = ItemOUTWithCurrencyInfoList.Where(x => x._Currency.CurrencyID == currencyIDlist[i]).ToList();
                        Currency cuurency = templist[0]._Currency;
                        for (int j = 0; j < templist.Count; j++)
                        {
                            valuecurrency = valuecurrency + templist[j]._ItemOUT._OUTValue.Value ;
                        }
                        value_str = value_str + valuecurrency + " " + cuurency.CurrencySymbol.Replace(" ", string.Empty) + "  ";
                    }
                    if (value_str.Length < 1)
                        return "-";
                    else
                        return value_str;
                }
                catch (Exception ee)
                {
                    return "حصل خطأ" + ee.Message;
                }
            }


        }
        public class ItemIN_ItemOUTReport
        {
            public ItemIN _ItemIN;
            public List<ItemOUT  > ItemOUTList;
            public ItemIN_ItemOUTReport(ItemIN ItemIN_, List<ItemOUT > ItemOUTList_)
            {
                _ItemIN = ItemIN_;
                ItemOUTList  = ItemOUTList_ ;
            }
        }
        //public class BuyOPR
        //{
        //    public uint BuyOPRID;
        //    public BillOUT _BillOUT;
        //    public Item _Item;
        //    public TradeState _TradeState;
        //    public double Amount;
        //    public ConsumeUnit _ConsumeUnit;
        //    public double BuyPrice;
        //    public string Notes;
        //    public BuyOPR(uint BuyOPRID_, BillOUT BillOUT_, Item  Item_, TradeState  TradeState_, double Amount_, ConsumeUnit ConsumeUnit_, double BuyPrice_,  string Notes_)
        //    {
        //        BuyOPRID = BuyOPRID_;
        //        _BillOUT = BillOUT_;
        //        _Item  = Item_ ;
        //        _TradeState = TradeState_;
        //        Amount = Amount_;
        //        _ConsumeUnit = ConsumeUnit_;
        //        BuyPrice = BuyPrice_;
        //        Notes = Notes_;

        //    }
        //}
        public class ItemINSellPrice
        {


            public ItemIN _ItemIN;
            public ConsumeUnit ConsumeUnit_;
            public SellType SellType_;
            public double Price;
            public ItemINSellPrice(ItemIN ItemIN_, ConsumeUnit ConsumeUnit__, SellType SellType__, double Price_)
            {

                _ItemIN = ItemIN_;
                ConsumeUnit_ = ConsumeUnit__;
                SellType_ = SellType__;
                Price = Price_;
            }

        }
        //public class SellOPR
        //{
        //    public uint SellOPRID;
        //    public BillSell _BillSell;
        //    public MaintenanceOPR _MaintenanceOPR;
        //    public BuyOPR _BuyOPR;
        //    public TradeStorePlace Place;
        //    public string _SellType;
        //    public double Amount;
        //    public ConsumeUnit _ConsumeUnit;
        //    public double SellPrice;
        //    public string Notes;
        //    public SellOPR(uint SellOPRID_, BillSell BillIN_, MaintenanceOPR MaintenanceOPR_, BuyOPR BuyOPR_, TradeStorePlace Place_, string  SellType_, double Amount_, ConsumeUnit ConsumeUnit_, double SellPrice_,string Notes_)
        //    {
        //        SellOPRID = SellOPRID_;
        //        _BillSell = BillIN_;
        //        _MaintenanceOPR = MaintenanceOPR_;
        //        _BuyOPR = BuyOPR_;
        //        Place = Place_;
        //        _SellType = SellType_;
        //        Amount = Amount_;
        //        _ConsumeUnit = ConsumeUnit_;
        //        SellPrice = SellPrice_;
        //        Notes = Notes_;

        //    }

        
        public class Contact
        {
            public const bool CONTACT_PERSON = false;
            public const bool CONTACT_COMPANY = true;

            public uint ContactID;
            public bool ContactType;
            public  string ContactName;
            public string Phone;
            public string Mobile;
            public string Address;
            public Contact(uint ContactID_,bool ContactType_, string ContactName_, string Phone_, string Mobile_, string Address_)
            {
                ContactID = ContactID_;
                ContactType = ContactType_;
                ContactName = ContactName_;
                Phone = Phone_;
                Mobile = Mobile_;
                Address = Address_;
            }
            public string GetContactTypeString()
            {
                if (ContactType == Contact.CONTACT_COMPANY ) return "شركة";
                else return "شخص";
            }
            public string GetContactTypeHeader()
            {
                if (ContactType == Contact.CONTACT_COMPANY) return "شركة";
                else return "السيد";
            }
            public static  string ConvertTypeToString(bool type)
            {
                if (type == Contact.CONTACT_COMPANY) return "شركة";
                else return "شخص";
            }
            public string Get_Complete_ContactName_WithHeader()
            {
                return GetContactTypeHeader()+":" + ContactName;
            }
        }
        public class TradeState
        {
            public uint TradeStateID;
            public string TradeStateName;

            public TradeState(uint TradeStateID_, string TradeStateName_)
            {
                TradeStateID = TradeStateID_;
                TradeStateName = TradeStateName_;

            }
        }
        public class SellType
        {
            public uint SellTypeID;
            public string SellTypeName;
            public SellType(uint SellTypeID_, string SellTypeName_)
            {
                SellTypeID = SellTypeID_;
                SellTypeName = SellTypeName_;
            }

        }
        public class TradeStoreContainer
        {
            public uint ContainerID;
            public string ContainerName;
            public uint? ParentContainerID;
            public string Desc;
            public TradeStoreContainer (uint ContainerID_,string ContainerName_,uint? ParentContainerID_,string Desc_)
            {
                ContainerID = ContainerID_;
                ContainerName = ContainerName_;
                ParentContainerID = ParentContainerID_;
                Desc = Desc_;
            }
        }
        public class PlaceAvailbeItems_ItemDetails
        {
            public uint ItemID;
            public string ItemName;
            public string ItemCompany;
            public string ItemFolder;
            public string AvailableItemStates;
            public PlaceAvailbeItems_ItemDetails(uint ItemID_, string ItemName_, string ItemCompany_, string ItemFolder_, string AvailableItemStates_ )
            {
                      ItemID= ItemID_;
              ItemName= ItemName_;
              ItemCompany= ItemCompany_;
              ItemFolder= ItemFolder_;
              AvailableItemStates= AvailableItemStates_;
            }
        }

        public class PlaceAvailbeItems_ItemINDetails
        {
            public TradeStorePlace Place;
            public Item _Item;
            public string ItemStoreType;
            public uint StoreType;
            public uint OprID;
            public string ParentOperationDesc;
            public uint ParentOperationID;
            public string consumeunitname;
            public double StoredAmount;
            public double SpentAmount;
            public double AvailableAmount;
            public PlaceAvailbeItems_ItemINDetails(   
             TradeStorePlace Place_,
             Item Item_,
              string ItemStoreType_,
              uint StoreType_,
               uint OprID_,
             string ParentOperationDesc_,
             uint ParentOperationID_,
             string consumeunitname_,
             double StoredAmount_,
             double SpentAmount_,
             double AvailableAmount_)
            {
                  Place=Place_ ;
              _Item=Item_ ;
                ItemStoreType = ItemStoreType_;
                StoreType = StoreType_;
                OprID = OprID_;
                ParentOperationDesc = ParentOperationDesc_;
              ParentOperationID = ParentOperationID_;
              consumeunitname= consumeunitname_;
              StoredAmount= StoredAmount_;
              SpentAmount =SpentAmount_ ;
              AvailableAmount =AvailableAmount_ ;
        }
        }
        public class AvailbeItems_ItemINDetails
        {
            public ItemIN _ItemIN;
            public double SpentAmount;
            public double AvailableAmount;
            public AvailbeItems_ItemINDetails(
            ItemIN ItemIN_,
             double SpentAmount_,
             double AvailableAmount_)
            {
                _ItemIN  = ItemIN_ ; 
                SpentAmount = SpentAmount_;
                AvailableAmount = AvailableAmount_;
            }
        }
        public class TradeStorePlace
        {
            public uint PlaceID;
            public string PlaceName;
            public TradeStoreContainer _TradeStoreContainer;
            public string Desc;
            public TradeStorePlace(uint PlaceID_, string PlaceName_, TradeStoreContainer TradeStoreContainer_ ,string Desc_)
            {
                PlaceID = PlaceID_;
                PlaceName = PlaceName_;
                _TradeStoreContainer = TradeStoreContainer_;
                Desc = Desc_;
            }

            internal string GetPlaceInfo()
            {
                if (_TradeStoreContainer == null) return PlaceName;
                return  _TradeStoreContainer.ContainerName + " : " + PlaceName;
            }
        }
        public class TradeItemStore
        {
            public const uint ITEMIN_STORE_TYPE = 0;
            public const uint MAINTENANCE_ITEM_STORE_TYPE = 1;
            public const uint MAINTENANCE_ACCESSORIES_ITEM_STORE_TYPE = 2;
            ///// 
            public TradeStorePlace _TradeStorePlace;
            public uint ItemSourceOPRID;
            public uint StoreType;
            public double   Amount;
            public ConsumeUnit _ConsumeUnit;
            public TradeItemStore ( TradeStorePlace TradeStorePlace_, uint ItemSourceOPRID_, uint StoreType_, double  Amount_, ConsumeUnit ConsumeUnit_)
            {

                _TradeStorePlace = TradeStorePlace_;
                ItemSourceOPRID = ItemSourceOPRID_;
                StoreType = StoreType_;
                Amount = Amount_;
                _ConsumeUnit = ConsumeUnit_;
            }
        }
        ////////////////////////
       

    }
}
