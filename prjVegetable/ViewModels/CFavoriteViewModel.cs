using System.ComponentModel;

namespace prjVegetable.ViewModels
{
    public class CFavoriteViewModel
    {
        [DisplayName("商品名稱")]
        public string Name { get; set; }
        [DisplayName("人氣")]
        public int Popular {  get; set; }
        public int FId { get; set; }
    }
}
