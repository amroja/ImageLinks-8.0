using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Users.Mappers;
public static class UserDtoMapper
{
    private static UserDto ToDto(this User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return new UserDto(user.UserId.ToString(), user.UserName);

    }

    public static List<UserDto> ToDtos(this IEnumerable<User> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
