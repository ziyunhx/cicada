namespace Cicada.ViewModels.Identity.Base
{
    public class BaseUserRolesDto<TUserId, TRoleId>
    {
        public TUserId UserId { get; set; }

        public TRoleId RoleId { get; set; }
    }
}