using System;
using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.Cqrs;
using CommonLibrary.Models.Requests;
using ProCamp.Managers.Cache.Interfaces;
using ProCamp.Models;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Commands.Fixtures
{
    public class CreateFixtureCommand : ICommand<Fixture, Fixture>
    {
        private readonly IFixturesRepository _fixturesRepository;
        private readonly IFixturesCacheManager _fixturesCacheManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fixturesRepository"></param>
        /// <param name="fixturesCacheManager"></param>
        public CreateFixtureCommand(IFixturesRepository fixturesRepository, IFixturesCacheManager fixturesCacheManager)
        {
            _fixturesRepository = fixturesRepository;
            _fixturesCacheManager = fixturesCacheManager;
        }

        /// <inheritdoc />
        public async Task<Fixture> ExecuteAsync(Fixture fixture)
        {
            if (!string.IsNullOrWhiteSpace(fixture.Id))
            {
                var result = await _fixturesRepository.GetById(fixture.Id);
                if (result != null)
                {
                    throw new ArgumentException("Fixture already exist");
                }
            }
            else
            {
                fixture.Id = Guid.NewGuid().ToString("N");
            }
            
            var success = await _fixturesRepository.Create(fixture);
            if (success)
            {
                await _fixturesCacheManager.AddFixture(fixture);
            }

            return fixture;
        }
    }
}