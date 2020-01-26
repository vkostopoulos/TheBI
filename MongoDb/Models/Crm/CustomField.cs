using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models
{
    public class CustomField : BaseModel
    {
        public CustomField()
        {
            Type = EntityType.CustomField;
        }

        public string Name { get; set; }

        public DesignType DesignType { get; set; }

        public IList<string> Choices { get; set; }

        public int SortOrder { get; set; }

        public bool IsRequired { get; set; }
    }
}