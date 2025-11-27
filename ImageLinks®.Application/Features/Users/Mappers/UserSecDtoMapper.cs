using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Users.Mappers;

public static class UserSecDtoMapper
{
    private static UserSecDto ToDto(this UserSec entity)
    {
        return new UserSecDto(
            entity.UserId?.ToString(),
            entity.ObjLevel?.ToString(),
            entity.ObjId?.ToString(),
            entity.ObjFlag?.ToString()
        );
    }

    public static List<UserSecDto> ToDtos(this IEnumerable<UserSec> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
