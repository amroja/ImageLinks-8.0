using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Mappers;

public static class DocDtoMapper
{
    private static DocDto ToDto(this Doc entity)
    {
        return new DocDto(
            entity.DocId?.ToString(),
            entity.DocCrstat?.ToString(),
            entity.DcnameArb,
            entity.DcnameEng,
            entity.DcRcount?.ToString(),
            entity.DcCabriv,
            entity.UpdDate?.ToString(),
            entity.UpdPrsn,
            entity.DocStat?.ToString(),
            entity.MaxVernum?.ToString(),
            entity.OcrTemplt,
            entity.RetPrd?.ToString(),
            entity.OcrPagenum?.ToString(),
            entity.OcrEverypage?.ToString(),
            entity.WfClass,
            entity.Docphase?.ToString(),
            entity.CatId?.ToString(),
            entity.TempId?.ToString(),
            entity.OcrType,
            entity.FilesFilter,
            entity.IsPublic?.ToString(),
            entity.RoleId?.ToString(),
            entity.DocnSource?.ToString(),
            entity.HasRoles?.ToString(),
            entity.HasBatchName?.ToString(),
            entity.HasFileClassification?.ToString(),
            entity.Barcodezone?.ToString(),
            entity.Barcodeformat,
            entity.HasWatermark?.ToString(),
            entity.LicenseDomainId?.ToString(),
            entity.NotMatchedOp?.ToString(),
            entity.ExpandGIndex?.ToString(),
            entity.ExpandCIndex?.ToString(),
            entity.MatchingOptions?.ToString(),
            entity.AutoIndexOp?.ToString(),
            entity.MaxDocnCount?.ToString()
        );
    }

    public static List<DocDto> ToDtos(this IEnumerable<Doc> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
