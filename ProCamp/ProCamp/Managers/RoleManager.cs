using System.Threading.Tasks;
using CommonLibrary.Helpers;
using ProCamp.Managers.Interfaces;

namespace ProCamp.Managers
{
    class RoleManager : IRoleManager
    {
        /// <inheritdoc />
        public Task<string[]> GetRolesById(string id)
        {
            return Task.FromResult(new[] {AuthConstants.FixtureReaderRole, AuthConstants.FixtureWriterRole});
        }
    }
}