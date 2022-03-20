using System.ComponentModel.DataAnnotations;

namespace TektonApi.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = CommonResponses.Required)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }

        [Required(ErrorMessage = CommonResponses.Required)]
        public decimal Price { get; set; }
    }
}
