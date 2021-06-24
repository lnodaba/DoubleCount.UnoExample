using DCx.WpfClient.ApiHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCx.Defines
{
    public static class AppConstants
    {
        public const    string      cURL_LOCALHOST_NAME     = "http://localhost:";
        public const    string      cURL_LOCALHOST_IP       = "http://127.0.0.1:";

        public const    string      cURL_STS_BASE           = "https://localhost:44341/";
        public static   string      cURL_STS_LOGIN          = $"https://localhost:44341/Account/Login?ReturnUrl={AuthHelper.LoginUrlParam}";
        public const    string      cURL_STS_CALLBACK       = "https://localhost:5009/authentication/login-callback";
        public const    string      cURL_STS_GETTOKEN       = "https://localhost:44341/connect/token";
        public const    string      cBFF_STS_GETTOKEN       = "http://localhost:53043/api/identity/GetAccessToken";

        public const    string      cSTS_CLIENT_ID          = "WPFClient";
        public const    string      cSTS_CLIENT_SECRET      = "secret";
        public const    string      cSTS_API_SCOPE          = "api1";
        public static   string[]    cSTS_CREDENTIALS        = new string[] { cSTS_CLIENT_ID, cSTS_CLIENT_SECRET, cSTS_API_SCOPE};

        public const    string      cURL_CFG_BASE           = "http://localhost:53136/";
        public const    string      cAPI_PROFILE_GET        = "http://localhost:53136/api/profile";
        public const    string      cAPI_PROFILE_GETINFO    = "http://localhost:53136/api/profile/GetInfoAsync";
        public const    string      cAPI_PROFILE_GETCONFIG  = "http://localhost:53136/api/profile/GetConfigAsync";

        public const    string      cAPI_DATA_GETSTORES     = "http://localhost:53043/api/data/GetStoresAsync";

        public const    string      cBFF_PROFILE_GETINFO    = "http://localhost:53043/api/profile/GetInfoAsync";
        public const    string      cBFF_PROFILE_GETCONFIG  = "http://localhost:53043/api/profile/GetConfigAsync";


        public const    string      cURL_WSS_SERVER         = "ws://localhost:9012";

        public const    string      cCOOKIE_NAME            = "AppServerCookie";

    }
}
