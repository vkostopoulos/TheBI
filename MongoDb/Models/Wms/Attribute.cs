using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class Attribute : BaseModel
    {
        public Attribute()
        {
            Type = EntityType.Attribute;
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}