using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Helpers
{
    public static class FileHelper
    {
        public static string CreateFolders(int ticketNr, string filename)
        {
            string ticketListFolder = Path.Combine(Directory.GetCurrentDirectory(), "TicketList");
            if (!Directory.Exists(ticketListFolder))
            {
                Directory.CreateDirectory(ticketListFolder);
            }

            string ticketNrFolder = Path.Combine(ticketListFolder, "Ticket_#" + ticketNr);
            if (!Directory.Exists(ticketNrFolder))
            {
                Directory.CreateDirectory(ticketNrFolder);
            }

            string fullPath = Path.Combine(ticketNrFolder, filename);
            return fullPath;
        }

        public static string GetFileContentType(string filename)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(filename, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
