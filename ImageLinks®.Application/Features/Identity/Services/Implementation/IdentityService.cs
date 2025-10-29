using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Identity.DTO;
using ImageLinks_.Application.Features.Identity.Requests;
using ImageLinks_.Application.Features.Identity.Services.Interface;
using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.Features.Users.Mappers;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ImageLinks_.Application.Features.Identity.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration ;
        public IdentityService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration= configuration;
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

            user = await _userRepository.SelectAsync(dapperUser, ct);  // Dapper

            user = await _userRepository.Get(u => u.UserName.ToLower() == dapperUser.UserName.ToLower() && u.UserPass == dapperUser.UserPass);  // EF

            if (user == null)
                return Error.NotFound("UserNotFound", "Invalid username or password.");

           return CreateAsync(user);
        }

        private TokenDto CreateAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var key = jwtSettings["Secret"]!;

            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["TokenExpirationInMinutes"]!));

            var claims = new List<Claim>
            {
            new (JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new (JwtRegisteredClaimNames.Name, user.UserName!),
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            return new TokenDto
            {
                AccessToken = tokenHandler.WriteToken(securityToken),
                ExpiresOnUtc = expires
            };
        }
    }
}
