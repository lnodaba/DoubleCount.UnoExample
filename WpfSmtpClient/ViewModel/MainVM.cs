using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Data;
using System.Net.Http.Headers;
using System.Net.Http;

using DCx.Mvvm;

namespace DCx.WpfSmtpClient
{
    public class MainVM : PropNotify
    {
        #region cmds
        public VMCommand    FxSmtpSendCmd       { get; private set; }
        public VMCommand    CsSmtpSendCmd       { get; private set; }

        #endregion

        #region vars

        private object      m_msgLock           = new object();

        private bool        IsProcessing        { get; set; }

        public  string      Host                { get; set; }   = "smtp.office365.com";
        public  int         Port                { get; set; }   = 587;
        public  string      Username            { get; set; }   = "backoffice@dialogik.ch";
        public  string      Password            { get; set; }
        public  bool        UseSSL              { get; set; }   = true;

        public  string      NameFrom            { get; set; }

        public  string      AdrTO               { get; set; }   = "christoph.braendle@outlook.com";
        public  string      AdrCC               { get; set; }
        public  string      AdrBCC              { get; set; }

        public  string      MsgSubj             { get; set; }   = "Subj Test";
        public  string      MsgBody             { get; set; }   = "Body Test";


        public  ObservableCollection<string>    MsgList { get; }

        #endregion

        #region ctors

        public MainVM()
        {
			this.FxSmtpSendCmd  = new VMCommand(this.RunFxSmtpSendCmd,  this.CanFxSmtpSendCmd);
            this.CsSmtpSendCmd  = new VMCommand(this.RunCsSmtpSendCmd,  this.CanCsSmtpSendCmd);

            this.MsgList        = new ObservableCollection<string>();

            BindingOperations.EnableCollectionSynchronization(this.MsgList, this.m_msgLock);
        }
        #endregion


        #region cmd - FxSmtpSendCmd

        private async void RunFxSmtpSendCmd(object param)
        {
            try
            {
                this.MsgList.Add($"fx connect {this.Host} {this.Port} {this.UseSSL} {this.Username}");

                var smtpClient = SmtpClientExt.Create(this.Host, this.Port, this.UseSSL, this.Username, this.Password);

                this.MsgList.Add($"fx send {this.Username} {this.AdrTO} {this.MsgSubj}");

                await smtpClient.SendMessage(this.Username, this.NameFrom, this.AdrTO, this.AdrCC, this.AdrBCC, this.MsgSubj, this.MsgBody);

                this.MsgList.Add($"fx success");
            }
            catch (Exception ex)
            {
                this.MsgList.Add($"#fx error {ex.Message}");
            }
        }

        private bool CanFxSmtpSendCmd(object param) => !this.IsProcessing;

        #endregion

        #region cmd - CsSmtpSendCmd

        private async void RunCsSmtpSendCmd(object param)
        {
            try
            {
                this.MsgList.Add($"cs connect {this.Host} {this.Port} {this.UseSSL} {this.Username}");

                var message = new DCx.Smtp.MailComposer()
                                .SetSender   (this.Username, this.NameFrom)
                                .AddRecipient(this.AdrTO)
                                .SetSubject  (this.MsgSubj)
                                .SetBody     (this.MsgBody, false).Build();


                this.MsgList.Add($"fx send {this.Username} {this.AdrTO} {this.MsgSubj}");

                using (var client = new DCx.Smtp.SmtpClient(this.Host, this.Port, this.UseSSL))
                {
                    client.SetCredentials(this.Username, this.Password);

                    await client.SendAsync(message);
                }

                this.MsgList.Add($"cs success");
            }
            catch (Exception ex)
            {
                this.MsgList.Add($"#cs error {ex.Message}");
            }
        }

        private bool CanCsSmtpSendCmd(object param) => !this.IsProcessing;

        #endregion

    }
}