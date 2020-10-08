using Abp.Authorization;
using Call.Authorization.Roles;
using Call.MultiTenancy;
using Call.Users;

namespace Call.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
