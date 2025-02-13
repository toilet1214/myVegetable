using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Services
{
    public class CarouselService
    {
        private readonly DbVegetableContext _context;

        public CarouselService(DbVegetableContext context)
        {
            _context = context;
        }

        /*---------------- 熱門商品輪播 ------------------*/
        public List<CHomePageViewModel.CarouselCardViewModel> GetTop9Products()
        {
            // 計算六個月前的日期
            DateOnly sixMonthsAgo = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6));

            // LINQ 查詢
            var query = (
                from g in _context.TGoodsInAndOuts
                join d in _context.TGoodsInAndOutDetails on g.FId equals d.FGoodsInandOutId
                join p in _context.TProducts on d.FProductId equals p.FId
                join img in _context.TImgs on p.FId equals img.FProductId into imgGroup
                from img in imgGroup.DefaultIfEmpty() // LEFT JOIN，避免產品沒有圖片
                where g.FInOut == 1 && g.FDate.CompareTo(sixMonthsAgo) >= 0
                group new { d, img } by new
                {
                    ProductId = p.FId,
                    ProductName = p.FName,
                    ProductDescription = p.FDescription
                } into grouped
                select new
                {
                    ProductId = grouped.Key.ProductId,
                    Name = grouped.Key.ProductName,
                    Description = grouped.Key.ProductDescription,
                    ImageName = grouped.Min(x => x.img.FName), // 選取最小的圖片名稱
                    TotalCount = grouped.Sum(x => x.d.FCount) // 計算總數量
                }
            )
            .OrderByDescending(x => x.TotalCount) // 按照總數量降序排列
            .Take(9) // 取前 9 筆資料
            .ToList();

            // 轉換成 ViewModel，並生成完整的圖片路徑
            return query.Select(x => new CHomePageViewModel.CarouselCardViewModel
            {
                ProductId = x.ProductId,
                ProductName = x.Name,
                ProductDescription = x.Description,
                ImagePath = x.ImageName != null ? $"/Images/{x.ImageName}" : "/Images/default.jpg" // 預設圖片路徑
            }).ToList();
        }
    }
}
