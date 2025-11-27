using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Mappers;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Implementation;

public class DocService : IDocService
{
    private readonly IDocRepository _repository;

    public DocService(IDocRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<DocDto>>> GetDocs(DocRequest filter, CancellationToken ct = default)
    {
        var list = await _repository.SelectAsync(filter, ct);
        return list.ToDtos();
    }
}
