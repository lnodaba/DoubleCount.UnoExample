using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//  https://github.com/aleripe/SmtpClient/tree/master/SmtpClient

namespace DCx.Smtp
{
    public class SmtpClient : IDisposable
    {
        public static readonly string COMMAND_EHLO          = "EHLO";
        public static readonly string COMMAND_STARTTLS      = "STARTTLS";
        public static readonly string COMMAND_AUTH_LOGIN    = "AUTH LOGIN";
        public static readonly string COMMAND_MAIL_FROM     = "MAIL FROM";
        public static readonly string COMMAND_RCPT_TO       = "RCPT TO";
        public static readonly string COMMAND_DATA          = "DATA";
        public static readonly string COMMAND_QUIT          = "QUIT";
        public static readonly string HEADER_SUBJECT        = "Subject";
        public static readonly string HEADER_FROM           = "From";
        public static readonly string HEADER_TO             = "To";
        public static readonly string HEADER_CC             = "CC";
        public static readonly string HEADER_SENDER         = "Sender";
        public static readonly string HEADER_MIME_VERSION   = "Mime-Version";
        public static readonly string HEADER_DATE           = "Date";
        public static readonly string HEADER_CONTENT_TYPE   = "Content-Type";
        public static readonly string HEADER_CONTENT_TRANSFER_ENCODING  = "Content-Transfer-Encoding";
        public static readonly string HEADER_CONTENT_DISPOSITION        = "Content-Disposition";

        public  string  SmtpServer              { get; set; } = "localhost";
        public  int     Port                    { get; set; } = 25;
        public  bool    EnableSsl               { get; set; } = false;
        public  NetworkCredential Credentials   { get; set; }

        private Socket client;

