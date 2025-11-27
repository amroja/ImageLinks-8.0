using ImageLinks_.Application.Features.Users.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Users.IRepository;

public interface IUserSecRepository : IRepository<UserSec>
{
    Task<List<UserSec>> SelectAsync(UserSec filter, CancellationToken ct = default);

    Task<List<UserSec>> GetByUserIds(List<string> userIds, CancellationToken ct = default);

    Task<List<UserSec>> GetByFilter(UserSecRequest filter, CancellationToken ct = default);
}
