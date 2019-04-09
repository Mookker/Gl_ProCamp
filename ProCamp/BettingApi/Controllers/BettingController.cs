using System.Threading.Tasks;
using CommunicationLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BettingApi.Controllers
{
    [Route("/api/v1/betting/")]
    public class BettingController : Controller
    {
        private readonly IFixtureService _fixtureService;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fixtureService"></param>
        /// <param name="authService"></param>
        /// <param name="configuration"></param>
        public BettingController(IFixtureService fixtureService, IAuthService authService, IConfiguration configuration)
        {
            _fixtureService = fixtureService;
            _authService = authService;
            _configuration = configuration;
        }

        [HttpGet("{fixtureId}")]
        public async Task<IActionResult> GetFixtureProxy(string fixtureId)
        {
            var jwt = await _authService.GetApiKeyJwt(_configuration.GetValue<string>("ApiKey")); //TODO: some smart logic
            _fixtureService.Authorize(jwt);
            return Ok(await _fixtureService.GetFixture(fixtureId));
        }
    }
}