using ImageLinks_.Application.Features.Master.UsersMaster.DTO;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Master.MasterConfig.Services.Interface;

public interface IMasterConfigService
{

  bool GetAllMasterConfig(CancellationToken ct);
}
