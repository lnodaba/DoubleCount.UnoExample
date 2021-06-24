using Dcx.App.Business.Authentication;
using Dcx.App.Storage;
using GalaSoft.MvvmLight.Ioc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Dcx.App
{
    [SuppressMessage("", "CA1716", Justification = "Stylistic choice")]
	public static class Module
	{
		public static void Initialize(SimpleIoc serviceProvider)
		{
			var adoUnauthorizedCodes = new[] { HttpStatusCode.NonAuthoritativeInformation, HttpStatusCode.Unauthorized };

			// Business wire-up
			serviceProvider.Register<ISecureStorage>(() => new SecureStorage());

			serviceProvider.Register<IAuthenticationService>(() => new AuthenticationService(
				serviceProvider.GetInstance<ISecureStorage>()));

		
		}
	}
}
