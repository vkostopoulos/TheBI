using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class Recall : BaseModel
    {
        public Recall()
        {
            Type = EntityType.Recall;
        }

        public string ContactId { get; set; }

        public string SaleId { get; set; }

        public string ReceiptId { get; set; }

        public string Text { get; set; }

        public double Amount { get; set; }

    }
}