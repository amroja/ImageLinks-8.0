namespace ImageLinks_.Application.Features.Groups.Requests;

public class GroupSecRequest
{
    public string? GroupId { get; set; }
    public string? ObjLevel { get; set; }
    public string? ObjId { get; set; }

    public List<string>? GroupIds { get; set; }
}
