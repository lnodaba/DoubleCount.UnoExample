using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Microsoft.AspNetCore.Identity;

using DCx.CsvStore;
using DCx.CsvIdentityStore.Models;

namespace DCx.CsvIdentityStore.UserServices
{
    public class ProfileRepository : IProfileRepository
    {
        private ICsvTable<UserProfile> _table;
        private List<UserProfile> _profiles
        {
            get => _table.Records.ToList();
        }

        public ProfileRepository(ITableMgr tableManager)
        {
            tableManager.LoadTable<UserProfile>();
            _table = tableManager.GetTable<UserProfile>();
        }

        public IdentityResult CreateOrUpdate(UserProfile profile)
        {
            try
            {
                if (HasProfile(profile.Email))
                    _table.UpdItem(profile);
                else
                    _table.AddItem(profile, true);
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

        public UserProfile FindByEmail(string email)
            => _profiles.FirstOrDefault(x => x.Email.Equals(email));

        public bool HasProfile(string email) => FindByEmail(email) != null;


    }
}
