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


namespace DCx.AppServer
{
    public class ProfileController : ControllerBase
    {
        private readonly HttpClient m_httpClient    = null;

        public ProfileController(HttpClient httpClient) 
        { 
            this.m_httpClient   = httpClient;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/profile")] 
        public ActionResult<string> Get()
        {
            return "DoubleCount api/profile";
        }


        [Authorize]
        [HttpPost] 
        [Route("api/profile/GetInfoAsync")] 
        //[Produces(WebConstants.cOCTETSTREAM)]
        public async Task<byte[]> GetInfoAsync([FromBody] byte[] rawData) 
        {
            await Task.CompletedTask;
            return null;
            //Console.WriteLine($"BFF Profile: call");

            //string accessToken = this.HttpContext.Request.Cookies[AppConstants.cCOOKIE_NAME];

            //this.m_httpClient.SetBearerHdr(accessToken);

            //var webBagBinary = await this.m_httpClient.CallApiAsync(AppConstants.cAPI_PROFILE_GETINFO, 
            //                                                     rawData,
            //                                                     (result) => Console.WriteLine($"BFF Profile: success"),
            //                                                     (error)  => Console.WriteLine($"BFF Profile: error"));
            //return webBagBinary.GetBytes();
        }


        //[Authorize]
        //[HttpPut]
        //[Route("api/profile/PutInfoAsync")] 
        //[Produces(WebConstants.cOCTETSTREAM)]
        //public async Task<byte[]> PutInfoAsync([FromBody] byte[] rawData)
        //{
        //    return new ApiResult(Http​Status​Code.BadRequest).GetBytes();
        //}
    }
}
