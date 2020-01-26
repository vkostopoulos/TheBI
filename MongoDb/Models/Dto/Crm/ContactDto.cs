using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MongoDb.Models.Dto.Crm 
    {
    public class ContactDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }

        public bool? Status { get; set; }

        public IList<CustomFieldValue> CustomFieldValues { get; set; }

        public IList<ActionValue> ActionValues { get; set; }

        public DateTime? SubscribedDate { get; set; }

        public DateTime? UnsubscribedDate { get; set; }

        public bool? IsEnabled { get; set; }
    }
}