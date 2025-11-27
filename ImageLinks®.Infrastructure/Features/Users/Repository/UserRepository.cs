using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Common.Models;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Data;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.Users.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IGenericRepository _genericService;
        public UserRepository(ApplicationDbContext db, IGenericRepository genericService) : base(db)
        {
            _db = db;
            _genericService = genericService;
        }

        public void Update(User entity)
        {
            _db.USERS.Update(entity);
        }

        public async Task<List<User?>> SelectAsync(User filter, CancellationToken ct = default)
        {
            var dbType = _genericService.GetDatabaseType();
            var sql = new StringBuilder(@"
                                                     SELECT 
                                                         USER_ID                  AS UserId,
                                                         USER_NAME                AS UserName,
                                                         USER_MAIL                AS UserMail,
                                                         USER_CDATE               AS UserCDate,
                                                         USER_CUSER               AS UserCUser,
                                                         USER_STAT                AS UserStat,
                                                         USER_PASS                AS UserPass,
                                                         USER_ABRIV               AS UserAbriv,
                                                         USER_PTYPE               AS UserPType,
                                                         LOGIN_STAT               AS LoginStat,
                                                         LICENSE_DOMAIN_ID        AS LicenseDomainId,
                                                         UNSUCCESSFUL_LOGIN_COUNT AS UnsuccessfulLoginCount,
                                                         TFA_CONFIRMED            AS TfaConfirmed,
                                                         IS_MANAGER               AS IsManager
                                                     FROM USERS 
                                                     WHERE 1 = 1
                                                    ");
            DynamicParameters? parameters = new DynamicParameters();


            if (filter.UserId > -1)
            {
                sql.Append($" AND USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
                parameters.Add("UserId", filter.UserId);
            }

            if (filter.UserCDate.HasValue)
            {
                sql.Append($" AND USER_CDATE = {GeneralFunction.GetParam("UserCDate", dbType)}");
                parameters.Add("UserCDate", filter.UserCDate);
            }

            if (filter.UserStat.HasValue)
            {
                sql.Append($" AND USER_STAT = {GeneralFunction.GetParam("UserStat", dbType)}");
                parameters.Add("UserStat", filter.UserStat);
            }

            if (filter.UserPType.HasValue)
            {
                sql.Append($" AND USER_PTYPE = {GeneralFunction.GetParam("UserPType", dbType)}");
                parameters.Add("UserPType", filter.UserPType);
            }

            if (filter.LoginStat.HasValue)
            {
                sql.Append($" AND LOGIN_STAT = {GeneralFunction.GetParam("LoginStat", dbType)}");
                parameters.Add("LoginStat", filter.LoginStat);
            }

            if (!string.IsNullOrEmpty(filter.UserName))
            {
                sql.Append($" AND LOWER(USER_NAME) = LOWER({GeneralFunction.GetParam("UserName", dbType)})");
                parameters.Add("UserName", filter.UserName.ToLower());
            }

            if (!string.IsNullOrEmpty(filter.UserMail))
            {
                sql.Append($" AND LOWER(USER_MAIL) = LOWER({GeneralFunction.GetParam("UserMail", dbType)})");
                parameters.Add("UserMail", filter.UserMail.ToLower());
            }

            if (!string.IsNullOrEmpty(filter.UserCUser))
            {
                sql.Append($" AND LOWER(USER_CUSER) = LOWER({GeneralFunction.GetParam("UserCUser", dbType)})");
                parameters.Add("UserCUser", filter.UserCUser.ToLower());
            }

            if (!string.IsNullOrEmpty(filter.UserPass))
            {
                sql.Append($" AND USER_PASS = {GeneralFunction.GetParam("UserPass", dbType)}");
                parameters.Add("UserPass", filter.UserPass);
            }

            if (!string.IsNullOrEmpty(filter.UserAbriv))
            {
                sql.Append($" AND LOWER(USER_ABRIV) = LOWER({GeneralFunction.GetParam("UserAbriv", dbType)})");
                parameters.Add("UserAbriv", filter.UserAbriv.ToLower());
            }

            return await _genericService.GetListAsync<User>(sql.ToString(), parameters, null, ct);
             
        }

        public async Task<List<User>> GetAllUsers(CancellationToken ct = default)
        {
            DataTable? users = await _genericService.GetDataTableAsync("select * from USERS", null, null, ct);

            string sql = @"SELECT 
                            USER_ID     AS UserId,
                            USER_NAME   AS UserName,
                            USER_MAIL   AS UserMail,
                            USER_CDATE  AS UserCDate,
                            USER_CUSER  AS UserCUser,
                            USER_STAT   AS UserStat,
                            USER_PASS   AS UserPass,
                            USER_ABRIV  AS UserAbriv,
                            USER_PTYPE  AS UserPType,
                            LOGIN_STAT  AS LoginStat,
                            LICENSE_DOMAIN_ID AS LicenseDomainId,
                            UNSUCCESSFUL_LOGIN_COUNT AS UnsuccessfulLoginCount,
                            TFA_CONFIRMED AS TfaConfirmed,
                            IS_MANAGER  AS IsManager
                        FROM USERS";

            var usersDapper= await _genericService.GetListAsync<User>(sql, null, null, ct);

            return users.ToUsersEntities();
        }

        public async Task<List<User>> GetUsersFilter(PageRequest pageRequest, CancellationToken ct = default)
        {
            #region Using Entity Framework
            //var usersQuery = _db.USERS.AsNoTracking().AsQueryable();
            //var count = await usersQuery.CountAsync(cancellationToken: ct);
            //return await usersQuery
            //  .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
            //  .Take(pageRequest.PageSize).ToListAsync(ct);
            #endregion

            #region Using Dapper
            string sql = string.Empty;
            string sqlStarter = string.Empty;
            string generatedSql = string.Empty;
            FilterFields filterFields = new FilterFields();
            List<FilterFields> filterFieldsList = new List<FilterFields>();
            DatabaseProvider dbType = _genericService.GetDatabaseType();

            filterFields.FieldName = "userId";
            filterFields.DbFiledName = "USER_ID";
            filterFields.TableName = "USERS";
            filterFieldsList.Add(filterFields);

            filterFields = new FilterFields();
            filterFields.FieldName = "userName";
            filterFields.DbFiledName = "USER_NAME";
            filterFields.TableName = "USERS";
            filterFieldsList.Add(filterFields);

            pageRequest.Filters = new List<Filters>();
            pageRequest.Filters.AddRange(new Filters { Field = "userName", IsCaseSensitive = false, JunctionOperator = "AND", Operator = "contains", Value = "a" });

            sql += "from USERS ";
            
            if (dbType == DatabaseProvider.SqlServer)
            {
                sqlStarter = "Select *,@totalElements as totalElements ";
                sqlStarter = "Select USERS.USER_ID AS UserId,USERS.USER_NAME AS UserName ";
                var (sqls, countSql, parameters) = GeneralFunction.BuildPagedSql(sqlStarter, sql, pageRequest, filterFieldsList, typeof(User), dbType);

                //_genericService.ExecuteScalarAsync<User>(sqls, parameters, null, ct);

                return await _genericService.GetListAsync<User>(sqls, parameters, null, ct);
            }
            else
            {
                sqlStarter = "USERS.USER_ID AS UserId,USERS.USER_NAME AS UserName,USERS.USER_MAIL,USERS.USER_CDATE,USERS.USER_CUSER,USERS.USER_STAT,USERS.USER_PASS,USERS.USER_ABRIV,USERS.USER_PTYPE,USERS.LOGIN_STAT,USERS.LICENSE_DOMAIN_ID,USERS.UNSUCCESSFUL_LOGIN_COUNT,USERS.TFA_CONFIRMED,USERS.IS_MANAGER ";

                var (sqls, countSql, parameters) = GeneralFunction.BuildPagedSql(sqlStarter, sql, pageRequest, filterFieldsList, typeof(User), dbType);
                return await _genericService.GetListAsync<User>(sqls, parameters, null, ct);
            }
            #endregion
        }
    }
}
