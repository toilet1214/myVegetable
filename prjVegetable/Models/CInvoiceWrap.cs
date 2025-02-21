using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace prjVegetable.Models
{
    public class CInvoiceWrap 
    {
        //use gobal variable
        private TInvoice _tInvoice ;

        public TInvoice TInvoice
        {
            get { return _tInvoice; }
            set { _tInvoice = value; }
        }
        public CInvoiceWrap()
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
        public string FNumber
        {
            get { return _tInvoice.FNumber; }
            set { _tInvoice.FNumber = value; }
        }
        [DisplayName("發票日期")]
        public DateTime FDate
        {
            get { return _tInvoice.FDate; }
            set { _tInvoice.FDate = value; }
        }
        [DisplayName("發票格式")]
        public string FForm
        {
            get { return _tInvoice.FForm; }
            set { _tInvoice.FForm = value; }
        }
        [DisplayName("會員編號")]
        public int FCustomerId
        {
            get { return _tInvoice.FCustomerId; }
            set { _tInvoice.FCustomerId = value; }
        }
        [DisplayName("會員統編")]
        public string FCustomerUbn
        {
            get { return _tInvoice.FCustomerUbn; }
            set { _tInvoice.FCustomerUbn = value; }
        }
        [DisplayName("供應商名稱")]
        public int FProviderId
        {
            get { return _tInvoice.FProviderId; }
            set { _tInvoice.FProviderId = value; }
        }
        [DisplayName("供應商統編")]
        public string FProviderUbn
        {
            get { return _tInvoice.FProviderUbn; }
            set { _tInvoice.FProviderUbn = value; }
        }
        [DisplayName("銷項或進項")]
        public int FInOut
        {
            get { return _tInvoice.FInOut; }
            set { _tInvoice.FInOut = value; }
        }
        [DisplayName("狀態")]
        public int FStatus
        {
            get { return _tInvoice.FStatus; }
            set { _tInvoice.FStatus = value; }
        }

        [DisplayName("總額")]
        public int FTotal
        {
            get { return _tInvoice.FTotal; }
            set { _tInvoice.FTotal = value; }
        }

        [DisplayName("修改人")]
        public int FEditor
        {
            get { return _tInvoice.FEditor; }
            set { _tInvoice.FEditor = value; }
        }

        //----------顯示文字使用-------------------------------------------------
        // 新增轉換後的屬性
        [DisplayName("發票格式(顯示)")]
        public string FFormText
        {
            get { return _tInvoice.FForm == "0" ? "二連" : "三連"; }
        }

        // 新增轉換後的屬性
        [DisplayName("銷項或進項(顯示)")]
        public string FInOutText
        {
            get { return _tInvoice.FInOut == 0 ? "進項" : "銷項"; }
        }

        // 新增轉換後的屬性
        [DisplayName("狀態(顯示)")]
        public string FStatusText
        {
            get { return _tInvoice.FStatus == 0 ? "一般" : "作廢"; }
        }

        [DisplayName("供應商名稱(顯示)")]
        public string? FProviderName { get; set; }
    }
}
