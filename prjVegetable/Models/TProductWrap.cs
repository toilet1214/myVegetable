using System.ComponentModel;

namespace prjVegetable.Models
{
    public class TProductWrap
    {
        private TProduct _product = null;

        public TProduct product
        {
            get { return _product; }
            set { _product = value; }
        }

        public TProductWrap()
        {
            _product = new TProduct();
        }
        public int FId
        {
            get { return _product.FId; }
            set { _product.FId = value; }
        }
        [DisplayName("產品名稱")]
        public string FName
        {
            get { return _product.FName; }
            set { _product.FName = value; }
        }

        [DisplayName("產品分類")]        
        public string FClassification
        {
            get { return _product.FClassification; }
            set { _product.FClassification = value; }
        }

        [DisplayName("產品價格")]
        public int FPrice
        {
            get { return _product.FPrice; }
            set { _product.FPrice = value; }
        }

        [DisplayName("產品描述")]
        public string FDescription
        {
            get { return _product.FDescription; }
            set { _product.FDescription = value; }
        }

        [DisplayName("庫存數量")]
        public int FQuantity
        {
            get { return _product.FQuantity; }
            set { _product.FQuantity = value; }
        }

        [DisplayName("上架日期")]
        public DateTime FLaunchAt
        {
            get { return _product.FLaunchAt; }
            set { _product.FLaunchAt = value; }
        }

        [DisplayName("藏溫方式")]
        public string FStorage
        {
            get { return _product.FStorage; }
            set { _product.FStorage = value; }
        }

        [DisplayName("產地")]
        public string FOrigin
        {
            get { return _product.FOrigin; }
            set { _product.FOrigin = value; }
        }

        [DisplayName("修改人")]
        public string FEditer
        {
            get { return _product.FEditer; }
            set { _product.FEditer = value; }
        }
    }
}
