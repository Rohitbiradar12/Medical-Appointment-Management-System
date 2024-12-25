using System.Security.Claims;

namespace AppointmentManagementService.Service
{
    public class TokenService : ITokenService
    {

        private readonly ILogger _logger;

        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }
        public int GetUserIdFromToken(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim not found in token.");
            }
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public string GetUserRoleFromToken(ClaimsPrincipal user)
        {
            var userRoleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (userRoleClaim == null)
            {
                _logger.LogWarning("Role claim not found in token.");
            }
            return userRoleClaim?.Value ?? string.Empty;
        }


    }
}