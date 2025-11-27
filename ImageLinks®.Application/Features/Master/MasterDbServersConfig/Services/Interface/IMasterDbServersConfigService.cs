namespace ImageLinks_.Application.Features.Master.MasterDbServersConfig.Services.Interface;

public interface IMasterDbServersConfigService
{

    bool GetAllMasterDbServersConfig(CancellationToken ct);
}