using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.IRepository;

public interface IDocRepository : IRepository<Doc>
{
    Task<List<Doc>> SelectAsync(DocRequest filter, CancellationToken ct = default);
}
