using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.ViewModels
{
    public class CGoodsInAndOutViewModel
    {
        public TGoodsInAndOut GoodsInAndOut { get; set; } = new TGoodsInAndOut();
        public List<TGoodsInAndOutDetail> GoodsInAndOutDetails { get; set; } = new List<TGoodsInAndOutDetail>();
    }
}
