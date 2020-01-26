using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class Store : BaseModel
    {
        public Store()
        {
            Type = EntityType.Store;
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}