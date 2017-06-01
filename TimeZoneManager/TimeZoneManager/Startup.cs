using System;
using System.IO;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;
using TimeZoneManager.AutoMapper;
using TimeZoneManager.Data.Context;
using TimeZoneManager.Data.Initialize;
using TimeZoneManager.Data.Models;
using TimeZoneManager.Services;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Common.Config;
using TimeZoneManager.Services.Interfaces;
using TimeZoneManager.Services.Services;
using TimeZoneManager.Services.Services.Interfaces;

namespace TimeZoneManager
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config/appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"config/appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            env.ConfigureNLog("config/NLog.config");
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(s => Configuration.GetSection("Auth").Get<AuthConfiguration>());
            
            services.AddDbContext<TimeZonesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TimeZoneApp")));

            services.AddIdentity<TimeZoneAppUser, IdentityRole>()
                .AddEntityFrameworkStores<TimeZonesContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication();

            services.AddMvc();

            services.AddAutoMapper(ctx => ctx.AddProfile(typeof(MappingProfile)));

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITimeZoneService, TimeZoneService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admins", policy => policy.RequireClaim(CustomClaimTypes.Permission, "admin"));
                options.AddPolicy("Managers", policy => policy.RequireClaim(CustomClaimTypes.Permission, "manager", "admin"));
                options.AddPolicy("Users", policy => policy.RequireClaim(CustomClaimTypes.Permission, "user", "manager", "admin"));
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            TimeZonesContext usersContext,
            UserManager<TimeZoneAppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<TimeZoneAppUser> signInManager,
            IOptions<IdentityOptions> optionsAccessor,
            AuthConfiguration authConfiguration)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    usersContext.Database.Migrate();
                }

                DbInitializer.Initialize(usersContext, userManager, roleManager).Wait();

                loggerFactory.AddNLog();

                app.AddNLogWeb();

                app.UseStaticFiles();

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"app/dist")),
                    RequestPath = new PathString("/dist")
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"app/static")),
                    RequestPath = new PathString("/static")
                });

                ConfigureAuth(app, signInManager, userManager, roleManager, optionsAccessor, authConfiguration);

                app.UseMvc(routes =>
                {
                    routes.MapRoute("spa", "{*url}", new {controller = "Spa", action = "Index"}); 
                });
            }
            catch (Exception ex)
            {
                app.Run(async context =>
                {
                    var logger = loggerFactory.CreateLogger(GetType());
                    logger?.LogError(new EventId(), ex, ex.Message);

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(ex.Message).ConfigureAwait(false);
                    await context.Response.WriteAsync(ex.StackTrace).ConfigureAwait(false);
                });
            }
        }
    }
}
