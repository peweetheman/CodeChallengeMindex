using System;
using CodeChallenge.Data;
using CodeChallenge.Repositories;
using CodeChallenge.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeChallenge.Config
{
    public class App
    {
        public WebApplication Configure(string[] args)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.UseEmployeeDB();
            
            AddServices(builder.Services);

            var app = builder.Build();

            var env = builder.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedEmployeeDB();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICompensationService, CompensationService>();
            services.AddScoped<IReportingStructureService, ReportingStructureService>();

            services.AddScoped<IEmployeeRepository, EmployeeRespository>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();


            services.AddControllers();
        }

        private void SeedEmployeeDB()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase("EmployeeDB");

            var context = new EmployeeContext(optionsBuilder.Options);

            new EmployeeDataSeeder(context).Seed().Wait();
        }

    }
}
