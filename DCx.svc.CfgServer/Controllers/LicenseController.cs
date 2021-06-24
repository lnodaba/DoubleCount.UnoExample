using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCx.Defines;
using DCx.svc.CfgServer.ApiExtenstions;
using DCx.svc.CfgServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DCx.svc.CfgServer.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        static readonly string[] scopeRequired = new string[] { "DCxLicScope" };


        public LicenseController(LicenseService licenseService)
        {
            this.licenseService = licenseService;
        }

        private LicenseService licenseService { get; set; }



        [HttpPost]
        [Route("GetUnitLicAsync")]
        [Produces(WebConstants.cOCTETSTREAM)]
        public byte[] GetUnitLicAsync([FromBody] byte[] rawData)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequired);
            
            var apiResult = this.licenseService.GetUnitLic(rawData);
            
            return apiResult.GetBytes();
        }


    }
}
