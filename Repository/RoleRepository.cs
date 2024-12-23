using Microsoft.EntityFrameworkCore;
using UserManagementService.Context;
using UserManagementService.Model;

namespace UserManagementService.Repository
{
    public class RoleRepository : IRepository<int, Role>
    {
        private readonly UserDbContext _context;
        public RoleRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task<Role> Add(Role entity)
        {
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Role> Delete(int key)
        {
            var role =  await Get(key);
            if (role == null)
            {
                throw new Exception("Role to be deleted not found");
            }
            _context.Remove(role);
            return role;
        }

        public async Task<Role> Get(int key)
        {
            var role =  await _context.Roles.SingleOrDefaultAsync(r => r.RoleId == key);
            if (role == null)
            {
                throw new Exception("User not found");
            }
             return role;
        }

        public async Task<Role> Get(string roleName)
        {
            var role  = await Get(roleName);
            if (role == null)
            {
                throw new Exception("Role not found of the user");
            }

            return role;
        }

        public async Task<ICollection<Role>> GetAll()
        {
            var role = _context.Roles;
            if (role.Count() == 0)
            {
                throw new Exception($"{nameof(Role)} has no role");
            }
             return await role.ToListAsync();
        }

        public async Task<Role> Update(Role entity)
        {
            var role = await Get(entity.RoleId);
            if (role == null)
            {
                throw new Exception("Role to be deleted not found");
            }
            _context.Roles.Update(entity);
             await _context.SaveChangesAsync();
            return entity;
        }
    }
}
