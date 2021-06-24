using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using DCx.Webshop.ResourceAnnotation;
using IdentityModel;

namespace DCx.Webshop.Models
{
    public class License
    {
        public static License FromClaims(IEnumerable<Claim> claims)
        {
            if (!claims.Any())
                return new License();

            string getClaimValue(string claimType)
                => claims.FirstOrDefault(x => x.Type == claimType)?.Value ?? string.Empty;

            return new License()
            {
                Email = getClaimValue("email"),
                Address = getClaimValue("address"),
                City = getClaimValue("city"),
                Company = getClaimValue("company"),
                FirstName = getClaimValue("first_name"),
                LastName = getClaimValue("last_name"),
                Zip = getClaimValue("zip"),
                Role = getClaimValue(JwtClaimTypes.Role)
            };
        }

        [Display(Name = "EmailAddress", ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource)), EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Display(Name = "Company", ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string Company { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string LastName { get; set; }

        [Display(Name = "Address", ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string Address { get; set; }

        [Display(Name = "Zip", ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string Zip { get; set; }

        [Display(Name = "City", ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string City { get; set; }

        public string Role { get; set; }
        public override string ToString()
        {
            return $"Email: {Email}\n" +
                $"Company: {Company}\n" +
                $"FirstName: {FirstName}\n" +
                $"LastName: {LastName}\n" +
                $"Address: {Address}\n" +
                $"Zip: {Zip}\n" +
                $"City: {City}\n";
        }


    }
}
