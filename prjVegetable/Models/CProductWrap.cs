using System.ComponentModel.DataAnnotations.Schema;

namespace prjVegetable.Models
{
    public class CProductWrap
    {
        public TProduct _product = null;

        public TProduct product
        {
            get { return _product; }
            set { _product = value; }
        }

        public CProductWrap()
        {
            _product = new TProduct();
        }


        public int FProductId
        {
            get { return _product.FProductId; }
            set { _product.FProductId = value; }
        }

        public string FProductName
        {
            get { return _product.FProductName; }
            set { _product.FProductName = value; }
        }
        public string FProductClassification
        {
            get { return _product.FProductClassification; }
            set { _product.FProductClassification = value; }
        }

        public int FProductPrice
        {
            get { return _product.FProductPrice; }
            set { _product.FProductPrice = value; }
        }

        public string FProductDescription
        {
            get { return _product.FProductDescription; }
            set { _product.FProductDescription = value; }
        }

        public int FProductQuantity
        {
            get { return _product.FProductQuantity; }
            set { _product.FProductQuantity = value; }
        }

        public DateTime FProductCreatedAt
        {
            get { return _product.FProductCreatedAt; }
            set { _product.FProductCreatedAt = value; }
        }

        public string FProductStorage
        {
            get { return _product.FProductStorage; }
            set { _product.FProductStorage = value; }
        }

        public string FProductOrigin
        {
            get { return _product.FProductOrigin; }
            set { _product.FProductOrigin = value; }
        }

        public string FProductEditer
        {
            get { return _product.FProductEditer; }
            set { _product.FProductEditer = value; }
        }

        public virtual ICollection<TImg> TImgs { get; set; } = new List<TImg>();

        [NotMapped]
        //public TImg TImg { get; set; }
        public string FImgName
        {
            get { return _product.FImgName; }
            set { _product.FImgName = value; }
        }

    }
}
