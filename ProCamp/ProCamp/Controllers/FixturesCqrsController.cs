using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.Models.Errors;
using CommonLibrary.Models.Requests;
using CommonLibrary.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using ProCamp.Commands.Fixtures;
using ProCamp.Handlers;
using ProCamp.Models;
using ProCamp.Queries.Fixtures;

namespace ProCamp.Controllers
{
    /// <summary>
    /// Cqrs example
    /// </summary>
    [Route("/api/v2/fixtures")]
    public class FixturesCqrsController : Controller
    {
        /// <summary>
        /// Gets single fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <param name="fixtureByIdHandler"></param>
        /// <returns></returns>
        [HttpGet("{fixtureId}")]
        [ProducesResponseType(typeof(FixtureResponse), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public async Task<IActionResult> GetFixtureById([FromRoute] string fixtureId,
            [FromServices] GetFixtureByIdHandler fixtureByIdHandler)
        {
            var fixture = await fixtureByIdHandler.ExecuteAsync(fixtureId);

            if (fixture == null)
                return NotFound(new NotFoundErrorResponse($"fixture with id {fixtureId}"));

            return Ok(fixture);
        }


        /// <summary>
        /// Creates fixture
        /// </summary>
        /// <param name="createFixtureRequest"></param>
        /// <param name="createFixtureHandler"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(FixtureResponse), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        public async Task<IActionResult> CreateFixture([FromBody] CreateFixtureRequest createFixtureRequest,
            [FromServices] CreateFixtureHandler createFixtureHandler)
        {
            var created =  await createFixtureHandler.ExecuteAsync(createFixtureRequest);

            return Ok(created);
        }
    }
}