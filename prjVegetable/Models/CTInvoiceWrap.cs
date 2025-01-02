using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace team_20241228.Models
{
    public class CTInvoiceWrap 
    {
        //use gobal variable
        private TInvoice _tInvoice ;

        public TInvoice TInvoice
        {
            get { return _tInvoice; }
            set { _tInvoice = value; }
        }
        public CTInvoiceWrap()
        {
            _tInvoice = new TInvoice();
        }

        //-------------------------------
        [DisplayName("序號")]
        public int FId
        {
            get { return _tInvoice.FId; }
            set { _tInvoice.FId = value; }
        }

        [DisplayName("發票號碼")]
        public string? FInvoiceNumber
        {
            get { return _tInvoice.FNumber; }
            set { _tInvoice.FNumber = value; }
        }
        [DisplayName("發票日期")]
        public DateOnly? FInvoiceDate
        {
            get { return _tInvoice.FDate; }
            set { _tInvoice.FDate = value; }
        }
        [DisplayName("發票格式")]
        public string? FInvoiceForm
        {
            get { return _tInvoice.FForm; }
            set { _tInvoice.FForm = value; }
        }
        [DisplayName("會員編號")]
        public int? FCustomerId
        {
            get { return _tInvoice.FCustomerId; }
            set { _tInvoice.FCustomerId = value; }
        }
        [DisplayName("會員統編")]
        public string? FCustomerUnifiedEdition
        {
            get { return _tInvoice.FCustomerUbn; }
            set { _tInvoice.FCustomerUbn = value; }
        }
        [DisplayName("供應商單號")]
        public int? FSupplierId
        {
            get { return _tInvoice.FSupplierId; }
            set { _tInvoice.FSupplierId = value; }
        }
        [DisplayName("供應商統編")]
        public string? FSupplierUnifiedEdition
        {
            get { return _tInvoice.FSupplierUbn; }
            set { _tInvoice.FSupplierUbn = value; }
        }
        [DisplayName("銷項或進項")]
        public int? FInOut
        {
            get { return _tInvoice.FInOut; }
            set { _tInvoice.FInOut = value; }
        }
        [DisplayName("狀態")]
        public int? FStatus
        {
            get { return _tInvoice.FStatus; }
            set { _tInvoice.FStatus = value; }
        }
        [DisplayName("總額")]
        public int? FTotal
        {
            get { return _tInvoice.FTotal; }
            set { _tInvoice.FTotal = value; }
        }
        [DisplayName("修改人")]
        public string? FEditer
        {
            get { return _tInvoice.FEditor; }
            set { _tInvoice.FEditor = value; }
        }
    }
}
