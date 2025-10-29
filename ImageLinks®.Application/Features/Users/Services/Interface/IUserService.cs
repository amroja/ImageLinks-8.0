using ImageLinks_.Application.Common.Models;
using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Users.Services.Interface
{
    public interface IUserService
    {
        Task<Result<List<UserDto>>> GetAllUsersEF(CancellationToken ct);
        Task<Result<List<UserDto>>> GetAllUsersDP(CancellationToken ct);
        Task<Result<PaginatedList<UserDto>>> GetAllUsersEfFilter(PageRequest pageRequest, CancellationToken ct);
    }
}
