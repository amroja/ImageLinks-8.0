using ImageLinks_.Application.Features.Master.UsersMaster.DTO;
using ImageLinks_.Application.Features.Master.UsersMaster.IRepository;
using ImageLinks_.Application.Features.Master.UsersMaster.Services.Interface;
using ImageLinks_.Domain.Models.MasterModels;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Master.UsersMaster.Services.Implementation;

public class UsersMasterService : IUsersMasterService
{
    private readonly IUsersMasterRepository _usersMasterRepository;

    public UsersMasterService(IUsersMasterRepository usersMasterRepository)
    {
        _usersMasterRepository = usersMasterRepository;
    }
    public Task<Result<List<UserMasterDto>>> GetAllUsersMaster(CancellationToken ct)
    {
     Task<List<UserMaster>>? user=   _usersMasterRepository.GetAll(ct);
        throw new NotImplementedException();
    }
}
