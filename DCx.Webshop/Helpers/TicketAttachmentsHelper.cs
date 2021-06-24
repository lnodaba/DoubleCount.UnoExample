using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Helpers
{
    public class TicketAttachmentsHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TicketAttachmentsHelper(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetTicketAttachment(string ticketNr, string id)
        {
            string rootPath = _hostingEnvironment.ContentRootPath;
            string path = rootPath + $"\\TicketList\\Ticket_#{ticketNr}\\{id}";

            return path;
        }
    }
}
