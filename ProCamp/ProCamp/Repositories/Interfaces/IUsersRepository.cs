using CommonLibrary.Repositories.Interfaces;
using ProCamp.Models;
using ProCamp.Models.Search;

namespace ProCamp.Repositories.Interfaces
{
    public interface IUsersRepository : IBaseRepository<User, UserSearchOptions>
    {
    }
}