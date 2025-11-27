using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Groups.Services.Interface;

public interface IGroupSecService
{
    Task<List<GroupSecDto>> SelectAsync(GroupSecRequest filter, CancellationToken ct = default);

    Task<Result<List<GroupSecDto>>> GetByGroupIds(List<string> groupIds, CancellationToken ct = default);

    Task<Result<List<GroupSecDto>>> GetByFilter(GroupSecRequest filter, CancellationToken ct = default);
}
