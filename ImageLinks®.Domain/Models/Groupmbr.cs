using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Keyless]
[Table("GROUPMBR")]
public partial class Groupmbr
{
    [Column("GROUP_ID")]
    [Precision(4)]
    public byte? GroupId { get; set; }

    [Column("USER_ID")]
    [Precision(4)]
    public byte? UserId { get; set; }
}
