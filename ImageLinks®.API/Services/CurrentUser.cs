using ImageLinks_.Application.Common;
using System.Security.Claims;

namespace ImageLinks_.API.Services
{
    public class CurrentUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? Id =>
            User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? UserName =>
            User?.FindFirstValue(ClaimTypes.Name);

        public string? Email =>
            User?.FindFirstValue(ClaimTypes.Email);

        public int? LicenseDomainId =>
            int.TryParse(User?.FindFirstValue("LicenseDomainId"), out var v) ? v : null;

    }
}
