using DCx.Webshop.Models;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCx.Webshop.Helpers
{
    public static class TicketHelper
    {
        public static Ticket TicketModel(EmailMessage message, int ticketNr)
        {
            string createdBy = message.FromAddresses.FirstOrDefault().Name != null ?
                message.FromAddresses.FirstOrDefault().Name : message.FromAddresses.FirstOrDefault().Address;

            Communication communication = new Communication
            {
                CreatedBy = createdBy,
                CreatedDate = message.Date,
                Description = message.Content
            };

            Ticket ticket = new Ticket
            {
                TicketNr = ticketNr,
                CreatedDate = message.Date,
                CreatedBy = createdBy,
                Email = message.FromAddresses.FirstOrDefault().Address,
                Subject = message.Subject,
                LastActionDate = message.Date,
                State = (int)State.Open,
                Communications = new List<Communication>() { communication }
            };

            return ticket;
        }

        public static string AttachmentModel(EmailMessage message, int ticketNr, Ticket ticket)
        {
            Guid guid = Guid.NewGuid();
            string fullPath = FileHelper.CreateFolders(ticketNr, guid.ToString());
            List<string> attachmentLinks = new List<string>();
            foreach (var emailAttachment in message.Attachments)
            {
                string fileName = CreateFileFromAttachment(fullPath, emailAttachment);

                Attachment attachment = new Attachment
                {
                    Id = guid.ToString(),
                    Filename = fileName,
                    UploadedBy = message.FromAddresses.FirstOrDefault().Address
                };

                string link = GetHost() + $"/FileManager/OpenFile?ticketNr={ticket.TicketNr}&filename={attachment.Filename}&id={attachment.Id}";

                ticket.Attachments.Add(attachment);
                attachmentLinks.Add(link);
            }

            string attachmentLink;
            if (attachmentLinks.Count > 1)
            {
                StringBuilder sBuilder = new StringBuilder();
                foreach (var link in attachmentLinks)
                {
                    sBuilder.AppendLine(link);
                }

                attachmentLink = $@"You can view the files here: {sBuilder}";
            }
            else
            {
                attachmentLink = $@"You can view the file here: {attachmentLinks.FirstOrDefault()}";
            }

            return attachmentLink;
        }

        private static string CreateFileFromAttachment(string fullPath, MimeEntity emailAttachment)
        {
            var fileName = emailAttachment.ContentDisposition?.FileName ?? emailAttachment.ContentType.Name;
            FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

            if (emailAttachment is MessagePart)
            {
                var rfc822 = (MessagePart)emailAttachment;

                rfc822.Message.WriteTo(fileStream);
            }
            else
            {
                var part = (MimePart)emailAttachment;
                part.Content.DecodeTo(fileStream);
            }

            fileStream.Close();
            return fileName;
        }

        public static string GetHost()
        {
            return Setup.Resources.GetIniValue("Environment", "Host");
        }
    }
}
