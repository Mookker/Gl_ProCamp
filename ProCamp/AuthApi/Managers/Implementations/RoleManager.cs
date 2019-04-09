using System.Threading.Tasks;
using AuthApi.Managers.Interfaces;
using CommonLibrary.Helpers;

namespace AuthApi.Managers.Implementations
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