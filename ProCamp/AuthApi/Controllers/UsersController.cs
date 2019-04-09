using System.Threading.Tasks;
using AuthApi.Managers.Interfaces;
using AuthApi.Models;
using AuthApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
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