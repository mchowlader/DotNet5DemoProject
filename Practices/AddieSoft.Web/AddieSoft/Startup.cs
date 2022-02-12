using AddieSoft.Data;
using AddieSoft.Foundation;
using AddieSoft.Foundation.Utilities;
using AddieSoft.Membership;
using AddieSoft.Membership.Contexts;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AddieSoft
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            WebHostEnvironment = env;

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var connectionInfo = GetConnectionStringAndAssemblyName();

            builder.RegisterModule(new MembershipModule(connectionInfo.connectionString,
                connectionInfo.migrationAssemblyName))
                .RegisterModule(new FoundationModule(connectionInfo.connectionString,
                connectionInfo.migrationAssemblyName))
                .RegisterModule(new WebModule());
        }

        private (string connectionString, string migrationAssemblyName) GetConnectionStringAndAssemblyName()
        {
            var connectionStringName = "DefaultConnection";
            var connectionString = Configuration.GetConnectionString(connectionStringName);
            var migrationAssemblyName = typeof(Startup).Assembly.FullName;
            return (connectionString, migrationAssemblyName);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionInfo = GetConnectionStringAndAssemblyName();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
           
            services.AddDbContext<MembershipDbContext>(options =>
                    options.UseSqlServer(connectionInfo.connectionString, b =>
                    b.MigrationsAssembly(connectionInfo.migrationAssemblyName)));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();



            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Account/Signin";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<PathSettings>(Configuration.GetSection("Paths"));
            services.Configure<DefaultImageSettings>(Configuration.GetSection("DefaultImageSettings"));

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{Id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{Id?}");

                endpoints.MapRazorPages();

            });

        }
    }
}