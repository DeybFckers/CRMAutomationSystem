using CRMSystem.Models.Configuration;
using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly JwtSettings _jwtSettings;

        public AuthController(
            IAuthServices authServices,
            IOptions<JwtSettings> jwtSettings)
        {
            _authServices = authServices;
            _jwtSettings = jwtSettings.Value;
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto request)
        {
            try
            {
                var user =
                    await _authServices.RegisterAsync(request);

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginDto request)
        {
            try
            {
                var result = await _authServices.LoginAsync(
                        request,
                        HttpContext.Connection.RemoteIpAddress?
                            .ToString(),
                        Request.Headers.UserAgent.ToString());

                SetAccessTokenCookie(result.AccessToken);

                SetRefreshTokenCookie( result.RefreshToken);

                return Ok(new
                {
                    message = "Login successful."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue( "refresh_token", out var refreshToken))
            {
                return Unauthorized(new
                {
                    message = "Refresh token is missing."
                });
            }

            var result = await _authServices.RefreshAsync(
                    refreshToken,
                    HttpContext.Connection.RemoteIpAddress?
                        .ToString(),
                    Request.Headers.UserAgent.ToString());

            if (result == null)
            {
                DeleteAuthCookies();

                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });
            }

            SetAccessTokenCookie(result.AccessToken);

            SetRefreshTokenCookie( result.RefreshToken);

            return Ok(new
            {
                message = "Token refreshed successfully."
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue( "refresh_token", out var refreshToken))
            {
                await _authServices.LogoutAsync( refreshToken);
            }

            DeleteAuthCookies();

            return Ok(new
            {
                message = "Logout successful."
            });
        }

        private void SetAccessTokenCookie(
            string token)
        {
            Response.Cookies.Append(
                "access_token",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires =
                        DateTimeOffset.UtcNow.AddMinutes(
                            _jwtSettings.AccessTokenMinutes),
                    Path = "/"
                });
        }

        private void SetRefreshTokenCookie(
            string token)
        {
            Response.Cookies.Append(
                "refresh_token",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires =
                        DateTimeOffset.UtcNow.AddDays(
                            _jwtSettings.RefreshTokenDays),
                    Path = "/api/auth"
                });
        }

        private void DeleteAuthCookies()
        {
            Response.Cookies.Delete(
                "access_token",
                new CookieOptions
                {
                    Path = "/"
                });

            Response.Cookies.Delete(
                "refresh_token",
                new CookieOptions
                {
                    Path = "/api/auth"
                });
        }
    }
}