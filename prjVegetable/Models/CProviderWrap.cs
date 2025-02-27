using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayName("名稱")]
        [Required(ErrorMessage = "必填欄位")]
        public string FName
        {
            get { return _provider.FName; }
            set { _provider.FName = value; }
        }

        [DisplayName("統編")]
        [Required(ErrorMessage = "必填欄位")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "統一編號必須是 8 位數字")]
        public string FUbn//統編
        {
            get { return _provider.FUbn; }
            set { _provider.FUbn = value; }
        }

        [DisplayName("電話")]
        [Required(ErrorMessage = "必填欄位")]
        [RegularExpression(@"^\d{8,10}$", ErrorMessage = "電話號碼必須是8到10位數字")]
        public string FTel
        {
            get { return _provider.FTel; }
            set { _provider.FTel = value; }
        }

        [DisplayName("聯絡人")]
        public string FConnect
        {
            get { return _provider.FConnect; }
            set { _provider.FConnect = value; }
        }

        [DisplayName("會計")]
        public string FAccountant
        {
            get { return _provider.FAccountant; }
            set { _provider.FAccountant = value; }
        }

        [DisplayName("地址")]
        [Required(ErrorMessage = "必填欄位")]
        public string FAddress
        {
            get { return _provider.FAddress; }
            set { _provider.FAddress = value; }
        }

        [DisplayName("送貨地址")]
        public string FDelivery
        {
            get { return _provider.FDelivery; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _provider.FDelivery = _provider.FAddress;
                }
                else 
                {
                    _provider.FDelivery = value;
                }
            }
        }

        [DisplayName("統編地址")]
        public string FInvoiceAdd
        {
            get { return _provider.FInvoiceAdd; }
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _provider.FInvoiceAdd = _provider.FAddress;
                }
                else
                {
                    _provider.FInvoiceAdd = value;
                }
            }
        }

        [DisplayName("資料修改人")]
        public int FEditor
        {
            get { return _provider.FEditor; }
            set { _provider.FEditor = value; }
        }
    }
}
