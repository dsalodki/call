using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Call.EntityFramework;

namespace Call.Migrator
{
    [DependsOn(typeof(CallDataModule))]
    public class CallMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<CallDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}