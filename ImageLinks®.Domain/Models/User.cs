using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("USERS")]
public class User
{
    [Key]
    [Column("USER_ID")]
    public decimal UserId { get; set; } = -1;

    [Column("USER_NAME")]
    public string? UserName { get; set; }

    [Column("USER_MAIL")]
    public string? UserMail { get; set; }

    [Column("USER_CDATE")]
    public decimal? UserCDate { get; set; }

    [Column("USER_CUSER")]
    [MaxLength(40)]
    public string? UserCUser { get; set; }

    [Column("USER_STAT")]
    public decimal? UserStat { get; set; }

    [Column("USER_PASS")]
    [MaxLength(100)]
    public string? UserPass { get; set; }

    [Column("USER_ABRIV")]
    [MaxLength(10)]
    public string? UserAbriv { get; set; }

    [Column("USER_PTYPE")]
    public decimal? UserPType { get; set; }

    [Column("LOGIN_STAT")]
    public decimal? LoginStat { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    public int? LicenseDomainId { get; set; }

    [Column("UNSUCCESSFUL_LOGIN_COUNT")]
    public decimal UnsuccessfulLoginCount { get; set; }

    [Column("TFA_CONFIRMED")]
    public bool TfaConfirmed { get; set; }

    [Column("IS_MANAGER")]
    public bool? IsManager { get; set; }
}

