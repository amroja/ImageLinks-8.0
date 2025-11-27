using Dapper;
using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.StorgeHierarchy
{
    public class DrawersRepository : Repository<DrawersModel>, IDrawersRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IGenericRepository _genericService;

        public DrawersRepository(ApplicationDbContext db, IGenericRepository genericService)
        {
            _db = db;
            _genericService = genericService;
        }

        public async Task<List<DrawersModel>> SelectAsync(DrawersRequest filter, CancellationToken ct = default)
        {
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            StringBuilder sql = new StringBuilder(@"
              SELECT
                  DRW_ID             AS DrwId            ,
                  DNAME_ARB          AS DnameArb         ,
                  DNAME_ENG          AS DnameEng         ,
                  DRW_STAT           AS DrwStat          ,
                  LICENSE_DOMAIN_ID  AS LicenseDomainId
              FROM DRAWERS
              WHERE 1 = 1
        ");

            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.DrwId))
            {
                sql.Append($" AND DRW_ID = {GeneralFunction.GetParam("DrwId", dbType)}");
                parameters.Add("DrwId", filter.DrwId);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameArb))
            {
                sql.Append($" AND LOWER(DNAME_ARB) = LOWER({GeneralFunction.GetParam("DnameArb", dbType)})");
                parameters.Add("DnameArb", filter.DnameArb);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameEng))
            {
                sql.Append($" AND LOWER(DNAME_ENG) = LOWER({GeneralFunction.GetParam("DnameEng", dbType)})");
                parameters.Add("DnameEng", filter.DnameEng);
            }

            if (!string.IsNullOrWhiteSpace(filter.DrwStat))
            {
                sql.Append($" AND DRW_STAT = {GeneralFunction.GetParam("DrwStat", dbType)}");
                parameters.Add("DrwStat", filter.DrwStat);
            }

            if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
            {
                sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
                parameters.Add("LicenseDomainId", filter.LicenseDomainId);
            }

            return await _genericService.GetListAsync<DrawersModel>(sql.ToString(), parameters, null, ct);
        }

        public async Task<List<DrawersModel>> SelectActiveDrawersForGroupsLevelOne(DrawersRequest filter, CancellationToken ct = default)
        {
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            StringBuilder sql = new StringBuilder(@"
                  SELECT DISTINCT
                      d.Drw_ID      AS DrwId   ,
                      d.DName_ARB   AS DnameArb,
                      d.DName_ENG   AS DnameEng,
                      cobj.DRW_SER  AS DrwSer
                  FROM Tree_Obj tob
                  INNER JOIN Trees t      ON tob.Tree_ID = t.Tree_ID
                  INNER JOIN Cabinets cab ON tob.Obj_ID  = cab.Cab_ID
                  INNER JOIN CabObj cobj  ON cab.Cab_ID  = cobj.Cab_ID
                  INNER JOIN Drawers d    ON cobj.Drw_ID = d.Drw_ID
                  LEFT JOIN GroupSec gs   ON d.Drw_ID    = gs.Obj_ID
                  WHERE 1 = 1
            ");

            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.TreeId))
            {
                sql.Append($" AND t.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
                parameters.Add("TreeId", filter.TreeId);
            }

            if (!string.IsNullOrWhiteSpace(filter.ParentId))
            {
                sql.Append($" AND cab.Cab_ID = {GeneralFunction.GetParam("CabId", dbType)}");
                parameters.Add("CabId", filter.ParentId);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameArb))
            {
                sql.Append($" AND LOWER(d.DNAME_ARB) = LOWER({GeneralFunction.GetParam("DnameArb", dbType)})");
                parameters.Add("DnameArb", filter.DnameArb);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameEng))
            {
                sql.Append($" AND LOWER(d.DNAME_ENG) = LOWER({GeneralFunction.GetParam("DnameEng", dbType)})");
                parameters.Add("DnameEng", filter.DnameEng);
            }

            if (!string.IsNullOrWhiteSpace(filter.DrwStat))
            {
                sql.Append($" AND d.DRW_STAT = {GeneralFunction.GetParam("DrwStat", dbType)}");
                parameters.Add("DrwStat", filter.DrwStat);
            }

            if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
            {
                sql.Append($" AND d.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
                parameters.Add("LicenseDomainId", filter.LicenseDomainId);
            }

            if (filter.GroupIds != null && filter.GroupIds.Any())
            {
                sql.Append($" AND gs.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
                parameters.Add("GroupIds", filter.GroupIds);
            }

            sql.Append($" AND gs.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");

            return await _genericService.GetListAsync<DrawersModel>(sql.ToString(), parameters, null, ct);
        }

        public async Task<List<DrawersModel>> SelectActiveDrawersForGroupsLevelTwo(DrawersRequest filter, CancellationToken ct = default)
        {
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            StringBuilder sql = new StringBuilder(@"
              SELECT DISTINCT
                  d.Drw_ID     AS DrwId,
                  d.DName_ARB  AS DnameArb,
                  d.DName_ENG  AS DnameEng,
                  c.DRW_SER    AS DrwSer
              FROM CabObj c
              INNER JOIN Drawers d ON c.Drw_ID = d.Drw_ID
              LEFT JOIN GroupSec gs ON d.Drw_ID = gs.Obj_ID
              WHERE 1 = 1
              ");

            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.DnameArb))
            {
                sql.Append($" AND LOWER(d.DNAME_ARB) = LOWER({GeneralFunction.GetParam("DnameArb", dbType)})");
                parameters.Add("DnameArb", filter.DnameArb);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameEng))
            {
                sql.Append($" AND LOWER(d.DNAME_ENG) = LOWER({GeneralFunction.GetParam("DnameEng", dbType)})");
                parameters.Add("DnameEng", filter.DnameEng);
            }

            if (!string.IsNullOrWhiteSpace(filter.DrwStat))
            {
                sql.Append($" AND d.DRW_STAT = {GeneralFunction.GetParam("DrwStat", dbType)}");
                parameters.Add("DrwStat", filter.DrwStat);
            }

            if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
            {
                sql.Append($" AND d.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
                parameters.Add("LicenseDomainId", filter.LicenseDomainId);
            }

            if (!string.IsNullOrWhiteSpace(filter.ParentId))
            {
                sql.Append($" AND c.Cab_ID = {GeneralFunction.GetParam("CabId", dbType)}");
                parameters.Add("CabId", filter.ParentId);
            }

            if (filter.GroupIds != null && filter.GroupIds.Any())
            {
                sql.Append($" AND gs.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
                parameters.Add("GroupIds", filter.GroupIds);
            }

            sql.Append($" AND gs.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");

            return await _genericService.GetListAsync<DrawersModel>(sql.ToString(), parameters, null, ct);
        }

        public async Task<List<DrawersModel>> SelectActiveDrawersForUserLevelOne(DrawersRequest filter, CancellationToken ct = default)
        {
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            StringBuilder sql = new StringBuilder(@"
              SELECT DISTINCT
                  d.Drw_ID      AS DrwId   ,
                  d.DName_ARB   AS DnameArb,
                  d.DName_ENG   AS DnameEng,
                  cobj.DRW_SER  AS DrwSer
              FROM Tree_Obj tob
              INNER JOIN Trees t      ON tob.Tree_ID = t.Tree_ID
              INNER JOIN Cabinets cab ON tob.Obj_ID  = cab.Cab_ID
              INNER JOIN CabObj cobj  ON cab.Cab_ID  = cobj.Cab_ID
              INNER JOIN Drawers d    ON cobj.Drw_ID = d.Drw_ID
              LEFT JOIN UserSec us    ON d.Drw_ID    = us.Obj_ID
              WHERE 1 = 1
        ");

            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.TreeId))
            {
                sql.Append($" AND t.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
                parameters.Add("TreeId", filter.TreeId);
            }

            if (!string.IsNullOrWhiteSpace(filter.ParentId))
            {
                sql.Append($" AND cab.Cab_ID = {GeneralFunction.GetParam("CabId", dbType)}");
                parameters.Add("CabId", filter.ParentId);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameArb))
            {
                sql.Append($" AND LOWER(d.DNAME_ARB) = LOWER({GeneralFunction.GetParam("DnameArb", dbType)})");
                parameters.Add("DnameArb", filter.DnameArb);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameEng))
            {
                sql.Append($" AND LOWER(d.DNAME_ENG) = LOWER({GeneralFunction.GetParam("DnameEng", dbType)})");
                parameters.Add("DnameEng", filter.DnameEng);
            }

            if (!string.IsNullOrWhiteSpace(filter.DrwStat))
            {
                sql.Append($" AND d.DRW_STAT = {GeneralFunction.GetParam("DrwStat", dbType)}");
                parameters.Add("DrwStat", filter.DrwStat);
            }

            if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
            {
                sql.Append($" AND d.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
                parameters.Add("LicenseDomainId", filter.LicenseDomainId);
            }

            if (!string.IsNullOrWhiteSpace(filter.UserId))
            {
                sql.Append($" AND us.User_ID = {GeneralFunction.GetParam("UserId", dbType)}");
                parameters.Add("UserId", filter.UserId);
            }

            sql.Append($" AND us.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");

            return await _genericService.GetListAsync<DrawersModel>(sql.ToString(), parameters, null, ct);
        }

        public async Task<List<DrawersModel>> SelectActiveDrawersForUserLevelTwo(DrawersRequest filter, CancellationToken ct = default)
        {
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            StringBuilder sql = new StringBuilder(@"
              SELECT DISTINCT
                  d.Drw_ID     AS DrwId   ,
                  d.DName_ARB  AS DnameArb,
                  d.DName_ENG  AS DnameEng,
                  c.DRW_SER    AS DrwSer
              FROM CabObj c
              INNER JOIN Drawers d ON c.Drw_ID = d.Drw_ID
              LEFT JOIN UserSec us ON d.Drw_ID = us.Obj_ID
              WHERE 1 = 1
        ");

            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.DnameArb))
            {
                sql.Append($" AND LOWER(d.DNAME_ARB) = LOWER({GeneralFunction.GetParam("DnameArb", dbType)})");
                parameters.Add("DnameArb", filter.DnameArb);
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameEng))
            {
                sql.Append($" AND LOWER(d.DNAME_ENG) = LOWER({GeneralFunction.GetParam("DnameEng", dbType)})");
                parameters.Add("DnameEng", filter.DnameEng);
            }

            if (!string.IsNullOrWhiteSpace(filter.DrwStat))
            {
                sql.Append($" AND d.DRW_STAT = {GeneralFunction.GetParam("DrwStat", dbType)}");
                parameters.Add("DrwStat", filter.DrwStat);
            }

            if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
            {
                sql.Append($" AND d.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
                parameters.Add("LicenseDomainId", filter.LicenseDomainId);
            }

            if (!string.IsNullOrWhiteSpace(filter.ParentId))
            {
                sql.Append($" AND c.Cab_ID = {GeneralFunction.GetParam("CabId", dbType)}");
                parameters.Add("CabId", filter.ParentId);
            }

            if (!string.IsNullOrWhiteSpace(filter.UserId))
            {
                sql.Append($" AND us.User_ID = {GeneralFunction.GetParam("UserId", dbType)}");
                parameters.Add("UserId", filter.UserId);
            }

            sql.Append($" AND us.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");

            return await _genericService.GetListAsync<DrawersModel>(sql.ToString(), parameters, null, ct);
        }
    }
}
