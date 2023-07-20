namespace ShopApp.UI.ViewModels
{
    public class BrandViewModel
    {
        public List<BrandViewModelItem> Brands { get; set; }
    }

    public class BrandViewModelItem
    {
        public int Id { get; set; } 
        public string Name { get; set; }
    }
}
