using AutoMapper;
using EnglishVibes.API.DTO;
using EnglishVibes.Data.Models;

namespace EnglishVibes.API.Helpers
{
    public class ActiveGroupMappingProfile : Profile
    {

        public ActiveGroupMappingProfile()
        {
            CreateMap<Group, ActiveGroupDto>();
        }
    }
}
