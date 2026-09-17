using CRMSystem.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRMSystem.Services.Implementation
{
    //Inject this to other user if you have assigneduser so you will use the jwt claim for organization because assigneduser is connected to organization
    public class CurrentUserServices : ICurrentUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                var value = user?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                    ?? user?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(value, out var userId))
                    throw new UnauthorizedAccessException("User ID claim is missing or invalid.");

                return userId;
            }
        }

        public Guid OrganizationId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?.User.FindFirstValue("organization_id");

                if (!Guid.TryParse(value, out var organizationId))
                    throw new UnauthorizedAccessException("Organization ID claim is missing or invalid.");

                return organizationId;
            }
        }

        public string? Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            }
        }
    }
}