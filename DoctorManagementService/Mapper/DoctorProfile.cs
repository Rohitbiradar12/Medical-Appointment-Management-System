using AutoMapper;
using DoctorManagementService.Model.DTO;
using DoctorManagementService.Models;

namespace DoctorManagementService.Mapper
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor,DoctorProfileDTO>().ReverseMap();
            
        }
    }
}
