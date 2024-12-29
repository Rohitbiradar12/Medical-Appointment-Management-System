using DoctorManagementService.Model.DTO;
using DoctorManagementService.Model;
using AutoMapper;

namespace DoctorManagementService.Mapper
{
    public class AvailabilityProfile : Profile
    {
        public AvailabilityProfile  ()
        {
            CreateMap<Availability, AvailabilityDto>().ReverseMap();
        }
    }
}
