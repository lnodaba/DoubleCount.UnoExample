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
using DCx.Defines;
using DCx.WpfClient.ApiHelper;
using DCx.Identity.Model;

namespace DCx.WpfClient
{
    public class MainVM : PropNotify
    {

        #region cmds
        public VMCommand    GetTokenCmd	        { get; private set; }
        public VMCommand    GetProfileCmd       { get; private set; }
        public VMCommand    StartWSCmd	        { get; private set; }
        public VMCommand    StopWSCmd	        { get; private set; }
        public VMCommand    SendMsgCmd	        { get; private set; }
        public VMCommand    BroadcastMsgCmd     { get; private set; }
        public VMCommand    WebMsgReceiveCmd    { get; private set; }

        #endregion

        #region vars

        private string                          m_accessToken   = null;
        private string                          m_profileInfo   = null;

    //  private WsConnection                    m_wsClient      = new WsConnection(ESessionType.Client);

        private object                          m_msgLock       = new object();
        private object                          m_urlLock       = new object();

        public  ObservableCollection<string>    MsgList { get; }
        public  ObservableCollection<string>    UrlList { get; }
        #endregion

        #region ctors

        public MainVM()
        {
            this.GetTokenCmd        = new VMCommand(this.RunGetTokenCmd);
            this.GetProfileCmd      = new VMCommand(this.RunGetProfileCmd,      this.CanGetProfileCmd);
			this.StartWSCmd         = new VMCommand(this.RunStartWSCmd,         this.CanStartWSCmd);
            this.StopWSCmd          = new VMCommand(this.RunStopWSCmd,          this.CanStopWSCmd);
            this.SendMsgCmd         = new VMCommand(this.RunSendMsgCmd,         this.CanSendMsgCmd);
            this.BroadcastMsgCmd    = new VMCommand(this.RunBroadcastMsgCmd,    this.CanBroadcastMsgCmd);
            this.WebMsgReceiveCmd   = new VMCommand(this.RunWebMsgReceiveCmd);

            this.MsgList            = new ObservableCollection<string>();
            this.UrlList            = new ObservableCollection<string>();

            BindingOperations.EnableCollectionSynchronization(this.MsgList, this.m_msgLock);
            BindingOperations.EnableCollectionSynchronization(this.UrlList, this.m_urlLock);

        //  this.m_wsClient.OnMessage = this.OnWSMessage;
        }
        #endregion


        #region inpc - MsgIsVisible

        private bool m_msgIsVisible = default;
        public bool MsgIsVisible
        {
            get => this.m_msgIsVisible;
            set => this.SetPropChanged(ref this.m_msgIsVisible, value);
        }
        #endregion

        #region inpc - MsgText

        private string m_msgText = "this is a WPF note!";
        public string MsgText
        {
            get => this.m_msgText;
            set => this.SetPropChanged(ref this.m_msgText, value);
        }
        #endregion

        #region inpc - StsIsVisible

        private bool m_stsIsVisible = default;
        public bool StsIsVisible
        {
            get => this.m_stsIsVisible;
            set => this.SetPropChanged(ref this.m_stsIsVisible, value);
        }
        #endregion

        #region inpc - StsSource

        private Uri m_stsSource = default;
        public Uri StsSource
        {
            get => this.m_stsSource;
            set => this.SetPropChanged(ref this.m_stsSource, value, () => OnUrlChanged(value));
        }
        #endregion

        #region handler - OnUrlChanged

        private async void OnUrlChanged(Uri urlSource)
        {
            this.UrlList.Add($">> {urlSource.AbsoluteUri}");

            // initiate get token
            if (urlSource.AbsoluteUri.Contains("authentication/login-callback"))
            {
                this.StsIsVisible = false;
                var queryParams = HttpUtility.ParseQueryString(urlSource.Query);
                var codeVerifier = CryptoRandom.CreateUniqueId(32);

                if (!string.IsNullOrWhiteSpace(queryParams["code"]))
                {
                    try
                    {
                        var accessToken = await TokenHelper.GetAccessTokenWithAuthCode(queryParams["code"]);
                        var accessTokenAgain = await TokenHelper.TryGetAccessTokenFromCache();
                        MessageBox.Show("Login Succeeded!");
                        var apiResult = await TestApiClient.GetData(accessToken);

                        this.UrlList.Add($"Api result:");
                        apiResult.ForEach(x =>
                        {
                            this.UrlList.Add(x);
                        });
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error in login! Try Again!");
                    }
                }
            }

            if (urlSource.AbsoluteUri.Contains("error=access_denied"))
            {
                this.StsIsVisible = false;
                MessageBox.Show("Error on login!");
            }
        }
        #endregion


