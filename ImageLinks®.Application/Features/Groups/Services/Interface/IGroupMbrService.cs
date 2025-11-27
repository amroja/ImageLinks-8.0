using ImageLinks_.Application.Features.Groups.DTO;

namespace ImageLinks_.Application.Features.Groups.Services.Interface;
public interface  IGroupMbrService
{
    Task<List<string>> GetGroupsByUser(GroupMbrDto groupMbrDto, CancellationToken ct = default);
}
