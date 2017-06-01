using AutoMapper;
using TimeZoneManager.Data.Models;
using TimeZoneManager.Services.Model;
using TimeZoneManager.ViewModel;

namespace TimeZoneManager.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, Register>();

            CreateMap<TimeZone, TimeZoneViewModel>()
            .ForMember(m => m.OwnerName, opt =>
            {
                opt.Condition((timeZone, model) => timeZone.Owner != null);
                opt.MapFrom(m => m.Owner.DisplayName);
            });

            CreateMap<TimeZoneViewModel, TimeZone>();

            CreateMap<TimeZoneAppUser, UsersViewModel>()
                .ForMember(m => m.LoginName, opt => opt.MapFrom(s => s.UserName));

            CreateMap<NewUserViewModel, Register>();
            CreateMap<UsersViewModel, Register>();
        }
    }
}
