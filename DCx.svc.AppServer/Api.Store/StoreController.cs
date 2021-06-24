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
    public class StoreController : ControllerBase
    {
        private readonly IStoreService  m_storeService = null;

        public StoreController(IStoreService storeService) 
        { 
            this.m_storeService = storeService;
        }

        #if DCxSECURITY
        [Authorize]
        #else
        [AllowAnonymous]
        #endif
        [HttpPost] 
        [Route("api/data/GetStoresAsync")] 
        //[Produces(WebConstants.cOCTETSTREAM)]
        public byte[] GetStoresAsync([FromBody] byte[] rawData) 
        {
            return null;
            //var msgBagStore =  this.m_storeService.GetStoresList(rawData);
            //return new WebBagBinary(HttpStatusCode.OK, msgBagStore.GetBinary(), null).GetBytes();
        }
    }
}
