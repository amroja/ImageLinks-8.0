using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("DOCS")]
public class Doc
{
    [Column("DOC_ID")]
    public short? DocId { get; set; }

    [Column("DOC_CRSTAT")]
    public byte? DocCrstat { get; set; }

    [Column("DCNAME_ARB")]
    public string? DcnameArb { get; set; }

    [Column("DCNAME_ENG")]
    public string? DcnameEng { get; set; }

    [Column("DC_RCOUNT")]
    public int? DcRcount { get; set; }

    [Column("DC_CABRIV")]
    public string? DcCabriv { get; set; }

    [Column("UPD_DATE")]
    public int? UpdDate { get; set; }

    [Column("UPD_PRSN")]
    public string? UpdPrsn { get; set; }

    [Column("DOC_STAT")]
    public byte? DocStat { get; set; }

    [Column("MAX_VERNUM")]
    public byte? MaxVernum { get; set; }

    [Column("OCR_TEMPLT")]
    public string? OcrTemplt { get; set; }

    [Column("RET_PRD")]
    public int? RetPrd { get; set; }

    [Column("OCR_PAGENUM")]
    public byte? OcrPagenum { get; set; }

    [Column("OCR_EVERYPAGE")]
    public byte? OcrEverypage { get; set; }

    [Column("WF_CLASS")]
    public string? WfClass { get; set; }

    [Column("DOCPHASE")]
    public byte? Docphase { get; set; }

    [Column("CAT_ID")]
    public byte? CatId { get; set; }

    [Column("TEMP_ID")]
    public byte? TempId { get; set; }

    [Column("OCR_TYPE")]
    public string? OcrType { get; set; }

    [Column("FILES_FILTER")]
    public string? FilesFilter { get; set; }

    [Column("IS_PUBLIC", TypeName = "NUMBER(1)")]
    public bool? IsPublic { get; set; }

    [Column("ROLE_ID", TypeName = "NUMBER(38)")]
    public decimal? RoleId { get; set; }

    [Column("DOCN_SOURCE")]
    public byte? DocnSource { get; set; }

    [Column("HAS_ROLES", TypeName = "NUMBER(1)")]
    public bool? HasRoles { get; set; }

    [Column("HAS_BATCH_NAME", TypeName = "NUMBER(1)")]
    public bool? HasBatchName { get; set; }

    [Column("HAS_FILE_CLASSIFICATION", TypeName = "NUMBER(1)")]
    public bool? HasFileClassification { get; set; }

    [Column("BARCODEZONE")]
    public byte? Barcodezone { get; set; }

    [Column("BARCODEFORMAT")]
    public string? Barcodeformat { get; set; }

    [Column("HAS_WATERMARK", TypeName = "NUMBER(1)")]
    public bool? HasWatermark { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    public int? LicenseDomainId { get; set; }

    [Column("NOT_MATCHED_OP")]
    public byte? NotMatchedOp { get; set; }

    [Required]
    [Column("EXPAND_G_INDEX", TypeName = "NUMBER(1)")]
    public bool? ExpandGIndex { get; set; }

    [Required]
    [Column("EXPAND_C_INDEX", TypeName = "NUMBER(1)")]
    public bool? ExpandCIndex { get; set; }

    [Column("MATCHING_OPTIONS")]
    public byte? MatchingOptions { get; set; }

    [Column("AUTO_INDEX_OP")]
    public byte? AutoIndexOp { get; set; }

    [Column("MAX_DOCN_COUNT", TypeName = "NUMBER")]
    public decimal? MaxDocnCount { get; set; }
}
