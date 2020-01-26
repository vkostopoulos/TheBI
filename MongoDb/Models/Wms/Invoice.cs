using System;
using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class Invoice : BaseModel
    {
        public Invoice()
        {
            Type = EntityType.Invoice;
        }

        public string StoreId { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public DateTime EntryDate { get; set; }

        public string SupplierId { get; set; }

        public double Vat { get; set; }

        public double Total { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }

    }
}