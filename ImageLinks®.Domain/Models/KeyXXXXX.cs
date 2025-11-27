using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public class KeyXXXXX
{
    public decimal DocId { get; set; }

    [Column("DOCN_LINK")]
    public decimal? DocnLink { get; set; }

    [Column("KEY_SER")]
    public decimal? KeySer { get; set; }

    [Column("KEYWORD")]
    public string? Keyword { get; set; }

    [Column("KEY_TYPE")]
    public decimal? KeyType { get; set; }

    [Column("PAGE_SER")]
    public decimal? PageSer { get; set; }
}
