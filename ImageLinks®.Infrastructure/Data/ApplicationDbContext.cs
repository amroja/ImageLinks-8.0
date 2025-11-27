using ImageLinks_.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageLinks_.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> USERS { get; set; }
    public DbSet<SysSettingDetail> SETT_SYSSETTDETAILS { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dbProvider = Database.ProviderName;
    }
}
