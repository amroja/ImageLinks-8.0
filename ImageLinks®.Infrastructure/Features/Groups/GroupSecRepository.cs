using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.IRepository;
using ImageLinks_.Application.Features.Groups.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.Groups;

public class GroupSecRepository : Repository<GroupSec>, IGroupSecRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;

    public GroupSecRepository(ApplicationDbContext db, IGenericRepository genericService)
        : base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<GroupSec>> SelectAsync(GroupSec filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();

        var sql = new StringBuilder(@"
            SELECT 
                GROUP_ID  AS GroupId,
                OBJ_LEVEL AS ObjLevel,
                OBJ_ID    AS ObjId,
                OBJ_FLAG  AS ObjFlag
            FROM GROUPSEC
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (filter.GroupId.HasValue)
        {
            sql.Append($" AND GROUP_ID = {GeneralFunction.GetParam("GroupId", dbType)}");
            parameters.Add("GroupId", filter.GroupId);
        }

        if (filter.ObjLevel.HasValue)
        {
            sql.Append($" AND OBJ_LEVEL = {GeneralFunction.GetParam("ObjLevel", dbType)}");
            parameters.Add("ObjLevel", filter.ObjLevel);
        }

        if (filter.ObjId.HasValue)
        {
            sql.Append($" AND OBJ_ID = {GeneralFunction.GetParam("ObjId", dbType)}");
            parameters.Add("ObjId", filter.ObjId);
        }

        if (filter.ObjFlag.HasValue)
        {
            sql.Append($" AND OBJ_FLAG = {GeneralFunction.GetParam("ObjFlag", dbType)}");
            parameters.Add("ObjFlag", filter.ObjFlag);
        }

        return await _genericService.GetListAsync<GroupSec>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<GroupSec>> GetByGroupIds(List<string> groupIds, CancellationToken ct = default)
    {
        if (groupIds == null || groupIds.Count == 0)
            return new List<GroupSec>();

        var sql = @"
            SELECT 
                GROUP_ID  AS GroupId,
                OBJ_LEVEL AS ObjLevel,
                OBJ_ID    AS ObjId,
                OBJ_FLAG  AS ObjFlag
            FROM GROUPSEC
            WHERE GROUP_ID IN @GroupIds";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("GroupIds", groupIds);

        return await _genericService.GetListAsync<GroupSec>(sql, parameters, null, ct);
    }

    public async Task<List<GroupSec>> GetByFilter(GroupSecRequest filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();

        var sql = new StringBuilder(@"
            SELECT 
                GROUP_ID  AS GroupId,
                OBJ_LEVEL AS ObjLevel,
                OBJ_ID    AS ObjId,
                OBJ_FLAG  AS ObjFlag
            FROM GROUPSEC
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.GroupId))
        {
            sql.Append($" AND GROUP_ID = {GeneralFunction.GetParam("GroupId", dbType)}");
            parameters.Add("GroupId", filter.GroupId);
        }

        if (!string.IsNullOrWhiteSpace(filter.ObjLevel))
        {
            sql.Append($" AND OBJ_LEVEL = {GeneralFunction.GetParam("ObjLevel", dbType)}");
            parameters.Add("ObjLevel", filter.ObjLevel);
        }

        if (!string.IsNullOrWhiteSpace(filter.ObjId))
        {
            sql.Append($" AND OBJ_ID = {GeneralFunction.GetParam("ObjId", dbType)}");
            parameters.Add("ObjId", filter.ObjId);
        }

        return await _genericService.GetListAsync<GroupSec>(sql.ToString(), parameters, null, ct);
    }
}
