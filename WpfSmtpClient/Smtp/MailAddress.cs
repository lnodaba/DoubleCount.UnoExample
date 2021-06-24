using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCx.Smtp
{
    public class MailAddress
    {
        public string Address   { get; set; }
        public string Name      { get; set; }

        public MailAddress(string address)
        {
            Address = address;
        }

        public MailAddress(string address, string name)
        {
            Address = address;
            Name = name;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                return $"{Name} <{Address}>";
            }
            else
            {
                return Address;
            }
        }
    }
}
