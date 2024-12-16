using AutoMapper;
using EnglishVibes.API.DTO;
using EnglishVibes.Data.Models;

namespace EnglishVibes.API.Helpers
{
    public class ActiveGroupMappingProfile : Profile
    {

        public ActiveGroupMappingProfile()
        {
            CreateMap<Group, ActiveGroupDto>()
         .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students.Select(s => s.UserName)))
         .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor.UserName))
         .ForMember(dest => dest.GroupWeekDays, opt => opt.MapFrom(src => src.GroupWeekDays.ToList()));
        }
    }
}
