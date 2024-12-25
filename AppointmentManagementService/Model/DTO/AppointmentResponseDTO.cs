namespace AppointmentManagementService.Model.DTO
{
    
        public class AppointmentResponseDTO
        {
            public int Id { get; set; }                       
            public int PatientId { get; set; }                 
            public int DoctorId { get; set; }                               
            public DateTime AppointmentDateTime { get; set; }  
            public string Status { get; set; }                
            public DateTime CreatedAt { get; set; }           
            public DateTime UpdatedAt { get; set; }            
        }
    

}
