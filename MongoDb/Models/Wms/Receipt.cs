using System;
using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class Receipt : BaseModel
    {
        public Receipt()
        {
            Type = EntityType.Receipt;
        }


        public string ReceiptCode { get; set; }

        public ReceiptType ReceiptType { get; set; }

        public List<Sale> Sales { get; set; }

        public DateTime EntryDate { get; set; }

        public double Vat { get; set; }

        public double Total { get; set; }
    }
}