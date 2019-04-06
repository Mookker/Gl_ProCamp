using System.Threading.Tasks;

namespace ProCamp.Managers.Interfaces
{
    /// <summary>
    /// Role manager
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Gets roles by id
        /// </summary>
        /// <param name="id"></param>
        Task<string[]> GetRolesById(string id);
    }
}