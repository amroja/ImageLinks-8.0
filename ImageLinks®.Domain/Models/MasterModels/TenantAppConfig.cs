using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models.MasterModels;
public class TenantAppConfig
{
    [Key]
    [Column("REC_ID")]
    public decimal RecId { get; set; }

    [Column("TENANT_ID")]
    public decimal TenantId { get; set; }

    [Column("APPLICATION_SERVER_IP")]
    public string ApplicationServerIp { get; set; } = null!;

    [Column("APPLICATION_SERVER_MAC")]
    public string ApplicationServerMac { get; set; } = null!;

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
    [InverseProperty("TenantAppConfigCreatedByNavigation")]
    public virtual UserMaster? CreatedByNavigation { get; set; }

    [ForeignKey("LastUpdateUser")]
    [InverseProperty("TenantAppConfigLastUpdateUserNavigation")]
    public virtual UserMaster? LastUpdateUserNavigation { get; set; }

    [ForeignKey("TenantId")]
    [InverseProperty("TenantAppConfig")]
    public virtual TenantConfig Tenant { get; set; } = null!;
}
