using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marketplace.Application.Common;
using Marketplace.Application.DTOs;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly MarketplaceDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(MarketplaceDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto request)
    {
        // Check if the email already exists
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
            return Result<AuthResponseDto>.Failure("Email is already registered.", ErrorType.Conflict);

        // Hash the password using BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create the Domain Entity
        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hashedPassword
        };

        // Save to SQL Server
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return Result<AuthResponseDto>.Success(new AuthResponseDto(string.Empty, "User registered successfully. Please log in."));
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto request)
    {
        // Find the user by Email
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        // Verify the user exists and the password matches the hash
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponseDto>.Failure("Invalid email or password.", ErrorType.Unauthorized);

        // Generate the JWT
        var token = GenerateJwtToken(user);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(token, "Login successful."));
    }

    private string GenerateJwtToken(User user)
    {
        // Get the secret key
        var jwtSettings = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        // Define the payload
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Create the cryptographic signature
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Assemble the token blueprint
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = credentials
        };

        // Forge the final token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
