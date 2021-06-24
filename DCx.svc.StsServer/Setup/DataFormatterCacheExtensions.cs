using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.StsServer.Setup
{
    internal static class DataFormatterCacheExtensions
    {
        public static IServiceCollection AddMicrosoftStateDataFormatterCache(this IServiceCollection services, params string[] schemes)
        {
            services.AddSingleton<IPostConfigureOptions<MicrosoftAccountOptions>>(
                svcs => new ConfigureMicrosoftOptions(
                    schemes,
                    svcs.GetRequiredService<IHttpContextAccessor>())
            );
            return services;
        }
    }
}
