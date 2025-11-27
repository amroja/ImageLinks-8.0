using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Interface;

public interface IDocService
{
    Task<Result<List<DocDto>>> GetDocs(DocRequest filter, CancellationToken ct = default);
}
