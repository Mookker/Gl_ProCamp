using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Models;
using CommonLibrary.Models.Search;
using CommonLibrary.Repositories.Interfaces;
using ProCamp.Models;

namespace ProCamp.Repositories.Implementations
{
    /// <inheritdoc />
    public abstract class InMemoryRepository<TModel, TSearch> : IBaseRepository<TModel, TSearch> 
                                                        where TModel : BaseModelWithId, new() 
                                                        where TSearch: BaseSearchOptions
    {
        /// <summary>
        /// List of data
        /// </summary>
        protected List<TModel> Data;

        /// <inheritdoc />
        public Task<TModel> GetFirstOrDefault(TSearch searchOptions)
        {
            return Task.FromResult(Data.FirstOrDefault(GetSearchDefinition(searchOptions)));
        }

        /// <inheritdoc />
        public Task<List<TModel>> GetMultiple(TSearch searchOptions = default(TSearch), PaginationOptions paginationOptions = null)
        {
            var query = Data.Where(GetSearchDefinition(searchOptions));
            if (paginationOptions != null)
            {
                if (paginationOptions.Offset.HasValue)
                    query = query.Skip(paginationOptions.Offset.Value);
                if (paginationOptions.Limit.HasValue)
                    query = query.Skip(paginationOptions.Limit.Value);
            }
            
            return Task.FromResult(query.ToList());

        }

        /// <inheritdoc />
        public Task<int> GetMultipleCount(TSearch searchOptions)
        {
            var query = Data.Where(GetSearchDefinition(searchOptions));
            return Task.FromResult(query.Count());
        }

        /// <inheritdoc />
        public Task<TModel> GetById(string id)
        {
            return Task.FromResult(Data.FirstOrDefault(i => i.Id == id));

        }

        /// <inheritdoc />
        public Task<bool> Create(TModel item)
        {
            Data.Add(item);
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task<bool> Replace(TModel item)
        {
            var index = Data.FindIndex(i => i.Id == item.Id);
            if (index < 0)
                return Task.FromResult(false);
            
            Data[index] = item;

            return Task.FromResult(true);

        }

        /// <inheritdoc />
        public Task<bool> Remove(string id)
        {
            var count = Data.RemoveAll(i => i.Id == id);
            return Task.FromResult(count > 0);
        }

        /// <inheritdoc />
        public Task<bool> Exists(string id)
        {
            return Task.FromResult(Data.Any(i => i.Id == id));
        }

        /// <summary>
        /// Search definition
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        protected abstract Func<TModel, bool> GetSearchDefinition(TSearch searchOptions);
    }
}