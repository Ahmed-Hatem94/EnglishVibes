using AutoMapper;
using EnglishVibes.API.DTO;
using System.Text.RegularExpressions;

namespace Talabat.APIS.Helpers
{
	public class MappingProfiles:Profile
	{
        public MappingProfiles()
        {
            CreateMap<Group, InActiveGroupDto>();
                
        }
    }
}
