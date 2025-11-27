using ImageLinks_.API.Middleware;
using ImageLinks_.API.Services;
using ImageLinks_.Application;
using ImageLinks_.Application.Common;
using ImageLinks_.Application.Features.Groups.IRepository;
using ImageLinks_.Application.Features.Groups.Services.Implementation;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.Identity.Services.Implementation;
using ImageLinks_.Application.Features.Identity.Services.Interface;
using ImageLinks_.Application.Features.Master.MasterConfig.IRepository;
using ImageLinks_.Application.Features.Master.MasterConfig.Services.Implementation;
using ImageLinks_.Application.Features.Master.MasterConfig.Services.Interface;
using ImageLinks_.Application.Features.Master.MasterDbServersConfig.IRepository;
using ImageLinks_.Application.Features.Master.MasterDbServersConfig.Services.Implementation;
using ImageLinks_.Application.Features.Master.MasterDbServersConfig.Services.Interface;
using ImageLinks_.Application.Features.Master.UsersMaster.IRepository;
using ImageLinks_.Application.Features.Master.UsersMaster.Services.Implementation;
using ImageLinks_.Application.Features.Master.UsersMaster.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Services.Implementation;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Implementation;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Implementation;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Implementation;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Services.Implementation;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Services.Interface;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.Features.Users.Services.Implementation;
using ImageLinks_.Application.Features.Users.Services.Interface;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Infrastructure;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Features.Groups;
using ImageLinks_.Infrastructure.Features.Master;
using ImageLinks_.Infrastructure.Features.StorgeHierarchy;
using ImageLinks_.Infrastructure.Features.Users.Repository;
using ImageLinks_.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string? appProvider = builder.Configuration.GetConnectionString("DefaultConnection_ProviderName");
string? appConnStr = builder.Configuration.GetConnectionString("DefaultConnection");

string? masterProvider = builder.Configuration.GetConnectionString("MasterConnection_ProviderName");
string? masterConnStr = builder.Configuration.GetConnectionString("MasterConnection");

IConfigurationSection? jwtSettings = builder.Configuration.GetSection("JwtSettings");
string? key = jwtSettings["Secret"]!;

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost", 
        ValidAudience = "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key))
    };
});

// Add authorization
builder.Services.AddAuthorization();


builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (string.Equals(appProvider, "SQL", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(appConnStr);
    }
    else if (string.Equals(appProvider, "ORACLE", StringComparison.OrdinalIgnoreCase))
    {
        options.UseOracle(appConnStr);
    }
    else
    {
        throw new InvalidOperationException($"Unsupported provider: {appProvider}");
    }
});

builder.Services.AddDbContext<MasterDbContext>(options =>
{
    if (string.Equals(masterProvider, "SQL", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(masterConnStr);
    }
    else if (string.Equals(masterProvider, "ORACLE", StringComparison.OrdinalIgnoreCase))
    {
        options.UseOracle(masterConnStr);
    }
    else
    {
        throw new InvalidOperationException($"Unsupported provider: {masterProvider}");
    }
});

builder.Services.AddScoped<IGenericRepository, GenericRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSecRepository, UserSecRepository>();
builder.Services.AddScoped<ISysSettingDetailRepository, SysSettingDetailRepository>();
builder.Services.AddScoped<IUsersMasterRepository, UsersMasterRepository>();
builder.Services.AddScoped<IMasterConfigRepository, MasterConfigRepository>();
builder.Services.AddScoped<IMasterDbServersConfigRepository, MasterDbServersConfigRepository>();

builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupSecRepository, GroupSecRepository>();
builder.Services.AddScoped<IGroupMbrRepository, GroupMbrRepository>();

builder.Services.AddScoped<ITreeRepository,TreeRepository>();
builder.Services.AddScoped<ICabinetsRepository, CabinetsRepository>();
builder.Services.AddScoped<IDrawersRepository, DrawersRepository>();
builder.Services.AddScoped<IFoldersRepository, FoldersRepository>();
builder.Services.AddScoped<IDocRepository, DocRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserSecService, UserSecService>();
builder.Services.AddScoped<IUserPrivilegeService, UserPrivilegeService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUsersMasterService, UsersMasterService>();
builder.Services.AddScoped<IMasterConfigService, MasterConfigService>();
builder.Services.AddScoped<IMasterDbServersConfigService, MasterDbServersConfigService>();

builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupSecService, GroupSecService>();
builder.Services.AddScoped<IGroupMbrService, GroupMbrService>();

builder.Services.AddScoped<ITreeService, TreeService>();
builder.Services.AddScoped<ICabinetsService, CabinetsService>();
builder.Services.AddScoped<IDrawersService, DrawersService>();
builder.Services.AddScoped<IFoldersService, FoldersService>();
builder.Services.AddScoped<IDocService, DocService>();

builder.Services.AddProblemDetails();   
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<IUser, CurrentUser>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();
app.MapControllers();

app.Run();