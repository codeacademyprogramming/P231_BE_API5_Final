using System.ComponentModel.DataAnnotations;

namespace ShopApp.UI.ViewModels
{
    public class BrandCreateViewModel
    {
        [Required]
        [MaxLength(35)]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
