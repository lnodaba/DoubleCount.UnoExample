using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Windows.Threading;
using System.Windows.Input;
using System.Collections;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;

namespace DCx.WpfClient
{
    public class WebViewBehavior : DependencyObject
    {
        private static readonly Type ThisType = typeof(WebViewBehavior);

        #region DP - MsgReceiveCmd

        public static readonly  DependencyProperty MsgReceiveCmdProperty =
                                DependencyProperty.RegisterAttached("MsgReceiveCmd",
                                typeof(ICommand), ThisType, new PropertyMetadata(null, OnMsgReceiveCmdChanged));

        public static ICommand GetMsgReceiveCmd(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(MsgReceiveCmdProperty);
        }

        public static void SetMsgReceiveCmd(DependencyObject obj, ICommand value)
        {
            obj.SetValue(MsgReceiveCmdProperty, value);
        }

        public static async void OnMsgReceiveCmdChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is WebView2 webView)
            {
                var oldCommand = e.OldValue as ICommand;
                var newCommand = e.NewValue as ICommand;

                if (oldCommand != null)
                {
                    webView.WebMessageReceived                  -= WebViewBehavior.OnWebMessageReceived;
                    webView.NavigationStarting                  -= WebViewBehavior.OnNavigationStarted;
                    webView.NavigationCompleted                 -= WebViewBehavior.OnNavigationCompleted;
                    webView.CoreWebView2.WebResourceRequested   -= WebViewBehavior.OnWebResourceRequested;
                }
                if (newCommand != null)
                {
                    await webView.EnsureCoreWebView2Async(null);

                    webView.WebMessageReceived                  += WebViewBehavior.OnWebMessageReceived;
                    webView.NavigationStarting                  += WebViewBehavior.OnNavigationStarted;
                    webView.NavigationCompleted                 += WebViewBehavior.OnNavigationCompleted;
                    webView.CoreWebView2.WebResourceRequested   -= WebViewBehavior.OnWebResourceRequested;

                    //webView.CoreWebView2.WebResourceReceived    
                    //  https://docs.microsoft.com/en-us/microsoft-edge/webview2/reference/win32/0-9-538/icorewebview2experimental#add_webresourceresponsereceived
                    //  Added experimental WebResourceResponseReceived event that fires after the WebView has received and processed the response for a WebResource request. Authentication headers, if any, are included in the response object.

                }

            }
        }
        #endregion

        #region handler - OnWebMessageReceived

        private static void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs eArgs)
        {
            if (sender is WebView2 webView)
            {
                var msgCommand = WebViewBehavior.GetMsgReceiveCmd(webView);
                var msgParam   = eArgs.TryGetWebMessageAsString();

                if (msgCommand != null)
                {
                    if (msgCommand.CanExecute(msgParam))
                    {
                        msgCommand.Execute(msgParam);
                    }
                }
            }
        }
        #endregion

        #region handler - OnNavigationStarted

        //  https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/blob/main/WpfWebView/WpfWebView/WpfEmbeddedBrowser.cs
        //  https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/blob/main/NetCoreConsoleClient/src/NetCoreConsoleClient/SystemBrowser.cs
        //  https://github.com/auth0/auth0-oidc-client-net/tree/master/src/Auth0.OidcClient.WPF

        private static void OnNavigationStarted(object sender, CoreWebView2NavigationStartingEventArgs eArgs)
        {
            if (sender is WebView2 webView)
            {
                if (!eArgs.Uri.StartsWith("https://"))   //  TODO allow only the auth site
                {
                    webView.CoreWebView2.ExecuteScriptAsync($"alert('{eArgs.Uri} is not safe, try an https link')");
                    eArgs.Cancel = true;
                }
            }
        }
        #endregion

        #region handler - OnNavigationCompleted

        private static void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs eArgs)
        {
            if (sender is WebView2 webView)
            {
            }
        }
        #endregion

        #region handler - OnSourceChanged

        private static void OnSourceChanged(object sender, CoreWebView2SourceChangedEventArgs eArgs)
        {
            if (sender is WebView2 webView)
            {
            }
        }
        #endregion

        #region handler - OnWebResourceRequested

        private static void OnWebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs eArgs)
        {
            if (sender is WebView2 webView)
            {
                var hdrList = eArgs.Response.Headers;
            }
        }
        #endregion

    }
}
