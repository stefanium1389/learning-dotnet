
using Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.RequestFeatures;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public UserController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers([FromQuery] UserParameters userParameters)
        {
            var (userDtos, metaData) = await _serviceManager.UserService.GetAllUsersAsync(userParameters);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(userDtos);
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] UserCreationDto userCreationDto)
        {
            var user = await _serviceManager.UserService.CreateUserAsync(userCreationDto);
            return Ok(user);
        }

        [HttpPost]
        [Route("registerManager")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UserDto>> RegisterManager([FromBody] UserCreationDto userCreationDto)
        {
            var user = await _serviceManager.UserService.CreateManagerAsync(userCreationDto);
            return Ok(user);
        }
        [HttpPatch]
        [Route("disable/{username}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DisableUser([FromRoute] string username)
        {
            await _serviceManager.UserService.SetUserEnabled(username, false);
            return NoContent();
        }
        [HttpPatch]
        [Route("enable/{username}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EnableUser([FromRoute] string username)
        {
            await _serviceManager.UserService.SetUserEnabled(username, true);
            return NoContent();
        }
        [HttpGet]
        [Route("suspicious")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetSuspiciousUsers([FromQuery] UserParameters userParameters)
        {
            var (userDtos, metaData) = await _serviceManager.UserService.GetSuspiciousUsersAsync(userParameters);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(userDtos);
        }
        [HttpGet]
        [Authorize]
        [Route("account")]
        public async Task<ActionResult<UserDto>> GetOwnAccount()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserDto user = await _serviceManager.UserService.GetUserByIdAsync(Guid.Parse(id!));
            return Ok(user);
        }
        [HttpPut]
        [Authorize]
        [Route("account")]
        public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserDto updatedUser = await _serviceManager.UserService.UpdateUserAsync(id, userUpdateDto);
            return Ok(updatedUser);
        }
    }
}
