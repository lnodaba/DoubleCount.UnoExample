using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCx.Smtp
{
    public class MailAttachment
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        
        public MailAttachment(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }
    }
}
