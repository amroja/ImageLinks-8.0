using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public class Folders
{
    [Column("FOLD_ID")]
    public decimal? FoldId { get; set; }

    [Column("FNAME_ARB")]
    public string? FnameArb { get; set; }

    [Column("FNAME_ENG")]
    public string? FnameEng { get; set; }

    [Column("FOLD_STAT")]
    public decimal? FoldStat { get; set; }

    [Column("FOLD_VAL")]
    public string? FoldVal { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    public decimal? LicenseDomainId { get; set; }
}
