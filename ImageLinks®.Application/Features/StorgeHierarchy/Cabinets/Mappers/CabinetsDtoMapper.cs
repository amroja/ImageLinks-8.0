using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.DTO;
using ImageLinks_.Domain.Models;
namespace ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Mappers;

public static class CabinetsDtoMapper
{
    private static CabinetsDto ToDto(this CabinetsModel entity)
    {
        return new CabinetsDto(
            entity.CabId?.ToString(),
            entity.CnameArb,
            entity.CnameEng,
            entity.CabStat?.ToString(),
            entity.LicenseDomainId?.ToString()
        );
    }

    public static List<CabinetsDto> ToDtos(this IEnumerable<CabinetsModel> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
