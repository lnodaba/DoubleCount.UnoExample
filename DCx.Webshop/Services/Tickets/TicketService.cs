using DCx.lib.Webshop.Storage.Services;
using DCx.Webshop.Models;
using DynamicExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DCx.Webshop.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly BaseMongoDBService<Ticket> _mongoDBService;
        public TicketService(IDatabaseSettings settings)
        {
            _mongoDBService = new BaseMongoDBService<Ticket>(settings);
        }

        public int GetLastTicketNumber()
        {
            int nr = 0;
            nr = _mongoDBService.Get().Select(x => x.TicketNr).DefaultIfEmpty().Max();
            return nr;
        }

        public List<Ticket> GetTickets(int state, string email)
        {
            List<Ticket> results;
            if (state.Equals((int)State.All))
            {
                results = _mongoDBService.Get(x => x.Email.Equals(email));
            }
            else
            {
                results = _mongoDBService.Get(x => x.State.Equals(state) && x.Email.Equals(email));
            }
            return results;
        }

        public Ticket GetTicketByTicketNr(int ticketNr)
        {
            return _mongoDBService.Get(x => x.TicketNr.Equals(ticketNr)).FirstOrDefault();
        }

        public List<Ticket> GetAllTickets(int state)
        {
            List<Ticket> results;
            if (state.Equals((int)State.All))
            {
                results = _mongoDBService.Get();
            }
            else
            {
                results = _mongoDBService.Get(x => x.State.Equals(state));
            }
            return results;
        }

        public List<Ticket> FilterTickets(int state, List<Filter> filters)
        {
            DynamicFilterBuilder<Ticket> starterpredicate = null;
            if (state.Equals((int)State.All))
            {
                starterpredicate = new DynamicFilterBuilder<Ticket>();
            }
            else
            {
                starterpredicate = new DynamicFilterBuilder<Ticket>().And("State", FilterOperator.Equals, state);
            }
            var predicate = filters.Aggregate(starterpredicate, (summRow, nextRow) =>
              {
                  summRow.And(b => nextRow.Selected.ToList().Aggregate((DynamicFilterBuilder<Ticket>)null, (all, next) =>
                  {
                      if (all == null)
                      {
                          return b.And(nextRow.Column, FilterOperator.Equals, next);
                      }
                      return all.Or(nextRow.Column, FilterOperator.Equals, next);
                  }));
                  return summRow;
              })
            .Build();


            return _mongoDBService.Get(predicate);
        }

        public void SaveTicket(Ticket ticket)
        {
            _mongoDBService.Create(ticket);
        }

        public void UpdateTicket(Ticket ticket)
        {
            _mongoDBService.Update(ticket.Id, ticket);
        }
    }
}
