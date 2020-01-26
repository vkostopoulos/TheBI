using System.ComponentModel.DataAnnotations;

namespace MongoDb.Models.Dto.Wms
{
    public class AttributeDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool? IsEnabled { get; set; }
    }
}