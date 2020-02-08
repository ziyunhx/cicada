using System.Collections.Generic;

namespace Cicada.ViewModels.Identity
{
    public class UserProvidersDto : UserProviderDto
    {
        public UserProvidersDto()
        {
            Providers = new List<UserProviderDto>();
        }

        public List<UserProviderDto> Providers { get; set; }
    }
}
