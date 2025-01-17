namespace prjVegetable.Models
{
    public class CCartWrap
    {
        public TPerson person { get; set; }


        private TCart _cart = null;
        public TCart Cart
        {
            get { return _cart; }
            set { _cart = value; }
        }

        // 新增一個使用者屬性
        public TPerson Person { get; set; }

        public CCartWrap()
        {
            _cart = new TCart();
        }

        public int FId
        {
            get { return _cart.FId; }
            set { _cart.FId = value; }
        }

        public int FProductId
        {
            get { return _cart.FProductId; }
            set { _cart.FProductId = value; }
        }

        public int FCount
        {
            get { return _cart.FCount; }
            set { _cart.FCount = value; }
        }

        public int FPersonId
        {
            get { return _cart.FPersonId; }
            set { _cart.FPersonId = value; }
        }

        // 價格(從 TProduct 撈)
        public int FPrice 
        { 
            get; 
            set; 
        }
        public string FProductName
        {
            get;
            set;
        }
        
        public int FTotalPrice { get; set; }
        // 圖片檔名 (從 TImg 撈)
        public string FName 
        { 
            get; 
            set; 
        }

    }

    
}
