using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("DB_LOGIN")]
public class AnnGroups
{
    [Column("GROUP_ID")]
    public decimal? GroupId { get; set; }

    [Column("GROUP_NAME")]
    public string? GroupName { get; set; }

    [Column("GROUP_STAT")]
    public decimal? GroupStat { get; set; }

    [Column("ANN_DESC")]
    public string? AnnDesc { get; set; }
}
