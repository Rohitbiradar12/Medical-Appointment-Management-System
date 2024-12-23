using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Model
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Collection<User> Users { get; set; }

    }
}
