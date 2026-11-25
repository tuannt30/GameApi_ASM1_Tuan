using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GameApi.Data;
using GameApi.DTOs;
using GameApi.Models;

namespace GameApi.Services
{
    public interface IAuthService
    {
        Task<AccountResponse?> RegisterAsync(RegisterRequest request);
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Account> _passwordHasher;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Account>();
        }

        public async Task<AccountResponse?> RegisterAsync(RegisterRequest request)
        {
            var existing = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == request.Username);
            if (existing != null)
            {
                return null;
            }

            var account = new Account
            {
                Username = request.Username,
                CreatedAt = DateTime.UtcNow
            };
            account.PasswordHash = _passwordHasher.HashPassword(account, request.Password);

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return new AccountResponse
            {
                Id = account.Id,
                Username = account.Username
            };
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == request.Username);
            if (account == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyString = _configuration["Jwt:Key"] ?? "SuperSecretGameApiKeyForAsm1Gam106_2026_MustBe32CharsMin!";
            var key = Encoding.UTF8.GetBytes(keyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Name, account.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Jwt:Issuer"] ?? "GameApi",
                Audience = _configuration["Jwt:Audience"] ?? "GameApiUsers",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new LoginResponse
            {
                Token = tokenString,
                Username = account.Username
            };
        }
    }
}
