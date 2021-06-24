using DCx.lib.Webshop.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Models
{
    public class PredefinedMessage : MongoEntity
    {
        public string Label { get; set; }
        public string Message { get; set; }
    }
}
