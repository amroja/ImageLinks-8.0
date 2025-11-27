using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("USERSEC")]
public class UserSec
{
    [Column("USER_ID")]
    public byte? UserId { get; set; }

    [Column("OBJ_LEVEL")]
    public byte? ObjLevel { get; set; }

    [Column("OBJ_ID")]
    public short? ObjId { get; set; }

    [Column("OBJ_FLAG", TypeName = "NUMBER(30)")]
    public decimal? ObjFlag { get; set; }
}
