using ImageLinks_.Application.Common.Models;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
namespace ImageLinks_.Application.Features.Users.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> SelectAsync(User filter, CancellationToken ct = default);
        Task<List<User>> GetAllUsers(CancellationToken ct = default);

        Task<List<User>> GetUsersFilter(PageRequest pageRequest, CancellationToken ct = default);

    }
}
