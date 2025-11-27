using ImageLinks_.Application.Features.Groups.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Groups.IRepository;

public interface IGroupSecRepository : IRepository<GroupSec>
{
    Task<List<GroupSec>> SelectAsync(GroupSec filter, CancellationToken ct = default);

    Task<List<GroupSec>> GetByGroupIds(List<string> groupIds, CancellationToken ct = default);

    Task<List<GroupSec>> GetByFilter(GroupSecRequest filter, CancellationToken ct = default);
}
