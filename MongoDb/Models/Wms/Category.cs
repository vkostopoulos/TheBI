using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class Category : BaseModel
    {
        public Category()
        {
            Type = EntityType.Category;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public bool IsSubCategory { get; set; }

        public string MainCategoryId { get; set; }
    }
}