using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Drawers.IRepository;

public interface IDrawersRepository : IRepository<DrawersModel>
{
    Task<List<DrawersModel>> SelectAsync(DrawersRequest filter, CancellationToken ct = default);

    Task<List<DrawersModel>> SelecActiveDrawersForGroupsLevelOne(CabinetsRequest filter, CancellationToken ct = default);

    Task<List<DrawersModel>> SelecActiveDrawersForGroupsLevelTwo(CabinetsRequest filter, CancellationToken ct = default);

    Task<List<DrawersModel>> SelecActiveDrawersForUserLevelOne(CabinetsRequest filter, CancellationToken ct = default);

    Task<List<DrawersModel>> SelecActiveDrawersForUserLevelTwo(CabinetsRequest filter, CancellationToken ct = default);

}
