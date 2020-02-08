using Cicada.ViewModels.Identity.Base;

namespace Cicada.ViewModels.Identity
{
    public class UserProviderDto : BaseUserProviderDto<string>
    {
        public string ProviderKey { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderDisplayName { get; set; }        
    }
}
