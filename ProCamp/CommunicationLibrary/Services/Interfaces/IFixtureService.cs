using System.Collections.Generic;
using System.Threading.Tasks;
using CommunicationLibrary.Models.Responses;

namespace CommunicationLibrary.Services.Interfaces
{
    public interface IFixtureService
    {
        Task<FixturesResponse> GetFixture(string id);
        void Authorize(string jwt);
    }
}