using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDb.Enums;

namespace MongoDb.Models.Dto.Wms
{
    public class ReceiptDto
    {
        public string Id { get; set; }

        public string ReceiptCode { get; set; }

        public ReceiptType? ReceiptType { get; set; }

        public List<SaleDto> Sales { get; set; }

        [Required(ErrorMessage = "Entry date is required")]
        public DateTime? EntryDate { get; set; }

        [Required(ErrorMessage = "Vat is required")]
        public double? Vat { get; set; }

        [Required(ErrorMessage = "Total is required")]
        public double? Total { get; set; }
    }
}