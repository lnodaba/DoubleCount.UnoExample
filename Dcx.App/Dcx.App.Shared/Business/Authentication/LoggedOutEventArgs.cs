using System;

namespace Dcx.App.Business.Authentication
{
    public class LoggedOutEventArgs : EventArgs
	{
		public Exception Exception { get; set; }
	}
}
