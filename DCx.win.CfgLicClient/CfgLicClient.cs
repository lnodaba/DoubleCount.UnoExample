using DCx.Defines;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DCx.CfgLicClient
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ICfgLicClient
    {
        Task<List<string>> GetUnitLicAsync(string unitNo, string regNo, int licYear);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class LicenseClient : ICfgLicClient
    {
        private const string cApiGetUnitLic = "api/License/GetUnitLicAsync";

        private string DomainUrl            { get; set; }
        private string StsClient            { get; set; }
        private string StsSecret            { get; set; }

        private string StsTokenEndpoint     => $"https://sts.{DomainUrl}/connect/token";
        private string CfgServerEndpoint    => $"https://cfg.{DomainUrl}/";

        public LicenseClient(string domainUrl, string stsClient, string stsSecret)
        {
            this.DomainUrl  = domainUrl;
            this.StsClient  = stsClient;
            this.StsSecret  = stsSecret;
        }

        public async Task<List<string>> GetUnitLicAsync(string unitNo, string regNo, int licYear)
        {
            TokenResponse token = await this.GetToken();

            var client = new HttpClient();
                client.BaseAddress = new Uri(CfgServerEndpoint);
                client.SetBearerHdr(token.AccessToken);

            var apiResult = string.Empty;
            var apiParam  = $"{unitNo};{regNo};{licYear}".ToUTF8();

            await client.CallApiAsync ( cApiGetUnitLic, 
                                        apiParam,
                                        onSuccess: success => apiResult = success.FromUTF8(),
                                        onError:   error   => throw new Exception(error));

            return apiResult.Split('|').ToList();
        }

        private async Task<TokenResponse> GetToken()
        {
            return await new HttpClient().RequestTokenAsync(new TokenRequest()
            {
                Address         = this.StsTokenEndpoint,
                GrantType       = OidcConstants.GrantTypes.ClientCredentials,
                ClientId        = this.StsClient,
                ClientSecret    = this.StsSecret,
                Parameters      =
                    {
                        { "scope", "DCxLicScope DCxCfgScope"}
                    }
            });
        }
    }
}
