using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OAuth;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4.Services;
using IdentityServer4.Infrastructure;

using DCx.StsServer.CsvIdentityStore;
using DCx.StsServer.Setup;
using DCx.StsServer.Services;

namespace DCx.StsServer
{
    public class Startup
    {
        #region vars
        public IConfiguration Configuration { get; init; }
        #endregion

        #region ctor

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        #endregion

        #region void - ConfigureServices
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddTransient<IProfileService, IdentityWithAdditionalClaimsProfileService>();
            services.AddSingleton<ISmsService>(new SmsService(Setup.Resources.GetSmsConfig()));

            services.AddMicrosoftStateDataFormatterCache("microsoft");

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
              builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.Csp.Level = 0;
                })
                .AddInMemoryClients(Setup.Resources.GetAuthClients())
                .AddInMemoryIdentityResources(Setup.Resources.GetIdentityResources())
                .AddInMemoryApiResources(Setup.Resources.GetApiResources())
                .AddInMemoryApiScopes(Setup.Resources.GetApiScopes())
                .AddCustomUserStore()
                .AddProfileService<IdentityWithAdditionalClaimsProfileService>()
                .AddDeveloperSigningCredential();

            services.AddAuthentication().AddMicrosoftAccount("microsoft", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = Setup.Resources.GetIniValue("MSLogin", "ClientID");
                options.ClientSecret = Setup.Resources.GetIniValue("MSLogin", "ClientSecret");
            });


            services.AddHttpContextAccessor();
        }

        #endregion

        #region void - Configure

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
        #endregion
    }
}
