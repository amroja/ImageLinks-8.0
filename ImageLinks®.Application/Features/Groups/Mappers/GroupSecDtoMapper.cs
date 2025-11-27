using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Groups.Mappers;

public static class GroupSecDtoMapper
{
    private static GroupSecDto ToDto(this GroupSec entity)
    {
        return new GroupSecDto(
            entity.GroupId?.ToString(),
            entity.ObjLevel?.ToString(),
            entity.ObjId?.ToString(),
            entity.ObjFlag?.ToString()
        );
    }

    public static List<GroupSecDto> ToDtos(this IEnumerable<GroupSec> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
