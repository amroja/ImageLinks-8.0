using Dapper;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;
using System.Text;

namespace ImageLinks_.Infrastructure.Features.StorgeHierarchy;

public class DocRepository : Repository<Doc>, IDocRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;

    public DocRepository(ApplicationDbContext db, IGenericRepository genericService) : base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<Doc>> SelectAsync(DocRequest filter, CancellationToken ct = default)
    {
        var dbType = _genericService.GetDatabaseType();
                   StringBuilder? sql = new StringBuilder(@"
                            SELECT 
                                DOC_ID                   AS DocId                 ,
                                DOC_CRSTAT               AS DocCrstat             ,
                                DCNAME_ARB               AS DcnameArb             ,
                                DCNAME_ENG               AS DcnameEng             ,
                                DC_RCOUNT                AS DcRcount              ,
                                DC_CABRIV                AS DcCabriv              ,
                                UPD_DATE                 AS UpdDate               ,  
                                UPD_PRSN                 AS UpdPrsn               ,
                                DOC_STAT                 AS DocStat               ,
                                MAX_VERNUM               AS MaxVernum             ,
                                OCR_TEMPLT               AS OcrTemplt             ,
                                RET_PRD                  AS RetPrd                ,
                                OCR_PAGENUM              AS OcrPagenum            ,
                                OCR_EVERYPAGE            AS OcrEverypage          ,
                                WF_CLASS                 AS WfClass               ,
                                DOCPHASE                 AS Docphase              ,
                                CAT_ID                   AS CatId                 ,
                                TEMP_ID                  AS TempId                ,
                                OCR_TYPE                 AS OcrType               ,
                                FILES_FILTER             AS FilesFilter           ,
                                IS_PUBLIC                AS IsPublic              ,
                                HAS_ROLES                AS HasRoles              ,
                                HAS_BATCH_NAME           AS HasBatchName          ,
                                HAS_FILE_CLASSIFICATION  AS HasFileClassification ,
                                BARCODEZONE              AS Barcodezone           ,
                                BARCODEFORMAT            AS Barcodeformat         ,
                                HAS_WATERMARK            AS HasWatermark          ,
                                LICENSE_DOMAIN_ID        AS LicenseDomainId       ,
                                NOT_MATCHED_OP           AS NotMatchedOp          ,
                                EXPAND_G_INDEX           AS ExpandGIndex          ,
                                EXPAND_C_INDEX           AS ExpandCIndex          ,
                                MATCHING_OPTIONS         AS MatchingOptions       ,
                                AUTO_INDEX_OP            AS AutoIndexOp           ,
                                MAX_DOCN_COUNT           AS MaxDocnCount          
                            FROM DOCS
                            WHERE 1 = 1
        ");

        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.DocId))
        {
            sql.Append($" AND DOC_ID = {GeneralFunction.GetParam("DocId", dbType)}");
            parameters.Add("DocId", filter.DocId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameArb))
        {
            sql.Append($" AND DCNAME_ARB LIKE {GeneralFunction.GetParam("DcnameArb", dbType)}");
            parameters.Add("DcnameArb", $"%{filter.DcnameArb}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.DcnameEng))
        {
            sql.Append($" AND DCNAME_ENG LIKE {GeneralFunction.GetParam("DcnameEng", dbType)}");
            parameters.Add("DcnameEng", $"%{filter.DcnameEng}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.CatId))
        {
            sql.Append($" AND CAT_ID = {GeneralFunction.GetParam("CatId", dbType)}");
            parameters.Add("CatId", filter.CatId);
        }

        if (!string.IsNullOrWhiteSpace(filter.DocStat))
        {
            sql.Append($" AND DOC_STAT = {GeneralFunction.GetParam("DocStat", dbType)}");
            parameters.Add("DocStat", filter.DocStat);
        }

        if (!string.IsNullOrWhiteSpace(filter.IsPublic))
        {
            sql.Append($" AND IS_PUBLIC = {GeneralFunction.GetParam("IsPublic", dbType)}");
            parameters.Add("IsPublic", filter.IsPublic);
        }

        return await _genericService.GetListAsync<Doc>(sql.ToString(), parameters, null, ct);
    }
}
