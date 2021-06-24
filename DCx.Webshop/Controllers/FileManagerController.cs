using DCx.Webshop.Helpers;
using DCx.Webshop.Services.Tickets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Controllers
{
    public class FileManagerController : Controller
    {
        private readonly TicketAttachmentsHelper _ticketsHelper;
        public FileManagerController(TicketAttachmentsHelper ticketsHelper)
        {
            _ticketsHelper = ticketsHelper;
        }

        [HttpGet]
        public async Task<IActionResult> OpenFile(string ticketNr, string filename, string id)
        {
            string path = _ticketsHelper.GetTicketAttachment(ticketNr, id);

            byte[] byteArray = System.IO.File.ReadAllBytes(path);
            MemoryStream ms = new MemoryStream(byteArray);

            string contentType = FileHelper.GetFileContentType(filename);

            return new FileStreamResult(ms, contentType);
        }
    }
}
