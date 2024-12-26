namespace prjVegetable.Models
{
    public class CImgWrap
    {

        public TImg _img = null;

        public TImg img
        {
            get { return _img; }
            set { _img = value; }
        }

        public CImgWrap()
        {
            _img = new TImg();
        }

        public int FImgId
        {
            get { return img.FImgId; }
            set { img.FImgId = value; }
        }

        public int FProductId
        {
            get { return img.FProductId; }
            set { img.FProductId = value; }
        }

        public string FImgName
        {
            get { return img.FImgName; }
            set { img.FImgName = value; }
        }

        public int FImgOrder
        {
            get { return img.FImgOrder; }
            set { img.FImgOrder = value; }
        }

        public DateTime? FImgCreatedAt
        {
            get { return img.FImgCreatedAt; }
            set { img.FImgCreatedAt = value; }
        }

        public string FImgEditer
        {
            get { return img.FImgEditer; }
            set { img.FImgEditer = value; }
        }

        public virtual TProduct FProduct { get; set; } = null!;
    }
}
