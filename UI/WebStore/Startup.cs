﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Services.Products.InCookies;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using AutoMapper;
using WebStore.Services.Mapping;
using WebStore.Interfaces.TestAPI;
using WebStore.Clients.Values;
using WebStore.Clients.Employees;
using WebStore.Clients.Products;
using WebStore.Clients.Orders;
using WebStore.Clients.Identity;
using Microsoft.Extensions.Logging;
using WebStore.Logger;
using WebStore.Middleware;
using WebStore.Hubs;

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
            services.AddSignalR();

            services.AddAutoMapper(cfg => 
                                   {
                                       cfg.AddProfile<ViewModelsMapping>(); 
                                   }, 
                                   typeof(Startup));

            services.AddIdentity<User, Role>().
                //AddEntityFrameworkStores<WebStoreDB>().
                AddDefaultTokenProviders();

            #region WebApi Identity clients -- хранение данных вместо базы данных
            services
                .AddTransient<IUserStore<User>, UsersClient>()
                .AddTransient<IUserPasswordStore<User>, UsersClient>()
                .AddTransient<IUserEmailStore<User>, UsersClient>()
                .AddTransient<IUserPhoneNumberStore<User>, UsersClient>()
                .AddTransient<IUserTwoFactorStore<User>, UsersClient>()
                .AddTransient<IUserClaimStore<User>, UsersClient>()
                .AddTransient<IUserLoginStore<User>, UsersClient>();

            services
                .AddTransient<IRoleStore<Role>, RolesClient>();
            #endregion

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

            services.AddScoped<IEmployeesData, EmployeesClient>();
            services.AddScoped<IProductData, ProductsClient>();
            services.AddScoped<IOrderService, OrdersClient>();
            services.AddScoped<ICartService, CookiesCartService>();
            

            services.AddScoped<IValueService, ValuesClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<InformationHub>("/info");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
