using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using DCx.CsvStore;
using DCx.CsvIdentityStore.Models;
using DCx.CsvIdentityStore.UserServices;

namespace DCx.StsServer.CsvIdentityStore
{
    public static class CustomIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
            var folder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName;

            builder.Services.AddSingleton<ITableMgr>(sp => new TableMgr($"{folder}\\CsvFiles"));
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IProfileRepository, ProfileRepository>();
            builder.Services.AddSingleton<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

            builder.AddProfileService<CustomProfileService>();
            builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

            return builder;
        }
    }
}
