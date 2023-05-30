using System.ComponentModel.DataAnnotations;

namespace FullstackStoreAPI.Models.DTO
{
    public class MenuItemUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpecialTag { get; set; }
        public string Category { get; set; }
        [Range(1, double.MaxValue)]
        public double Price { get; set; }
        public IFormFile File { get; set; }
    }
}
