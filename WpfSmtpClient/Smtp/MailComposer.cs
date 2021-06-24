using System;
using System.Collections.Generic;
using System.Linq;

namespace DCx.Smtp
{
    public class MailComposer
    {
        private MailAddress         From        { get; set; }
        private List<MailAddress>   To          { get; set; } = new List<MailAddress>();
        private List<MailAddress>   CC          { get; set; } = new List<MailAddress>();
        private List<MailAddress>   Bcc         { get; set; } = new List<MailAddress>();
        private string              Subject     { get; set; }
        private string              Body        { get; set; }
        private bool                IsBodyHtml  { get; set; }
        public List<MailAttachment> Attachments { get; set; } = new List<MailAttachment>();

        public MailComposer SetSender(string address, string name = null)
        {
            From = new MailAddress(address, name);
            return this;
        }

        public MailComposer AddRecipient(string address, string name = null, MailAdrType type = MailAdrType.To)
        {
            MailAddress mailAddress = new MailAddress(address, name);

            switch (type)
            {
                case MailAdrType.To:    this.To .Add(mailAddress);      break;
                case MailAdrType.CC:    this.CC .Add(mailAddress);      break;
                case MailAdrType.Bcc:   this.Bcc.Add(mailAddress);      break;
            }

            return this;
        }

        public MailComposer SetSubject(string subject)
        {
            Subject = subject;
            return this;
        }

        public MailComposer SetBody(string body, bool isBodyHtml = false)
        {
            Body = body;
            IsBodyHtml = isBodyHtml;
            return this;
        }

        public MailComposer AddAttachment(string name, byte[] data)
        {
            Attachments.Add(new MailAttachment(name, data));
            return this;
        }

        public MailMessage Build()
        {
            var mailMessage = new MailMessage();

            if (From == null || String.IsNullOrWhiteSpace(From.Address))
            {
                throw new ArgumentException(nameof(From));
            }
            else
            {
                mailMessage.From = From;
            }

            if (To != null && To.Any())
            {
                mailMessage.To.AddRange(To);
            }

            if (CC != null && CC.Any())
            {
                mailMessage.CC.AddRange(CC);
            }

            if (Bcc != null && Bcc.Any())
            {
                mailMessage.Bcc.AddRange(Bcc);
            }

            if (!String.IsNullOrWhiteSpace(Subject))
            {
                mailMessage.Subject = Subject;
            }

            if (!String.IsNullOrWhiteSpace(Body))
            {
                mailMessage.Body = Body;
                mailMessage.IsBodyHtml = IsBodyHtml;
            }

            if (Attachments != null && Attachments.Any())
            {
                mailMessage.Attachments.AddRange(Attachments);
            }

            return mailMessage;
        }
    }
}