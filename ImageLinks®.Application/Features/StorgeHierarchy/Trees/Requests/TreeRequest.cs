namespace ImageLinks_.Application.Features.StorgeHierarchy.Trees.Requests;
public class TreeRequest
{
    public string? TreeId { get; set; }
    public string? UserId { get; set; }
    public List<string>? GroupIds { get; set; }
}