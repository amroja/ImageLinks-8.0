using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.IRepository;

public interface IDocRepository : IRepository<Doc>
{
    Task<List<Doc>> SelectAsync(DocumentsRequest filter, CancellationToken ct = default);
    Task<List<Doc>> SelectActiveDocumentsForGroupsLevelOne(DocumentsRequest filter, CancellationToken ct = default);
    Task<List<Doc>> SelectActiveDocumentsForGroupsLevelFour(DocumentsRequest filter, CancellationToken ct = default);
    Task<List<Doc>> SelectActiveDocumentsForUserLevelOne(DocumentsRequest filter, CancellationToken ct = default);
    Task<List<Doc>> SelectActiveDocumentsForUserLevelFour(DocumentsRequest filter, CancellationToken ct = default);
}
