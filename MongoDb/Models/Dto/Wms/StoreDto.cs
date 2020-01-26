using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Wms
{
    public class StoreDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool? IsEnabled { get; set; }
    }
}