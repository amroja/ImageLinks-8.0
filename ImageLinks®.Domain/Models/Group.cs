using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Keyless]
[Table("GROUPS")]
public partial class Group
{
    [Column("GROUP_ID")]
    [Precision(4)]
    public byte? GroupId { get; set; }

    [Column("GROUP_NAME")]
    [StringLength(40)]
    [Unicode(false)]
    public string? GroupName { get; set; }

    [Column("GROUP_STAT")]
    [Precision(4)]
    public byte? GroupStat { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    [Precision(10)]
    public int? LicenseDomainId { get; set; }
}
