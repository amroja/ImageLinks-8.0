using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.IRepository;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.Groups
{
    public class GroupMbrRepository : Repository<GroupMbr>, IGroupMbrRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IGenericRepository _genericService;
        public GroupMbrRepository(ApplicationDbContext db, IGenericRepository genericService) : base(db)
        {
            _db = db;
            _genericService = genericService;
        }
       
        public async Task<List<GroupMbr?>> SelectAsync(GroupMbr filter, CancellationToken ct = default)
        {
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            StringBuilder sql = new StringBuilder(@"
              SELECT 
                  GROUP_ID   AS GroupId,
                  USER_ID    AS UserId
              FROM GROUPMBR
              WHERE 1 = 1
            ");

            DynamicParameters parameters = new DynamicParameters();

            if (filter.GroupId.HasValue)
            {
                sql.Append($" AND GROUP_ID = {GeneralFunction.GetParam("GroupId", dbType)}");
                parameters.Add("GroupId", filter.GroupId);
            }

            if (filter.UserId.HasValue)
            {
                sql.Append($" AND USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
                parameters.Add("UserId", filter.UserId);
            }

            return await _genericService.GetListAsync<GroupMbr>(sql.ToString(), parameters, null, ct);
        }

        public async Task<List<string>> GetGroupsByUser(GroupMbrDto filter, CancellationToken ct = default)
        {
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            StringBuilder sql = new StringBuilder(@"
                     SELECT 
                         GROUP_ID AS GroupId
                     FROM GROUPMBR
                     WHERE 1 = 1
                       AND GROUP_ID != 0
                    ");
                    
            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.UserId))
            {
                sql.Append($" AND USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
                parameters.Add("UserId", filter.UserId);
            }

            List<GroupMbr?>? result = await _genericService.GetListAsync<GroupMbr>(sql.ToString(), parameters, null, ct);

            return result.Select(x => x.GroupId.ToString()).ToList();
        }
    }
}
