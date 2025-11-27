using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.Features.Users.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.Users.Repository;

public class UserSecRepository : Repository<UserSec>, IUserSecRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;

    public UserSecRepository(ApplicationDbContext db, IGenericRepository genericService)
        : base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<UserSec>> SelectAsync(UserSec filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();

        var sql = new StringBuilder(@"
            SELECT 
                USER_ID   AS UserId   ,
                OBJ_LEVEL AS ObjLevel ,
                OBJ_ID    AS ObjId    ,
                OBJ_FLAG  AS ObjFlag
            FROM USERSEC
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (filter.UserId.HasValue)
        {
            sql.Append($" AND USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
            parameters.Add("UserId", filter.UserId);
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

        return await _genericService.GetListAsync<UserSec>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<UserSec>> GetByUserIds(List<string> userIds, CancellationToken ct = default)
    {
        if (userIds == null || userIds.Count == 0)
            return new List<UserSec>();

        var sql = @"
            SELECT 
                USER_ID  AS UserId,
                OBJ_LEVEL AS ObjLevel,
                OBJ_ID    AS ObjId,
                OBJ_FLAG  AS ObjFlag
            FROM USERSEC
            WHERE USER_ID IN @UserIds";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("UserIds", userIds);

        return await _genericService.GetListAsync<UserSec>(sql, parameters, null, ct);
    }

    public async Task<List<UserSec>> GetByFilter(UserSecRequest filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();

        var sql = new StringBuilder(@"
            SELECT 
                USER_ID  AS UserId,
                OBJ_LEVEL AS ObjLevel,
                OBJ_ID    AS ObjId,
                OBJ_FLAG  AS ObjFlag
            FROM USERSEC
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            sql.Append($" AND USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
            parameters.Add("UserId", filter.UserId);
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

        return await _genericService.GetListAsync<UserSec>(sql.ToString(), parameters, null, ct);
    }
}
