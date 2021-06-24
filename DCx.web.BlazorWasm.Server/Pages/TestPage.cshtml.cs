using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DCx.web.BlazorWasm.Server.Pages
{
    [Authorize]
    public class TestPageModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}