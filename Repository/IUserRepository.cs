using UserManagementService.Model;

namespace UserManagementService.Repository
{
    public interface IUserRepository : IRepository<int,User>
    {
        Task<User> GetUserByEmailVerificationTokenAsync(string token);
    }
}
