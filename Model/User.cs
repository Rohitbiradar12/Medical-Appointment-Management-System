using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementService.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] Password { get; set; } 
        public DateTime CreatedAt { get; set; }
        public byte[] Key { get; set; }
        public int RoleId {  get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role role { set; get; }  
           
    }
}
