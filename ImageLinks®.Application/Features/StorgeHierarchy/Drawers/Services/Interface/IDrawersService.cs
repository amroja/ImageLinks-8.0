using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;

using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Interface;

public interface IDrawersService
{
    Task<Result<List<DrawersDto>>> SelectAsync(DrawersRequest filter, CancellationToken ct = default);

    Task<Result<List<DrawersDto>>> GetDrawersByParent(DrawersRequest filter, CancellationToken ct = default);
}
