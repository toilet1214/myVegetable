using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace prjVegetable.Models
{
    public class CInvoiceDetailWrap 
    {
        //----define the value
        private TInvoiceDetail _InvoiceDetail;
        public TInvoiceDetail InvoiceDetail
        {
            get { return _InvoiceDetail; }
            set { _InvoiceDetail = value; }
        }

        //------save the data by global variable
        public CInvoiceDetailWrap()
        {
            _InvoiceDetail = new TInvoiceDetail();
        }

        //--------------------------------------
        public int FId
        {
            get { return _InvoiceDetail.FId; }
            set { _InvoiceDetail.FId = value; }
        }

        [DisplayName("發票號碼")]
        public string? FNumber
        {
            get { return _InvoiceDetail.FNumber; }
            set { _InvoiceDetail.FNumber = value; }
        }
        [DisplayName("品名")]
        public string? FProductName
        {
            get { return _InvoiceDetail.FProductName; }
            set { _InvoiceDetail.FProductName = value; }
        }
        [DisplayName("數量")]
        public int FCount
        {
            get { return _InvoiceDetail.FCount; }
            set { _InvoiceDetail.FCount = value; }
        }
        [DisplayName("單價")]
        public int FPrice
        {
            get { return _InvoiceDetail.FPrice; }
            set { _InvoiceDetail.FPrice = value; }
        }
        [DisplayName("小計")]
        public int FSum
        {
            get { return _InvoiceDetail.FSum; }
            set { _InvoiceDetail.FSum = value; }
        }
    }
}
