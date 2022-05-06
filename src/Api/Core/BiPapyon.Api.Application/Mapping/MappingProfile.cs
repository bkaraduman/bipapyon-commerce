using AutoMapper;
using BiPapyon.Api.Domain.Models;
using BiPapyon.Common.Models.Queries;
using BiPapyon.Common.Models.RequestModels;

namespace BiPapyon.Api.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>()
                .ReverseMap();

            CreateMap<CreateUserCommand, User>();
            CreateMap<UpdateUserCommand, User>();
        }
    }
}