        public SmtpClient()
        {
            client = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public SmtpClient(string smtpServer, int port, bool enableSSL) : this()
        {
            SmtpServer  = smtpServer;
            Port        = port;
            EnableSsl   = enableSSL;
        }

        public void SetCredentials(string userName, string password)
        {
            Credentials = new NetworkCredential(userName, password);
        }

        public async Task SendAsync(MailMessage mailMessage)
        {
            await Connect();
            await SendEhloCommand();
            await SendStartTlsCommand();
            await SendAuthLoginCommand();
            await SendMailFromCommand(mailMessage);
            await SendRcptToCommand(mailMessage);
            await SendDataCommand();
            await SendData(mailMessage);
            await SendQuitCommand();
        }

        private async Task Connect()
        {
            try
            {
                await client.ConnectAsync(SmtpServer, Port);
            }
            catch (ArgumentNullException)
            {
                throw new InvalidOperationException("SMTP server address cannot be null or empty.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException("SMTP server port is out of range.");
            }
            catch (SocketException)
            {
                throw new SmtpException("SMTP service is not available at specified address and port.", SmtpStatusCode.ServiceNotAvailable);
            }
            catch (ObjectDisposedException)
            {
                throw new InvalidOperationException("Could not establish connection. Socket has been closed.");
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException("SMTP server address cannot be null or empty.");
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Could not establish connection. Socket is busy.");
            }

            CommandResponse response = GetResult();
            
            if (response.Code != 220)
            {
                throw new SmtpException($"Could not establish connection to SMTP server. {response.Code}: {response.Message}", SmtpStatusCode.ServiceNotAvailable);
            }
        }

        private async Task SendEhloCommand()
        {
            CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($"{COMMAND_EHLO} {Environment.MachineName}{Environment.NewLine}"));

            if (response.Code != 250)
            {
                throw new SmtpException($"{COMMAND_EHLO} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.ServiceNotAvailable);
            }
        }

        private async Task SendStartTlsCommand()
        {
            if (EnableSsl)
            {
                CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($"{COMMAND_STARTTLS}{Environment.NewLine}"));

                if (response.Code != 220)
                {
                    throw new SmtpException($"{COMMAND_EHLO} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.SyntaxError);
                }
            }
        }

        private async Task SendAuthLoginCommand()
        {
            if (Credentials != null)
            {
                string userName = Convert.ToBase64String(Encoding.UTF8.GetBytes(Credentials.UserName));
                string password = Convert.ToBase64String(Encoding.UTF8.GetBytes(Credentials.Password));

                CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($"{COMMAND_AUTH_LOGIN}{Environment.NewLine}{userName}{Environment.NewLine}{password}{Environment.NewLine}"));
                
                if (response.Code != 235)
                {
                    throw new SmtpException($"{COMMAND_AUTH_LOGIN} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.ClientNotPermitted);
                }
            }
        }

        private async Task SendMailFromCommand(MailMessage mailMessage)
        {
            CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($"{COMMAND_MAIL_FROM}: <{mailMessage.From.Address}>{Environment.NewLine}"));

            if (response.Code != 250)
            {
                throw new SmtpException($"{COMMAND_MAIL_FROM} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.SyntaxError);
            }
        }

        private async Task SendRcptToCommand(MailMessage mailMessage)
        {
            foreach (MailAddress mailAddress in mailMessage.To.Union(mailMessage.CC))
            {
                CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($"{COMMAND_RCPT_TO}: <{mailAddress.Address}>{Environment.NewLine}"));

                if (response.Code != 250)
                {
                    throw new SmtpException($"{COMMAND_RCPT_TO} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.SyntaxError);
                }
            }
        }

        private async Task SendDataCommand()
        {
            CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($"{COMMAND_DATA}{Environment.NewLine}"));

            if (response.Code != 354)
            {
                throw new SmtpException($"{COMMAND_DATA} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.SyntaxError);
            }
        }

        private async Task SendQuitCommand()
        {
            CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($"{COMMAND_QUIT}{Environment.NewLine}"));

            if (response.Code != 221)
            {
                throw new SmtpException($"{COMMAND_QUIT} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.SyntaxError);
            }
        }

        private async Task SendData(MailMessage mailMessage)
        {
            string mimeType = mailMessage.IsBodyHtml ? "text/html; charset=utf-8" : "text/plain; charset=utf-8";
                
            await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_SUBJECT}: {mailMessage.Subject}{Environment.NewLine}"));
            await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_FROM}: {mailMessage.From}{Environment.NewLine}"));
            await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_TO}: {string.Join(",", mailMessage.To.Select(mailAddress => mailAddress))}{Environment.NewLine}"));
            await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_CC}: {string.Join(",", mailMessage.CC.Select(mailAddress => mailAddress))}{Environment.NewLine}"));
            await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_SENDER}: {mailMessage.From}{Environment.NewLine}"));
            await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_MIME_VERSION}: 1.0{Environment.NewLine}"));
            await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_DATE}: {DateTime.Now}{Environment.NewLine}"));

            if (mailMessage.Attachments.Count > 0)
            {
                string boundary = Guid.NewGuid().ToString("N");

                await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_CONTENT_TYPE}: multipart/mixed; boundary=\"{boundary}\"{ Environment.NewLine}"));

                await SendCommand(Encoding.UTF8.GetBytes($"{Environment.NewLine}--{boundary}{Environment.NewLine}"));
                await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_CONTENT_TYPE}: {mimeType}{Environment.NewLine}"));
                await SendCommand(Encoding.UTF8.GetBytes($"{Environment.NewLine}{mailMessage.Body}{Environment.NewLine}"));

                foreach (MailAttachment mailAttachment in mailMessage.Attachments)
                {
                    await SendCommand(Encoding.UTF8.GetBytes($"{Environment.NewLine}--{boundary}{Environment.NewLine}"));
                    await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_CONTENT_TYPE}: application/octet-stream; name=\"{mailAttachment.Name}\"{Environment.NewLine}"));
                    await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_CONTENT_TRANSFER_ENCODING}: base64{Environment.NewLine}"));
                    await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_CONTENT_DISPOSITION}: attachment; filename=\"{mailAttachment.Name}\"{Environment.NewLine}"));
                    await SendCommand(Encoding.UTF8.GetBytes($"{Environment.NewLine}{Convert.ToBase64String(mailAttachment.Data)}{Environment.NewLine}"));
                }

                await SendCommand(Encoding.UTF8.GetBytes($"{Environment.NewLine}--{boundary}--{Environment.NewLine}"));
            }
            else
            {
                await SendCommand(Encoding.UTF8.GetBytes($"{HEADER_CONTENT_TYPE}: {mimeType}{Environment.NewLine}"));
                await SendCommand(Encoding.UTF8.GetBytes($"{Environment.NewLine}{mailMessage.Body}{Environment.NewLine}"));
            }

            CommandResponse response = await SendCommandWithResult(Encoding.UTF8.GetBytes($".{Environment.NewLine}"));

            if (response.Code != 250)
            {
                throw new SmtpException($"{COMMAND_QUIT} command resulted in error condition. {response.Code}: {response.Message}", SmtpStatusCode.SyntaxError);
            }
        }

        private async Task SendCommand(byte[] data)
        {
            await Task.Run(() => client.Send(data));
        }

        private async Task<CommandResponse> SendCommandWithResult(byte[] data)
        {
            await SendCommand(data);
            return GetResult();
        }

        private CommandResponse GetResult()
        {
            byte[] result = new byte[1024];

            try
            {
                client.Receive(result);
            }
            catch (SocketException)
            {
                throw new SmtpException("SMTP service is not available at specified address and port.", SmtpStatusCode.ServiceNotAvailable);
            }
            catch (ObjectDisposedException)
            {
                throw new InvalidOperationException("Could not establish connection. Socket has been closed.");
            }

            string textResponse = Encoding.UTF8.GetString(result).TrimEnd('\0').Trim();
            Match match = Regex.Match(textResponse, "([0-9]{3}) (.+)");

            if (!match.Success || match.Groups.Count != 3)
            {
                throw new SmtpException("Could not parse SMTP server response.", SmtpStatusCode.SyntaxError);
            }

            CommandResponse response = new CommandResponse(int.Parse(match.Groups[1].Value), match.Groups[2].Value);
            Debug.WriteLine($"{response.Code}: {response.Message}");
            return response;
        }

        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
    }
}
