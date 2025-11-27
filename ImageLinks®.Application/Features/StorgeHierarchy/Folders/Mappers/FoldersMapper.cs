using ImageLinks_.Application.Features.StorgeHierarchy.Folders.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Folders.Mappers;
public static class FoldersMapper
{
    private static FoldersDto ToDto(this FoldersModel entity)
    {
        return new FoldersDto(
            entity.FoldId?.ToString(),
            entity.FnameArb,
            entity.FnameEng,
            entity.FoldStat?.ToString(),
            entity.FoldVal,
            entity.LicenseDomainId?.ToString()
        );
    }

    public static List<FoldersDto> ToDtos(this IEnumerable<FoldersModel> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }
}
