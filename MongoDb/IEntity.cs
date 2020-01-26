using System;
using MongoDb.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDb
{
    /// <summary>
    ///     Generic Entity interface.
    /// </summary>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        ///     Gets or sets the Id of the Entity.
        /// </summary>
        /// <value>Id of the Entity.</value>
        [BsonId]
        TKey Id { get; set; }

        EntityType Type { get; set; }

        int SiteId { get; set; }

        DateTime UpdatedDate { get; set; }

        DateTime InsertedDate { get; set; }

        bool IsDeleted { get; set; }
    }

    /// <summary>
    ///     "Default" Entity interface.
    /// </summary>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public interface IEntity : IEntity<string>
    {
    }
}