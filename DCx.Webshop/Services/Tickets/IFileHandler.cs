using BlazorInputFile;
using DCx.Webshop.Models;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Services.Tickets
{
    public interface IFileHandler
    {
        public Task<Attachment> UploadFile(IFileListEntry file, License license, int ticketNr);
        public Task DownloadFile(Ticket ticket, Attachment attachment, IJSRuntime jsRuntime);
    }
}
