using System.Threading.Tasks;
using Abp.Application.Services;
using Call.Roles.Dto;

namespace Call.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
