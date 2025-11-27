using ImageLinks_.Domain.Models.MasterModels;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Infrastructure.Data;

public class MasterDbContext : DbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
    {
    }
    public DbSet<UserMaster> USERS { get; set; }
    public DbSet<MasterConfigModel> Master_Config { get; set; }
    public DbSet<MasterDbServersConfigModel> masterDbServersConfigModels { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dbProvider = Database.ProviderName;
    }
}
