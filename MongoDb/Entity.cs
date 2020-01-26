using System;
using System.Runtime.Serialization;
using MongoDb.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDb
{
    /// <summary>
    ///     Abstract Entity for all the BusinessEntities.
    /// </summary>
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity<string>
    {
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        public EntityType Type { get; set; }

        public int SiteId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }
    }
}