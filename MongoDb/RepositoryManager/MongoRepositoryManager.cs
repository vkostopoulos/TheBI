﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoRepository;

namespace MongoDb.RepositoryManager
{
    // TODO: Code coverage here is near-zero. A new RepoManagerTests.cs class needs to be created and we need to
    //      test these methods. Ofcourse we also need to update documentation.
    //      This is a work-in-progress.

    /// <summary>
    ///     Deals with the collections of entities in MongoDb. This class tries to hide as much MongoDb-specific details
    ///     as possible but it's not 100% *yet*. It is a very thin wrapper around most methods on MongoDb's MongoCollection
    ///     objects.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class MongoRepositoryManager<T, TKey> : IRepositoryManager<T, TKey>
        where T : IEntity<TKey>
    {
        /// <summary>
        ///     MongoCollection field.
        /// </summary>
        private readonly IMongoCollection<T> collection;

        /// <summary>
        ///     Initializes a new instance of the MongoRepositoryManager class.
        ///     Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepositoryManager()
            : this(Util<TKey>.GetDefaultConnectionString())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepositoryManager(string connectionString)
        {
            collection = Util<TKey>.GetCollectionFromConnectionString<T>(connectionString);
        }

        /// <summary>
        ///     Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepositoryManager(string connectionString, string collectionName)
        {
            collection = Util<TKey>.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        /// <summary>
        ///     Gets a value indicating whether the collection already exists.
        /// </summary>
        /// <value>Returns true when the collection already exists, false otherwise.</value>
        public virtual bool Exists()
        {
            return collection.Database
                .ListCollections(new ListCollectionsOptions {Filter = new BsonDocument("name", Name)}).Any();
        }

        /// <summary>
        ///     Gets a value indicating whether the collection already exists.
        /// </summary>
        /// <value>Returns true when the collection already exists, false otherwise.</value>
        public virtual async Task<bool> ExistsAsync()
        {
            var options = new ListCollectionsOptions {Filter = new BsonDocument("name", Name)};
            return await (await collection.Database.ListCollectionsAsync(options)).AnyAsync();
        }

        /// <summary>
        ///     Gets the name of the collection as Mongo uses.
        /// </summary>
        /// <value>The name of the collection as Mongo uses.</value>
        public virtual string Name => collection.CollectionNamespace.CollectionName;

        /// <summary>
        ///     Drops the collection.
        /// </summary>
        public virtual void Drop()
        {
            collection.Database.DropCollection(Name);
        }

        /// <summary>
        ///     Drops the collection.
        /// </summary>
        public virtual async Task DropAsync()
        {
            await collection.Database.DropCollectionAsync(Name);
        }

        /// <summary>
        ///     Tests whether the repository is capped.
        /// </summary>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        public virtual bool IsCapped()
        {
            return GetStats().IsCapped;
        }


        /// <summary>
        ///     Tests whether the repository is capped.
        /// </summary>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        public virtual async Task<bool> IsCappedAsync()
        {
            return (await GetStatsAsync()).IsCapped;
        }

        /// <summary>
        ///     Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        public virtual void DropIndex(string keyname)
        {
            DropIndexes(new[] {keyname});
        }

        /// <summary>
        ///     Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        public virtual async Task DropIndexAsync(string keyname)
        {
            await DropIndexesAsync(new[] {keyname});
        }

        /// <summary>
        ///     Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        public virtual void DropIndexes(IEnumerable<string> keynames)
        {
            foreach (var k in keynames)
                collection.Indexes.DropOne(k);
        }

        /// <summary>
        ///     Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        public virtual async Task DropIndexesAsync(IEnumerable<string> keynames)
        {
            foreach (var k in keynames)
                await collection.Indexes.DropOneAsync(k);
        }

        /// <summary>
        ///     Drops all indexes on this repository.
        /// </summary>
        public virtual void DropAllIndexes()
        {
            collection.Indexes.DropAll();
        }

        /// <summary>
        ///     Drops all indexes on this repository.
        /// </summary>
        public virtual async Task DropAllIndexesAsync()
        {
            await collection.Indexes.DropAllAsync();
        }

        /// <summary>
        ///     Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        ///     Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public virtual void EnsureIndex(string keyname)
        {
            EnsureIndexes(new[] {keyname});
        }

        /// <summary>
        ///     Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        ///     Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public virtual async Task EnsureIndexAsync(string keyname)
        {
            await EnsureIndexesAsync(new[] {keyname});
        }

        /// <summary>
        ///     Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public virtual void EnsureIndex(string keyname, bool descending, bool unique, bool sparse)
        {
            EnsureIndexes(new[] {keyname}, descending, unique, sparse);
        }

        /// <summary>
        ///     Ensures that the desired index exist and creates it if it doesn't exist.
        /// </summary>
        /// <param name="keyname">The indexed field.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public virtual async Task EnsureIndexAsync(string keyname, bool descending, bool unique, bool sparse)
        {
            await EnsureIndexesAsync(new[] {keyname}, descending, unique, sparse);
        }

        /// <summary>
        ///     Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        ///     Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public virtual void EnsureIndexes(IEnumerable<string> keynames)
        {
            EnsureIndexes(keynames, false, false, false);
        }

        /// <summary>
        ///     Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        ///     Index will be ascending order, non-unique, non-sparse.
        /// </remarks>
        public virtual async Task EnsureIndexesAsync(IEnumerable<string> keynames)
        {
            await EnsureIndexesAsync(keynames, false, false, false);
        }

        /// <summary>
        ///     Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public virtual void EnsureIndexes(IEnumerable<string> keynames, bool descending, bool unique, bool sparse)
        {
            foreach (var k in keynames)
            {
                var opt = new CreateIndexOptions {Unique = unique, Sparse = sparse};
                var builder = Builders<T>.IndexKeys;
                if (descending)
                    collection.Indexes.CreateOne(builder.Descending(k), opt);
                else
                    collection.Indexes.CreateOne(builder.Descending(k), opt);
            }
        }

        /// <summary>
        ///     Ensures that the desired indexes exist and creates them if they don't exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <param name="descending">Set to true to make index descending, false for ascending.</param>
        /// <param name="unique">Set to true to ensure index enforces unique values.</param>
        /// <param name="sparse">Set to true to specify the index is sparse.</param>
        /// <remarks>
        ///     This is a convenience method for EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options).
        /// </remarks>
        public virtual async Task EnsureIndexesAsync(IEnumerable<string> keynames, bool descending, bool unique,
            bool sparse)
        {
            foreach (var k in keynames)
            {
                var opt = new CreateIndexOptions {Unique = unique, Sparse = sparse};
                var builder = Builders<T>.IndexKeys;
                if (descending)
                    await collection.Indexes.CreateOneAsync(builder.Descending(k), opt);
                else
                    await collection.Indexes.CreateOneAsync(builder.Descending(k), opt);
            }
        }

        /// <summary>
        ///     Tests whether indexes exist.
        /// </summary>
        /// <param name="keyname">The indexed fields.</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public virtual bool IndexExists(string keyname)
        {
            return IndexesExists(new[] {keyname});
        }

        /// <summary>
        ///     Tests whether indexes exist.
        /// </summary>
        /// <param name="keyname">The indexed fields.</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public virtual async Task<bool> IndexExistsAsync(string keyname)
        {
            return await IndexesExistsAsync(new[] {keyname});
        }

        /// <summary>
        ///     Tests whether indexes exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public virtual bool IndexesExists(IEnumerable<string> keynames)
        {
            var ix = collection.Indexes.List().ToList();
            return keynames.All(k => ix.Contains(BsonValue.Create(k)));
        }

        /// <summary>
        ///     Tests whether indexes exist.
        /// </summary>
        /// <param name="keynames">The indexed fields.</param>
        /// <returns>Returns true when the indexes exist, false otherwise.</returns>
        public virtual async Task<bool> IndexesExistsAsync(IEnumerable<string> keynames)
        {
            var ix = await (await collection.Indexes.ListAsync()).ToListAsync();
            return keynames.All(k => ix.Contains(BsonValue.Create(k)));
        }

        /// <summary>
        ///     Runs the ReIndex command on this repository.
        /// </summary>
        public virtual void ReIndex()
        {
            collection.Database.RunCommand(new BsonDocumentCommand<BsonDocument>(new BsonDocument {{"reIndex", Name}}));
        }

        /// <summary>
        ///     Runs the ReIndex command on this repository.
        /// </summary>
        public virtual async Task ReIndexAsync()
        {
            await collection.Database.RunCommandAsync(
                new BsonDocumentCommand<BsonDocument>(new BsonDocument {{"reIndex", Name}}));
        }

        /// <summary>
        ///     Gets the total size for the repository (data + indexes).
        /// </summary>
        /// <returns>Returns total size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public virtual long GetTotalDataSize()
        {
            return GetStats().DataSize;
        }

        /// <summary>
        ///     Gets the total size for the repository (data + indexes).
        /// </summary>
        /// <returns>Returns total size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public virtual async Task<long> GetTotalDataSizeAsync()
        {
            return (await GetStatsAsync()).DataSize;
        }

        /// <summary>
        ///     Gets the total storage size for the repository (data + indexes).
        /// </summary>
        /// <returns>Returns total storage size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public virtual long GetTotalStorageSize()
        {
            return GetStats().StorageSize;
        }


        /// <summary>
        ///     Gets the total storage size for the repository (data + indexes).
        /// </summary>
        /// <returns>Returns total storage size for the repository (data + indexes).</returns>
        [Obsolete("This method will be removed in the next version of the driver")]
        public virtual async Task<long> GetTotalStorageSizeAsync()
        {
            return (await GetStatsAsync()).StorageSize;
        }

        /// <summary>
        ///     Validates the integrity of the repository.
        /// </summary>
        /// <returns>Returns a ValidateCollectionResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public virtual ValidateCollectionResult Validate()
        {
            return new ValidateCollectionResult(
                collection.Database.RunCommand(
                    new BsonDocumentCommand<BsonDocument>(new BsonDocument {{"validate", Name}}))
            );
        }

        /// <summary>
        ///     Validates the integrity of the repository.
        /// </summary>
        /// <returns>Returns a ValidateCollectionResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public virtual async Task<ValidateCollectionResult> ValidateAsync()
        {
            return new ValidateCollectionResult(
                await collection.Database.RunCommandAsync(
                    new BsonDocumentCommand<BsonDocument>(new BsonDocument {{"validate", Name}}))
            );
        }

        /// <summary>
        ///     Gets stats for this repository.
        /// </summary>
        /// <returns>Returns a CollectionStatsResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public virtual CollectionStatsResult GetStats()
        {
            return new CollectionStatsResult(
                collection.Database.RunCommand(
                    new BsonDocumentCommand<BsonDocument>(new BsonDocument {{"collstats", Name}}))
            );
        }

        /// <summary>
        ///     Gets stats for this repository.
        /// </summary>
        /// <returns>Returns a CollectionStatsResult.</returns>
        /// <remarks>You will need to reference MongoDb.Driver.</remarks>
        public virtual async Task<CollectionStatsResult> GetStatsAsync()
        {
            return new CollectionStatsResult(
                await collection.Database.RunCommandAsync(
                    new BsonDocumentCommand<BsonDocument>(new BsonDocument {{"collstats", Name}}))
            );
        }

        /// <summary>
        ///     Gets the indexes for this repository.
        /// </summary>
        /// <returns>Returns the indexes for this repository.</returns>
        public virtual List<BsonDocument> GetIndexes()
        {
            return collection.Indexes.List().ToList();
        }

        /// <summary>
        ///     Gets the indexes for this repository.
        /// </summary>
        /// <returns>Returns the indexes for this repository.</returns>
        public virtual async Task<List<BsonDocument>> GetIndexesAsync()
        {
            return await (await collection.Indexes.ListAsync()).ToListAsync();
        }
    }

    /// <summary>
    ///     Deals with the collections of entities in MongoDb. This class tries to hide as much MongoDb-specific details
    ///     as possible but it's not 100% *yet*. It is a very thin wrapper around most methods on MongoDb's MongoCollection
    ///     objects.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public class MongoRepositoryManager<T> : MongoRepositoryManager<T, string>, IRepositoryManager<T>
        where T : IEntity<string>
    {
        /// <summary>
        ///     Initializes a new instance of the MongoRepositoryManager class.
        ///     Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepositoryManager()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepositoryManager(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepositoryManager(string connectionString, string collectionName)
            : base(connectionString, collectionName)
        {
        }
    }
}