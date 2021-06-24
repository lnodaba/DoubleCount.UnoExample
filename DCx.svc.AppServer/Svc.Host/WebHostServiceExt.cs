using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;

namespace DCx.AppServer
{
    public static class WebHostServiceExt
    {
        public static void RunAsWinService(this IWebHost host)  
        {  
            //var webHostService = new DxWebHostService(host);  
            //ServiceBase.Run(webHostService);  
        }  
    }
}
