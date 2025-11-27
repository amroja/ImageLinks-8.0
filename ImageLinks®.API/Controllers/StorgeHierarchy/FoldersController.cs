using ImageLinks_.Application.Common;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers.StorgeHierarchy;

[Route("api/[controller]")]
public class FoldersController : ApiController
{
    private readonly IFoldersService _foldersService;
    IUser _user;
    public FoldersController(IFoldersService foldersService, IUser user)
    {
        _foldersService = foldersService;
        _user = user;
    }

    [HttpPost("Select")]
    public async Task<IActionResult> SelectAsync([FromBody] FoldersRequest filter, CancellationToken ct)
    {
        FoldersRequest cabinetsRequest = new FoldersRequest();
        filter.UserId = _user.Id;
        cabinetsRequest = filter;
        var result = await _foldersService.GetFoldersByParent(filter, ct);
        return Ok(result);
    }
}
