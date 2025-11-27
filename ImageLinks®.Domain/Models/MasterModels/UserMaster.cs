using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models.MasterModels;
[Table("Users")]
public class UserMaster
{
    [Key]
    [Column("REC_ID")]
    public decimal RecId { get; set; }  // ✅ غيّرها لـ decimal

    [Column("USER_NAME")]
    [Required]
    public string UserName { get; set; } = null!;

    [Column("PASSWORD")]
    [Required]
    public string Password { get; set; } = null!;

    [Column("PRIVILIGE")]
    [Required]
    public string Privilige { get; set; } = null!;

    [Column("CREATED_BY")]
    public decimal? CreatedBy { get; set; }  // ✅ decimal

    [Column("CREATE_DATE")]
    [Required]
    public string CreateDate { get; set; } = null!;

    [Column("LAST_UPDATE_DATE")]
    [Required]
    public string LastUpdateDate { get; set; } = null!;

    [Column("LAST_UPDATE_USER")]
    public decimal? LastUpdateUser { get; set; }  // ✅ decimal

    // Navigation Properties
    [ForeignKey("CreatedBy")]
    [InverseProperty("InverseCreatedByNavigation")]
    public virtual UserMaster? CreatedByNavigation { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserMaster> InverseCreatedByNavigation { get; set; } = new List<UserMaster>();

    [ForeignKey("LastUpdateUser")]
    [InverseProperty("InverseLastUpdateUserNavigation")]
    public virtual UserMaster? LastUpdateUserNavigation { get; set; }

    [InverseProperty("LastUpdateUserNavigation")]
    public virtual ICollection<UserMaster> InverseLastUpdateUserNavigation { get; set; } = new List<UserMaster>();

    // TenantAppConfig Relations
    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TenantAppConfig> TenantAppConfigCreatedByNavigation { get; set; } = new List<TenantAppConfig>();

    [InverseProperty("LastUpdateUserNavigation")]
    public virtual ICollection<TenantAppConfig> TenantAppConfigLastUpdateUserNavigation { get; set; } = new List<TenantAppConfig>();

    // TenantConfig Relations
    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TenantConfig> TenantConfigCreatedByNavigation { get; set; } = new List<TenantConfig>();

    [InverseProperty("LastUpdateUserNavigation")]
    public virtual ICollection<TenantConfig> TenantConfigLastUpdateUserNavigation { get; set; } = new List<TenantConfig>();
}
