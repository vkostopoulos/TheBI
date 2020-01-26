using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Wms
{
    public class ProductDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Store is required")]
        public string StoreId { get; set; }

        public string Barcode { get; set; }

        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Invoice is required")]
        public string InvoiceId { get; set; }

        public double? Stock { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Sell price is required")]
        public double? SPrice { get; set; }

        public double? Discount { get; set; }

        public List<AttributeDto> Attributes { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string CategoryId { get; set; }

        public bool? IsEnabled { get; set; }

    }
}