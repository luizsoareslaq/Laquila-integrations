using Laquila.Integrations.Application.DTO.Users.Request;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO dto)
        {
            var user = await _userService.CreateUserAsync(dto);

            return Created("Created.", user);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string orderBy = "id", bool ascending = true, [FromQuery] UserFilters? filters = null)
        {
            (var users, int count) = await _userService.GetUsers(page, pageSize, orderBy, ascending, filters);

            return Ok(new { Data = users, Count = count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserById(id);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserDTO dto)
        {
            await _userService.UpdateUser(id, dto);

            return NoContent();
        }
    }
}