using System.Security.Claims;

namespace AppointmentManagementService.Service
{
    public interface ITokenService
    {
        int GetUserIdFromToken(ClaimsPrincipal user);
        string GetUserRoleFromToken(ClaimsPrincipal user);
    }
}
