namespace prjVegetable.Models
{
    public class CInventoryAdjustmentWrap
    {
        private TInventoryAdjustment _inventoryAdjustment = null;

        public TInventoryAdjustment InventoryAdjustment
        {
            get { return _inventoryAdjustment; }
            set { _inventoryAdjustment = value; }
        }

        public CInventoryAdjustmentWrap()
        {
            _inventoryAdjustment = new TInventoryAdjustment();
        }

        public int FId
        {
            get { return _inventoryAdjustment.FId; }
            set { _inventoryAdjustment.FId = value; }
        }

        public DateOnly FAdjustmentDate
        {
            get { return _inventoryAdjustment.FadjustmentDate; }
            set { _inventoryAdjustment.FadjustmentDate = value; }
        }

        public DateOnly FCreatedAt
        {
            get { return _inventoryAdjustment.FCreatedAt; }
            set { _inventoryAdjustment.FCreatedAt = value; }
        }

        public int FEditor
        {
            get { return _inventoryAdjustment.FEditor; }
            set { _inventoryAdjustment.FEditor = value; }
        }

        public string FNote
        {
            get { return _inventoryAdjustment.FNote; }
            set { _inventoryAdjustment.FNote = value; }
        }

        public int? FCheckerId
        {
            get { return _inventoryAdjustment.FCheckerId; }
            set { _inventoryAdjustment.FCheckerId = value; }
        }

        public DateOnly? FAdjustmentStartDate { get; set; }
        public DateOnly? FAdjustmentEndDate { get; set; }

        public DateOnly? FCreatedAtStartDate { get; set; }
        public DateOnly? FCreatedAtEndDate { get; set; }
    }

}
