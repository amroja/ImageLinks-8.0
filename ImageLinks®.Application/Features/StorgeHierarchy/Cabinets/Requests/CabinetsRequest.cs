namespace ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;

public class CabinetsRequest
{
    public string? ParentId { get; set; }
    public string? CabId { get; set; }
    public string? CnameArb { get; set; }
    public string? CnameEng { get; set; }
    public string? CabStat { get; set; }
    public string? LicenseDomainId { get; set; }
    public string? UserId { get; set; }
    public List<string>? GroupIds { get; set; }
}
