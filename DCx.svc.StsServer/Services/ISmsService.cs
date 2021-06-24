using System;

namespace DCx.StsServer.Services
{
    public interface ISmsService
    {
        bool SendMessage(string smsNumber, string smsMessage);
    }
}
