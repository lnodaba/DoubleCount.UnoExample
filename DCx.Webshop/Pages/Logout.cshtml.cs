using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace DCx.Webshop.Pages
{
    public class LogoutModel : PageModel
    {
        private string _logoutDomain;

        public LogoutModel()
        {
            _logoutDomain = Setup.Resources.GetIniValue("oicd", "Authority");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync();

            var returnUrl = HttpUtility.UrlEncode($"https://{HttpContext.Request.Host}/");

            return Redirect($"{_logoutDomain}/Account/Logout?returnUrl={returnUrl}");
        }
    }
}