namespace ImageLinks_.Application.Features.Users.Requests;

public class UserSecRequest
{
    public string? UserId { get; set; }
    public string? ObjLevel { get; set; }
    public string? ObjId { get; set; }

    public List<string>? UserIds { get; set; }
}
