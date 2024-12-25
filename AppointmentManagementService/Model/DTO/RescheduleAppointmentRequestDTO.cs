namespace AppointmentManagementService.Model.DTO
{
    
        public class RescheduleAppointmentRequestDTO
        {
            public int AppointmentId { get; set; }   
            public DateTime NewAppointmentDateTime { get; set; }  
        }
    

}
