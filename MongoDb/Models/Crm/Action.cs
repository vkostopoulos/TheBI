using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models
{
    public class Action : BaseModel
    {
        public Action()
        {
            Type = EntityType.Action;
        }

        public string Name { get; set; }

        public DesignType? DesignType { get; set; }

        public IList<string> Choices { get; set; }

        public int SortOrder { get; set; }

        public string Description { get; set; }
    }
}