using System.ComponentModel;

namespace prjVegetable.Models
{
    public class CProviderWrap
    {
        private TProvider _provider = null;
        public TProvider provider
        {
            get { return _provider; }
            set { _provider = value; }
        }
        public CProviderWrap()
        {
            _provider = new TProvider();
        }

        [DisplayName("廠商編號")]
        public int FId
        {
            get { return _provider.FId; }
            set { _provider.FId = value; }
        }

        [DisplayName("廠商名稱")]
        public string FName
        {
            get { return _provider.FName; }
            set { _provider.FName = value; }
        }

        [DisplayName("廠商統編")]
        public string FUbn//統編
        {
            get { return _provider.FUbn; }
            set { _provider.FUbn = value; }
        }

        [DisplayName("廠商電話")]
        public string FTel
        {
            get { return _provider.FTel; }
            set { _provider.FTel = value; }
        }

        [DisplayName("廠商聯絡人")]
        public string FConnect
        {
            get { return _provider.FConnect; }
            set { _provider.FConnect = value; }
        }

        [DisplayName("廠商會計")]
        public string FAccountant
        {
            get { return _provider.FAccountant; }
            set { _provider.FAccountant = value; }
        }

        [DisplayName("廠商地址")]
        public string FAddress
        {
            get { return _provider.FAddress; }
            set { _provider.FAddress = value; }
        }

        [DisplayName("廠商送貨地址")]
        public string FDelivery
        {
            get { return _provider.FDelivery; }
            set { _provider.FDelivery = value; }
        }

        [DisplayName("廠商統編地址")]
        public string FInvoiceAdd
        {
            get { return _provider.FInvoiceAdd; }
            set { _provider.FInvoiceAdd = value; }
        }

        [DisplayName("資料修改人")]
        public int FEditor
        {
            get { return _provider.FEditor; }
            set { _provider.FEditor = value; }
        }
    }
}
