using ImageLinks_.Application.Common;
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
    IUser _user;
    public DrawersController(IDrawersService drawersService, IUser user)
    {
        _drawersService = drawersService;
        _user = user;
    }

    [HttpPost("Select")]
    public async Task<IActionResult> SelectAsync([FromBody] DrawersRequest filter, CancellationToken ct)
    {
        DrawersRequest cabinetsRequest = new DrawersRequest();
        filter.UserId = _user.Id;
        cabinetsRequest = filter;
        var result = await _drawersService.GetDrawersByParent(filter, ct);
        return Ok(result);
    }
}
