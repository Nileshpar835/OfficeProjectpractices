using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SimpleMvcApp.Models
{
    public class Product
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]


        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is Required")]
        [Range(100,10000, ErrorMessage = "Price must be between 100 and 10000.")]

        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is Required")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters.")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "In Stock")]
        public bool IsInStock { get; set; }

    }
}
