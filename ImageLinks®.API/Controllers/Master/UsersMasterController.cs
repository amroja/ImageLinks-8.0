using ImageLinks_.Application.Features.Master.UsersMaster.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers.Master
{
    [Route("api/[controller]")]
    public class UsersMasterController : ApiController
    {
        private readonly IUsersMasterService _usersMasterService;

        public UsersMasterController( IUsersMasterService usersMasterService)
        {
            _usersMasterService = usersMasterService;
        }

        [HttpPost("qw")]
        public async Task<IActionResult> GetAllEf( CancellationToken ct)
        {
            var result = await _usersMasterService.GetAllUsersMaster(ct);

            return result.Match( response => Ok(response), Problem);
        }
    }
}
