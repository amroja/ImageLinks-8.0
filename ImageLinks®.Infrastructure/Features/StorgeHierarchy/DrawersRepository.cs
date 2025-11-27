using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;
using ImageLinks_.Application.IRepository;
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
            var dbType = _genericService.GetDatabaseType();

            var sql = new StringBuilder(@"
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
                parameters.Add("DnameArb", filter.DnameArb.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(filter.DnameEng))
            {
                sql.Append($" AND LOWER(DNAME_ENG) = LOWER({GeneralFunction.GetParam("DnameEng", dbType)})");
                parameters.Add("DnameEng", filter.DnameEng.ToLower());
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

        public Task<List<DrawersModel>> SelecActiveDrawersForGroupsLevelOne(CabinetsRequest filter, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<DrawersModel>> SelecActiveDrawersForGroupsLevelTwo(CabinetsRequest filter, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<DrawersModel>> SelecActiveDrawersForUserLevelOne(CabinetsRequest filter, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<DrawersModel>> SelecActiveDrawersForUserLevelTwo(CabinetsRequest filter, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
