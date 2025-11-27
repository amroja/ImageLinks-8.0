namespace ImageLinks_.Application.Common;
public interface IUser
{
    string? Id { get; }
    string? UserName { get; }
    string? Email { get; }
    int? LicenseDomainId { get; }
}
