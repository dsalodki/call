using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using Call.Authorization.Roles;
using Call.MultiTenancy;
using Call.Users;

namespace Call.EntityFramework
{
    public class CallDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
        public virtual IDbSet<Calls.Request> Providers { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public CallDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in CallDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of CallDbContext since ABP automatically handles it.
         */
        public CallDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public CallDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}
