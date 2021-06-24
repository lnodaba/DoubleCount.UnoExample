using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DCx.svc.CfgServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Setup.Resources.Init("DCx.CfgServer.ini");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
