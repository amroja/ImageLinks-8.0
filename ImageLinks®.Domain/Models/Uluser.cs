using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Domain.Models;

[Keyless]
[Table("ULUSERS")]
public partial class Uluser
{
    [Column("USER_ID")]
    [Precision(4)]
    public byte? UserId { get; set; }

    [Column("SERVER_NAME")]
    [StringLength(64)]
    [Unicode(false)]
    public string? ServerName { get; set; }

    [Column("DOMAIN_NAME")]
    [StringLength(64)]
    [Unicode(false)]
    public string? DomainName { get; set; }
}
