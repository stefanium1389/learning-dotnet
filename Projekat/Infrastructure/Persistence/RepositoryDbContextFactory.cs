using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class RepositoryDbContextFactory : IDesignTimeDbContextFactory<RepositoryDbContext>
{
     public RepositoryDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Learning DotNet")))
                    .AddJsonFile("appsettings.json")
                    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<RepositoryDbContext>();

            var connectionString = configuration
                        .GetConnectionString("Database");

            optionsBuilder.UseNpgsql(connectionString);

            return new RepositoryDbContext(optionsBuilder.Options);
        }
}
