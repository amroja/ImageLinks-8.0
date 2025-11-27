using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models.MasterModels;

[Table("MASTER_CONFIG")]
public class MasterConfigModel
{
    [Key]
    [Column("REC_ID")]
    public int RecId { get; set; }

    [Column("CONNECTION")]
    public string Connection { get; set; } = null!;

    [Column("LICENSE")]
    public string License { get; set; } = null!;

    [InverseProperty("Master")]
    public virtual ICollection<MasterDbServersConfigModel> MasterDbServersConfig { get; set; } = new List<MasterDbServersConfigModel>();
}
