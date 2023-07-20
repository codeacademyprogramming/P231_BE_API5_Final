namespace ShopApp.UI.ViewModels
{
    public class ProductCreateViewModel
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }  
        public decimal CostPrice { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
