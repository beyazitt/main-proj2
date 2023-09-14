//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity;
//using OnionBase.Domain.Entities.Identity;
//using OnionBase.Persistance;
//using OnionBase.Persistance.Contexts;
//using OnionBase.InfraStructure;
//using Microsoft.EntityFrameworkCore;
//using MediatR;
//using Microsoft.Extensions.DependencyInjection;
//using Vonage.Request;
//using OnionBase.Presentation.Helpers;
//using OnionBase.Presentation.Interfaces;
//using Vonage;

//namespace OnionBase.Presentation;

//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);
//        builder.Services.AddHttpClient();
//        // Add services to the container.
//        builder.Services.AddControllersWithViews();
//        builder.Services.AddEndpointsApiExplorer();
//        builder.Services.AddPersistenceServices(builder.Configuration);
//        builder.Services.AddInfraStructureServices();
//        builder.Services.AddScoped<ISmsHelper, SmsHelper>();
//        builder.Services.AddHttpClient();
//        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
//        {
//            options.Cookie.Name = "AuthCookie";
//            options.LoginPath = "/Account/Login";
//            options.AccessDeniedPath = "/Account/Deny";
//        });
//        builder.Services.AddIdentity<AppUser, AppRole>(options => {
//            options.SignIn.RequireConfirmedAccount = false;
//            options.Password.RequireDigit = false;
//            options.Password.RequireUppercase = false;
//            options.Password.RequireLowercase = false;
//            options.User.RequireUniqueEmail = true;
//        })
//            .AddEntityFrameworkStores<UserDbContext>()
//            .AddDefaultTokenProviders();
//        var app = builder.Build();

//        //using (var scope = app.Services.CreateScope())
//        //{

//        //    var services = scope.ServiceProvider;

//        //    System.Threading.Thread.Sleep(5000);
//        //    var context = services.GetRequiredService<UserDbContext>();
//        //    context.Database.Migrate();
//        //}

//        // Configure the HTTP request pipeline.
//        if (!app.Environment.IsDevelopment())
//        {
//            app.UseExceptionHandler("/Home/Error");
//            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//            app.UseHsts();
//        }

//        app.UseHttpsRedirection();
//        app.UseStaticFiles();

//        app.UseRouting();
//        app.UseAuthentication();
//        app.UseAuthorization();

//        app.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Home}/{action=Index}/{id?}");

//        app.Run();
//    }
//}


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using OnionBase.Domain.Entities.Identity;
using OnionBase.Persistance;
using OnionBase.Persistance.Contexts;
using OnionBase.InfraStructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnionBase.Presentation.Helpers;
using OnionBase.Presentation.Interfaces;
using Vonage;

namespace OnionBase.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddInfraStructureServices();
            builder.Services.AddScoped<ISmsHelper, SmsHelper>();
            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "AuthCookie";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Deny";
            });
            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {

                var services = scope.ServiceProvider;

                System.Threading.Thread.Sleep(5000);
                var context = services.GetRequiredService<UserDbContext>();
                context.Database.Migrate();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                await CreateAdminRoleAndAssignUser(roleManager, userManager);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }

        private static async Task CreateAdminRoleAndAssignUser(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            string role = "Admin";
            string adminUserEmail = "admin@admin.com";

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole(role));
            }

            var user = await userManager.FindByEmailAsync(adminUserEmail);

            if (user != null)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                IdentityResult result = await userManager.CreateAsync(new AppUser()
                {
                    Email = "admin@admin.com",
                    Name = "Admin",
                    UserName = "Admin",
                    PhoneNumber = "05050439008",
                }, "AskAdamiIcardi,");
                var user2 = await userManager.FindByEmailAsync(adminUserEmail);
                await userManager.AddToRoleAsync(user2, role);

                // Kullanýcý bulunamadý, burada yeni bir kullanýcý oluþturabilirsiniz.
                // Bu örnekte, böyle bir kullanýcýnýn zaten mevcut olduðunu varsayýyoruz.
            }
        }
    }
}
