using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models
{
    public  class SettSysSettTypes
    {
        [Key]
        [Column("SYS_SETT_TYPE_ID")]
        public decimal SysSettTypeId { get; set; }

        [Column("SYS_TYPE_DESC_EN")]
        public string? SysTypeDescEn { get; set; }

        [Column("SYS_TYPE_DESC_AR")]
        public string? SysTypeDescAr { get; set; }

        public virtual ICollection<SysSettingDetail> SettSysSettDetails { get; set; } = new List<SysSettingDetail>();
    }
}
