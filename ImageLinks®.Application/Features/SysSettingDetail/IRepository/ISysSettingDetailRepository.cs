using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
namespace ImageLinks_.Application;
public interface ISysSettingDetailRepository : IRepository<SysSettingDetail>
{
    Task<List<SysSettingDetail>?> SelectAsync(SysSettingDetail filter, CancellationToken ct = default);
}

