using System.ComponentModel;

namespace prjVegetable.Models
{
    public class COrderListWrap
    {
        private TOrderList _orderList = null;
        public TOrderList orderList
        {
            get { return _orderList; }
            set { _orderList = value; }
        }
        public COrderListWrap()
        {
            _orderList = new TOrderList();
        }
        public int FId
        {
            get { return _orderList.FId; }
            set { _orderList.FId = value; }
        }
        [DisplayName("訂單編號")]
        public int FOrderId
        {
            get { return _orderList.FOrderId; }
            set { _orderList.FOrderId = value; }
        }
        [DisplayName("商品編號")]
        public int FProductId
        {
            get { return _orderList.FProductId; }
            set { _orderList.FProductId = value; }
        }

        [DisplayName("單價")]
        public int FPrice
        {
            get { return _orderList.FPrice; }
            set { _orderList.FPrice = value; }
        }
        [DisplayName("數量")]
        public int FCount
        {
            get { return _orderList.FCount; }
            set { _orderList.FCount = value; }
        }
        [DisplayName("總計")]
        public int FSum
        {
            get { return _orderList.FSum; }
            set { _orderList.FSum = value; }
        }


        public int OrderStatus { get; set; }
        public bool HasComment { get; set; }
    }
}
