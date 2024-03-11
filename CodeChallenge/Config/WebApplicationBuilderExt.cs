using CodeChallenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Config
{
    public static class WebApplicationBuilderExt
    {
        private static readonly string DB_NAME = "EmployeeDB";
        public static void UseEmployeeDB(this WebApplicationBuilder builder)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase("EmployeeDB");

            var context = new EmployeeContext(optionsBuilder.Options);

            new EmployeeDataSeeder(context).Seed().Wait();

            builder.Services.AddDbContext<EmployeeContext>(_ =>
            {
                var options = new DbContextOptionsBuilder<EmployeeContext>()
                    .UseInMemoryDatabase("EmployeeDB")
                    .Options;
                var context = new EmployeeContext(options);
                new EmployeeDataSeeder(context).Seed().Wait();
                return context;

            });
        }
    }
}
