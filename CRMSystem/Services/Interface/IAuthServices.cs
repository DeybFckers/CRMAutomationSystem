using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IAuthServices
    {
        Task<UserResponseDto> RegisterAsync(CreateUserDto request);

        Task<AuthTokenResult> LoginAsync(LoginDto request, string? ipAddress, string? userAgent);

        Task<AuthTokenResult?> RefreshAsync(string refreshToken, string? ipAddress, string? userAgent);

        Task LogoutAsync(string refreshToken);
    }
}
