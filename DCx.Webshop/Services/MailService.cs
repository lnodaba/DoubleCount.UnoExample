using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using DCx.Webshop.Config;
using DCx.Webshop.Models;
using DCx.Webshop.Services.Tickets;
using Microsoft.AspNetCore.Hosting;

namespace DCx.Webshop.Services
{
    public class MailService : IMailService
    {
        #region vars
        private MailConfig Cfg { get; init; }
        private SmtpClient SmtpClient { get; set; }
        #endregion

        #region gets

        private string Language => System.Globalization.CultureInfo.CurrentCulture.Name;

        #endregion

        #region ctor
        public MailService(MailConfig cfg)
        {
            this.Cfg = cfg;
        }
        #endregion

        #region void - CreateSmtp

        private void CreateSmtp()
        {
            if (this.Cfg.IsValid)
            {
                this.SmtpClient = new SmtpClient(this.Cfg.Host, this.Cfg.Port)
                {
                    EnableSsl = this.Cfg.UseSSL,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(this.Cfg.Username, this.Cfg.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    DeliveryFormat = SmtpDeliveryFormat.SevenBit
                };

                if (this.Cfg.UseSSL && this.Cfg.Host == "smtp.office365.com")
                {
                    this.SmtpClient.TargetName.Replace("SMTPSVC", "STARTTLS");
                }
            }
            else
            {
                throw new NotSupportedException(this.Cfg.ToString());
            }
        }

        #endregion

        #region //func - SendMsgAsync (Framework)
        //public async Task<bool> SendMsgAsync(string adrTO, string msgSubject, string msgBody)
        //{
        //    if (this.SmtpClient == default)
        //    {
        //        this.CreateSmtp();
        //    }

        //    var mailSender      = new MailAddress(this.Cfg.Username, this.Cfg.NameFrom);

        //    var mailMsg         = new MailMessage();
        //    mailMsg.Subject     = msgSubject;
        //    mailMsg.IsBodyHtml  = false;
        //    mailMsg.Body        = msgBody;
        //    mailMsg.From        = mailSender;
        //    mailMsg.Sender      = mailSender;
        //    mailMsg.ReplyToList.Add(mailSender.Address);
        //    mailMsg.To.Add(adrTO);

        //    if (this.Cfg.AdrCC.IsUsed())    mailMsg.CC.Add(this.Cfg.AdrCC);
        //    if (this.Cfg.AdrBCC.IsUsed())   mailMsg.Bcc.Add(this.Cfg.AdrBCC);

        //    await this.SmtpClient.SendMailAsync(mailMsg);

        //    return true;
        //}
        #endregion

        #region func - SendMsgAsync (MailKit)
        public async Task<bool> SendMsgAsync(string adrTO, string subject, string body)
        {
            var mailSender = MimeKit.MailboxAddress.Parse(this.Cfg.Username);
            var mailReceiver = MimeKit.MailboxAddress.Parse(adrTO);

            var mailMsg = new MimeKit.MimeMessage();
            mailMsg.Subject = subject;
            mailMsg.Sender = mailSender;
            mailMsg.From.Add(mailSender);
            mailMsg.ReplyTo.Add(mailSender);
            mailMsg.To.Add(mailReceiver);

            mailMsg.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Plain) { Text = body };

            if (this.Cfg.AdrCC.IsUsed()) mailMsg.Cc.Add(MimeKit.MailboxAddress.Parse(this.Cfg.AdrCC));
            if (this.Cfg.AdrBCC.IsUsed()) mailMsg.Bcc.Add(MimeKit.MailboxAddress.Parse(this.Cfg.AdrBCC));

            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(this.Cfg.Host, this.Cfg.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(this.Cfg.Username, this.Cfg.Password);
            await smtp.SendAsync(mailMsg);
            smtp.Disconnect(true);

            return true;
        }

        #endregion

    }
}
