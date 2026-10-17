using LSMS.Application.DTO.Auth;
using LSMS.Application.Interfaces.Auth;
using LSMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LSMS.Infrastructure.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        #region Field

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration; 
        #endregion

        #region Ctor
        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        } 
        #endregion
        public async Task<UserDTO?> LoginAsync(LoginDTO request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !user.IsActive)
                return null;

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
                
            if (!passwordValid)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var key = _configuration["Jwt:Key"]?? throw new InvalidOperationException("Jwt:Key is missing.");
                

            var issuer = _configuration["Jwt:Issuer"]?? throw new InvalidOperationException("Jwt:Issuer is missing.");
                

            var audience = _configuration["Jwt:Audience"]?? throw new InvalidOperationException("Jwt:Audience is missing.");
                
            var expiresAt = DateTime.UtcNow.AddMinutes(30);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            claims.AddRange(roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

            var signingKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(key));
               
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new UserDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Roles = roles
            };
        }
    }
    
}
