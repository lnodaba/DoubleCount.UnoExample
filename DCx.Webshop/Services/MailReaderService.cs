using DCx.lib.Webshop.Storage.Services;
using DCx.Webshop.Config;
using DCx.Webshop.Models;
using DCx.Webshop.Services.Tickets;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Search;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DCx.Webshop.Services
{
    public class MailReaderService : IMailReaderService
    {
        private MailConfig _mailConfig { get; init; }
        private readonly EmailTicketHandler ticketHelper;

        public MailReaderService(ITicketConfig ticketConfig)
        {
            _mailConfig = ticketConfig.MailConfig;
            ticketHelper = new EmailTicketHandler(ticketConfig);
        }

        public async Task ProcessEmails()
        {
            try
            {
                var client = new ImapClient();

                client.Connect(_mailConfig.ImapServer, _mailConfig.ImapPort, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_mailConfig.Username, _mailConfig.Password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);

                var uids = inbox.Search(SearchQuery.NotSeen);

                var mainFolder = client.GetFolder(client.PersonalNamespaces[0]);
                IMailFolder processedFolder = mainFolder.GetSubfolder("Processed");
                if (processedFolder == null)
                {
                    processedFolder = mainFolder.Create("Processed", true);
                }

                foreach (var uid in uids)
                {
                    var email = client.Inbox.GetMessage(uid);
                    var fromAddress = email.From.Select(x => (MailboxAddress)x).Select(x => x.Address).FirstOrDefault();

                    if (!Regex.IsMatch(fromAddress, @"microsoft\.com"))
                    {
                        var emailMessage = new EmailMessage
                        {
                            Subject = email.Subject,
                            Date = email.Date.DateTime
                        };

                        if (email.Attachments.Any())
                        {
                            foreach (var attachment in email.Attachments)
                            {
                                emailMessage.Attachments.Add(attachment);
                            }
                        }

                        emailMessage.ToAddresses.AddRange(email.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                        emailMessage.FromAddresses.AddRange(email.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));

                        if (Regex.IsMatch(email.Subject, @"(Re:\s)(\[Ticket\s#\d{1,}\])"))
                        {
                            string content = GetReply(email);
                            emailMessage.Content = content;

                            ticketHelper.AddCommentToTicketFromEmail(emailMessage);
                        }
                        else
                        {
                            emailMessage.Content = email.TextBody.Replace("\r\n", "");
                            await ticketHelper.CreateTicketFromEmail(emailMessage);
                        }

                        inbox.SetFlags(uid, MessageFlags.Seen, true);
                        inbox.MoveTo(uid, processedFolder);
                    }
                    else
                    {
                        inbox.SetFlags(uid, MessageFlags.Seen, true);
                        inbox.MoveTo(uid, processedFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetReply(MimeMessage email)
        {
            var stringToMatch = @"On ";
            var content = email.TextBody.Substring(0, email.TextBody.IndexOf(stringToMatch)).Replace("\r\n", "");
            return content;
        }
    }
}
