using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Groups.IRepository;
public interface IGroupRepository : IRepository<Group>
{
    Task<List<Group>> SelectAsync(Group filter, CancellationToken ct = default);
}
