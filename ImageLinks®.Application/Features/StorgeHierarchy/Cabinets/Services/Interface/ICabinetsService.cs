using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Services.Interface;

public interface ICabinetsService
{
    Task<Result<List<CabinetsDto>>> GetCabinets(CabinetsRequest filter, CancellationToken ct = default);

    Task<Result<List<CabinetsDto>>> GetCabinetsByParent(CabinetsRequest filter, CancellationToken ct = default);
}
