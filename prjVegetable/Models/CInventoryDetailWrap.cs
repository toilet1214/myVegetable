namespace prjVegetable.Models
{
    public class CInventoryDetailWrap
    {
        private TInventoryDetail _inventoryDetail = null;

        public TInventoryDetail InventoryDetail
        {
            get { return _inventoryDetail; }
            set { _inventoryDetail = value; }
        }

        public CInventoryDetailWrap()
        {
            _inventoryDetail = new TInventoryDetail();
        }

        public int FId
        {
            get { return _inventoryDetail.FId; }
            set { _inventoryDetail.FId = value; }
        }

        public int FInventoryDetailId
        {
            get { return _inventoryDetail.FId; }
            set { _inventoryDetail.FId = value; }
        }

        public int FProductId
        {
            get { return _inventoryDetail.FProductId; }
            set { _inventoryDetail.FProductId = value; }
        }

        public int FSystemQuantity
        {
            get { return (int)_inventoryDetail.FSystemQuantity; }
            set { _inventoryDetail.FSystemQuantity = value; }
        }

        public int? FActualQuantity
        {
            get { return _inventoryDetail.FActualQuantity; }
            set { _inventoryDetail.FActualQuantity = value; }
        }

        public string FName { get; set; }
        public int? DifferenceQuantity
        {
            get
            {
                if (FActualQuantity.HasValue)
                {
                    return FActualQuantity.Value - FSystemQuantity;
                }
                return null;
            }
        }
    }
}
