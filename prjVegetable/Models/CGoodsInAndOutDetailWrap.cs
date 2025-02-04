namespace prjVegetable.Models
{
    public class CGoodsInAndOutDetailWrap
    {
        private TGoodsInAndOutDetail _GoodsInAndOutDetail = null;
        public TGoodsInAndOutDetail GoodsInAndOutDetail
        {
            get { return _GoodsInAndOutDetail; }
            set { _GoodsInAndOutDetail = value; }
        }

        public CGoodsInAndOutDetailWrap()
        {
            _GoodsInAndOutDetail = new TGoodsInAndOutDetail();
        }
        public int FId
        {
            get { return _GoodsInAndOutDetail.FId; }
            set { _GoodsInAndOutDetail.FId = value; }
        }

        public int FGoodsInandOutId
        {
            get { return _GoodsInAndOutDetail.FGoodsInandOutId; }
            set { _GoodsInAndOutDetail.FGoodsInandOutId = value; }
        }

        public int FProductId
        {
            get { return _GoodsInAndOutDetail.FProductId; }
            set { _GoodsInAndOutDetail.FProductId = value; }
        }
        public int FCount
        {
            get { return _GoodsInAndOutDetail.FCount; }
            set { _GoodsInAndOutDetail.FCount = value; }
        }
        public int FPrice
        {
            get { return _GoodsInAndOutDetail.FPrice; }
            set { _GoodsInAndOutDetail.FPrice = value; }
        }

        public int FSum
        {
            get { return _GoodsInAndOutDetail.FSum; }
            set { _GoodsInAndOutDetail.FSum = value; }
        }
    }
}
