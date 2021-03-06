using Dcx.App.Business.Authentication;
using GalaSoft.MvvmLight.Ioc;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dcx.App.Controls
{
    public partial class UnoWebView : ContentControl
	{
		private static bool _loggedOut;
		private readonly IAuthenticationService _authenticationService;
		private WebView _webView;
		private bool _isFirstNavigation;

		public UnoWebView()
		{
			DefaultStyleKey = nameof(UnoWebView);
			_authenticationService = SimpleIoc.Default.GetInstance<IAuthenticationService>();
			_authenticationService.LoggedOut += OnLoggedOut;

			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}

		public object InternalWebView { get; set; }

		partial void ClearCacheAndCookies();

		partial void SetWebViewControl();

		public Uri SourceUri
		{
			get => (Uri)GetValue(SourceUriProperty);
			set => SetValue(SourceUriProperty, value);
		}

		// Using a DependencyProperty as the backing store for SourceUri.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SourceUriProperty = DependencyProperty.Register(
			"SourceUri",
			typeof(Uri),
			typeof(UnoWebView),
			new PropertyMetadata(default(Uri)));

		public Uri NavigatedUri
		{
			get => (Uri)GetValue(NavigatedUriProperty);
			set => SetValue(NavigatedUriProperty, value);
		}

		// Using a DependencyProperty as the backing store for SourceUrl.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty NavigatedUriProperty = DependencyProperty.Register(
			nameof(NavigatedUri),
			typeof(Uri),
			typeof(UnoWebView),
			new PropertyMetadata(default(Uri)));

		public bool IsClearCacheAndCookies
		{
			get => (bool)GetValue(IsClearCacheAndCookiesProperty);
			set => SetValue(IsClearCacheAndCookiesProperty, value);
		}

		public static readonly DependencyProperty IsClearCacheAndCookiesProperty =
			DependencyProperty.RegisterAttached(nameof(IsClearCacheAndCookies), typeof(bool), typeof(UnoWebView), new PropertyMetadata(false, null));

		private void SetWebView()
		{
			_isFirstNavigation = true;
			NavigatedUri = null;

			if (SourceUri != null)
			{
				SetWebViewControl();

				Content = InternalWebView ?? GetDefaultWebView();
			}
			else if (Content != null)
			{
				ClearDefaultWebView();
				Content = null;
				InternalWebView = null;
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			SetWebView();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			ClearDefaultWebView();
			ClearCacheAndCookies();
		}

		private WebView GetDefaultWebView()
		{
			if (_webView != null)
			{
				return _webView;
			}

			_webView = new WebView { Source = SourceUri };
			_webView.NavigationStarting += OnNavigationStarting;
			_webView.NavigationCompleted += OnNavigationCompleted;

			return _webView;
		}

		private void ClearDefaultWebView()
		{
			if (_webView != null)
			{
				ClearCacheAndCookies();
				_webView.NavigationStarting -= OnNavigationStarting;
				_webView = null;
			}
		}

		private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			if (_isFirstNavigation && IsClearCacheAndCookies)
			{
				_isFirstNavigation = false;

				if (Uri.IsWellFormedUriString(SourceUri?.OriginalString, UriKind.Absolute))
				{
					ClearCacheAndCookies();
				}
			}

			if (_loggedOut)
			{
#if __IOS__
				ClearCacheAndCookies();
#endif
			}

			NavigatedUri = args.Uri;
		}

		private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
		{
			Visibility = Visibility.Visible;
		}

		private void OnLoggedOut(LoggedOutEventArgs args)
		{
			_loggedOut = true;
		}
	}
}
