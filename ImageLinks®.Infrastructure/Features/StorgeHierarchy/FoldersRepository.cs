using Dapper;
using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.StorgeHierarchy;
public class FoldersRepository : Repository<FoldersModel>, IFoldersRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;

    public FoldersRepository(ApplicationDbContext db, IGenericRepository genericService):base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<FoldersModel>> SelectAsync(FoldersRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT
                FOLD_ID            AS FoldId,
                FNAME_ARB          AS FnameArb,
                FNAME_ENG          AS FnameEng,
                FOLD_STAT          AS FoldStat,
                LICENSE_DOMAIN_ID  AS LicenseDomainId
            FROM FOLDERS
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.FoldId))
        {
            sql.Append($" AND FOLD_ID = {GeneralFunction.GetParam("FoldId", dbType)}");
            parameters.Add("FoldId", filter.FoldId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameArb))
        {
            sql.Append($" AND LOWER(FNAME_ARB) = LOWER({GeneralFunction.GetParam("FnameArb", dbType)})");
            parameters.Add("FnameArb", filter.FnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameEng))
        {
            sql.Append($" AND LOWER(FNAME_ENG) = LOWER({GeneralFunction.GetParam("FnameEng", dbType)})");
            parameters.Add("FnameEng", filter.FnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.FoldStat))
        {
            sql.Append($" AND FOLD_STAT = {GeneralFunction.GetParam("FoldStat", dbType)}");
            parameters.Add("FoldStat", filter.FoldStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        return await _genericService.GetListAsync<FoldersModel>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<FoldersModel>> SelectActiveFoldersForGroupsLevelOne(FoldersRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                f.Fold_ID     AS FoldId,
                f.FName_ARB   AS FnameArb,
                f.FName_ENG   AS FnameEng,
                dobj.FOLD_SER AS FoldSer
            FROM Folders f
            INNER JOIN DrwObj dobj    ON f.Fold_ID     = dobj.Fold_ID
            INNER JOIN Drawers d      ON dobj.Drw_ID   = d.Drw_ID
            INNER JOIN CabObj cobj    ON d.Drw_ID      = cobj.Drw_ID
            INNER JOIN Cabinets cab   ON cobj.Cab_ID   = cab.Cab_ID
            INNER JOIN Tree_Obj tob   ON cab.Cab_ID    = tob.Obj_ID
            INNER JOIN Trees t        ON tob.Tree_ID   = t.Tree_ID
            LEFT JOIN GroupSec gs     ON f.Fold_ID     = gs.Obj_ID
            LEFT JOIN GroupSec gs1    ON d.Drw_ID      = gs1.Obj_ID
            LEFT JOIN GroupSec gs2    ON cab.Cab_ID    = gs2.Obj_ID
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

        if (!string.IsNullOrWhiteSpace(filter.DrawerId))
        {
            sql.Append($" AND d.Drw_ID = {GeneralFunction.GetParam("DrwId", dbType)}");
            parameters.Add("DrwId", filter.DrawerId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FoldId))
        {
            sql.Append($" AND f.Fold_ID = {GeneralFunction.GetParam("FoldId", dbType)}");
            parameters.Add("FoldId", filter.FoldId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameArb))
        {
            sql.Append($" AND LOWER(f.FNAME_ARB) = LOWER({GeneralFunction.GetParam("FnameArb", dbType)})");
            parameters.Add("FnameArb", filter.FnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameEng))
        {
            sql.Append($" AND LOWER(f.FNAME_ENG) = LOWER({GeneralFunction.GetParam("FnameEng", dbType)})");
            parameters.Add("FnameEng", filter.FnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND f.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        sql.Append($" AND f.FOLD_STAT = 1");

        if (filter.GroupIds != null && filter.GroupIds.Any())
        {
            sql.Append($" AND gs.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
            sql.Append($" AND gs1.Group_ID IN {GeneralFunction.GetParam("GroupIds1", dbType)}");
            sql.Append($" AND gs2.Group_ID IN {GeneralFunction.GetParam("GroupIds2", dbType)}");
            parameters.Add("GroupIds", filter.GroupIds);
            parameters.Add("GroupIds1", filter.GroupIds);
            parameters.Add("GroupIds2", filter.GroupIds);
        }

        sql.Append($" AND gs.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.FOLDER_PRIVILEGES}");
        sql.Append($" AND gs1.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");
        sql.Append($" AND gs2.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.CABINET_PRIVILEGES}");

        return await _genericService.GetListAsync<FoldersModel>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<FoldersModel>> SelectActiveFoldersForGroupsLevelThree(FoldersRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                f.Fold_ID     AS FoldId,
                f.FName_ARB   AS FnameArb,
                f.FName_ENG   AS FnameEng,
                dobj.FOLD_SER AS FoldSer
            FROM Folders f
            INNER JOIN DrwObj dobj  ON f.Fold_ID   = dobj.Fold_ID
            INNER JOIN Drawers d    ON dobj.Drw_ID = d.Drw_ID
            INNER JOIN CabObj cobj  ON d.Drw_ID    = cobj.Drw_ID
            LEFT JOIN GroupSec gs   ON f.Fold_ID   = gs.Obj_ID
            LEFT JOIN GroupSec gs1  ON d.Drw_ID    = gs1.Obj_ID
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.ParentId))
        {
            sql.Append($" AND cobj.Cab_ID = {GeneralFunction.GetParam("CabId", dbType)}");
            parameters.Add("CabId", filter.ParentId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DrawerId))
        {
            sql.Append($" AND d.Drw_ID = {GeneralFunction.GetParam("DrwId", dbType)}");
            parameters.Add("DrwId", filter.DrawerId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FoldId))
        {
            sql.Append($" AND f.Fold_ID = {GeneralFunction.GetParam("FoldId", dbType)}");
            parameters.Add("FoldId", filter.FoldId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameArb))
        {
            sql.Append($" AND LOWER(f.FNAME_ARB) = LOWER({GeneralFunction.GetParam("FnameArb", dbType)})");
            parameters.Add("FnameArb", filter.FnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameEng))
        {
            sql.Append($" AND LOWER(f.FNAME_ENG) = LOWER({GeneralFunction.GetParam("FnameEng", dbType)})");
            parameters.Add("FnameEng", filter.FnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND f.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        sql.Append($" AND f.FOLD_STAT = 1");

        if (filter.GroupIds != null && filter.GroupIds.Any())
        {
            sql.Append($" AND gs.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
            sql.Append($" AND gs1.Group_ID IN {GeneralFunction.GetParam("GroupIds1", dbType)}");
            parameters.Add("GroupIds", filter.GroupIds);
            parameters.Add("GroupIds1", filter.GroupIds);
        }

        sql.Append($" AND gs.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.FOLDER_PRIVILEGES}");
        sql.Append($" AND gs1.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");

        return await _genericService.GetListAsync<FoldersModel>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<FoldersModel>> SelectActiveFoldersForUserLevelOne(FoldersRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                f.Fold_ID     AS FoldId,
                f.FName_ARB   AS FnameArb,
                f.FName_ENG   AS FnameEng,
                dobj.FOLD_SER AS FoldSer
            FROM Folders f
            INNER JOIN DrwObj dobj    ON f.Fold_ID     = dobj.Fold_ID
            INNER JOIN Drawers d      ON dobj.Drw_ID   = d.Drw_ID
            INNER JOIN CabObj cobj    ON d.Drw_ID      = cobj.Drw_ID
            INNER JOIN Cabinets cab   ON cobj.Cab_ID   = cab.Cab_ID
            INNER JOIN Tree_Obj tob   ON cab.Cab_ID    = tob.Obj_ID
            INNER JOIN Trees t        ON tob.Tree_ID   = t.Tree_ID
            LEFT JOIN UserSec us      ON f.Fold_ID     = us.Obj_ID
            LEFT JOIN UserSec us1     ON d.Drw_ID      = us1.Obj_ID
            LEFT JOIN UserSec us2     ON cab.Cab_ID    = us2.Obj_ID
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

        if (!string.IsNullOrWhiteSpace(filter.DrawerId))
        {
            sql.Append($" AND d.Drw_ID = {GeneralFunction.GetParam("DrwId", dbType)}");
            parameters.Add("DrwId", filter.DrawerId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FoldId))
        {
            sql.Append($" AND f.Fold_ID = {GeneralFunction.GetParam("FoldId", dbType)}");
            parameters.Add("FoldId", filter.FoldId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameArb))
        {
            sql.Append($" AND LOWER(f.FNAME_ARB) = LOWER({GeneralFunction.GetParam("FnameArb", dbType)})");
            parameters.Add("FnameArb", filter.FnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameEng))
        {
            sql.Append($" AND LOWER(f.FNAME_ENG) = LOWER({GeneralFunction.GetParam("FnameEng", dbType)})");
            parameters.Add("FnameEng", filter.FnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND f.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        sql.Append($" AND f.FOLD_STAT = 1");

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            sql.Append($" AND us.User_ID = {GeneralFunction.GetParam("UserId", dbType)}");
            sql.Append($" AND us1.User_ID = {GeneralFunction.GetParam("UserId1", dbType)}");
            sql.Append($" AND us2.User_ID = {GeneralFunction.GetParam("UserId2", dbType)}");
            parameters.Add("UserId", filter.UserId);
            parameters.Add("UserId1", filter.UserId);
            parameters.Add("UserId2", filter.UserId);
        }

        sql.Append($" AND us.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.FOLDER_PRIVILEGES}");
        sql.Append($" AND us1.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");
        sql.Append($" AND us2.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.CABINET_PRIVILEGES}");

        return await _genericService.GetListAsync<FoldersModel>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<FoldersModel>> SelectActiveFoldersForUserLevelThree(FoldersRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                f.Fold_ID     AS FoldId,
                f.FName_ARB   AS FnameArb,
                f.FName_ENG   AS FnameEng,
                dobj.FOLD_SER AS FoldSer
            FROM Folders f
            INNER JOIN DrwObj dobj  ON f.Fold_ID   = dobj.Fold_ID
            INNER JOIN Drawers d    ON dobj.Drw_ID = d.Drw_ID
            INNER JOIN CabObj cobj  ON d.Drw_ID    = cobj.Drw_ID
            LEFT JOIN UserSec us    ON f.Fold_ID   = us.Obj_ID
            LEFT JOIN UserSec us1   ON d.Drw_ID    = us1.Obj_ID
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.ParentId))
        {
            sql.Append($" AND cobj.Cab_ID = {GeneralFunction.GetParam("CabId", dbType)}");
            parameters.Add("CabId", filter.ParentId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DrawerId))
        {
            sql.Append($" AND d.Drw_ID = {GeneralFunction.GetParam("DrwId", dbType)}");
            parameters.Add("DrwId", filter.DrawerId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FoldId))
        {
            sql.Append($" AND f.Fold_ID = {GeneralFunction.GetParam("FoldId", dbType)}");
            parameters.Add("FoldId", filter.FoldId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameArb))
        {
            sql.Append($" AND LOWER(f.FNAME_ARB) = LOWER({GeneralFunction.GetParam("FnameArb", dbType)})");
            parameters.Add("FnameArb", filter.FnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameEng))
        {
            sql.Append($" AND LOWER(f.FNAME_ENG) = LOWER({GeneralFunction.GetParam("FnameEng", dbType)})");
            parameters.Add("FnameEng", filter.FnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND f.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        sql.Append($" AND f.FOLD_STAT = 1");

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            sql.Append($" AND us.User_ID = {GeneralFunction.GetParam("UserId", dbType)}");
            sql.Append($" AND us1.User_ID = {GeneralFunction.GetParam("UserId1", dbType)}");
            parameters.Add("UserId", filter.UserId);
            parameters.Add("UserId1", filter.UserId);
        }

        sql.Append($" AND us.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.FOLDER_PRIVILEGES}");
        sql.Append($" AND us1.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");

        return await _genericService.GetListAsync<FoldersModel>(sql.ToString(), parameters, null, ct);
    }
}
