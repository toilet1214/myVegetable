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

        public string FInventoryNo
        {
            get { return _inventoryDetail.FInventoryNo; }
            set { _inventoryDetail.FInventoryNo = value; }
        }

        public int FProductId
        {
            get { return _inventoryDetail.FProductId; }
            set { _inventoryDetail.FProductId = value; }
        }

        public string FProductName
        {
            get { return _inventoryDetail.FProductName; }
            set { _inventoryDetail.FProductName = value; }
        }

        public int FSystemQuantity
        {
            get { return _inventoryDetail.FSystemQuantity; }
            set { _inventoryDetail.FSystemQuantity = value; }
        }

        public int? FActualQuantity
        {
            get { return _inventoryDetail.FActualQuantity; }
            set { _inventoryDetail.FActualQuantity = value; }
        }

        public int? FDifferenceQuantity
        {
            get { return _inventoryDetail.FDifferenceQuantity; }
            set { _inventoryDetail.FDifferenceQuantity = value; }
        }

        public int FAmount
        {
            get { return _inventoryDetail.FAmount; }
            set { _inventoryDetail.FAmount = value; }
        }

        public string? FRemark
        {
            get { return _inventoryDetail.FRemark; }
            set { _inventoryDetail.FRemark = value; }
        }

        public string? FEditorId
        {
            get { return _inventoryDetail.FEditorId; }
            set { _inventoryDetail.FEditorId = value; }
        }
    }
}
