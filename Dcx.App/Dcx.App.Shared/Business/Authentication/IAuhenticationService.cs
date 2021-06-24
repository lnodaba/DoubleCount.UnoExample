﻿using Dcx.App.Data.Auth;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dcx.App.Business.Authentication
{
		public delegate void LoggedOutEventHandler(LoggedOutEventArgs args);

		public interface IAuthenticationService
		{
			event LoggedOutEventHandler LoggedOut;

			Task<T> AuthenticatedExecution<T>(Func<CancellationToken, OAuthData, Task<T>> func, CancellationToken ct = default);

			Task AuthenticatedExecution(Func<CancellationToken, OAuthData, Task> func, CancellationToken ct = default);

			bool IsAuthenticated();

			Task Login(string authenticationCode, CancellationToken ct = default);

			void Logout();
		}
}
