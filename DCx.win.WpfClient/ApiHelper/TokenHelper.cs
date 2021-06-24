using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DCx.Defines;
using DCx.WpfClient;
using DCx.WpfClient.ApiHelper;
using IdentityModel;
using IdentityModel.Client;
using System.Runtime.Caching;

public static class TokenHelper
{
    #region GetMsgRequest

    /// <summary>
    /// composes the RequestMessage
    /// </summary>
    /// <param name="uriAddress"></param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="apiScope"></param>
    /// <returns></returns>

    public static HttpRequestMessage GetMsgRequest(string grantType, string uriAddress, string[] credentials)
    {
        var clientId        = credentials[0]; 
        var clientSecret    = credentials[1]; 
        var apiScope        = credentials[2];

        var requestParams  = new List<KeyValuePair<string, string>>();
            requestParams.Add(new KeyValuePair<string, string>(OidcConstants.TokenRequest.GrantType,      grantType));

        if (grantType == OidcConstants.GrantTypes.ClientCredentials)
        {
            requestParams.Add(new KeyValuePair<string, string>(OidcConstants.TokenRequest.ClientId,       clientId));
            requestParams.Add(new KeyValuePair<string, string>(OidcConstants.TokenRequest.ClientSecret,   clientSecret));
            requestParams.Add(new KeyValuePair<string, string>(OidcConstants.TokenRequest.Scope,          apiScope));
        }

        var requestMessage              = new HttpRequestMessage();
            requestMessage.RequestUri   = new Uri(uriAddress);
            requestMessage.Method       = HttpMethod.Post;
            requestMessage.Content      = new FormUrlEncodedContent(requestParams);

        return requestMessage;
    }
    #endregion

    /// <summary>
    /// With the CODE received from the STS Login it initiates a post call to the Token Endpoint for the access token.
    /// </summary>
    /// <returns></returns>
    public static async Task<string> GetAccessTokenWithAuthCode(string code)
    {
        var response = await new HttpClient().RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
        {
            Address = AppConstants.cURL_STS_GETTOKEN,
            ClientId = AppConstants.cSTS_CLIENT_ID,
            Code = code,
            CodeVerifier = AuthHelper.CodeVerifier,
            RedirectUri = AppConstants.cURL_STS_CALLBACK
        });

        if (response.IsError)
            throw new Exception(response.ErrorDescription);

        cacheToken(response);

        return response.AccessToken;
    }

    

    public static async Task<string> TryGetAccessTokenFromCache()
    {
        ObjectCache cache = MemoryCache.Default;

        var cacheKey = AppConstants.cSTS_CLIENT_ID;
        if (!cache.Contains(cacheKey))
            return string.Empty;

        var token  = (TokenResponse)cache.Get(cacheKey);

        if (DateTime.Now >= token.ExpiresAt)
            return await refreshToken();

        return token.AccessToken;
    }

    private async static Task<string> refreshToken()
    {
        ObjectCache cache = MemoryCache.Default;

        var cacheKey = AppConstants.cSTS_CLIENT_ID;
        var token = (TokenResponse)cache.Get(cacheKey);

        var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = AppConstants.cURL_STS_GETTOKEN,
            ClientId = AppConstants.cSTS_CLIENT_ID,
            RefreshToken = token.RefreshToken,
            Scope = "openid profile DCxCfgScope aud offline_access" //TODO: Later on get this from somewher else instead of hard coded
        });

        cacheToken(response);

        return response.AccessToken;
    }

    private static void cacheToken(TokenResponse response)
    {
        var cacheKey = AppConstants.cSTS_CLIENT_ID;

        ObjectCache cache = MemoryCache.Default;
        if (cache.Contains(cacheKey))
            cache.Remove(cacheKey);

        response.ExpiresAt = DateTime.Now.AddSeconds(response.ExpiresIn); 

        CacheItemPolicy cacheItemPolicy = new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTime.Now.AddDays(30)
        };

        cache.Add(cacheKey, response, cacheItemPolicy);
    }

}
