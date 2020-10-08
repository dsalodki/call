using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Configuration;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using Quartz;
using Quartz.Impl;

namespace Call.Web
{
    public class MvcApplication : AbpWebApplication<CallWebModule>
    {
        private IScheduler scheduler;
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            base.Application_Start(sender, e);

            // Grab the Scheduler instance from the Factory
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory();//props
            scheduler = factory.GetScheduler();

            // and start it off
            scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<DBJob>()
                .WithIdentity("deleteOldRequests", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            scheduler.Shutdown();
        }

        public class DBJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                var connStr = WebConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                     var cmd = new SqlCommand("DELETE FROM [dbo].[Requests] WHERE Created < DATEADD(year, -1, GETDATE())", conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
