using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public class DrwObj
{
    [Column("DRW_ID")]
    public decimal? DrwId { get; set; }

    [Column("FOLD_ID")]
    public decimal? FoldId { get; set; }

    [Column("FOLD_SER")]
    public decimal? FoldSer { get; set; }
}
