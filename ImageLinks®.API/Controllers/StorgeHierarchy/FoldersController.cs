using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers.StorgeHierarchy;

[Route("api/[controller]")]
public class FoldersController : ApiController
{
    private readonly IFoldersService _foldersService;

    public FoldersController(IFoldersService foldersService)
    {
        _foldersService = foldersService;
    }

    [HttpPost("Select")]
    public async Task<IActionResult> SelectAsync([FromBody] FoldersRequest filter, CancellationToken ct)
    {
        var result = await _foldersService.SelectAsync(filter, ct);
        return Ok(result);
    }
}
