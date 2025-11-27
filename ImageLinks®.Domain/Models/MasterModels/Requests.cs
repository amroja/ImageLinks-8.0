using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models.MasterModels;
public class Requests
{
    [Key]
    [Column("REC_ID")]
    public decimal RecId { get; set; }

    [Column("TYPE")]
    public string Type { get; set; } = null!;

    [Column("STATUS")]
    public string Status { get; set; } = null!;

    [Column("TENANT_ID")]
    public decimal? TenantId { get; set; }

    [Column("DATA")]
    public string? Data { get; set; }

    [Column("REQUESTED_BY")]
    public decimal? RequestedBy { get; set; }

    [Column("REQUESTED_DATE")]
    public string? RequestedDate { get; set; }

    [ForeignKey("RequestedBy")]
    [InverseProperty("Requests")]
    public virtual UserMaster? RequestedByNavigation { get; set; }
}
