using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;
using WebStore.Services.Products;
using WebStore.Services.Products.InCookies;
using WebStore.Services.Products.InSQL;
using WebStore.Logger;

namespace WebStore.ServiceHosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            //services.AddScoped<IProductData, SqlProductData>();
            //services.AddScoped<IEmployeesData, SqlEmployeesData>();
            //services.AddScoped<ICartService, CookiesCartService>();
            //services.AddScoped<IOrderService, SqlOrderService>();

            services.AddWebStoreServices();

            // вспомогательный сервис для сервиса корзины
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.API", Version = "v1"});

                const string web_domain_xml = "WebStore.Domain.xml";
                const string web_api_xml = "WebStore.ServiceHosting.xml";
                const string debug_path = "bin/debug/netcoreapp3.1";

                opt.IncludeXmlComments(web_api_xml);

                if (File.Exists(web_domain_xml))
                    opt.IncludeXmlComments(web_domain_xml);
                else if (File.Exists(Path.Combine(debug_path, web_domain_xml)))
                    opt.IncludeXmlComments(Path.Combine(debug_path, web_domain_xml));
            }    
            );

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDBInitializer db, ILoggerFactory log)
        {
            log.AddLog4Net();

            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.API");
                opt.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
