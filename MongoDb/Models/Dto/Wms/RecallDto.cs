using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Wms
{
    public class RecallDto
    {
        public string Id { get; set; }

        public string ContactId { get; set; }

        public string SaleId { get; set; }

        public string ReceiptId { get; set; }

        public string Text { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public double? Amount { get; set; }

    }
}