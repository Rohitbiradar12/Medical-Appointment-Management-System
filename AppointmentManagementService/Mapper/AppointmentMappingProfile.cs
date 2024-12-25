using AppointmentManagementService.Model.DTO;
using AppointmentManagementService.Model;
using AutoMapper;


namespace AppointmentManagementService.Mapper
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile()
        {
            // Map from BookAppointmentRequestDTO to Appointment entity
            CreateMap<BookAppointmentRequestDTO, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Map from Appointment entity to AppointmentResponseDTO
            CreateMap<Appointment, AppointmentResponseDTO>();

            CreateMap<RescheduleAppointmentRequestDTO, Appointment>()
                .ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(src => src.NewAppointmentDateTime))
                .ForAllMembers(opt => opt.Ignore());


        }
    }
}