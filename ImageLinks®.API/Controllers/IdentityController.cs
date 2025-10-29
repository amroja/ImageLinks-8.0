using ImageLinks_.API.Extensions;
using ImageLinks_.Application.Features.Identity.DTO;
using ImageLinks_.Application.Features.Identity.Requests;
using ImageLinks_.Application.Features.Identity.Services.Interface;
using Microsoft.AspNetCore.Mvc;
namespace ImageLinks_.API.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : ApiController
    {
        private readonly IIdentityService _identity;

        public IdentityController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Generates an access and refresh token for a valid user.")]
        [EndpointDescription("Authenticates a user using provided credentials and returns a JWT token pair.")]
        [EndpointName("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginRequest request, CancellationToken ct)
        {
            var result = await _identity.Login(request, ct);
            return result.Match(
            response => Ok(response),
            Problem);

        }
    }
}
