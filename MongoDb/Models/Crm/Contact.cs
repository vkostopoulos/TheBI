using System;
using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class Contact : BaseModel
    {
        public Contact()
        {
            Type = EntityType.Contact;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }

        public bool Status { get; set; }

        public IList<CustomFieldValue> CustomFieldValues { get; set; }

        public IList<ActionValue> ActionValues { get; set; }

        public DateTime SubscribedDate { get; set; }

        public DateTime UnsubscribedDate { get; set; }
    }
}