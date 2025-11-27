using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;
public class Cabinets
{
    [Column("CAB_ID")]
    public decimal? CabId { get; set; }

    [Column("CNAME_ARB")]
    public string? CnameArb { get; set; }

    [Column("CNAME_ENG")]
    public string? CnameEng { get; set; }

    [Column("CAB_STAT")]
    public decimal? CabStat { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    public decimal? LicenseDomainId { get; set; }
}
