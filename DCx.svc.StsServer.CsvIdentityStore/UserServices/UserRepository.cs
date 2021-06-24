using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;

using DCx.CsvStore;
using DCx.CsvIdentityStore.Models;


namespace DCx.CsvIdentityStore.UserServices
{
    public class UserRepository : IUserRepository
    {
        private ICsvTable<AppUser> _table;
        private IPasswordHasher<AppUser> _passwordHasher;

        public UserRepository(ITableMgr tableManager, IPasswordHasher<AppUser> passwordHasher)
        {
            tableManager.LoadTable<AppUser>();
            _table = tableManager.GetTable<AppUser>();
            _passwordHasher = passwordHasher;
        }

        private List<AppUser> _users
        {
            get => _table.Records.ToList();
        }

        public bool ValidateCredentials(string email, string password)
        {
            var user = FindByEmail(email);
            if (user != null)
            {
                return user.Password.Equals(password.AsHashBase64());
            }
            return false;
        }

        public AppUser FindBySubjectId(string subjectId)
        {
            return _users.FirstOrDefault(x => x.SubjectId == subjectId);
        }

        public AppUser FindByEmail(string email)
        {
            return _users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public IdentityResult Create(AppUser appUser, string password)
        {

            if (FindByEmail(appUser.Email) != null)
            {
                return IdentityResult.Failed(new IdentityError[]
                {
                    new IdentityErrorDescriber().DuplicateUserName(appUser.UserName)
                });
            }

            if (!appUser.Password.Equals(password))
            {
                return IdentityResult.Failed(new IdentityError[]
                {
                    new IdentityErrorDescriber().PasswordMismatch()
                });
            }

            try
            {
                appUser.SubjectId = Guid.NewGuid().ToString();
                appUser.Password = password.AsHashBase64();
                _table.AddItem(appUser, true);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError[]
                {
                    new IdentityError()
                    {
                        Code = "-1",
                        Description = ex.Message
                    }
                });
            }
            return IdentityResult.Success;
        }

        public AppUser FindByExternalProvider(string provider, string providerUserId) 
            => _users.FirstOrDefault(x => x.ProviderName == provider && x.ProviderSubjectId == providerUserId);

        public AppUser AutoProvisionUser(string provider, string providerUserId, List<Claim> lists)
        {
            var appUser = new AppUser()
            {
                SubjectId = Guid.NewGuid().ToString(),
                UserName = lists.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                Email = lists.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                ProviderName = provider,
                ProviderSubjectId = providerUserId
            };

            _table.AddItem(appUser,true);

            return appUser;
        }


    }
}
