using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Services
{
    public interface IMailReaderService
    {
        [DisplayName("Email Processing")]
        public Task ProcessEmails();
    }
}
