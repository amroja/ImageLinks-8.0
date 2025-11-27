using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public class FoldObj
{
    [Column("FOLD_ID")]
    public decimal? FoldId { get; set; }

    [Column("DOC_ID")]
    public decimal? DocId { get; set; }

    [Column("DOC_SER")]
    public decimal? DocSer { get; set; }
}
