using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjVegetable.Models
{
    public class CProductWrap
    {
        private TProduct _product = null;

        public TProduct product
        {
            get { return _product; }
            set { _product = value; }
        }

        public CProductWrap()
        {
            _product = new TProduct();
            ImgList = new List<string>();
        }


        public int FId
        {
            get { return _product.FId; }
            set { _product.FId = value; }
        }

        [Required (ErrorMessage = "必填")]
        [DisplayName("商品名稱")]
        public string FName
        {
            get { return _product.FName; }
            set { _product.FName = value; }
        }


        [Required(ErrorMessage = "必填")]
        [DisplayName("商品分類")]
        public string FClassification
        {
            get { return _product.FClassification; }
            set { _product.FClassification = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("商品價格")]
        public int FPrice
        {
            get { return _product.FPrice; }
            set { _product.FPrice = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("商品描述")]
        public string FDescription
        {
            get { return _product.FDescription; }
            set { _product.FDescription = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("庫存數量")]
        public int FQuantity
        {
            get { return _product.FQuantity; }
            set { _product.FQuantity = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("上架日期")]
        public DateOnly FLaunchAt
        {
            get { return _product.FLaunchAt; }
            set { _product.FLaunchAt = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("藏溫方式")]
        public int FStorage
        {
            get { return _product.FStorage; }
            set { _product.FStorage = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("產地")]
        public string FOrigin
        {
            get { return _product.FOrigin; }
            set { _product.FOrigin = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("上架狀態")]
        public int FLaunch
        {
            get { return _product.FLaunch; }
            set { _product.FLaunch = value; }

        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("修改人")]
        public int FEditor
        {
            get { return _product.FEditor; }
            set { _product.FEditor = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("商品照片")]
        public string FImgName
        { get; set; }

        public bool IsFavorite
        { get; set; }

        public bool IsInCart
        { get; set; }


        public List<string> ImgList { get; set; }
    }
}
