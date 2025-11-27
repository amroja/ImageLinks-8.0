using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.IRepository;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.Groups;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;
    public GroupRepository(ApplicationDbContext db, IGenericRepository genericService) : base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<Group?>> SelectAsync(Group filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();

        var sql = new StringBuilder(@"
              SELECT 
                  GROUP_ID           AS GroupId,
                  GROUP_NAME         AS GroupName,
                  GROUP_STAT         AS GroupStat,
                  LICENSE_DOMAIN_ID  AS LicenseDomainId
              FROM GROUPS
              WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (filter.GroupId.HasValue)
        {
            sql.Append($" AND GROUP_ID = {GeneralFunction.GetParam("GroupId", dbType)}");
            parameters.Add("GroupId", filter.GroupId);
        }

        if (!string.IsNullOrEmpty(filter.GroupName))
        {
            sql.Append($" AND LOWER(GROUP_NAME) = LOWER({GeneralFunction.GetParam("GroupName", dbType)})");
            parameters.Add("GroupName", filter.GroupName.ToLower());
        }

        if (filter.GroupStat.HasValue)
        {
            sql.Append($" AND GROUP_STAT = {GeneralFunction.GetParam("GroupStat", dbType)}");
            parameters.Add("GroupStat", filter.GroupStat);
        }

        if (filter.LicenseDomainId.HasValue)
        {
            sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        return await _genericService.GetListAsync<Group>(sql.ToString(), parameters, null, ct);
    }
}
