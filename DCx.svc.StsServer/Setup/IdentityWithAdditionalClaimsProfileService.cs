using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

using DCx.CsvIdentityStore.Models;
using DCx.CsvIdentityStore.UserServices;


namespace DCx.StsServer.Setup
{

    public class IdentityWithAdditionalClaimsProfileService : IProfileService
    {
        private readonly IUserRepository _userRepo;
        private readonly IProfileRepository _profileRepo;
        private readonly ClaimsIdentityOptions _options;

        public IdentityWithAdditionalClaimsProfileService(IUserRepository userRepo, IProfileRepository profileRepo)
        {
            _userRepo = userRepo;
            _profileRepo = profileRepo;
            _options = new ClaimsIdentityOptions();
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _userRepo.FindBySubjectId(sub);
            var principal = CreateClaimsPrincipalAsync(user);

            var claims = principal.Claims.ToList();

            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));
            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));

            if (_profileRepo.HasProfile(user.Email))
            {
                var profile = _profileRepo.FindByEmail(user.Email);

                claims.Add(new Claim("company", profile.Company));
                claims.Add(new Claim("first_name", profile.FirstName));
                claims.Add(new Claim("last_name", profile.LastName));
                claims.Add(new Claim("address", profile.Address));
                claims.Add(new Claim("zip", profile.Zip));
                claims.Add(new Claim("city", profile.City));
            }

            context.IssuedClaims = claims;

            await Task.CompletedTask;
        }

        private ClaimsIdentity CreateClaimsPrincipalAsync(AppUser user)
        {
            var result = new ClaimsIdentity("Identity.Application", _options.UserNameClaimType, _options.RoleClaimType);

            result.AddClaim(new Claim(_options.UserIdClaimType, user.SubjectId));
            result.AddClaim(new Claim(_options.UserNameClaimType, user.UserName));
            result.AddClaim(new Claim(JwtClaimTypes.Role, user.Role));

            return result;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _userRepo.FindBySubjectId(sub);
            context.IsActive = user != null;

            await Task.CompletedTask;
        }
    }
}
