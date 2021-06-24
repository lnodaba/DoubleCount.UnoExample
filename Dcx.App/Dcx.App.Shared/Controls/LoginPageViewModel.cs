using Dcx.App.Business.Authentication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Windows.Security.Authentication.Web;

namespace Dcx.App.Controls
{
    [Windows.UI.Xaml.Data.Bindable]
	public class LoginPageViewModel : ViewModelBase
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly string _azureADLoginUrl;

		private Uri _sourceUri;
		private Uri _navigatedUri;
		private bool _isAuthenticating;

		public LoginPageViewModel()
		{
			_authenticationService = SimpleIoc.Default.GetInstance<IAuthenticationService>();

			ReloadPage = new RelayCommand(() => ReloadPageCommand());
			IsAuthenticating = false;

			var baseURL = "https://localhost:44341";
			var client = "WasmClient";
			var scopes = "openid profile DCxCfgScope aud offline_access";
			var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().OriginalString;

			_azureADLoginUrl =
				$"{baseURL}?client_id={client}" +
				$"&response_type={"code"}&scope={scopes}&redirect_uri={redirectUri}";

			SourceUri = new Uri(_azureADLoginUrl);
		}

		public ICommand ReloadPage { get; }

		public Uri SourceUri
		{
			get => _sourceUri;
			set => Set(nameof(SourceUri), ref _sourceUri, value);
		}

		public bool IsAuthenticating
		{
			get => _isAuthenticating;
			private set => Set(nameof(IsAuthenticating), ref _isAuthenticating, value);
		}

		public Uri NavigatedUri
		{
			get => _navigatedUri;
			set
			{
				Set(nameof(NavigatedUri), ref _navigatedUri, value);

				if (_navigatedUri?.OriginalString?.Contains("code=") ?? false)
				{
					OnAuthenticatedUri(_navigatedUri);
				}

				if (_navigatedUri?.OriginalString?.Contains("error=access_denied") ?? false)
				{
					SourceUri = new Uri(_azureADLoginUrl);
				}
			}
		}

		private void ReloadPageCommand()
		{
			SourceUri = new Uri(_azureADLoginUrl);
			IsAuthenticating = false;
		}

		private void OnAuthenticatedUri(Uri uri)
		{
			var parsed = HttpUtility.ParseQueryString(uri.Query);
			var authenticationCode = parsed["code"];

			IsAuthenticating = true;
				_authenticationService.Login(authenticationCode)
				.ContinueWith(
					task =>
					{
						var result = true;
						if (task.Exception != null)
						{
							task.Exception.Handle(ex =>
							{
								SourceUri = null;
								NavigatedUri = null;

								// We don't flag exception handled so that the TaskNotifier can complete to a Faulted state
								return false;
							});

							SourceUri = new Uri(_azureADLoginUrl);

							result = false;
						}
						

						return result;
					},
					CancellationToken.None,
					TaskContinuationOptions.ExecuteSynchronously,
					TaskScheduler.Default);
		}
	}
}
