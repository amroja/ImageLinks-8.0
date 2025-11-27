using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public class CabObj
{
    [Column("CAB_ID")]
    public decimal? CabId { get; set; }

    [Column("DRW_ID")]
    public decimal? DrwId { get; set; }

    [Column("DRW_SER")]
    public decimal? DrwSer { get; set; }
}
