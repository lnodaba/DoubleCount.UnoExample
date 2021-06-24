﻿using Newtonsoft.Json;

namespace Dcx.App.Data.Auth
{
	public class OAuthData
	{
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		[JsonProperty("expires_in")]
		public string ExpiresIn { get; set; }

		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
	}
}
