using ImageLinks_.Application.Features.StorgeHierarchy.Trees.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Trees.Services.Interface;
public interface ITreeService
{
    Task<Result<List<TreeDto>>> GetActiveTreesForUser(TreeRequest tree, CancellationToken ct = default);
}
