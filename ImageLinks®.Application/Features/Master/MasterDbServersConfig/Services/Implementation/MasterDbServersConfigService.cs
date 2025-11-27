using ImageLinks_.Application.Features.Master.MasterDbServersConfig.IRepository;
using ImageLinks_.Application.Features.Master.MasterDbServersConfig.Services.Interface;

namespace ImageLinks_.Application.Features.Master.MasterDbServersConfig.Services.Implementation;

public class MasterDbServersConfigService : IMasterDbServersConfigService
{
    private readonly IMasterDbServersConfigRepository _masterDbServersConfigRepository;

    public MasterDbServersConfigService(IMasterDbServersConfigRepository masterDbServersConfigRepository)
    {
        _masterDbServersConfigRepository = masterDbServersConfigRepository;
    }
    public bool GetAllMasterDbServersConfig(CancellationToken ct)
    {
        var x = _masterDbServersConfigRepository.GetAll(ct,null, "Master").Result;
        return true;
    }
}