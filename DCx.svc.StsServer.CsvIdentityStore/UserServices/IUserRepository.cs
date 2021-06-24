using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;

using DCx.CsvIdentityStore.Models;

namespace DCx.CsvIdentityStore.UserServices
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string email, string password);

        AppUser FindBySubjectId(string subjectId);

        AppUser FindByEmail(string email);
        AppUser FindByExternalProvider(string provider, string providerUserId);
        AppUser AutoProvisionUser(string provider, string providerUserId, List<Claim> lists);

        IdentityResult Create(AppUser appUser, string password);
    }
}
