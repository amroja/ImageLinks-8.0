using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models.MasterModels;

namespace ImageLinks_.Application.Features.Master.UsersMaster.IRepository;
public interface IUsersMasterRepository : IRepository<UserMaster>
{
    //Task<List<UserMaster>> GetAllUsers(CancellationToken ct = default);
}
