using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dcx.Uno
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            PrepareClient();
        }

        private OidcClient _oidcClient;
        private AuthorizeState _loginState;
        private Uri _logoutUrl;

        private async void PrepareClient()
        {
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().OriginalString;

            // Create options for endpoint discovery
            var options = new OidcClientOptions
            {
                Authority = "https://localhost:44341",
                ClientId = "WasmClient",
                Scope = "openid profile DCxCfgScope aud offline_access",
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = redirectUri,
            };

            // Create the client. In production application, this is often created and stored
            // directly in the Application class.
            _oidcClient = new OidcClient(options);

            // Invoke Discovery and prepare a request state, containing the nonce.
            // This is done here to ensure the discovery mechanism is done before
            // the user clicks on the SignIn button. Since the opening of a web window
            // should be done during the handling of a user interaction (here it's the button click),
            // it will be too late to reach the discovery endpoint.
            // Not doing this could trigger popup blocker mechanisms in browsers.
            _loginState = await _oidcClient.PrepareLoginAsync();
            btnSignin.IsEnabled = true;

            // Same for logout url.
            _logoutUrl = new Uri(await _oidcClient.PrepareLogoutAsync(new LogoutRequest()));
            btnSignout.IsEnabled = true;
        }

        private async void SignIn_Clicked(object sender, RoutedEventArgs e)
        {
            var startUri = new Uri(_loginState.StartUrl);

            // Important: there should be NO await before calling .AuthenticateAsync() - at least
            // on WebAssembly, in order to prevent triggering the popup blocker mechanisms.
            var userResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri);

            if (userResult.ResponseStatus != WebAuthenticationStatus.Success)
            {
                // Error or user cancellation
                return;
            }

            // User authentication process completed successfully.
            // Now we need to get authorization tokens from the response
            var authenticationResult = await _oidcClient.ProcessResponseAsync(userResult.ResponseData, _loginState);

            if (authenticationResult.IsError)
            {
                var errorMessage = authenticationResult.Error;
                txtAuthResult.Text = errorMessage;
                // TODO: do something with error message
                return;
            }

            // That's completed. Here you have to token, ready to do something
            var token = authenticationResult.AccessToken;
            txtUser.Text = authenticationResult.User.Identity.Name;
            txtAuthResult.Text = token;
            var refreshToken = authenticationResult.RefreshToken;

            // TODO: make something useful with the tokens
        }

        private async void SignOut_Clicked(object sender, RoutedEventArgs e)
        {
            // Important: there should be NO await before calling .AuthenticateAsync() - at least
            // on WebAssembly, in order to prevent triggering the popup blocker mechanisms.
            await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, _logoutUrl);
        }
    }
}
