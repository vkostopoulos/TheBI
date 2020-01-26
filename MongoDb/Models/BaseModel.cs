using System;
using MongoDb.Enums;

namespace MongoDb.Models
{
    public class BaseModel : IEntity
    {
        public BaseModel()
        {
            UpdatedDate = DateTime.Now;
        }

        public bool IsEnabled { get; set; }

        public string Id { get; set; }

        public EntityType Type { get; set; }

        public int SiteId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }
    }
}