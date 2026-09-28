using System.ComponentModel.DataAnnotations;

namespace MyProject.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        [Range(0, 100)]
        public decimal Discount { get; set; }
    }
}