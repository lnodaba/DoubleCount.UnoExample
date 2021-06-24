using DCx.Webshop.Models;
using System;
using System.Threading.Tasks;

namespace DCx.Webshop.Services
{
    public interface IMailService
    {
        Task<bool> SendMsgAsync(string adrTO, string subject, string body);
    }
}
