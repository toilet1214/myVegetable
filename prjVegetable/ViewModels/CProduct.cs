using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CProduct
    {
        public IEnumerable<CProductWrap> CProducts { get; set; }
        public IEnumerable<CImgWrap> CImg { get; set; }
    }
}
