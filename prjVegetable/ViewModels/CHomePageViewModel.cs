namespace prjVegetable.ViewModels
{
    public class CHomePageViewModel
    {
        public List<CarouselCardViewModel> TopProducts { get; set; } = new();
        public CarouselImageViewModel UploadImages { get; set; } = new();

        public class CarouselCardViewModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public string ProductDescription { get; set; } = string.Empty;
            public string ImagePath { get; set; } = string.Empty;
        }

        public class CarouselImageViewModel
        {
            public List<string> ImagePaths { get; set; }
            public IFormFile Image1 { get; set; }
            public IFormFile Image2 { get; set; }
            public IFormFile Image3 { get; set; }
            public IFormFile Image4 { get; set; }
            public IFormFile Image5 { get; set; }
        }
        
    }

}
