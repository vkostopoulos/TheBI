using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MongoDb.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoDb.Repository
{
    /// <summary>
    ///     Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class MongoRepository<T, TKey> : IRepository<T, TKey>
        where T : IEntity<TKey>
    {
        protected static readonly TypeInfo TypeInfo = typeof(T).GetTypeInfo();

        private static readonly bool ShouldConvertToObjectId;

        /// <summary>
        ///     MongoCollection field.
        /// </summary>
        protected internal IMongoCollection<T> Collection;

        static MongoRepository()
        {
            if (typeof(TKey) == typeof(string))
            {
                var fieldInfo = TypeInfo.GetField("Id",
                    BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField |
                    BindingFlags.FlattenHierarchy);
                if (fieldInfo != null)
                {
                    ShouldConvertToObjectId = fieldInfo.GetCustomAttribute(typeof(BsonRepresentationAttribute)) != null;
                }
                else
                {
                    var propertyInfo = TypeInfo.GetProperty("Id");
                    if (propertyInfo != null)
                        ShouldConvertToObjectId =
                            propertyInfo.GetCustomAttribute(typeof(BsonRepresentationAttribute)) != null;
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the MongoRepository class.
        ///     Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepository()
            : this(Util<TKey>.GetDefaultConnectionString())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepository(string connectionString)
        {
            Collection = Util<TKey>.GetCollectionFromConnectionString<T>(connectionString);
        }

        ///// <summary>
        ///// Gets the Mongo collection (to perform advanced operations).
        ///// </summary>
        ///// <remarks>
        ///// One can argue that exposing this property (and with that, access to it's Database property for instance
        ///// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        ///// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        ///// </remarks>
        ///// <value>The Mongo collection (to perform advanced operations).</value>
        //public IMongoCollection<T> Collection
        //{
        //    get { return this.collection; }
        //}

        /// <summary>
        ///     Gets the name of the collection
        /// </summary>
        public string CollectionName => Collection.CollectionNamespace.CollectionName;

        /// <summary>
        ///     Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public virtual T GetById(TKey id)
        {
            return Collection.FindSync<T>(GetIDFilter(id)).FirstOrDefault();
        }

        /// <summary>
        ///     Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public virtual async Task<T> GetByIdAsync(TKey id)
        {
            return await (await Collection.FindAsync<T>(GetIDFilter(id))).FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Returns the T by its given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The Entity T.</returns>
        public virtual async Task<IList<T>> GetAllAsync(EntityType type)
        {
            return await (await Collection.FindAsync<T>(GetTypeFilter(type))).ToListAsync();
        }

        ///// <summary>
        ///// Returns the T by its given id.
        ///// </summary>
        ///// <param name="id">The Id of the entity to retrieve.</param>
        ///// <returns>The Entity T.</returns>
        //public virtual T GetById(ObjectId id)
        //{
        //    return this.collection.FindSync<T>(GetIDFilter(id)).Single();
        //}

        /// <summary>
        ///     Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public virtual T Add(T entity)
        {
            entity.SiteId = GlobalVariables.SiteId;
            entity.InsertedDate = entity.UpdatedDate = DateTime.Now;
            entity.IsDeleted = false;
            Collection.InsertOne(entity);

            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            entity.SiteId = GlobalVariables.SiteId;
            entity.InsertedDate = entity.UpdatedDate = DateTime.Now;
            entity.IsDeleted = false;
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        /// <summary>
        ///     Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        //public virtual void Add(IEnumerable<T> entities)
        //{
        //    this.collection.InsertMany(entities);
        //}
        public virtual void Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.SiteId = GlobalVariables.SiteId;
                entity.InsertedDate = entity.UpdatedDate = DateTime.Now;
                entity.IsDeleted = false;
            }
            Collection.InsertMany(entities);
        }

        /// <summary>
        ///     Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        public virtual async Task AddAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.SiteId = GlobalVariables.SiteId;
                entity.InsertedDate = entity.UpdatedDate = DateTime.Now;
                entity.IsDeleted = false;
            }
            await Collection.InsertManyAsync(entities);
        }

        /// <summary>
        ///     Upserts an entity
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The upserted entity.</returns>
        public virtual T AddOrUpdate(T entity)
        {
            if (entity.Id == null)
            {
                Add(entity);
            }
            else
            {
                entity.UpdatedDate = DateTime.Now;
                Collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = true});
            }
            return entity;
        }

        /// <summary>
        ///     Upserts an entity
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The upserted entity.</returns>
        public virtual async Task<T> AddOrUpdateAsync(T entity)
        {
            if (entity.Id == null)
            {
                await AddAsync(entity);
            }
            else
            {
                entity.UpdatedDate = DateTime.Now;
                await Collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = true});
            }
            return entity;
        }

        /// <summary>
        ///     Upserts a range of entity.
        /// </summary>
        /// <param name="entities">The entities to upserted.</param>
        public virtual void AddOrUpdate(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedDate = DateTime.Now;
                Collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = true});
            }
        }

        /// <summary>
        ///     Upserts a range of entity.
        /// </summary>
        /// <param name="entities">The entities to upserted.</param>
        public virtual async Task AddOrUpdateAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedDate = DateTime.Now;
                await Collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = true});
            }
        }

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public virtual T Update(T entity)
        {
            if (entity.Id == null)
                throw new ArgumentNullException(nameof(entity.Id));
            {
                entity.UpdatedDate = DateTime.Now;
                Collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = false});
                return entity;
            }
        }

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity.Id == null)
                throw new ArgumentNullException(nameof(entity.Id));
            {
                entity.UpdatedDate = DateTime.Now;
                await Collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = false});
                return entity;
            }
        }

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedDate = DateTime.Now;
                Collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = false});
            }
        }

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual async Task UpdateAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedDate = DateTime.Now;
                await Collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions {IsUpsert = false});
            }
        }

        /// <summary>
        ///     Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public virtual void Delete(TKey id)
        {
            Collection.DeleteOne(GetIDFilter(id));
        }

        /// <summary>
        ///     Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public virtual async Task DeleteAsync(TKey id)
        {
            await Collection.DeleteOneAsync(GetIDFilter(id));
        }

        ///// <summary>
        ///// Deletes an entity from the repository by its ObjectId.
        ///// </summary>
        ///// <param name="id">The ObjectId of the entity.</param>
        //public virtual void Delete(ObjectId id)
        //{
        //    this.collection.DeleteOne(GetIDFilter(id));
        //}

        ///// <summary>
        ///// Deletes an entity from the repository by its ObjectId.
        ///// </summary>
        ///// <param name="id">The ObjectId of the entity.</param>
        //public async virtual Task DeleteAsync(ObjectId id)
        //{
        //    await this.collection.DeleteOneAsync(GetIDFilter(id));
        //}

        /// <summary>
        ///     Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(T entity)
        {
            Delete(entity.Id);
        }

        /// <summary>
        ///     Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual async Task DeleteAsync(T entity)
        {
            await DeleteAsync(entity.Id);
        }

        /// <summary>
        ///     Deletes all entities in the repository.
        /// </summary>
        public virtual void DeleteAll()
        {
            Collection.DeleteMany(t => true && t.SiteId == GlobalVariables.SiteId);
        }

        /// <summary>
        ///     Deletes all entities in the repository.
        /// </summary>
        public virtual async Task DeleteAllAsync()
        {
            await Collection.DeleteManyAsync(t => true && t.SiteId == GlobalVariables.SiteId);
        }

        /// <summary>
        ///     Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual long Count()
        {
            return Collection.Count(t => true && t.SiteId == GlobalVariables.SiteId);
        }

        /// <summary>
        ///     Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual async Task<long> CountAsync()
        {
            return await Collection.CountAsync(t => true && t.SiteId == GlobalVariables.SiteId);
        }

        /// <summary>
        ///     Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Collection.AsQueryable().Where(x => x.SiteId == GlobalVariables.SiteId).Any(predicate);
        }

        /// <summary>
        ///     Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.AsQueryable().Where(x => x.SiteId == GlobalVariables.SiteId).AnyAsync(predicate);
        }

        private static FilterDefinition<T> GetIDFilter(ObjectId id)
        {
            return Builders<T>.Filter.Eq("_id", id) & Builders<T>.Filter.Eq(x => x.SiteId, GlobalVariables.SiteId);
        }

        private static FilterDefinition<T> GetTypeFilter(EntityType type)
        {
            return Builders<T>.Filter.Eq("Type", type) & Builders<T>.Filter.Eq(x => x.SiteId, GlobalVariables.SiteId);
        }

        private static FilterDefinition<T> GetSiteIdFilter(int siteId)
        {
            return Builders<T>.Filter.Eq("SiteId", siteId);
        }

        private static FilterDefinition<T> GetIDFilter(TKey id)
        {
            if (ShouldConvertToObjectId)
                return GetIDFilter(new ObjectId(id as string));
            return Builders<T>.Filter.Eq("_id", id) & Builders<T>.Filter.Eq(x => x.SiteId, GlobalVariables.SiteId);
        }

        #region IQueryable<T>

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        /// <summary>
        ///     Gets the type of the element(s) that are returned when the expression tree associated with this instance of
        ///     IQueryable is executed.
        /// </summary>
        public virtual Type ElementType => Collection.AsQueryable().ElementType;

        /// <summary>
        ///     Gets the expression tree that is associated with the instance of IQueryable.
        /// </summary>
        public virtual Expression Expression => Collection.AsQueryable().Expression;

        /// <summary>
        ///     Gets the query provider that is associated with this data source.
        /// </summary>
        public virtual IQueryProvider Provider => Collection.AsQueryable().Provider;

        #endregion
    }

    /// <summary>
    ///     Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public class MongoRepository<T> : MongoRepository<T, string>, IRepository<T>
        where T : IEntity<string>
    {
    }
}