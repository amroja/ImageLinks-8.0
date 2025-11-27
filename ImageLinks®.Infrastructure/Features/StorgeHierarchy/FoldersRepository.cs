using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.StorgeHierarchy;
public class FoldersRepository : Repository<FoldersModel>, IFoldersRepository
{
    private readonly IGenericRepository _genericService;

    public FoldersRepository(IGenericRepository genericService)
    {
        _genericService = genericService;
    }

    public async Task<List<FoldersModel>> SelectAsync(FoldersRequest filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();

        var sql = new StringBuilder(@"
            SELECT
                FOLD_ID             AS FoldId           ,
                FNAME_ARB           AS FnameArb         , 
                FNAME_ENG           AS FnameEng         ,
                FOLD_STAT           AS FoldStat         ,
                FOLD_VAL            AS FoldVal          ,
                LICENSE_DOMAIN_ID   AS LicenseDomainId
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
            parameters.Add("FnameArb", filter.FnameArb.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(filter.FnameEng))
        {
            sql.Append($" AND LOWER(FNAME_ENG) = LOWER({GeneralFunction.GetParam("FnameEng", dbType)} )");
            parameters.Add("FnameEng", filter.FnameEng.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(filter.FoldStat))
        {
            sql.Append($" AND FOLD_STAT = {GeneralFunction.GetParam("FoldStat", dbType)}");
            parameters.Add("FoldStat", filter.FoldStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.FoldVal))
        {
            sql.Append($" AND FOLD_VAL = {GeneralFunction.GetParam("FoldVal", dbType)}");
            parameters.Add("FoldVal", filter.FoldVal);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        return await _genericService.GetListAsync<FoldersModel>(sql.ToString(), parameters, null, ct);
    }
}
