using ImageLinks_.Application.Features.Master.MasterDbServersConfig.IRepository;
using ImageLinks_.Domain.Models.MasterModels;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;


namespace ImageLinks_.Infrastructure.Features.Master.Repository
{
    public class MasterDbServersConfigRepository : Repository<MasterDbServersConfigModel>, IMasterDbServersConfigRepository
    {
        private readonly MasterDbContext _db;
        public MasterDbServersConfigRepository(MasterDbContext db) : base(null, db)
        {
            _db = db;
        }
    }
}
