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

        public int FId
        {
            get { return img.FId; }
            set { img.FId = value; }
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

        public int FOrderBy
        {
            get { return img.FOrderBy; }
            set { img.FOrderBy = value; }
        }

        public DateTime? FUploadAt
        {
            get { return img.FUploadAt; }
            set { img.FUploadAt = value; }
        }

        public string FEditor
        {
            get { return img.FEditor; }
            set { img.FEditor = value; }
        }

        public virtual TProduct FProduct { get; set; } = null!;
    }
}
