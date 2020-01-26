using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Wms
{
    public class InventoryMovementDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "From Store is required")]
        public string FromStoreId { get; set; }

        [Required(ErrorMessage = "To Store is required")]
        public string ToStoreId { get; set; }

        public string ProductCode { get; set; }
    }
}