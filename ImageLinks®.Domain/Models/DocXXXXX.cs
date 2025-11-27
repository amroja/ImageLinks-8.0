using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public class DocXXXXX
{
    public decimal DocId { get; set; }

    [Column("DOCN_LINK")]
    public decimal? DocnLink { get; set; }

    [Column("DOCN_ETITL")]
    public string? DocnEtitl { get; set; }

    [Column("DOCN_ATITL")]
    public string? DocnAtitl { get; set; }

    [Column("DOCN_DATE")]
    public decimal? DocnDate { get; set; }

    [Column("DOCN_CABRV")]
    public string? DocnCabrv { get; set; }

    [Column("UPD_DATE")]
    public decimal? UpdDate { get; set; }

    [Column("UPD_PRSN")]
    public string? UpdPrsn { get; set; }

    [Column("LOCK_FLAG")]
    public decimal? LockFlag { get; set; }

    [Column("LPRSN_ID")]
    public decimal? LprsnId { get; set; }

    [Column("REF_COUNT")]
    public decimal? RefCount { get; set; }

    [Column("DOCMODE")]
    public decimal? Docmode { get; set; }

    [Column("FOLD_VAL")]
    public string? FoldVal { get; set; }

    [Column("DOCN_LVL")]
    public decimal? DocnLvl { get; set; }

    [Column("DOCN_DEL")]
    public decimal? DocnDel { get; set; }

    [Column("OCRFLAG")]
    public decimal? Ocrflag { get; set; }

    [Column("ROLE_ID")]
    public decimal? RoleId { get; set; }

    [Column("DOCN_SOURCE")]
    public decimal? DocnSource { get; set; }
}
