using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.IRepository;
using ImageLinks_.Application.Features.Groups.Services.Interface;

namespace ImageLinks_.Application.Features.Groups.Services.Implementation;
public class GroupMbrService : IGroupMbrService
{
    private readonly IGroupMbrRepository _groupMbrRepository;

    public GroupMbrService(IGroupMbrRepository groupMbrRepository)
    {
        _groupMbrRepository = groupMbrRepository;
    }

    public Task<List<string>> GetGroupsByUser(GroupMbrDto groupMbrDto, CancellationToken ct = default)
    {
       return _groupMbrRepository.GetGroupsByUser(groupMbrDto, ct);
    }
}
