using Domain.Repositories.UserRepositories;
using Domain.Service;
using Infrastructure.DataContext;
using Infrastructure.Repositories.UserRepositories;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Helper
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DataDbContext>(o=>
            o.UseSqlServer(configuration.GetConnectionString("SqlCon"), x=>x.MigrationsAssembly("Infrastructure")));
            services.AddHttpContextAccessor();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserReference, UserReference>();
            return services;
        }
    }
}
