using BlazorInputFile;
using DCx.Webshop.Helpers;
using DCx.Webshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Services.Tickets
{
    public class FileHandler : IFileHandler
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Attachment attachment = new Attachment();
        public FileHandler(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task DownloadFile(Ticket ticket, Attachment attachment, IJSRuntime jsRuntime)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            string path = rootPath + $"\\TicketList\\Ticket_#{ticket.TicketNr}\\{attachment.Id}";
            var fileBytes = File.ReadAllBytes(path);

            await jsRuntime.InvokeVoidAsync("DownloadFile", attachment.Filename, Convert.ToBase64String(fileBytes));
        }

        public async Task<Attachment> UploadFile(IFileListEntry file, License license, int ticketNr)
        {
            Guid guid = Guid.NewGuid();
            string fullPath = FileHelper.CreateFolders(ticketNr, guid.ToString());

            MemoryStream memoryStream = new MemoryStream();
            await file.Data.CopyToAsync(memoryStream);

            attachment.Id = guid.ToString();
            attachment.Filename = file.Name;
            attachment.UploadedBy = license.Email;

            try
            {
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fileStream);
                    fileStream.Close();
                    memoryStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return attachment;
        }


    }
}
