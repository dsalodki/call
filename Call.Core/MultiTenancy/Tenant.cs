using System.Collections.Generic;
using Abp.MultiTenancy;
using Call.Users;

namespace Call.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }

        public virtual ICollection<Calls.Request> Requests { get; set; }
    }
}