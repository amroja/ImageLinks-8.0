using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
namespace ImageLinks_.Application.Features.Groups.IRepository;

public interface IGroupMbrRepository : IRepository<GroupMbr>
{
    Task<List<GroupMbr>> SelectAsync(GroupMbr filter, CancellationToken ct = default);

    Task<List<string>> GetGroupsByUser(GroupMbrDto filter, CancellationToken ct = default);
}
