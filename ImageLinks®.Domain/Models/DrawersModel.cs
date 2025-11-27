using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("DRAWERS")]
public class DrawersModel
{
    [Column("DRW_ID")]
    public decimal? DrwId { get; set; }

    [Column("DNAME_ARB")]
    public string? DnameArb { get; set; }

    [Column("DNAME_ENG")]
    public string? DnameEng { get; set; }

    [Column("DRW_STAT")]
    public decimal? DrwStat { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    public decimal? LicenseDomainId { get; set; }
}
