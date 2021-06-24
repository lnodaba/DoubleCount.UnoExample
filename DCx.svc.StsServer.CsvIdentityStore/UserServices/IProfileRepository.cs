using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Identity;

using DCx.CsvIdentityStore.Models;

namespace DCx.CsvIdentityStore.UserServices
{
    public interface IProfileRepository
    {
        UserProfile FindByEmail(string email);
        public bool HasProfile(string email);
        IdentityResult CreateOrUpdate(UserProfile profile);
    }
}
