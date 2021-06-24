using DCx.Webshop.Config;
using DCx.Webshop.Models;
using DCx.Webshop.Services.Tickets;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DCx.Webshop.Services;

namespace DCx.Webshop.Pages.Tickets
{
    public class TicketsBase : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private ITicketService ticketService { get; set; }
        [Inject]
        private OicdConfig OicdConfig { get; set; }
        [Inject]
        private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject]
        private IMailReaderService mailReaderService { get; set; }

        public Attachment attachment = new Attachment();
        public License license = new License();

        public List<Ticket> tickets = null;
        public List<Filter> filters = new List<Filter>();

        [Parameter]
        public int? CurrentTicket { get; set; }

        protected async override Task OnInitializedAsync()
        {
            UserInfoResponse response = await GetUserInfo();
            license = License.FromClaims(response.Claims);
            LoadTickets();
        }

        public async override Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            CurrentTicket = CurrentTicket ?? 0;
        }
        public void LoadTickets()
        {
            int state = (int)Enum.Parse(typeof(State), SelectedTab);
            if (license.Role == "admin")
            {
                tickets = ticketService.GetAllTickets(state);
            }
            else
            {
                tickets = ticketService.GetTickets(state, license.Email);
            }
            BuildFilters();
        }
        public void SelectTab(string tabToSelect)
        {
            SelectedTab = tabToSelect;
            LoadTickets();

            this.StateHasChanged();
        }

        public void FilterTickets()
        {
            var selectedFilters = filters.Where(filter => filter.Selected.Count() > 0);
            int state = (int)Enum.Parse(typeof(State), SelectedTab);
            tickets = ticketService.FilterTickets(state, selectedFilters.ToList());
        }


        private async Task<UserInfoResponse> GetUserInfo()
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

        private void BuildFilters()
        {
            filters.Clear();
            filters.Add(new Filter()
            {
                Category = "Subject",
                Column = "Subject",
                Values = tickets.GroupBy(ticket => ticket.Subject)
                    .Select(ticketGroup => new FilterItem { Value = ticketGroup.Key })
                    .Where(ticket => !string.IsNullOrEmpty(ticket.Value))
                    .ToList()
            });
            filters.Add(new Filter()
            {
                Category = "Created by",
                Column = "CreatedBy",
                Values = tickets.GroupBy(ticket => ticket.CreatedBy)
                                .Select(ticketGroup => new FilterItem { Value = ticketGroup.Key })
                                .Where(ticket => !string.IsNullOrEmpty(ticket.Value))
                                .ToList()
            });
            filters.Add(new Filter()
            {
                Category = "Creation date",
                Column = "CreatedDateString",
                Values = tickets.GroupBy(ticket => ticket.CreatedDate.Date)
                                  .Select(ticketGroup => new FilterItem { Value = ticketGroup.Key.ToShortDateString() })
                                  .Where(ticket => !string.IsNullOrEmpty(ticket.Value))
                                  .ToList()
            });
            filters.Add(new Filter()
            {
                Category = "Modified date",
                Column = "LastActionDateString",
                Values = tickets.GroupBy(ticket => ticket.LastActionDate.Date)
                                .Select(ticketGroup => new FilterItem { Value = ticketGroup.Key.ToShortDateString() })
                                .Where(ticket => !string.IsNullOrEmpty(ticket.Value))
                                .ToList()
            });
            filters.Add(new Filter()
            {
                Category = "Product",
                Column = "Product.Name",
                Values = tickets.GroupBy(ticket => ticket.Product?.Name)
                                .Select(ticketGroup => new FilterItem { Value = ticketGroup.Key })
                                .Where(ticket => !string.IsNullOrEmpty(ticket.Value))
                                .ToList()
            });
        }

        private string _selectedTab = "Open";
        public string SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (value == "Open")
                    LoadTickets();

                _selectedTab = value;
            }
        }
        public void ShowCreateTicket()
        {
            NavigationManager.NavigateTo("/CreateTicket", true);
        }
        public string GetClass(string page) => SelectedTab == page ? "tab_active" : string.Empty;
    }
}
