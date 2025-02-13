using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjVegetable.Models
{
    public class CCommentWrap
    {
        private TComment _comment = null;

        public TComment comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public CCommentWrap()
        { 
            _comment = new TComment();
        }


        public int FId 
        {
            get { return _comment.FId; }
            set {_comment.FId = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("會員Id")]
        public int FPersonId
        {
            get { return _comment.FPersonId; }
            set { _comment.FPersonId = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("商品Id")]
        public int FProductId
        {
            get { return _comment.FProductId; }
            set { _comment.FProductId = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("訂單細項Id")]
        public int FOrderListId
        {
            get { return _comment.FOrderListId; }
            set { _comment.FOrderListId = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("評論")]
        public string? FComment
        {
            get { return _comment.FComment; }
            set { _comment.FComment = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("評分")]
        public int FStar 
        {
            get { return _comment.FStar; }
            set { _comment.FStar = value; }
        }

        [Required(ErrorMessage = "必填")]
        [DisplayName("評論日期")]
        public DateOnly FCreatedAt
        {
            get { return _comment.FCreatedAt; }
            set { _comment.FCreatedAt = value; }
        }
    }
}
