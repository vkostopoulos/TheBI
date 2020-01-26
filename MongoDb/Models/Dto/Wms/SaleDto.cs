using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Wms
{
    public class SaleDto
    {
        public string Id { get; set; }

        public string ProductCode { get; set; }

        public string ProductBarcode { get; set; }

        public double? ProductDiscount { get; set; }

        [Required(ErrorMessage = "Sale type is required")]
        public ReceiptType? SaleType { get; set; }

        [Required(ErrorMessage = "Sell price is required")]
        public double? ProductSPrice { get; set; }

        [Required(ErrorMessage = "Buy price is required")]
        public double? ProductBPrice { get; set; }

        public List<AttributeDto> ProductAttributes { get; set; }

        [Required(ErrorMessage = "Product Id is required")]
        public string ProductId { get; set; }

        public string ContactId { get; set; }

    }
}