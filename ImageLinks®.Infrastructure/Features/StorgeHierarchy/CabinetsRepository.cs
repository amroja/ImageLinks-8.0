using Dapper;
using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.StorgeHierarchy;

public class CabinetsRepository : Repository<CabinetsModel>, ICabinetsRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;

    public CabinetsRepository(ApplicationDbContext db, IGenericRepository genericService) : base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<CabinetsModel>> SelectAsync(CabinetsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
              SELECT 
                  CAB_ID             AS CabId          ,
                  CNAME_ARB          AS CnameArb       ,
                  CNAME_ENG          AS CnameEng       ,
                  CAB_STAT           AS CabStat        ,
                  LICENSE_DOMAIN_ID  AS LicenseDomainId  
              FROM CABINETS
              WHERE 1 = 1
        ");   

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.CabId))
        {
            sql.Append($" AND CAB_ID = {GeneralFunction.GetParam("CabId", dbType)}");
            parameters.Add("CabId", filter.CabId);
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameArb))
        {
            sql.Append($" AND CNAME_ARB LIKE {GeneralFunction.GetParam("CnameArb", dbType)}");
            parameters.Add("CnameArb", $"%{filter.CnameArb}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameEng))
        {
            sql.Append($" AND CNAME_ENG LIKE {GeneralFunction.GetParam("CnameEng", dbType)}");
            parameters.Add("CnameEng", $"%{filter.CnameEng}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CabStat))
        {
            sql.Append($" AND CAB_STAT = {GeneralFunction.GetParam("CabStat", dbType)}");
            parameters.Add("CabStat", filter.CabStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        return await _genericService.GetListAsync<CabinetsModel>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<CabinetsModel>> SelectByParentAsync(CabinetsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
              SELECT 
                  CAB_ID             AS CabId          ,
                  CNAME_ARB          AS CnameArb       ,
                  CNAME_ENG          AS CnameEng       ,
                  CAB_STAT           AS CabStat        ,
                  LICENSE_DOMAIN_ID  AS LicenseDomainId  
              FROM CABINETS
              WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.CabId))
        {
            sql.Append($" AND CAB_ID = {GeneralFunction.GetParam("CabId", dbType)}");
            parameters.Add("CabId", filter.CabId);
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameArb))
        {
            sql.Append($" AND CNAME_ARB LIKE {GeneralFunction.GetParam("CnameArb", dbType)}");
            parameters.Add("CnameArb", $"%{filter.CnameArb}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameEng))
        {
            sql.Append($" AND CNAME_ENG LIKE {GeneralFunction.GetParam("CnameEng", dbType)}");
            parameters.Add("CnameEng", $"%{filter.CnameEng}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CabStat))
        {
            sql.Append($" AND CAB_STAT = {GeneralFunction.GetParam("CabStat", dbType)}");
            parameters.Add("CabStat", filter.CabStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        return await _genericService.GetListAsync<CabinetsModel>(sql.ToString(), parameters, null, ct);
    }


    public async Task<List<CabinetsModel>> SelecActiveCabinetsForGroups(CabinetsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
              SELECT DISTINCT
                  Cabinets.Cab_ID             AS CabId          ,
                  Cabinets.CName_ARB          AS CnameArb       ,
                  Cabinets.CName_ENG          AS CnameEng       ,
                  Cabinets.CAB_STAT           AS CabStat        ,
                  Cabinets.License_Domain_Id  AS LicenseDomainId,
                  TREE_OBJ.OBJ_SER            AS ObjSer
              FROM Tree_Obj
              INNER JOIN Cabinets ON Tree_Obj.Obj_ID = Cabinets.Cab_ID
              INNER JOIN Trees ON Tree_Obj.Tree_ID = Trees.Tree_ID
              LEFT OUTER JOIN GroupSec ON Cabinets.Cab_ID = GroupSec.Obj_ID
              WHERE 1 = 1
           ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.ParentId))
        {
            sql.Append($" AND Trees.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.ParentId);
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameArb))
        {
            sql.Append($" AND Cabinets.CNAME_ARB LIKE {GeneralFunction.GetParam("CnameArb", dbType)}");
            parameters.Add("CnameArb", $"%{filter.CnameArb}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameEng))
        {
            sql.Append($" AND Cabinets.CNAME_ENG LIKE {GeneralFunction.GetParam("CnameEng", dbType)}");
            parameters.Add("CnameEng", $"%{filter.CnameEng}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CabStat))
        {
            sql.Append($" AND Cabinets.CAB_STAT = {GeneralFunction.GetParam("CabStat", dbType)}");
            parameters.Add("CabStat", filter.CabStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND Cabinets.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        sql.Append($" AND GroupSec.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
        parameters.Add("GroupIds", filter.GroupIds);

        sql.Append(@$" AND GroupSec.Obj_Level = {GeneralEnums.OBJ_LEVEL.CABINET_PRIVILEGES.GetHashCode()}");

        return await _genericService.GetListAsync<CabinetsModel>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<CabinetsModel>> SelecActiveCabinetsForUser(CabinetsRequest filter, CancellationToken ct = default)
    {

        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
              SELECT DISTINCT
                  Cabinets.Cab_ID             AS CabId          ,
                  Cabinets.CName_ARB          AS CnameArb       ,
                  Cabinets.CName_ENG          AS CnameEng       ,
                  Cabinets.CAB_STAT           AS CabStat        ,
                  Cabinets.License_Domain_Id  AS LicenseDomainId,
                  TREE_OBJ.OBJ_SER            AS ObjSer
              FROM Tree_Obj
              INNER JOIN Cabinets ON Tree_Obj.Obj_ID = Cabinets.Cab_ID
              INNER JOIN Trees ON Tree_Obj.Tree_ID = Trees.Tree_ID
              LEFT OUTER JOIN UserSec ON Cabinets.Cab_ID = UserSec.Obj_ID
              WHERE 1 = 1
       ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.ParentId))
        {
            sql.Append($" AND Trees.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.ParentId);
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameArb))
        {
            sql.Append($" AND Cabinets.CNAME_ARB LIKE {GeneralFunction.GetParam("CnameArb", dbType)}");
            parameters.Add("CnameArb", $"%{filter.CnameArb}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CnameEng))
        {
            sql.Append($" AND Cabinets.CNAME_ENG LIKE {GeneralFunction.GetParam("CnameEng", dbType)}");
            parameters.Add("CnameEng", $"%{filter.CnameEng}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CabStat))
        {
            sql.Append($" AND Cabinets.CAB_STAT = {GeneralFunction.GetParam("CabStat", dbType)}");
            parameters.Add("CabStat", filter.CabStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND Cabinets.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        sql.Append($" AND UserSec.User_ID = {GeneralFunction.GetParam("UserId", dbType)}");
        parameters.Add("UserId", filter.UserId);

         sql.Append(@$" AND UserSec.Obj_Level = {GeneralEnums.OBJ_LEVEL.CABINET_PRIVILEGES.GetHashCode()}");

        return await _genericService.GetListAsync<CabinetsModel>(sql.ToString(), parameters, null, ct);
    }

}
