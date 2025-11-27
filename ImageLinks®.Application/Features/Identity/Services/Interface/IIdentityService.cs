using ImageLinks_.Application.Features.Identity.DTO;
using ImageLinks_.Application.Features.Identity.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Identity.Services.Interface
{
    public interface IIdentityService
    {
        Task<Result<TokenDto>> Login(LoginRequest login, CancellationToken cancellationToken);
    }
}
