using System;
using System.Collections.Generic;
using DCx.IniData;
using IdentityServer4.Models;

namespace DCx.StsServer.Config
{
    public class ClientConfig
    {
        #region const

        public const string cBlazorWasm = "BlazorWasm";
        public const string cWebShop = "WebShop";
        public const string cWPFClient = "WPFClient";
        public const string cLicClient = "CfgLicClient";
        public const string wasmClient = "WasmClient";

        #endregion

        #region vars
        public List<Client> Clients { get; private set; }

        #endregion

        #region ctor
        public ClientConfig(IDxIniSection iniSection)
        {
            this.Clients = new();

            var urlBlazorWasm = iniSection.GetValue(cBlazorWasm);
            var urlWebShop = iniSection.GetValue(cWebShop);
            var urlWPFClient = iniSection.GetValue(cWPFClient);
            var urlWasmClient = iniSection.GetValue(wasmClient);

            var codeScopes = new string[] { "openid", "profile", "DCxCfgScope", "aud" };
            var pwdScopes = new string[] { "DCxLicScope", "DCxCfgScope" };

            this.Clients.Add(CreateCodeClient(cBlazorWasm, urlBlazorWasm, codeScopes, true));
            this.Clients.Add(CreateCodeClient(cWebShop, urlWebShop, codeScopes, true));
            this.Clients.Add(CreateCodeClient(wasmClient, urlWasmClient, codeScopes, false));
            this.Clients.Add(CreateCodeClient(cWPFClient, urlWPFClient, codeScopes, false));
            this.Clients.Add(CreatePwdClient(cLicClient, urlBlazorWasm, pwdScopes));

            #region func - CreateCodeClient

            Client CreateCodeClient(string clientId, string clientUrl, string[] allowedScopes, bool needConsent)
            {
                var client = new Client
                {
                    ClientId = clientId,
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { clientUrl },
                    AllowedScopes = allowedScopes,
                    AllowOfflineAccess = true,
                    RedirectUris = { $"{clientUrl}/signin-oidc", $"{clientUrl}/authentication/login-callback", $"{clientUrl}/authentication-callback" },
                    PostLogoutRedirectUris = { $"{clientUrl}/" },
                    Enabled = true,
                    RequireConsent = needConsent
                };

                return client;
            }
            #endregion

            #region func - CreatePwdClient

            Client CreatePwdClient(string clientId, string clientName, string[] allowedScopes)
            {
                var client = new Client
                {
                    ClientId = clientId,
                    ClientName = clientName,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = allowedScopes,
                    ClientSecrets = new List<Secret>()
                        {
                            new Secret("CfgLicClient".Sha256())
                        },
                };

                return client;
            }
            #endregion
        }
        #endregion
    }
}
