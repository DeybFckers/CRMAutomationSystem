using CRMSystem.Data;
using CRMSystem.Models.Configuration;
using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CRMSystem.Services.Implementation
{
    public class AuthServices : IAuthServices
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthServices(IAuthRepository authRepository, IOptions<JwtSettings> jwtSettings)
        {
            _authRepository = authRepository;
             _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthTokenResult> LoginAsync(LoginDto request, string? ipAddress, string? userAgent)
        {
            var user = await _authRepository.GetUserByEmail(request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (user.Status != "ACTIVE")
            {
                throw new UnauthorizedAccessException("User account is inactive.");
            }

            var passwordValid = await _authRepository.CheckPassword(user, request.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var roles = await _authRepository.GetUserRoles(user);

            var accessToken = GenerateAccessToken(user, roles);

            var refreshToken = GenerateRefreshToken();

            var refreshTokenHash = HashToken(refreshToken);

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt =
                    DateTime.UtcNow.AddDays(
                        _jwtSettings.RefreshTokenDays),
                CreatedByIp = ipAddress,
                UserAgent = userAgent
            };

            await _authRepository.SaveRefreshToken(refreshTokenEntity);

            return new AuthTokenResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var hashedToken = HashToken(refreshToken);

            var token =await _authRepository.GetRefreshToken(hashedToken);

            if (token == null)
                return;

            if (token.RevokedAt != null)
                return;

            await _authRepository.RevokeRefreshToken(
                token);
        }

        public async Task<AuthTokenResult?> RefreshAsync(string refreshToken, string? ipAddress, string? userAgent)
        {
            var hashedToken = HashToken(refreshToken);

            var existingToken = await _authRepository.GetRefreshToken(
                    hashedToken);

            if (existingToken == null)
                return null;

            if (existingToken.RevokedAt != null)
                return null;

            if (existingToken.ExpiresAt <= DateTime.UtcNow)
                return null;

            var user = existingToken.User;

            if (user.Status != "ACTIVE")
                return null;

            var roles =await _authRepository.GetUserRoles(user);

            var accessToken = GenerateAccessToken(user, roles);

            var newRefreshToken = GenerateRefreshToken();

            var newRefreshTokenHash = HashToken(newRefreshToken);

            var newRefreshTokenEntity = new RefreshToken
                {
                    UserId = user.Id,
                    TokenHash = newRefreshTokenHash,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt =
                        DateTime.UtcNow.AddDays(
                            _jwtSettings.RefreshTokenDays),
                    CreatedByIp = ipAddress,
                    UserAgent = userAgent
                };

            await _authRepository.SaveRefreshToken( newRefreshTokenEntity);

            existingToken.RevokedAt =DateTime.UtcNow;

            existingToken.ReplacedByTokenId = newRefreshTokenEntity.Id;

            await _authRepository.RevokeRefreshToken(existingToken);

            return new AuthTokenResult
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<UserResponseDto> RegisterAsync(CreateUserDto request)
        {
            var existingUser = await _authRepository.GetUserByEmail(request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "Email is already registered.");
            }

            var user = request.Adapt<ApplicationUser>();
            user.UserName = request.Email;
            user.Status = "ACTIVE";

            var result =
                await _authRepository.CreateUser(
                    user,
                    request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description));

                throw new InvalidOperationException(errors);
            }

            if (request.Roles != null &&
                request.Roles.Count > 0)
            {
                var roleResult =
                    await _authRepository.AddRolesToUser(
                        user,
                        request.Roles);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(x => x.Description));

                    throw new InvalidOperationException(errors);
                }
            }

            var roles = await _authRepository.GetUserRoles(user);

            var response = user.Adapt<UserResponseDto>();

            response.Roles = roles.ToList();

            return response;
        }
        private string GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),

                new Claim( JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),

                new Claim( "organization_id", user.OrganizationId.ToString()),

                new Claim("first_name", user.FirstName),

                new Claim( "last_name", user.LastName)
            };

            foreach (var role in roles)
            {
                claims.Add( new Claim( ClaimTypes.Role,role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credentials =new SigningCredentials( key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires:
                    DateTime.UtcNow.AddMinutes(
                        _jwtSettings.AccessTokenMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(bytes);
        }

        private static string HashToken(
            string token)
        {
            var hash = SHA256.HashData( Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(hash);
        }
    }
}

