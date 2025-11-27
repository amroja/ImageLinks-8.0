using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models.MasterModels;
public class TenantConfig
{
    [Key]
    [Column("REC_ID")]
    public decimal RecId { get; set; }

    [Column("TENANT_NAME")]
    public string TenantName { get; set; } = null!;

    [Column("LICENSE")]
    public string License { get; set; } = null!;

    [Column("CONNECTION")]
    public string Connection { get; set; } = null!;

    [Column("STATUS")]
    public string Status { get; set; } = null!;

    [Column("CREATED_BY")]
    public decimal? CreatedBy { get; set; }

    [Column("CREATE_DATE")]
    public string CreateDate { get; set; } = null!;

    [Column("LAST_UPDATE_USER")]
    public decimal? LastUpdateUser { get; set; }

    [Column("LAST_UPDATE_DATE")]
    public string LastUpdateDate { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("TenantConfigCreatedByNavigation")]
    public virtual UserMaster? CreatedByNavigation { get; set; }

    [ForeignKey("LastUpdateUser")]
    [InverseProperty("TenantConfigLastUpdateUserNavigation")]
    public virtual UserMaster? LastUpdateUserNavigation { get; set; }

    [InverseProperty("Tenant")]
    public virtual ICollection<TenantAppConfig> TenantAppConfig { get; set; } = new List<TenantAppConfig>();
}
