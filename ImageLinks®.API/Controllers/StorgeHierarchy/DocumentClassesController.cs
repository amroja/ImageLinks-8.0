using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers.StorgeHierarchy;

[Route("api/[controller]")]
[Authorize]
public class DocumentClassesController : ApiController
{
    private readonly IDocService _docService;

    public DocumentClassesController(IDocService docService)
    {
        _docService = docService;
    }

    [HttpPost("Select")]
    public async Task<IActionResult> SelectAsync([FromBody] DocumentsRequest docRequest, CancellationToken ct)
    {
        var result = await _docService.GetDocumentsByParent(docRequest, ct);

        return result.Match(response => Ok(response), Problem);
    }
}
