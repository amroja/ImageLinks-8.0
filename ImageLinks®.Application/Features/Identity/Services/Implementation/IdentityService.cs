using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Identity.DTO;
using ImageLinks_.Application.Features.Identity.Requests;
using ImageLinks_.Application.Features.Identity.Services.Interface;
using ImageLinks_.Application.Features.Master.MasterConfig.Services.Interface;
using ImageLinks_.Application.Features.Master.MasterDbServersConfig.Services.Interface;
using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.Features.Users.Services.Interface;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ImageLinks_.Application.Features.Identity.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration ;
        private readonly IMasterConfigService _masterConfigService;
        private readonly IMasterDbServersConfigService _masterDbServersConfigService;
        private readonly IUserPrivilegeService _userPrivilegeService;

        public IdentityService(IUserRepository userRepository,
            IConfiguration configuration,
            IMasterConfigService masterConfigService,
            IMasterDbServersConfigService masterDbServersConfigService,
            IUserPrivilegeService userPrivilegeService)
        {
            _userRepository = userRepository;
            _configuration= configuration;
            _masterConfigService = masterConfigService;
            _masterDbServersConfigService = masterDbServersConfigService;
            _userPrivilegeService = userPrivilegeService;
        }

        public async Task<Result<TokenDto>> Login(LoginRequest login, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.Password))
                return Error.Validation("InvalidCredentials", "Username and password are required.");

            User? user = null;

            var dapperUser = new User
            {
                UserName = login.UserName,
                UserPass = Encryption.EncryptAES(login.Password)
            };

            user = (await _userRepository.SelectAsync(dapperUser, ct)).SingleOrDefault();

            if (user == null)
                return Error.NotFound("UserNotFound", "Invalid username or password.");

            UserPrivilegeDto? privilage = await _userPrivilegeService.GetUserPrivilege(new UserDto { UserId = user.UserId.ToString() }, ct);

            return CreateAsync(user, privilage);
        }

        private TokenDto CreateAsync(User user, UserPrivilegeDto privileges)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");
            string issuer = jwtSettings["Issuer"]!;
            string audience = jwtSettings["Audience"]!;
            string key = jwtSettings["Secret"]!;
            DateTime expires = DateTime.UtcNow.AddMinutes(
                int.Parse(jwtSettings["TokenExpirationInMinutes"]!)
            );

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.UserMail ?? ""),
                new Claim("LicenseDomainId", user.LicenseDomainId?.ToString() ?? ""),
                new Claim("privileges", JsonSerializer.Serialize(privileges, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }))
            };

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(descriptor);

            return new TokenDto
            {
                AccessToken = tokenHandler.WriteToken(securityToken),
                ExpiresOnUtc = expires
            };
        }
    }
}
