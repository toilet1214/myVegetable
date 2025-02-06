using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace prjVegetable.Models
{
    public class CPurchaseWrap
    {
        //use gobal variable
        private TPurchase _Purchase;


        //建立變數
        //--------------------------------------
        public TPurchase Purchase
        {
            get { return _Purchase; }
            set { _Purchase = value; }
        }
        public CPurchaseWrap()
        {
            _Purchase = new TPurchase();
        }

        //-----------欄位定義---------------------------------------

        [DisplayName("採購單號")]
        public int FId
        {

            get { return _Purchase.FId; }
            set { _Purchase.FId = value; }
        }

        [DisplayName("採購日")]
        public DateTime FBuyDate
        {

            get { return _Purchase.FBuyDate; }
            set { _Purchase.FBuyDate = value; }
        }

        [DisplayName("供應商id")]
        public int FProviderId
        {
            get { return _Purchase.FProviderId; }
            set { _Purchase.FProviderId = value; }
        }

        [DisplayName("發票格式")]
        public int FInvoiceForm
        {
            get { return _Purchase.FInvoiceForm; }
            set { _Purchase.FInvoiceForm = value; }
        }

        [DisplayName("支付方式")]
        public int FPayment
        {
            get { return _Purchase.FPayment; }
            set { _Purchase.FPayment = value; }
        }

        [DisplayName("資料異動人員")]
        public int FEditor
        {
            get { return _Purchase.FEditor; }
            set { _Purchase.FEditor = value; }
        }


        [DisplayName("採購金額")]
        public int FPreTax
        {
            get { return _Purchase.FPreTax; }
            set { _Purchase.FPreTax = value; }
        }

        [DisplayName("稅")]
        public int FTax
        {
            get { return _Purchase.FTax; }
            set { _Purchase.FTax = value; }
        }

        [DisplayName("總額")]
        public int FTotal
        {
            get { return _Purchase.FTotal; }
            set { _Purchase.FTotal = value; }
        }

        [DisplayName("備註")]
        public string FNote
        {
            get { return _Purchase.FNote; }
            set { _Purchase.FNote = value; }
        }
    }
}
