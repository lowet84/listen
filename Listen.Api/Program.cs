using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Listen.Api
{
    public class Program
    {
        public const string DatabaseName = "Listen";

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://*:7000")
                .Build();

            host.Run();
        }
    }
}
