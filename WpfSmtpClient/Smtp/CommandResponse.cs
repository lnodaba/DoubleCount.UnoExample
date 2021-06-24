using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCx.Smtp
{
     public class CommandResponse
    {
        public  int     Code        { get; private set; }
        public  string  Message     { get; private set; }

        public CommandResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
