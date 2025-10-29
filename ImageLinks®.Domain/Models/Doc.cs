using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Keyless]
[Table("DOCS")]
public partial class Doc
{
    [Column("DOC_ID")]
    [Precision(5)]
    public short? DocId { get; set; }

    [Column("DOC_CRSTAT")]
    [Precision(4)]
    public byte? DocCrstat { get; set; }

    [Column("DCNAME_ARB")]
    [StringLength(40)]
    [Unicode(false)]
    public string? DcnameArb { get; set; }

    [Column("DCNAME_ENG")]
    [StringLength(40)]
    [Unicode(false)]
    public string? DcnameEng { get; set; }

    [Column("DC_RCOUNT")]
    [Precision(10)]
    public int? DcRcount { get; set; }

    [Column("DC_CABRIV")]
    [StringLength(10)]
    [Unicode(false)]
    public string? DcCabriv { get; set; }

    [Column("UPD_DATE")]
    [Precision(10)]
    public int? UpdDate { get; set; }

    [Column("UPD_PRSN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UpdPrsn { get; set; }

    [Column("DOC_STAT")]
    [Precision(4)]
    public byte? DocStat { get; set; }

    [Column("MAX_VERNUM")]
    [Precision(4)]
    public byte? MaxVernum { get; set; }

    [Column("OCR_TEMPLT")]
    [StringLength(255)]
    [Unicode(false)]
    public string? OcrTemplt { get; set; }

    [Column("RET_PRD")]
    [Precision(10)]
    public int? RetPrd { get; set; }

    [Column("OCR_PAGENUM")]
    [Precision(4)]
    public byte? OcrPagenum { get; set; }

    [Column("OCR_EVERYPAGE")]
    [Precision(4)]
    public byte? OcrEverypage { get; set; }

    [Column("WF_CLASS")]
    [StringLength(50)]
    [Unicode(false)]
    public string? WfClass { get; set; }

    [Column("DOCPHASE")]
    [Precision(4)]
    public byte? Docphase { get; set; }

    [Column("CAT_ID")]
    [Precision(4)]
    public byte? CatId { get; set; }

    [Column("TEMP_ID")]
    [Precision(4)]
    public byte? TempId { get; set; }

    [Column("OCR_TYPE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? OcrType { get; set; }

    [Column("FILES_FILTER")]
    [StringLength(50)]
    [Unicode(false)]
    public string? FilesFilter { get; set; }

    [Column("IS_PUBLIC", TypeName = "NUMBER(1)")]
    public bool? IsPublic { get; set; }

    [Column("ROLE_ID", TypeName = "NUMBER(38)")]
    public decimal? RoleId { get; set; }

    [Column("DOCN_SOURCE")]
    [Precision(2)]
    public byte? DocnSource { get; set; }

    [Column("HAS_ROLES", TypeName = "NUMBER(1)")]
    public bool? HasRoles { get; set; }

    [Column("HAS_BATCH_NAME", TypeName = "NUMBER(1)")]
    public bool? HasBatchName { get; set; }

    [Column("HAS_FILE_CLASSIFICATION", TypeName = "NUMBER(1)")]
    public bool? HasFileClassification { get; set; }

    [Column("BARCODEZONE")]
    [Precision(4)]
    public byte? Barcodezone { get; set; }

    [Column("BARCODEFORMAT")]
    [StringLength(40)]
    [Unicode(false)]
    public string? Barcodeformat { get; set; }

    [Column("HAS_WATERMARK", TypeName = "NUMBER(1)")]
    public bool? HasWatermark { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    [Precision(10)]
    public int? LicenseDomainId { get; set; }

    [Column("NOT_MATCHED_OP")]
    [Precision(4)]
    public byte? NotMatchedOp { get; set; }

    [Required]
    [Column("EXPAND_G_INDEX", TypeName = "NUMBER(1)")]
    public bool? ExpandGIndex { get; set; }

    [Required]
    [Column("EXPAND_C_INDEX", TypeName = "NUMBER(1)")]
    public bool? ExpandCIndex { get; set; }

    [Column("MATCHING_OPTIONS")]
    [Precision(4)]
    public byte? MatchingOptions { get; set; }

    [Column("AUTO_INDEX_OP")]
    [Precision(4)]
    public byte? AutoIndexOp { get; set; }

    [Column("MAX_DOCN_COUNT", TypeName = "NUMBER")]
    public decimal? MaxDocnCount { get; set; }
}
