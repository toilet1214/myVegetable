namespace prjVegetable.Models
{
    public class CCartWrap
    {
        private TCart _cart = null;
        public TCart Cart
        {
            get { return _cart; }
            set { _cart = value; }
        }
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

        public string FProductName 
        { 
            get { return _cart.FProductName; } 
            set { _cart.FProductName = value; } 
        }

        public int FCount 
        { 
            get { return _cart.FCount; } 
            set { _cart.FCount = value; } 
        }

        public int FPrice 
        { 
            get { return _cart.FPrice; } 
            set { _cart.FPrice = value; } 
        }

        public int FImgId
        { 
            get { return _cart.FImgId; } 
            set { _cart.FImgId = value; } 
        }

        public int FBuyerId
        {
            get { return _cart.FBuyerId; }
            set { _cart.FBuyerId = value; }
        }
        public int TotalPrice
        {
            get
            {
                return FPrice * FCount; // 總價格 = 單價 * 數量
            }
        }
    }
}
