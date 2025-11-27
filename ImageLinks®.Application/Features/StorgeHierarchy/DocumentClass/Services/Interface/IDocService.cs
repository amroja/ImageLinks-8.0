using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Interface;

public interface IDocService
{
    Task<Result<List<DocDto>>> GetDocs(DocumentsRequest filter, CancellationToken ct = default);
    Task<Result<List<DocDto>>> GetDocumentsByParent(DocumentsRequest filter, CancellationToken ct = default);
}
