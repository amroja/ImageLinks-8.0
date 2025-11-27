using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Mappers;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Implementation;
public class DrawersService : IDrawersService
{
    private readonly IDrawersRepository _drawersRepository;

    public DrawersService(IDrawersRepository drawersRepository)
    {
        _drawersRepository = drawersRepository;
    }

    public async Task<Result<List<DrawersDto>>> SelectAsync(DrawersRequest filter, CancellationToken ct = default)
    {
        var list = await _drawersRepository.SelectAsync(filter, ct);
        return list.ToDtos();
    }
}
