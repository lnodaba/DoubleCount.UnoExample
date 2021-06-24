using DCx.Webshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Services.Tickets
{
    public interface ITicketService
    {
        public List<Ticket> GetTickets(int state, string email);
        public Ticket GetTicketByTicketNr(int ticketNr);
        public List<Ticket> GetAllTickets(int state);
        public void SaveTicket(Ticket ticket);
        public int GetLastTicketNumber();
        public void UpdateTicket(Ticket ticket);
        public List<Ticket> FilterTickets(int state, List<Filter> filters);
    }
}
