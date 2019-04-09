using AuthApi.Models;
using AuthApi.Models.Search;
using CommonLibrary.Repositories.Interfaces;

namespace AuthApi.Repositories.Interfaces
{
    public interface IUsersRepository : IBaseRepository<User, UserSearchOptions>
    {
    }
}