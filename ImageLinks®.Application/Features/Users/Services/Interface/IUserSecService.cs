using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Application.Features.Users.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Users.Services.Interface;

public interface IUserSecService
{
    Task<List<UserSecDto>> SelectAsync(UserSecRequest filter, CancellationToken ct = default);

    Task<Result<List<UserSecDto>>> GetByUserIds(List<string> userIds, CancellationToken ct = default);

    Task<Result<List<UserSecDto>>> GetByFilter(UserSecRequest filter, CancellationToken ct = default);
}
