using Blazored.Modal;
using BlazorInputFile;
using DCx.lib.Webshop.Storage.Services;
using DCx.Webshop.Config;
using DCx.Webshop.Products;
using DCx.Webshop.Services;
using DCx.Webshop.Services.Tickets;
using DCx.Webshop.Helpers;
using DCx.Webshop.Setup;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Collections.Generic;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Dashboard;

namespace DCx.Webshop
{
    public class Startup
    {
        #region vars
        public IConfiguration Configuration { get; init; }
        #endregion

        #region ctor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion


        #region void - ConfigureServices

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory>(new StringLocalizerFactory());

            services.AddRazorPages()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });


            services.AddSingleton<IDatabaseSettings>(sp => new DatabaseSettings()
            {
                CollectionName = Setup.Resources.GetIniValue("Database", "CollectionName"),
                DatabaseName = Setup.Resources.GetIniValue("Database", "DatabaseName"),
                ConnectionString = Setup.Resources.GetIniValue("Database", "ConnectionString")
            });

            var _client = new MongoClient(Setup.Resources.GetIniValue("Database", "ConnectionString"));
            services.AddSingleton<IMongoClient>(_client);

            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("cookies", options =>
            {
                options.Cookie.Name = "bff";
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = Setup.Resources.GetIniValue("oicd", "Authority");
                options.ClientId = Setup.Resources.GetIniValue("oicd", "ClientId");

                options.ResponseType = "code";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapJsonKey("role", "role", "role");

                new List<string>() { "given_name", "company", "first_name", "last_name", "address", "zip", "city" }
                    .ForEach(x => options.ClaimActions.MapUniqueJsonKey(x, x));

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("DCxCfgScope");
                options.Scope.Add("aud");
                options.Scope.Add("offline_access");
                options.SaveTokens = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });

            services.AddSingleton<IProductRepo>(new ProductRepo());

            var mongoConnection = Setup.Resources.GetIniValue("Database", "ConnectionString");
            var mongoUrlBuilder = new MongoUrlBuilder(mongoConnection);
            var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMongoStorage(mongoClient, Setup.Resources.GetIniValue("Database", "DatabaseName"), new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                Prefix = "Hangfire.Mongo",
                CheckConnection = true
            }));

            // Add the processing server as IHostedService
            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = "Hangfire Mongo Server";
            });

            //var provider = services.BuildServiceProvider();
            //var host = provider.GetService<IWebHostEnvironment>();
            services.AddSingleton<IMailService>(new MailService(Setup.Resources.GetMailConfig()));
            services.AddSingleton<OicdConfig>(Setup.Resources.GetOicdConfig());

            services.AddSingleton<ITicketService>(s => new TicketService(s.GetService<IDatabaseSettings>()));

            services.AddSingleton<IFileHandler, FileHandler>();
            services.AddSingleton<TicketAttachmentsHelper>();

            services.AddScoped<ITicketConfig>(s => new TicketConfig
            {
                MailConfig = Setup.Resources.GetMailConfig(),
                DatabaseSettings = s.GetService<IDatabaseSettings>(),
                HttpContextAccessor = s.GetService<IHttpContextAccessor>()
            });

            services.AddScoped<IMailReaderService>(s => new MailReaderService(s.GetService<ITicketConfig>()));
            services.AddScoped<IEmailTicketHandler>(s => new EmailTicketHandler(s.GetService<ITicketConfig>()));
            services.AddSingleton<PredefinedMessagesHandler>();
            services.AddBlazoredModal();
        }
        #endregion

        #region void - Configure

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] { "en-US", "de" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new AuthorizationFilter() }
                });
            });

            RecurringJob.AddOrUpdate<IMailReaderService>((x) => x.ProcessEmails(), "*/5 * * * *"); //every 5 minutes
        }
        #endregion
    }

    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return httpContext.User.Identity.IsAuthenticated;
        }
    }
}
