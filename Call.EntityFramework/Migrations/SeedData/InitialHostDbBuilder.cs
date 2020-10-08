using Call.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Call.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly CallDbContext _context;

        public InitialHostDbBuilder(CallDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
