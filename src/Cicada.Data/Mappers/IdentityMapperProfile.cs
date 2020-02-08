using AutoMapper;
using Cicada.Data.Extensions;
using Cicada.ViewModels.Common;
using Cicada.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Cicada.Data.Mappers
{
    public class IdentityMapperProfile : Profile
    {
        public IdentityMapperProfile()
        {
            // entity to model
            CreateMap<IdentityUser, UserDto>(MemberList.Destination);

            CreateMap<UserLoginInfo, UserProviderDto>(MemberList.Destination);

            CreateMap<IdentityError, ViewErrorMessage>(MemberList.Destination)
                .ForMember(x => x.ErrorKey, opt => opt.MapFrom(src => src.Code))
                .ForMember(x => x.ErrorMessage, opt => opt.MapFrom(src => src.Description));

            // entity to model
            CreateMap<IdentityRole, RoleDto>(MemberList.Destination);

            CreateMap<IdentityUser, IdentityUser>(MemberList.Destination)
                .ForMember(x => x.SecurityStamp, opt => opt.Ignore());

            CreateMap<PagedList<IdentityUser>, UsersDto>(MemberList.Destination)
                .ForMember(x => x.Users,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<IdentityUserClaim<string>, UserClaimDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserClaim<string>, UserClaimsDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PagedList<IdentityRole>, RolesDto>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<IdentityRole>, UserRolesDto>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<IdentityUserClaim<string>>, UserClaimsDto>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<IdentityRoleClaim<string>>, RoleClaimsDto>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<List<UserLoginInfo>, UserProvidersDto>(MemberList.Destination)
                .ForMember(x => x.Providers, opt => opt.MapFrom(src => src));

            CreateMap<IdentityRoleClaim<string>, RoleClaimDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityRoleClaim<string>, RoleClaimsDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserLogin<string>, UserProviderDto>(MemberList.Destination);

            // model to entity
            CreateMap<RoleDto, IdentityRole>(MemberList.Source);

            CreateMap<RoleClaimsDto, IdentityRoleClaim<string>>(MemberList.Source);

            CreateMap<UserClaimsDto, IdentityUserClaim<string>>(MemberList.Source)
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(src => src.ClaimId));

            // model to entity
            CreateMap<UserDto, IdentityUser>(MemberList.Source);
        }
    }
}