using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("GROUPMBR")]
public class GroupMbr
{
    [Column("GROUP_ID")]
    public byte? GroupId { get; set; }

    [Column("USER_ID")]
    public byte? UserId { get; set; }
}
