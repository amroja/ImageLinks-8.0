using ImageLinks_.API.Middleware;
using ImageLinks_.Application.Common;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Requests;
using ImageLinks_.Domain.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ImageLinks_.Application.Common.Enums.GeneralEnums;

namespace ImageLinks_.API.Controllers.StorgeHierarchy;

[Route("api/[controller]")]
[Authorize]
public class CabinetsController : ApiController
{
    private readonly ICabinetsService _cabinetsService;
    IUser _user;

    public CabinetsController(ICabinetsService cabinetsService, IUser user)
    {
        _cabinetsService = cabinetsService;
        _user = user;
    }

    [HttpPost("Select")]
    [ApiAuthorization(UserPrivileges.CanShowUserDocuments)]
    public async Task<IActionResult> SelectAsync([FromBody] CabinetsRequest filter, CancellationToken ct)
    {
        CabinetsRequest cabinetsRequest = new CabinetsRequest();
        filter.UserId = _user.Id;
        cabinetsRequest = filter;
        Result<List<CabinetsDto>>? result = await _cabinetsService.GetCabinetsByParent(filter, ct);
        return result.Match(response => Ok(response), Problem);
    }
}
