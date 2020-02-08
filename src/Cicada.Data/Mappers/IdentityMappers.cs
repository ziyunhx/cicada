using AutoMapper;
using Cicada.Data.Extensions;
using Cicada.ViewModels.Common;
using Cicada.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Cicada.Data.Mappers
{
    public static class IdentityMappers
    {
        internal static IMapper Mapper { get; }

        static IdentityMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityMapperProfile>())
                .CreateMapper();
        }

        public static UsersDto ToModel(this PagedList<IdentityUser> users)
        {
            return Mapper.Map<UsersDto>(users);
        }

        public static UserClaimsDto ToModel(this PagedList<IdentityUserClaim<string>> claims)
        {
            return Mapper.Map<UserClaimsDto>(claims);
        }

        public static UserClaimsDto ToModel(this IdentityUserClaim<string> claim)
        {
            return Mapper.Map<UserClaimsDto>(claim);
        }

        public static RolesDto ToModel(this PagedList<IdentityRole> roles)
        {
            return Mapper.Map<RolesDto>(roles);
        }

        public static UserRolesDto MapToModel(this PagedList<IdentityRole> roles)
        {
            return Mapper.Map<UserRolesDto>(roles);
        }

        public static UserDto ToModel(this IdentityUser user)
        {
            return Mapper.Map<UserDto>(user);
        }

        public static List<ViewErrorMessage> ToModel(this IEnumerable<IdentityError> error)
        {
            return Mapper.Map<List<ViewErrorMessage>>(error);
        }

        public static RoleDto ToModel(this IdentityRole role)
        {
            return Mapper.Map<RoleDto>(role);
        }

        public static UserProviderDto ToModel(this IdentityUserLogin<string> login)
        {
            return Mapper.Map<UserProviderDto>(login);
        }

        public static UserProvidersDto ToModel(this List<UserLoginInfo> login)
        {
            return Mapper.Map<UserProvidersDto>(login);
        }

        public static RoleClaimsDto ToModel(this IdentityRoleClaim<string> roleClaim)
        {
            return Mapper.Map<RoleClaimsDto>(roleClaim);
        }

        public static List<RoleDto> ToModel(this List<IdentityRole> roles)
        {
            return Mapper.Map<List<RoleDto>>(roles);
        }

        public static RoleClaimsDto ToModel(this PagedList<IdentityRoleClaim<string>> roleClaim)
        {
            return Mapper.Map<RoleClaimsDto>(roleClaim);
        }
        
        public static IdentityUserClaim<string> ToEntity(this UserClaimsDto claim)
        {
            return Mapper.Map<IdentityUserClaim<string>>(claim);
        }

        public static IdentityUser ToEntity(this UserDto user)
        {
            return Mapper.Map<IdentityUser>(user);
        }

        public static IdentityRoleClaim<string> ToEntity(this RoleClaimsDto roleClaim)
        {
            return Mapper.Map<IdentityRoleClaim<string>>(roleClaim);
        }

        public static IdentityRole ToEntity(this RoleDto role)
        {
            return Mapper.Map<IdentityRole>(role);
        }

        public static IdentityUser MapTo(this IdentityUser user, IdentityUser newUser)
        {
            return Mapper.Map(newUser, user);
        }
    }
}
