using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
using ProCamp.Models.QueryParams;
using ProCamp.Models.Requests;
using ProCamp.Models.Responses;
using ProCamp.Models.Search;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Controllers
{
    /// <summary>
    /// Handles fixtures
    /// </summary>
    [Route("/api/v1/fixtures")]
    public class FixturesApiController : Controller
    {
        private readonly IFixturesRepository _fixturesRepository;
        private readonly IFixtureManager _fixtureManager;

        private static List<Fixture> _fixtures = new List<Fixture>
        {
            new Fixture
            {
                Id = "1",
                AwayTeamName = "ManCity",
                HomeTeamName = "Fulham",
                Date = new DateTime(2019, 3, 30, 12, 30, 0),
                Location = new List<double>{-2.2002, 53.4765}
            },
            new Fixture
            {
                Id = "2",
                AwayTeamName = "Cardiff City",
                HomeTeamName = "ManCity",
                Date = new DateTime(2019, 4, 3, 19, 45, 0),
                Location = new List<double>{ -3.2018, 51.4703}
            },
            
            new Fixture
            {
                Id = "3",
                AwayTeamName = "Brighton",
                HomeTeamName = "Man City",
                Date = new DateTime(2019, 4, 6, 17, 30, 0),
                Location = new List<double>{ -0.0766330268, 50.857089905}
            },
            
        };

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="fixturesRepository"></param>
        /// <param name="fixtureManager"></param>
        public FixturesApiController(IFixturesRepository fixturesRepository, IFixtureManager fixtureManager)
        {
            _fixturesRepository = fixturesRepository;
            _fixtureManager = fixtureManager;
        }

        /// <summary>
        /// Gets all fixtures
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<FixturesResponse>), 200)]
        public async Task<IActionResult> GetAllFixtures([FromQuery] FixturesQueryParams queryParams)
        {
            var fixtures = await _fixturesRepository.GetMultiple(queryParams !=null ? new FixturesSearchOptions
            {
                Id = queryParams.Id,
                HomeTeamName = queryParams.HomeTeamName,
                AwayTeamName = queryParams.AwayTeamName,
                DateTo = queryParams.DateTo,
                DateFrom = queryParams.DateFrom
            } : null);
            
            return Ok(fixtures.Select(Mapper.Map<FixturesResponse>).ToList());
        }

        /// <summary>
        /// Gets single fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        [HttpGet("{fixtureId}")]
        [ProducesResponseType(typeof(FixturesResponse), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public async Task<IActionResult> GetFixtureById([FromRoute]string fixtureId)
        {
            var fixture = await _fixtureManager.GetFixture(fixtureId);
            
            if (fixture == null)
                return NotFound(new NotFoundErrorResponse($"fixture with id {fixtureId}"));

            return Ok(Mapper.Map<FixturesResponse>(fixture));
        }

        /// <summary>
        /// Creates fixture
        /// </summary>
        /// <param name="createFixtureRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(FixturesResponse), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        public async Task<IActionResult> CreateFixture([FromBody] CreateFixtureRequest createFixtureRequest)
        {
            if (createFixtureRequest == null)
            {
                return BadRequest(new BadRequestResponse("invalid model"));
            }
            if (createFixtureRequest.AwayTeamName == string.Empty)
            {
                return BadRequest(new BadRequestResponse("empty away team"));
            }
            if (createFixtureRequest.HomeTeamName == string.Empty)
            {
                return BadRequest(new BadRequestResponse("empty home team"));
            }
            
            
            var newFixture = Mapper.Map<Fixture>(createFixtureRequest);
            newFixture.Id = Guid.NewGuid().ToString("N");
            await _fixturesRepository.Create(newFixture);

            return Ok(Mapper.Map<FixturesResponse>(newFixture));
        }

        /// <summary>
        /// Updates or creates fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <param name="updateFixtureRequest"></param>
        /// <returns></returns>
        [HttpPut("{fixtureId}")]
        [ProducesResponseType(typeof(FixturesResponse), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        public async Task<IActionResult> UpdateOrCreateFixture(string fixtureId,
            [FromBody] UpdateFixtureRequest updateFixtureRequest)
        {
            if (updateFixtureRequest == null)
            {
                return BadRequest(new BadRequestResponse("invalid model"));
            }

            if (updateFixtureRequest.Id != fixtureId)
            {
                return BadRequest(new BadRequestResponse("different fixture ids in body and path"));
            }
            if (updateFixtureRequest.Id == string.Empty)
            {
                return BadRequest(new BadRequestResponse("empty id"));
            }
            if (updateFixtureRequest.AwayTeamName == string.Empty)
            {
                return BadRequest(new BadRequestResponse("empty away team"));
            }
            if (updateFixtureRequest.HomeTeamName == string.Empty)
            {
                return BadRequest(new BadRequestResponse("empty home team"));
            }

            var fixture = Mapper.Map<Fixture>(updateFixtureRequest);
            await _fixturesRepository.Replace(fixture);

            return Ok(Mapper.Map<FixturesResponse>(fixture));
        }
        
        /// <summary>
        /// Removes fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        [HttpDelete("{fixtureId}")]
        [ProducesResponseType(typeof(FixturesResponse), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public async Task<IActionResult> DeleteFixtureById([FromRoute]string fixtureId)
        {
            var removed = await _fixturesRepository.Remove(fixtureId);
            if (!removed)
                return NotFound(new NotFoundErrorResponse($"fixture with id {fixtureId}"));

            return Ok();
        }


        /// <summary>
        /// Checks if fixture exists
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        [HttpHead("{fixtureId}")]
        [ProducesResponseType(typeof(FixturesResponse), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public IActionResult FixtureExists([FromRoute] string fixtureId)
        {
            var exists = _fixtures.Any(f => f.Id == fixtureId);
            if (!exists)
                return NotFound(new NotFoundErrorResponse($"fixture with id {fixtureId}"));

            return Ok();
        }

        /// <summary>
        /// Gets nearest fixtures
        /// </summary>
        /// <param name="nearestQueryParams"></param>
        /// <returns></returns>
        [HttpGet("/nearest")]
        [ProducesResponseType(typeof(List<NearestFixture>), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        public async Task<IActionResult> GetNearestFixtures([FromQuery] NearestQueryParams nearestQueryParams)
        {
            //TODO: params validation
            var fixtures = await _fixtureManager.GetNearestFixtures(nearestQueryParams.Longitude,
                nearestQueryParams.Latitude, nearestQueryParams.Offset, nearestQueryParams.Limit);

            return Ok(fixtures);
        }
        
        /// <summary>
        /// Seeds data
        /// </summary>
        /// <returns></returns>
        [HttpPut("seed")]
        [ProducesResponseType(typeof(Fixture), 200)]
        public async Task<IActionResult> Seed()
        {
            foreach (var fixture in _fixtures)
            {
                if (await _fixturesRepository.Exists(fixture.Id))
                {
                    await _fixturesRepository.Replace(fixture);
                }
                else
                {
                    await _fixturesRepository.Create(fixture);
                }
            }

            return Ok();
        }
    }
}