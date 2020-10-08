using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Call.MultiTenancy;

namespace Call.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}