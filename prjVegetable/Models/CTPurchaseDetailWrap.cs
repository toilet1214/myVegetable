using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace team_20241228.Models
{
    public class CTPurchaseDetailWrap 
    {
        //----define the value
        private TPurchaseDetail _PurchaseDetail;
        public TPurchaseDetail PurchaseDetail
        {
            get { return _PurchaseDetail; }
            set { _PurchaseDetail = value; }
        }

        //------save the data by global variable
        public CTPurchaseDetailWrap()
        {
            _PurchaseDetail = new TPurchaseDetail();
        }

        //--------------------------------------
        [DisplayName("採購明細號碼")]
        public int FId
        {
            get { return _PurchaseDetail.FId; }
            set { _PurchaseDetail.FId = value; }
        }

        [DisplayName("採購號碼")]
        public string? FPurchaseId
        {
            get { return _PurchaseDetail.FPurchaseId; }
            set { _PurchaseDetail.FPurchaseId = value; }
        }

        [DisplayName("品名")]
        public string? FProductName
        {
            get { return _PurchaseDetail.FProductName; }
            set { _PurchaseDetail.FProductName = value; }
        }

        [DisplayName("數量")]
        public int? FConut
        {
            get { return _PurchaseDetail.FConut; }
            set { _PurchaseDetail.FConut = value; }
        }


        [DisplayName("單價")]
        public int? FPrice
        {
            get { return _PurchaseDetail.FPrice; }
            set { _PurchaseDetail.FPrice = value; }
        }

        [DisplayName("小計")]
        public int? FSum
        {
            get { return _PurchaseDetail.FSum; }
            set { _PurchaseDetail.FSum = value; }
        }
    }
}
