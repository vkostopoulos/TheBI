using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Wms
{
    public class CategoryDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        [Required(ErrorMessage = "IsSubcategory is required")]
        public bool? IsSubCategory { get; set; }

        public string MainCategoryId { get; set; }

        public bool? IsEnabled { get; set; }

    }
}