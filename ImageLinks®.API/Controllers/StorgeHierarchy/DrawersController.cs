using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers.StorgeHierarchy;

[Route("api/[controller]")]
[Authorize]
public class DrawersController : ApiController
{
    private readonly IDrawersService _drawersService;

    public DrawersController(IDrawersService drawersService)
    {
        _drawersService = drawersService;
    }

    [HttpPost("Select")]
    public async Task<IActionResult> SelectAsync([FromBody] DrawersRequest filter, CancellationToken ct)
    {
        var result = await _drawersService.SelectAsync(filter, ct);
        return Ok(result);
    }
}
