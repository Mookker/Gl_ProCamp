using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
using ProCamp.Models.Requests;

namespace ProCamp.Controllers
{
    [Route("/api/v1/users")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> CreateUser(LoginRequest loginModel)
        {
            var newUser = await _userManager.CreateUser(loginModel);
            
            return Ok(newUser);
        }
    }
}