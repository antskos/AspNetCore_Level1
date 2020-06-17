using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Contetxt;
using WebStore.Data;
using WebStore.Infrastructure.Services.InSQL;
using WebStore.Infrastructure.Services.InMemory;
using WebStore.Infrastructure.Interfaces;
using WebStore.Domain.Entities.Employees;
using WebStore.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;

namespace WebStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<WebStoreDBInitializer>();

            services.AddIdentity<User, Role>().
                AddEntityFrameworkStores<WebStoreDB>().
                AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
                {
#if DEBUG
                    // требования к паролю
                    opt.Password.RequiredLength = 3;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredUniqueChars = 3;

                    // требования к пользователю
                    opt.User.RequireUniqueEmail = false;
#endif
                    //создаваемые пользователи не заблокированы
                    opt.Lockout.AllowedForNewUsers = true;
                    // кол-во попыток ввода пароля
                    opt.Lockout.MaxFailedAccessAttempts = 7;
                    // блокировка пользователя при неверном пароле на кол-во минут
                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                });

            services.ConfigureApplicationCookie(opt =>
                {
                    // настройки cookies
                    opt.Cookie.Name = "CookiesForWebStore";
                    opt.Cookie.HttpOnly = true;
                    opt.ExpireTimeSpan = TimeSpan.FromDays(10);
                    
                    // настройки адресов авторизации
                    opt.LoginPath = "/Account/Login";
                    opt.LogoutPath = "/Account/Logout";
                    opt.AccessDeniedPath = "/Account/AccessDenied";

                    // повышение безопасности
                    opt.SlidingExpiration = true;
                });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //варианты регистрации сервисов: их 3 вида
            
           //services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();     // 1-ый вариант

            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();   // 2-ой вариант

            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();      // 3-ий вариант

            //services.AddSingleton<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IEmployeesData, SqlEmployeesData>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDBInitializer db)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
