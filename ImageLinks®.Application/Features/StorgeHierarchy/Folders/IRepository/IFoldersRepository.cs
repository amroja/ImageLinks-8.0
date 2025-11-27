using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Folders.IRepository;
public interface IFoldersRepository : IRepository<FoldersModel>
{
    Task<List<FoldersModel>> SelectAsync(FoldersRequest filter, CancellationToken ct = default);
}
