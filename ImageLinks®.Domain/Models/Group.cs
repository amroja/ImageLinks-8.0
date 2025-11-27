using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Table("GROUPS")]
public class Group
{
    [Column("GROUP_ID")]
    public byte? GroupId { get; set; }

    [Column("GROUP_NAME")]
    public string? GroupName { get; set; }

    [Column("GROUP_STAT")]
    public byte? GroupStat { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    public int? LicenseDomainId { get; set; }
}
