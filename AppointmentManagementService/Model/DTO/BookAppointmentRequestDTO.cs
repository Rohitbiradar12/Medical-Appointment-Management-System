namespace AppointmentManagementService.Model.DTO
{
    public class BookAppointmentRequestDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
