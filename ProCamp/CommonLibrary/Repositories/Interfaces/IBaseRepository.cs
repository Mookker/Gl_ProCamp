using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibrary.Models;
using CommonLibrary.Models.Search;

namespace CommonLibrary.Repositories.Interfaces
{
    public interface IBaseRepository<TModel, TSearch> where TModel : BaseModelWithId, new() 
                                                      where TSearch: BaseSearchOptions
    {
        /// <summary>
        /// Gets first or null
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        Task<TModel> GetFirstOrDefault(TSearch searchOptions);

        /// <summary>
        /// Gets list of items
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <param name="paginationOptions"></param>
        /// <returns></returns>
        Task<List<TModel>> GetMultiple(TSearch searchOptions = null, PaginationOptions paginationOptions = null);

        /// <summary>
        /// Gets number of items by search criteria
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        Task<long> GetMultipleCount(TSearch searchOptions);

        /// <summary>
        /// Convenience method to get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetById(string id);

        /// <summary>
        /// Creates new item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<TModel> Create(TModel item);

        /// <summary>
        /// Replaces item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> Replace(TModel item);

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Remove(string id);

        /// <summary>
        /// Checks if item exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Exists(string id);
    }
}