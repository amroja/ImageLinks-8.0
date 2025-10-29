using ImageLinks_.API.Middleware;
using ImageLinks_.Application;
using ImageLinks_.Application.Features.Identity.Services.Implementation;
using ImageLinks_.Application.Features.Identity.Services.Interface;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.Features.Users.Services.Implementation;
using ImageLinks_.Application.Features.Users.Services.Interface;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Infrastructure;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Features.Users.Repository;
using ImageLinks_.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var provider = builder.Configuration.GetConnectionString("DefaultConnection_ProviderName");
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = jwtSettings["Secret"]!;

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
    if (string.Equals(provider, "SQL", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(connStr);
    }
    else if (string.Equals(provider, "ORACLE", StringComparison.OrdinalIgnoreCase))
    {
        options.UseOracle(connStr);
    }
    else
    {
        throw new InvalidOperationException($"Unsupported provider: {provider}");
    }
});

builder.Services.AddScoped<IGenericRepository, GenericRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISysSettingDetailRepository, SysSettingDetailRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddProblemDetails();   
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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