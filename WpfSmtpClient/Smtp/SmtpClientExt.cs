
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class SmtpClientExt
{
    #region SmtpClient.Create

    public static SmtpClient Create(string host, int port, bool useSSL, string user, string pwd)
    {
        var smtpClient                          = new SmtpClient(host, port);
            smtpClient.UseDefaultCredentials    = false;
            smtpClient.Credentials              = new NetworkCredential(user, pwd);
            smtpClient.EnableSsl                = useSSL;

        if (useSSL && host == "smtp.office365.com")
        {
            smtpClient.TargetName = $"STARTTLS/{host}";
        }

        return smtpClient;
    }
    #endregion

    #region func @ SendMessage
    public static async Task<bool> SendMessage(this SmtpClient smtpClient, string userName, string nameFrom, string adrTO, string adrCC, string adrBCC, string msgSubject, string msgBody)
    {
        var mailSender = new MailAddress(userName, nameFrom);

        var mailMsg             = new MailMessage();
            mailMsg.Subject     = msgSubject;
            mailMsg.IsBodyHtml  = false;
            mailMsg.Body        = msgBody;
            mailMsg.From        = mailSender;
            mailMsg.Sender      = mailSender;
            mailMsg.ReplyToList .Add(mailSender.Address);
            mailMsg.To          .Add(adrTO);

            if (!String.IsNullOrEmpty(adrCC))   mailMsg.CC .Add(adrCC);
            if (!String.IsNullOrEmpty(adrBCC))  mailMsg.Bcc.Add(adrBCC);

		await smtpClient.SendMailAsync(mailMsg);

        return true; 
    }
    #endregion

}

