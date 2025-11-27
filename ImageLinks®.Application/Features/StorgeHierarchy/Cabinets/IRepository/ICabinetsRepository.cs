using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.IRepository;

public interface ICabinetsRepository : IRepository<CabinetsModel>
{
    Task<List<CabinetsModel>> SelectAsync(CabinetsRequest filter, CancellationToken ct = default);

    Task<List<CabinetsModel>> SelectByParentAsync(CabinetsRequest filter, CancellationToken ct = default);

    Task<List<CabinetsModel>> SelecActiveCabinetsForGroups(CabinetsRequest filter, CancellationToken ct = default);

    Task<List<CabinetsModel>> SelecActiveCabinetsForUser(CabinetsRequest filter, CancellationToken ct = default);

}
