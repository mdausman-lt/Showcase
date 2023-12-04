using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Api.Authentication;
using PhotoAlbum.Api.Repositories;
using PhotoAlbum.Api.Repositories.Interfaces;
using PhotoAlbum.Shared.Models;

namespace PhotoAlbum.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController(IUserRepository userRepository, JwtSettings jwtSettings) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly JwtSettings _jwtSecurity = jwtSettings;

        [HttpGet]
        public async Task<ActionResult<UserModel>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("{userId:int}")]
        public async Task<ActionResult<UserModel>> GetUserAsync(int userId)
        {
            var user = await _userRepository.GetUserAsync(userId);

            return user == null
                ? NotFound()
                : Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenModel>> LoginAsync([FromBody] LoginModel loginModel)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager((UserRepository)_userRepository, _jwtSecurity);
            TokenModel? tokenModel = await jwtAuthenticationManager.GenerateJwtTokenAsync(loginModel.Username, loginModel.Password, loginModel.RememberMe);

            return tokenModel == null
                ? Unauthorized()
                : Ok(tokenModel);
        }

        [HttpGet]
        [Route("{userId:int}/ValidatePassword/{password}")]
        public async Task<ActionResult<bool>> ValidatePasswordAsync(int userId, string password)
        {
            bool isValid = await userRepository.PasswordIsValidAsync(userId, password);
            return Ok(isValid);
        }
    }
}
