using UserManagementService.Model;

namespace UserManagementService.Service
{
    public interface IEmailService
    {
        public Task SendEmailVerificationAsync(User user);
    }
}
