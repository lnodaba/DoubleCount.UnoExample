using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

using Blazored.Modal;
using Blazored.Modal.Services;

using IdentityModel.Client;

using DCx.Webshop.Models;
using DCx.Webshop.Services;
using DCx.Webshop.Shared;
using DCx.Webshop.Config;
using DCx.Webshop.Products;

namespace DCx.Webshop.Pages
{
    public class LicensesBase : ComponentBase
    {
        [Inject]
        private IProductRepo                ProductRepo         { get; set; }
        [Inject]
        private IMailService                MailService         { get; set; }
        [Inject]
        private OicdConfig                  OicdConfig          { get; set; }

        [Inject]
        private IHttpContextAccessor        HttpContextAccessor { get; set; }
        [Inject]
        private NavigationManager           NavigationManager   { get; set; }
        [Inject]
        private IStringLocalizer<Licenses>  Localizer           { get; set; }

        [Inject]

        [CascadingParameter]
        public IModalService                Modal               { get; set; }

        private string _selectedTab = "Product";
        public string SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (value == "Invoice")
                    UseLicenseClick(this.useLicenseFlag);

                _selectedTab = value;
            }
        }

        public License license = new License();
        public Invoice invoice = new Invoice();

        public List<ProductItem> products;
        public ProductItem selectedProduct = new ProductItem();

        public string Period
        {
            get => selectedProduct.Prices.FirstOrDefault(x => x.HasPeriod)?.Period ?? "-";
        }

        public bool useLicenseFlag = true;
        public bool useLoginFlag = true;


        protected async override Task OnInitializedAsync()
        {
            products = ProductRepo.GetProducts();

            UserInfoResponse response = await getUserInfo();

            license = License.FromClaims(response.Claims);
        }

        private async Task<UserInfoResponse> getUserInfo()
        {
            var client = new HttpClient();
            var token = await HttpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var address = $"{OicdConfig.Authority}/connect/userinfo";

            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = address,
                Token = token
            });

            return response;
        }

        public string GetClass(string page)
            => SelectedTab == page ? "tab_active" : string.Empty;

        public void SelectTab(string tabToSelect)
        {
            SelectedTab = tabToSelect;
            this.StateHasChanged();
        }

        protected void UseLicenseClick(object checkedValue)
        {
            if ((bool)checkedValue)
            {
                invoice.Email = license.Email;
                invoice.Company = license.Company;
                invoice.FirstName = license.FirstName;
                invoice.LastName = license.LastName;
                invoice.Address = license.Address;
                invoice.City = license.City;
                invoice.Zip = license.Zip;
                StateHasChanged();
            }
        }

        protected void UseLogineClick(object checkedValue)
        {
            var value = (bool)checkedValue;
        }

        protected void NextLicense(EditContext licenseContext)
        {
            if (!licenseContext.Validate())
                return;

            UseLogineClick(this.useLicenseFlag);
            SelectTab("Invoice");
        }
        protected void NextInvoice(EditContext InvoiceContext)
        {
            if (!InvoiceContext.Validate())
                return;

            SelectTab("Complete");
        }

        public async Task OrderNow()
        {
            var mailBody    = MailTemplateService.GetOrderEmail(this.selectedProduct, this.invoice, this.license, Localizer);
            var msgResult   = await MailService.SendMsgAsync(this.license.Email, "New order placed!", mailBody);

            var parameters = new ModalParameters();
            parameters.Add("Message", $"Order has been placed, a copy has been sent to {this.invoice.Email}");

            await Modal.Show<Modal>("Order Placed", parameters).Result;

            NavigationManager.NavigateTo("/");
        }
        
    }
}
