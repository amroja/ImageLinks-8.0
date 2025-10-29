using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Keyless]
[Table("GROUPSEC")]
public partial class Groupsec
{
    [Column("GROUP_ID")]
    [Precision(4)]
    public byte? GroupId { get; set; }

    [Column("OBJ_LEVEL")]
    [Precision(4)]
    public byte? ObjLevel { get; set; }

    [Column("OBJ_ID")]
    [Precision(5)]
    public short? ObjId { get; set; }

    [Column("OBJ_FLAG", TypeName = "NUMBER(30)")]
    public decimal? ObjFlag { get; set; }
}
