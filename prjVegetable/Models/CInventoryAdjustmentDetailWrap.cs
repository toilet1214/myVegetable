namespace prjVegetable.Models
{
    public class CInventoryAdjustmentDetailWrap
    {
        private TInventoryAdjustmentDetail _inventoryAdjustmentDetail = null;

        public TInventoryAdjustmentDetail InventoryAdjustmentDetail
        {
            get { return _inventoryAdjustmentDetail; }
            set { _inventoryAdjustmentDetail = value; }
        }

        public CInventoryAdjustmentDetailWrap()
        {
            _inventoryAdjustmentDetail = new TInventoryAdjustmentDetail();
        }

        public int FId
        {
            get { return _inventoryAdjustmentDetail.FId; }
            set { _inventoryAdjustmentDetail.FId = value; }
        }

        public int FInventoryAdjustmentId
        {
            get { return _inventoryAdjustmentDetail.FInventoryAdjustmentId; }
            set { _inventoryAdjustmentDetail.FInventoryAdjustmentId = value; }
        }

        public int FProductId
        {
            get { return _inventoryAdjustmentDetail.FProductId; }
            set { _inventoryAdjustmentDetail.FProductId = value; }
        }

        public int FQuantity
        {
            get { return _inventoryAdjustmentDetail.FQuantity; }
            set { _inventoryAdjustmentDetail.FQuantity = value; }
        }

        public decimal FCost
        {
            get { return _inventoryAdjustmentDetail.FCost; }
            set { _inventoryAdjustmentDetail.FCost = value; }
        }
    }
}
