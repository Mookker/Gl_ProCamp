using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibrary.Enums;
using CommonLibrary.Models;
using CommonLibrary.Models.Search;
using CommonLibrary.Repositories.Interfaces;
using MongoDB.Driver;

namespace CommonLibrary.Repositories.Implementations
{
public abstract class BaseMongoDbRepository<TModel, TSearch> : IBaseRepository<TModel, TSearch> where TModel : BaseModelWithId, new() where TSearch : BaseSearchOptions
    {
        /// <summary>
        /// Gets the events collection.
        /// </summary>
        /// <value>
        /// The events collection.
        /// </value>
        protected abstract IMongoCollection<TModel> Collection { get; }

        /// <inheritdoc />
        public virtual async Task<List<TModel>> GetMultiple(TSearch searchOptions = null, PaginationOptions paginationOptions = null)
        {
            var query = GetMultipleItemsQuery(searchOptions);
            var request = Collection.Find(query);
            
            if (paginationOptions != null)
            {
                if (paginationOptions.Offset.HasValue && paginationOptions.Offset.Value > 0)
                {
                    request = request.Skip(paginationOptions.Offset);
                }

                if (paginationOptions.Limit.HasValue && paginationOptions.Limit.Value > 0)
                {
                    request = request.Limit(paginationOptions.Limit);
                }
                
                if (!String.IsNullOrWhiteSpace(paginationOptions.OrderBy))
                {
                    switch (paginationOptions.Ordering)
                    {
                        case Ordering.Ascending:
                            request = request.Sort(Builders<TModel>.Sort.Ascending(paginationOptions.OrderBy));
                            break;
                        case Ordering.Descending:
                            request = request.Sort(Builders<TModel>.Sort.Descending(paginationOptions.OrderBy));
                            break;
                        default:
                            request = request.Sort(Builders<TModel>.Sort.Descending(paginationOptions.OrderBy));
                            break;
                    }
                }
            }

            return await request.ToListAsync();
        }

        public virtual async Task<TModel> GetFirstOrDefault(TSearch searchOptions)
        {
            var query = GetMultipleItemsQuery(searchOptions);
            var item = await Collection.Find(query).FirstOrDefaultAsync();

            return item;
        }

        /// <inheritdoc />
        public virtual async Task<long> GetMultipleCount(TSearch searchOptions = null)
        {
            var query = GetMultipleItemsQuery(searchOptions);

            return await Collection.CountDocumentsAsync(query);
        }

        /// <inheritdoc />
        public virtual async Task<TModel> GetById(string id)
        {
            return await Collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TModel> Create(TModel item)
        {
            if (String.IsNullOrWhiteSpace(item.Id))
            {
                item.Id = Guid.NewGuid().ToString("N");
            }

            await Collection.InsertOneAsync(item);

            return item;
        }

        /// <inheritdoc />
        public virtual async Task<bool> Remove(string id)
        {
            var deleteResult = await Collection.DeleteOneAsync(it => it.Id == id);

            return deleteResult.DeletedCount > 0;
        }

        public Task<bool> Exists(string id)
        {
            return Collection.Find(it => it.Id == id).AnyAsync();
        }

        /// <inheritdoc />
        public virtual async Task<bool> Replace(TModel item)
        {
            var updateResult = await Collection.ReplaceOneAsync(it => it.Id == item.Id, item);

            return updateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Creates query for multiple options
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        protected abstract FilterDefinition<TModel> GetMultipleItemsQuery(TSearch searchOptions);
    }
}