using Dapper;
using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.StorgeHierarchy;

public class DocRepository : Repository<Doc>, IDocRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;

    public DocRepository(ApplicationDbContext db, IGenericRepository genericService)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<Doc>> SelectAsync(DocumentsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT
                DOC_ID             AS DocId,
                DCNAME_ARB         AS DcnameArb,
                DCNAME_ENG         AS DcnameEng,
                DOC_STAT           AS DocStat,
                LICENSE_DOMAIN_ID  AS LicenseDomainId
            FROM DOCS
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.DocId))
        {
            sql.Append($" AND DOC_ID = {GeneralFunction.GetParam("DocId", dbType)}");
            parameters.Add("DocId", filter.DocId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameArb))
        {
            sql.Append($" AND LOWER(DCNAME_ARB) = LOWER({GeneralFunction.GetParam("DcnameArb", dbType)})");
            parameters.Add("DcnameArb", filter.DcnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameEng))
        {
            sql.Append($" AND LOWER(DCNAME_ENG) = LOWER({GeneralFunction.GetParam("DcnameEng", dbType)})");
            parameters.Add("DcnameEng", filter.DcnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.DocStat))
        {
            sql.Append($" AND DOC_STAT = {GeneralFunction.GetParam("DocStat", dbType)}");
            parameters.Add("DocStat", filter.DocStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        return await _genericService.GetListAsync<Doc>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<Doc>> SelectActiveDocumentsForGroupsLevelOne(DocumentsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                doc.Doc_ID      AS DocId,
                doc.DcName_ARB  AS DcnameArb,
                doc.DcName_ENG  AS DcnameEng,
                fobj.DOC_SER    AS DocSer
            FROM Docs doc
            INNER JOIN FoldObj fobj   ON doc.Doc_ID    = fobj.Doc_ID
            INNER JOIN Folders f      ON fobj.Fold_ID  = f.Fold_ID
            LEFT JOIN GroupSec gs     ON doc.Doc_ID    = gs.Obj_ID
            LEFT JOIN GroupSec gs1    ON f.Fold_ID     = gs1.Obj_ID
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.TreeId))
        {
            sql.Append($" AND t.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.TreeId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FolderId))
        {
            sql.Append($" AND f.Fold_ID = {GeneralFunction.GetParam("FoldId", dbType)}");
            parameters.Add("FoldId", filter.FolderId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameArb))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ARB) = LOWER({GeneralFunction.GetParam("DcnameArb", dbType)})");
            parameters.Add("DcnameArb", filter.DcnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameEng))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ENG) = LOWER({GeneralFunction.GetParam("DcnameEng", dbType)})");
            parameters.Add("DcnameEng", filter.DcnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND doc.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DocumentClassId) && filter.DocumentClassId != "-1")
        {
            sql.Append($" AND doc.Doc_ID IN ({filter.DocumentClassId})");
        }

        sql.Append($" AND doc.DOC_STAT = 1");

        if (filter.GroupIds != null && filter.GroupIds.Any())
        {
            sql.Append($" AND gs.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
            sql.Append($" AND gs1.Group_ID IN {GeneralFunction.GetParam("GroupIds1", dbType)}");

            parameters.Add("GroupIds", filter.GroupIds);
            parameters.Add("GroupIds1", filter.GroupIds);
        }

        sql.Append($" AND gs.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DOCUMENT_PRIVILEGES}");
        sql.Append($" AND gs.Obj_Flag >= 1");
        sql.Append($" AND gs1.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.FOLDER_PRIVILEGES}");
        sql.Append($" AND gs1.Obj_Flag >= 1");

        return await _genericService.GetListAsync<Doc>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<Doc>> SelectActiveDocumentsForGroupsLevelFour(DocumentsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                doc.Doc_ID      AS DocId,
                doc.DcName_ARB  AS DcnameArb,
                doc.DcName_ENG  AS DcnameEng,
                tob.OBJ_SER     AS DocSer
            FROM Docs doc
            INNER JOIN Tree_Obj tob   ON doc.Doc_ID    = tob.Obj_ID
            INNER JOIN Trees t        ON tob.Tree_ID   = t.Tree_ID
            LEFT JOIN GroupSec gs     ON doc.Doc_ID    = gs.Obj_ID
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.TreeId))
        {
            sql.Append($" AND t.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.TreeId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameArb))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ARB) = LOWER({GeneralFunction.GetParam("DcnameArb", dbType)})");
            parameters.Add("DcnameArb", filter.DcnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameEng))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ENG) = LOWER({GeneralFunction.GetParam("DcnameEng", dbType)})");
            parameters.Add("DcnameEng", filter.DcnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND doc.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DocumentClassId) && filter.DocumentClassId != "-1")
        {
            sql.Append($" AND doc.Doc_ID IN ({filter.DocumentClassId})");
        }

        sql.Append($" AND doc.DOC_STAT = 1");

        if (filter.GroupIds != null && filter.GroupIds.Any())
        {
            sql.Append($" AND gs.Group_ID IN {GeneralFunction.GetParam("GroupIds", dbType)}");
            parameters.Add("GroupIds", filter.GroupIds);
        }

        sql.Append($" AND gs.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DOCUMENT_PRIVILEGES}");
        sql.Append($" AND gs.Obj_Flag >= 1");

        return await _genericService.GetListAsync<Doc>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<Doc>> SelectActiveDocumentsForUserLevelOne(DocumentsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                doc.Doc_ID      AS DocId,
                doc.DcName_ARB  AS DcnameArb,
                doc.DcName_ENG  AS DcnameEng,
                fobj.DOC_SER    AS DocSer
            FROM Docs doc
            INNER JOIN FoldObj fobj   ON doc.Doc_ID    = fobj.Doc_ID
            INNER JOIN Folders f      ON fobj.Fold_ID  = f.Fold_ID
            INNER JOIN DrwObj dobj    ON f.Fold_ID     = dobj.Fold_ID
            INNER JOIN Drawers d      ON dobj.Drw_ID   = d.Drw_ID
            INNER JOIN CabObj cobj    ON d.Drw_ID      = cobj.Drw_ID
            INNER JOIN Cabinets cab   ON cobj.Cab_ID   = cab.Cab_ID
            INNER JOIN Tree_Obj tob   ON cab.Cab_ID    = tob.Obj_ID
            INNER JOIN Trees t        ON tob.Tree_ID   = t.Tree_ID
            LEFT JOIN UserSec us      ON doc.Doc_ID    = us.Obj_ID
            LEFT JOIN UserSec us1     ON f.Fold_ID     = us1.Obj_ID
            LEFT JOIN UserSec us2     ON d.Drw_ID      = us2.Obj_ID
            LEFT JOIN UserSec us3     ON cab.Cab_ID    = us3.Obj_ID
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.TreeId))
        {
            sql.Append($" AND t.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.TreeId);
        }

        if (!string.IsNullOrWhiteSpace(filter.CabinetId))
        {
            sql.Append($" AND cab.Cab_ID = {GeneralFunction.GetParam("CabId", dbType)}");
            parameters.Add("CabId", filter.CabinetId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DrawerId))
        {
            sql.Append($" AND d.Drw_ID = {GeneralFunction.GetParam("DrwId", dbType)}");
            parameters.Add("DrwId", filter.DrawerId);
        }

        if (!string.IsNullOrWhiteSpace(filter.FolderId))
        {
            sql.Append($" AND f.Fold_ID = {GeneralFunction.GetParam("FoldId", dbType)}");
            parameters.Add("FoldId", filter.FolderId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameArb))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ARB) = LOWER({GeneralFunction.GetParam("DcnameArb", dbType)})");
            parameters.Add("DcnameArb", filter.DcnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameEng))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ENG) = LOWER({GeneralFunction.GetParam("DcnameEng", dbType)})");
            parameters.Add("DcnameEng", filter.DcnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND doc.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DocumentClassId) && filter.DocumentClassId != "-1")
        {
            sql.Append($" AND doc.Doc_ID IN ({filter.DocumentClassId})");
        }

        sql.Append($" AND doc.DOC_STAT = 1");

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            sql.Append($" AND us.User_ID = {GeneralFunction.GetParam("UserId", dbType)}");
            sql.Append($" AND us1.User_ID = {GeneralFunction.GetParam("UserId1", dbType)}");
            sql.Append($" AND us2.User_ID = {GeneralFunction.GetParam("UserId2", dbType)}");
            sql.Append($" AND us3.User_ID = {GeneralFunction.GetParam("UserId3", dbType)}");
            parameters.Add("UserId", filter.UserId);
            parameters.Add("UserId1", filter.UserId);
            parameters.Add("UserId2", filter.UserId);
            parameters.Add("UserId3", filter.UserId);
        }

        sql.Append($" AND us.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DOCUMENT_PRIVILEGES}");
        sql.Append($" AND us.Obj_Flag >= 1");
        sql.Append($" AND us1.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.FOLDER_PRIVILEGES}");
        sql.Append($" AND us2.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DRAWER_PRIVILEGES}");
        sql.Append($" AND us3.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.CABINET_PRIVILEGES}");

        return await _genericService.GetListAsync<Doc>(sql.ToString(), parameters, null, ct);
    }

    public async Task<List<Doc>> SelectActiveDocumentsForUserLevelFour(DocumentsRequest filter, CancellationToken ct = default)
    {
        DatabaseProvider dbType = _genericService.GetDatabaseType();

        StringBuilder sql = new StringBuilder(@"
            SELECT DISTINCT
                doc.Doc_ID      AS DocId,
                doc.DcName_ARB  AS DcnameArb,
                doc.DcName_ENG  AS DcnameEng,
                tob.OBJ_SER     AS DocSer
            FROM Docs doc
            INNER JOIN Tree_Obj tob   ON doc.Doc_ID    = tob.Obj_ID
            INNER JOIN Trees t        ON tob.Tree_ID   = t.Tree_ID
            LEFT JOIN UserSec us      ON doc.Doc_ID    = us.Obj_ID
            WHERE 1 = 1
        ");

        DynamicParameters parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.TreeId))
        {
            sql.Append($" AND t.Tree_ID = {GeneralFunction.GetParam("TreeId", dbType)}");
            parameters.Add("TreeId", filter.TreeId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameArb))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ARB) = LOWER({GeneralFunction.GetParam("DcnameArb", dbType)})");
            parameters.Add("DcnameArb", filter.DcnameArb);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameEng))
        {
            sql.Append($" AND LOWER(doc.DCNAME_ENG) = LOWER({GeneralFunction.GetParam("DcnameEng", dbType)})");
            parameters.Add("DcnameEng", filter.DcnameEng);
        }

        if (!string.IsNullOrWhiteSpace(filter.LicenseDomainId))
        {
            sql.Append($" AND doc.LICENSE_DOMAIN_ID = {GeneralFunction.GetParam("LicenseDomainId", dbType)}");
            parameters.Add("LicenseDomainId", filter.LicenseDomainId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DocumentClassId) && filter.DocumentClassId != "-1")
        {
            sql.Append($" AND doc.Doc_ID IN ({filter.DocumentClassId})");
        }

        sql.Append($" AND doc.DOC_STAT = 1");

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            sql.Append($" AND us.User_ID = {GeneralFunction.GetParam("UserId", dbType)}");
            parameters.Add("UserId", filter.UserId);
        }

        sql.Append($" AND us.Obj_Level = {(int)GeneralEnums.OBJ_LEVEL.DOCUMENT_PRIVILEGES}");
        sql.Append($" AND us.Obj_Flag >= 1");

        return await _genericService.GetListAsync<Doc>(sql.ToString(), parameters, null, ct);
    }
}
