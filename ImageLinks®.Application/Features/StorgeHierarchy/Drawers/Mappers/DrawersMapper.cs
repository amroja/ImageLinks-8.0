using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Mappers
{
    public static class DrawersMapper
    {
        private static DrawersDto ToDto(this DrawersModel entity)
        {
            return new DrawersDto(
                entity.DrwId?.ToString(),
                entity.DnameArb,
                entity.DnameEng,
                entity.DrwStat?.ToString(),
                entity.LicenseDomainId?.ToString()
            );
        }

        public static List<DrawersDto> ToDtos(this IEnumerable<DrawersModel> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
}
