using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using StudentRegistry.Data;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var inMemoryDbName = "TestDb_" + Guid.NewGuid();

        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextOptionsConfiguration<>))
            ).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(inMemoryDbName));
        });
    }
}
