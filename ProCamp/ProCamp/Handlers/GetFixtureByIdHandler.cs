using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.Handlers;
using CommonLibrary.Models.Responses;
using ProCamp.Queries.Fixtures;

namespace ProCamp.Handlers
{
    public class GetFixtureByIdHandler : IHandler<string, FixtureResponse>
    {
        private readonly GetFixtureByIdQuery _fixtureByIdQuery;

        public GetFixtureByIdHandler(GetFixtureByIdQuery fixtureByIdQuery)
        {
            _fixtureByIdQuery = fixtureByIdQuery;
        }

        public async Task<FixtureResponse> ExecuteAsync(string fixtureId)
        {
            var fixture = await _fixtureByIdQuery.ExecuteAsync(fixtureId);
            
            return Mapper.Map<FixtureResponse>(fixture);
        }
    }
}