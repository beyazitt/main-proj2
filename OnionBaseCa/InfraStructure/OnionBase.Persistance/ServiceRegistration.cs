using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionBase.Application.Repositories;
using OnionBase.Application.Services;
using OnionBase.Domain.Entities.Identity;
using OnionBase.Persistance.Contexts;
using OnionBase.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionBase.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<UserDbContext>(options =>
            //    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")));
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer("Server=sql-server-db;Database=OnionUser-Product;User Id=sa;Password=30071998-Bey@z!;"));
            //Console.WriteLine("//////////////////////////");
            //Console.WriteLine(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            //services.AddSingleton<IProductService, ProductService>();
        }
    }
}
