using DCx.CsvStore;
using DCx.svc.CfgServer.ApiExtenstions;
using DCx.svc.CfgServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;

namespace DCx.svc.CfgServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _currentEnvironment = env;
            Configuration = configuration;
        }

        private readonly IWebHostEnvironment _currentEnvironment;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => 
            {
                options.InputFormatters.Insert(0, new BinaryInputFormatter());
                options.OutputFormatters.Insert(0, new BinaryOutputFormatter());

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = Setup.Resources.GetIniValue("oicd", "Authority");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            //get folder name for Csv Store.
            var folder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            if (_currentEnvironment.IsDevelopment())
                folder = folder.Parent.Parent.Parent;
            
            services.AddSingleton<ITableMgr>(sp => new TableMgr($"{folder.FullName}\\CsvFiles"));
            services.AddTransient<LicenseService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(config =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
                config.AllowAnyHeader();
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
