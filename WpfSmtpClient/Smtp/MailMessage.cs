﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCx.Smtp
{
    public class MailMessage
    {
        public MailAddress From { get; set; }
        public List<MailAddress> To { get; set; } = new List<MailAddress>();
        public List<MailAddress> CC { get; set; } = new List<MailAddress>();
        public List<MailAddress> Bcc { get; set; } = new List<MailAddress>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = false;
        public List<MailAttachment> Attachments { get; set; } = new List<MailAttachment>();
    }}
