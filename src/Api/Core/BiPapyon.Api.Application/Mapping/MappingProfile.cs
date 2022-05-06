using AutoMapper;
using BiPapyon.Api.Domain.Models;
using BiPapyon.Common.Models.Queries;

namespace BiPapyon.Api.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>()
                .ReverseMap();
        }
    }
}
