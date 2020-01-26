using System.Collections.Generic;
using MongoDb.Enums;
using MongoDb.Models.Wms;

namespace MongoDb.Models.Wms
{
    public class Sale : BaseModel
    {
        public Sale()
        {
            Type = EntityType.Sale;
        }

        public string ProductCode { get; set; }

        public string ProductBarcode { get; set; }

        public double ProductDiscount { get; set; }

        public ReceiptType SaleType { get; set; }

        public double ProductSPrice { get; set; }

        public double ProductBPrice { get; set; }

        public List<Attribute> ProductAttributes { get; set; }

        public string ProductId { get; set; }

        public string ContactId { get; set; }

    }
}