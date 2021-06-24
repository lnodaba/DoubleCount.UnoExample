using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServerHost.Quickstart.UI;


using DCx.StsServer.Controllers.Account.Model;
using DCx.CsvIdentityStore.Models;
using DCx.CsvIdentityStore.UserServices;
using DCx.StsServer.Extensions;
using DCx.StsServer.Services;


namespace DCx.StsServer.Controllers.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IUserRepository _usersRepository;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore                   _clientStore;
        private readonly IAuthenticationSchemeProvider  _schemeProvider;
        private readonly IEventService                  _events;
        private readonly ISmsService                    _smsService;
        private readonly IProfileRepository             _profileRepository;

        public RegisterController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserRepository usersRepository,
            IProfileRepository profileRepository,
            ISmsService smsService)
        {
            _usersRepository = usersRepository;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _smsService = smsService;
            _profileRepository = profileRepository;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel model, string button)
        {
            if (button == "cancel")
            {
                return RedirectToAction("Login", "Account", new
                {
                    returnUrl = model.ReturnUrl
                });
            }

            if (ModelState.IsValid)
            {
                var smsCode   = new Random().Next(0, 999999).ToString("D6");
                var smsResult = _smsService.SendMessage(model.Phone, smsCode);

                HttpContext.Session.SetObject("user",   model);
                HttpContext.Session.SetObject("code",   smsCode);
                HttpContext.Session.SetObject("sms",    smsResult);

                return RedirectToAction("Verify");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Verify()
        {
            var smsCode     = HttpContext.Session.GetObject<string>("code");
            var smsResult   = HttpContext.Session.GetObject<bool>  ("sms");
            var regModel    = HttpContext.Session.GetObject<RegisterModel>("user");

            if (smsCode.IsBlank())
            {
                return RedirectToAction("Account", "Login");
            }

            return View(new VerifyModel()
            {
                SmsFailed   = !smsResult,
                Phone       = regModel.Phone,
                UnlockCode  = HttpContext.Session.GetObject<string>("code")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(VerifyModel model)
        {
            var originalCode = HttpContext.Session.GetObject<string>("code");
            var smsResult    = HttpContext.Session.GetObject<bool>  ("sms");

            if (!model.UnlockCode.Equals(originalCode.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return View(new VerifyModel()
                {
                    SmsFailed   = !smsResult,
                    Phone       = model.Phone,
                    ErrMessage  = "Invalid Code try again!"
                });
            }

            var regiserModel = HttpContext.Session.GetObject<RegisterModel>("user");
            var appUser = new AppUser()
            {
                UserName = regiserModel.Email,
                Password = regiserModel.Password,
                Email = regiserModel.Email,
                Role = "member"
            };

            var result = _usersRepository.Create(appUser, regiserModel.ConfirmPassword);
            if (result == IdentityResult.Success)
            {
                _profileRepository.CreateOrUpdate(new UserProfile()
                {
                    Email = regiserModel.Email,
                    Company = regiserModel.Company,
                    FirstName = regiserModel.FirstName,
                    LastName = regiserModel.LastName,
                    Address = regiserModel.Address,
                    Zip = regiserModel.Zip,
                    City = regiserModel.City,
                });

                return RedirectToAction("Success", "Register");
            }

            var errors = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
            return RedirectToAction("Error", "Register", new { error = errors });
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Success(string returnUrl)
        {
            var regiserModel = HttpContext.Session.GetObject<RegisterModel>("user");
            return RedirectToAction("Login", "Account", new
            {
                returnUrl = regiserModel.ReturnUrl
            });
        }



        [HttpGet]
        public IActionResult Error(string error)
        {
            return View((object)error);
        }
    }
}
