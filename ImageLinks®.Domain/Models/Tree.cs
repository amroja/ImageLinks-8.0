using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Keyless]
[Table("TREES")]
public partial class Tree
{
    [Column("TREE_ID")]
    [Precision(4)]
    public byte? TreeId { get; set; }

    [Column("RNAME_ARB")]
    [StringLength(40)]
    [Unicode(false)]
    public string? RnameArb { get; set; }

    [Column("RNAME_ENG")]
    [StringLength(40)]
    [Unicode(false)]
    public string? RnameEng { get; set; }

    [Column("CAB_NUM")]
    [Precision(4)]
    public byte? CabNum { get; set; }

    [Column("DRW_NUM")]
    [Precision(4)]
    public byte? DrwNum { get; set; }

    [Column("FOLD_NUM")]
    [Precision(4)]
    public byte? FoldNum { get; set; }

    [Column("DOC_NUM")]
    [Precision(4)]
    public byte? DocNum { get; set; }

    [Column("TREE_STAT")]
    [Precision(4)]
    public byte? TreeStat { get; set; }

    [Column("TREE_LVL")]
    [Precision(4)]
    public byte? TreeLvl { get; set; }

    [Column("LICENSE_DOMAIN_ID")]
    [Precision(10)]
    public int? LicenseDomainId { get; set; }
}
