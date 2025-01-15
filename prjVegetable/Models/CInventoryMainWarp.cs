namespace prjVegetable.Models
{
    public class CInventoryMainWrap
    {
        private TInventoryMain _inventoryMain = null;

        public TInventoryMain InventoryMain
        {
            get { return _inventoryMain; }
            set { _inventoryMain = value; }
        }

        public CInventoryMainWrap()
        {
            _inventoryMain = new TInventoryMain();
        }

        public int FId
        {
            get { return _inventoryMain.FId; }
            set { _inventoryMain.FId = value; }
        }

        public string FInventoryNo
        {
            get { return _inventoryMain.FInventoryNo; }
            set { _inventoryMain.FInventoryNo = value; }
        }

        public DateOnly FBaselineDate
        {
            get { return _inventoryMain.FBaselineDate; }
            set { _inventoryMain.FBaselineDate = value; }
        }

        public DateTime FCreatedAt
        {
            get { return _inventoryMain.FCreatedAt; }
            set { _inventoryMain.FCreatedAt = value; }
        }

        public string FCreatorId
        {
            get { return _inventoryMain.FCreatorId; }
            set { _inventoryMain.FCreatorId = value; }
        }

        public string? FEditorId
        {
            get { return _inventoryMain.FEditorId; }
            set { _inventoryMain.FEditorId = value; }
        }
    }

}
