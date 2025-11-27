using ImageLinks_.Application.Features.StorgeHierarchy.Folders.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Interface;

public interface IFoldersService
{
    Task<Result<List<FoldersDto>>> SelectAsync(FoldersRequest filter, CancellationToken ct = default);
}
