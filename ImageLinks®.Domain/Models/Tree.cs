using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("TREES")]
public class Tree
{
    [Column("TREE_ID")]
    public byte? TreeId { get; set; }

    [Column("RNAME_ARB")]
    public string? RnameArb { get; set; }

    [Column("RNAME_ENG")]
    public string? RnameEng { get; set; }

    [Column("CAB_NUM")]
    public byte? CabNum { get; set; }

    [Column("DRW_NUM")]
    public byte? DrwNum { get; set; }

    [Column("FOLD_NUM")]
    public byte? FoldNum { get; set; }

    [Column("DOC_NUM")]
    public byte? DocNum { get; set; }

    [Column("TREE_STAT")]
    public byte? TreeStat { get; set; }

    [Column("TREE_LVL")]
    public byte? TreeLvl { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    public int? LicenseDomainId { get; set; }
}
