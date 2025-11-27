using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Table("DB_LOGIN")]
[Index("LicenseDomainId", Name = "DOMAIN_ID_IDX")]
[Index("UserId", "LoginFrom", Name = "UC_USERID_LOGINFROM", IsUnique = true)]
public class DbLogin
{
    [Key]
    [Column("REC_ID")]
    [Precision(19)]
    public long RecId { get; set; }

    [Column("USER_ID")]
    [Precision(4)]
    public byte UserId { get; set; }

    [Column("LOGIN_DT", TypeName = "NCLOB")]
    public string LoginDt { get; set; } = null!;

    [Column("LOGIN_FROM")]
    [StringLength(255)]
    public string LoginFrom { get; set; } = null!;

    [Column("LICENSE_DOMAIN_ID")]
    [Precision(10)]
    public int? LicenseDomainId { get; set; }

    [Column("LAST_ACTION")]
    [StringLength(100)]
    public string? LastAction { get; set; }

    [Column("IP_ADDRESS")]
    [StringLength(100)]
    public string? IpAddress { get; set; }
}
