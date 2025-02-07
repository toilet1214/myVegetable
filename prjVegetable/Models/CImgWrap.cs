using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [Required (ErrorMessage = "必填")]
        [DisplayName("照片名稱")]
        public string FName
        {
            get { return img.FName; }
            set { img.FName = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("照片順序")]
        public int FOrderBy
        {
            get { return img.FOrderBy; }
            set { img.FOrderBy = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("上傳日期")]
        public DateOnly FUploadAt
        {
            get { return img.FUploadAt; }
            set { img.FUploadAt = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("修改人")]
        public int FEditor
        {
            get { return img.FEditor; }
            set { img.FEditor = value; }
        }

        public virtual TProduct FProduct { get; set; } = null!;
    }
}
