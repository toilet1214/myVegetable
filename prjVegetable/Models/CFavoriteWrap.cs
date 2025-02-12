using System.ComponentModel;

namespace prjVegetable.Models
{
    public class CFavoriteWrap
    {
        public TFavorite _favorite = null;

        public TFavorite favorite
        { 
            get { return _favorite; }
            set { _favorite = value; }
        }

        public CFavoriteWrap()
        { 
            _favorite = new TFavorite();
        }

        public int FId 
        {
            get { return favorite.FId; }
            set { favorite.FId = value; }
        }
        [DisplayName("商品名稱")]
        public int FProductId 
        {
            get { return favorite.FProductId; }
            set { favorite.FProductId = value; }
        }

        [DisplayName("會員")]
        public int FPersonId
        {
            get { return favorite.FPersonId; }
            set { favorite.FPersonId = value; }
        }
    }
}
