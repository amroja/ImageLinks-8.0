
using ImageLinks_.Application.Features.Master.UsersMaster.DTO;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Master.UsersMaster.Services.Interface
{
    public interface IUsersMasterService
    {
        Task<Result<List<UserMasterDto>>> GetAllUsersMaster(CancellationToken ct);
    }
}
