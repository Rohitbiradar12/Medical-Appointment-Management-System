using System.ComponentModel.DataAnnotations.Schema;
using DoctorManagementService.Models;

namespace DoctorManagementService.Model
{
    public class Availability
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
