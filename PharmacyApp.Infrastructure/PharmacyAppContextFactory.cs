using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PharmacyApp.Infrastructure
{
    public class PharmacyAppContextFactory : IDesignTimeDbContextFactory<PharmacyAppContext>
    {
        public PharmacyAppContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PharmacyAppContext>();
            optionsBuilder.UseSqlite("Data Source=PharmacyApp.db");

            return new PharmacyAppContext(optionsBuilder.Options);
        }
    }
}
