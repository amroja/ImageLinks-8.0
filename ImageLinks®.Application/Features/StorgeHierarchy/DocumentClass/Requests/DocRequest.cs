namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;

public class DocumentsRequest
{
    public string? DocId { get; set; }
    public string? DcnameArb { get; set; }
    public string? DcnameEng { get; set; }
    public string? DocStat { get; set; }
    public string? LicenseDomainId { get; set; }
    public string? TreeId { get; set; }
    public string? CabinetId { get; set; }
    public string? DrawerId { get; set; }
    public string? FolderId { get; set; }
    public string? DocumentClassId { get; set; }
    public string? UserId { get; set; }
    public List<string>? GroupIds { get; set; }
}