        #region cmd - GetTokenCmd

        private void RunGetTokenCmd(object param)
        {
            var   stsClient     = new HttpClient();
            var   msgRequest    = TokenHelper.GetMsgRequest(OidcConstants.GrantTypes.ClientCredentials,
                                                            AppConstants.cURL_STS_GETTOKEN,
                                                            AppConstants.cSTS_CREDENTIALS);
            // call api in case we have token?
            //await stsClient.CallMsgAsync(msgRequest,
            //                             (result) => this.m_accessToken = TokenHelper.GetAccessToken(result),
            //                             (error)  => this.m_accessToken = null);

            if (this.m_accessToken.IsBlank())
            {
                //  login
                this.StsSource      = new Uri(AppConstants.cURL_STS_LOGIN);
                this.StsIsVisible   = true;

            }

            this.MsgList.Add($"token: {this.m_accessToken}");
        }

        private bool canGetTokenCmd(object param) => this.m_accessToken.IsUsed();

        #endregion

        #region cmd - GetProfileCmd

        private async void RunGetProfileCmd(object param)
        {
            var     txtKeys     = $"{"userId"}|{Guid.Empty.ToString("N")}";

            var     cfgClient   = new HttpClient();
                    cfgClient.SetBearerHdr(this.m_accessToken);

            await   cfgClient.CallApiAsync(AppConstants.cAPI_PROFILE_GETINFO, 
                                           txtKeys.ToUTF8(),
                                           (result) => this.m_profileInfo= result.FromUTF8(),
                                           (error)  => this.m_profileInfo = null);

            this.MsgList.Add($"token: {this.m_profileInfo}");
        }

        private bool CanGetProfileCmd(object param) => this.m_accessToken.IsUsed();

        #endregion


        #region cmd - StartWSCmd

        private async void RunStartWSCmd(object param)
        {
            //var cfgConnection   = new CfgConnection()
            //                        {

            //                        };

            //var cfgUserInfo     = new CfgUserInfo()
            //                        {
            //                            AccessToken = this.m_accessToken
            //                        };

             await Task.CompletedTask; //this.m_wsClient.ConnectAsync(cfgConnection, cfgUserInfo);
        }

        private bool CanStartWSCmd(object param)        =>  false; //this.m_accessToken.IsUsed() && !this.m_wsClient.IsConnected;

        #endregion

        #region cmd - StopWSCmd

        private async void RunStopWSCmd(object param)   =>  await Task.CompletedTask;   //this.m_wsClient.DisconnectAsync();

        private bool CanStopWSCmd(object param)         =>  false; //this.m_wsClient.IsConnected;

        #endregion

        #region cmd - SendMsgCmd

        private void RunSendMsgCmd(object param)
        {
            var rawMsg = $"{this.MsgText}".ToUTF8();

            this.MsgList.Add($">> {this.MsgText}");
          //this.m_wsClient.SendMessage(rawMsg);
        }

        private bool CanSendMsgCmd(object param)        =>  false;  //this.m_wsClient.IsConnected && this.MsgText.Length > 1;

        #endregion

        #region cmd - BroadcastMsgCmd

        private void RunBroadcastMsgCmd(object param)
        {
            var rawMsg = $"@{this.MsgText}".ToUTF8();

            this.MsgList.Add($">> {this.MsgText}");
        //  this.m_wsClient.SendMessage(rawMsg);
        }

        private bool CanBroadcastMsgCmd(object param)   =>  false;  //this.m_wsClient.IsConnected && this.MsgText.Length > 1;

        #endregion

        #region cmd - WebMsgReceiveCmd

        private void RunWebMsgReceiveCmd(object param)
        {
            var msgArgs = param.ToString();
        }
        #endregion


        #region //handler - OnWSMessage

        //private void OnWSMessage(byte[] raw)
        //{
        //    this.MsgList.Add($"<< {Encoding.UTF8.GetString(raw)}");
        //}
        #endregion
    }
}