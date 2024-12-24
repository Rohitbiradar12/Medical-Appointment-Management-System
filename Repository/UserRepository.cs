using Microsoft.EntityFrameworkCore;
using UserManagementService.Context;
using UserManagementService.Model;

namespace UserManagementService.Repository
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly UserDbContext _context;    
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task<User> Add(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Delete(int key)
        {
            var user = await Get(key);
            if(user != null)
            {
                _context.Remove(user);
            }
            else
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> Get(int key)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id==key);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> Get(string email)
        {
            return await _context.Users
                .Include(u => u.role) 
                .SingleOrDefaultAsync(u => u.Email == email);
        }


        public async Task<ICollection<User>> GetAll()
        {
            var user = _context.Users;
            if (user.Count() == 0)
            {
                throw new Exception("No users found");
            }
               return await user.ToListAsync();
        }

        public async Task<User> Update(User entity)
        {
            var user = await Get(entity.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
