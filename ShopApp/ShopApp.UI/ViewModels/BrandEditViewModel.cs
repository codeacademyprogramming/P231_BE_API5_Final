using System.ComponentModel.DataAnnotations;

namespace ShopApp.UI.ViewModels
{
    public class BrandEditViewModel
    {
        [Required]
        [MaxLength(35)]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
