using ImageLinks_.Domain.Models;
using System.Data;

namespace ImageLinks_.Infrastructure
{
    public static class SysSettingDetailEntityMapper
    {
        public static SysSettingDetail ToSysSettingDetailEntity(this DataRow row)
        {
            ArgumentNullException.ThrowIfNull(row);

            return new SysSettingDetail
            {
                SysSettId = row["SYS_SETT_ID"] != DBNull.Value ? Convert.ToInt64(row["SYS_SETT_ID"]) : 0,
                SysSettTypeId = row["SYS_SETT_TYPE_ID"] != DBNull.Value ? Convert.ToInt64(row["SYS_SETT_TYPE_ID"]) : 0,
                SubSettingDescEn = row["SUB_SETTING_DESC_EN"].ToString() ?? string.Empty,
                SubSettingDescAr = row["SUB_SETTING_DESC_AR"].ToString() ?? string.Empty,
                UserId = row["USER_ID"] != DBNull.Value ? Convert.ToInt64(row["USER_ID"]) : -1,
                SysSettValue = row["SYS_SETT_VALUE"].ToString() ?? string.Empty,
                CrtBy = row["CRT_BY"] != DBNull.Value ? Convert.ToInt64(row["CRT_BY"]) : null,
                CrtDt = row["CRT_DT"] != DBNull.Value ? Convert.ToInt64(row["CRT_DT"]) : 0,
            };
        }

        public static List<SysSettingDetail> ToSysSettingDetailsEntities(this DataTable table)
        {
            ArgumentNullException.ThrowIfNull(table);

            return table.AsEnumerable()
                        .Select(r => r.ToSysSettingDetailEntity())
                        .ToList();
        }
    }
}
