using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models.MasterModels;

[Table("MASTER_DB_SERVERS_CONFIG")]
public class MasterDbServersConfigModel
{
    [Key]
    [Column("REC_ID")]
    public int RecId { get; set; }

    [Column("MASTER_ID")]
    public int MasterId { get; set; }

    [Column("DB_SERVER_IP")]
    public string DbServerIp { get; set; } = null!;

    [Column("DB_SERVER_MAC")]
    public string DbServerMac { get; set; } = null!;

    [Column("STATUS")]
    public string Status { get; set; } = null!;

    [ForeignKey("MasterId")]
    [InverseProperty("MasterDbServersConfig")]
    public virtual MasterConfigModel Master { get; set; } = null!;
}
