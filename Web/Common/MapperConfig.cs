using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Core.Entities;
using Web.Identity;

namespace Web.Common
{
    public static class MapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<IdentityRole, Role>()
                    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
                //.ForMember(dest => dest.Users, opt => opt.Ignore())
                //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

                //cfg.CreateMap<Role, IdentityRole>()
                //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId));

                cfg.CreateMap<IdentityUser, User>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();

                cfg.CreateMap<User, IdentityUser>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId)).ReverseMap();

            });
        }
    }
}