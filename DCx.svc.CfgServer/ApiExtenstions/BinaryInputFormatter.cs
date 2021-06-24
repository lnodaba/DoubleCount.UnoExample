using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

using DCx.Defines;

namespace DCx.svc.CfgServer.ApiExtenstions
{
    public class BinaryInputFormatter : InputFormatter
    {
        public BinaryInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(WebConstants.cOCTETSTREAM));
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            int bufLength = (int)context.HttpContext.Request.ContentLength * 2;

            using (MemoryStream memStream = new MemoryStream(bufLength))
            {
                await context.HttpContext.Request.Body.CopyToAsync(memStream);
                return await InputFormatterResult.SuccessAsync(memStream.ToArray());
            }
        }

        protected override bool CanReadType(Type type) => (type == typeof(byte[]));
    }
}