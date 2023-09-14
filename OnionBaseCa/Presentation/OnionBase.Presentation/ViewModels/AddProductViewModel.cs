using System.ComponentModel.DataAnnotations;

namespace OnionBase.Presentation.ViewModels
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Açıklama gereklidir.")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string ProductColor { get; set; }

        [Required(ErrorMessage = "Fiyat gereklidir.")]
        public int Stock { get; set; }
        public int ProductCode { get; set; }
        public int Price { get; set; }
    }
}
