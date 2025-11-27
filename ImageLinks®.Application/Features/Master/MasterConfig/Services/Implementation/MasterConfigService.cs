using ImageLinks_.Application.Features.Master.MasterConfig.IRepository;
using ImageLinks_.Application.Features.Master.MasterConfig.Services.Interface;

namespace ImageLinks_.Application.Features.Master.MasterConfig.Services.Implementation;

public class MasterConfigService : IMasterConfigService
{
    private readonly IMasterConfigRepository _masterConfigRepository;

    public MasterConfigService(IMasterConfigRepository masterConfigRepository)
    {
        _masterConfigRepository = masterConfigRepository;
    }
    public bool GetAllMasterConfig(CancellationToken ct)
    {
        var x = _masterConfigRepository.GetAll(ct).Result;
        return true;
    }
}
