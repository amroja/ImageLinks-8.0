using Dapper;
using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.StorgeHierarchy;
public class TreeRepository : Repository<Tree>, ITreeRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;
    public TreeRepository(ApplicationDbContext db, IGenericRepository genericService) : base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<Tree?>> SelectAsync(Tree filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();

        var sql = new StringBuilder(@"
                  SELECT 
                      TREE_ID            AS TreeId           ,
                      RNAME_ARB          AS RnameArb         ,
                      RNAME_ENG          AS RnameEng         ,
                      CAB_NUM            AS CabNum           ,
                      DRW_NUM            AS DrwNum           ,
                      FOLD_NUM           AS FoldNum          ,
                      DOC_NUM            AS DocNum           ,
                      TREE_STAT          AS TreeStat         ,
                      TREE_LVL           AS TreeLvl          ,
                      LICENSE_DOMAIN_ID  AS LicenseDomainId
                  FROM TREES
                  WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (filter.TreeId.HasValue)
        {
            sql.Append($" AND TREE_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.TreeId);
        }

        if (!string.IsNullOrEmpty(filter.RnameArb))
        {
            sql.Append($" AND LOWER(RNAME_ARB) = LOWER({GeneralFunction.GetParam("RnameArb", dbType)})");
            parameters.Add("RnameArb", filter.RnameArb.ToLower());
        }

        if (!string.IsNullOrEmpty(filter.RnameEng))
        {
            sql.Append($" AND LOWER(RNAME_ENG) = LOWER({GeneralFunction.GetParam("RnameEng", dbType)})");
            parameters.Add("RnameEng", filter.RnameEng.ToLower());
        }

        if (filter.CabNum.HasValue)
        {
            sql.Append($" AND CAB_NUM = {GeneralFunction.GetParam("CabNum", dbType)}");
            parameters.Add("CabNum", filter.CabNum);
        }

        if (filter.DrwNum.HasValue)
        {
            sql.Append($" AND DRW_NUM = {GeneralFunction.GetParam("DrwNum", dbType)}");
            parameters.Add("DrwNum", filter.DrwNum);
        }

        if (filter.FoldNum.HasValue)
        {
            sql.Append($" AND FOLD_NUM = {GeneralFunction.GetParam("FoldNum", dbType)}");
            parameters.Add("FoldNum", filter.FoldNum);
        }

        if (filter.DocNum.HasValue)
        {
            sql.Append($" AND DOC_NUM = {GeneralFunction.GetParam("DocNum", dbType)}");
            parameters.Add("DocNum", filter.DocNum);
        }

        if (filter.TreeStat.HasValue)
        {
            sql.Append($" AND TREE_STAT = {GeneralFunction.GetParam("TreeStat", dbType)}");
            parameters.Add("TreeStat", filter.TreeStat);
        }

        if (filter.TreeLvl.HasValue)
        {
            sql.Append($" AND TREE_LVL = {GeneralFunction.GetParam("TreeLvl", dbType)}");
            parameters.Add("TreeLvl", filter.TreeLvl);
        }

        if (filter.LicenseDomainId.HasValue)
        {
            sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        return await _genericService.GetListAsync<Tree>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<Tree?>> GetActiveTreesForUser(TreeRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
              SELECT DISTINCT 
                  T.TREE_ID        AS TreeId  ,
                  T.RNAME_ARB      AS RnameArb,
                  T.RNAME_ENG      AS RnameEng,
                  T.TREE_LVL       AS TreeLvl
              FROM UserSec U
              RIGHT OUTER JOIN Trees T ON U.Obj_ID = T.TREE_ID
              WHERE 1 = 1
                  AND T.TREE_STAT = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.TreeId))
        {
            sql.Append($" AND T.TREE_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.TreeId);
        }

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            sql.Append($" AND U.USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
            parameters.Add("UserId", filter.UserId);
        }

        sql.Append($" AND U.Obj_Level = {GeneralFunction.GetParam("ObjLevel", dbType)}");
        parameters.Add("ObjLevel", GeneralEnums.OBJ_LEVEL.TREE_PRIVILEGES.GetHashCode());

        return await _genericService.GetListAsync<Tree>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<Tree>> GetTreesByGroups(TreeRequest filter, CancellationToken ct = default)
    {
        if (filter.GroupIds == null || filter.GroupIds.Count == 0)
            return new List<Tree>();

        var dbType = _genericService.GetDatabaseType();

        StringBuilder? sql = new StringBuilder($@"
              SELECT DISTINCT 
                  Trees.Tree_ID     AS TreeId  ,
                  Trees.RName_ENG   AS RnameEng,
                  Trees.RName_ARB   AS RnameArb,
                  Trees.Tree_Lvl    AS TreeLvl
              FROM GroupSec
              RIGHT OUTER JOIN Trees ON GroupSec.Obj_ID = Trees.Tree_ID
              WHERE TREE_STAT = 1
                AND GroupSec.Obj_Level = {GeneralEnums.OBJ_LEVEL.TREE_PRIVILEGES.GetHashCode()}");

        DynamicParameters parameters = new DynamicParameters();

        sql.Append($" AND GroupSec.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
        parameters.Add("GroupIds", filter.GroupIds);

        return await _genericService.GetListAsync<Tree>(sql.ToString(), parameters, null, ct);
    }


}
