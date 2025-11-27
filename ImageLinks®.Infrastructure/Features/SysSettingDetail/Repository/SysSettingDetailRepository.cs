using Dapper;
using ImageLinks_.Application;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.SysSettingDetail.DTO;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure
{
    public class SysSettingDetailRepository : Repository<SysSettingDetail>, ISysSettingDetailRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IGenericRepository _genericService;
        public SysSettingDetailRepository(ApplicationDbContext db, IGenericRepository genericService) : base(db)
        {
            _db = db;
            _genericService = genericService;
        }

        public void Update(SysSettingDetail entity)
        {
            _db.SETT_SYSSETTDETAILS.Update(entity);
        }

        public async Task<List<SysSettingDetail>?> SelectAsync(SysSettingDetailDto filter, CancellationToken ct = default)
        {
            var dbType = _genericService.GetDatabaseType();
            var sql = new StringBuilder(@"
                                                     SELECT 
                                                         SYS_SETT_ID                AS SysSettId       ,
                                                         SYS_SETT_TYPE_ID           AS SysSettTypeId   ,
                                                         SUB_SETTING_DESC_EN        AS SubSettingDescEn,
                                                         SUB_SETTING_DESC_AR        AS SubSettingDescAr,
                                                         USER_ID                    AS UserId          ,
                                                         SYS_SETT_VALUE             AS SysSettValue    ,
                                                         CRT_BY                     AS CrtBy           ,
                                                         CRT_DT                     AS CrtDt
                                                     FROM SETT_SYSSETTDETAILS 
                                                     WHERE 1 = 1
                                                    ");
            DynamicParameters? parameters = new DynamicParameters();


            if (!string.IsNullOrWhiteSpace(filter.SettId))
            {
                sql.Append($" AND SYS_SETT_ID = {GeneralFunction.GetParam("SysSettId", dbType)}");
                parameters.Add("SysSettId", filter.SettId);
            }

            if (!string.IsNullOrWhiteSpace(filter.SettTypeId))
            {
                sql.Append($" AND SYS_SETT_TYPE_ID = {GeneralFunction.GetParam("SysSettTypeId", dbType)}");
                parameters.Add("SysSettTypeId", filter.SettTypeId);
            }

            if (!string.IsNullOrWhiteSpace(filter.DescEn))
            {
                sql.Append($" AND LOWER(SUB_SETTING_DESC_EN) = LOWER({GeneralFunction.GetParam("SubSettingDescEn", dbType)})");
                parameters.Add("SubSettingDescEn", filter.DescEn);
            }

            if (!string.IsNullOrWhiteSpace(filter.DescAr))
            {
                sql.Append($" AND LOWER(SUB_SETTING_DESC_AR) = LOWER({GeneralFunction.GetParam("SubSettingDescAr", dbType)})");
                parameters.Add("SubSettingDescAr", filter.DescAr);
            }

            if (!string.IsNullOrWhiteSpace(filter.UserId))
            {
                sql.Append($" AND USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
                parameters.Add("UserId", filter.UserId);
            }

            if (!string.IsNullOrWhiteSpace(filter.SettValue))
            {
                sql.Append($" AND LOWER(SYS_SETT_VALUE) = LOWER({GeneralFunction.GetParam("SysSettValue", dbType)})");
                parameters.Add("SysSettValue", filter.SettValue);
            }

            if (!string.IsNullOrWhiteSpace(filter.CrtBy))
            {
                sql.Append($" AND CRT_BY = {GeneralFunction.GetParam("CrtBy", dbType)}");
                parameters.Add("CrtBy", filter.CrtBy);
            }

            if (!string.IsNullOrWhiteSpace(filter.CrtDt))
            {
                sql.Append($" AND CRT_DT = {GeneralFunction.GetParam("CrtDt", dbType)}");
                parameters.Add("CrtDt", filter.CrtBy);
            }

            var list = await _genericService.GetListAsync<SysSettingDetail>(sql.ToString(), parameters, null, ct);
            return list;
        }
    }
}
