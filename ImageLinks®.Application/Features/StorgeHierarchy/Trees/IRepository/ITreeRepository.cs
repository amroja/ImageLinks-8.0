using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Trees.IRepository;
public interface ITreeRepository : IRepository<Tree>
{
    Task<List<Tree>> SelectAsync(Tree filter, CancellationToken ct = default);

    Task<List<Tree>> GetActiveTreesForUser(TreeRequest filter, CancellationToken ct = default);

    Task<List<Tree>> GetTreesByGroups(TreeRequest filter, CancellationToken ct = default);
}
