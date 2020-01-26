using System.ComponentModel.DataAnnotations;

namespace MongoDb.Models.Wms
{
    public class InvoiceItem
    {

        public string ProductId { get; set; }

        [Required(ErrorMessage = "ProductCode is required")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Pieces is required")]
        public  int Pieces { get; set; }

        [Required(ErrorMessage = "Buy price is required")]
        public double BPrice { get; set; }

        public double Discount { get; set; }
    }
}