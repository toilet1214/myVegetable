using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace prjVegetable.Models
{
    public class CPurchaseDetailWrap 
    {
        //----define the value
        private TPurchaseDetail _PurchaseDetail;
        public TPurchaseDetail PurchaseDetail
        {
            get { return _PurchaseDetail; }
            set { _PurchaseDetail = value; }
        }

        //------save the data by global variable
        public CPurchaseDetailWrap()
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
        public int FPurchaseId
        {
            get { return _PurchaseDetail.FPurchaseId; }
            set { _PurchaseDetail.FPurchaseId = value; }
        }

        [DisplayName("產品ID")]
        public int FProductId
        {
            get { return _PurchaseDetail.FProductId; }
            set { _PurchaseDetail.FProductId = value; }
        }


        [DisplayName("數量")]
        public int FCount
        {
            get { return _PurchaseDetail.FCount; }
            set { _PurchaseDetail.FCount = value; }
        }


        [DisplayName("單價")]
        public int FPrice
        {
            get { return _PurchaseDetail.FPrice; }
            set { _PurchaseDetail.FPrice = value; }
        }

        [DisplayName("小計")]
        public int FSum
        {
            get { return _PurchaseDetail.FSum; }
            set { _PurchaseDetail.FSum = value; }
        }
    }
}
