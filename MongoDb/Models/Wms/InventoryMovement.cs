using System.Collections.Generic;
using MongoDb.Enums;

namespace MongoDb.Models.Wms
{
    public class InventoryMovement : BaseModel
    {
        public InventoryMovement()
        {
            Type = EntityType.InventoryMovement;
        }

        public string Name { get; set; }

        public string FromStoreId { get; set; }

        public string ProductCode { get; set; }

    }
}