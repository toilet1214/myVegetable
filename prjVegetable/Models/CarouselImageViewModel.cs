namespace prjVegetable.Models
{
    public class CarouselImageViewModel
    {
        public List<string> ImagePaths { get; set; }
        public IFormFile Image1 { get; set; }
        public IFormFile Image2 { get; set; }
        public IFormFile Image3 { get; set; }
    }
}
