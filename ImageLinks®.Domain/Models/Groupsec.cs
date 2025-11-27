using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Table("GROUPSEC")]
public class GroupSec
{
    [Column("GROUP_ID")]
    public byte? GroupId { get; set; }

    [Column("OBJ_LEVEL")]
    public byte? ObjLevel { get; set; }

    [Column("OBJ_ID")]
    public short? ObjId { get; set; }

    [Column("OBJ_FLAG", TypeName = "NUMBER(30)")]
    public decimal? ObjFlag { get; set; }
}
