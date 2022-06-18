using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("/api/get-users")]
        public async Task<IActionResult> GetUserList([FromQuery] PaginationParams pParams)
        {
            var users = await _service.GetAllUserAsync(pParams);
            return Ok(PaginatedList<UserDto>.Create(users, pParams.PageNumber, pParams.pageSize, users.Select(u => u.TotalRowCount).FirstOrDefault()));
        }
    }
}
