using ImageLinks_.Application.Features.Master.UsersMaster.IRepository;
using ImageLinks_.Domain.Models.MasterModels;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;

namespace ImageLinks_.Infrastructure.Features.Master;

public class UsersMasterRepository : Repository<UserMaster>, IUsersMasterRepository
{
    private readonly MasterDbContext _db;
    public UsersMasterRepository(MasterDbContext db) : base(null, db)
    {
        _db = db;
    }
    //public Task<List<UserMaster>> GetAllUsers(CancellationToken ct = default)
    //{
    //    throw new NotImplementedException();
    //}
}
