using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.Models.Errors;
using CommonLibrary.Models.Requests;
using CommonLibrary.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
using ProCamp.Models.QueryParams;
using ProCamp.Models.Search;

namespace ProCamp.Controllers
{
    /// <summary>
    /// Handles fixtures
    /// </summary>
    [Route("/api/v1/fixtures")]
    public class FixturesApiController : Controller
    {
        private readonly IFixtureManager _fixtureManager;


        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="fixtureManager"></param>
        public FixturesApiController(IFixtureManager fixtureManager)
        {
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
            var fixtures = await _fixtureManager.GetMultiple(queryParams != null ? new FixturesSearchOptions
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
            await _fixtureManager.CreateFixture(newFixture);

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
            await _fixtureManager.ReplaceFixture(fixture);

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
            var removed = await _fixtureManager.RemoveFixture(fixtureId);
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
            var exists = _fixtureManager.GetFixture(fixtureId);
            if (exists == null)
                return NotFound(new NotFoundErrorResponse($"fixture with id {fixtureId}"));

            return Ok();
        }

        /// <summary>
        /// Seeds data
        /// </summary>
        /// <returns></returns>
        [HttpPut("seed")]
        [ProducesResponseType(typeof(Fixture), 200)]
        public async Task<IActionResult> Seed()
        {
            await _fixtureManager.Seed();

            return Ok();
        }
    }
}