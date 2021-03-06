using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Windows.Security.Credentials;
#if __WASM__
using PasswordVault = Dcx.App.Storage.PseudoPasswordVault;
#endif

namespace Dcx.App.Storage
{
#if __WASM__
	public sealed class PseudoPasswordVault : Windows.Security.Credentials.PasswordVault
	{
		public PseudoPasswordVault()
			: base(new UnsecuredPersister(Uno.Foundation.WebAssemblyRuntime.InvokeJS("(function(){ return window.location.hostname; })();")?.ToLowerInvariant()))
		{
		}
	}
#endif

	public partial class SecureStorage : ISecureStorage
	{
		private const string UadoResourceId = "Uno.AzureDevOps";

        private readonly PasswordVault _passwordVault = new PasswordVault();

		public bool Delete(string key)
		{
			ValidateKey(key);

			var isDeleted = false;

			DeleteValue(key, ref isDeleted);

			return isDeleted;
		}

		public bool DeleteAll()
		{
			var isDeleted = false;

			DeleteAllValues(ref isDeleted);

			return isDeleted;
		}

		public T GetValue<T>(string key)
		{
			ValidateKey(key);

			var retrievedValue = string.Empty;

			GetValue(key, ref retrievedValue);

			if (string.IsNullOrWhiteSpace(retrievedValue))
			{
				return default;
			}

			return JsonConvert.DeserializeObject<T>(retrievedValue);
		}

		public bool HasKey(string key)
		{
			return GetValue<string>(key) != null;
		}

		public bool SetValue<T>(string key, T value)
			where T : class
		{
			ValidateKey(key);

			var serializedValue = JsonConvert.SerializeObject(value);
			var isSet = false;

			SetValue(key, serializedValue, ref isSet);

			return isSet;
		}

		private void ValidateKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentException("Please provide a non empty storage key", nameof(key));
			}
		}

		/// <summary>
		/// Delete a Credential locker from password vault
		/// </summary>
		/// <param name="key">Key of the credential to delete</param>
		/// <param name="isDeleted">Bool to determine if delete were sucessfull</param>
		private void DeleteValue(string key, ref bool isDeleted)
		{
			var credential = _passwordVault.Retrieve(UadoResourceId, key);

			if (credential != null)
			{
				_passwordVault.Remove(credential);

				isDeleted = true;
			}
			else
			{
				isDeleted = false;
			}
		}

		/// <summary>
		/// Delete all Credentials locker from the password vault
		/// </summary>
		/// <param name="isDeleted">Bool to determine if delete were sucessfull</param>
		private void DeleteAllValues(ref bool isDeleted)
		{
			var credentials = _passwordVault.RetrieveAll();

			foreach (var credential in credentials)
			{
				if (credential != null)
				{
					_passwordVault.Remove(credential);
					isDeleted = true;
				}
				else
				{
					isDeleted = false;
				}
			}
		}

		private void GetValue(string key, ref string value)
		{
			try
			{
				var credential = _passwordVault.Retrieve(UadoResourceId, key);

				credential.RetrievePassword();

				value = credential?.Password;
			}
			catch
			{
				value = string.Empty;
			}
		}

		private void SetValue(string key, string value, ref bool isSet)
		{
			try
			{
				_passwordVault.Add(new PasswordCredential()
				{
					Password = value,
					Resource = UadoResourceId,
					UserName = key
				});

				isSet = true;
			}
			catch (Exception e)
			{
				e.ToString();
				isSet = false;
			}
		}
	}
}
