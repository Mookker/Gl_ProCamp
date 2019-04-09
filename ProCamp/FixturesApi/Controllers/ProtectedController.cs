using CommonLibrary.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixturesApi.Controllers
{
    [Route("/api/v1/protected")]
    public class ProtectedController : Controller
    {
        /// <summary>
        /// Gets very secret info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AuthConstants.ProtectedUserRole)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(403)]
        public IActionResult GetStatusProtected()
        {
            return Ok("We're inside protected controller");
        }
        
    }
}