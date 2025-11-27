using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.Features.Users.Requests;
using ImageLinks_.Application.Features.Users.Services.Interface;
using ImageLinks_.Application.Features.Users.Mappers;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Users.Services.Implementation;

public class UserSecService : IUserSecService
{
    private readonly IUserSecRepository _repo;

    public UserSecService(IUserSecRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<UserSecDto>> SelectAsync(UserSecRequest filter, CancellationToken ct = default)
    {
        UserSec? entity = new UserSec
        {
            UserId = byte.TryParse(filter.UserId, out byte uid) ? uid : null,
            ObjLevel = byte.TryParse(filter.ObjLevel, out byte lvl) ? lvl : null,
            ObjId = short.TryParse(filter.ObjId, out short oid) ? oid : null
        };

        List<UserSec>? list = await _repo.SelectAsync(entity, ct);

        return list.ToDtos();
    }

    public async Task<Result<List<UserSecDto>>> GetByUserIds(List<string> userIds, CancellationToken ct = default)
    {
        var list = await _repo.GetByUserIds(userIds, ct);

        return list.ToDtos();
    }

    public async Task<Result<List<UserSecDto>>> GetByFilter(UserSecRequest filter, CancellationToken ct = default)
    {
        var list = await _repo.GetByFilter(filter, ct);

        return list.ToDtos();
    }
}
