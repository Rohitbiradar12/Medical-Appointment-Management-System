namespace DoctorManagementService.Model.DTO
{
    public class DoctorProfileDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
    }
}
