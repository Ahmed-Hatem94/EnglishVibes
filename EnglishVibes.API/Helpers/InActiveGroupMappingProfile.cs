using AutoMapper;
using EnglishVibes.API.DTO;
using EnglishVibes.Data.Models;

namespace EnglishVibes.API.Helpers
{
    public class InActiveGroupMappingProfile : Profile
    {
        public InActiveGroupMappingProfile()
        {

            CreateMap<Group, InActiveGroupDto>()
           .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students.Select(s => s.UserName)));



        }

    }
}
