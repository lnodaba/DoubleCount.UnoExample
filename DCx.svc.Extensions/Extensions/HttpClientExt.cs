using DCx.Defines;
using DCx.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class HttpClientExt
{
    #region HttpClient.SetBearerHdr

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="accessToken">Bearer AccessToken (Authorization-Header)</param>

    public static HttpClient SetBearerHdr(this HttpClient httpClient, string accessToken)
    {
        var headerVal = accessToken.IsUsed()
                        ? new AuthenticationHeaderValue(WebConstants.cBEARER, accessToken)
                        : null;
        
        httpClient.DefaultRequestHeaders.Authorization = headerVal;

        return httpClient;
    }
    #endregion


    #region HttpClient.CallEmptyAsync

    public static async Task<bool> CallEmptyAsync(this HttpClient httpClient, string apiRoute)
    {
        HttpResponseMessage msgResponse;

        try
        {
            msgResponse = await httpClient.GetAsync(apiRoute).ConfigureAwait(false);
        }
        catch
        {
            return false;
        }

        return msgResponse.IsSuccessStatusCode;
    }
    #endregion

    #region HttpClient.CallMsgAsync

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="uriAddress">full uri of endpoint</param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="apiScope"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    /// <returns></returns>

    public static async Task<WebBagBinary> CallMsgAsync(this HttpClient httpClient, 
                                                        HttpRequestMessage msgRequest, 
                                                        Action<string> onSuccess = null, 
                                                        Action<string> onError   = null)
    {
        HttpResponseMessage msgResponse;

        try
        {
            msgResponse = await httpClient.SendAsync(msgRequest, CancellationToken.None).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex.Message);

            return new WebBagBinary(Http​Status​Code.InternalServerError, null, ex.Message);
        }

        if (msgResponse.IsSuccessStatusCode)
        {
            string msgResult = msgResponse.Content.ReadAsStringAsync().Result;

            onSuccess?.Invoke(msgResult);

            return new WebBagBinary(msgResponse.StatusCode, msgResult.ToUTF8(), null);
        }
        else
        {
            string msgError = msgResponse.Content.ReadAsStringAsync().Result;

            onError?.Invoke(msgError);

            return new WebBagBinary(msgResponse.StatusCode, null, msgError);
        }
    }
    #endregion

    #region HttpClient.CallApiAsync

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="apiRoute">Web-Api Route for Call</param>
    /// <param name="apiParams">params as byte[]</param>
    /// <param name="onSuccess">Action<byte[]) on success</param>
    /// <param name="onError">Acction<string> on error</string></param>
    /// <returns></returns>

    public static async Task<WebBagBinary> CallApiAsync(this HttpClient httpClient, 
                                                        string apiRoute, byte[] apiParams,
                                                        Action<byte[]> onSuccess = null, 
                                                        Action<string> onError   = null)
    {
        using (ByteArrayContent binContent = new ByteArrayContent(apiParams))
        {
            binContent.Headers.ContentType = new MediaTypeHeaderValue(WebConstants.cOCTETSTREAM);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(WebConstants.cOCTETSTREAM));

            HttpResponseMessage msgResponse;
            WebBagBinary        webBagBinary;

            try
            {
                msgResponse = await httpClient.PostAsync(apiRoute, binContent);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);

                return new WebBagBinary(Http​Status​Code.InternalServerError, null, ex.Message);
            }

            webBagBinary = new WebBagBinary(msgResponse.Content.ReadAsByteArrayAsync().Result);

            if (!msgResponse.IsSuccessStatusCode)
            {
                webBagBinary = new WebBagBinary(msgResponse.StatusCode, null, webBagBinary.ErrorMsg);
            }

            if (webBagBinary.IsSuccess)
            {
                onSuccess?.Invoke(webBagBinary.Result);
            }
            else
            {
                onError?.Invoke(webBagBinary.Error);
            }
            return webBagBinary;
        }
    }
    #endregion
}

