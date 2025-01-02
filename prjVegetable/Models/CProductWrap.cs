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


        public int FId
        {
            get { return _product.FId; }
            set { _product.FId = value; }
        }

        public string FName
        {
            get { return _product.FName; }
            set { _product.FName = value; }
        }
        public string FClassification
        {
            get { return _product.FClassification; }
            set { _product.FClassification = value; }
        }

        public int FPrice
        {
            get { return _product.FPrice; }
            set { _product.FPrice = value; }
        }

        public string FDescription
        {
            get { return _product.FDescription; }
            set { _product.FDescription = value; }
        }

        public int FQuantity
        {
            get { return _product.FQuantity; }
            set { _product.FQuantity = value; }
        }

        public DateTime FLaunchAt
        {
            get { return _product.FLaunchAt; }
            set { _product.FLaunchAt = value; }
        }

        public string FStorage
        {
            get { return _product.FStorage; }
            set { _product.FStorage = value; }
        }

        public string FOrigin
        {
            get { return _product.FOrigin; }
            set { _product.FOrigin = value; }
        }

        public string FEditor
        {
            get { return _product.FEditor; }
            set { _product.FEditor = value; }
        }


        public string FImgName
        { get;set; }

        public List<string> ImgList { get; set; }
    }
}
