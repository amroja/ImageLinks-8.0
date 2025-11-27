using ImageLinks_.Application.Features.Master.MasterConfig.IRepository;
using ImageLinks_.Domain.Models.MasterModels;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;

namespace ImageLinks_.Infrastructure.Features.Master.Repository;

public class MasterConfigRepository : Repository<MasterConfigModel>, IMasterConfigRepository
{
    private readonly MasterDbContext _db;
    public MasterConfigRepository(MasterDbContext db) : base(null, db)
    {
        _db = db;
    }
}
