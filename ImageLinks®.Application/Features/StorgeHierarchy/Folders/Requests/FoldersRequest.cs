namespace ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;

public class FoldersRequest
{
    public string? ParentId { get; set; }
    public string? TreeId { get; set; }
    public string? DrawerId { get; set; }
    public string? FoldId { get; set; }
    public string? FnameArb { get; set; }
    public string? FnameEng { get; set; }
    public string? FoldStat { get; set; }
    public string? FoldVal { get; set; }
    public string? LicenseDomainId { get; set; }
    public string? UserId { get; set; }
    public List<string>? GroupIds { get; set; }
}
