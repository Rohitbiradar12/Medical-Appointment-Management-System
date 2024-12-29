namespace AppointmentManagementService.Model.DTO
{
    public class AvailabilityDto
    {
        public int DoctorId { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
