using DCx.CfgLicClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DCx.win.CfgLicClient.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client  = new LicenseClient("apit2.doublecount.cloud", "CfgLicClient", "CfgLicClient");

            var unitNo  = "1001";
            var regNo   = "CHE";
            var licYear = 2021;
            var licText = client.GetUnitLicAsync(unitNo, regNo, licYear).Result.FirstOrDefault();

            Console.WriteLine(licText);
            Console.ReadLine();
        }
    }
}
