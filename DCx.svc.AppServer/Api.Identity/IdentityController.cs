using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using DCx.Defines;
//using DCx.Messaging;

namespace DCx.AppServer
{
    public class IdentityController : ControllerBase
    {
        private readonly HttpClient             m_httpClient            = null;
        //private readonly IHttpContextAccessor   m_httpContextAccessor   = null;

        public IdentityController(HttpClient httpClient/*, IHttpContextAccessor httpContextAccessor*/) 
        { 
            this.m_httpClient           = httpClient;
            //this.m_httpContextAccessor  = httpContextAccessor;  
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/profile")] 
        public ActionResult<string> Get()
        {
            return "DCx api/identity";
        }


        [AllowAnonymous]
        [HttpPost] 
        [Route("api/identity/GetAccessToken")] 
        //[Produces(WebConstants.cOCTETSTREAM)]
        public async Task<byte[]> GetAccessToken([FromBody] byte[] rawData) 
        {
            await Task.CompletedTask;
            return null;
            //Console.WriteLine($"BFF Token: call");

            //string[] loginArgs   = rawData.FromUTF8()?.Split(";");

            //if (loginArgs?.Length == 3)
            //{
            //    var msgRequest  = TokenHelper.GetMsgRequest(OidcGrantTypes.ClientCredentials,
            //                                                AppConstants.cURL_STS_GETTOKEN,
            //                                                loginArgs);

            //    var webBagBinary = await this.m_httpClient.CallMsgAsync(msgRequest,
            //                                                            (result) => Console.WriteLine($"BFF Token: success"),
            //                                                            (error)  => Console.WriteLine($"BFF Token: error"));

            //    if (webBagBinary.IsSuccess)
            //    {
            //        Response.Cookies.Append(AppConstants.cCOOKIE_NAME, 
            //                                TokenHelper.GetAccessToken(webBagBinary.Result.FromUTF8()), 
            //                                new CookieOptions());

            //        Console.WriteLine("BFF Token: added cookie");

            //    }
            //    return webBagBinary.GetBytes();
            //}
            //else
            //{
            //    Console.WriteLine("BFF Token: incorrect params");
            //    return new WebBagBinary(Http​Status​Code.BadRequest).GetBytes();
            //}
        }
    }
}
