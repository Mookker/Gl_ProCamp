using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthApi.Managers.Interfaces;
using AuthApi.Models.Requests;
using AuthApi.Models.Responses;
using CommonLibrary.Config;
using CommonLibrary.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthApi.Controllers
{
    [Route("/api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IApiKeyManager _apiKeyManager;
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly IRoleManager _roleManager;
        private IUserManager _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKeyManager"></param>
        /// <param name="jwtOptions"></param>
        /// <param name="roleManager"></param>
        public AuthController(IApiKeyManager apiKeyManager, IOptions<JwtOptions> jwtOptions, IRoleManager roleManager, IUserManager userManager)
        {
            _apiKeyManager = apiKeyManager;
            _jwtOptions = jwtOptions;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Generates api key for user.
        /// In real project requires Authorization to be validated, but this one
        /// will give keys to everyone
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api-key")]
        [Authorize(Roles = AuthConstants.ApiKeyWriterRole)]
        public async Task<ActionResult<string>> GenerateApiKey()
        {
            var key = await _apiKeyManager.GenerateKey();
            
            return Ok(key);
        }

        /// <summary>
        /// Marks key as invalid
        /// </summary>
        /// <param name="disableKeyModel"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api-key")]
        [Authorize(Roles = AuthConstants.ApiKeyWriterRole)]
        public async Task<ActionResult> InvalidateApiKey(DisableKeyModelRequest disableKeyModel)
        {
            var valid = await _apiKeyManager.IsKeyValid(disableKeyModel.Key);
            if (!valid)
                return BadRequest("Key is not valid");

            var success = await _apiKeyManager.InvalidateKey(disableKeyModel.Key);
            if (!success)
                BadRequest("Unable to mark key as invalid");

            return Ok();
        }

        /// <summary>
        /// Generates short-time JWT token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("jwt")]
        public async Task<ActionResult<string>> GenerateJwtToken()
        {
            if (!Request.Headers.ContainsKey(AuthConstants.ApiKeyHeaderName))
                return Unauthorized($"{AuthConstants.ApiKeyHeaderName} is missing");

            var apiKey = Request.Headers[AuthConstants.ApiKeyHeaderName];
            var isKeyValid = await _apiKeyManager.IsKeyValid(apiKey);
            if (!isKeyValid)
                return Unauthorized("Key is invalid");
            var jwt = JwtGenerator.GenerateProtectedToken(_jwtOptions.Value.Key, _jwtOptions.Value.Iss);
            
            return Ok(jwt);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var isLoginValid = await _userManager.CheckLoginValid(loginRequest.Login, loginRequest.Password);
            if (!isLoginValid)
                return Unauthorized();
            var user = await _userManager.GetUserByLogin(loginRequest.Login);
            
            var roles = await _roleManager.GetRolesById(user.Id);
            if (roles?.Any() != true)
                return NotFound();
            
            var authToken =
                JwtGenerator.GenerateUserToken(_jwtOptions.Value.Key, _jwtOptions.Value.Iss, roles, user.Id);

            var refreshToken = JwtGenerator.GenerateUserToken(_jwtOptions.Value.Key, _jwtOptions.Value.Iss,
                new[] {AuthConstants.RefreshTokenRole}, user.Id);

            return Ok(new LoginResponse {AuthToken = authToken, RefreshToken = refreshToken});
        }

        [HttpPost("refresh")]
        [Authorize(Roles = AuthConstants.RefreshTokenRole)]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        public async Task<ActionResult<LoginResponse>> Refresh()
        {
            var userId = HttpContext.User.Claims.ToList().First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            
            var roles = await _roleManager.GetRolesById(userId);
            if (roles?.Any() != true)
                return NotFound();
            
            var authToken =
                JwtGenerator.GenerateUserToken(_jwtOptions.Value.Key, _jwtOptions.Value.Iss, roles, userId);

            var refreshToken = JwtGenerator.GenerateUserToken(_jwtOptions.Value.Key, _jwtOptions.Value.Iss,
                new[] {AuthConstants.RefreshTokenRole}, userId);

            return Ok(new LoginResponse {AuthToken = authToken, RefreshToken = refreshToken});
        }
    }
}