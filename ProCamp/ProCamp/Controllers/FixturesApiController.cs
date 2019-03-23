using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommonLibrary.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using ProCamp.Models;
using ProCamp.Models.Requests;
using ProCamp.Models.Responses;

namespace ProCamp.Controllers
{
    /// <summary>
    /// Handles fixtures
    /// </summary>
    [Route("/api/v1/fixtures")]
    public class FixturesApiController : Controller
    {
        private static List<Fixture> _fixtures = new List<Fixture>
        {
            new Fixture
            {
                Id = "1",
                AwayTeamName = "ManCity",
                HomeTeamName = "Fulham",
                Date = new DateTime(2019, 3, 30, 12, 30, 0)
            },
            new Fixture
            {
                Id = Guid.NewGuid().ToString("N"),
                AwayTeamName = "Cardiff City",
                HomeTeamName = "ManCity",
                Date = new DateTime(2019, 4, 3, 19, 45, 0)
            },
            
            new Fixture
            {
                Id = Guid.NewGuid().ToString("N"),
                AwayTeamName = "ManCity",
                HomeTeamName = "Brighton",
                Date = new DateTime(2019, 4, 6, 17, 30, 0)
            },
            
        }; 
        /// <summary>
        /// Gets all fixtures
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Fixture>), 200)]
        public IActionResult GetAllFixtures()
        {
            return Ok(_fixtures?.Select(Mapper.Map<FixturesResponse>).ToList());
        }

        /// <summary>
        /// Gets single fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        [HttpGet("{fixtureId}")]
        [ProducesResponseType(typeof(Fixture), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public IActionResult GetFixtureById([FromRoute]string fixtureId)
        {
            var fixture = _fixtures.FirstOrDefault(f => f.Id == fixtureId);
            if (fixture == null)
                return NotFound(new NotFoundErrorResponse($"fixture with id {fixtureId}"));

            return Ok(fixture);
        }

        /// <summary>
        /// Creates fixture
        /// </summary>
        /// <param name="createFixtureRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Fixture), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        public IActionResult CreateFixture([FromBody] CreateFixtureRequest createFixtureRequest)
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
            _fixtures.Add(newFixture);

            return Ok(newFixture);
        }

        /// <summary>
        /// Updates or creates fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <param name="updateFixtureRequest"></param>
        /// <returns></returns>
        [HttpPut("{fixtureId}")]
        [ProducesResponseType(typeof(Fixture), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        public IActionResult UpdateOrCreateFixture(string fixtureId,
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
            var index = _fixtures.FindIndex(f => f.Id == fixture.Id);
            if (index < 0)
            {
                _fixtures.Add(fixture);
            }
            else
            {
                _fixtures[index] = fixture;
            }

            return Ok(fixture);
        }
        
        /// <summary>
        /// Removes fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        [HttpDelete("{fixtureId}")]
        [ProducesResponseType(typeof(Fixture), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public IActionResult DeleteFixtureById([FromRoute]string fixtureId)
        {
            var count = _fixtures.RemoveAll(f => f.Id == fixtureId);
            if (count < 1)
                return NotFound(new NotFoundErrorResponse($"fixture with id {fixtureId}"));

            return Ok();
        }
    }
}