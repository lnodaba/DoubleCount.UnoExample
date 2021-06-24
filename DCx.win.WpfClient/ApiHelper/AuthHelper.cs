using DCx.Defines;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DCx.WpfClient.ApiHelper
{
    public static class AuthHelper
    {
        public static string LoginUrlParam
        {
            get => HttpUtility.UrlEncode(getReturnUrl());
        }

        private static string getReturnUrl() => $"/connect/authorize/callback?client_id={AppConstants.cSTS_CLIENT_ID}" +
                $"&redirect_uri={AppConstants.cURL_STS_CALLBACK}" +
                $"&response_type=code" +
                $"&scope=openid profile DCxCfgScope aud offline_access" + // this will come from the AppConstants
                $"&state=eb8a46fe91ce46589e40d993755447a7" + // STS echoes back this value, Cristoph said we might use that for logging
                $"&code_challenge={CodeChallenge}" +
                $"&code_challenge_method=S256" +
                $"&response_mode=query";

        public static string CodeVerifier { get; private set; }

        private static string CodeChallenge
        {
            get
            {
                CodeVerifier = DCx.Identity.Model.CryptoRandom.CreateUniqueId(32);

                using (var sha256 = SHA256.Create())
                {
                    var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(CodeVerifier));
                    return DCx.Identity.Model.Base64Url.Encode(challengeBytes);
                }
            }
        }


    }
}
