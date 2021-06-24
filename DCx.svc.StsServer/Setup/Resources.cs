using DCx.IniData;
using DCx.StsServer.Config;
using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.StsServer.Setup
{
    public class Resources
    {
        #region (vars)
        private static IDxIniData   sIniData { get; set; }
        #endregion

        #region (ctor)
        public static void Init(string iniFile)
        {
            sIniData = DxIniFile.GetIniData(iniFile, false);
        }
        #endregion

        #region (func) - GetIniValue
        public static string GetIniValue(string section, string param) 
            => sIniData.IniSections.GetValue(section).GetValue(param);
        #endregion

        #region (func) - GetSmsConfig
        public static SmsConfig GetSmsConfig() 
            => new SmsConfig(sIniData.IniSections.GetValue("SmsCode"));
        #endregion

        #region (func) - GetAuthClients
        public static IEnumerable<Client> GetAuthClients()
            => new ClientConfig (sIniData.IniSections.GetValue("AuthClients")).Clients;
        #endregion

        #region (func) - GetIdentityResources
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "aud",
                    UserClaims = new List<string> {"aud"}
                }
            };
        }
        #endregion

        #region (func) - GetApiResources
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
              //new ApiResource("DCxCfgScope", "The Weather API", new[] { JwtClaimTypes.Audience }),
                new ApiResource
                {
                    Name = "customAPI",
                    DisplayName = "API #1",
                    Description = "Allow the application to access API #1 on your behalf",
                    Scopes = new List<string> {"api1.read", "api1.write"},
                    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                    UserClaims = new List<string> {"role"}
                }
            };
        }
        #endregion

        #region (func) - GetApiScopes
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("api1.read", "Read Access to API #1"),
                new ApiScope("api1.write", "Write Access to API #1"),
                new ApiScope("DCxCfgScope", "The read write scope for the api"),
                new ApiScope("DCxLicScope", "The read scope for the Licenses")
            };
        }
        #endregion
   }
}
