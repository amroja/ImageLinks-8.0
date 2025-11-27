using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public partial class PagXXXXX
{
    public decimal DocId { get; set; }

    [Column("DOCN_LINK")]
    public decimal? DocnLink { get; set; }

    [Column("PAGE_SER")]
    public decimal? PageSer { get; set; }

    [Column("PAGE_ETITL")]
    public string? PageEtitl { get; set; }

    [Column("PAGE_ATITL")]
    public string? PageAtitl { get; set; }

    [Column("PAGE_SRC")]
    public decimal? PageSrc { get; set; }

    [Column("PAGE_DATE")]
    public decimal? PageDate { get; set; }

    [Column("LOCK_FLAG")]
    public decimal? LockFlag { get; set; }

    [Column("LPRSN_ID")]
    public decimal? LprsnId { get; set; }

    [Column("PAGE_VOL")]
    public string? PageVol { get; set; }

    [Column("PAGE_DIR")]
    public string? PageDir { get; set; }

    [Column("PAGE_NAME")]
    public string? PageName { get; set; }

    [Column("PAGE_TYPE")]
    public decimal? PageType { get; set; }

    [Column("UPD_DATE")]
    public decimal? UpdDate { get; set; }

    [Column("UPD_PRSN")]
    public string? UpdPrsn { get; set; }

    [Column("VERIFYPRSN")]
    public decimal? Verifyprsn { get; set; }

    [Column("VERIFYDATE")]
    public decimal? Verifydate { get; set; }

    [Column("REJECTFLAG")]
    public decimal? Rejectflag { get; set; }

    [Column("PAGE_DEL")]
    public decimal? PageDel { get; set; }

    [Column("OCRFLAG")]
    public decimal? Ocrflag { get; set; }

    [Column("IS_PUBLIC")]
    public decimal? IsPublic { get; set; }

    [Column("IS_ENCRYPTED")]
    public decimal? IsEncrypted { get; set; }

    [Column("PAGE_CNT")]
    public decimal? PageCnt { get; set; }
}
