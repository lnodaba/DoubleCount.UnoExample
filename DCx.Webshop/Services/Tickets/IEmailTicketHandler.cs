using DCx.Webshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Services.Tickets
{
    public interface IEmailTicketHandler
    {
        public Task CreateTicketFromEmail(EmailMessage message);
        public void AddCommentToTicketFromEmail(EmailMessage message);
    }
}
