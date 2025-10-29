using ImageLinks_.Application.Common.Models;
using ImageLinks_.Application.Features.Users.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("ef")]

        public async Task<IActionResult> GetAllEf(PageRequest pageRequest, CancellationToken ct)
        {
            var result = await _userService.GetAllUsersEfFilter(pageRequest, ct);

            return result.Match(
            response => Ok(response),
            Problem);
        }

        [HttpGet("da")]
        [Authorize]
        public async Task<IActionResult> GetAllDa(CancellationToken ct)
        {
            var result = await _userService.GetAllUsersDP(ct);

            return result.Match(response => Ok(response), Problem);
        }
    }
}
