using DCx.lib.Webshop.Storage.Services;
using DCx.Webshop.Config;
using DCx.Webshop.Helpers;
using DCx.Webshop.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DCx.Webshop.Services.Tickets
{
    public class EmailTicketHandler : IEmailTicketHandler
    {
        private readonly ITicketService _ticketService;
        private readonly IMailService _mailService;

        public EmailTicketHandler(ITicketConfig ticketConfig)
        {
            _ticketService = new TicketService(ticketConfig.DatabaseSettings);
            _mailService = new MailService(ticketConfig.MailConfig);
        }

        public void AddCommentToTicketFromEmail(EmailMessage message)
        {
            Regex regex = new Regex(@"(\d{1,})");
            int ticketNr = int.Parse(regex.Matches(message.Subject).FirstOrDefault().Value);

            Ticket currentTicket = _ticketService.GetTicketByTicketNr(ticketNr);

            if (currentTicket != null)
            {
                Communication newCommunication = new Communication
                {
                    CreatedBy = message.FromAddresses.FirstOrDefault().Name != null ?
                    message.FromAddresses.FirstOrDefault().Name : message.FromAddresses.FirstOrDefault().Address,
                    CreatedDate = message.Date,
                    Description = message.Content
                };

                currentTicket.LastActionDate = message.Date;
                currentTicket.Communications.Add(newCommunication);

                if (message.Attachments.Count > 0)
                {
                    TicketHelper.AttachmentModel(message, ticketNr, currentTicket);
                }

                _ticketService.UpdateTicket(currentTicket);
            }
        }

        public async Task CreateTicketFromEmail(EmailMessage message)
        {
            int ticketNr = _ticketService.GetLastTicketNumber() + 1;

            Ticket ticket = TicketHelper.TicketModel(message, ticketNr);

            string attachmentLink = string.Empty;
            if (message.Attachments.Count > 0)
            {
                attachmentLink = TicketHelper.AttachmentModel(message, ticketNr, ticket);
            }

            string ticketLink = TicketHelper.GetHost() + $"/Tickets/{ticketNr}";
            MailLinks mailLinks = new MailLinks
            {
                TicketLink = ticketLink,
                AttachmentLink = attachmentLink
            };

            _ticketService.SaveTicket(ticket);

            var mailBody = MailTemplateService.GetTicketCreatedFromEmail(ticket, mailLinks);
            await _mailService.SendMsgAsync(ticket.Email, $"[Ticket #{ticketNr}] - Your ticket was created", mailBody);
        }
    }
}
