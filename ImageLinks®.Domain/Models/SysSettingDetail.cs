using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("SETT_SYSSETTDETAILS")]
[PrimaryKey(nameof(SysSettId), nameof(SysSettTypeId))]
public class SysSettingDetail
{
    [Column("SYS_SETT_ID")]
    public decimal SysSettId { get; set; }

    [Column("SYS_SETT_TYPE_ID")]
    public decimal SysSettTypeId { get; set; }

    [Column("SUB_SETTING_DESC_EN")]
    public string? SubSettingDescEn { get; set; }

    [Column("SUB_SETTING_DESC_AR")]
    public string? SubSettingDescAr { get; set; }

    [Column("USER_ID")]
    public decimal? UserId { get; set; }

    [Column("SYS_SETT_VALUE")]
    public string? SysSettValue { get; set; }

    [Column("CRT_BY")]
    public decimal? CrtBy { get; set; }

    [Column("CRT_DT")]
    public decimal? CrtDt { get; set; }

    [Column("UPDT_BY")]
    public decimal? UpdtBy { get; set; }

    public virtual SettSysSettTypes SysSettType { get; set; } = null!;
}

