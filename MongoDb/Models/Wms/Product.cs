using System.Collections.Generic;
using MongoDb.Enums;
using MongoDb.Models.Wms;
using MongoDb.Models.Dto.Wms;

namespace MongoDb.Models.Wms
{
    public class Product : BaseModel
    {
        public Product()
        {
            Type = EntityType.Product;
        }

        public string StoreId { get; set; }

        public string Barcode { get; set; }

        public string ProductCode { get; set; }

        public string InvoiceId { get; set; }

        public double Stock { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double SPrice { get; set; }

        public double Discount { get; set; }

        public List<Attribute> Attributes { get; set; }

        public string CategoryId { get; set; }


    }
}