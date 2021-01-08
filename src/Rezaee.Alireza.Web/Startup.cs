using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rezaee.Alireza.Web.Areas.Identity.Data;
using Rezaee.Alireza.Web.Services.Email;
using Rezaee.Alireza.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Rezaee.Alireza.Web.Areas.Identity.Helpers;
using Rezaee.Alireza.Web.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Rewrite;
using Rezaee.Alireza.Web.RouteConstraint;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Diagnostics;

namespace Rezaee.Alireza.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AlirezaRezaee.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            //KissLog
            //services.AddScoped<ILogger>((context) =>
            //{
            //    return Logger.Factory.Get();
            //});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("BlogCS")));

            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();

            var microsoftAuth = Configuration.GetSection("Authentication").GetSection("Microsoft");
            var gitHubAuth = Configuration.GetSection("Authentication").GetSection("GitHub");
            var stackExchangeAuth = Configuration.GetSection("Authentication").GetSection("StackExchange");
            services.AddAuthentication()
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = microsoftAuth.GetSection("ClientId").Value;
                    microsoftOptions.ClientSecret = microsoftAuth.GetSection("ClientSecret").Value;
                    microsoftOptions.CallbackPath = "/signin-microsoft";
                })
                .AddGitHub(gitHubOptions =>
                {
                    gitHubOptions.ClientId = gitHubAuth.GetSection("ClientId").Value;
                    gitHubOptions.ClientSecret = gitHubAuth.GetSection("ClientSecret").Value;
                    gitHubOptions.CallbackPath = "/signin-github";
                })
                .AddStackExchange(stackExchangeOptions =>
                {
                    stackExchangeOptions.ClientId = stackExchangeAuth.GetSection("ClientId").Value;
                    stackExchangeOptions.ClientSecret = stackExchangeAuth.GetSection("ClientSecret").Value;
                    stackExchangeOptions.CallbackPath = "/signin-stackexchange";
                    stackExchangeOptions.RequestKey = stackExchangeAuth.GetSection("Key").Value;
                });

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("pagePath", typeof(PageConstraint));
            });

            services.AddRazorPages();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<ISiteEmailSender, SiteEmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //    name: "EditArticle",
                //    pattern: "edit/article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
