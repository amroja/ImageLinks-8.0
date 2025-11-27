using ImageLinks_.Application.Common;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Services.Interface;
using ImageLinks_.Domain.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers.StorgeHierarchy;

[Route("api/[controller]")]
[Authorize]
public class TreeController : ApiController
{
    private readonly ITreeService _treeService;
    IUser _user;
    public TreeController(ITreeService treeService, IUser user)
    {
        _treeService = treeService;
        _user = user;
    }

    [HttpPost("Select")]
    public async Task<IActionResult> SelectAsync(CancellationToken ct)
    {
        TreeRequest treeRequest = new TreeRequest { UserId = _user.Id };
        Result<List<TreeDto>>? result = await _treeService.GetActiveTreesForUser(treeRequest, ct);

        return result.Match(response => Ok(response), Problem);
    }
}
