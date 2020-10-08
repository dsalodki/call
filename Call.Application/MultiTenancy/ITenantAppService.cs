using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Call.MultiTenancy.Dto;

namespace Call.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
