using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using DCx.Defines;
using DCx.StsServer.Config;

namespace DCx.StsServer.Services
{
    public class SmsService : ISmsService
    {
        #region vars
        private SmsConfig Cfg    { get; init; }
        #endregion

        #region ctor
        public SmsService(SmsConfig cfg)
        {
            this.Cfg = cfg;
        }
        #endregion

        #region func - SendMessage
        public bool SendMessage(string smsNumber, string smsMessage)
        {
            var result = false;

            if (this.Cfg.IsValid)
            {
                try
                {
                    var  httpClient = new HttpClient();
                         httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(WebConstants.cJSONDATA));

                    var msgJson  = $"{{ UserName: \"{this.Cfg.ApiUsr}\", Password: \"{this.Cfg.ApiPwd}\", Originator: \"doublecount\", Recipients: [\"{smsNumber}\"], MessageText: \"{smsMessage}\", ForceGSM7bit: true }}";
                    
                    var response = httpClient.PostAsync(this.Cfg.EndPoint, new StringContent(msgJson));

                    result = response.Result.IsSuccessStatusCode;
                }
                catch {}

            }

            return result;
        }
        #endregion
    }
}
