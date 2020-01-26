using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;
using MongoDb.Models.Wms;

namespace MongoDb.Models.Dto.Wms
{
    public class InvoiceDto
    {
        public string Id { get; set; }

        public string StoreId { get; set; }

        [Required(ErrorMessage = "InvoiceType is required")]
        public InvoiceType? InvoiceType { get; set; }

        [Required(ErrorMessage = "EntryDate is required")]
        public DateTime? EntryDate { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        public string SupplierId { get; set; }

        [Required(ErrorMessage = "Vat is required")]
        public double? Vat { get; set; }

        [Required(ErrorMessage = "Total is required")]
        public double? Total { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }

    }
}