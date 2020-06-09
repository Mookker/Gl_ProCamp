using System;
using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.Handlers;
using CommonLibrary.Models.Requests;
using CommonLibrary.Models.Responses;
using ProCamp.Commands.Fixtures;
using ProCamp.Models;

namespace ProCamp.Handlers
{
    /// <summary>
    /// Creates fixture
    /// </summary>
    public class CreateFixtureHandler : IHandler<CreateFixtureRequest, FixtureResponse>
    {
        private readonly CreateFixtureCommand _createFixtureCommand;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="createFixtureCommand"></param>
        public CreateFixtureHandler(CreateFixtureCommand createFixtureCommand)
        {
            _createFixtureCommand = createFixtureCommand;
        }

        /// <inheritdoc />
        public async Task<FixtureResponse> ExecuteAsync(CreateFixtureRequest createFixtureRequest)
        {
            var fixture = Mapper.Map<Fixture>(createFixtureRequest);

            if (fixture == null)
            {
                throw new ArgumentException("invalid fixture");
            }
            if (string.IsNullOrWhiteSpace(fixture.AwayTeamName))
            {
                throw new ArgumentException("empty away team");
            }
            if (string.IsNullOrWhiteSpace(fixture.HomeTeamName))
            {
                throw new ArgumentException("empty home team");
            }

            var created = await _createFixtureCommand.ExecuteAsync(fixture);
            return Mapper.Map<FixtureResponse>(created);
        }
    }
}