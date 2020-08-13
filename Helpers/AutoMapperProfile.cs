using AutoMapper;
using goOfflineE.Entites;
using goOfflineE.Models;

namespace goOfflineE.Helpers
{
    public class AutoMapperProfile : Profile
    {
        // mappings between model and entity objects
        public AutoMapperProfile()
        {
            CreateMap<User, AccountResponse>();

            CreateMap<User, AuthenticateResponse>();

            CreateMap<RegisterRequest, User>();

            CreateMap<CreateRequest, User>();
            CreateMap<Entites.School, Models.School>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RowKey));
            CreateMap<Models.School, Entites.School>().ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => src.Id));

            CreateMap<TeacherRequest, User>();
            CreateMap<TeacherRequest, Teacher>();

            CreateMap<User, TeacherResponse>();
            CreateMap<Teacher, TeacherResponse>();

            CreateMap<UpdateRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        // ignore null role
                        if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                        return true;
                    }
                ));

        }
    }

}