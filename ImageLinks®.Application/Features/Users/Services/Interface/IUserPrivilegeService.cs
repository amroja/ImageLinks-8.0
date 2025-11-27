using ImageLinks_.Application.Features.Users.DTO;

namespace ImageLinks_.Application.Features.Users.Services.Interface;
public interface IUserPrivilegeService
{
    Task<UserPrivilegeDto> GetUserPrivilege(UserDto filter, CancellationToken ct = default);
}
