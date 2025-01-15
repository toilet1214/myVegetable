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

        public int FTotalPrice
        {
            get;
            set;
        }

    }
}
