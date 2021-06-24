using IdentityServer4.Infrastructure;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.StsServer.Setup
{
    internal class ConfigureMicrosoftOptions : IPostConfigureOptions<MicrosoftAccountOptions>
    {
        private string[] _schemes;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConfigureMicrosoftOptions(string[] schemes, IHttpContextAccessor httpContextAccessor)
        {
            _schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void PostConfigure(string name, MicrosoftAccountOptions options)
        {
            if (_schemes.Length == 0 || _schemes.Contains(name))
            {
                options.StateDataFormat = new DistributedCacheStateDataFormatter(_httpContextAccessor, name);
            }
        }
    }
}
