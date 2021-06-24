using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

using DCx.Defines;

namespace DCx.svc.CfgServer.ApiExtenstions
{
    public class BinaryOutputFormatter : OutputFormatter
    {
        public BinaryOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(WebConstants.cOCTETSTREAM));
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            byte[] _rawData = (byte[])context.Object;

            await context.HttpContext.Response.Body.WriteAsync(_rawData, 0, _rawData.Length);        
        }
        
        protected override bool CanWriteType (Type type) => (type == typeof(byte[]));

    }
}