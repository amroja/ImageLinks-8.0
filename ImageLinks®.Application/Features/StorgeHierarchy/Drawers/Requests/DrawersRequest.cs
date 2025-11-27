namespace ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;

public class DrawersRequest
{
    public string? ParentId { get; set; }
    public string? TreeId { get; set; }
    public string? DrwId { get; set; }
    public string? DnameArb { get; set; }
    public string? DnameEng { get; set; }
    public string? DrwStat { get; set; }
    public string? LicenseDomainId { get; set; }
    public string? UserId { get; set; }
    public List<string>? GroupIds { get; set; }
}
