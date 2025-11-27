using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.IRepository;
using ImageLinks_.Application.Features.Groups.Requests;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.Groups.Mappers;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;


namespace ImageLinks_.Application.Features.Groups.Services.Implementation;

public class GroupSecService : IGroupSecService
{
    private readonly IGroupSecRepository _repo;

    public GroupSecService(IGroupSecRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<GroupSecDto>> SelectAsync(GroupSecRequest filter, CancellationToken ct = default)
    {
        var entity = new GroupSec
        {
            GroupId = byte.TryParse(filter.GroupId, out var gid) ? gid : null,
            ObjLevel = byte.TryParse(filter.ObjLevel, out var lvl) ? lvl : null,
            ObjId = short.TryParse(filter.ObjId, out var oid) ? oid : null
        };

        var list = await _repo.SelectAsync(entity, ct);

        return list.ToDtos();
    }

    public async Task<Result<List<GroupSecDto>>> GetByGroupIds(List<string> groupIds, CancellationToken ct = default)
    {
        var list = await _repo.GetByGroupIds(groupIds, ct);

        return list.ToDtos();
    }

    public async Task<Result<List<GroupSecDto>>> GetByFilter(GroupSecRequest filter, CancellationToken ct = default)
    {
        var list = await _repo.GetByFilter(filter, ct);

        return list.ToDtos();
    }
}
