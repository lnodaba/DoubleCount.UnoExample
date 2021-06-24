using DCx.lib.Webshop.Storage.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Config
{
    public class TicketConfig : ITicketConfig
    {
        public MailConfig MailConfig { get; set; }
        public IDatabaseSettings DatabaseSettings { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
    }

    public interface ITicketConfig
    {
        MailConfig MailConfig { get; set; }
        IDatabaseSettings DatabaseSettings { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}
