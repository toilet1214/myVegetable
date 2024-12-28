namespace prjVegetable.Models
{
    public class CartItems
    {
        public int Id { get; set; } // 購物車項目 ID
        public int ProductId { get; set; } // 商品 ID
        public string ProductName { get; set; } // 商品名稱
        public int Quantity { get; set; } // 購買數量
        public decimal Price { get; set; } // 單價
        public decimal TotalPrice => Quantity * Price; // 總價 (計算屬性)
        public int ImgId { get; set; } // 商品圖片 ID
        public int BuyerId { get; set; } // 購買者 ID

        // 可選：建構函式
        public CartItems(int id, int productId, string productName, int quantity, decimal price, int imgId, int buyerId)
        {
            Id = id;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            ImgId = imgId;
            BuyerId = buyerId;
        }

        // 無參數建構函式
        public CartItems() { }
    }
}
