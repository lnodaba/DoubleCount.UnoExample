using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.StsServer.Controllers.Account.Model
{
    public class VerifyModel
    {
        public  bool    SmsFailed   { get; set; }
        public  string  Phone       { get; set; }
        public  string  ErrMessage  { get; set; }

        public  string  UnlockCode  { get; set; }
    }
}
