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

        public DateOnly FBaselineDate
        {
            get { return _inventoryMain.FBaselineDate; }
            set { _inventoryMain.FBaselineDate = value; }
        }

        public DateOnly FCreatedAt
        {
            get { return _inventoryMain.FCreatedAt; }
            set { _inventoryMain.FCreatedAt = value; }
        }

        public int FEditor
        {
            get { return _inventoryMain.FEditor; }
            set { _inventoryMain.FEditor = value; }
        }

        public string FNote
        {
            get { return _inventoryMain.FNote; }
            set { _inventoryMain.FNote = value; }
        }
    }
}
